'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Message = db.model('requestMessage');
  var Request = db.model('request');

  /**
   * Check if a user id is allowed to request or create resources for a specific Request.
   */
  router.all([
    '/', '/of/:id'
  ], function (req, res, next) {

    if (!req.session.user || !req.session.user.id) {
      return res.sendStatus(403);
    }

    Request.count({
      where: {
        id: Number(req.params.id || req.body.requestId),
        $or: [{
          creatorUserId: req.session.user.id
        }, {
          specialistUserId: req.session.user.id
        }]
      }
    }).then(function success(count) {
      if (!count) {
        return res.sendStatus(403);
      }

      next();

      return null;
    }).catch(next);

  });

  /**
   * Create a new message.
   */
  router.post('/', function (req, res, next) {

    Message.create({
      workplaceId: req.session.workplace.id,
      requestId: Number(req.body.requestId),
      userId: req.session.user.id,
      body: req.body.body
    }).then(function success(message) {
      if (!message || is.empty(message)) {
        return res.sendStatus(400);
      }

      res.status(201).send(message);
    }).catch(next);

  });

  /**
   * Get messages for a request.
   */
  router.get('/of/:id', function (req, res, next) {

    var query = {
      where: {
        requestId: Number(req.params.id)
      },
      order: [
        ['sentAt', 'ASC']
      ]
    };

    if (req.query.newest && !isNaN(Date.parse(req.query.newest))) {
      query.where.sentAt = {
        $gt: new Date(req.query.newest)
      };
    } else if (req.query.oldest && !isNaN(Date.parse(req.query.oldest))) {
      query.where.sentAt = {
        $lt: new Date(req.query.oldest)
      };
    } else {
      query.limit = 10;
      query.order = [
        ['sentAt', 'DESC']
      ];
    }

    if (req.query.update) {
      query.where.userId = {
        $not: req.session.user.id
      };
    }

    Message.findAll(query).then(function success(messages) {
      if (!messages || is.empty(messages)) {
        return res.status(204).send([]);
      }

      if (query.limit) {
        messages.reverse();
      }

      res.send(messages);
    }).catch(next);

  });

  /**
   * Mark a message as received.
   */
  router.put('/received', function (req, res, next) {

    Message.findById(Number(req.body.id)).then(function success(message) {
      if (!message || is.empty(message)) {
        return res.sendStatus(400);
      }

      message.set('receivedAt', new Date());

      return message.save().then(function success(message) {
        if (!message || is.empty(message)) {
          return res.sendStatus(400);
        }

        res.send({
          receivedAt: message.receivedAt,
          id: message.id
        });
      });
    }).catch(next);

  });

};
