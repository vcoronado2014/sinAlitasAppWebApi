'use strict';

var debug = require('debug')('app:database');
var Sequelize = require('sequelize');
var is = require('fi-is');

var sequelize;

/**
 * Retrieves a model for queries.
 */
function _model(name) {
  if (!sequelize) {
    throw new Error("You must initialize the database first!");
  }

  if (sequelize.isDefined(name)) {
    return sequelize.model(name);
  }

  debug("There's no model defined with that name! (%s)", name);

  return null;
}

/**
 * Register a collection.
 */
function _define(name, attributes, options) {
  if (!sequelize) {
    throw new Error("You must initialize the database first!");
  }

  return sequelize.define(name, attributes, options);
}

/**
 * Initializes the connections and collections.
 */
function _initialize(config) {
  if (is.not.string(config.database)) {
    throw new Error("Database must be a [String]!");
  }

  if (is.not.string(config.username)) {
    throw new Error("Username must be a [String]!");
  }

  if (is.not.string(config.password)) {
    throw new Error("Password must be a [String]!");
  }

  if (is.not.string(config.host)) {
    throw new Error("Host must be a [String]!");
  }

  if (is.not.number(config.port)) {
    throw new Error("Port must be a [Number]!");
  }

  sequelize = new Sequelize(config.database, config.username, config.password, {
    logging: require('debug')('app:database:sequelize'),
    dialectOptions: config.dialectOptions,
    dialect: config.dialect,
    host: config.host,
    port: config.port
  });
}

function _import(path) {
  if (!sequelize) {
    throw new Error("You must initialize the database first!");
  }

  return sequelize.import(path);
}

function _sync(options) {
  if (!sequelize) {
    throw new Error("You must initialize the database first!");
  }

  return sequelize.sync(options);
}

function _models() {
  if (!sequelize) {
    throw new Error("You must initialize the database first!");
  }

  return sequelize.models;
}

module.exports = {

  initialize: _initialize,

  import: _import,

  define: _define,

  models: _models,

  model: _model,

  sync: _sync

};
