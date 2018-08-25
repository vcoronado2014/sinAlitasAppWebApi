'use strict';

var debug = require('debug')('app:sockets:messaging');

module.exports = function (nsp, io, authorizer) {

  nsp.use(function (socket, next) {
    debug("User [%s] is connecting...", socket.client.id);
    next();
  });

  nsp.use(authorizer);

  nsp.on('connection', function (socket) {
    debug("A user connected to [%s]", nsp.name);

    var room;

    socket.on('disconnect', function () {
      debug("User disconnected");
    });

    socket.on('join', function (id) {
      room = 'request-' + id;

      nsp.in(room).clients(function (err, clients) {
        if (err) {
          throw err;
        }

        debug("Request to join room [%s] on [%s]", room, nsp.name);

        if (clients.length < 2) {
          debug("Room [%s] on namespace [%s] has [%d] clients", room, nsp.name, clients.length);

          socket.join(room);

          socket.emit('joined');
        }
      });
    });

    socket.on('update', function () {
      socket.to(room).broadcast.emit('update');
    });

    socket.on('received', function (data) {
      socket.to(room).broadcast.emit('received', data);
    });

    socket.on('writing', function (writing) {
      socket.to(room).broadcast.emit('writing', writing);
    });

  });

};
