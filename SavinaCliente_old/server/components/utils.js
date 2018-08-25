'use strict';

module.exports = {

  format: {
    asName: function (string) {
      return string
        /* Keep only letters with or without specials and spaces */
        .replace(/[^a-z\u00E0-\u00FC\s]/gi, '')
        /* Capitalize each word */
        .replace(/[a-z\u00E0-\u00FC]\S*/gi, function (word) {
          return word.charAt(0).toUpperCase() + word.substr(1).toLowerCase();
        })
        /* Trim and replace multiple spaces with only one space */
        .trim().replace(/\s+/g, ' ');
    }
  }

};
