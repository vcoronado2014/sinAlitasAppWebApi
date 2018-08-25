(function (ng) {
  'use strict';

  ng.module('App').directive('worktableSwitcher', [
    '$session', '$notifications', '$timeout',

    function ($session, $notifications, $timeout) {
      return {
        templateUrl: '/assets/templates/requests/worktable/switcher.html',
        restrict: 'E',

        scope: {},

        link: function ($scope) {
          function update() {
            if ($notifications.requests) {
              if (ng.isObject($notifications.requests[$session.get('activity').name])) {
                $scope.notifications = $notifications.requests[$session.get('activity').name];
              }
            }

            $timeout();
          }

          $scope.$on($notifications.REQUESTS_UPDATED, update);

          update();
        }
      };
    }
  ]);

}(angular));
