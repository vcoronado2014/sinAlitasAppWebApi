'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Gender = db.model('gender');

  router.get('/', function (req, res, next) {

    Gender.findAll().then(function success(genders) {
      if (!genders || is.empty(genders)) {
        return res.status(204).send([]);
      }

      res.send(genders);
    }).catch(next);

  });

};
