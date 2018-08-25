(function (ng) {
  'use strict';

  ng.module('App').filter('icon', function () {

    return function (body) {
      if (typeof body === 'string') {
        return 'comment';
      }

      return 'clipboard';
    };

  });

}(angular));
