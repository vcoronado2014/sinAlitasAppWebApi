'use strict';

var fileman = require('fi-fileman');
var path = require('path');
var is = require('fi-is');

module.exports = function (router, db) {

  var Attachment = db.model('requestAttachment');
  var Request = db.model('request');

  /**
   * Check if the user is part of the associated Request.
   */
  router.all([
    '/', '/of/:id'
  ], function (req, res, next) {

    Request.count({
      where: {
        id: Number((req.body.requestId || req.params.id)),
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
   * Check if the user is part of the associated Request.
   */
  router.all([
    '/by/id/:id', '/seen'
  ], function (req, res, next) {

    Attachment.count({
      // attributes: ['id', 'requestId'],
      where: {
        id: Number(req.body.id || req.params.id)
      },
      include: [{
        // attributes: ['id', 'creatorUserId', 'specialistUserId'],
        model: db.model('request'),
        required: true,
        where: {
          $or: [{
            creatorUserId: req.session.user.id
          }, {
            specialistUserId: req.session.user.id
          }]
        }
      }]
    }).then(function success(count) {
      if (!count) {
        return res.sendStatus(403);
      }

      next();

      return null;
    });

  });

  /**
   * Create a new Attachment.
   */
  router.post('/', function (req, res, next) {

    req.body.workplaceId = req.session.workplace.id;
    req.body.userId = req.session.user.id;
    req.body.files = [];

    if (!req.files || is.empty(req.files)) {
      return next();
    }

    /* Iterate over each uploaded file */
    req.files.forEach(function (file) {
      /* Path relative to the storage path */
      var destpath = path.join('requests', String(req.body.requestId), 'attachments');

      /* Move the file from the temp folder to the assigned one in the storage forlder */
      fileman.save(file, destpath, function (err, fileinfo) {
        if (err) {
          return next(err);
        }

        /* Queue up a new file to be added and a record to be created in the join table */
        req.body.files.push({
          filesize: fileinfo.stats.size,
          mimetype: fileinfo.type,
          filename: fileinfo.name,
          path: fileinfo.path,
          md5: fileinfo.md5
        });

        if (req.body.files.length === req.files.length) {
          next();
        }
      });
    });

  }, function (req, res, next) {

    Attachment.create(req.body, {
      include: [{
        model: db.model('file')
      }]
    }).then(function success(attachment) {
      if (!attachment || is.empty(attachment)) {
        return res.sendStatus(400);
      }

      res.status(201).send({
        id: attachment.id
      });
    }).catch(next);

  });

  /**
   * Get a Request's Attachments.
   */
  router.get('/of/:id', function (req, res, next) {

    Attachment.findAll({
      where: {
        requestId: Number(req.params.id)
      },
      include: [{
        model: db.model('workplace')
      }, {
        attributes: ['id', 'name'],
        model: db.model('user')
      }, {
        model: db.model('file')
      }]
    }).then(function (attachments) {
      if (!attachments || is.empty(attachments)) {
        return res.status(204).send([]);
      }

      res.send(attachments);
    }).catch(next);

  });

  /**
   * Gets an attachment.
   */
  router.get('/by/id/:id', function (req, res, next) {

    /* Now fetch the complete attachment */
    Attachment.findOne({
      where: {
        id: Number(req.params.id)
      },
      include: [{
        model: db.model('file')
      }, {
        attributes: ['id', 'name'],
        model: db.model('user')
      }]
    }).then(function (attachment) {
      if (!attachment || is.empty(attachment)) {
        return res.status(204).send({});
      }

      res.send(attachment);
    }).catch(next);

  });

  /**
   * Marks an attachment as seen.
   */
  router.put('/seen', function (req, res, next) {

    Attachment.findOne({
      where: {
        id: Number(req.body.id),
        userId: {
          $not: req.session.user.id
        }
      }
    }).then(function success(attachment) {
      if (!attachment || is.empty(attachment)) {
        return res.sendStatus(400);
      }

      attachment.set('seenAt', new Date());

      return attachment.save().then(function success(attachment) {
        res.send({
          seenAt: attachment.seenAt
        });
      });
    }).catch(next);

  });

};
