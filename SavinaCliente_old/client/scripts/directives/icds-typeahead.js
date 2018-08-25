(function (ng) {
  'use strict';

  ng.module('App').directive('icdsTypeahead', [
    '$http', '$timeout', '$q', '$parse',

    function ($http, $timeout, $q, $parse) {

      return {
        restrict: 'E',

        templateUrl: function ($element, $attrs) {
          return $attrs.taTemplate;
        },

        link: function ($scope, $element, $attrs) {
          var getter = $parse($attrs.taName);
          var canceler = $q.defer();
          var timeout, list;

          if ($attrs.taList) {
            list = $parse($attrs.taList);
          }

          getter.assign($scope, {
            fetching: false,
            fetched: false,
            results: [],
            error: 0,

            clear: function (input) {
              if (input) {
                $element.find('input').val(null);
              }

              getter($scope).fetched = false;
              getter($scope).results = null;
            },

            add: function (result) {
              if (list($scope)) {
                if (!ng.isArray(list($scope))) {
                  list.assign($scope, []);
                }

                list($scope).push(result);

                this.clear(true);
              }
            }
          });

          $element.on('keyup', function () {
            var filter = $element.find('input').val();

            if (timeout) {
              $timeout.cancel(timeout);
            }

            if (filter) {
              timeout = $timeout(function fetch() {
                getter($scope).fetching = true;
                getter($scope).clear();

                var params = {
                  limit: parseInt($attrs.taLimit),
                  filter: filter
                };

                if ($attrs.taNoDups === 'true') {
                  if (ng.isArray(list($scope)) && list($scope).length) {
                    params.not = [];

                    ng.forEach(list($scope), function (icd) {
                      params.not.push(icd.id);
                    });
                  }
                }

                $http.get('/api/icds', {
                  timeout: canceler.promise,
                  params: params
                }).then(function success(res) {
                  getter($scope).results = res.data;
                }, function error(res) {
                  if (res.status > 0) {
                    getter($scope).error = status;
                  }
                }).then(function complete() {
                  getter($scope).fetching = false;
                  getter($scope).fetched = true;
                });

              }, parseInt($attrs.taDelay));
            }
          });
        }
      };
    }
  ]);

}(angular));
