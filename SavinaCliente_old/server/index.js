'use strict';

/**** Register globals *****/
require('./globals')(global);
require('colors');

/*** Shutdown application ****/
process.on('SIGINT', function () {
  console.log("\n\n  Shutting down SAVINA...\n".bold);
  process.exit();
});

/**** Load app module ****/
module.exports = require('./app');
