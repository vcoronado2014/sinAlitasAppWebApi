(function (ng) {
  'use strict';

  ng.module('App').config([
    '$routeProvider',

    function ($routeProvider) {

      $routeProvider.

      when('/users/recover', {
        templateUrl: '/assets/templates/users/recover.html',
        controller: 'Users:Recover'
      }).

      when('/users/reset/:secret', {
        templateUrl: '/assets/templates/users/reset.html',
        controller: 'Users:Reset'
      }).

      when('/users/profile', {
        templateUrl: '/assets/templates/users/profile.html',
        controller: 'Users:Profile',

        auth: {
          requires: ['user', 'user workplace'],
          redirect: '/'
        }
      }).

      when('/users/counterparts', {
        templateUrl: '/assets/templates/users/counterparts.html',
        controller: 'Users:Counterparts',

        auth: {
          requires: ['user', 'user workplace'],
          redirect: '/'
        },
      })

      ;

    }

  ]);

}(angular));
