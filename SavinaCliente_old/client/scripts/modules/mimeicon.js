(function (ng) {
  'use strict';

  function geticon(type) {
    var idx, len,
      types = [{
        regexp: /^application\/.*?(?:zip|compressed).*?$/gi,
        icon: 'zip-o'
      }, {
        regexp: /^image\/.+$/gi,
        icon: 'image-o'
      }, {
        regexp: /^(?:(?:application\/(?:(?:vnd\.)?.*?(?:officedocument\.wordprocessingml|opendocument\.text|ms-?word).*?))|text\/plain)$/gi,
        icon: 'text-o'
      }, {
        regexp: /^(?:application\/vnd\.(?:.*?(?:(?:officedocument|opendocument)\.spreadsheet|ms-excel).*?))$/gi,
        icon: 'excel-o'
      }, {
        regexp: /^application\/pdf$/gi,
        icon: 'pdf-o'
      }, {
        regexp: /^audio\/.+?$/gi,
        icon: 'sound-o'
      }, {
        regexp: /^video\/.+?$/gi,
        icon: 'video-o'
      }];

    for (len = types.length, idx = 0; idx < len; idx += 1) {
      if (types[idx].regexp.test(type)) {
        return types[idx].icon;
      }
    }

    return 'o';
  }

  ng.module('Mimeicon', []).

  factory('$mimeicon', function () {
    return {
      for: geticon
    };
  }).

  filter('mimeicon', function () {
    return geticon;
  });

}(angular));
