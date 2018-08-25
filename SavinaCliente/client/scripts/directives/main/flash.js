(function (ng) {
  'use strict';

  ng.module('App').directive('mainFlash', [
    function () {
      return {
        templateUrl: '/assets/templates/main/flash.html',
        restrict: 'E',

        scope: false,

        link: function ($scope) {
          $scope.icon = function (type) {
            switch (type) {
              case 'danger':
                return 'fa-exclamation-triangle';

              case 'warning':
                return 'fa-exclamation-circle';

              case 'info':
                return 'fa-info-circle';

              case 'success':
                return 'fa-check-circle';

              default:
                return 'fa-circle';
            }
          };
        }
      };
    }
  ]);

}(angular));
