(function (ng) {
  'use strict';

  ng.module('App').controller('Requests:Room', [
    '$scope', '$location', '$session', '$routeParams', '$io', '$notifications', '$q', '$http', '$room', 'request',

    function ($scope, $location, $session, $routeParams, $io, $notifications, $q, $http, $room, request) {
      var canceler = $q.defer();

      $scope.connected = false;

      $session.get('activity').request = request.data.requestType.slug;

      if (!request) {
        $session.flash('danger', "No se pudo obtener esa solicitud!");

        var activity = $session.get('activity').name || 'ongoing';
        var type = $session.get('activity').request || 'consultation';

        $location.path('/requests/worktable/' + activity + '/' + type);
      }

      if (request.closedAt) {
        $session.flash('info', "La solicitud fué cerrada");
        $location.path('/requests/' + request.data.id);
      }

      $room.connect();

      $room.on('connect', function onconnect() {
        $room.emit('join', request.data.id);
      });

      $room.on('full', function onfull() {
        $location.path('/requests/worktable/ongoing/' + request.data.requestType.slug);

        $session.flash('danger', "Esa sala está en uso!");

        $scope.$apply();
      });

      $room.on('joined', function onjoined() {
        $scope.request = request.data;

        $scope.conference = {
          active: false,
          busy: false,

          toggle: function toggle() {
            this.active = !this.active;
          }
        };

        if (!$location.hash()) {
          $location.hash('details');
        }

        $notifications.notify.request.messaged.cancel(request.data.id);

        $scope.connected = true;

        $scope.$apply();
      });

      $room.on('closed', function onclosed() {
        $location.path('/requests/' + request.data.id);

        $session.flash('info', "La solicitud fué cerrada");

        $scope.$apply();
      });

      $room.on('returned', function onreturned() {
        if ($session.user('specialty')) {
          $location.path('/requests/worktable/waiting/' + request.data.requestType.slug);
        } else {
          $location.path('/requests/worktable/sent/' + request.data.requestType.slug);
        }

        $session.flash('info', "La solicitud fué devuelta a la mesa de trabajo");

        $scope.$apply();
      });

      /* Clear any request taken notification */
      $http.put('/api/notifications/request/taken', {
        requestId: request.data.id
      }, {
        timeout: canceler.promise
      }).then(function success() {
        $notifications.update();

        $scope.$watch(function () {
          return $notifications.requests.ongoing[request.data.requestType.slug];
        }, function (notifications) {
          $scope.notifications = {
            attached: 0,
            answered: 0,
            messaged: 0
          };

          ng.forEach(notifications, function (notification) {
            if (notification.foreignKey === request.data.id) {
              $scope.notifications[notification.action]++;
            }
          });
        });
      });

      $scope.$on('$destroy', function $destroy() {
        $location.search('');
        $location.hash('');

        $room.disconnect();

        canceler.resolve();
      });
    }

  ]);

}(angular));
