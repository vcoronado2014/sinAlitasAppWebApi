'use strict';

var is = require('fi-is');

var REQUEST = 'request';

module.exports = function (router, db) {

  var RequestType = db.model(REQUEST + 'Type');
  var Notification = db.model('notification');

  /**
   * Check if the type parameter exists and it's correct.
   */
  router.all('/:type', function (req, res, next) {

    if (is.not.string(req.params.type)) {
      return res.sendStatus(400);
    }

    RequestType.findOne({
      where: {
        slug: req.params.type
      }
    }).then(function success(requestType) {
      if (!requestType || is.empty(requestType)) {
        return res.sendStatus(400);
      }

      req.body.type = requestType;

      next();

      return null;
    }).catch(next);

  });

  /**
   * Get ongoing notifications.
   */
  router.get('/:type', function (req, res, next) {

    var request = {
      model: db.model('request'),
      attributes: ['id'],
      required: true,
      where: {
        requestTypeId: req.body.type.id,
        closedAt: null,
        specialistUserId: {
          $not: null
        },
        specialistWorkplaceId: {
          $not: null
        }
      }
    };

    if (req.session.user.specialty) {
      request.where.specialistWorkplaceId = req.session.workplace.id;
      request.where.specialistUserId = req.session.user.id;
    } else {
      request.where.creatorWorkplaceId = req.session.workplace.id;
      request.where.creatorUserId = req.session.user.id;
    }

    Notification.findAll({
      where: {
        action: ['answered', 'attached', 'messaged', 'taken'],
        workplaceId: req.session.workplace.id,
        receiverId: req.session.user.id,
        model: REQUEST,
        seenAt: null
      },
      include: [request]
    }).then(function success(notifications) {
      if (!notifications || is.empty(notifications)) {
        return res.status(204).send([]);
      }

      res.send(notifications);
    }).catch(next);

  });

};
