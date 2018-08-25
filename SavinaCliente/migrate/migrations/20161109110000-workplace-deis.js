'use strict';

const DEIS_INDEX = 'workplaces_deis_unique';
const WORKPLACES = 'workplaces';
const UNIQUE = 'UNIQUE';
const DEIS = 'deis';

module.exports = {
  up: function (queryInterface, Sequelize) {
    return queryInterface.addColumn(
      WORKPLACES,
      DEIS, {
        defaultValue: Sequelize.fn('NEWID'),
        type: Sequelize.STRING,
        allowNull: false
      }
    ).then(() => {
      return queryInterface.addIndex(
        WORKPLACES, [DEIS], {
          indexName: DEIS_INDEX,
          indicesType: UNIQUE
        }
      )
    });
  },

  down: function (queryInterface) {
    return queryInterface.removeIndex(WORKPLACES, DEIS_INDEX).then(() => {
      return queryInterface.removeColumn(WORKPLACES, DEIS);
    });
  }
};
