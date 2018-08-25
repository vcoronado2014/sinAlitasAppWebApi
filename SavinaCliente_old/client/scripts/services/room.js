(function (ng) {
  'use strict';

  ng.module('App').factory('$room', [
    '$io',

    function ($io) {

      return {

        socket: null,

        connect: function () {
          if (this.socket) {
            this.disconnect();
          }

          this.socket = $io.connect('/room', {
            multiplex: false
          });
        },

        on: function () {
          this.socket.on.apply(this.socket, arguments);
        },

        emit: function () {
          this.socket.emit.apply(this.socket, arguments);
        },

        disconnect: function () {
          this.socket.disconnect();
          this.socket = null;
        }

      };

    }
  ]);

}(angular));
