@ECHO OFF

chcp 65001

IF "%1"=="migrate" GOTO migrate
IF "%1"=="reinstall" GOTO reinstall
IF "%1"=="install" GOTO install
IF "%1"=="setup" GOTO setup
IF "%1"=="dev" GOTO dev
IF "%1"=="start" GOTO start
IF "%1"=="stop" GOTO stop

ECHO. && ECHO Por favor use uno de estos argumentos: setup, start, stop, dev o migrate && ECHO.
GOTO :eof

:migrate
IF "%2"=="check" GOTO migrate-check
IF "%2"=="notifications" GOTO migrate-notifications
node migrate
GOTO :eof

:migrate-check
node migrate\check
GOTO :eof

:migrate-notifications
node migrate\notifications
GOTO :eof

:setup
ECHO. && ECHO Ejecutando setup... && ECHO.

IF "%2"=="admin" GOTO setup-admin

node setup
GOTO :eof

:setup-admin
node setup\admin
GOTO :eof

:dev
ECHO. && ECHO. Ejecutando aplicación en modo desarrollo... && ECHO.
set NODE_ENV=development
set DEBUG=app:*
GOTO app

:start
ECHO. && ECHO Ejecutando aplicación en modo producción... && ECHO.
set NODE_ENV=production

:app
IF "%2"=="fork" GOTO fork

IF "%2"=="cluster" GOTO cluster

node server
GOTO :eof

:stop
ECHO. && ECHO Deteniendo proceso de aplicación PM2 (fork) && ECHO.
node_modules\.bin\pm2 delete savina
GOTO :eof

:fork
ECHO. && ECHO Ejecutando aplicación como proceso PM2 (fork)... && ECHO.
node_modules/.bin/pm2 start server --name "savina"
GOTO :eof

:cluster
ECHO. && ECHO La aplicación no puede ser ejecutada como cluster PM2 && ECHO.
GOTO :eof
