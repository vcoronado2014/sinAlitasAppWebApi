'use strict';

var moment = require('moment');

var integration = component('integration');

module.exports = function (router) {

  router.get('/patient/auth/:run/:pass', function (req, res, next) {

    integration.auth(req.params.run, req.params.pass, function (err, token, data) {
      if (err) {
        return next(err);
      }

      res.send({
        token: token,
        data: data
      });
    });

  });

  router.get('/patient/:ryfid/:estcode/:token', function (req, res, next) {

    integration.patient(req.params.ryfid, req.params.estcode, req.params.token, function (err, token, data) {
      if (err) {
        return next(err);
      }

      res.send({
        token: token,
        data: data
      });
    });

  });

  router.get('/patient/info/:ryfid/:estcode/:token', function (req, res, next) {

    integration.patientInfo(req.params.ryfid, req.params.estcode, req.params.token, function (err, token, data) {
      if (err) {
        return next(err);
      }

      res.send({
        token: token,
        data: data
      });
    });

  });

  router.get('/patient/history/primary/:run/:ryfid/:estcode', function (req, res, next) {

    integration.patientHistoryPrimary(req.params.run, req.params.ryfid, req.params.estcode, {
      start: moment().subtract(12, 'months'),
      end: moment()
    }, function (err, token, data) {
      if (err) {
        return next(err);
      }

      res.send({
        token: token,
        data: data
      });
    });

  });

};
