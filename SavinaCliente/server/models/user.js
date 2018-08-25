'use strict';

var bcrypt = require('bcryptjs');

module.exports = function (db, Types) {

  var User = db.define('user', {

    name: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    email: {
      type: Types.STRING,
      allowNull: false,
      unique: true,

      validate: {
        isEmail: true
      }
    },

    password: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        len: [4, 255],
        notEmpty: true
      },

      set: function (value) {
        this.setDataValue('password', bcrypt.hashSync(value, bcrypt.genSaltSync(8)));
      }
    },

    deletedAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        User.belongsTo(models.specialty);

        User.hasOne(models.preference);

        User.belongsTo(models.role);

        User.belongsTo(models.user, {
          as: 'creator'
        });

        User.belongsToMany(models.workplace, {
          through: {
            model: models.userWorkplace,
            unique: false
          }
        });
      }
    }

  });

  return User;

};
