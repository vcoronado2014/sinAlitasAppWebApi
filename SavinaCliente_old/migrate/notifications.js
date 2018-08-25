'use strict';

require('../server/globals')(global);
require('colors');

const config = require('../server/config/database');
const Sequelize = require('sequelize');
const prompt = require('prompt');
const Umzug = require('umzug');
const path = require('path');

console.log("\n  Migration script for New Request Notifications for SAVINA".cyan.bold);
console.log("  For version 3.x");

console.log("\n  Using [%s] database at [%s].\n", config.database.bold, config.host.bold);

prompt.message = '  '.reset;
prompt.delimiter = ''.reset;

prompt.start();

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

  console.log('');

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

  umzug.up().then(() => {
    /* Register needed models */
    db.import('../server/models/user');
    db.import('../server/models/preference');

    /* Associate models */
    db.model('user').hasOne(db.model('preference'));
    db.model('preference').belongsTo(db.model('user'));

    return db.sync();
  })

  .then(() => db.model('preference').update({
    emailRequestCreated: true,
    soundNotifications: true
  }, {
    where: {}
  }))

  .then(() => {
    console.log('\n  Migrations excecuted!\n'.green.bold);
    process.exit();
  })

  .catch((err) => {
    console.error(`\n  ${ String(err).red.bold }\n`);
    process.exit(1);
  });
});
