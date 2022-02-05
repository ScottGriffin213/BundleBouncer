# Changelog

## 1.2.7 - In Development

* Core
  * Added avatar whitelist functionality.

## 1.2.6

* Core
  * Standardized more avatar ID detection notifications.
* User State Tracking
  * Fixed NRE caused by null players joining/leaving.
* Definitions
  * Fixed some avatar hashes not getting added to the bundle hash check.
  
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