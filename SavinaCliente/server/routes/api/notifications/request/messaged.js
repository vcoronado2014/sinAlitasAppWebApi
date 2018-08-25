'use strict';

var sockets = require('fi-sockets');
var is = require('fi-is');

var notifier = component('notifier');

var REQUEST = 'request';
var MESSAGED = 'messaged';

module.exports = function (router, db) {

  var Notification = db.model('notification');
  var Preference = db.model('preference');
  var Request = db.model(REQUEST);
  var Link = db.model('link');

  /**
   * Check if both users are already in the room.
   */
  router.post('/', function (req, res, next) {

    sockets.of('room').in('request-' + req.body.requestId).clients(function (err, clients) {
      if (err) {
        return next(err);
      }

      if (clients && clients.length > 1) {
        return res.sendStatus(204);
      }

      next();
    });

  });

  /**
   * Notify user of a new message on an assigned request.
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
        return res.sendStatus(400);
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
        return res.sendStatus(403);
      }

      return Notification.create({
        senderId: req.session.user.id,
        workplaceId: workplaceId,
        foreignKey: request.id,
        receiverId: receiverId,
        action: MESSAGED,
        model: REQUEST
      }).then(function success() {
        /* Notify the other user */
        sockets.of('notifications').to('user-' + receiverId)
          .emit(REQUEST + ' ' + MESSAGED);

        return Preference.findOne({
          attributes: ['emailRequestMessaged', 'userId'],
          where: {
            userId: receiverId,
          }
        }).then(function (preference) {
          if (!preference || is.empty(preference) || preference.emailRequestMessaged) {
            var subject = "Nuevo mensaje en solicitud #" + request.get('sid');

            Link.create({
              requestId: request.id,
              url: '/requests/room/' + request.id
            }).then(function (link) {
              var locals = {
                url: req.protocol + '://' + req.get('host') + '/links/' + link.hash,
                sender: req.session.user,
                request: request.get()
              };

              notifier.message.pending(req.session.user, receiver, subject, request, locals);
            }).catch(function (err) {
              console.dir(err);
            });

          }

          return res.sendStatus(204);
        });
      });
    }).catch(next);

  });

  /**
   * Cancel any notification of a new message on an own or assigned request for current user.
   */
  router.delete('/:id', function (req, res, next) {

    Request.findById(Number(req.params.id)).then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      var senderId, receiverId;

      if (req.session.user.id === request.creatorUserId) {
        senderId = request.specialistUserId;
        receiverId = request.creatorUserId;
      } else if (req.session.user.id === request.specialistUserId) {
        receiverId = request.specialistUserId;
        senderId = request.creatorUserId;
      } else {
        return res.sendStatus(403);
      }

      try {
        notifier.message.cancel(senderId, receiverId, request.id);
      } catch (ex) {}

      res.sendStatus(204);
    }).catch(next);

  });

  /**
   * Clears message notification.
   */
  router.put('/', function (req, res, next) {

    Notification.update({
      seenAt: new Date()
    }, {
      where: {
        foreignKey: Number(req.body.requestId),
        receiverId: req.session.user.id,
        action: MESSAGED,
        model: REQUEST,
        seenAt: null
      }
    }).then(function success() {
      res.sendStatus(204);
    }).catch(next);

  });

};
