'use strict';

var request = require("request");

const OPEN = '1';
const IN_PROGRESS = '2';
const CLOSED = '3';

function send(req, status) {
  request({
      method: 'PUT',
      uri: 'https://prepplataformadeintegraciones.saludenred.cl:200/Savina/CambioEstadoTLM?Parametros=' +
        encodeURIComponent(JSON.stringify({
          rayenId: req,
          statusId: status
        }))
    },
    function (error, response, body) {
      if (error) {
        return console.error('send failed:', error);
      }
      console.log('Send successful!  Server responded:', response);
      console.log('Send successful!  Server body:', body);
    })

}

function notifyClose(req) {
  send(req, CLOSED);
}

function notifyInProgress(req) {
  send(req, IN_PROGRESS);
}

function notifyOpen(req) {
  send(req, OPEN);
}

function test(req) {
  console.log('prueba de test', req);
}

module.exports = {
  notifyClose,
  notifyInProgress,
  notifyOpen
}
