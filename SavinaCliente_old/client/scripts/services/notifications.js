(function (ng) {
  'use strict';

  ng.module('App').factory('$notifications', [
    '$location', '$http', '$session', '$q', '$rootScope', 'ngAudio',

    function ($location, $http, $session, $q, $rootScope, $audio) {
      var $notifications = {
        socket: null
      };

      $notifications.WORKPLACES_UPDATED = 'notifications workplaces updated';
      $notifications.REQUESTS_UPDATED = 'notifications requests updated';

      function Request(name) {
        this.consultation = [];
        this.diagnosis = [];
        this.name = name;

        this.cancelers = {
          consultation: null,
          diagnosis: null
        };
      }

      Request.prototype.total = function () {
        return this.consultation.length + this.diagnosis.length;
      };

      Request.prototype.update = function (type) {
        var request = this;

        if (ng.isString(type)) {
          console.log("Updating %s notifications", type);

          if (ng.isArray(request[type])) {
            console.log("Type:", type);

            if (request.cancelers[type]) {
              request.cancelers[type].resolve();
            }

            request.cancelers[type] = $q.defer();

            $http.get('/api/notifications/request/' + request.name + '/' + type, {
              timeout: request.cancelers[type].promise
            }).then(function success(res) {
              console.log("Got notifications response for %s", type, res);

              request.cancelers[type] = null;

              if (ng.isArray(res.data)) {
                request[type] = res.data;
              } else {
                request[type] = [];
              }
            }, function error(res) {
              request.cancelers[type] = null;

              if (res.status > 0) {
                request[type] = [];
              }
            }).then(function () {
              console.log("Got updated notifications for %s", type);
              $rootScope.$broadcast($notifications.REQUESTS_UPDATED);
            });
          } else {
            console.error("request[%s] is not an array! it looks like %s...", type, typeof type);
            console.log(request[type]);
          }
        } else {
          request.update('consultation');
          request.update('diagnosis');
        }
      };

      $notifications.join = function () {
        if ($notifications.socket) {
          $notifications.socket.emit('join');
        }
      };

      $notifications.leave = function () {
        if ($notifications.socket) {
          $notifications.socket.emit('leave');
        }
      };

      $notifications.init = function () {
        this.requests = {
          waiting: new Request('waiting'),
          ongoing: new Request('ongoing'),
          closed: new Request('closed'),
          sent: new Request('sent')
        };
      };

      $notifications.update = function update() {
        // if ($session.user() && $session.get('workplace') */) {
        if ($session.user()) {
          $notifications.requests.ongoing.update();

          if ($session.user('specialty')) {
            $notifications.requests.waiting.update();
            $notifications.requests.closed.update();
          }
        }

        $notifications.update.workplaces();
      };

      /** Notification sound */
      var sound = $audio.load('sound-notification');
      var shouldPlay = true;

      function resetSound() {
        sound.progress = 0;
        shouldPlay = true;
      }

      $notifications.playSound = function playSound() {
        var prefs = $session.get('preferences');

        if (shouldPlay && (!prefs || prefs.soundNotifications)) {
          setTimeout(resetSound, 3000);
          shouldPlay = false;
          sound.play();
        }
      };

      $notifications.workplaces = {
        list: [],
        all: 0
      };

      $notifications.update.workplaces = function (workplaces) {
        if ($session.user() && workplaces) {
          var promises = [];

          workplaces.forEach(function (workplace) {
            promises.push($http.get('/api/notifications/workplaces/by/id/' + workplace.id));
          });

          $q.all(promises).then(function (responses) {
            responses.forEach(function (res) {
              $notifications.workplaces.list[Number(res.data.workplaceId)] = res.data.count;
            });
          }).catch(function (res) {
            console.error(res);
          }).finally(function () {
            $rootScope.$broadcast($notifications.WORKPLACES_UPDATED);
          });
        } else {
          $http.get('/api/notifications/workplaces').then(function (res) {
            $notifications.workplaces.all = Number(res.data.count);
          }).finally(function () {
            $rootScope.$broadcast($notifications.WORKPLACES_UPDATED);
          });
        }
      };

      $notifications.reset = function () {
        $notifications.init();
        $notifications.join();

        if ($session.user() && $session.get('workplace')) {
          $notifications.update();
        }
      };

      $notifications.notify = function (uri, data) {
        if (ng.isObject(data)) {
          return $http.post('/api/notifications/' + uri, data);
        }

        $http.post('/api/notifications/' + uri, {
          requestId: parseInt(data)
        });
      };

      $notifications.notify.request = {
        answered: function (id) {
          $notifications.notify('request/answered', id);
        },

        attached: function (id) {
          $notifications.notify('request/attached', id);
        },

        taken: function (id) {
          $notifications.notify('request/taken', id);
        },

        returned: function (data) {
          $notifications.notify('request/returned', data);
        },

        closed: function (id) {
          $notifications.notify('request/closed', id);
        },

        messaged: function (id) {
          $notifications.notify('request/messaged', id);
        },

        new: function (id) {
          $notifications.notify('request/waiting', id);
        }

      };

      $notifications.notify.request.messaged.cancel = function (id) {
        $http.delete('/api/notifications/request/messaged/' + id);
      };

      return $notifications;
    }
  ])

  .run([
    '$rootScope', '$notifications', '$session', '$io',

    function ($rootScope, $notifications, $session, $io) {
      function onEvent() {
        $notifications.playSound();
        $notifications.update();
      }

      $rootScope.$on($session.UPDATED, function () {
        if ($session.user()) {
          if (!$notifications.socket) {
            $notifications.socket = $io.connect('/notifications');

            $notifications.socket.on('request messaged', onEvent);
            $notifications.socket.on('request answered', onEvent);
            $notifications.socket.on('request attached', onEvent);
            $notifications.socket.on('request closed', onEvent);
            $notifications.socket.on('request taken', onEvent);
            $notifications.socket.on('new request', onEvent);

            $notifications.socket.on('connect', function () {
              console.log("Connected to the notifications socket");
              $notifications.reset();
            });
          } else {
            $notifications.reset();
          }
        } else {
          if ($notifications.socket) {
            $notifications.socket.disconnect();
            $notifications.socket = null;
          }
        }
      });

    }
  ]);

}(angular));
