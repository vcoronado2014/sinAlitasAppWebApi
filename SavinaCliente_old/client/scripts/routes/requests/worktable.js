(function (ng) {
  'use strict';

  ng.module('App').config([
    '$routeProvider',

    function ($routeProvider) {

      $routeProvider.

      when('/requests/worktable/sent/:type', {
        templateUrl: '/assets/templates/requests/worktable/sent.html',
        controller: 'Requests:Worktable:Sent',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      }).

      when('/requests/worktable/waiting/:type', {
        templateUrl: '/assets/templates/requests/worktable/waiting.html',
        controller: 'Requests:Worktable:Waiting',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      }).

      when('/requests/worktable/ongoing/:type', {
        templateUrl: '/assets/templates/requests/worktable/ongoing.html',
        controller: 'Requests:Worktable:Ongoing',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      }).

      when('/requests/worktable/closed/:type', {
        templateUrl: '/assets/templates/requests/worktable/closed.html',
        controller: 'Requests:Worktable:Closed',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      })

      ;

    }

  ]);

}(angular));
