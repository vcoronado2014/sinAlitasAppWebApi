'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Motive = db.model('motive');

  router.get('/', function (req, res, next) {

    Motive.findAll().then(function success(motives) {
      if (!motives || is.empty(motives)) {
        return res.status(204).send([]);
      }

      res.send(motives);
    }).catch(next);

  });

};
