'use strict';

var sockets = require('fi-sockets');
var is = require('fi-is');

var notifier = component('notifier');

var REQUEST = 'request';
var ANSWERED = 'answered';

module.exports = function (router, db) {

  var Notification = db.model('notification');
  var Preference = db.model('preference');
  var Request = db.model(REQUEST);
  var Link = db.model('link');

  /**
   * Notify user of a new answer on an own or assigned request.
   */
  router.post('/', function (req, res, next) {

    Request.findOne({
      where: {
        id: Number(req.body.requestId)
      },
      include: [{
        attributes: ['id', 'name', 'email'],
        model: db.model('user'),
        as: 'creatorUser'
      }, {
        attributes: ['id', 'name'],
        model: db.model('workplace'),
        as: 'creatorWorkplace'
      }, {
        attributes: ['id', 'name', 'email'],
        model: db.model('user'),
        as: 'specialistUser'
      }, {
        model: db.model('requestType')
      }]
    }).then(function success(request) {
      if (!request || is.empty(request)) {
        return res.status(400).end();
      }

      var workplaceId, receiverId, receiver;

      if (req.session.user.id === request.creatorUserId) {
        workplaceId = request.specialistWorkplaceId;
        receiverId = request.specialistUserId;
        receiver = request.specialistUser;
      } else if (req.session.user.id === request.specialistUserId) {
        workplaceId = request.creatorWorkplaceId;
        receiverId = request.creatorUserId;
        receiver = request.creatorUser;
      } else {
        return res.status(403).end();
      }

      return Notification.create({
        senderId: req.session.user.id,
        workplaceId: workplaceId,
        foreignKey: request.id,
        receiverId: receiverId,
        action: ANSWERED,
        model: REQUEST
      }).then(function success() {
        /* Send a notification to the room */
        sockets.of('room').to('request-' + request.id)
          .emit(ANSWERED);

        /* Notify the other user */
        sockets.of('notifications').to('user-' + receiverId)
          .emit(REQUEST + ' ' + ANSWERED);

        return Preference.findOne({
          attributes: ['emailRequestAnswered', 'userId'],
          where: {
            userId: receiverId,
          }
        }).then(function success(preference) {
          if (!preference || is.empty(preference) || preference.emailRequestAnswered) {
            var subject = "Nueva respuesta para solicitud #" + request.get('sid');

            Link.create({
              requestId: request.id,
              url: '/requests/room/' + request.id + '#answers'
            }).then(function (link) {
              var locals = {
                url: req.protocol + '://' + req.get('host') + '/links/' + link.hash,
                sender: req.session.user,
                request: request.get()
              };

              notifier.request.answered(receiver.email, subject, locals);
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
   * Mark as seen any answered request notifications for a request.
   */
  router.put('/', function (req, res, next) {

    Notification.update({
      seenAt: new Date()
    }, {
      where: {
        foreignKey: Number(req.body.requestId),
        receiverId: req.session.user.id,
        action: ANSWERED,
        model: REQUEST,
        seenAt: null
      }
    }).then(function success() {
      res.status(204).end();
    }).catch(next);

  });

};
