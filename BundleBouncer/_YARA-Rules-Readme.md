# User-Provided YARA Rules

This directory is for storing your own custom YARA rules to be run against suspect assetbundles.

For more information, see https://virustotal.github.io/yara

## External Variables

We provide the following external variables:

| Variable | Type | Example | Notes |
|---|---|---|---|
| `name` | `string` | TODO | Name of the object being scanned |
| `type` | `EScanType` | `ASSETBUNDLE` | Type of the object being scanned |

## Example
