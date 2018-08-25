(function (ng) {
  'use strict';

  ng.module('App').directive('requestAnswers', [
    '$session', '$http', '$routeParams', '$q', '$notifications', '$room', '$location',

    function ($session, $http, $routeParams, $q, $notifications, $room, $location) {

      return {
        templateUrl: '/assets/templates/requests/answers/list.html',

        restrict: 'E',

        scope: {
          request: '='
        },

        link: function ($scope) {
          var canceler = $q.defer();

          $scope.fetching = false;

          $session.get('activity').name = 'answers';

          $scope.fetch = function () {
            $scope.fetching = true;

            $http.get('/api/requests/answers/of/' + $routeParams.id, {
              timeout: canceler.promise
            }).then(function success(res) {
              $scope.request.requestAnswers = res.data;

              /* Update notifications */
              $http.put('/api/notifications/request/answered', {
                requestId: $routeParams.id
              }, {
                timeout: canceler.promise
              }).then(function success() {
                $notifications.requests.ongoing.update();
              });
            }, function error(res) {
              if (res.status > 0) {
                $session.flash('danger', "No se pudo obtener las respuestas");
              }
            }).then(function complete() {
              $scope.fetching = false;
            });
          };

          $room.on('answered', function () {
            if ($location.hash() === 'answers') {
              $scope.fetch();
            }
          });

          $scope.$on('$destroy', function () {
            canceler.resolve();
          });

          $scope.fetch();
        }
      };

    }

  ]);

}(angular));
