(function (ng) {
  'use strict';

  var SELECTOR = 'meta[name="%s"]';
  var CONTENT = 'content';
  var PS = '%s';

  function getMetaContent(name) {
    return document.querySelector(SELECTOR.replace(PS, name))
      .getAttribute(CONTENT);
  }

  /* Application info */
  window.app = {
    environment: getMetaContent('environment'),
    version: getMetaContent('version'),
    stage: getMetaContent('stage'),
    title: getMetaContent('title'),
    name: getMetaContent('name')
  };

  ng.module('App', [

    /** Angular dependencies */
    'ngRoute',

    /** Modules */
    'ngScrollGlue',
    'ngFileUpload',
    'Formatter',
    'Mimeicon',
    'ngAudio',
    'Moment',
    'ngRut'

  ]);

}(angular));
