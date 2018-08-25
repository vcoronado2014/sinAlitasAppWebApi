'use strict';

var sockets = require('fi-sockets');
var is = require('fi-is');

var notifier = component('notifier');

var REQUEST = 'request';
var RETURNED = 'returned';

module.exports = function (router, db) {

  var Notification = db.model('notification');
  var Preference = db.model('preference');
  var Request = db.model(REQUEST);
  var Link = db.model('link');
  var User = db.model('user');

  /**
   * Notify user of a request returned.
   */
  router.post('/', function (req, res, next) {

    req.body.specialistWorkplaceId = parseInt(req.body.specialistWorkplaceId);
    req.body.specialistUserId = parseInt(req.body.specialistUserId);
    req.body.requestId = parseInt(req.body.requestId);

    Request.findOne({
      where: {
        id: req.body.requestId
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
        model: db.model('requestType')
      }]
    }).then(function (request) {
      if (!request || is.empty(request)) {
        return res.status(400).end();
      }

      if (request.creatorUserId !== req.session.user.id) {
        return res.status(403).end();
      }

      sockets.of('room').to('request-' + req.body.requestId)
        .emit(RETURNED);

      Notification.create({
        workplace: req.body.specialistWorkplaceId,
        receiver: req.body.specialistUserId,
        sender: req.session.user.id,
        foreignKey: request.id,
        action: RETURNED,
        model: REQUEST
      }).then(function () {
        sockets.of('notifications').to('user-' + req.body.specialistUserId)
          .emit(REQUEST + ' ' + RETURNED);

        return Preference.findOne({
          select: ['emailRequestReturned'],
          where: {
            userId: req.body.specialistUserId,
          }
        }).then(function (preference) {
          if (!preference || preference.emailRequestReturned) {
            var subject = "La solicitud #" + request.get('sid') + " fué devuelta a la mesa de trabajo";

            Link.create({
              requestId: request.id,
              url: '/requests/waiting/' + request.requestType.slug
            }).then(function (link) {
              return User.findOne({
                attributes: ['email'],
                where: {
                  id: req.body.specialistUserId
                }
              }).then(function (specialist) {
                var locals = {
                  url: req.protocol + '://' + req.get('host') + '/links/' + link.hash,
                  sender: req.session.user,
                  request: request.get()
                };

                notifier.request.returned(specialist.email, subject, locals);
              });
            });
          }
        });
      }).catch(function (err) {
        console.log("\n\nNotification error!".red.bold);
        console.dir(err);
        console.log("\n");
      });

      return res.status(204).end();
    }).catch(next);

  });

};
