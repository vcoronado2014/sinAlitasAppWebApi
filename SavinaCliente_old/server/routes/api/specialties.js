'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Specialty = db.model('specialty');

  /**
   * Get all specialties.
   */
  router.get('/', function (req, res, next) {

    Specialty.findAll().then(function success(specialties) {
      if (!specialties || is.empty(specialties)) {
        return res.status(204).send([]);
      }

      res.send(specialties);
    }).catch(next);

  });

  /**
   * Get specialties available for the current workplace.
   */
  router.get('/for/current/workplace', function (req, res, next) {

    Specialty.findAll({
      include: [{
        model: db.model('agreement'),
        attributes: ['id'],
        required: true,
        where: {
          deletedAt: null
        },
        include: [{
          attributes: ['id'],
          model: db.model('workplace'),
          required: true,
          where: {
            id: req.session.workplace.id
          }
        }]
      }]
    }).then(function (specialties) {
      if (!specialties || is.empty(specialties)) {
        return res.status(204).send([]);
      }

      res.send(specialties);
    }).catch(next);
  });

};
