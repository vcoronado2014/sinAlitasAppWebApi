'use strict';

var debug = require('debug')('app:sockets:presence');
var is = require('fi-is');

var rooms = {};

module.exports = function (nsp, io, authorizer) {

  nsp.use(function (socket, next) {
    debug("User [%s] is connecting...", socket.client.id);
    next();
  });

  nsp.use(authorizer);

  /**
   * A client socket has connected.
   */
  nsp.on('connection', function (socket) {

    /**
     * Client socket wants to join.
     */
    socket.on('join', function () {
      socket.getSession(function (err, session) {
        if (err) {
          debug("Invalid session!");
        }

        if (!session || !session.user || !session.workplace) {
          return debug("Joining user [%s] is not signed in!", socket.client.id);
        }

        socket.session = session;

        debug("User [%s] connected to [%s]", socket.client.id, nsp.name);

        var group = session.user.specialtyId ? 'specialists' : 'gps';
        var room = 'workplace-' + session.workplace.id;

        if (is.not.object(rooms[room])) {
          rooms[room] = {
            specialists: [],
            gps: []
          };
        }

        if (rooms[room][group].indexOf(session.user.id) < 0) {
          socket.join(room);

          socket.emit('joined');

          rooms[room][group].push(session.user.id);

          debug("User [%s] joined [%s]", socket.client.id, room);

          session.workplace.counterparts.forEach(function (wpid) {
            room = 'workplace-' + wpid;

            socket.to(room).broadcast.emit('update users');

            debug("Notified room [%s] that user [%s] joined", room, socket.client.id);
          });
        }
      });
    });

    /**
     * Client socket wants the connected counterpart users.
     */
    socket.on('users', function () {
      var users = [];

      socket.session.workplace.counterparts.forEach(function (wpid) {
        var group = socket.session.user.specialtyId ? 'gps' : 'specialists';
        var room = 'workplace-' + wpid;

        if (is.object(rooms[room]) && is.array(rooms[room][group])) {
          rooms[room][group].forEach(function (userId) {
            if (userId !== socket.session.user.id && users.indexOf(userId) < 0) {
              users.push(userId);
            }
          });
        }
      });

      socket.emit('users', users);
    });

    /**
     * Client socket has left.
     */
    function onleave() {
      if (!socket.session || !socket.session.user || !socket.session.workplace) {
        return debug("Leaving user [%s] is not signed in!", socket.client.id);
      }

      var group = socket.session.user.specialtyId ? 'specialists' : 'gps';
      var room = 'workplace-' + socket.session.workplace.id;

      if (is.object(rooms[room])) {
        var index = rooms[room][group].indexOf(socket.session.user.id);

        if (index > -1) {
          rooms[room][group].splice(index, 1);

          socket.leave(room);

          socket.emit('leaved');

          debug("%s user [%s] has left [%s]", group, socket.client.id, room);

          socket.session.workplace.counterparts.forEach(function (wpid) {
            room = 'workplace-' + wpid;

            socket.to(room).broadcast.emit('update users');

            debug("Notified room [%s] that user [%s] left", room, socket.client.id);
          });
        }
      }
    }

    socket.on('disconnect', onleave);
    socket.on('leave', onleave);

  });

};
