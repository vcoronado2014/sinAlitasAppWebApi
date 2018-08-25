'use strict';

var inspect = require('util').inspect;
var path = require('path');
var walk = require('walk');

var data = {};

walk.walkSync(__dirname, {
  listeners: {
    file: (root, stats) => {
      if (path.extname(stats.name) === '.json') {
        data[path.basename(stats.name, '.json')] = require(path.join(__dirname, stats.name));
        console.log("  Loaded data from [%s]", stats.name);
      }
    },

    errors: (root, stats) => {
      console.error("\n  Could not load data:".bold.red, String(root).red, inspect(stats));
      process.exit(1);
    }
  }
});

module.exports = data;
