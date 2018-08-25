'use strict';

require('../server/globals')(global);
require('colors');

const config = require('../server/config/database');
const Sequelize = require('sequelize');
const prompt = require('prompt');
const Umzug = require('umzug');
const path = require('path');

function done(err) {
  if (err) {
    console.error(`\n  ${ String(err).red.bold }\n`);
    console.log(JSON.stringify(err));
    return process.exit(1);
  }

  console.log('\n  Migración exitosa!\n'.green.bold);
  process.exit();

}

console.log("\n  Script de migración para código Id Rayen".cyan.bold);
console.log(" version 3.x");

console.log("\n  Usando la base de datos [%s] en [%s].\n", config.database.bold, config.host.bold);

prompt.message = '  '.reset;
prompt.delimiter = ''.reset;

prompt.start();

prompt.get([{
  name: 'answer',
  pattern: /^s|n$/i,
  message: "  Porfavor ingrese " + "s".bold + " o " + "n".bold + "...",
  description: "Continuar? (s/n)",
  required: true
}], (err, result) => {
  if (err || !result.answer.match(/s/i)) {
    console.log("\n  Abortado\n".bold.yellow);
    process.exit(0);
  }

  console.log("\n  Para agregar la columa ingresa 'a' para eliminar 'e'");

  prompt.get([{
    name: 'answer',
    pattern: /^a|e$/i,
    message: "  Porfavor ingrese " + "a".bold + " o " + "e".bold + "...",

    description: "Agregar/Eliminar? (a/e)",
    required: true
  }], (err, result) => {
    if (err || !result.answer.match(/a|e/i)) {
      console.log("\n  Abortado\n".bold.yellow);
      process.exit(0);
    }

    const db = new Sequelize(config.database, config.username, config.password, {
      dialectOptions: config.dialectOptions,
      dialect: config.dialect,
      host: config.host,
      port: config.port,
      logging: false
    });
    const umzug = new Umzug({
      storage: 'sequelize',
      storageOptions: {
        sequelize: db
      },

      migrations: {
        path: path.join(__dirname, 'migrations'),
        params: [db.getQueryInterface(), db.constructor, function () {
          throw new Error('Migration tried to use old style "done" callback. Please upgrade to "umzug" and return a promise instead.');
        }],
      }
    });

    var action = result.answer.match(/a/i) && 'up' || result.answer.match(/e/i) && 'down'

    umzug[action]().then(() => {
      return db.sync().then(() => {
        done();
      });
    }).catch(done);
  });
});
