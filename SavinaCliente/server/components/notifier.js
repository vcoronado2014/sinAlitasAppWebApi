'use strict';

var schedule = require('node-schedule');
var moment = require('moment');
var is = require('fi-is');

var mailer = component('mailer');

var debug = function () {};

var config = {
  delay: [15, 'minutes']
};

module.exports = {
  configure: function (cfg) {
    if (is.function(cfg.debug)) {
      debug = cfg.debug;
    } else if (is.boolean(cfg.debug) && cfg.debug) {
      debug = console.log;
    }

    if (is.array(cfg.delay)) {
      config.delay = cfg.delay;
    }
  },

  request: {
    waiting: function (email, subject, locals) {
      debug("Sending new request (waiting) notification to %s", email);
      mailer.send('requests/waiting', email, subject, locals);
    },

    taken: function (email, subject, locals) {
      debug("Sending request taken notification to %s", email);
      mailer.send('requests/taken', email, subject, locals);
    },

    answered: function (email, subject, locals) {
      debug("Sending request answer notification to %s", email);
      mailer.send('requests/answered', email, subject, locals);
    },

    attached: function (email, subject, locals) {
      debug("Sending request new attachment notification to %s", email);
      mailer.send('requests/attached', email, subject, locals);
    },

    returned: function (email, subject, locals) {
      debug("Sending request returned notification to %s", email);
      mailer.send('requests/returned', email, subject, locals);
    },

    closed: function (email, subject, locals) {
      debug("Sending request closed notification to %s", email);
      mailer.send('requests/closed', email, subject, locals);
    }
  },

  message: {
    list: {},

    namer: function (sender, receiver, request) {
      return sender + '-' + receiver + '@' + request;
    },

    callback: function (email, subject, locals, name) {
      mailer.send('request/messaged', email, subject, locals);
      this.cancel(name);
    },

    pending: function (sender, receiver, subject, request, locals) {
      var name = this.namer(sender.id, receiver.id, request.id);

      if (this.list[name]) {
        return;
      }

      debug("Adding pending unread message notification for %s (%s)", receiver.email, name);

      var callback = this.callback.bind(this, receiver.email, subject, locals, name);
      var date = moment().add.apply(moment(), config.delay).toDate();

      this.list[name] = schedule.scheduleJob(date, callback);
    },

    cancel: function () {
      var name;

      if (arguments.length === 1 && is.string(arguments[0])) {
        name = arguments[0];
      } else if (arguments.length === 3 && is.all.number.apply(null, arguments)) {
        name = this.namer.apply(this, arguments);
      } else {
        throw new Error("Wrong argument number (%d) or types. Must be one [String] or three [Number].", arguments.length);
      }

      debug("Deleting any notification for %s", name);

      if (this.list[name]) {
        this.list[name].cancel();
        delete this.list[name];
      }
    }
  }
};
