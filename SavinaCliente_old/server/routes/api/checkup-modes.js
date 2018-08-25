'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var CheckupMode = db.model('checkupMode');

  router.get('/', function (req, res, next) {

    CheckupMode.findAll().then(function success(checkupModes) {
      if (!checkupModes || is.empty(checkupModes)) {
        return res.status(204).send([]);
      }

      res.send(checkupModes);
    }).catch(next);

  });

};
