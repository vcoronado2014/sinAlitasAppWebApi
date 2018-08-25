'use strict';

module.exports = function (db, Types) {

  var CheckupMode = db.define('checkupMode', {

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

  return CheckupMode;

};
