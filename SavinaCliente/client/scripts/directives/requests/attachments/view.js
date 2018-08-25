(function (ng) {
  'use strict';

  ng.module('App').directive('requestAttachmentsView', [
    '$location', '$session', '$http', '$routeParams', '$mimeicon', '$q',

    function ($location, $session, $http, $routeParams, $mimeicon, $q) {

      return {
        templateUrl: '/assets/templates/requests/attachments/view.html',

        restrict: 'E',

        scope: {},

        link: function ($scope) {
          var canceler = $q.defer();

          $session.get('activity').name = 'attachments';

          $scope.fetch = function () {
            $scope.fetching = true;

            $http.get('/api/requests/attachments/by/id/' + $location.search().id, {
              timeout: canceler.promise
            }).then(function success(res) {
              $scope.attachment = res.data;
            }, function error(res) {
              if (res.status > 0) {
                $scope.errored = true;
              }
            }).then(function complete() {
              $scope.fetching = false;
            });
          };

          $scope.$on('$destroy', function () {
            $location.search('id', null);
            canceler.resolve();
          });

          $scope.fetch();
        }
      };

    }

  ]);

}(angular));
