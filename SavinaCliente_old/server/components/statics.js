'use strict';

var debug = require('debug')('app:statics');
var inflection = require('inflection');
var path = require('path');
var walk = require('walk');

var waterline = component('database');

module.exports = {

  models: [],

  model: function (model, slug) {
    model = inflection.singularize(String(model).toLowerCase());
    slug = inflection.singularize(String(slug).toLowerCase());

    return this.models[model][slug];
  },

  load: function statics(config, done) {
    var walker = walk.walk(config.basedir);
    var models = this.models;

    walker.on('file', function (root, stats, next) {
      if (path.extname(stats.name) !== '.js') {
        return next();
      }

      var modelname = config.prefix;
      modelname += root.replace(config.basedir, '').replace(/[\/\\]+/g, '_');
      modelname += '_' + path.basename(stats.name, '.js');
      modelname = modelname.replace(/_+/g, '_');

      var Model = waterline.model(modelname);

      if (!Model) {
        throw new Error("Model '" + modelname + "' does not exists!");
      }

      Model.find(function (err, results) {
        if (err) {
          debug("Could not retrieve %s!", modelname);
          return next();
        }

        if (!results.length) {
          debug("There is no data for %s!", modelname);
          return next();
        }

        var staticname = modelname.replace(config.prefix, '');

        models[staticname] = {};

        results.forEach(function (result) {
          models[staticname][result.slug] = result;

          debug("%s : %s --> %s", staticname, result.slug, result.id);
        });

        next();
      });
    });

    walker.on('error', function (root, stats) {
      panic("Could not register statics!\n", root, stats);
    });

    walker.on('end', done);
  }

};
