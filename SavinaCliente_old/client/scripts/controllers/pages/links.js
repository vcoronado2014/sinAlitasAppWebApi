(function (ng) {
  'use strict';

  ng.module('App').controller('Pages:Links', [
    '$scope', '$http', '$location', '$session', '$q', '$routeParams',

    function ($scope, $http, $location, $session, $q, $routeParams) {
      var canceler = $q.defer();
      var params = {
        timeout: canceler.promise
      };

      $scope.submitting = false;
      $scope.fetching = true;
      $scope.expired = false;
      $scope.error = false;
      $scope.link = null;
      $scope.data = {};

      $http.get('/api/links/validate/' + $routeParams.hash, params).then(null, function error(res) {
        if (res.status > 0) {
          if (res.status === 412) {
            $scope.expired = true;
          } else if (res.status === 400) {
            $scope.error = true;
          }
        }
      }).then(function complete() {
        $scope.fetching = false;
      });

      $scope.signin = function () {
        $scope.submitting = true;

        $session.flash();

        $http.post('/api/users/signin', {
          email: $scope.data.email,
          password: $scope.data.password
        }, params).then(function success(res) {
          $session.signin(res.data);

          $http.get('/api/links/' + $routeParams.hash, params).then(function success(res) {
            $http.put('/api/workplaces/active', {
              id: res.data.workplaceId
            }, params).then(function success() {
              $location.url(res.data.url);
            }, function error(res) {
              if (res.status > 0) {
                $session.flash('danger', "Al parecer no se puede ver este enlace.");
              }
            }).then(function complete() {
              $scope.setting = false;
            });
          }, function error(res) {
            if (res.status > 0) {
              if (res.status === 403) {
                $session.flash('danger', "No estás autorizado para ver el enlace.");
              } else {
                $session.flash('danger', "No se pudo obtener el enlace.");
              }
            }
          }).then(function complete() {
            $scope.submitting = false;
          });
        }, function error(res) {
          if (res.status > 0) {
            if (res.status === 401) {
              $session.flash('warning', "Email o clave equivocados.");
            } else {
              $session.flash('danger', "No se pudo iniciar sesión.");
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
