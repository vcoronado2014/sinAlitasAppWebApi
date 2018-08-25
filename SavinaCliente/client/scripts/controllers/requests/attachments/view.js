(function (ng) {
  'use strict';

  ng.module('App').controller('Request:Attachments:View', [
    '$scope', '$routeParams', '$location', '$session', '$q', '$http', '$window',

    function ($scope, $routeParams, $location, $session, $q, $http) {
      var canceler = $q.defer();

      $session.get('activity').name = 'attachments-view';

      $scope.fetch = function () {
        $scope.fetching = true;
        $scope.errored = false;

        $http.get('/api/requests/attachments/by/id/' + $routeParams.id, {
          timeout: canceler.promise
        }).then(function success(res) {
          $scope.attachment = res.data;
        }, function error(res) {
          if (res.status > 0) {
            if (res.status === 403) {
              $session.flash('danger', "Ese adjunto no pertenece a una de tus solicitudes!");
              $location.path('/requests/worktable/ongoing/consultation');
            } else {
              $session.flash('danger', "No se pudo obtener el adjunto!");
              $scope.errored = true;
            }
          }
        }).then(function complete() {
          $scope.fetching = false;
        });
      };

      $scope.back = function () {
        if ($scope.attachment) {
          $location.path('/requests/' + $scope.attachment.requestId);
        }
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });

      $scope.fetch();
    }

  ]);

}(angular));
