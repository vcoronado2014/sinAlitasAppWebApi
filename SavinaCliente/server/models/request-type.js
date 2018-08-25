'use strict';

module.exports = function (db, Types) {

  var RequestType = db.define('requestType', {

    slug: {
      type: Types.STRING,
      unique: true,
      allowNull: false
    },

    name: {
      type: Types.STRING,
      allowNull: false
    }

  }, {

    timestamps: false,

    indexes: [{
      unique: true,
      fields: ['slug']
    }]

  });

  return RequestType;

};
