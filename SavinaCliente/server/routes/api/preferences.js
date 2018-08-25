'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Preference = db.model('preference');

  /**
   * Obtain current user's preferences.
   */
  router.get('/own', function (req, res, next) {

    Preference.findOne({
      where: {
        userId: req.session.user.id
      }
    }).then(function (preference) {
      if (!preference || is.empty(preference)) {
        return Preference.create({
          userId: req.session.user.id
        }).then(function success(preference) {
          res.status(201).send(preference);
        });

      }

      res.send(preference);
    }).catch(next);

  });

  /**
   * Create or update current user's preferences.
   */
  router.put('/', function (req, res, next) {

    Preference.findOne({
      where: {
        userId: req.session.user.id
      }
    }).then(function success(preference) {
      if (!preference || is.empty(preference)) {
        return res.sendStatus(400);
      }

      return preference.update(req.body).then(function success() {
        res.sendStatus(204);
      });
    }).catch(next);

  });

};
