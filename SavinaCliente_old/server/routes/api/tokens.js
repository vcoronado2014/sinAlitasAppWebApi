'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Token = db.model('token');

  router.get('/exists/:secret', function (req, res, next) {

    if (!req.params.secret) {
      return res.sendStatus(400);
    }

    Token.findOne({
      where: {
        secret: req.params.secret
      }
    }).then(function success(token) {
      if (!token || is.empty(token)) {
        return res.sendStatus(400);
      }

      if (token.hasExpired()) {
        return res.sendStatus(412);
      }

      res.sendStatus(204);
    }).catch(next);

  });

};
