'use strict';

var debug = require('debug')('app:sockets:conference');

module.exports = function (nsp, io, authorizer) {

  nsp.use(function (socket, next) {
    debug("User [%s] is connecting...", socket.client.id);
    next();
  });

  nsp.use(authorizer);

  nsp.on('connection', function onconnection(socket) {
    debug("User [%s] has connected to [%s]", socket.client.id, nsp.name);

    var room;

    socket.on('disconnect', function ondisconnect() {
      debug("User [%s] has disconnected", socket.client.id);
      socket.to(room).broadcast.emit('peer left');
    });

    socket.on('join', function onjoin(_room) {
      room = _room;

      debug("Request to create or join room [%s] on [%s]", _room, nsp.name);

      nsp.in(room).clients(function (err, clients) {
        if (err) {
          throw err;
        }

        debug("Room [%s] on namespace [%s] has [%d] clients", room, nsp.name, clients.length);

        if (clients.length < 2) {
          socket.join(room);

          if (!clients.length) {
            debug("User [%s] has created the room [%s]", socket.client.id, room);
            socket.emit('created', room);
          } else {
            debug("User [%s] has joined the room [%s]", socket.client.id, room);
            socket.emit('joined', room);
          }
        } else {
          debug("User  [%s] tried to join room [%s] but it's full", socket.client.id, room);
          socket.emit('full', room);
        }
      });
    });

    socket.on('offer', function onoffer(offer) {
      debug("Got offer from [%s] in room [%s]", socket.client.id, room);
      socket.to(room).broadcast.emit('offer', offer);
    });

    socket.on('answer', function onanswer(answer) {
      debug("Got answer from [%s] in room [%s]", socket.client.id, room);
      socket.to(room).broadcast.emit('answer', answer);
    });

    socket.on('ice candidate', function onicecandidate(candidate) {
      debug("Got ICE candidate from [%s] in room [%s]", socket.client.id, room);
      socket.to(room).broadcast.emit('ice candidate', candidate);
    });

    socket.on('leave', function onleave() {
      socket.disconnect();
    });
  });

};
