'use strict';

require('../server/globals')(global);
require('colors');

const config = require('../server/config/database');
const validator = require('validator');
const Sequelize = require('sequelize');
const prompt = require('prompt');

console.log("\n  Administrator creation script for SAVINA".cyan.bold);
console.log("  For version 2.x");

prompt.message = "  ".reset;
prompt.delimiter = "";

prompt.start();

console.log("\n  IMPORTANT: ".bold + "This script will create an administrator account. Please keep the information in a safe place.\n");

prompt.get([{
  name: 'answer',
  pattern: /^yes|no$/i,
  message: "  Please type " + "yes".bold + " or " + "no".bold + "...",
  description: "Continue? (yes/no)",
  required: true
}], (err, result) => {
  if (err || !result.answer.match(/yes/i)) {
    console.log("\n  Aborted\n".bold.yellow);
    process.exit(0);
  }

  console.log("\n  Using [%s] database at [%s].", config.database.bold, config.host.bold);

  var db = new Sequelize(config.database, config.username, config.password, {
    dialectOptions: config.dialectOptions,
    dialect: config.dialect,
    host: config.host,
    port: config.port,
    logging: false
  });

  /* Register needed models */
  db.import('../server/models/user');
  db.import('../server/models/role');

  /* Associate models */
  db.model('user').belongsTo(db.model('role'));
  db.model('role').hasMany(db.model('user'));

  db.sync().then(() => {
    console.log("\n  Synchronization complete!\n".green.bold);

    return db.model('role').findOne({
      slug: 'admin'
    }).then((role) => {
      if (!role) {
        console.error("\n  There's no [admin] role in the database!\n".red.bold);
        process.exit(1);
      }

      prompt.message = '';
      prompt.delimiter = '';

      prompt.start();

      return prompt.get([{
        name: 'name',
        required: true,
        conform: value => value.match(/^[a-z\u00C0-\u02AB'´`]+\.?\s([a-z\u00C0-\u02AB'´`]+\.?\s?)+$/i),
        description: "  Enter administrator\'s name:",
        message: "  Name must contain only letters or spaces and match a valid person's name! E.g.: 'Jon Li'."
      }, {
        name: 'email',
        required: true,
        conform: validator.isEmail,
        description: "  Enter administrator\'s email address:",
        message: "  Email must be a valid email address!"
      }, {
        name: 'password',
        required: true,
        conform: value => String(value).length > 3,
        description: "  Enter administrator\'s password:",
        message: "  Password must be at least 4 chars long!"
      }], (err, administrator) => {
        if (err) {
          console.log("\n\n  Aborted\n".bold.yellow);
          process.exit(0);
        }

        return db.model('user').findAll({
          roleId: role.id
        }).then((users) => {
          var exists = false;

          if (users.length) {
            if (users.length > 1) {
              console.log("\n  There are %d administrators already in the database:\n".bold, users.length);
            } else {
              console.log("\n  An administrator is already in the database:\n".bold);
            }

            users.forEach((user) => {
              var duplicated = user.email === administrator.email;

              if (duplicated) {
                console.log("  Name:  %s".bold.red, "".reset + String(user.name).red);
                console.log("  Email: %s".bold.red, "".reset + String(user.email).red);
              } else {
                console.log("  Name:  %s".bold, String(user.name).reset);
                console.log("  Email: %s".bold, String(user.email).reset);
              }

              if (duplicated && !exists) {
                exists = true;
              }

              console.log("");
            });
          }

          if (exists) {
            console.error("  An administrator with the same email already exists!\n".bold.red);
            process.exit(1);
          }

          console.log("\n  New administrator account review:\n".bold);

          console.log("  Name:      %s".bold, String(administrator.name).reset);
          console.log("  Email:     %s".bold, String(administrator.email).reset);
          console.log("  Password:  %s".bold, String(administrator.password).reset);
          console.log("  Role:      %s\n".bold, String(role.name).reset);

          return prompt.get({
            name: 'answer',
            description: "  Create the administrator? (yes/no):",
            message: "  Please type " + "yes".bold + " or " + "no".bold + "...",
            pattern: /^yes|no$/i,
            required: true
          }, (err, result) => {
            if (err || !result.answer.match(/^yes$/i)) {
              console.log("\n\n  The administrator was not created!\n".bold.yellow);
              process.exit(0);
            }

            return db.model('user').create({
              name: administrator.name,
              email: administrator.email,
              password: administrator.password,
              roleId: role.id
            }).then((user) => {
              if (user) {
                console.log("\n  The administrator was created successfully!\n".bold.green);
                process.exit(0);
              }

              console.log("\n  There was an unidetified problem when creating the administrator\n".bold.red);
              process.exit(1);
            }).catch(err => {
              console.log("\n  Couldn't create the administartor:\n".bold.red + err.message.red + "\n");
              process.exit(1);
            });
          });
        }).catch(err => {
          console.log("\n  Couldn't fetch current users: ".red.bold + err.message.red + "\n");
          process.exit(0);
        });
      });
    }).catch(err => {
      console.error("\n  Couldn't fetch roles: ".red.bold + err.message.red + "\n");
      process.exit(1);
    });
  }).catch(err => {
    console.error("\n  Synchronization failed: ".red.bold + err.message.red + "\n");
    process.exit(1);
  });
});
