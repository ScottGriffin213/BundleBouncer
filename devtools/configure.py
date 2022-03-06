'''
Build Configuration Tool

Copyright (c) 2022 BundleBouncer Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

---

Our internal buildsystem is a bit of a clusterfsck, so this is for pubbies who
don't need to pre-generate tries, inject PGP keys, etc.

If you need to rebuild dnYARA, use devtools/buildDNYara.py.
'''

import os, sys, argparse
from pathlib import Path
from _common import BBBuildConfig


def existingFilePath(inp: str) -> Path:
    p = Path(inp)
    assert p.is_file(), f'{inp} does not represent an existing file'
    return p


def existingDirPath(inp: str) -> Path:
    p = Path(inp)
    assert p.is_dir(), f'{inp} does not represent an existing directory'
    return p

def main():
    argp = argparse.ArgumentParser()
    argp.add_argument('--vrchat-dir', type=existingDirPath, default=None)
    argp.add_argument('--cmake', type=existingFilePath, default=None)
    argp.add_argument('--dotnet', type=existingFilePath, default=None)
    argp.add_argument('--gradle', type=existingFilePath, default=None)
    argp.add_argument('--java', type=existingFilePath, default=None)
    argp.add_argument('--auto-detect', action='store_true', default=False)
    args = argp.parse_args()

    bcfg = BBBuildConfig()
    bcfg.load()
    print(bcfg.dump())

    if args.auto_detect:
        bcfg.detectPaths()
    if args.vrchat_dir is not None:
        bcfg.VRCHAT_DIR=args.vrchat_dir.absolute()
        print(f'VRCHAT_DIR = {bcfg.VRCHAT_DIR}')
    if args.cmake is not None:
        bcfg.CMAKE=args.cmake.absolute()
        print(f'CMAKE = {bcfg.CMAKE}')
    if args.dotnet is not None:
        bcfg.DOTNET = args.dotnet.absolute()
        print(f'DOTNET = {bcfg.DOTNET}')
    if args.gradle is not None:
        bcfg.GRADLE=args.gradle.absolute()
        print(f'GRADLE = {bcfg.GRADLE}')
    if args.java is not None:
        bcfg.JAVA=args.java.absolute()
        print(f'JAVA = {bcfg.JAVA}')

    print(bcfg.dump())
    bcfg.save()

if __name__ == '__main__':
    main()