(function (ng) {
  'use strict';

  ng.module('App').controller('Users:Recover', [
    '$scope', '$http', '$location', '$session', '$q',

    function ($scope, $http, $location, $session, $q) {
      var canceler = $q.defer();

      $scope.submitting = false;

      $scope.submit = function () {
        $scope.submitting = true;

        $http.post('/api/users/recover', {
          email: $scope.form.email
        }, {
          timeout: canceler.promise
        }).then(function success() {
          $session.flash('success', "Te hemos enviado un correo con las instrucciones para restablecer tu clave");
          $location.path('/');
        }, function error(res) {
          if (res.status > 0) {
            if (res.status === 400) {
              return $session.flash('warning', "Al parecer el correo no está registrado.");
            }

            $session.flash('danger', "Alguien se enojó contigo...");
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
