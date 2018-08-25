'use strict';

var path = require('path');
var fs = require('fs');

var credentials = path.join(__basedir, 'credentials');

module.exports = {

  key: fs.readFileSync(path.join(credentials, 'server.key')),

  cert: fs.readFileSync(path.join(credentials, 'server.crt')),

  ca: fs.readFileSync(path.join(credentials, 'server.ca')),

  port: 3001,

  secure: true

};
