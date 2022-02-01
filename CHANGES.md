# Changelog

## 1.2.6 - In Development

* Bugfixes
  * Fixed NRE caused by null players joining/leaving.
  
## 1.2.5

* Definitions
  * Fix shitlist auto-updates crippled by crappy logic.

## 1.2.4

* User State Tracking
  * Now tracks the `properties_changed` event properly.
* Logging
  * Logs are cleared on game start.
  * Logging directory is created if it doesn't exist.
* Shitlist
  * Add avatar ID and assetbundle hash count to shitlist startup