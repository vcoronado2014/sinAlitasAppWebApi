(function (ng) {
  'use strict';

  ng.module('App').controller('Users:Reset', [
    '$scope', '$http', '$location', '$routeParams', '$session', '$q',

    function ($scope, $http, $location, $routeParams, $session, $q) {
      var canceler = $q.defer();

      $scope.submitting = false;
      $scope.validating = true;

      $http.get('/api/tokens/exists/' + $routeParams.secret, {
        timeout: canceler.promise
      }).then(null, function error(res) {
        if (res.status > 0) {
          $session.flash('warning', "Debes solicitar un nuevo cambio de clave");
          $location.path('/users/recover');
        }
      }).then(function complete() {
        $scope.validating = false;
      });

      $scope.submit = function () {
        $scope.submitting = true;

        $http.post('/api/users/reset/' + $routeParams.secret, {
          password: $scope.form.password
        }, {
          timeout: canceler.promise
        }).then(function success() {
          $session.flash('success', "Por favor ingresa con tu nueva clave");
          $location.path('/');
        }, function error(res) {
          if (res.status > 0) {
            if (res.status === 400) {
              $session.flash('danger', "Por favor solicita un nuevo cambio de clave");
              $location.path('/users/recover');
            } else if (res.status === 409) {
              $session.flash('danger', "La cuenta asociada no existe o fué desactivada");
              $location.path('/');
            } else {
              $session.flash('danger', "Algo malo pasó al procesar esto...");
            }
          }
        }).then(function complete() {
          $scope.submitting = false;
        });
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });
    }
  ]);

}(angular));
