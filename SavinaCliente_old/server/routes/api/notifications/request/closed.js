'use strict';

var sockets = require('fi-sockets');
var is = require('fi-is');

var notifier = component('notifier');

var REQUEST = 'request';
var CLOSED = 'closed';

module.exports = function (router, db) {

  var RequestType = db.model(REQUEST + 'Type');
  var Notification = db.model('notification');
  var Preference = db.model('preference');
  var Request = db.model(REQUEST);
  var Link = db.model('link');

  /**
   * Check if the type parameter exists and it's correct.
   */
  router.get('/:type', function (req, res, next) {

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
   * Gets all closed request notifications for a request type.
   */
  router.get('/:type', function (req, res, next) {

    var query = {
      attributes: ['id'],
      where: {
        requestTypeId: req.body.type.id,
        closedAt: {
          $not: null
        }
      }
    };

    if (req.session.user.specialty) {
      query.where.specialistWorkplaceId = req.session.workplace.id;
      query.where.specialistUserId = req.session.user.id;
    } else {
      return res.status(412).end();
    }

    Request.findAll(query).then(function (requests) {
      if (!requests || is.empty(requests)) {
        return res.status(204).send([]);
      }

      var query = {
        where: {
          workplaceId: req.session.workplace.id,
          receiverId: req.session.user.id,
          foreignKey: [],
          model: REQUEST,
          action: CLOSED,
          seenAt: null
        }
      };

      requests.forEach(function (request) {
        query.where.foreignKey.push(request.id);
      });

      return Notification.findAll(query).then(function (notifications) {
        if (!notifications || is.empty(notifications)) {
          return res.status(204).send([]);
        }

        res.send(notifications);
      });
    }).catch(next);
  });

  /**
   * Notify specialist user of a request closed.
   */
  router.post('/', function (req, res, next) {

    Request.findOne({
      where: {
        id: Number(req.body.requestId)
      },
      include: [{
        attributes: ['id', 'name'],
        model: db.model('user'),
        as: 'creatorUser'
      }, {
        attributes: ['id', 'name'],
        model: db.model('workplace'),
        as: 'creatorWorkplace'
      }, {
        attributes: ['id', 'email'],
        model: db.model('user'),
        as: 'specialistUser'
      }, {
        model: db.model('requestType')
      }]
    }).then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      if (request.creatorUserId !== req.session.user.id) {
        return res.sendStatus(403);
      }

      return Notification.create({
        workplaceId: request.specialistWorkplaceId,
        receiverId: request.specialistUserId,
        sender: req.session.user.id,
        foreignKey: request.id,
        action: CLOSED,
        model: REQUEST
      }).then(function success() {
        sockets.of('room').to('request-' + request.id)
          .emit(CLOSED);

        sockets.of('notifications').to('user-' + request.specialistUserId)
          .emit(REQUEST + ' ' + CLOSED);

        return Preference.findOne({
          attributes: ['emailRequestClosed', 'userId'],
          where: {
            userId: request.specialistUserId,
          }
        }).then(function (preference) {
          if (!preference || is.empty(preference) || preference.emailRequestClosed) {
            var subject = "Se ha cerrado la solicitud #" + request.get('sid');

            Link.create({
              requestId: request.id,
              url: '/requests/' + request.id
            }).then(function (link) {
              var locals = {
                url: req.protocol + '://' + req.get('host') + '/links/' + link.hash,
                sender: req.session.user,
                request: request.get()
              };

              notifier.request.closed(request.specialistUser.email, subject, locals);
            }).catch(function (err) {
              console.dir(err);
            });
          }

          return res.status(204).end();
        });
      });
    }).catch(next);

  });

  /**
   * Clears closed request notification.
   */
  router.put('/', function (req, res, next) {

    var query = {
      where: {
        foreignKey: Number(req.body.requestId),
        receiverId: req.session.user.id,
        action: CLOSED,
        model: REQUEST,
        seenAt: null
      }
    };

    Notification.findOne(query).then(function success(notification) {
      if (notification) {
        notification.set('seenAt', new Date());

        return notification.save();
      }

      query.where.seenAt = new Date();

      return Notification.create(query.where);
    }).then(function success(notification) {
      if (!notification || is.empty(notification)) {
        return res.sendStatus(400);
      }

      res.send(notification);
    }).catch(next);

  });

};
