(function (ng) {
  'use strict';

  ng.module('App').directive('mainNavbar', [
    '$location', '$http', '$session', '$notifications', '$timeout', '$presence',

    function ($location, $http, $session, $notifications, $timeout, $presence) {

      return {
        templateUrl: '/assets/templates/main/navbar.html',
        restrict: 'E',

        scope: {},

        link: function ($scope) {
          $scope.presenceUsers = 0;

          $scope.signout = function () {
            $http.get('/api/users/signout').then(function success() {
              $session.signout();
              $location.path('/');
            });
          };

          $scope.$on($presence.USERS_UPDATED, function (event, users) {
            $timeout(function () {
              $scope.presenceUsers = users.length;
            }, 1);
          });

          $scope.$on($notifications.WORKPLACES_UPDATED, function () {
            $timeout(function () {
              $scope.workplaceNotifications = $notifications.workplaces.all;

              // if ($session.get('workplace')) {
              //   var wpid = $session.get('workplace').id;
              //
              //
              //   ng.forEach($notifications.workplaces.list, function (count, id) {
              //     if (id !== wpid && count && !$scope.workplaceNotifications) {
              //       $scope.workplaceNotifications = true;
              //     }
              //   });
              // }
            }, 1);
          });
        }
      };
    }
  ]);

}(angular));
