(function (ng) {
  'use strict';

  ng.module('App').directive('worktableRequest', [
    '$rootScope', '$notifications', '$q', '$http', '$timeout', '$session', 'NOTIFY_TIMEOUT', '$moment',

    function ($rootScope, $notifications, $q, $http, $timeout, $session, NOTIFY_TIMEOUT, $moment) {
      return {
        templateUrl: '/assets/templates/requests/worktable/request.html',
        restrict: 'E',

        scope: {
          request: '=',
          created: '='
        },

        link: function ($scope, $element) {
          //como mejora, ahora trae la fecha hora del servidor en el request
          var activity = $rootScope.session.activity.name;
          var $scroller = $element.closest('.scroll-y');
          var canceler = $q.defer();
          var timeout;
          //agregado por coro
          var fechaServidor = $scope.request.fechaServidor;
          var fechaCreacion = $moment($scope.request.createdAt);
          var fechaCierre = $moment($scope.request.closedAt);
          var fechaCumple = $moment($scope.request.patient.birth);

          $scope.dateCreate = fechaCreacion.from(fechaServidor).split(/\s/);
          $scope.dateClose = fechaCierre.from(fechaServidor).split(/\s/);
          $scope.dateBirth = fechaCumple.from(fechaServidor).split(/\s/);
          $scope.age = fechaServidor.diff(fechaCumple, 'years') + ' años';


          /*
          
          var fechaServidor;
            $http.get('/api/date', {
                          timeout: 9000
                        }).then(function success(res) {
                          if (res.status === 200) {
                            //var obj1 = JSON.stringify(res.data[0]);
                            //fechaServidor =  $.parseJSON(obj1)[0].fecha;
                            fechaServidor =  $moment(res.data);
                            $scope.dateCreate = fechaCreacion.from(fechaServidor).split(/\s/);
                            $scope.dateClose = fechaCierre.from(fechaServidor).split(/\s/);
                            $scope.dateBirth = fechaCumple.from(fechaServidor).split(/\s/);
                            $scope.age = fechaServidor.diff(fechaCumple, 'years') + ' años';
                            //break;
                          } 
                        }, function error(data, status) {
                          if (status > 0) {
                            $session.flash('danger', "No se pudo obtener fecha Servidor!");
                            //dejamos en el scope la fecha hora local
                            $scope.dateCreate = fechaCreacion.fromNow(true).split(/\s/);
                            $scope.dateClose = fechaCierre.fromNow(true).split(/\s/);
                            $scope.dateBirth = fechaCumple.fromNow(true).split(/\s/);
                            $scope.age = $scope.dateBirth[0] + ' años';
                          }
                        }).then(function complete() {
                          self.fetching = false;
                          self.fetched = true;

                        });

        */

          function isVisible() {
            return $element.offset().top + Math.round($element.height() * 0.1) <= $(window).height();
          }

          /**
           * Mark answer as seen.
           *
           * Makes the request to mark the answer as seen and cancels scroll
           * events and timeouts.
           */
          function markAsSeen() {
            if (isVisible()) {
              $http.put('/api/notifications/request/' + activity, {
                requestId: $scope.request.id
              }, {
                timeout: canceler.promise
              }).then(function success(res) {
                $notifications.requests[activity].update($scope.request.requestType.slug);

                $scope.request.highlight = false;

                if (activity === 'waiting') {
                  $scope.request.seenNotification = res.data;
                }

                if (activity === 'closed') {
                  $scope.request.closedNotification = res.data;
                }
              });

              $scroller.off('scroll', onscroll);

              stopTimeout();
            }
          }

          function stopTimeout() {
            $timeout.cancel(timeout);
            timeout = null;
          }

          function startTimeout() {
            timeout = $timeout(markAsSeen, NOTIFY_TIMEOUT * 2);
          }

          /**
           * Scroll listener.
           *
           * Checks if at least 10% of the answer is on screen after starting
           * the timeout to mark it as seen.
           */
          function onscroll() {
            if (isVisible()) {
              if (!timeout) {
                startTimeout();
              }
            } else {
              if (timeout) {
                stopTimeout();
              }
            }
          }

          if (activity === 'waiting') {
            $scope.request.highlight = !$scope.request.seenNotification ||
              !$scope.request.seenNotification.seenAt;
          }

          if (activity === 'closed') {
            $scope.request.highlight = $scope.request.creatorUserId !== $session.user('id') &&
              (!$scope.request.closedNotification || !$scope.request.closedNotification.seenAt);
          }

          if ((activity === 'closed' || activity === 'waiting') && $scope.request.highlight) {
            if (isVisible()) {
              startTimeout();
            }

            $scroller.on('scroll', onscroll);
          }

          function updateOngoing() {
            /* Mock an apply */
            $timeout(function () {
              var notifications = $notifications.requests[$session.get('activity').name][$scope.request.requestType.slug];

              $scope.notifications = {
                answered: 0,
                messaged: 0,
                attached: 0,
                taken: 0
              };

              ng.forEach(notifications, function (notification) {
                if (notification.foreignKey === $scope.request.id) {
                  $scope.notifications[notification.action]++;
                }
              });

              $scope.request.highlight = $scope.notifications.taken ||
                $scope.notifications.answered ||
                $scope.notifications.attached ||
                $scope.notifications.messaged;
            }, 1);
          }

          if (activity === 'ongoing') {
            $scope.$on($notifications.REQUESTS_UPDATED, updateOngoing);
            updateOngoing();
          }

          $scope.$on('$destroy', function () {
            $timeout.cancel(timeout);
            canceler.resolve();
          });
        }
      };
    }
  ]);

}(angular));
