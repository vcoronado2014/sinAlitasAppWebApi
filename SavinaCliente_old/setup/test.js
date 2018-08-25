'use strict';

require('../server/globals')(global);
require('colors');

const config = require('../server/config/database');
const Sequelize = require('sequelize');
const walk = require('walk');
const path = require('path');
const is = require('fi-is');

function getPath() {
  return path.normalize(path.join.apply(null, arguments));
}

var db = new Sequelize(config.database, config.username, config.password, {
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

  console.log("\n  Synchronizing models with the database...".bold);

  db.sync().then(() => {
    console.log("\n  Synchronization complete!".green.bold);

    console.log("\n  Creating test users...".bold);

    return db.model('user').bulkCreate([{
      name: 'Guillermo General',
      password: '1234',
      email: 'general@savina.cl',
      roleId: 2
    }, {
      name: 'Néstor Neurólogo',
      password: '1234',
      email: 'neurologo@savina.cl',
      specialtyId: 2,
      roleId: 2
    }, {
      name: 'Carlos Cardiólogo',
      password: '1234',
      email: 'cardiologo@savina.cl',
      specialtyId: 1,
      roleId: 2
    }]).then(() => {
      console.log("\n  Creating test workplaces...".bold);

      return db.model('workplace').bulkCreate([{
        name: 'Clínica',
        private: true
      }, {
        name: 'Hospital',
        private: false
      }, {
        name: 'Consulta',
        private: true
      }, {
        name: 'SAPU',
        private: false
      }]).then(() => {
        console.log("\n  Associating workplaces to users...".bold);

        return db.model('workplace').findAll().then((workplaces) => {
          return workplaces[0].setUsers([2, 3]).then(() => {
            return workplaces[1].setUsers([1]).then(() => {
              return workplaces[2].setUsers([3]).then(() => {
                return workplaces[3].setUsers([1, 3]).then(() => {
                  console.log("\n  Creating test agreements...".bold);

                  return db.model('agreement').bulkCreate([{
                    name: 'Clínica - Hospital',
                    quota: 10
                  }, {
                    name: 'Clínica - Consulta',
                    quota: 10
                  }, {
                    name: 'Clínica - SAPU',
                    quota: 10
                  }, {
                    name: 'Consulta - SAPU',
                    quota: 10
                  }, {
                    name: 'Interno Hospital',
                    quota: 10
                  }, {
                    name: 'Interno Clínica',
                    quota: 10

                  }]).then(() => {
                    console.log("\n  " + "Associating agreements to workplaces...".bold);

                    return db.model('agreement').findAll().then((agreements) => {
                      return agreements[0].setWorkplaces([1, 2]).then(() => {
                        return agreements[1].setWorkplaces([1, 3]).then(() => {
                          return agreements[2].setWorkplaces([1, 4]).then(() => {
                            return agreements[3].setWorkplaces([3, 4]).then(() => {
                              return agreements[4].setWorkplaces([2, 2]).then(() => {
                                return agreements[4].setWorkplaces([1, 1]).then(() => {
                                  console.log("\n  Everything should be fine now :)\n".rainbow);
                                  process.exit(0);
                                });
                              });
                            });
                          });
                        });
                      });
                    });
                  });
                });
              });
            });
          });
        });
      });
    }).catch((err) => {
      console.error("\n  Operation error:".red.bold, err.message.red, "\n");
      console.dir(err, {
        colors: true
      });
      console.error("");
      process.exit(1);
    });
  }).catch(err => {
    console.error("\n  Synchronization failed:".red.bold, err.message.red, "\n");
    process.exit(1);
  });
});
