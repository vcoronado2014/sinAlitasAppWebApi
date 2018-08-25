'use strict';

const fileman = require('fi-fileman');
const PDFDocument = require('pdfkit');
// const uuid = require('node-uuid');
const moment = require('moment');
const fs = require('fs-extra');
// const Jimp = require('jimp2');
// const path = require('path');
const imagesize = require('image-size');

const rut = component('rut');

const DOC_INFO_CREATOR = 'SAVINA';
const DOC_WIDTH = 612;
const DOC_HEIGHT = 792;
const DOC_MARGIN_LEFT = 30;
const DOC_MARGIN_RIGHT = DOC_MARGIN_LEFT;
const DOC_MARGIN_TOP = 80;
const DOC_MARGIN_BOTTOM = 100;
const DOC_INNER_WIDTH = DOC_WIDTH - DOC_MARGIN_LEFT - DOC_MARGIN_RIGHT;

const OPTS_DOC = {
  bufferPages: true,

  size: [DOC_WIDTH, DOC_HEIGHT], // letter

  margins: {
    bottom: DOC_MARGIN_BOTTOM,
    right: DOC_MARGIN_RIGHT,
    left: DOC_MARGIN_LEFT,
    top: DOC_MARGIN_TOP
  }
};

// const HEADER_LEFT = DOC_MARGIN_LEFT;
const HEADER_TOP = DOC_MARGIN_LEFT;
const HEADER_TITLE_PARAMS = {
  width: DOC_INNER_WIDTH,
  height: 2,
  align: 'left'
};

const HEADER_DATE_FORMAT = 'LLL';
const HEADER_DATE_PARAMS = {
  width: DOC_INNER_WIDTH,
  height: 2,
  align: 'right'
};

// const FOOTER_LEFT = DOC_MARGIN_LEFT;
const FOOTER_TOP = DOC_HEIGHT - DOC_MARGIN_TOP + DOC_MARGIN_LEFT;
const FOOTER_PARAMS = {
  width: DOC_INNER_WIDTH,
  align: 'center',
  height: 2
};

const HEADER_FOOTER_COLOR = '#aaa';

const HEADER_STROKE_TOP = DOC_MARGIN_TOP - HEADER_TOP;
// const FOOTER_STROKE_TOP = FOOTER_TOP - DOC_MARGIN_LEFT;
const HEADER_FOOTER_STROKE_RIGHT = OPTS_DOC.size[0] - DOC_MARGIN_RIGHT;

const DEFAULT_FONT_FAMILY = 'Helvetica';
const DEFAULT_FONT_COLOR = 'black';
const DEFAULT_FONT_SIZE = 12;

const HEADING_FONT_FAMILY = 'Helvetica-Bold';
const HEADING_FONT_SIZE = 20;
const HEADING_FONT_COLOR = 'gray';
const HEADING_PARAMS = {
  width: DOC_INNER_WIDTH,
  lineGap: 10,
  height: 1
};

const PREFIX_TITLE = 'Reporte solicitud de';
const KEYWORDS_DEFAULT = 'Savina Reporte Solicitud';

const HEADINGS = {
  DETAILS: 'DETALLES DE LA SOLICITUD',
  REPORT: 'INFORME DE TELECONSULTA',
  HYPOTHESIS: 'HIPÓTESIS DIAGNÓSTICA',
  DIAGNOSIS: 'DIAGNÓSTICO CONFIRMADO',
  PLAN: 'INFORME / PLAN TERAPÉUTICO',
  EXAMS: 'EXÁMENES SOLICITADOS',
  CHECKUP: 'CONTROL',
  ANSWERS: 'RESPUESTAS',
  ATTACHMENTS: 'ADJUNTOS'
};

const DETAILS = {
  NAME: 'Nombre del paciente',
  RUT: 'RUT',
  AGE: 'Edad',
  CITY: 'Cuidad del paciente',
  WORKPLACE: 'Centro de derivación',
  CREATOR: 'Derivado por',
  CREATED: 'Fecha de derivación',
  CLOSED: 'Fecha de cierre'
};

const REPORT = {
  SPECIALTY: 'Especialidad',
  PRIORITY: 'Prioridad',
  MOTIVE: 'Motivo de consulta',
  COMMENT: 'Motivo'
};

const ICDS = {
  DESCRIPTION: 'Descripción',
  CODE: 'Código'
};

const EXAMS = {
  EMPTY: 'No incluye exámenes.',
  NAME: 'Nombre',
  PLACE: 'Lugar',
  SAME_PLACE: 'En el mismo establecimiento',
  OTHER_PLACE: 'En otro establecimiento'
};

const PROP_ELLIPSIS = '..:';

const PARAMS_CONTINUED = {
  continued: true
};

const DETAILS_PROP_WIDTH = DOC_INNER_WIDTH * 0.3;
const DETAILS_PROP_PARAMS = {
  width: DETAILS_PROP_WIDTH,
  ellipsis: PROP_ELLIPSIS,
  height: 1
};

const DETAILS_VALUE_WIDTH = DOC_INNER_WIDTH - DETAILS_PROP_WIDTH;
const DETAILS_VALUE_SPAN = DOC_INNER_WIDTH * 0.05;
const DETAILS_VALUE_PARAMS = {
  width: DETAILS_VALUE_WIDTH - DETAILS_VALUE_SPAN
};

const DETAILS_VALUE_LEFT = DOC_MARGIN_LEFT + DETAILS_PROP_WIDTH + DETAILS_VALUE_SPAN;

const ICDS_CODE_COL_WIDTH = DOC_INNER_WIDTH * 0.15;
const ICDS_CODE_COL_PARAMS = {
  width: ICDS_CODE_COL_WIDTH,
  ellipsis: PROP_ELLIPSIS,
  height: 1
};

const ICDS_DESC_COL_WIDTH = DOC_INNER_WIDTH - ICDS_CODE_COL_WIDTH;
const ICDS_DESC_COL_SPAN = DOC_INNER_WIDTH * 0.05;
const ICDS_DESC_COL_PARAMS = {
  width: ICDS_DESC_COL_WIDTH - ICDS_DESC_COL_SPAN
};

const ICDS_DESC_COL_LEFT = DOC_MARGIN_LEFT + ICDS_CODE_COL_WIDTH + ICDS_DESC_COL_SPAN;

const EXAMS_NAME_COL_WIDTH = DOC_INNER_WIDTH * 0.5;
const EXAMS_NAME_COL_PARAMS = {
  width: EXAMS_NAME_COL_WIDTH
};

const EXAMS_PLACE_COL_WIDTH = DOC_INNER_WIDTH - EXAMS_NAME_COL_WIDTH;
const EXAMS_PLACE_COL_SPAN = DOC_INNER_WIDTH * 0.05;
const EXAMS_PLACE_COL_PARAMS = {
  width: EXAMS_PLACE_COL_WIDTH - EXAMS_PLACE_COL_SPAN
};

const EXAMS_PLACE_COL_LEFT = DOC_MARGIN_LEFT + EXAMS_NAME_COL_WIDTH + EXAMS_PLACE_COL_SPAN;

const IMAGES_PER_ROW = 3;
const IMAGES_SPAN = (DOC_INNER_WIDTH * 0.015);
const IMAGES_SIZE = (DOC_INNER_WIDTH / IMAGES_PER_ROW) - IMAGES_SPAN;
const IMAGES_SEPARATION = (DOC_INNER_WIDTH / IMAGES_PER_ROW) + (IMAGES_SPAN / (IMAGES_PER_ROW - 1));
// const IMAGES_SIZE_PIXELS = 256;

const DIAGNOSIS = {
  NO_PLAN: 'No incluye.'
};

const ATTACHMENTS = {
  NO_FILES: 'No incluye archivos.',
  EMPTY: 'No incluye adjuntos.'
};

const ANSWERS = {
  EMPTY: 'No incluye respuestas.'
};

const FORMAT_DATE_HUMAN = 'dddd, D [de] MMMM [de] YYYY';
const FORMAT_DATE_LLLL = 'LLLL';

const COMMA = ', ';
const COLON = '.';
const EMPTY = '';

moment.locale('es_CL');

/**
 * Savina Report class.
 *
 * @param {PDFDocument} doc The associated PDF Document object.
 * @param {Object} request The request data.
 *
 * @constructor
 */
function SavinaReport(doc, request) {
  this.request = request;
  this.doc = doc;
}

/**
 * Adds a heading element.
 *
 * @param {String} text The header's text.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addHeading = function addHeading(text) {
  var doc = this.doc;

  return doc.font(HEADING_FONT_FAMILY).fontSize(HEADING_FONT_SIZE)
    .fillColor(HEADING_FONT_COLOR)
    .text(text, HEADING_PARAMS)
    .fillColor(DEFAULT_FONT_COLOR)
    .font(DEFAULT_FONT_FAMILY)
    .fontSize(DEFAULT_FONT_SIZE);
};

/**
 * Adds a detail list element.
 *
 * @param {String} name The detail's name.
 * @param {String} value The detail's value.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addDetail = function addDetail(name, value) {
  var doc = this.doc;

  return doc.font(HEADING_FONT_FAMILY).text(`${ name }:`, DETAILS_PROP_PARAMS)
    .moveUp().font(DEFAULT_FONT_FAMILY)
    .text(value, DETAILS_VALUE_LEFT, doc.y, DETAILS_VALUE_PARAMS)
    .text(EMPTY, DOC_MARGIN_LEFT, doc.y)
    .moveDown();
};

/**
 * Adds an ICD's list heading.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addICDHeading = function addICDHeading() {
  var doc = this.doc;

  return doc.font(HEADING_FONT_FAMILY).text(ICDS.CODE, ICDS_CODE_COL_PARAMS)
    .moveUp()
    .text(ICDS.DESCRIPTION, ICDS_DESC_COL_LEFT, doc.y, ICDS_DESC_COL_PARAMS)
    .text(EMPTY, DOC_MARGIN_LEFT, doc.y);
};

/**
 * Adds an ICD list item.
 *
 * @param {String} code The ICD's code.
 * @param {String} description The ICD's description.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addICDDetails = function addICDDetails(code, description) {
  var doc = this.doc;

  return doc.font(DEFAULT_FONT_FAMILY).text(code, ICDS_CODE_COL_PARAMS)
    .moveUp()
    .text(description, ICDS_DESC_COL_LEFT, doc.y, ICDS_DESC_COL_PARAMS)
    .text(EMPTY, DOC_MARGIN_LEFT, doc.y);
};

/**
 * Adds an exams list heading.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addExamHeading = function addExamHeading() {
  var doc = this.doc;

  return doc.font(HEADING_FONT_FAMILY).text(EXAMS.NAME, EXAMS_NAME_COL_PARAMS)
    .moveUp()
    .text(EXAMS.PLACE, EXAMS_PLACE_COL_LEFT, doc.y, EXAMS_PLACE_COL_PARAMS)
    .text(EMPTY, DOC_MARGIN_LEFT, doc.y);
};

/**
 * Adds an exams list item.
 *
 * @param {String} code The ICD's code.
 * @param {String} description The ICD's description.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addExamDetails = function addExamDetails(name, samePlace) {
  var doc = this.doc;

  return doc.font(DEFAULT_FONT_FAMILY).text(name, EXAMS_NAME_COL_PARAMS)
    .moveUp()
    .text(samePlace ? EXAMS.SAME_PLACE : EXAMS.OTHER_PLACE, EXAMS_PLACE_COL_LEFT, doc.y, EXAMS_PLACE_COL_PARAMS)
    .text(EMPTY, DOC_MARGIN_LEFT, doc.y);
};

/**
 * Adds an attachment or answer header.
 *
 * @param {String} data The answer or attachment's data.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addAnswerAttachmentHeader = function addAnswerAttachmentHeader(data) {
  var doc = this.doc;

  doc.font(HEADING_FONT_FAMILY).text(data.user.name, DOC_MARGIN_LEFT, doc.y, {
    width: DOC_INNER_WIDTH * 0.6,
    continued: true
  });

  doc.font(DEFAULT_FONT_FAMILY).text(` - ${ data.user.specialtyId ? 'Especialista' : 'Médico general' }`);

  doc.moveUp().text(moment(data.createdAt).format(FORMAT_DATE_LLLL), ((DOC_INNER_WIDTH * 0.6) + DOC_MARGIN_LEFT), doc.y, {
    width: DOC_INNER_WIDTH * 0.4,
    align: 'right'
  });

  doc.moveTo(DOC_MARGIN_LEFT, doc.y).lineTo(DOC_INNER_WIDTH + DOC_MARGIN_LEFT, doc.y)
    .stroke().moveDown().moveUp(0.5);
};

/**
 * Adds the page header and footer.
 *
 * @param {Object} title The header's title.
 * @param {Object} date The header's date.
 * @param {Object} footer The footer's text.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addHeaderAndFooter = function addHeaderAndFooter(title, date, footer) {
  var doc = this.doc;

  return doc.font(DEFAULT_FONT_FAMILY)
    .fontSize(DEFAULT_FONT_SIZE).fillColor(HEADER_FOOTER_COLOR)
    .text(title, DOC_MARGIN_LEFT, HEADER_TOP, HEADER_TITLE_PARAMS)
    .text(date, DOC_MARGIN_LEFT, HEADER_TOP, HEADER_DATE_PARAMS)
    .text(footer, DOC_MARGIN_LEFT, FOOTER_TOP, FOOTER_PARAMS);
};

/**
 * Adds the page header and footer divider lines.
 *
 * @returns {PDFDocument}
 */
SavinaReport.prototype.addHeaderAndFooterDividers = function addHeaderAndFooterDividers() {
  var doc = this.doc;

  return doc.moveTo(DOC_MARGIN_LEFT, HEADER_STROKE_TOP)
    .strokeColor(HEADER_FOOTER_COLOR)
    .lineTo(HEADER_FOOTER_STROKE_RIGHT, HEADER_STROKE_TOP)
    .moveTo(DOC_MARGIN_LEFT, DOC_HEIGHT - DOC_MARGIN_BOTTOM)
    .lineTo(HEADER_FOOTER_STROKE_RIGHT, DOC_HEIGHT - DOC_MARGIN_BOTTOM)
    .stroke();
};

/**
 * Defines the document's metadata.
 */
SavinaReport.prototype.defineMetadata = function defineMetadata() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    doc.info.Title = `${ PREFIX_TITLE } ${ request.requestType.name } #${ request.sid }`;
    doc.info.Keywords = `${ KEYWORDS_DEFAULT } ${ request.requestType.name }`;
    doc.info.CreationDate = request.report.date;
    doc.info.Author = request.creatorUser.name;
    doc.info.ModDate = request.report.date;
    doc.info.Subject = doc.info.Title;
    doc.info.Creator = DOC_INFO_CREATOR;

    resolve();
  });
};

/**
 * Writes the document's details section.
 */
SavinaReport.prototype.writeDetails = function writeDetails() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    this.addHeading(HEADINGS.DETAILS);

    this.addDetail(DETAILS.NAME, `${ request.patient.firstname} ${ request.patient.lastname }`);
    this.addDetail(DETAILS.RUT, rut.format(request.patient.run));
    //odificado por coro
    //this.addDetail(DETAILS.AGE, moment(request.patient.birth).fromNow(true));
    this.addDetail(DETAILS.AGE, moment().diff(moment(request.patient.birth), 'years') + ' años');
    this.addDetail(DETAILS.CITY, request.patient.city);
    this.addDetail(DETAILS.WORKPLACE, request.creatorWorkplace.name);
    this.addDetail(DETAILS.CREATOR, request.creatorUser.name);
    this.addDetail(DETAILS.CREATED, moment(request.createdAt).format(FORMAT_DATE_LLLL));
    this.addDetail(DETAILS.CLOSED, moment(request.closedAt).format(FORMAT_DATE_LLLL));

    doc.moveDown();

    resolve();
  });
};

/**
 * Writes the document's report section.
 */
SavinaReport.prototype.writeReport = function writeReport() {
  return new Promise((resolve) => {
    var request = this.request;

    this.addHeading(HEADINGS.REPORT);

    this.addDetail(REPORT.SPECIALTY, request.specialty.name);
    this.addDetail(REPORT.PRIORITY, request.priority.name);
    this.addDetail(REPORT.MOTIVE, request.motive.name);
    this.addDetail(REPORT.COMMENT, request.comment);

    resolve();
  });
};

/**
 * Writes the document's hypothesis section.
 */
SavinaReport.prototype.writeHypothesis = function writeHypothesis() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    doc.addPage();

    this.addHeading(HEADINGS.HYPOTHESIS);

    doc.text(request.hypothesis).moveDown();

    this.addICDHeading();

    for (let i = 0, l = request.icds.length; i < l; i++) {
      let icd = request.icds[i];

      this.addICDDetails(icd.code, icd.description);
    }

    doc.moveDown().moveDown();

    resolve();
  });
};

/**
 * Writes the document's diagnosis therapeutic plan section.
 */
SavinaReport.prototype.writeDiagnosisTherapeuticPlan = function writeDiagnosisTherapeuticPlan() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    var diagnosis = request.diagnosticAnswer && request.diagnosticAnswer.requestAnswerDiagnosis;

    this.addHeading(HEADINGS.PLAN);

    if (diagnosis) {
      if (diagnosis.therapeuticPlan) {
        doc.text(diagnosis.therapeuticPlan);
      } else {
        doc.text(DIAGNOSIS.NO_PLAN);
      }

      doc.moveDown();
    } else {
      doc.text('No especifica plan terapéutico.').moveDown().moveDown();
    }

    resolve();
  });
};

/**
 * Writes the document's diagnosis exams section.
 */
SavinaReport.prototype.writeDiagnosisExams = function writeDiagnosisExams() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    doc.addPage();

    this.addHeading(HEADINGS.EXAMS);

    var diagnosis = request.diagnosticAnswer && request.diagnosticAnswer.requestAnswerDiagnosis;
    var diagnosisExams = diagnosis && diagnosis.requestAnswerDiagnosisExams || [];

    if (diagnosisExams.length) {
      this.addExamHeading(doc);

      for (let i = 0, l = diagnosisExams.length; i < l; i++) {
        let exam = diagnosisExams[i];

        this.addExamDetails(exam.name, exam.samePlace);
      }

      doc.moveDown();
    } else {
      doc.text('No especifica exámenes.').moveDown();
    }

    doc.moveDown();

    resolve();
  });
};

/**
 * Writes the document's diagnosis control section.
 */
SavinaReport.prototype.writeDiagnosisControl = function writeDiagnosisControl() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    this.addHeading(HEADINGS.CHECKUP);

    var diagnosis = request.diagnosticAnswer && request.diagnosticAnswer.requestAnswerDiagnosis;

    if (diagnosis && diagnosis.checkupDate) {
      doc.text(diagnosis.checkupComment).moveDown();

      var list = [];

      if (diagnosis.checkupExamsRequired) {
        list.push('Paciente debe venir con los examen realizados.');
      } else {
        list.push('Paciente puede venir sin los exámenes.');
      }

      if (diagnosis.checkupSameSpecialist) {
        list.push(`Control con el Dr. ${ request.diagnosticAnswer.user.name }`);
      } else {
        list.push('Control con otro profesional.');
      }

      list.push(`Tipo de Control: ${ diagnosis.checkupMode.name }`);

      list.push(`Fecha de control: ${ moment(diagnosis.checkupDate).format(FORMAT_DATE_HUMAN) }`);

      doc.list(list).moveDown();

      doc.moveTo(DOC_MARGIN_LEFT, doc.y).lineTo(DOC_INNER_WIDTH + DOC_MARGIN_LEFT, doc.y)
        .stroke().moveDown();

      this.addDetail('Firma', request.diagnosticAnswer.user.name);
      this.addDetail('Fecha', moment(request.report.date).format(FORMAT_DATE_HUMAN));
    } else {
      doc.text('No especifica control.').moveDown();
    }

    resolve();
  });
};

/**
 * Writes the document's diagnosis section.
 */
SavinaReport.prototype.writeDiagnosis = function writeDiagnosis() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    this.addHeading(HEADINGS.DIAGNOSIS);

    var diagnosis = request.diagnosticAnswer && request.diagnosticAnswer.requestAnswerDiagnosis;

    if (diagnosis) {
      if (diagnosis.confirmed) {
        doc.text('Confirma la hipótesis diagnóstica: ', PARAMS_CONTINUED);
      }

      doc.text(diagnosis.description).moveDown();

      var diagnosisIcds = diagnosis.icds || [];

      if (diagnosisIcds.length) {
        this.addICDHeading();

        for (let i = 0, l = diagnosisIcds.length; i < l; i++) {
          let icd = diagnosisIcds[i];

          this.addICDDetails(icd.code, icd.description);
        }

        doc.moveDown();
      }

      doc.moveDown();
    } else {
      doc.text('No especifica diagnósitco confirmado.').moveDown().moveDown();
    }

    resolve();
  });
};

/**
 * Writes the document's answers section.
 */
SavinaReport.prototype.writeAnswers = function writeAnswers() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    doc.addPage();

    this.addHeading(HEADINGS.ANSWERS);

    if (request.requestAnswers && request.requestAnswers.length) {
      var answers = request.requestAnswers.reverse();

      for (let i = 0, l = answers.length; i < l; i++) {
        var answer = answers[i];

        this.addAnswerAttachmentHeader(answer);

        if (request.diagnosticAnswer && answer.requestAnswerDiagnosis && request.diagnosticAnswer.id === answer.id) {
          doc.font(HEADING_FONT_FAMILY);
          doc.text('Diagnóstico confirmado', DOC_MARGIN_LEFT, doc.y);
          doc.moveDown();
        }

        if (answer.requestAnswerDiagnosis) {
          if (answer.requestAnswerDiagnosis.evaluation) {
            doc.font(HEADING_FONT_FAMILY).text('Evaluación: ', PARAMS_CONTINUED)
              .font(DEFAULT_FONT_FAMILY).text(answer.requestAnswerDiagnosis.evaluation)
              .moveDown();
          }
          if (answer.requestAnswerDiagnosis.description) {
            doc.font(HEADING_FONT_FAMILY).text('Descripción: ', PARAMS_CONTINUED)
              .font(DEFAULT_FONT_FAMILY).text(answer.requestAnswerDiagnosis.description)
              .moveDown();
          }

          if (answer.requestAnswerDiagnosis.therapeuticPlan) {
            doc.font(HEADING_FONT_FAMILY).text('Plan terapéutico: ', PARAMS_CONTINUED)
              .font(DEFAULT_FONT_FAMILY).text(answer.requestAnswerDiagnosis.therapeuticPlan)
              .moveDown();
          }
        } else {
          doc.text(answer.comment, DOC_MARGIN_LEFT, doc.y);
        }

        doc.moveDown().moveDown();
      }
    } else {
      doc.text(ANSWERS.EMPTY).moveDown().moveDown();
    }

    resolve();
  });
};

/**
 * Writes an attachment image.
 */
SavinaReport.prototype.writeAttachmentImage = function writeAttachmentImage(image, row, done) {
  var request = this.request;
  var doc = this.doc;

  if (DOC_HEIGHT - DOC_MARGIN_BOTTOM - row.y < IMAGES_SIZE) {
    doc.addPage();

    row.count = 0;
    row.x = DOC_MARGIN_LEFT;
    row.y = doc.y;
  }

  // Jimp.read(fileman.resolve(image.path), (err, img) => {
  //   if (err) {
  //     return done(err);
  //   }

  // image.thumbnail = path.join(path.dirname(request.report.output), `${ uuid.v4() }.jpg`);

  // img.cover(IMAGES_SIZE_PIXELS, IMAGES_SIZE_PIXELS);

  // img.quality(75);

  // img.write(image.thumbnail, (err) => {
  //   if (err) {
  //     return done(err);
  //   }

  var imgPath = fileman.resolve(image.path);
  var imgDims;

  try {
    imgDims = imagesize(imgPath);
  } catch (ex) {
    console.error(ex);
  }

  var linkPosY = row.y + IMAGES_SIZE + 10;
  var imgOffsetX = 0;
  var imgOffsetY = 0;
  var imgParams = {};

  row.x = DOC_MARGIN_LEFT + (IMAGES_SEPARATION * row.count);

  doc.rect(row.x, row.y, IMAGES_SIZE, IMAGES_SIZE).fill('#efefef');

  if (imgDims) {
    if (imgDims.width > imgDims.height) {
      imgOffsetY = ((imgDims.width - imgDims.height) / 2) * (IMAGES_SIZE / imgDims.width);
      imgParams.width = IMAGES_SIZE;
    } else {
      imgOffsetX = ((imgDims.height - imgDims.width) / 2) * (IMAGES_SIZE / imgDims.height);
      imgParams.height = IMAGES_SIZE;
    }

    doc.image(imgPath, row.x + imgOffsetX, row.y + imgOffsetY, imgParams);
  }

  doc.fillColor('blue').text(image.filename, row.x, linkPosY, {
    link: `${ request.host }/api/files/${ image.id }/${ image.filename }`,
    width: IMAGES_SIZE,
    ellipsis: true,
    height: 1
  });

  doc.fillColor('black');

  if (++row.count === IMAGES_PER_ROW) {
    doc.moveDown();

    row.count = 0;
    row.x = DOC_MARGIN_LEFT;
    row.y = doc.y;
  }

  doc.moveDown();

  done();
  // });
  // });
};

/**
 * Writes an attachment's images.
 */
SavinaReport.prototype.writeAttachmentImages = function writeAttachmentImages(images) {
  return new Promise((resolve, reject) => {
    if (!images.length) {
      return resolve();
    }

    var report = this;
    var doc = this.doc;

    if (DOC_HEIGHT - DOC_MARGIN_BOTTOM - doc.y < IMAGES_SIZE) {
      doc.addPage();
    }

    doc.moveDown().font(HEADING_FONT_FAMILY).text(`Imágenes (${ images.length })`)
      .moveDown(0.25).font(DEFAULT_FONT_FAMILY);

    var count = 0;
    var row = {
      count: 0,
      x: doc.x,
      y: doc.y
    };

    function loop() {
      report.writeAttachmentImage(images[count], row, (err) => {
        if (err) {
          return reject(err);
        }

        if (++count === images.length) {
          return resolve();
        }

        loop();
      });
    }

    loop();
  });
};

/**
 * Writes an attachment's documents.
 */
SavinaReport.prototype.writeAttachmentDocuments = function writeAttachmentDocuments(documents) {
  return new Promise((resolve) => {
    if (!documents.length) {
      return resolve();
    }

    var request = this.request;
    var doc = this.doc;

    doc.moveDown();

    doc.font(HEADING_FONT_FAMILY).text(`Documentos (${ documents.length })`, DOC_MARGIN_LEFT, doc.y);

    doc.moveDown(0.5).font(DEFAULT_FONT_FAMILY);

    for (let i = 0, l = documents.length; i < l; i++) {
      let docu = documents[i];

      doc.fill('blue').text(docu.filename, {
        link: `${ request.host }/api/files/${ docu.id }/${ docu.filename }`,
        continued: true
      });

      doc.fill('black');

      if (i + 1 < l) {
        doc.text(COMMA, PARAMS_CONTINUED);
      } else {
        doc.text(COLON);
      }
    }

    doc.fill('black');

    resolve();
  });
};

/**
 * Writes an attachment.
 *
 * @param {Object} attachment The attachment data.
 *
 * @returns {Promise}
 */
SavinaReport.prototype.writeAttachment = function writeAttachment(attachment) {
  return new Promise((resolve) => {
    var doc = this.doc;

    this.addAnswerAttachmentHeader(attachment);

    doc.font(HEADING_FONT_FAMILY);

    doc.text(`${ attachment.name }: `, DOC_MARGIN_LEFT, doc.y, {
      continued: true
    });

    doc.font(DEFAULT_FONT_FAMILY).text(attachment.description);

    // doc.moveDown();

    console.log("Writing attachment for %s", attachment.id);

    if (attachment.files && attachment.files.length) {
      var documents = [];
      var images = [];

      for (let i = 0, l = attachment.files.length; i < l; i++) {
        let file = attachment.files[i];

        if (/image\/(jpeg|png)/gi.test(file.mimetype)) {
          images.push(file);
        } else {
          documents.push(file);
        }
      }

      console.log("Attachment %s has %d images and %d files.", attachment.id, images.length, documents.length);

      return this.writeAttachmentImages(images)
        .then(() => this.writeAttachmentDocuments(documents))
        .then(resolve);
    } else {
      doc.text(ATTACHMENTS.NO_FILES).moveDown();
    }

    resolve();
  });
};

/**
 * Writes the document's attachments section.
 */
SavinaReport.prototype.writeAttachments = function writeAttachments() {
  return new Promise((resolve, reject) => {
    var request = this.request;
    var doc = this.doc;
    var report = this;
    var count = 0;

    doc.addPage();

    this.addHeading(HEADINGS.ATTACHMENTS);

    function loop() {
      if (count === request.requestAttachments.length) {
        return resolve();
      }

      report.writeAttachment(request.requestAttachments[count]).then(() => {
        doc.moveDown().moveDown();

        count++;

        loop();
      }).catch(reject);
    }

    if (request.requestAttachments && request.requestAttachments.length) {
      return loop();
    }

    doc.text(ATTACHMENTS.EMPTY).moveDown().moveDown();

    resolve();
  });
};

/**
 * Writes the document's headers and footers on each page.
 */
SavinaReport.prototype.writeHeadersAndFooters = function writeHeadersAndFooters() {
  return new Promise((resolve) => {
    var request = this.request;
    var doc = this.doc;

    var headerDate = moment(request.report.date).format(HEADER_DATE_FORMAT);
    var range = doc.bufferedPageRange();

    for (let page = 0, total = range.count; page < total; page++) {
      doc.switchToPage(page);

      this.addHeaderAndFooter(doc.info.Title, headerDate, `${ page + 1 } / ${ total }`);

      this.addHeaderAndFooterDividers();
    }

    resolve();
  });
};

/**
 * Generates the PDF document.
 *
 * @param {PDFDocument} doc The PDF Document object.
 * @param {Object} request The request data.
 *
 * @returns {Promise}
 */
function generateDocument(doc, request) {
  var report = new SavinaReport(doc, request);

  /* Metadata */
  return report.defineMetadata()

  /* DETAILS */
  .then(() => report.writeDetails())

  /* REPORT */
  .then(() => report.writeReport())

  /* HYPOTHESIS */
  .then(() => report.writeHypothesis())

  /* DIAGNOSIS */
  .then(() => report.writeDiagnosis())

  /* DIAGNOSIS THERAPEUTIC PLAN */
  .then(() => report.writeDiagnosisTherapeuticPlan())

  /* DIAGNOSIS EXAMS */
  .then(() => report.writeDiagnosisExams())

  /* DIAGNOSIS CONTORL */
  .then(() => report.writeDiagnosisControl())

  /* ANSWERS */
  .then(() => report.writeAnswers())

  /* ATTACHMENTS */
  .then(() => report.writeAttachments())

  /* HEADERS AND FOOTERS */
  .then(() => report.writeHeadersAndFooters())

  .then(() => report = null);
}

/**
 * Creates a PDF document.
 *
 * @param {Object} request The request data.
 *
 * @returns {Promise}
 */
function create(request) {
  return new Promise((resolve, reject) => {
    fs.ensureFile(request.report.output, (err) => {
      if (err) {
        return reject(err);
      }

      var ws = fs.createWriteStream(request.report.output);
      var doc = new PDFDocument(OPTS_DOC);

      doc.pipe(ws);

      ws.once('close', () => {
        resolve(request);
      });

      ws.once('error', () => {
        reject();
      });

      generateDocument(doc, request).then(() => {
        doc.end();
        doc = null;
      }).catch(reject);
    });
  });
}

module.exports = {

  create: create

};
