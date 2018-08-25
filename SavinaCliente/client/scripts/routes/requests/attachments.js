(function (ng) {
  'use strict';

  ng.module('App').config([
    '$routeProvider',

    function ($routeProvider) {

      $routeProvider.when('/requests/attachments/:id', {
        templateUrl: '/assets/templates/requests/view/attachment.html',
        controller: 'Request:Attachments:View',

        auth: {
          requires: 'user workplace',
          redirect: '/'
        }
      });

    }
  ]);

}(angular));
