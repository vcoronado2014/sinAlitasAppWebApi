# Migrating SAVINA from v1.x to v2.x

## Important
After migrating make sure you installed the new **Savina** into a clean directory. Do not replace the old version with any of the new version files.

For further details, see the [Installation Instructions](../README.md).

## Instructions

#### Follow this instructions carefully:

0. Open your database manager. Ideally **Microsoft SQL Server Management Studio**.

0. Backup the current `savina` database to Disk.

0. Proceed with the test migration.

  0. Create a new test database named `savina_test` with `savina_dbo`, or the same owner of the previous database, as the database owner and set the collation to `Latin1_General_100_CI_AI`. Setting the collation to this is important as it will simplify searches and filtering by keywords.

  0. Make sure both the old and the new databases have the same database owner. In case the db owner is different from savina_dbo, add savina_dbo in the database login group with public and db_owner access.

  0. Configure the new application to use `savina_test` in `server/config/database`.

  0. Open a terminal, go to the `savina` folder and execute `app migrate` on Windows or `. app migrate` on Linux.

  0. Set the source database as `savina` and the destination database as `savina_test`.

  0. Check the migration logs carefully for any error.

  0. Run the application normally and double-check everything is as expected.

0. Proceed with the definitive migration.

  0. Make sure no instance of Savina is running.

  0. Restore the backed up database into `savina_old`, or something that makes sense, and assign `savina_dbo`, or the previous database owner, as database owner.

  0. Delete the current `savina` database.

  0. Create a new `savina` database with `savina_dbo`, or the same owner of the previous database, as the database owner and set the collation to `Latin1_General_100_CI_AI`. Setting the collation to this is important as it will simplify searches and filtering by keywords.

  0. Make sure both the old and the new databases have the same database owner. In case the db owner is different from savina_dbo, add savina_dbo in the database login group with public and db_owner access.

  0. Configure the new application to use `savina` in `server/config/database`.

  0. Open a terminal, go to the `savina` folder and execute `app migrate` on Windows or `. app migrate` on Linux.

  0. Set the source database as `savina_old` and the destination database as `savina`.

  0. Check the migration logs carefully for any error.

  0. Run the application normally and triple-check everything is as expected.

  0. That's it. The migration is now complete.

  0. You can delete the old database if you like.

## Troubleshooting

Errors will be thrown if a record in the old database is missing a value. This is because the new database includes a set of constraints and indexes that enforce data consistency.

Carefully Read the errors thrown by the migrate script for details and information that may lead to a solution.
