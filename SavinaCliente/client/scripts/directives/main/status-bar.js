(function () {
  'use strict';

  angular.module('App').directive('mainStatusbar', [
    function () {

      return {
        templateUrl: '/assets/templates/main/statusbar.html'
      };
    }
  ]);

}());
