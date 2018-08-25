(function (ng) {
  'use strict';

  ng.module('App').controller('Pages:Home', [
    '$scope', '$http', '$location', '$session', '$q', '$notifications',

    function ($scope, $http, $location, $session, $q, $notifications) {
      var canceler = $q.defer();

      $notifications.leave();
      $notifications.init();

      $scope.submitting = false;
      $scope.error = false;

      /* Signout any logged in user */
      $session.signout();

      $scope.signin = function () {
        $scope.submitting = true;
        $session.flash();

        $http.post('/api/users/signin', {
          email: $scope.form.email,
          password: $scope.form.password
        }, {
          timeout: canceler.promise
        }).then(function success(res) {
          $session.signin(res.data);
          $location.path('/workplace');
        }, function error(res) {
          if (res.status > 0) {
            if (res.status === 401) {
              $session.flash('warning', "Email o clave equivocados");
            } else {
              $session.flash('danger', "No se pudo iniciar sesión");
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
