'use strict';

module.exports = {

  csrf: {
    angular: true,
    exclude: [{
      method: 'POST',
      path: '/api/rayen/requests'
    }]
  },

  xssProtection: {
    enabled: true
  },

  xframe: 'DENY',

  /* TODO: Refine and implement CSP */
  // csp: {
  //   /* TODO: Implement this CSP reporter */
  //   'report-uri': 'https://savina.cl/reporter',
  //
  //   policy: {
  //     'style-src': "'self' 'unsafe-inline'",
  //     'connect-src': "'self' wss:",
  //     'media-src': "'self' blob:",
  //     'default-src': "'self'"
  //   }
  // },

  hsts: {
    includeSubDomains: true,
    maxAge: 31536000,
    preload: true
  }

};
