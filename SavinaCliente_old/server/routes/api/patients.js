'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Patient = db.model('patient');

  /**
   * Get patient's data.
   */
  router.get('/by/run/:run', function (req, res, next) {

    Patient.findOne({
      where: {
        run: req.params.run
      },
      include: [{
        model: db.model('gender')
      }]
    }).then(function (patient) {
      if (!patient || is.empty(patient)) {
        return res.status(204).send({});
      }

      res.send(patient);
    }).catch(next);

  });

  /**
   * Get patient's information.
   */
  router.get('/info/by/run/:run', function (req, res) {

    /* TODO: implement the integration API */
    res.status(204).send({});

  });

};
