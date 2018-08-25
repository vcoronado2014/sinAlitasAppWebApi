(function (ng) {
  'use strict';

  ng.module('App').controller('Requests:Take', [
    '$scope', '$http', '$routeParams', '$location', '$session', '$q', '$notifications',

    function ($scope, $http, $routeParams, $location, $session, $q, $notifications) {
      var canceler = $q.defer();

      $scope.fetching = true;
      $scope.taking = false;

      $http.get('/api/requests/brief/' + $routeParams.id, {
        timeout: canceler.promise
      }).then(function success(res) {
        $scope.request = res.data;
      }, function error(res) {
        if (res.status > 0) {
          $session.flash('danger', "Ocurrió un problema al obtener esa solicitud");
          $location.path('/requests/worktable/waiting/' + ($session.get('activity').request || 'consultation'));
        }
      }).then(function complete() {
        $scope.fetching = false;
      });

      $scope.proceed = function () {
        $scope.taking = true;

        $http.put('/api/requests/take/', {
          id: $scope.request.id
        }, {
          timeout: canceler
        }).then(function success() {
          $notifications.notify.request.taken($routeParams.id);
          $location.path('/requests/room/' + $routeParams.id);
        }, function error(res) {
          if (res.status > 0) {
            $session.flash('danger', "No se te pudo asignar esta solicitud");
          }
        }).then(function complete() {
          $scope.taking = false;
        });
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });
    }
  ]);

}(angular));
