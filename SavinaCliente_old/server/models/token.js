'use strict';

var uuid = require('node-uuid');
var crypto = require('crypto');
var moment = require('moment');

module.exports = function (db, Types) {

  var Token = db.define('token', {

    secret: {
      type: Types.STRING,
      allowNull: false,
      unique: true,

      validate: {
        notEmpty: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Token.belongsTo(models.user);
      }
    },

    instanceMethods: {
      hasExpired: function hasExpired() {
        if (moment(Date.now()).diff(this.createdAt, 'hours') > 24) {
          return true;
        }

        return false;
      }
    },

    hooks: {
      beforeValidate: function beforeValidate(token) {
        token.secret = crypto.createHash('sha1').update(uuid.v4()).digest().toString('hex');
      },

      beforeUpdate: function beforeUpdate() {
        throw new Error("You cannot modify a Token! Create a new one.");
      }
    },

    indexes: [{
      unique: true,
      fields: ['secret']
    }]

  });

  return Token;

};
