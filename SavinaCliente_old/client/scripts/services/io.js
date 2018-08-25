(function (ng, window) {
  'use strict';

  ng.module('App').factory('$io', function () {

    return {

      connect: function (namespace, options) {
        return window.io(window.location.origin + namespace, options);
      }

    };

  });

}(angular, window));
