'use strict';

require('../server/globals')(global);
require('colors');

const inflection = require('inflection');
const Sequelize = require('sequelize');
const Progress = require('progress');
const prompt = require('prompt');
const walk = require('walk');
const path = require('path');
const is = require('fi-is');

const config = require('../server/config/database');

console.log("\n  Setup script for SAVINA".cyan.bold);
console.log("  For version 2.x");

console.log("\n  Loading models data...\n".bold);
const data = require('./data');
console.log("\n  Models data loaded!\n".green.bold);

var db;

function getPath() {
  return path.normalize(path.join.apply(null, arguments));
}

function saveData() {
  var validations = [];
  var builds = [];
  var saves = [];
  var total = 0;

  console.log("\n  Building models and validating...".bold);

  for (var table in data) {
    if (is.array(data[table])) {
      for (let i = 0, l = data[table].length; i < l; i++) {
        let model = inflection.singularize(table);
        let build = db.model(model).build(data[table][i]);

        total++;

        validations.push(build.validate());
        builds.push(build);
      }
    }
  }

  Promise.all(validations).then(errors => {
    for (let i = 0, l = errors.length; i < l; i++) {
      let err = errors[i];

      if (err) {
        console.error("\n  Validation failed:".red.bold, err.message);
        process.exit(1);
      }
    }

    console.log("\n  All validations passed!".green.bold);

    console.log("\n  Inserting base data. Please wait...\n".bold);

    var progress = new Progress("  [:bar] :percent :eta\s", {
      renderThrottle: (1000 / 30),
      total: total,
      width: 30
    });

    function onCatch(build) {
      console.error("\n  Insert failed for [%s]:".red.bold, build.Model.name);
      console.error("  Values:".red.bold, JSON.stringify(build.get()).red);
    }

    for (let i = 0, l = builds.length; i < l; i++) {
      let build = builds[i];
      let save = build.save();

      save.then(progress.tick.bind(progress)).catch(onCatch.bind(null, build));

      saves.push(save);
    }

    Promise.all(saves).then(() => {
      console.log("\n  All data inserted!".green.bold, "\n");

      prompt.get([{
        name: 'answer',
        pattern: /yes|no/i,
        message: "Please type " + "yes".bold + " or " + "no".bold + "...",
        description: "Do you want to create the administrator's account? (yes/no)",
        required: true
      }], (err, result) => {
        if (err) {
          console.log("\n\n  Aborted\n".bold.yellow);
          process.exit(0);
        }

        if (result.answer.match(/yes/i)) {
          require('./admin');
        } else {
          console.log("\n  Finished!\n".green.bold);
          process.exit(0);
        }
      });

      return null;
    }).catch(err => {
      for (let i = 0, l = err.errors.length; i < l; i++) {
        let error = err.errors[i];
        console.error(("  Column [" + error.path + "] ").red.bold + (error.type + ": ").red.bold + error.message.red);
      }

      console.log("");
      process.exit(1);
    });
  }).catch(err => {
    console.error("\n  Validations failed:".red.bold, err.message.red, "\n");
    process.exit(1);
  });
}

console.log("  Using [%s] database at [%s].\n", config.database.bold, config.host.bold);

console.log("  IMPORTANT:".bold, "This will drop any existing tables, create the new tables and insert all base data into the [" + config.database.bold + "] database.\n");

prompt.message = "  ".reset;
prompt.delimiter = "";

prompt.start();

prompt.get([{
  name: 'answer',
  pattern: /^yes|no$/i,
  message: "Please type " + "yes".bold + " or " + "no".bold + "...",
  description: "Continue? (yes/no)",
  required: true
}], (err, result) => {
  if (err || !result.answer.match(/^yes$/i)) {
    console.log("\n\n  Aborted\n".bold.yellow);
    process.exit(0);
  }

  db = new Sequelize(config.database, config.username, config.password, {
    dialectOptions: config.dialectOptions,
    dialect: config.dialect,
    host: config.host,
    port: config.port,
    logging: false
  });

  const modelsPath = path.resolve(getPath('server', 'models'));
  const walker = walk.walk(modelsPath);

  console.log("\n  Loading models from [%s]...\n".bold, modelsPath);

  walker.on('file', (root, stats, next) => {
    if (path.extname(stats.name) === '.js') {
      /* Get file name */
      var file = getPath(root, stats.name);

      /* Create the model in Mongoose */
      var Model = db.import(file);

      console.log("  %s [%s] --> %s", Model.name.bold.cyan, Model.tableName.bold, file.replace(modelsPath + '/', ''));
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

    console.log("\n  Synchronizing models with the database (dropping and creating tables)...".bold);

    db.sync({
      force: true
    }).then(() => {
      console.log("\n  Synchronization complete!".green.bold);

      saveData();

      return null;
    }).catch(err => {
      console.error("\n  Synchronization failed: ".red.bold + err.message.red + "\n");
      process.exit(1);
    });
  });
});
