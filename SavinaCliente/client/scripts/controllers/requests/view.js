(function (ng) {
  'use strict';

  ng.module('App').controller('Requests:View', [
    '$scope', '$http', '$routeParams', '$session', '$q', '$location', '$notifications',

    function ($scope, $http, $routeParams, $session, $q, $location, $notifications) {
      var canceler = $q.defer();

      $scope.generating = false;

      $scope.fetching = {
        request: true,
        patient: true
      };

      $http.get('/api/requests/by/id/' + $routeParams.id, {
        timeout: canceler.promise
      }).then(function success(res) {
        if (res.status === 204) {
          return $session.flash('danger', "Al parecer la solicitud ya no existe.");
        }

        $scope.request = res.data;

        $http.get('/api/patients/info/by/run/' + $scope.request.patient.run, {
          timeout: canceler.promise
        }).then(function success(res) {
          $scope.request.patient.info = res.data.ObtenerInformacionPacienteRestResult;
        }).then(function complete() {
          $scope.fetching.patient = false;
        });

        if ($scope.request.creatorUserId !== $session.user('id') && $scope.request.closedAt && $scope.request.closedNotification && !$scope.request.closedNotification.seenAt) {
          $http.put('/api/notifications/request/closed', {
            requestId: $scope.request.id
          }, {
            timeout: canceler.promise
          }).then(function success(res) {
            $notifications.requests.closed.update($scope.request.requestType.slug);
            $scope.request.closedNotification.seenAt = res.data.seenAt;
          });
        }
      }, function error(res) {
        if (res.status > 0) {
          $session.flash('danger', "Ocurrio un problema al obtener la solicitud");
        }
      }).then(function complete() {
        $scope.fetching.request = false;
      });

      $scope.view = function (id) {
        $location.path('/requests/attachments/' + id);
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });
    }
  ]);

}(angular));
