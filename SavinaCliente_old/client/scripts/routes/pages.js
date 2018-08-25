(function (ng) {
  'use strict';

  ng.module('App').config([
    '$routeProvider',

    function ($routeProvider) {

      $routeProvider.

      when('/', {
        templateUrl: '/assets/templates/pages/home.html',
        controller: 'Pages:Home'
      }).

      when('/help', {
        templateUrl: '/assets/templates/pages/help.html',
        controller: 'Pages:Help'
      }).

      when('/links/:hash', {
        templateUrl: '/assets/templates/pages/links.html',
        controller: 'Pages:Links'
      }).

      when('/workplace', {
        templateUrl: '/assets/templates/pages/workplace.html',
        controller: 'Pages:Workplace',

        auth: {
          requires: ['user', 'user workplace'],
          redirect: '/'
        },

        resolve: {
          workplaces: [
            '$http',

            function ($http) {
              return $http.get('/api/workplaces/own');
            }
          ]
        }
      }).

      when('/create', {
        templateUrl: '/assets/templates/pages/create.html',
        controller: 'Pages:Create',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        },

        resolve: {
          requestTypes: [
            '$http',

            function ($http) {
              return $http.get('/api/request-types');
            }
          ]
        }
      })

      ;

    }

  ]);

}(angular));
