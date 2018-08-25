'use strict';

const RAYEN_SIC_ID = 'rayenSicId';
const REQUEST = 'requests';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.addColumn(
      REQUEST,
      RAYEN_SIC_ID, {
        type: Sequelize.INTEGER,
        defaultValue: null,
        allowNull: true
      }
    );
  },

  down: function (queryInterface) {
    return queryInterface.removeColumn(REQUEST, RAYEN_SIC_ID);
  }
};
