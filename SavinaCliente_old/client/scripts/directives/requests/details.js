(function (ng) {
  'use strict';

  ng.module('App').directive('requestDetails', [
    '$http', '$routeParams', '$session', '$q',

    function ($http, $routeParams, $session, $q) {

      return {
        templateUrl: '/assets/templates/requests/details.html',

        restrict: 'E',

        scope: {
          request: '='
        },

        link: function ($scope) {
          var canceler = $q.defer();

          $session.get('activity').name = 'details';

          $scope.fetching = {
            request: false,
            patient: true
          };

          $scope.tab = 1;

          $http.get('/api/patients/info/by/run/' + $scope.request.patient.run, {
            timeout: canceler.promise
          }).then(function success(res) {
            $scope.request.patient.info = res.data && res.data.ObtenerInformacionPacienteRestResult;
          }, function error(res) {
            if (res.status > 0) {
              $session.flash('danger', "No se pudo obtener la información del paciente");
            }
          }).then(function complete() {
            $scope.fetching.patient = false;
          });

          $scope.$on('$destroy', function () {
            canceler.resolve();
          });

        }

      };

    }

  ]);

}(angular));
