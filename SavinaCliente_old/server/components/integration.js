'use strict';

var debug = require('debug')('app:integration');
var moment = require('moment');
var merge = require('merge');
var https = require('https');
var http = require('http');
var is = require('fi-is');

var utils = component('utils');

var inspect = function (data) {
  return require('util').inspect(data, {
    showHidden: true,
    colors: true,
    depth: null
  });
};

var hostnames = {
  prep: {
    pdi: 'prepplataformadeintegraciones.saludenred.cl',
    wsi: 'prepwsintegraciones.saludenred.cl'
  }
};

/**
 * Builds the query params.
 */
function buildParams(base, options) {
  var params = {};

  params[base] = {
    FechaHoraMensaje: moment().format('YYYYMMDD HH:mm'),
    VersionSoftwareInforma: '1',
    IdSoftwareInforma: '1',
    IdSitioSoftware: '1',
    ExtensionData: null,
    TipoMensaje: '1'
  };

  params = merge.recursive(params, options);

  debug(inspect(params));

  return encodeURIComponent(JSON.stringify(params));
}

/**
 * Base requester function.
 */
function request(protocol, options, callback) {
  debug("Request start");
  debug(options);

  var req = protocol.get(options, function (response) {
    var buffer = [];

    response.on('data', function (chunk) {
      debug("Received data");
      buffer = buffer.concat(chunk);
    });

    response.on('end', function () {
      var data = buffer.toString('hex');

      debug("Response ended");
      debug(inspect(buffer));
      debug(inspect(data));

      try {
        data = JSON.parse(data);
      } catch (ex) {
        debug("Response is not JSON");

        return callback(ex);
      }

      callback(null, data);
    });
  });

  req.on('error', function (err) {
    debug("Integration service request error!");
    debug(inspect(err));

    callback(err);
  });

  req.end();
}

/**
 * Authorizes a patient by it's RUN number.
 */
module.exports.auth = function auth(run, pass, callback) {
  var params = buildParams('ParametroConsulta', {
    Identificacion: {
      NumeroIdentificacion: run,
      TipoIdentificacion: '1',
      Clave: pass,

      ParametroConsulta: {
        CodigoEstablecimientoConsulta: null
      }
    }
  });

  var options = {
    hostname: hostnames.prep.wsi,
    path: '/WSAutentificacionTT/Autentificacion.svc/ObtenerAutentificacionRest?ParametroEntrada=' + params
  };

  request(https, options, function (err, res) {
    if (err) {
      return callback(err);
    }

    if (!res.RespuestaBase) {
      return callback(new Error("Response was empty..."));
    }

    if (res.RespuestaBase.Estatus) {
      return callback(new Error(res.RespuestaBase.Descripcion));
    }

    callback(null, res.Token, {
      estcode: res.CodigoEstablecimiento,
      ryfid: res.IdentificadorPaciente,
      authorized: res.EstaAutorizado
    });
  });
};

/**
 * Get patient's basic data.
 */
module.exports.patient = function getPatient(ryfid, estcode, token, callback) {
  var params = buildParams('ParametroBaseConsulta', {
    IdPaciente: ryfid,
    Token: token,

    ParametroBaseConsulta: {
      CodigoEstablecimientoConsulta: estcode
    }
  });

  var options = {
    hostname: hostnames.prep.wsi,
    path: '/WSPacienteMS/Paciente.svc/ObtenerPacienteRest?ParametroEntrada=' + params
  };

  request(https, options, function (err, res) {
    if (err) {
      return callback(err);
    }

    var result = res.ObtenerPacienteRestResult;

    if (!result) {
      return callback(new Error("Response was empty..."));
    }

    if (result.RespuestaBase.Estatus) {
      return callback(new Error(result.RespuestaBase.Descripcion));
    }

    var patient = result.Paciente;

    callback(null, result.NuevoToken, {
      firstname: utils.format.asName(patient.Nombres),
      lastname: utils.format.asName(patient.PrimerApellido + ' ' + patient.SegundoApellido),

      gender: patient.Sexo,

      birth: new Date(parseInt(patient.FechaDeNacimiento.replace(/\/Date\((\-?\d+?)\-\d+?\)\//, '$1'))),

      parents: {
        father: {
          name: patient.NombrePadre === 'No Informado' ? null : patient.NombrePadre
        },

        mother: {
          name: patient.NombreMadre === 'No Informado' ? null : patient.NombreMadre
        }
      },

      contact: {
        email: {
          allows: patient.Contactibilidad.Email.AdmiteContacto,
          value: parseInt(patient.Contactibilidad.Email.ValorContacto) ? patient.Contactibilidad.Email.ValorContacto : null
        },

        phone: {
          allows: patient.Contactibilidad.TelefonoContacto.AdmiteContacto,
          value: parseInt(patient.Contactibilidad.TelefonoContacto.ValorContacto) ? patient.Contactibilidad.TelefonoContacto.ValorContacto : null
        },

        mobile: {
          allows: patient.Contactibilidad.TelefonoMovil.AdmiteContacto,
          value: parseInt(patient.Contactibilidad.TelefonoMovil.ValorContacto) ? patient.Contactibilidad.TelefonoMovil.ValorContacto : null
        },

        home: {
          allows: patient.Contactibilidad.TelefonoResidencial.AdmiteContacto,
          value: parseInt(patient.Contactibilidad.TelefonoResidencial.ValorContacto) ? patient.Contactibilidad.TelefonoResidencial.ValorContacto : null
        }
      }
    });
  });
};

/**
 * Get patient's detailed information.
 */
module.exports.patientInfo = function getPatientInfo(ryfid, estcode, token, callback) {
  var params = buildParams('ParametroBaseConsulta', {
    IdPaciente: ryfid,
    Token: token,

    ParametroBaseConsulta: {
      CodigoEstablecimientoConsulta: estcode
    }
  });

  var options = {
    hostname: hostnames.prep.wsi,
    path: '/WSPacienteMS/Paciente.svc/ObtenerInformacionPacienteRest?ParametroEntrada=' + params
  };

  request(https, options, function (err, res) {
    if (err) {
      return callback(err);
    }

    var result = res.ObtenerInformacionPacienteRestResult;

    if (!result) {
      return callback(new Error("Response was empty..."));
    }

    if (result.RespuestaBase.Estatus) {
      return callback(new Error(result.RespuestaBase.Descripcion));
    }

    callback(null, result.NuevoToken, {
      allergies: result.Alergias,

      diagnoses: (function () {
        var diagnoses = [];

        if (result.Diagnostico && result.Diagnostico.length) {
          result.Diagnostico.forEach(function (diagnosis) {
            diagnoses.push({
              description: diagnosis.DescripcionDiagnostico,
              code: diagnosis.CodigoDiagnostico
            });
          });
        }

        return diagnoses;
      }()),

      immunizations: result.InmunizacionesAdministradas,

    });
  });
};

/**
 * Get patient's primary history.
 */
module.exports.patientHistoryPrimary = function getPatientHistoryPrimary(run, ryfid, estcode, range, callback) {
  var params = buildParams('ParametroBase', {
    IdPaciente: ryfid,

    FechaInicio: moment(range.start).format('YYYYMMDD'),
    FechaTermino: moment(range.end).format('YYYYMMDD'),

    IdentificacionPaciente: {
      OtraIdentificacion: '1',
      TipoIdentificacion: '1',
      Run: run
    },

    ParametroBase: {
      CodigoEstablecimientoConsulta: estcode
    }
  });

  var options = {
    hostname: hostnames.prep.pdi,
    path: '/FUC_REST/ObtenerResumenHistorialClinicoPrimaria?ParametroFUC=' + params,
    port: 90
  };

  request(http, options, function (err, res) {
    if (err) {
      return callback(err);
    }

    /* Obtain the data of the data that the data dated as data */
    var result = res.ObtenerResumenHistorialClinicoResponse;

    if (!result) {
      return callback(new Error("Response was empty..."));
    }

    if (result.RespuestaBase.Estatus) {
      return callback(new Error(result.RespuestaBase.Descripcion));
    }

    var history = result.ObtenerResumenHistorialClinicoResult.ResumenHistorial.TypeResumenHistorial;

    var data = [];

    if (is.array(history)) {
      history.forEach(function (item) {
        data.push(item.HistorialResumido.ResumenHistorialAtenciones);
      });
    } else if (is.object(history)) {
      data.push(history.HistorialResumido.ResumenHistorialAtenciones);
    } else {
      return callback(new Error("Couldn't get data"));
    }

    callback(null, result.Token, data);
  });
};
