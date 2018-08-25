'use strict';

require('../server/globals')(global);
require('colors');

const config = require('../server/config/database');
// const validator = require('validator');
const Sequelize = require('sequelize');
const moment = require('moment');
const prompt = require('prompt');

console.log("\n  Migration check script for SAVINA".cyan.bold);
console.log("  For version 2.x");

prompt.message = "  ".reset;
prompt.delimiter = "";

prompt.start();

prompt.get([{
  name: 'source',
  description: 'Enter the source (old) database name to check:',
  default: 'savina_old',
  required: true
}], (err, dbnames) => {
  if (err) {
    console.log("\n\n  Aborted\n".bold.yellow);
    process.exit(0);
  }

  console.log("\n  Using [%s] database at [%s].", dbnames.source.bold, config.host.bold);

  var db = new Sequelize(dbnames.source, config.username, config.password, {
    dialectOptions: config.dialectOptions,
    dialect: config.dialect,
    host: config.host,
    port: config.port,
    logging: false
  });

  return db.query("SELECT TOP 1 [id] FROM [dbo].[static_roles] WHERE [slug] = ?;", {
    replacements: ['admin'],
    type: db.QueryTypes.SELECT
  }).then((role) => {
    if (!role.length) {
      console.error("\n  There's no [admin] role in the database!\n".red.bold);
      process.exit(1);
    }

    return db.query("SELECT * FROM [dbo].[users] WHERE [role] = ?;", {
      replacements: [role[0].id],
      type: db.QueryTypes.SELECT
    }).then((users) => {
      if (users.length) {
        var fixed = 0;
        var curr = 0;

        var check = function () {
          var user = users[curr];

          if (user) {
            console.log("\n  Name:    %s".bold, String(user.name).reset);
            console.log("  Email:   %s".bold, String(user.email).reset);
            console.log("  Created: %s".bold, String(user.createdAt).reset);
            console.log("  Updated: %s\n".bold, String(user.updatedAt).reset);

            if (!user.createdAt || !user.updatedAt) {
              prompt.get([{
                name: 'fix',
                description: 'This administrator account is missing dates. Fill it with today\'s date? (Y/n)',
                default: 'Y',
                required: true
              }], (err, answer) => {
                if (err) {
                  console.log("\n\n  Aborted\n".bold.yellow);
                  process.exit(0);
                }

                if (/Y|Yes/i.test(answer.fix)) {
                  db.query("UPDATE [dbo].[users] SET createdAt=:date, updatedAt = :date WHERE id = :user;", {
                    replacements: {
                      date: moment().format('YYYY-MM-DD HH:mm:ss'),
                      user: user.id
                    },
                    type: db.QueryTypes.UPDATE
                  }).then(() => {
                    fixed++;
                    complete();
                  }).catch(err => {
                    console.log("\n  Couldn't update user: ".red.bold + err.message.red + "\n");
                    process.exit(1);
                  });
                } else {
                  complete();
                }
              });
            } else {
              console.log("  Administrator account is healthy.".green.bold);
              complete();
            }
          }
        };

        var complete = function () {
          if (++curr === users.length) {
            complete();

            if (fixed) {
              console.log("\n  All administrator accounts fixed.\n".green.bold);
              process.exit(0);
            }

            console.log("\n  Nothing was modified.\n".yellow.bold);
            process.exit(0);
          } else {
            check();
          }
        };

        check();
      } else {
        console.log("\n  There are no administrator in the database. Please run `app setup admin`.");
      }
    }).catch(err => {
      console.log("\n  Couldn't fetch users: ".red.bold + err.message.red + "\n");
      process.exit(1);
    });
  }).catch(err => {
    console.error("\n  Couldn't fetch roles: ".red.bold + err.message.red + "\n");
    process.exit(1);
  });

});
