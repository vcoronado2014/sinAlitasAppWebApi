'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var RequestType = db.model('requestType');

  router.get('/', function (req, res, next) {

    RequestType.findAll().then(function success(requestTypes) {
      if (!requestTypes || is.empty(requestTypes)) {
        return res.status(204).send([]);
      }

      res.send(requestTypes);
    }).catch(next);

  });

};
