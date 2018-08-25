# Savina

## Migrating
If you're migrating from `v1.x.x` to `v2.x.x` follow the install instructions into a clean directory. Do not replace the old version with any of the new version files.

For further details, see the [Migration Instructions](migrate/INSTRUCTIONS.md).

## Installation
### Prerequisites
* A machine with Windows Server 2012 R2 or Ubuntu 14.04 LTS.
* A local, remote or virtual instance of Microsoft SQL Server 2012 or newer.

### Windows
* Download and install [Microsoft Visual Studio Express 2013 for Windows Desktop with Update 5](http://www.microsoft.com/en-us/download/details.aspx?id=48131).
* Download and install latest [Git](http://git-scm.com/download/win). When installing, make sure to select **Use Git from the Windows Command Prompt** to make the `git` command available globally.
* Download and install latest [Python Latest v2.x.x](https://www.python.org/downloads/windows/). Make sure you add it to `PATH`.
* Download and install latest [Redis for Windows](https://github.com/MSOpenTech/redis/releases). Make sure you add it to `PATH`.
* Download and install latest [Node.js v4.x.x LTS](https://nodejs.org/en/download/). Make sure you add both **Node.js** and **NPM** to `PATH`.

**IMPORTANT:** all of this dependencies must be installed for all users and added to `PATH`, so be aware of the installation options.

### Ubuntu
* Open a terminal:
  <kbd>CTRL</kbd>+<kbd>ALT</kbd>+<kbd>T</kbd>

* Install CURL:
  ```sh
  sudo apt-get install curl
  ```

* Install the build tools:
  ```sh
  sudo apt-get install build-essential
  ```

* Install Git:
  ```sh
  sudo apt-get install git
  ```

* Python should be installed by default, but to be sure:
  ```sh
  sudo apt-get install python
  ```

* Install Node.js v4.x.x LTS:
  ```sh
  curl --silent --location https://deb.nodesource.com/setup_4.x | sudo bash -
  sudo apt-get install --yes nodejs
  ```

* Install Redis:
  ```sh
  sudo add-apt-repository ppa:chris-lea/redis-server
  sudo apt-get update
  sudo apt-get install redis-server
  ```

Quick command:
```
sudo add-apt-repository ppa:chris-lea/redis-server && curl --silent --location https://deb.nodesource.com/setup_4.x | sudo bash - && sudo apt-get install --yes curl build-essential git python nodejs redis-server
```

### Troubleshooting
In Windows, make sure you don't have another version of Visual Studio installed and always run the command prompt as administrator.

If you get any problem installing **npm** modules please consult the [Node GYP documentation](https://github.com/nodejs/node-gyp) for further reference.

### Setup
0. In Windows, open a command prompt as **administrator**. In Ubuntu just open a terminal.

0. Go into the application's folder and run `. app install` in Ubuntu and `app install` in Windows. This will install all local and global dependencies and compile client scripts, templates and styles.

0. Watch for any errors.

0. Configure a database connection in `server/config/database.js`.

  Example `server/config/database.js`:
  ```js
  'use strict';

  module.exports = {

    database: 'savina',

    username: 'savina_dbo',

    password: 'savina_password',

    host: 'localhost',

    port: 1433

  };
  ```

0. Configure the file storage directory at `server/config/fileman.js` by replacing the `stordir` parameter with the desired destination folder.

  Example `server/config/fileman.js`:
  ```js
  'use strict';

  var path = require('path');
  var os = require('os');

  module.exports = {

    tempdir: path.normalize(path.join(os.tmpDir(), 'savina', 'uploads')), // No need to change

    stordir: path.normalize(path.join(__appdir, '..', 'savina-storage'))

  };
  ```

  E.g., the following value will resolve to `C:\Users\Administrator\savina-storage`:

  ```js
  path.normalize(path.join('\\', 'Users', 'Administrator', 'savina-storage'))
  ```

  **Important:** `path.normalize` and `path.join` must be kept in order to avoid any inconsistencies. Read more about this methods in the [Node.js documentation](https://nodejs.org/api/path.html).

  **Tip:** A good practice would be to create a symbolic link at the `stordir`'s resolved path and point it to another drive or cloud backup folder.

0. Place the `server.key`, `server.crt` and `server.ca` SSL certificate files into the `server/credentials` folder.

0. Configure the HTTPS port and credentials in `server/config/server.js`.

0. Configure the Mailer (SMTP) component at `server/config/mailer.js`.

0. Open a terminal or command prompt and go to the app folder.

0. **IMPORTANT:** If you're migrating, follow the [Migration Instructions](migrate/INSTRUCTIONS.md) from here on instead.

0. Run the `. app setup` command on Ubuntu and `app setup` in Windows.

0. Follow the prompts.

0. Check for any error on the log.

0. If you made a mistake, cancel the script by pressing <kbd>CTRL</kbd>+<kbd>C</kbd> _(and then <kbd>Y</kbd> on Windows)_ and re run the setup script.

0. If you just need to create an administrator account, run `. app setup admin` in Ubuntu and `app setup admin` in Windows.

0. Once complete, start the application with `. app start` in Ubuntu and `app start` in Windows.

0. If there are any errors or when the application logs `HTTPS Server Listening on port ####` stop it by pressing <kbd>CTRL</kbd>+<kbd>C</kbd> _(and then <kbd>Y</kbd> on Windows)_.

#### Troubleshooting
If the application cannot connect to the database then your database configuration might be wrong. You need to make sure that SQL Server is allowed to use TCP/IP and named pipes. You can turn these on by opening the `SQL Server Configuration Manager Start > Programs > Microsoft SQL Server 2012 > Configuration Tools`, and make sure that TCP/IP and Named Pipes are enabled.

## Running
Use `. app start` on Ubuntu/OSX and `app start` on Windows. This will run in HTTPS mode and will secure all cookies and proxies. This is the only suitable option for a production environment.

To stop the application just press <kbd>CTRL</kbd>+<kbd>C</kbd> _(and then <kbd>Y</kbd> on Windows)_.

### Running as fork
To run the application as a **fork** means that the application will be kept alive forever, even if it fails, it will be reloaded with zero downtime.

To start the application as **fork** run the following command in Windows:
```
app start fork
```

And with the following command in Ubuntu:
```
. app start fork
```

### Running as cluster with load balancer
The **cluster** mode has the same functionality as the **fork** mode with the difference that multiple instances will be created and distributed across the available CPU cores, acting as a load balancer.

To start the application as **cluster** run the following command in Windows:
```
app start cluster
```

And with the following command in Ubuntu:
```
. app start cluster
```

If you need further information please refer to the [PM2 documentation](https://github.com/Unitech/PM2).

## Updating
To update the application first stop the application instance, by either using <kbd>CTRL</kbd>+<kbd>C</kbd> or `pm2 delete savina`, and remove all files and folders but the `server/config/database.js`, `server/config/mailer.js`, `server/config/server.js` and `server/credentials` folder. Then, open the packaged release and extract all files into the application folder. Be aware to not replace any of the previously mentioned files.

Once the files are extracted, run `app install` on Windows or `. app install` on Ubuntu and then start the application normally.
