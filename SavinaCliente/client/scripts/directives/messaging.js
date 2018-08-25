(function (ng) {
  'use strict';

  ng.module('App').directive('messaging', [
    '$rootScope', '$http', '$io', '$routeParams', '$interval', '$q', '$filter', '$timeout', '$session', '$notifications', 'NOTIFY_TIMEOUT',

    function ($rootScope, $http, $io, $routeParams, $interval, $q, $filter, $timeout, $session, $notifications, NOTIFY_TIMEOUT) {

      return {
        templateUrl: '/assets/templates/messaging.html',
        restrict: 'E',

        scope: {},

        link: function link($scope) {
          var getUrl = '/api/requests/messages/of/' + $routeParams.id;
          var canceler = $q.defer();

          var refresher, oldest, newest, writingTimeout;

          var receivers = {};

          var socket = $io.connect('/messaging', {
            multiplex: false
          });

          $scope.session = $rootScope.session;
          $scope.peerWriting = false;
          $scope.connected = false;
          $scope.fetching = null;
          $scope.ready = false;
          $scope.messages = [];
          $scope.glued = true;

          /* Marks a message as received and notifies it */
          function receive(message) {
            function success(res) {
              message.receivedAt = res.data.receivedAt;

              socket.emit('received', {
                receivedAt: res.data,
                id: res.data.id
              });

              delete receivers[message.id];
            }

            /* Mark the message as received */
            $http.put('/api/requests/messages/received', {
              id: message.id
            }, {
              timeout: canceler.promise
            }).then(success);
          }

          /**
           * Refreshes all messages.
           */
          $scope.refresh = function refresh() {
            function foreach(message) {
              if (message.sentAt) {
                if (!newest || message.sentAt > newest) {
                  newest = message.sentAt;
                }

                if (!oldest || message.sentAt < oldest) {
                  oldest = message.sentAt;
                }

                if (!message.receivedAt && message.userId !== $session.user('id')) {
                  receivers[message.id] = $timeout(receive.bind(null, message), NOTIFY_TIMEOUT);
                }

                message.age = $filter('fromNow')(message.sentAt);
              }
            }

            if ($scope.messages.length) {
              ng.forEach($scope.messages, foreach);
            }
          };

          /**
           * Retrieves the older messages.
           */
          $scope.fetchOlder = function fetchOlder() {
            $scope.fetching = 'older';

            function success(res) {
              if (ng.isArray(res.data)) {
                $scope.messages = $scope.messages.concat(res.data);
              }
            }

            function error(res) {
              if (res.status > 0) {
                $session.flash('warning', "No se pudo obtener los mensajes más antiguos");
              }
            }

            function complete() {
              $scope.fetching = null;
              $scope.ready = true;
              $scope.refresh();
            }

            $http.get(getUrl, {
              timeout: canceler.promise,
              params: {
                oldest: oldest,
                update: true
              }
            }).then(success, error).then(complete);
          };

          /**
           * Retrieves the newer messages.
           */
          $scope.fetchNewer = function fetchNewer() {
            $scope.fetching = 'newer';

            function success(res) {
              if (ng.isArray(res.data)) {
                $scope.messages = $scope.messages.concat(res.data);
              }
            }

            function error(res) {
              if (res.status > 0) {
                $session.flash('warning', "No se pudieron obtener los mensajes más nuevos");
              }
            }

            function complete() {
              $scope.fetching = null;
              $scope.ready = true;
              $scope.refresh();
            }

            $http.get(getUrl, {
              timeout: canceler.promise,
              params: {
                newest: newest,
                update: true
              }
            }).then(success, error).then(complete);
          };

          /** When connected */
          socket.on('connect', function onconnect() {
            $scope.connected = true;

            /* Join the messaging room */
            socket.emit('join', $routeParams.id);

            socket.on('joined', function onjoined() {
              function success(res) {
                if (ng.isArray(res.data)) {
                  $scope.messages = res.data;
                  $scope.refresh();
                }
              }

              function error(res) {
                if (res.status > 0) {
                  $scope.messages = [];
                }
              }

              function complete() {
                $scope.ready = true;
              }

              /* Retrieve last 10 messages */
              $http.get(getUrl, {
                timeout: canceler.promise
              }).then(success, error).then(complete);

              /* Refresh messages interval */
              refresher = $interval($scope.refresh, 10000);

              /** Should update messages */
              socket.on('update', function () {
                $scope.fetchNewer();
              });

              socket.on('received', function onreceived(data) {
                $scope.messages.forEach(function (message) {
                  if (message.id === data.id) {
                    message.receivedAt = data.receivedAt;
                  }
                });

                $scope.$apply();
              });

              socket.on('writing', function (writing) {
                $scope.$apply(function () {
                  $scope.peerWriting = writing;
                });
              });

              /* Clear the messaged notifications for this request room */
              $http.put('/api/notifications/request/messaged', {
                requestId: $routeParams.id
              }, {
                timeout: canceler.promise
              }).then(function success() {
                $notifications.update();
              });
            });
          });

          $scope.writing = function () {
            console.log("Writing...");

            if (writingTimeout) {
              console.log("Rescheduling timeout...");

              $timeout.cancel(writingTimeout);

              writingTimeout = $timeout(function () {
                socket.emit('writing', false);
                writingTimeout = null;
              }, 3000);
            } else {
              console.log("Scheduling timeout...");

              socket.emit('writing', true);

              writingTimeout = $timeout(function () {
                socket.emit('writing', false);
                writingTimeout = null;
              }, 3000);
            }
          };

          /**
           * Sends a new message.
           */
          $scope.send = function send() {
            $scope.messages.push({
              workplaceId: $session.get('workplace').id,
              userId: $session.user('id'),
              requestId: $routeParams.id,
              body: $scope.form.body
            });

            socket.emit('writing', false);
            $scope.form.body = null;
          };

          $scope.notify = function notify() {
            socket.emit('update');

            $http.post('/api/notifications/request/messaged', {
              requestId: $routeParams.id
            });

            $scope.refresh();
          };

          /* Disconnect from messaging socket on destroy */
          $scope.$on('$destroy', function $destroy() {
            if (refresher) {
              $interval.cancel(refresher);
            }

            /* Cancel all received notifiers */
            if (Object.keys(receivers).length) {
              for (var id in receivers) {
                if (receivers[id]) {
                  $timeout.cancel(receivers[id]);
                }
              }
            }

            socket.disconnect();
            canceler.resolve();
          });

        }
      };

    }
  ]);

}(angular));
