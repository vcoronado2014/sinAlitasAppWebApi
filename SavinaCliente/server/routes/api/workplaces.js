'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  // var Agreement = db.model('agreement');
  var Workplace = db.model('workplace');

  /**
   * List all workplaces for the current user.
   */
  router.get('/own', function own(req, res, next) {

    Workplace.findAll({
      attributes: ['id', 'name'],
      include: [{
        model: db.model('user'),
        attributes: ['id'],
        required: true,
        where: {
          id: req.session.user.id
        }
      }]
    }).then(function success(workplaces) {
      if (!workplaces || is.empty(workplaces)) {
        return res.status(204).send([]);
      }

      res.send(workplaces);
    }).catch(next);

  });

  /**
   * Set current workplace.
   */
  router.put('/active', function set(req, res, next) {

    Workplace.findOne({
      where: {
        id: Number(req.body.id),
      },
      include: [{
        model: db.model('user'),
        attributes: ['id'],
        required: true,
        where: {
          id: req.session.user.id
        }
      }]
    }).then(function (workplace) {
      if (!workplace || is.empty(workplace)) {
        return res.sendStatus(400);
      }

      req.session.workplace = workplace.get();

      /* Find the current workplace counterparts and save it to the session */
      return Workplace.getCounterparts(req.session.workplace.id).then(function (counterparts) {
        req.session.workplace.counterparts = counterparts;
        res.sendStatus(204);
      });
    }).catch(next);

  });

};
