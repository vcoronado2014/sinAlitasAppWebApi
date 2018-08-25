'use strict';

require('../server/globals.js')(global);
require('colors');

const Sequelize = require('sequelize');
const prompt = require('prompt');
const walk = require('walk');
const path = require('path');
const is = require('fi-is');
const fs = require('fs');

const definitions = require('./definitions');

var dbnames = {};
var db;

function getPath() {
  return path.normalize(path.join.apply(null, arguments));
}

function migrate() {
  var queryTemplate = fs.readFileSync(getPath('migrate', 'copy.sql'), 'utf8');
  var counter = 0;

  console.log("\n  Copying data...".bold);

  function copyNext() {
    var definition = definitions[counter];
    var query = queryTemplate;

    query = query.replace(/\$sourceDb/g, dbnames.source);
    query = query.replace(/\$destDb/g, dbnames.dest);
    query = query.replace(/\$sourceTable/g, definition.sourceTable);
    query = query.replace(/\$destTable/g, definition.destTable);
    query = query.replace(/\$sourceFields/g, '[' + definition.sourceFields.join('], [') + ']');
    query = query.replace(/\$destFields/g, '[' + definition.destFields.join('], [') + ']');

    db.query(query).then(copied).catch(error);
  }

  function validateNext() {
    var definition = definitions[counter];

    Promise.all([
      db.query('SELECT * FROM [' + dbnames.source + '].[dbo].[' + definition.sourceTable + '] ORDER BY [id]', {
        type: db.QueryTypes.SELECT
      }),

      db.query('SELECT * FROM [' + dbnames.dest + '].[dbo].[' + definition.destTable + '] ORDER BY [id]', {
        type: db.QueryTypes.SELECT
      })
    ]).then((values) => {
      var oldData = values[0];
      var newData = values[1];

      oldData.forEach((odata, row) => {
        for (let oldColumn in odata) {
          if (odata[oldColumn]) {
            let newColumn = definition.destFields[definition.sourceFields.indexOf(oldColumn)];
            let newEntry = newData[row][newColumn];
            let oldEntry = odata[oldColumn];
            let isValid = false;

            fs.appendFileSync(path.join(__dirname, 'validation.log'), "[" + definition.sourceTable + "][" + oldColumn + "][" + oldEntry + "] = [" + definition.destTable + "][" + newColumn + "][" + newEntry + "] ? ", 'utf8');

            if (!newColumn) {
              fs.appendFileSync(path.join(__dirname, 'validation.log'), "(Column [" + oldColumn + "] is not migrated) ", 'utf8');
              isValid = true;
            } else if (is.date(oldEntry) && is.date(newEntry)) {
              isValid = oldEntry.getTime() === newEntry.getTime();
            } else {
              isValid = newEntry === oldEntry;
            }

            fs.appendFileSync(path.join(__dirname, 'validation.log'), (isValid ? "OK" : "ERROR") + "\n", 'utf8');

            if (!isValid) {
              throw new Error("[" + definition.sourceTable + "][" + oldColumn + "]: '" + oldEntry + "' != [" + definition.destTable + "][" + newColumn + "]: '" + newEntry + "'");
            }
          }
        }

        fs.appendFileSync(path.join(__dirname, 'validation.log'), "\n", 'utf8');
      });

      validated();

      return null;
    }).catch(err => {
      console.error("\n  Couldn't validate the data:".red.bold, err.message.red, "\n");
      process.exit(1);
    });
  }

  function copyComplete() {
    console.log("\n  Data copied!".green.bold);
    console.log("\n  Validating data...".bold);

    counter = 0;

    fs.writeFileSync(path.join(__dirname, 'validation.log'), '', 'utf8');

    validateNext();
  }

  function validationComplete() {
    console.log("\n  Data validated!".green.bold);
    console.log("\n  Migration is now complete!\n\n".bold);

    process.exit(0);
  }

  function copied() {
    if (++counter === definitions.length) {
      return copyComplete();
    }

    copyNext();

    return null;
  }

  function validated() {
    if (++counter === definitions.length) {
      return validationComplete();
    }

    validateNext();
  }

  function error(err) {
    console.error("\n  Couldn't copy the data:".red.bold, err.message.red + "\n");
    process.exit(1);
  }

  copyNext();
}

console.log("\n  Migration script for SAVINA".cyan.bold);
console.log("  From version 1.x to version 2.x\n");

prompt.message = "  ".reset;
prompt.delimiter = "";

prompt.start();

prompt.get([{
  name: 'source',
  description: 'Enter the source database name:',
  default: 'savina_old',
  required: true
}, {
  name: 'dest',
  description: 'Enter the destination database name:',
  default: 'savina',
  required: true
}], (err, answers) => {
  if (err) {
    console.log("\n\n  Aborted\n".bold.yellow);
    process.exit(0);
  }

  dbnames = answers;

  console.log("\n  IMPORTANT: ".bold + "This script will create all the necessary tables on [$1], deleting any data it may holds.\n  The data on [$2] will not be modified in any way.\n".replace('$1', dbnames.dest.bold.cyan).replace('$2', dbnames.source.bold.cyan));

  prompt.get([{
    name: 'answer',
    pattern: /^yes|no$/i,
    message: "Please type " + "yes".bold + " or " + "no".bold + "...",
    description: 'Continue? (yes/no)',
    required: true
  }], (err, result) => {
    if (err || !result.answer.match(/^yes$/i)) {
      console.log("\n\n  Aborted\n".bold.yellow);
      process.exit(0);
    }

    const config = require('../server/config/database.js');

    db = new Sequelize(config.database, config.username, config.password, {
      dialectOptions: config.dialectOptions,
      dialect: config.dialect,
      host: config.host,
      port: config.port,
      logging: false
    });

    const modelsDir = path.resolve(getPath('server', 'models'));
    const walker = walk.walk(modelsDir);

    console.log("\n  Loading models from [%s]...\n".bold, modelsDir);

    walker.on('file', (root, stats, next) => {
      if (path.extname(stats.name) === '.js') {
        /* Get file name */
        let file = getPath(root, stats.name);

        /* Create the model in Mongoose */
        let Model = db.import(file);

        console.log("  %s [%s] --> %s", Model.name.bold.cyan, Model.tableName.bold, file.replace(modelsDir + '/', ''));
      }

      next();
    });

    walker.on('errors', (root, stats) => {
      console.error("\n  Could not define models!\n".bold.red, root, stats);
      process.exit(1);
    });

    walker.on('end', () => {
      console.log("\n  Performing associations...".bold);

      for (var name in db.models) {
        if (db.model(name)) {
          let Model = db.model(name);

          if (Model && is.function(Model.associate)) {
            Model.associate(db.models);
          }
        }
      }

      console.log("\n  Associations complete!".green.bold);

      console.log("\n  Force synchronizing models with database (dropping and creating tables)...".bold);

      db.sync({
        force: true
      }).then(() => {
        console.log("\n  Synchronization complete!".green.bold);

        migrate();

        return null;
      }).catch((err) => {
        console.error("\n  Synchronization failed: ".red.bold + err.message.red + "\n");
        process.exit(1);
      });
    });
  });
});
