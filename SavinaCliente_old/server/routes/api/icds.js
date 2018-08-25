'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Icd = db.model('icd');

  /**
   * Filter ICD static data (typeahead).
   */
  router.get('/', function (req, res, next) {

    if (is.not.string(req.query.filter)) {
      return res.sendStatus(400);
    }

    var query = {
      limit: Number(req.query.limit) || 10,
      where: {
        $or: [{
          code: {
            $like: '%' + req.query.filter + '%'
          }
        }, {
          description: {
            $like: '%' + req.query.filter + '%'
          }
        }]
      }
    };

    if (is.string(req.query.not) || is.number(req.query.not)) {
      query.where.id = {
        $not: Number(req.query.not)
      };
    } else if (is.array(req.query.not)) {
      req.query.not.forEach(function (num, idx) {
        req.query.not[idx] = Number(num);
      });

      query.where.id = {
        $notIn: req.query.not
      };
    }

    Icd.findAll(query).then(function success(icds) {
      if (!icds || is.empty(icds)) {
        return res.status(204).send([]);
      }

      res.send(icds);
    }).catch(next);

  });

};
