'use strict';

var smtpTransport = require('nodemailer-smtp-transport');
var nodemailer = require('nodemailer');
var moment = require('moment');
var jade = require('jade');
var path = require('path');
var is = require('fi-is');

var config = {};

var debug = function () {};

/**
 * Prepares the connection to the SMTP server.
 */
function send(template, to, subject, locals) {
  var html;

  locals = locals || {};
  locals.moment = moment;

  try {
    html = jade.compileFile(path.join(config.templates, template + '.jade'))(locals);
  } catch (ex) {
    return debug(ex);
  }

  var transporter = nodemailer.createTransport(smtpTransport(config.connection));

  var message = {
    from: config.sender,
    subject: subject,
    html: html,
    to: to,
  };

  transporter.sendMail(message, function (err, info) {
    if (err) {
      debug(err);
    }

    if (info) {
      debug(info);
    }
  });
}

function configure(cfg) {
  if (is.function(cfg.debug)) {
    debug = cfg.debug;
  } else if (is.boolean(cfg.debug) && cfg.debug) {
    debug = console.log;
  }

  if (is.not.string(cfg.templates)) {
    throw new Error("Templates path must be a [String]!");
  }

  config = cfg;
}

module.exports = {

  configure: configure,

  send: send

};
