'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Link = db.model('link');

  /**
   * Checks if a link exists and has not expired.
   */
  router.get('/validate/:hash', function (req, res, next) {

    Link.findOne({
      where: {
        hash: req.params.hash
      }
    }).then(function success(link) {
      if (!link || is.empty(link)) {
        return res.sendStatus(400);
      }

      if (link.hasExpired()) {
        return res.sendStatus(412);
      }

      res.sendStatus(204);
    }).catch(next);

  });

  /**
   * Obtains a link's public data.
   */
  router.get('/:hash', function (req, res, next) {

    Link.findOne({
      where: {
        hash: req.params.hash
      },
      include: [{
        attributes: ['creatorWorkplaceId', 'creatorUserId', 'specialistUserId', 'specialistWorkplaceId'],
        model: db.model('request')
      }]
    }).then(function success(link) {
      if (!link || is.empty(link)) {
        return res.sendStatus(400);
      }

      if (!link.request || is.empty(link.request)) {
        return res.sendStatus(409);
      }

      var response = {
        url: link.url
      };

      if (req.session.user.id === link.request.creatorUserId) {
        response.workplaceId = link.request.creatorWorkplaceId;
      } else if (req.session.user.id === link.request.specialistUserId) {
        response.workplaceId = link.request.specialistWorkplaceId;
      } else {
        return res.sendStatus(403);
      }

      res.send(response);
    }).catch(next);

  });

};
