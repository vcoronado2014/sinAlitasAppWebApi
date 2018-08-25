(function (ng) {
  'use strict';

  ng.module('App').controller('Users:Counterparts', [
    '$scope', '$presence', '$http', '$q', '$session', '$timeout', '$window',

    function ($scope, $presence, $http, $q, $session, $timeout, $window) {
      var canceler = $q.defer();

      $scope.specialists = [];
      $scope.fetching = false;
      $scope.fetched = false;

      $scope.back = function () {
        $window.history.back();
      };

      $scope.fetch = function () {
        $scope.fetching = true;

        $http.get('/api/users/counterparts', {
          params: {
            ids: $presence.users
          }
        }, {
          timeout: canceler.promise
        }).then(function success(res) {
          $scope.users = res.data;
        }, function error(res) {
          if (res.status > 0) {
            $session.flash('danger', "No pudimos obtener los usuarios conectados");
          }
        }).then(function complete() {
          $scope.fetching = false;
          $scope.fetched = true;
        });
      };

      $scope.$on($presence.USERS_UPDATED, function () {
        $timeout(function () {
          if ($scope.fetching) {
            canceler.resolve();
            canceler = $q.defer();
          }

          $scope.fetch();
        }, 1);
      });

      $scope.$on('$routeChangeStart', function () {
        canceler.resolve();
      });

      $scope.fetch();
    }
  ]);

}(angular));
