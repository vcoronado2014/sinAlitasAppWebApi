'use strict';

module.exports = function (db, Types) {

  var Gender = db.define('gender', {

    slug: {
      type: Types.STRING,
      unique: true,
      allowNull: false
    },

    name: {
      type: Types.STRING,
      allowNull: false
    },

    value: {
      type: Types.INTEGER
    }

  }, {

    timestamps: false,

    indexes: [{
      unique: true,
      fields: ['slug']
    }]

  });

  return Gender;

};
