(function (ng) {
  'use strict';

  ng.module('App').directive('includeTemplate', [

    function () {
      return {
        restrict: 'E',

        templateUrl: function ($element, $attrs) {
          return $attrs.ngSrc;
        }
      };

    }
  ]);

}(angular));
