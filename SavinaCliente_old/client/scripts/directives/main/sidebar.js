(function (ng) {
  'use strict';

  ng.module('App').directive('mainSidebar', [
    '$location', '$notifications', '$timeout',

    function ($location, $notifications, $timeout) {

      return {
        templateUrl: '/assets/templates/main/sidebar.html',

        scope: {},

        link: function ($scope) {
          $scope.notifications = {};

          function update() {
            $timeout(function () {
              $scope.notifications = $notifications.requests;
            }, 1);
          }

          $scope.$on($notifications.REQUESTS_UPDATED, update);

          $scope.$on('$routeChangeStart', function () {
            $scope.location = $location.path();
          });

          update();
        }
      };
    }
  ]);

}(angular));
