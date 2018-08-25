'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Notification = db.model('notification');
  var Workplace = db.model('workplace');
  var Request = db.model('request');

  /**
   * Count all current user's workplace notifications by workplace id.
   */
  router.get(['/', '/by/id/:id'], function (req, res, next) {

    var query = {
      where: {
        receiverId: req.session.user.id,
        seenAt: null
      }
    };

    if (req.params.id) {
      query.where.workplaceId = Number(req.params.id);
    }

    if (req.session.user.specialtyId) {
      query.include = [{
        attributes: [],
        model: db.model('request'),
        required: true,
        where: {
          specialistUserId: req.session.user.id,
          creatorUserId: {
            $not: null
          }
        }
      }];

      query.where.$and = {
        $or: [{
          action: ['answered', 'attached', 'messaged'],
          '$request.closedAt$': null
        }, {
          action: {
            $notIn: ['answered', 'attached', 'messaged']
          },
          '$request.closedAt$': {
            $not: null
          }
        }]
      };
    } else {
      query.include = [{
        attributes: [],
        model: db.model('request'),
        required: true,
        where: {
          creatorUserId: req.session.user.id,
          specialistUserId: {
            $not: null
          }
        }
      }];

      query.where.$and = {
        $or: [{
          action: ['answered', 'attached', 'messaged', 'taken'],
          '$request.closedAt$': null
        }, {
          action: {
            $notIn: ['answered', 'attached', 'messaged', 'taken']
          },
          '$request.closedAt$': {
            $not: null
          }
        }]
      };
    }

    Notification.count(query).then(function (count) {
      req.body.counter = {
        count: count
      };

      if (req.params.id) {
        req.body.counter.workplaceId = Number(req.params.id);
      }

      if (req.session.user.specialtyId) {
        /* Count new requests */
        return Workplace.getCounterparts(req.body.counter.workplaceId).then(function (counterparts) {
          return Request.findAll({
            attributes: ['id'],
            where: {
              specialtyId: req.session.user.specialtyId,
              creatorWorkplaceId: counterparts,
              specialistWorkplaceId: null,
              specialistUserId: null,
              closedAt: null
            }
          });
        }).then(function (requests) {
          if (!requests || is.empty(requests)) {
            return null;
          }

          var promises = [];

          requests.forEach(function (request) {
            promises.push(Notification.count({
              where: {
                receiverId: req.session.user.id,
                foreignKey: request.id,
                model: 'request',
                action: 'seen',
                seenAt: {
                  $not: null
                }
              }
            }));
          });

          return Promise.all(promises);
        }).then(function () {
          if (is.array(arguments[0])) {
            arguments[0].forEach(function (count) {
              if (!count) {
                req.body.counter.count++;
              }
            });
          }
        }).then(next);
      }

      next();
    }).catch(next);

  }, function (req, res) {

    res.send(req.body.counter);

  });

};
