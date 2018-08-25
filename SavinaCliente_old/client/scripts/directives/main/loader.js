(function (ng) {
  'use strict';

  ng.module('App').directive('mainLoader', [
    '$rootScope',

    function ($rootScope) {
      return {
        link: function ($scope,$element) {
          $element.find('.hidden').removeClass('hidden');

          $rootScope.$on('$locationChangeStart', function () {
            $rootScope.loading = true;
          });

          $rootScope.$on('$routeChangeSuccess', function () {
            $rootScope.loading = false;
          });

          $rootScope.$on('$routeChangeError', function () {
            $scope.errored = true;
          });
        }
      };
    }
  ]);

}(angular));
