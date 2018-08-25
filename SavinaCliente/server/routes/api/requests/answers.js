'use strict';

var is = require('fi-is');

module.exports = function (router, db) {

  var Answer = db.model('requestAnswer');
  var Request = db.model('request');

  router.all([
    '/', '/of/:id', '/seen'
  ], function (req, res, next) {
    Request.count({
      where: {
        id: Number(req.params.id || req.body.requestId),
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
   * Create a new Answer.
   */
  router.post('/', function (req, res, next) {

    var options = {};

    var query = {
      workplaceId: req.session.workplace.id,
      requestId: Number(req.body.requestId),
      userId: req.session.user.id,
      comment: req.body.comment
    };

    if (req.session.user.specialty) {
      if (!req.body.diagnosis) {
        return res.sendStatus(412);
      }

      delete query.comment;

      options.include = [{
        model: db.model('requestAnswerDiagnosis'),
        include: [{
          model: db.model('requestAnswerDiagnosisExam')
        }]
      }];

      query.requestAnswerDiagnosis = {
        therapeuticPlan: req.body.diagnosis.therapeuticPlan,
        description: req.body.diagnosis.description,
        evaluation: req.body.diagnosis.evaluation,
        confirmed: req.body.diagnosis.confirmed,

        checkupSameSpecialist: req.body.diagnosis.checkupSameSpecialist,
        checkupExamsRequired: req.body.diagnosis.checkupExamsRequired,
        checkupComment: req.body.diagnosis.checkupComment,
        checkupModeId: req.body.diagnosis.checkupModeId,
        checkupDate: req.body.diagnosis.checkupDate,

        requestAnswerDiagnosisExams: req.body.diagnosis.exams
      };
    }

    /* Create the answer */
    Answer.create(query, options).then(function success(answer) {
      if (!answer || is.empty(answer)) {
        return res.sendStatus(400);
      }

      if (req.session.user.specialty && req.body.diagnosis) {
        if (is.array(req.body.diagnosis.icds)) {
          req.body.diagnosis.icds.forEach(function (icdId, index) {
            req.body.diagnosis.icds[index] = Number(icdId);
          });

          return answer.requestAnswerDiagnosis.setIcds(req.body.diagnosis.icds);
        }
      }

      return null;
    }).then(function success() {
      res.sendStatus(201);
    }).catch(next);

  });

  /**
   * Get all Answers of a Request.
   */
  router.get('/of/:id', function (req, res, next) {

    Answer.findAll({
      where: {
        requestId: Number(req.params.id)
      },
      include: [{
        attributes: ['id', 'name', 'specialtyId'],
        model: db.model('user'),
        include: [{
          model: db.model('specialty')
        }]
      }, {
        model: db.model('requestAnswerDiagnosis'),
        include: [{
          model: db.model('requestAnswerDiagnosisExam')
        }, {
          model: db.model('checkupMode')
        }, {
          model: db.model('icd')
        }]
      }]
    }).then(function success(answers) {
      if (!answers || is.empty(answers)) {
        return res.status(204).send([]);
      }

      res.send(answers);
    }).catch(next);

  });

  /**
   * Mark an answer as seen.
   */
  router.put('/seen', function (req, res, next) {

    Answer.findOne({
      where: {
        id: Number(req.body.id),
        userId: {
          $not: req.session.user.id
        }
      }
    }).then(function success(answer) {
      if (!answer || is.empty(answer)) {
        return res.sendStatus(400);
      }

      answer.set('seenAt', new Date());

      return answer.save().then(function success(answer) {
        res.send({
          seenAt: answer.seenAt
        });
      });
    }).catch(next);

  });

};
