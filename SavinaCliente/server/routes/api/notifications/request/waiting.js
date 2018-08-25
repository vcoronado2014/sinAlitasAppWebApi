'use strict';

var sockets = require('fi-sockets');
var is = require('fi-is');

var notifier = component('notifier');

var REQUEST = 'request';
var SEEN = 'seen';

module.exports = function (router, db) {

  var RequestType = db.model(REQUEST + 'Type');
  var Notification = db.model('notification');
  var Request = db.model(REQUEST);
  var User = db.model('user');
  var Link = db.model('link');

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
   * Notify related workplaces of the new request.
   */
  router.post('/', function (req, res, next) {

    Request.findOne({
      where: {
        id: Number(req.body.requestId)
      },
      include: [{
        model: db.model('user'),
        attributes: ['id', 'name'],
        as: 'creatorUser'
      }, {
        model: db.model('requestType')
      }]
    })

    /* Send a realtime notification to all connected counterpart users. */
    .then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      for (let i = 0, l = req.session.workplace.counterparts.length; i < l; i++) {
        sockets.of('notifications')
          .to(`workplace-${ req.session.workplace.counterparts[i] }`)
          .emit(`new ${ REQUEST }`);
      }

      return request.get();
    })

    /* Send an email to all counterpart users with the `emailRequestCreated`
     * preference turned on. */
    .then((request) => {
      return User.findAll({
        attributes: ['email'],
        distinct: true,
        where: {
          specialtyId: request.specialtyId,
          id: {
            $ne: req.session.user.id
          }
        },
        include: [{
          model: db.model('preference'),
          attributes: ['emailRequestCreated']
        }, {
          model: db.model('workplace'),
          attributes: ['id'],
          required: true,
          where: {
            id: req.session.workplace.counterparts
          }
        }]
      })

      .then((users) => {
        if (!users.length) {
          return null;
        }

        var subject = `Se ha creado una nueva solicitud #${ request.sid }`;

        var locals = {
          url: `${ req.protocol }://${ req.get('host') }/requests/worktable/waiting/${ request.requestType.slug }`,
          sender: req.session.user,
          request
        };

        for (let i = 0, l = users.length; i < l; i++) {
          let user = users[i];

          if (!user.preference || user.preference.emailRequestCreated) {
            notifier.request.waiting(users[i].email, subject, locals);
          }
        }
      });
    })

    .then(() => {
      return res.sendStatus(204);
    })

    .catch(next);

  });

  /**
   * Count all current user's waiting Requests.
   */
  router.get('/:type', function (req, res, next) {

    if (!req.session.user.specialtyId) {
      return res.status(403).end();
    }

    req.body.notifications = [];

    /**
     * Finds the requests for the provided agreement.
     */
    Request.findAll({
      attributes: ['id'],
      where: {
        creatorWorkplaceId: req.session.workplace.counterparts,
        specialtyId: req.session.user.specialtyId,
        requestTypeId: req.body.type.id,
        specialistWorkplaceId: null,
        specialistUserId: null,
        closedAt: null
      }
    }).then(function success(requests) {
      if (!requests || is.empty(requests)) {
        return next();
      }

      var current = 0;

      requests.forEach(function (request) {
        Notification.findOne({
          where: {
            receiverId: req.session.user.id,
            foreignKey: request.id,
            model: REQUEST,
            action: SEEN,
            seenAt: {
              $not: null
            }
          }
        }).then(function success(notification) {
          /* Mock a notification for the user */
          if (!notification) {
            req.body.notifications.push({
              receiverId: req.session.user.id,
              createdAt: request.createdAt,
              foreignKey: request.id,
              model: REQUEST,
              action: SEEN,
              seenAt: null
            });
          }

          if (++current === requests.length) {
            next();
          }
        }).catch(next);
      });

      return null;
    }).catch(next);

  }, function (req, res) {

    if (!req.body.notifications || is.empty(req.body.notifications)) {
      return res.status(204).send([]);
    }

    return res.send(req.body.notifications);

  });

  /**
   * Creates the seen notification.
   */
  router.put('/', function (req, res, next) {

    if (!req.body.requestId) {
      return res.sendStatus(400);
    }

    Notification.findOne({
      where: {
        foreignKey: Number(req.body.requestId),
        receiverId: req.session.user.id,
        model: REQUEST,
        action: SEEN,
        seenAt: {
          $not: null
        }
      }
    }).then(function success(notification) {
      if (notification) {
        return res.sendStatus(204);
      }

      return Request.findOne({
        where: {
          id: Number(req.body.requestId)
        }
      }).then(function success(request) {
        if (!request || is.empty(request)) {
          return res.sendStatus(400);
        }

        return Notification.create({
          foreignKey: Number(req.body.requestId),
          receiverId: req.session.user.id,
          senderId: request.creatorUserId,
          seenAt: new Date(),
          model: REQUEST,
          action: SEEN
        }).then(function success(notification) {
          if (!notification || is.empty(notification)) {
            return res.sendStatus(400);
          }

          res.send(notification);
        });
      });
    }).catch(next);

  });

};
