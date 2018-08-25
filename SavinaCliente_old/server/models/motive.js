'use strict';

module.exports = function (db, Types) {

  var Motive = db.define('motive', {

    slug: {
      type: Types.STRING,
      unique: true,
      allowNull: false
    },

    name: {
      type: Types.TEXT,
      allowNull: false
    }

  }, {

    timestamps: false,

    indexes: [{
      unique: true,
      fields: ['slug']
    }]

  });

  return Motive;

};
