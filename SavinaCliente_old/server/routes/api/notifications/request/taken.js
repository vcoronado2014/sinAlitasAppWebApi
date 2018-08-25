'use strict';

var notifier = component('notifier');

var sockets = require('fi-sockets');
var is = require('fi-is');

var REQUEST = 'request';
var TAKEN = 'taken';

module.exports = function (router, db) {

  var Notification = db.model('notification');
  var Preference = db.model('preference');
  var Request = db.model(REQUEST);
  var Link = db.model('link');

  /**
   * Notify creator user of a request taken.
   */
  router.post('/', function (req, res, next) {

    Request.findOne({
      where: {
        id: Number(req.body.requestId)
      },
      include: [{
        attributes: ['id', 'email'],
        model: db.model('user'),
        as: 'creatorUser'
      }, {
        attributes: ['id', 'name'],
        model: db.model('workplace'),
        as: 'creatorWorkplace'
      }, {
        attributes: ['id', 'name'],
        model: db.model('user'),
        as: 'specialistUser'
      }, {
        model: db.model('requestType')
      }]
    }).then(function success(request) {
      if (!request || is.empty(request)) {
        return res.status(400).end();
      }

      if (request.specialistUserId !== req.session.user.id) {
        return res.status(403).end();
      }

      return Notification.create({
        workplaceId: request.creatorWorkplaceId,
        receiverId: request.creatorUserId,
        sender: req.session.user.id,
        foreignKey: request.id,
        model: REQUEST,
        action: TAKEN
      }).then(function success() {
        /* Send realtime notification */
        sockets.of('notifications').to('user-' + request.creatorUserId)
          .emit('request taken');

        /* Send email notification */
        return Preference.findOne({
          attributes: ['emailRequestTaken', 'userId'],
          where: {
            userId: request.creatorUserId,
          }
        }).then(function success(preference) {
          if (!preference || is.empty(preference) || preference.emailRequestTaken) {
            var subject = "Han tomado tu solicitud #" + request.get('sid');

            Link.create({
              requestId: request.id,
              url: '/requests/room/' + request.id
            }).then(function (link) {
              var locals = {
                url: req.protocol + '://' + req.get('host') + '/links/' + link.hash,
                sender: req.session.user,
                request: request.get()
              };

              notifier.request.taken(request.creatorUser.email, subject, locals);
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
   * Get all taken requests notifications.
   */
  router.get('/', function (req, res, next) {

    Notification.findAll({
      where: {
        workplaceId: req.session.workplace.id,
        receiverId: req.session.user.id,
        model: REQUEST,
        action: TAKEN,
        seenAt: null
      }
    }).then(function success(notifications) {
      if (!notifications || is.empty(notifications)) {
        return res.status(204).send([]);
      }

      res.send(notifications);
    }).catch(next);

  });

  /**
   * Mark as seen any taken request notifications for a request.
   */
  router.put('/', function (req, res, next) {

    Notification.update({
      seenAt: new Date()
    }, {
      where: {
        foreignKey: req.body.requestId,
        receiverId: req.session.user.id,
        model: REQUEST,
        action: TAKEN,
        seenAt: null
      }
    }).then(function success() {
      res.sendStatus(204);
    }).catch(next);

  });

};
