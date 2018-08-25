'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Workplace = db.model('workplace');
  var Request = db.model('request');
  var Patient = db.model('patient');
  var ICD = db.model('icd');
  var rayen = component('rayen');

  function validatePatient(patient) {
    var required = [
      'firstname',
      'lastname',
      'birth',
      'city',
      'run'
    ]

    var isValid = !required.filter((key) => {
      return !!patient[key] === false;
    }).length;

    return isValid;
  }

  function validateBody(body) {
    var required = [
      'workplaceDeis',
      'requestTypeId',
      'creatorUserId',
      'specialtyId',
      'hypothesis',
      'priorityId',
      'motiveId',
      'comment',
      'rayenSicId'
    ]

    var bodyIsValid = !required.filter((key) => {
      return !!body[key] === false;
    }).length;

    var patientIsValid = validatePatient(body.patient);

    return bodyIsValid && patientIsValid;
  }

  /**
   * Create a new Request.
   */
  router.post('/', function (req, res, next) {
    if (!validateBody(req.body)) {
      return res.sendStatus(400);
    }

    var where = {
      where: {
        run: req.body.patient.run
      }
    };

    Workplace.findByDeis(req.body.workplaceDeis)

    .then(function (workplace) {
      if (!workplace || is.empty(workplace)) {
        return res.sendStatus(400);
      }

      /* Start by creating or updating the patient */
      return Patient.createOrUpdate(where, req.body.patient)

      .then(function (patient) {

        return Request.create({
          requestTypeId: req.body.requestTypeId,
          creatorUserId: req.body.creatorUserId,
          specialtyId: req.body.specialtyId,
          priorityId: req.body.priorityId,
          motiveId: req.body.motiveId,
          creatorWorkplaceId: workplace.id,
          patientId: patient.id,
          hypothesis: req.body.hypothesis,
          patient: req.body.patient,
          comment: req.body.comment,
          rayenSicId: req.body.rayenSicId
        })

        .then(function success(request) {
          if (!request || is.empty(request)) {
            return res.sendStatus(400);
          }

          where.where = {
            code: {
              $in: req.body.icds || []
            }
          };

          return ICD.findAll(where)

          .then(function (icds) {

            icds = icds.map(function (icd) {
              return icd.id;
            });

            return request.setIcds(icds)

            .then(function success() {
              res.status(201).send({
                id: request.id
              });
              if (is.number(request.rayenSicId)) {
                rayen.notifyOpen(request.rayenSicId)
              }
            });
          });
        });
      });
    }).catch(function (err) {
      console.log(err);

      if (err.name === 'SequelizeValidationError') {
        return res.sendStatus(400);
      }

      next(err);
    });

  });

  /**
   * Get a single current user's Request by its id.
   */
  router.get('/by/id/:id', function (req, res, next) {

    Request.scope('diagnosticAnswer').findOne({
      where: {
        id: Number(req.params.id)
      },
      include: [{
        attributes: ['id', 'name'],
        model: db.model('user'),
        as: 'creatorUser'
      }, {
        model: db.model('workplace'),
        as: 'creatorWorkplace'
      }, {
        attributes: ['id', 'name'],
        model: db.model('user'),
        as: 'specialistUser'
      }, {
        model: db.model('workplace'),
        as: 'specialistWorkplace'
      }, {
        model: db.model('icd')
      }, {
        model: db.model('requestType')
      }, {
        model: db.model('motive')
      }, {
        model: db.model('priority')
      }, {
        model: db.model('patient')
      }, {
        model: db.model('specialty')
      }, {
        model: db.model('requestAttachment'),
        include: [{
          attributes: ['id', 'name'],
          model: db.model('user')
        }, {
          model: db.model('workplace')
        }, {
          model: db.model('file')
        }]
      }, {
        model: db.model('notification'),
        as: 'closedNotification'
      }, {
        model: db.model('requestAnswer'),
        include: [{
          attributes: ['id', 'name'],
          model: db.model('user'),
          include: [{
            model: db.model('specialty')
          }]
        }, {
          model: db.model('requestAnswerDiagnosis'),
          include: [{
            model: db.model('icd')
          }, {
            model: db.model('checkupMode')
          }, {
            model: db.model('requestAnswerDiagnosisExam')
          }]
        }]
      }]
    }).then(function success(request) {
      if (!request || is.empty(request)) {
        return res.status(204).send({});
      }

      res.send(request);
    }).catch(next);

  });

};
