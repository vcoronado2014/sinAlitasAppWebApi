'use strict';

var slug = require('slug');
var is = require('fi-is');

module.exports = function (router, db) {

  var RequestType = db.model('requestType');
  var Request = db.model('request');

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
   * Get all current user's sent Requests.
   */
  router.get('/:type', function (req, res, next) {

    if (req.session.user.specialtyId) {
      return res.sendStatus(403);
    }

    var query = {
      where: {
        creatorWorkplaceId: req.session.workplace.id,
        creatorUserId: req.session.user.id,
        requestTypeId: req.body.type.id,
        specialistWorkplaceId: null,
        specialistUserId: null,
        closedAt: null
      },
      include: [{
        model: db.model('workplace'),
        as: 'creatorWorkplace'
      }, {
        attributes: ['id', 'name'],
        model: db.model('user'),
        as: 'creatorUser'
      }, {
        model: db.model('icd')
      }, {
        model: db.model('requestType')
      }, {
        model: db.model('specialty'),
        required: true
      }, {
        model: db.model('patient'),
        required: true
      }, {
        model: db.model('motive'),
        required: true
      }, {
        model: db.model('priority'),
        required: true
      }],

      limit: 50
    };

    if (is.string(req.query.keywords)) {
      req.query.keywords = slug(req.query.keywords, {
        replacement: ' ',
        lower: true
      });

      var $keywords = {
        $or: [{
          $like: '%' + req.query.keywords + '%'
        }]
      };

      var words = req.query.keywords.split(/[\W\s]/g);

      if (words.length > 1) {
        query.where.$or = [];

        words.forEach(function (word) {
          $keywords.$or.push({
            $like: '%' + word + '%'
          });
        });
      }

      query.where.$or = [{
        'hypothesis': $keywords
      }, {
        'comment': $keywords
      }, {
        '$motive.name$': $keywords
      }, {
        '$priority.name$': $keywords
      }, {
        '$patient.firstname$': $keywords
      }, {
        '$patient.lastname$': $keywords
      }, {
        '$specialty.name$': $keywords
      }];
    }

    /* Sort by parameter */
    if (is.string(req.query.order)) {
      if (req.query.order.charAt(0) === '-') {
        query.order = [
          [req.query.order.substr(1), 'ASC']
        ];
      } else {
        query.order = [
          [req.query.order, 'DESC']
        ];
      }
    }

    /* Filter by date range */
    if (is.date(req.query.dateMin)) {
      query.where.createdAt = {
        $gte: req.query.dateMin
      };
    }

    if (is.date(req.query.dateMax)) {
      query.where.createdAt = {
        $lte: req.query.dateMax
      };
    }

    Request.findAll(query).then(function success(requests) {
      if (!requests || is.empty(requests)) {
        return res.status(204).send([]);
      }

      res.send(requests);
    }).catch(next);

  });

};
