'use strict';

var fileman = require('fi-fileman');
var path = require('path');
var is = require('fi-is');
var fs = require('fs');

module.exports = function (router, db) {

  var File = db.model('file');

  /**
   * Obtain a file.
   */
  router.get([
    '/:id', '/:id/:filename'
  ], function (req, res, next) {

    /* Check if the user is allowed to see the file */
    File.findOne({
      attributes: ['id'],
      where: {
        id: Number(req.params.id)
      },
      include: [{
        attributes: ['id', 'requestId'],
        model: db.model('requestAttachment'),
        required: false,
        include: [{
          attributes: ['id', 'creatorUserId', 'specialistUserId'],
          model: db.model('request'),
          required: false,
          where: {
            $or: [{
              creatorUserId: req.session.user.id
            }, {
              specialistUserId: req.session.user.id
            }]
          }
        }]
      }]
    }).then(function success(file) {
      if (!file || is.empty(file)) {
        return res.sendStatus(204);
      }

      if (!file.requestAttachments || is.empty(file.requestAttachments) || !file.requestAttachments[0].request) {
        return res.sendStatus(403);
      }

      /* Now get the full file data from the database */
      return File.findById(Number(req.params.id)).then(function success(file) {
        fs.access(fileman.resolve(file.path), fs.R_OK, function (err) {
          if (err) {
            err.status = 404;
            return next(err);
          }

          res.set({
            'Content-disposition': 'filename=' + (req.params.filename || path.basename(file.path)),
            'Cache-Control': 'max-age=31536000', // ~ 1 year
            'Content-Length': file.filesize,
            'Last-Modified': file.updatedAt,
            'Content-Type': file.mimetype,
            'ETag': file.md5
          });

          var reader = fileman.read(file.path);

          reader.once('readable', reader.pipe.bind(reader, res));
          reader.once('error', next);
        });
      });
    }).catch(next);

  });

};
