using MelonLoader;
using System;
using System.Runtime.InteropServices;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties


// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM.
[assembly: Guid("E22130A0-1166-448B-B4D9-B4E3AC7E305E")]

[assembly: MelonInfo(typeof(BundleBouncer.BundleBouncer), "BundleBouncer", "1.2.5", "Scott Griffin")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.Red)]
[assembly: MelonAdditionalDependencies("VRChatUtilityKit", "UIExpansionKit")]
[assembly: MelonPlatform(MelonPlatformAttribute.CompatiblePlatforms.WINDOWS_X64)]
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.IL2CPP)]
// Broken in 0.5.3.0 [assembly: VerifyLoaderVersion(0, 5, 3, true)] // Bug in ML 0.5.2 results in false obfuscation detection.