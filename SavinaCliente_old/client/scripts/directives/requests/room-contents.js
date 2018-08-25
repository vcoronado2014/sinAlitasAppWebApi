(function (ng) {
  'use strict';

  ng.module('App').directive('requestRoomContents', [
    '$rootScope', '$compile', '$location',

    function ($rootScope, $compile, $location) {

      return {
        restrict: 'E',

        $scope: {
          request: '='
        },

        link: function ($scope, $element) {

          var dscope = null;

          $scope.$watch(function () {
            return $location.hash();
          }, function () {
            if (dscope) {
              dscope.$destroy();
            }

            dscope = $scope.$new();

            var content = ng.element('<request-' + $location.hash() + ' request="request" class="block"></request-' + $location.hash() + '>');

            $element.empty().append($compile(content)(dscope));
          });

          $scope.$on('$locationChangeSuccess', function () {
            $rootScope.loading = false;
          });
        }
      };

    }

  ]);

}(angular));
