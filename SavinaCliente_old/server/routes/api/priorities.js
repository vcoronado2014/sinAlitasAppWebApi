'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Priority = db.model('priority');

  router.get('/', function (req, res, next) {

    Priority.findAll().then(function success(priorities) {
      if (!priorities || is.empty(priorities)) {
        return res.status(204).send([]);
      }

      res.send(priorities);
    }).catch(next);

  });

};
