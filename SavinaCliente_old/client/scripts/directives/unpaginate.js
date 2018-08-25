(function (ng) {
  'use strict';

  ng.module('App').directive('unpaginate', [

    function () {
      return {
        restrict: 'A',

        scope: {
          onBottom: '=',
          onTop: '='
        },

        link: function ($scope, $element) {
          console.log($scope);

          if ($scope.onTop && !ng.isFunction($scope.onTop)) {
            throw new Error("The 'on-top' attribute must reference a [Function]");
          }

          if ($scope.onBottom && !ng.isFunction($scope.onBottom)) {
            throw new Error("The 'on-bottom' attribute must reference a [Function]");
          }

          function onscroll() {
            var sh = $element[0].scrollHeight;
            var ih = $element.innerHeight();
            var st = $element.scrollTop();

            if (st === 0 && $scope.onTop) {
              $scope.onTop();
            } else if (st + ih >= sh && $scope.onBottom) {
              $scope.onBottom();
            }
          }

          if ($scope.onTop || $scope.onBottom) {
            $element.on('scroll', onscroll);
          }
        }
      };
    }

  ]);

}(angular));
