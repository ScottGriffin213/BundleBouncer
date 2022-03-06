# Changelog

## 1.3.1 - In Development

* Fixes
  * Public-access buildsystem updated for dnYara.
  
## 1.3.0

* Updated for VRChat build 1169.
* Core
  * Now intercepts Unity internals in order to capture bundles just prior to loading.
  * Added [YARA](http://virustotal.github.io/yara/) binary scanning engine from VirusTotal and Airbus CERT.
  * Added [AssetTools.NET](https://github.com/nesrak1/AssetsTools.NET) for detailed inspection of assetbundles.
* Configuration
  * Added `UserData/BundleBouncer/My-Allowed-Avatars.txt` to allow manually whitelisting avatar IDs.
  * Added `UserData/BundleBouncer/My-Blocked-Avatars.txt` to allow manually blacklisting avatar IDs.
  * Added `UserData/BundleBouncer/My-Allowed-Asset-Hashes.txt` to allow manually whitelisting asset hashes.
  * Added `UserData/BundleBouncer/My-Blocked-Asset-Hashes.txt` to allow manually blacklisting asset hashes.
  * Added `UserData/BundleBouncer/User-YARA-Rules/` so users can provide their own YARA rules.
* **Known issues**
  * **This changeset is very experimental and invasive, and will likely cause strange problems. User beware.**
  * Progress bars do not work due to internal Unity weirdness. WIP.

## 1.2.7

* Core
  * Added avatar whitelist functionality.
  * Added assetbundle hash whitelist functionality.
  * Added user avatar ID whitelist to bypass automatic tests.

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