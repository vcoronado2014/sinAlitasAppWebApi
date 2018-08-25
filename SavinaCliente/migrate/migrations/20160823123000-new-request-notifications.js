'use strict';

const EMAIL_REQUEST_CREATED = 'emailRequestCreated';
const PREFERENCES = 'preferences';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.addColumn(
      PREFERENCES,
      EMAIL_REQUEST_CREATED, {
        type: Sequelize.BOOLEAN,
        defaultValue: true,
        allowNull: true
      }
    );
  },

  down: function (queryInterface) {
    return queryInterface.removeColumn(PREFERENCES, EMAIL_REQUEST_CREATED);
  }
};
