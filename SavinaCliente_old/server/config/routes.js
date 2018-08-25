'use strict';

var path = require('path');

module.exports = {

  basedir: path.normalize(path.join(__basedir, 'routes')),

  debug: require('debug')('app:routes'),

  arguments: [
    component('database')
  ]

};
