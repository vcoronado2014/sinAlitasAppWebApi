'use strict';

var path = require('path');

module.exports = {

  prefix: 'static_',

  basedir: path.normalize(path.join(__basedir, 'collections', 'static'))

};
