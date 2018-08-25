'use strict';

var debug = require('debug')('app:models');
var walk = require('walk');
var path = require('path');
var is = require('fi-is');

var db = component('database');

function getPath() {
  return path.normalize(path.join.apply(null, arguments));
}

module.exports = function (config, callback) {

  var walker = walk.walk(config.basedir);

  walker.on('file', function (root, stats, next) {
    if (path.extname(stats.name) === '.js') {
      /* Get file name */
      var file = getPath(root, stats.name);

      /* Create the model in Mongoose */
      var Model = db.import(file);

      debug("%s (%s) --> %s", Model.name, Model.tableName, file);
    }

    next();
  });

  walker.on('errors', function (root, stats) {
    panic("Could not define models!\n", root, stats);
  });

  walker.on('end', function () {
    debug("Performing associations...");

    Object.keys(db.models()).forEach(function (name) {
      if (db.model(name)) {
        var Model = db.model(name);

        if (Model && is.function(Model.associate)) {
          debug("Associating [%s]...", name);
          Model.associate(db.models());
        }
      }
    });

    debug("Associations complete!");
    debug("Synchronizing models into database...");

    db.sync().then(function () {
      debug("Synchronization complete!");
      callback();
    }).catch(function (err) {
      debug("Synchronization failed!".red);
      panic(err);
    });
  });

};
