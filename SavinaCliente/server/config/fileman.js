'use strict';

var path = require('path');
var os = require('os');

module.exports = {

  tempdir: path.normalize(path.join(os.tmpDir(), 'savina', 'uploads')),

  stordir: path.normalize(path.join(__appdir, '..', 'savina-storage'))

};
