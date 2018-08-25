'use strict';

module.exports = function (db, Types) {

  var Role = db.define('role', {

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

  return Role;

};
