(function (ng) {
  'use strict';

  ng.module('App').directive('faStatus', [
    '$parse',

    function ($parse) {
      return {
        restrict: 'A',

        link: function ($scope, $element, $attrs) {
          var model = $parse($attrs.faStatus);
          var base = $attrs.class || '';

          $scope.$watch(model, function () {
            var $model = $parse($attrs.faStatus)($scope);

            if (!$model) {
              return;
            }

            if ($model.hasOwnProperty('$modelValue')) {
              $scope.$watchGroup([
                $attrs.faStatus + '.$modelValue',
                $attrs.faStatus + '.$valid'
              ], function () {
                if ($model.$modelValue) {
                  if ($model.$valid) {
                    $element.attr('class', base + ' fa-check-circle');
                  } else {
                    $element.attr('class', base + ' fa-times-circle');
                  }
                } else {
                  if ($model.$error.required) {
                    $element.attr('class', base + ' fa-dot-circle-o');
                  } else {
                    $element.attr('class', base + ' fa-circle-o');
                  }
                }
              });
            }

            if ($model.hasOwnProperty('$submitted')) {
              $element.attr('class', base + ' fa-circle-o');

              $scope.$watchGroup([
                $attrs.faStatus + '.$valid',
                $attrs.faStatus + '.$dirty'
              ], function () {
                if ($model.$dirty) {
                  if ($model.$valid) {
                    $element.attr('class', base + ' fa-check-circle');
                  } else {
                    $element.attr('class', base + ' fa-times-circle');
                  }
                }
              });
            }
          });
        }
      };
    }

  ]);

}(angular));
