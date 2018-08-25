(function (ng, window) {
  'use strict';

  var serverOffset = 0;

  ng.module('Moment', []).factory('$moment', function () {

    window.moment.locale('es');

    window.moment.setServerOffset = function (offset) {
      serverOffset = offset;
    };

    return window.moment;

  }).

  filter('formatUTC', [
    '$moment', 

    function ($moment) {
      return function (date, format) {
          return $moment.utc(date).format(format);
      };
    }
  ]).

  filter('fromNow', [
    '$moment',

    function ($moment) {
      return function (date, strip) {
        var now = Date.now() - serverOffset;

        if (now < date) {
          now = new Date();
        }

        return $moment(date).from(now, strip);
      };
    }

  ]).

  filter('calendar', [
    '$moment',

    function ($moment) {
      return function (date) {
        return $moment(date).calendar();
      };
    }

  ]).

  filter('toLocal', [
    '$moment',

    function ($moment) {
      return function (date) {
        return $moment(date).format('L LT');
      };
    }

  ]).

  filter('age', [
    '$moment',

    function ($moment) {
      return function (birthdate) {
        return $moment().diff($moment(birthdate), 'years');
      };
    }
  ]).

  filter('ageServerDetallado', [
    '$moment', '$http',

    function ($moment, $http) {
      return function (birthdate, fechaHoraServidor) {
        var retorno = "0 horas";
          var duration = $moment.duration(moment().diff($moment(birthdate)));
          if (fechaHoraServidor)
              duration = $moment.duration($moment(fechaHoraServidor).diff($moment(birthdate)));
          
          if (duration._data)
          {
            if (duration._data.years > 0)
                retorno = duration._data.years + ' años, ' + duration._data.months + ' meses';
            else if (duration._data.years == 0 && duration._data.months > 0)
                retorno = duration._data.months + ' meses, ' + duration._data.days + ' días';
            else if (duration._data.years == 0 && duration._data.months == 0 && duration._data.days > 0)
                retorno = duration._data.days + ' días, ' + duration._data.hours + ' horas';
            else if (duration._data.years == 0 && duration._data.months == 0 && duration._data.days == 0 && duration._data.hours > 0)
                retorno = duration._data.hours + ' horas';
            else if (duration._data.years == 0 && duration._data.months == 0 && duration._data.days == 0 && duration._data.hours == 0 && duration._data.minutes > 0)
                retorno = duration._data.minutes + ' minutos';              
            else
                retorno = '0 horas';

          }

          return retorno;
      };
    }
  ]).

  filter('ageServer', [
    '$moment', '$http',

    function ($moment, $http) {
      return function (birthdate) {
        var fechaServidor;
        var retorno = $moment().diff($moment(birthdate), 'years');
        var obtenerFecha = jQuery.ajax({
          url : '/api/date',
          type: 'GET',
          timeout: 9000
        });
        $.when(obtenerFecha).then(
          function success(res){
              if (res) {
                  fechaServidor =  $moment(res);
                  retorno = fechaServidor.diff($moment(birthdate), 'years');
                  return retorno;                  
              } 
          },
          function error(data, status){
            retorno = $moment().diff($moment(birthdate), 'years');
            return retorno;
          }

        );

        return retorno;
        
      };
    }
  ]);

}(angular, window));
