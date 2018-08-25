(function (ng) {
  'use strict';

  ng.module('App').config([
    '$routeProvider',

    function ($routeProvider) {

      $routeProvider.

      when('/requests/create/:type', {
        templateUrl: '/assets/templates/requests/create.html',
        controller: 'Requests:Create',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        },

        resolve: {
          related: [
            '$q', '$http',

            function ($q, $http) {
              return $q.all({
                specialties: $http.get('/api/specialties/for/current/workplace'),
                requestTypes: $http.get('/api/request-types'),
                priorities: $http.get('/api/priorities'),
                genders: $http.get('/api/genders'),
                motives: $http.get('/api/motives')
              });
            }
          ]
        }
      }).

      when('/requests/:id', {
        templateUrl: '/assets/templates/requests/view.html',
        controller: 'Requests:View',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      }).

      when('/requests/take/:id', {
        templateUrl: '/assets/templates/requests/take.html',
        controller: 'Requests:Take',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      }).

      when('/requests/room/:id', {
        templateUrl: '/assets/templates/requests/room.html',
        controller: 'Requests:Room',
        reloadOnSearch: false,

        resolve: {
          request: [
            '$http', '$route',

            function ($http, $route) {
              return $http.get('/api/requests/by/id/' + $route.current.params.id);
            }
          ]
        },

        auth: {
          requires: 'user workplace',
          redirect: '/workplace'
        }
      })

      ;

    }

  ]);

}(angular));
