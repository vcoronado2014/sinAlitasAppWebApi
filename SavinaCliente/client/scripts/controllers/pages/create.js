(function (ng) {
  'use strict';

  ng.module('App').controller('Pages:Create', [
    '$scope', '$session', 'requestTypes',

    function ($scope, $session, requestTypes) {
      $session.get('activity').name = 'create';

      $scope.requestTypes = requestTypes.data;
    }
  ]);

}(angular));
