'use strict';

const moment = require('moment');
const shortid = require('shortid');

module.exports = function (db, Types) {

  var Link = db.define('link', {

    hash: {
      type: Types.STRING,
      allowNull: false,
      unique: true
    },

    url: {
      type: Types.TEXT,
      allowNull: false
    },

    expiresAt: {
      type: Types.DATE
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Link.belongsTo(models.request);
      }
    },

    instanceMethods: {
      hasExpired: function hasExpired() {
        if (moment(Date.now()).diff(this.createdAt, 'days') > 30) {
          return true;
        }

        return false;
      }
    },

    hooks: {
      beforeValidate: function beforeValidate(link) {
        link.hash = shortid.generate();
      }
    },

    indexes: [{
      unique: true,
      fields: ['hash']
    }]

  });

  return Link;

};
