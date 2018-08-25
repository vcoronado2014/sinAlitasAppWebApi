'use strict';

const fs = require('fs-extra');
const path = require('path');
const is = require('fi-is');
const uuid = require('node-uuid');
const os = require('os');
const moment = require('moment');

const pdf = component('pdf');

const REPORTS_FOLDER = path.normalize(path.join(os.tmpDir(), 'savina', 'reports'));
const FORMAT_DATE = 'DD-MM-YYYY-HH-mm';

module.exports = (router, db) => {

  var Request = db.model('request');

  /** Get a Consultation Request report in PDF format */
  router.get('/of/:id', (req, res, next) => {
    var canceled = false;

    req.files = req.files || [];

    req.connection.on('close', () => {
      console.log('Request cancelled!');

      canceled = true;

      req.files.forEach((file) => {
        fs.remove(path.dirname(file.path));
      });
    });

    if (canceled) {
      return res.end();
    }

    Request.findOne({
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
        as: 'specialistUser',
        include: [{
          model: db.model('specialty')
        }]
      }, {
        model: db.model('workplace'),
        as: 'specialistWorkplace'
      }, {
        model: db.model('specialty')
      }, {
        model: db.model('requestType')
      }, {
        model: db.model('icd')
      }, {
        model: db.model('priority')
      }, {
        model: db.model('patient')
      }, {
        model: db.model('motive')
      }, {
        model: db.model('requestAnswer'),
        as: 'diagnosticAnswer',
        required: false,
        order: [
          ['createdAt', 'DESC']
        ],
        where: {
          comment: null
        },
        include: [{
          attributes: ['id', 'name', 'specialtyId'],
          model: db.model('user'),
          include: [{
            model: db.model('specialty')
          }]
        }, {
          model: db.model('workplace')
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
      }, {
        model: db.model('requestAnswer'),
        include: [{
          model: db.model('requestAnswerDiagnosis'),
          include: [{
            model: db.model('icd')
          }, {
            model: db.model('requestAnswerDiagnosisExam')
          }, {
            model: db.model('checkupMode')
          }]
        }, {
          attributes: ['id', 'name', 'specialtyId'],
          model: db.model('user'),
          include: [{
            model: db.model('specialty')
          }]
        }, {
          model: db.model('workplace')
        }]
      }, {
        model: db.model('requestAttachment'),
        include: [{
          attributes: ['id', 'name', 'specialtyId'],
          model: db.model('user'),
          include: [{
            model: db.model('specialty')
          }]
        }, {
          model: db.model('workplace')
        }, {
          model: db.model('file')
        }]
      }]
    })

    .then((request) => {
      if (canceled) {
        return res.end();
      }

      if (!request || is.empty(request)) {
        return res.sendStatus(400);
      }

      if ([request.specialistUserId, request.creatorUserId].indexOf(req.session.user.id) < 0) {
        return res.sendStatus(403);
      }

      request = request.get();

      request.host = `${ req.protocol }://${ req.get('host') }`;

      request.report = {
        date: new Date()
      };

      var filename = `request-${ request.requestType.slug }-${ request.sid }-report-${ moment(request.report.date).format(FORMAT_DATE) }.pdf`;

      request.report.output = path.join(REPORTS_FOLDER, uuid.v4(), filename);

      /* Queue for Fileman cleaning */
      req.files.push({
        path: path.dirname(request.report.output)
      });

      console.time('report');

      if (canceled) {
        return res.end();
      }

      return pdf.create(request).then((request) => {
        if (canceled) {
          return res.end();
        }

        fs.stat(request.report.output, (err, stats) => {
          if (canceled) {
            return res.end();
          }

          if (err) {
            return next(err);
          }

          console.timeEnd('report');

          res.set({
            'Content-Disposition': 'filename=' + path.basename(request.report.output),
            'Last-Modified': request.report.date,
            'Content-Type': 'application/pdf',
            'Content-Length': stats.size,
            'ETag': uuid.v4()
          });

          var rs = fs.createReadStream(request.report.output);

          rs.once('error', next);

          rs.pipe(res);
        });
      });
    })

    .catch(next);

  });

};
