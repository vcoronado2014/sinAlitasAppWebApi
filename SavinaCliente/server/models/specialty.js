'use strict';

module.exports = function (db, Types) {

  var Specialty = db.define('specialty', {

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
    }],

    classMethods: {
      associate: function associate(models) {
        Specialty.hasMany(models.agreement);
      }
    }

  });

  return Specialty;

};
