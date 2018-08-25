'use strict';

const SOUND_NOTIFICATIONS = 'soundNotifications';
const PREFERENCES = 'preferences';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.addColumn(
      PREFERENCES,
      SOUND_NOTIFICATIONS, {
        type: Sequelize.BOOLEAN,
        defaultValue: true,
        allowNull: true
      }
    );
  },

  down: function (queryInterface) {
    return queryInterface.removeColumn(PREFERENCES, SOUND_NOTIFICATIONS);
  }
};
