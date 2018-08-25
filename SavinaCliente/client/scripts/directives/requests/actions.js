(function (ng) {
  'use strict';

  ng.module('App').directive('requestActions', [
    '$http', '$session', '$routeParams', '$location', '$q', '$notifications',

    function ($http, $session, $routeParams, $location, $q, $notifications) {

      return {
        templateUrl: '/assets/templates/requests/actions.html',
        restrict: 'E',

        scope: {
          request: '='
        },

        link: function ($scope) {
          var canceler = $q.defer();

          $session.get('activity').name = 'actions';

          $scope.return = function () {
            $http.put('/api/requests/return', {
              id: $routeParams.id
            }, {
              timeout: canceler.promise
            }).then(function success() {
              $notifications.notify.request.returned({
                specialistWorkplaceId: $scope.request.specialistWorkplaceId,
                specialistUserId: $scope.request.specialistUserId,
                requestId: $scope.request.id,
              });
            }, function error(res) {
              if (res.status > 0) {
                $session.flash('danger', "La solicitud no pudo ser retornada");
              }
            });
          };

          $scope.close = function () {
            $http.put('/api/requests/close', {
              id: $routeParams.id
            }, {
              timeout: canceler.promise
            }).then(function success() {
              $notifications.notify.request.closed($routeParams.id);
            }, function error(res) {
              if (res.status > 0) {
                $session.flash('danger', "La solicitud no pudo ser cerrada");
              }
            });
          };

          $scope.$on('$destroy', function () {
            canceler.resolve();
          });
        }

      };

    }
  ]);

}(angular));
