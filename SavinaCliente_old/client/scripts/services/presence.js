(function (ng) {
  'use strict';

  ng.module('App').factory('$presence', [

    function () {

      return {
        CONNECTION_STATUS: 'presence connection status',
        USERS_UPDATED: 'presence users updated',

        workplaceId: null,
        connected: false,
        joined: true,
        socket: null,
        users: []
      };

    }

  ]);

  ng.module('App').run([
    '$presence', '$rootScope', '$session', '$io',

    function ($presence, $rootScope, $session, $io) {
      $rootScope.$on($session.UPDATED, function () {
        console.log("Session was updated!");

        if (!$session.get('workplace') || $session.get('workplace').id !== $presence.workplaceId) {
          $rootScope.$broadcast('session workplace changed');
        }
      });

      $rootScope.$on('session workplace changed', function () {
        console.log("Workplace has changed!");

        var workplace = $session.get('workplace');

        if (!workplace) {
          if ($presence.socket && $presence.connected) {
            $presence.socket.emit('leave');
            $presence.socket.disconnect();
            $presence.workplaceId = null;
            $presence.connected = false;
            $presence.socket = null;
            $presence.users = [];

            $rootScope.$broadcast($presence.USERS_UPDATED, $presence.users);
          }

          return;
        }

        if ($presence.socket && $presence.connected) {
          if (!workplace || workplace.id !== $presence.socketId) {
            $presence.socket.emit('leave');

            if (workplace) {
              $presence.workplaceId = workplace.id;
              $presence.socket.emit('join');
            }
          }
        }

        if (!$presence.socket) {
          $presence.socket = $io.connect('/presence', {
            multiplex: false
          });

          $presence.socket.on('connect', function () {
            console.log("Connected to presence socket");

            $presence.connected = true;

            $rootScope.$broadcast($presence.CONNECTION_STATUS, true);

            if ($session.get('workplace')) {
              $presence.workplaceId = $session.get('workplace').id;
              $presence.socket.emit('join');
            }
          });

          $presence.socket.on('error', console.error);

          $presence.socket.on('joined', function () {
            console.log("Presence joined room");

            $presence.joined = true;

            $presence.socket.emit('users');
          });

          $presence.socket.on('leaved', function () {
            $presence.socket.emit('users');
            $presence.joined = false;
          });

          $presence.socket.on('update users', function () {
            $presence.socket.emit('users');
          });

          $presence.socket.on('users', function (users) {
            $presence.users = users;

            $rootScope.$broadcast($presence.USERS_UPDATED, users);

            console.log("Update presence users");
          });

          $presence.socket.on('disconnect', function () {
            console.warn("Disconnected from presence socket!");

            $presence.connected = false;

            $presence.users = [];

            $rootScope.$broadcast($presence.USERS_UPDATED, $presence.users);

            $rootScope.$broadcast($presence.CONNECTION_STATUS, false);
          });
        }
      });
    }
  ]);

}(angular));
