'use strict';

var cookieParser = require('cookie-parser');
var base64url = require('base64url');
var crypto = require('crypto');
var path = require('path');
var is = require('fi-is');
var fs = require('fs');

module.exports = {

  saveUninitialized: true,

  cookieParser: null,

  secret: null,

  resave: true,

  store: null,

  name: null,

  cookie: {
    secure: false
  },

  config: function () {
    return {
      saveUninitialized: this.saveUninitialized,
      cookieParser: this.cookieParser,
      cookie: this.cookie,
      secret: this.secret,
      resave: this.resave,
      store: this.store,
      name: this.name
    };
  },

  configure: function (sess, cfg) {
    if (is.function(cfg.store)) {
      this.store = cfg.store(sess);
    }

    if (is.string(cfg.secret) || is.array(cfg.secret)) {
      this.secret = cfg.secret;
    } else {
      var sessionKeyFile = path.join(__basedir, 'credentials', 'session.key');

      try {
        this.secret = fs.readFileSync(sessionKeyFile, 'utf-8');
      } catch (ex) {
        this.secret = fs.writeFileSync(sessionKeyFile, base64url(crypto.randomBytes(48)), 'utf-8');
      }
    }

    if (is.string(cfg.name)) {
      this.name = cfg.name;
    }

    if (is.object(cfg.cookie)) {
      for (var key in cfg.cookie) {
        if (cfg.cookie[key]) {
          this.cookie[key] = cfg.cookie[key];
        }
      }
    }

    this.saveUninitialized = !!cfg.saveUninitialized;
    this.cookieParser = cookieParser(this.secret);
    this.resave = !!cfg.resave;
  }
};
