'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Request = db.model('request');
  var Patient = db.model('patient');
  var rayen = component('rayen');

  /**
   * Create a new Request.
   */
  router.post('/', function (req, res, next) {

    /* Start by creating or updating the patient */
    Patient.findOne({
      where: {
        run: req.body.patient.run
      }
    }).then(function success(patient) {
      if (patient) {
        return patient.update(req.body.patient);
      }

      return Patient.create(req.body.patient);
    }).then(function (patient) {
      return Request.create({
        creatorWorkplaceId: req.session.workplace.id,
        requestTypeId: req.body.requestTypeId,
        creatorUserId: req.session.user.id,
        specialtyId: req.body.specialtyId,
        hypothesis: req.body.hypothesis,
        priorityId: req.body.priorityId,
        motiveId: req.body.motiveId,
        patient: req.body.patient,
        comment: req.body.comment,
        patientId: patient.id
      }).then(function success(request) {
        if (!request || is.empty(request)) {
          return res.sendStatus(400);
        }
        return request.setIcds(req.body.icds).then(function success() {
          res.status(201).send({
            id: request.id
          });
        });
      });
    }).catch(next);

  });

  /**
   * Assign a Request by it's id to the current user if it is a specialist.
   */
  router.put('/take', function (req, res, next) {

    /* Check if the user is really a specialist */
    if (!req.session.user.specialty) {
      return res.sendStatus(403);
    }

    /* Assign the specialist */
    Request.findById(Number(req.body.id)).then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      if (request.specialistWorkplaceId || request.specialistUserId) {
        return res.sendStatus(409);
      }

      if (request.specialtyId !== req.session.user.specialty.id) {
        return res.sendStatus(412);
      }

      request.set('specialistWorkplaceId', req.session.workplace.id);
      request.set('specialistUserId', req.session.user.id);
      return request.save().then(function success() {
        res.sendStatus(204);
        if (is.number(request.rayenSicId)) {
          rayen.notifyInProgress(request.rayenSicId);
        }
      });
    }).catch(next);

  });

  /**
   * Close a current user's Request by its id.
   */
  router.put('/close', function (req, res, next) {

    Request.findOne({
      where: {
        creatorUserId: req.session.user.id,
        id: Number(req.body.id)
      }
    }).then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }
      request.set('closedAt', new Date());
      return request.save().then(function () {
        res.sendStatus(204);
        if (is.number(request.rayenSicId)) {
          rayen.notifyClose(request.rayenSicId);
        }
      });
    }).catch(next);
  });

  /**
   * Return a current user's Request to the worktable by its id.
   */
  router.put('/return', function (req, res, next) {

    Request.findOne({
      where: {
        creatorUserId: req.session.user.id,
        id: Number(req.body.id)
      }
    }).then(function (request) {
      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      request.set('specialistWorkplaceId', null);
      request.set('specialistUserId', null);

      return request.save().then(function success() {
        res.sendStatus(204);
      });
    }).catch(next);

  });

  /**
   * Get a Request's brief.
   */
  router.get('/brief/:id', function (req, res, next) {

    Request.findOne({
      attributes: ['id', 'requestTypeId', 'specialtyId'],
      where: {
        id: Number(req.params.id)
      },
      include: [{
        model: db.model('requestType')
      }, {
        model: db.model('specialty')
      }]
    }).then(function (request) {
      if (!request || is.empty(request)) {
        return res.status(204).send({});
      }

      res.send(request);
    }).catch(next);

  });

  /**
   * Get a single current user's Request by its id.
   */
  router.get('/by/id/:id', function (req, res, next) {

    Request.scope('diagnosticAnswer').findOne({
      where: {
        id: Number(req.params.id),
        $or: [{
          creatorUserId: req.session.user.id
        }, {
          specialistUserId: req.session.user.id
        }]
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
