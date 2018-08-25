'use strict';

var debug = require('debug')('app:sockets');
var cookieParser = require('cookie-parser');
var cookie = require('cookie');
var path = require('path');

var session = component('session');

module.exports = {

  basedir: path.normalize(path.join(__basedir, 'sockets')),

  debug: debug,

  arguments: [
    /**
     * Authorizer socket middleware.
     *
     * Assigns a session getter method to the current socket.
     */
    function authorizer(socket, next) {
      var sesscfg = session.config();
      var cookies = cookie.parse(socket.request.headers.cookie);

      cookies = cookieParser.signedCookies(cookies, sesscfg.secret);

      socket.getSession = function (callback) {
        return sesscfg.store.get(cookies[sesscfg.name], callback);
      };

      socket.getSession(function (err, sess) {
        if (err) {
          return next(err);
        }

        if (sess && sess.user) {
          return next();
        }

        next(new Error("Invalid session!"));
      });
    }
  ]

};
