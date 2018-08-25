'use strict';

var debug = require('debug')('app:sockets:notifications');
var cookieParser = require('cookie-parser');
var cookie = require('cookie');

var session = component('session');

module.exports = function (nsp) {

  nsp.use(function (socket, next) {
    debug("User [%s] is connecting...", socket.client.id);
    next();
  });

  nsp.on('connection', function onconnection(socket) {
    debug("A user connected to [%s]", nsp.name);

    var workplaceRoom, userRoom;

    function reset() {
      if (userRoom) {
        socket.leave(userRoom);
        userRoom = null;
      }

      if (workplaceRoom) {
        socket.leave(workplaceRoom);
        workplaceRoom = null;
      }
    }

    socket.on('disconnect', function ondisconnect() {
      debug("User disconnected");
      reset();
    });

    socket.on('join', function () {
      reset();

      var sesscfg = session.config();
      var cookies = cookie.parse(socket.request.headers.cookie);

      cookies = cookieParser.signedCookies(cookies, sesscfg.secret);

      return sesscfg.store.get(cookies[sesscfg.name], function (err, sess) {
        if (err) {
          throw err;
        }

        if (sess && sess.user) {
          userRoom = 'user-' + sess.user.id;

          socket.join(userRoom);

          debug("User joined [%s]", userRoom);
        }

        if (sess && sess.workplace) {
          workplaceRoom = 'workplace-' + sess.workplace.id;

          socket.join(workplaceRoom);

          debug("User joined [%s]", workplaceRoom);
        }
      });
    });

    socket.on('leave', reset);

  });

};
