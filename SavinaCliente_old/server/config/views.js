'use strict';

var path = require('path');

module.exports = {

  basedir: path.normalize(path.join(__basedir, 'views')),

  engine: 'jade'

};
