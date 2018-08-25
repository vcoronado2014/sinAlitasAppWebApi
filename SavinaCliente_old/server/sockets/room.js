'use strict';

var debug = require('debug')('app:sockets:room');

module.exports = function (nsp, io, authorizer) {

  nsp.use(function (socket, next) {
    debug("User [%s] is connecting...", socket.client.id);
    next();
  });

  nsp.use(authorizer);

  nsp.on('connection', function onconnection(socket) {
    debug("A user connected to [%s]", nsp.name);

    socket.on('disconnect', function ondisconnect() {
      debug("User disconnected");
    });

    socket.on('join', function onjoin(id) {
      var room = 'request-' + id;

      debug("Request to join room [%s] on [%s]", room, nsp.name);

      nsp.in(room).clients(function (err, clients) {
        if (err) {
          throw err;
        }

        debug("Room [%s] on namespace [%s] has [%d] clients", room, nsp.name, clients.length);

        if (clients.length > 1) {
          debug("Room [%s] is full!", room);

          return socket.emit('full');
        }

        socket.join(room);

        socket.emit('joined');
      });
    });
  });

};
