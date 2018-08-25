(function (ng) {
  'use strict';

  ng.module('App').controller('Users:Profile', [
    '$scope', '$http', '$location', '$session', '$q',

    function ($scope, $http, $location, $session, $q) {
      var canceler = $q.defer();

      var options = {
        timeout: canceler.promise
      };

      $scope.fetching = {
        preferences: true,
        user: true
      };

      $scope.updating = {
        preferences: false
      };

      $http.get('/api/users/me', options).then(function success(res) {
        $scope.user = res.data;

        $http.get('/api/preferences/own', options).then(function success(res) {
          if (res.data) {
            $session.set('preferences', res.data);
            $scope.preferences = res.data;
          }
        }, function error(res) {
          if (res.status > 0) {
            $session.flash('danger', "Oh oh... No se pudo obtener tus preferencias...");
          }
        }).then(function complete() {
          $scope.fetching.preferences = false;
        });
      }, function error(res) {
        if (res.status > 0) {
          $session.flash('danger', "Oh oh... No se pudo obtener tu información...");
        }
      }).then(function complete() {
        $scope.fetching.user = false;
      });

      $scope.update = {
        preferences: function updatePreferences() {
          $scope.updating.preferences = true;

          $http.put('/api/preferences', $scope.preferences, options).then(null, function error(res) {
            if (res.status > 0) {
              $session.flash('danger', "Oh no... tus preferencias no puedieron ser actualizadas...");
            }
          }).then(function complete() {
            $scope.updating.preferences = false;
            $session.set('preferences', $scope.preferences);
          });
        }
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });

    }
  ]);

}(angular));
