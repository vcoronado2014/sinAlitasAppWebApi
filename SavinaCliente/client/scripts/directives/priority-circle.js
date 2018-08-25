(function (ng) {
  'use strict';

  ng.module('App').directive('priorityCircle', [
    '$moment', '$http',

    function ($moment, $http) {
      return {
        templateUrl: '/assets/templates/priority-circle.html',
        restrict: 'E',

        scope: {
          priority: '=',
          created: '=',
          dateCreate: '='
        },

        link: function ($scope) {
          //fecha de cración
          var fechaCreacion = $moment($scope.created);
          //$scope.date = fechaCreacion.from(request.fechaServidor).split(/\s/);
          $scope.date = $scope.$parent.dateCreate;
          $scope.transparent = 'rgba(255, 255, 255, 0.3)';

          /*
          var fechaServidor;
            $http.get('/api/date', {
                          timeout: 9000
                        }).then(function success(res) {
                          if (res.status === 200) {
                            //var obj1 = JSON.stringify(res.data[0]);
                            //var localTime  = moment.utc($.parseJSON(obj1)[0].fecha).toDate();
                            //localTime = moment(localTime).format('YYYY-MM-DD HH:mm:ss');
                            fechaServidor =  $moment(res.data);
                            $scope.date = fechaCreacion.from(fechaServidor).split(/\s/);
                            $scope.transparent = 'rgba(255, 255, 255, 0.3)';
                            return;
                          } 
                        }, function error(data, status) {
                          if (status > 0) {
                            $session.flash('danger', "No se pudo obtener fecha Servidor!");
                            //dejamos en el scope la fecha hora local
                            $scope.date = fechaCreacion.fromNow(true).split(/\s/);
                            $scope.transparent = 'rgba(255, 255, 255, 0.3)';
                          }
                        }).then(function complete() {
                          self.fetching = false;
                          self.fetched = true;

                        });

          */
        }
      };

    }
  ]);

}(angular));
