(function (ng) {
  'use strict';

  ng.module('App').

  config([
    '$routeProvider', '$locationProvider',

    function ($routeProvider, $locationProvider) {
      /** Not found route **/
      $routeProvider.

      otherwise({
        templateUrl: '/assets/templates/notfound.html'
      })

      ;

      $locationProvider.html5Mode(true);
    }
  ]).

  constant('NOTIFY_TIMEOUT', 3000).

  run([
    '$rootScope', '$location', '$session', '$http', '$moment',

    function ($rootScope, $location, $session, $http, $moment) {
      $rootScope.app = window.app;

      /* Initialize the session */
      $session.init({
        url: '/api/session'
      });

      /* Get user preferences */
      $http.get('/api/preferences/own').then(function success(res) {
        $session.set('preferences', res.data);
      });

      /* Calculate server time difference */
      var start = Date.now();

      $http.get('/api/time').then(function success(res) {
        var now = Date.now();
        var time = Number(res.data) + (now - start);
        var offset = now - time;

        $moment.setServerOffset(offset);
      });
    }
  ]);

}(angular));
