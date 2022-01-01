# BundleBouncer

This project is a quick and dirty avatar ID blocker, designed to prevent corrupted assetbundles from crashing your game by preventing them from being downloaded at all.

## Installing

* Requires VRChatUtilityKit
* Install DLL from Releases into MelonLoader Mods/ directory

## Notes

* **Using ANY mods can get you banned from VRChat.** Don't talk about using mods and don't be obvious around people you don't trust.
* Some C# files in this project are generated from sets of data not included in this repository, so **the files seen here are not a complete representation of the codebase**.
  * However, the files in this project **are** representative of everything in the DLL.
* For security, avatar IDs are hashed and mildly obfuscated so skiddies can't easily grab a list of them.

## Adding an Avatar ID

1. Open or create `VRChat\UserData\BundleBouncer\Avatars.txt` in your favorite text editor that isn't Word or Wordpad.
2. Add the avatar ID (usually of format `avtr_<gibberish>`) to a new line.
3. Save and close the file.
4. [Let us know about it.](#shitlist-additions)

TODO: Discord

## Support

### Bugs and Issues

Please report any problems at the issue board above.

When reporting an issue, please provide the following:

* Your OS
* Full MelonLoader log (remove any private information)
* Steps we can take to have the same problem as you (Reproduction steps)
* What you were trying to do at the time
* What you expected to happen
* What actually happened

### Feature Requests

Feature requests are welcome, but are not guaranteed to be added.

Please note that any feature added is NOT your property, but the property of the project. 

### Shitlist Additions

Please send any crasher, lagger, or otherwise malicious avatar IDs to `scgriffin213@outlook.com` (this is not my real email, but will still reach me). **DO NOT REPORT THESE AVATARS TO THE ISSUE LIST ABOVE!**

Include the following:

* Asset ID
* Avatar Name, if available
* Pictures, if available
* Description of malicious behaviour
* World(s) encountered in
* The name you wish to be credited as, if applicable.

## Contact

TODO: Discord

## License

The public source code is MIT-licensed. Feel free to send Merge Requests (MRs) and report issues!

The full buildsystem and avatar dataset is proprietary. Copyright &copy;2021-2022 "Scott Griffin". All rights reserved.