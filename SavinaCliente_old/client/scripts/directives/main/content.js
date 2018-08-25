(function (ng) {
  'use strict';

  ng.module('App').directive('mainContent', [
    function () {
      return {
        link: function ($scope, $element) {
          $scope.$on('$routeChangeStart', function () {
            $element.addClass('hidden');
          });

          $scope.$on('$routeChangeSuccess', function () {
            $element.removeClass('hidden');
          });
        }
      };
    }
  ]);

}(angular));
