<p align="center">
  <a href="#"><img src="https://github.com/ScottGriffin213/BundleBouncer/raw/main/BundleBouncer/images/png/BundleBouncer-512.png"></a>
  <a href="#"><img src="https://github.com/ScottGriffin213/BundleBouncer/raw/main/BundleBouncer/images/png/BundleBouncer-text.png"></a>
</p>

---

<p align="center">
	<a href="https://github.com/ScottGriffin213/BundleBouncer/releases/latest"><img src="https://img.shields.io/github/v/release/ScottGriffin213/BundleBouncer?label=latest&style=for-the-badge"></a>
	<a href="https://github.com/ScottGriffin213/BundleBouncer/releases"><img src="https://img.shields.io/github/downloads/ScottGriffin213/BundleBouncer/total.svg?style=for-the-badge"></a>
	<a href="https://github.com/ScottGriffin213/BundleBouncer/graphs/contributors"><img src="https://img.shields.io/github/contributors/ScottGriffin213/BundleBouncer?style=for-the-badge"></a>
	<a href="https://github.com/ScottGriffin213/BundleBouncer/graphs/contributors"><img alt="GitHub" src="https://img.shields.io/github/license/ScottGriffin213/BundleBouncer?style=for-the-badge"></a>
  <a href="https://discord.gg/jt8xppHeQq"><img alt="Discord" src="https://img.shields.io/discord/935695835012952085?style=for-the-badge"></a>
</p>

<p align="center">
  <a href="https://github.com/ScottGriffin213/BundleBouncer/wiki/FAQ">⚠️FAQ⚠️</a>
  |
  <a href="https://github.com/ScottGriffin213/BundleBouncer/wiki/Compiling">Compiling</a>
</p>

---

This project is an advanced assetbundle scanning system, designed to prevent maliciously corrupted assetbundles from crashing your game by preventing them from being downloaded at all, or at the very least blocking them from being loaded.  It makes use of a wide range of technologies, including trie-lookup hash matching, VirusTotal's [YARA](http://virustotal.github.io/yara/) signature matching engine, and [AssetsTools.NET](https://github.com/nesrak1/AssetsTools.NET).

We know there are clients out there with some of this featureset, but we're tired of basic safety features like this being locked up behind invite-only Discords and Patreons.

Quit being dicks.  You know who you are.

## Notes

* **Using ANY mods can get you banned from VRChat.** Don't talk about using mods, and don't be obvious around people you don't trust.
* While BundleBouncer tries to block as much as it can, it still relies on known signatures and hashes for much of it's power.  **False negatives (and positives) are possible.**
* BundleBouncer is now tied into a significant part of Unity's crusty underbelly.  This can result in occasional weirdness.
* Some C# files in this project are generated from sets of data not included in this repository (to prevent skiddies getting access to all the bad avatars we know of), so **the files seen here are _not_ a complete representation of the codebase**.
  * However, the files in this project **are** representative of everything in the DLLs. Compiling this code *should* result in a DLL mostly identical to the one released as a binary (beyond some compiler gibberish).
* For security, malicious avatar IDs are [hashed](https://en.wikipedia.org/wiki/Cryptographic_hash_function) and mildly obfuscated so skiddies can't easily grab a list of them.
* **We will not be releasing this on VRCMG yet, since they require manual reviews and would result in horrendously outdated definitions.** We have split the project into a core DLL, and an automatically-generated definitions DLL to simplify things in the future.
* **ONLY** grab this DLL from [https://github.com/ScottGriffin213/BundleBouncer/releases/latest](https://github.com/ScottGriffin213/BundleBouncer/releases/latest) or the mirror at [https://gitgud.io/Scottinator/BundleBouncer/-/releases](https://gitgud.io/Scottinator/BundleBouncer/-/releases)! Forks can contain dangerous code, so review any files from forks using dnSpy before installing them. Tip:  If they're obfuscating the code, there's something they don't want you to see.

## Installing

* Install [VRChatUtilityKit](https://github.com/SleepyVRC/Mods#vrchatutilitykit)
* Install [UIExpansionKit](https://github.com/knah/VRCMods)
* Install BundleBouncer.dll from [Releases](https://github.com/ScottGriffin213/BundleBouncer/releases/latest) into MelonLoader `Mods/` directory.
* Optionally, pre-download BundleBouncer.Shitlist.dll from [LATEST_DEFINITIONS](https://github.com/ScottGriffin213/BundleBouncer/releases/tag/LATEST_DEFINITIONS) into a subfolder of your VRChat install called `Dependencies/`.
* Optionally, pre-download Global-YARA-Rules.bin from [LATEST_DEFINITIONS](https://github.com/ScottGriffin213/BundleBouncer/releases/tag/LATEST_DEFINITIONS) into `UserData/BundleBouncer/`.

## Adding an Avatar ID
NOTE: There are a bunch of avatars that are automatically blocked by the mod.

1. Open or create `VRChat\UserData\BundleBouncer\My-Blocked-Avatars.txt` in your favorite text editor that isn't Word or Wordpad.
2. Add the avatar ID (usually of format `avtr_<gibberish>`) to a new line.
3. Save and close the file.
4. [Let us know about it.](#shitlist-additions)

The file should look like this when you're done:

```
# Any line beginning with # are comments and are ignored by BundleBouncer.
# NOTE: The avtr_ IDs used in this example are fake and shouldn't exist in real life.

avtr_6c6b51e5-5abe-4534-a223-f4d955770201
```

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

Please send any crasher, lagger, or otherwise malicious avatar IDs to scgriffin213@outlook.com (this is not my real email, but will still reach me). **DO NOT REPORT THESE AVATARS TO THE ISSUE LIST ABOVE!**

Include the following:

* Asset ID (`avtr_` or `file_`)
* Assetbundle and/or its SHA256 checksum, if known (use `sha256sum filename`)
* Avatar Name, if available
* Pictures, if available
* Description of malicious behaviour (screenspace shader, assetbundle crasher, etc)
* World(s) encountered in
* The name you wish to be credited as, if you want.

## Contact

* Discord: https://discord.gg/jt8xppHeQq

## License

The public source code (which is contained in this repository) is MIT-licensed. Feel free to send Merge Requests (MRs) and report issues!

The full buildsystem and avatar dataset is proprietary for security reasons. Copyright &copy;2021-2022 "Scott Griffin". All rights reserved.

We make use of dnYara and libyara for signature matching.  See [LICENSE.dnYara](licenses/LICENSE.dnYara) for dnYara's license and [CHANGES.dnYara.md](licenses/CHANGES.dnYara.md) for our changes to dnYara.

We also make use of [AssetsTools.NET](https://github.com/nesrak1/AssetsTools.NET) for smoke-testing AssetBundles.

Mono.Cecil is a dependency of AssetsTools.NET. We're fairly sure it's not actually used, but it's bundled into the mod, just in case.
## Contributors

* "Scott Griffin"
* null

## Credits

* [AdvancedSafety](https://github.com/knah/VRCMods/tree/master/AdvancedSafety) by Knah - IL2CPP interface code
* [Finitizer](https://github.com/knah/VRCMods/tree/master/Finitizer) by Knah - IL2CPP icall hooking code
* Airbus CERT - Made dnYara
* AssetTools.NET
* Behemoth - More help with IL2CPP
* Benji, Requi - Handholding, ideas, help with backend particulars
* Jewordi - Testing, ideas, being a bro
* StackOverflow - Code outsourcing
* VRCMG Discord - Putting up with my dumb questions
* Various skiddies - Outright stole and modified code for some sketchier API calls.
* VirusTotal - Made YARA