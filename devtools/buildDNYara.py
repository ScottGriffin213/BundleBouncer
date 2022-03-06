'''
Public-Access Buildsystem FOR DNYARA ONLY

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
'''

import os
import re
import shutil
from pathlib import Path

from _common import BBBuildConfig, cmd

import logging #isort: skip
log = logging.getLogger(__name__)

BINSTR = os.sep + 'bin' + os.sep
OBJSTR = os.sep + 'obj' + os.sep
DISTSTR = os.sep + 'dist' + os.sep

def copyFrom(source: Path, dest: Path) -> None:
    for path in dest.rglob('*'):
        if not path.is_file():
            continue
        if path.suffix == '.csproj':
            continue
        if BINSTR in str(path): 
            continue
        if OBJSTR in str(path):
            continue
        if DISTSTR in str(path):
            continue
        try:
            path.unlink()
        except Exception as e:
            log.info(str(e))
    for path in source.rglob('*'):
        if not path.is_file():
            continue
        if path.suffix == '.csproj':
            continue
        if BINSTR in str(path):
            continue
        if OBJSTR in str(path):
            continue
        if DISTSTR in str(path):
            continue
        destfile = dest / path.relative_to(source)
        destfile.parent.mkdir(parents=True,exist_ok=True)
        log.info(f'{path} -> {destfile}')
        destfile.write_bytes(path.read_bytes())

PROJ_ROOT = Path(__file__).parent.parent

LIB_DIR: Path = PROJ_ROOT / 'lib'

CORE_PROJ_DIR = PROJ_ROOT / 'BundleBouncer'

YARA_BUILD_BASE = LIB_DIR / 'dnYara' / 'build'
YARA_BUILD_X86_DIR = YARA_BUILD_BASE / 'x86'
YARA_BUILD_X64_DIR = YARA_BUILD_BASE / 'x64'
YARA_C_DIR = LIB_DIR / 'dnYara' / 'libs' / 'yara'
YARA_CMAKE_DIR = LIB_DIR / 'dnYara' / 'libs' / 'cmake'

METHODS_CS = LIB_DIR / 'dnYara' / 'Libraries' / 'dnYara.Interop' / 'Interops' / 'Methods.cs'

def buildDNYara(bcfg: BBBuildConfig) -> None:

    YARA_BUILD_X86_DIR.mkdir(parents=True, exist_ok=True)
    YARA_BUILD_X64_DIR.mkdir(parents=True, exist_ok=True)

    log.info(f'Patching {METHODS_CS}...')
    REG_YARA_LIBNAME = re.compile(r'public const string YaraLibName = "([^"]+)";')
    code = METHODS_CS.read_text()
    code = REG_YARA_LIBNAME.sub(f'public const string YaraLibName = "Dependencies/libyara";', code)
    METHODS_CS.write_text(code)


    olddir = Path.cwd()
    os.chdir(YARA_BUILD_X64_DIR)
    try:
        cmd([str(bcfg.CMAKE), '-G', 'Visual Studio 16 2019', str(YARA_CMAKE_DIR), '-DBUILD_SHARED_LIB=ON', '-Dyara_ALL_MODULES=ON', '-A', 'x64'])
        cmd([str(bcfg.CMAKE), '--build', '.', '--config', 'release'])
    finally:
        os.chdir(olddir)
    copyFrom(LIB_DIR / 'dnYara' / 'dnYara',                       PROJ_ROOT / 'dnYara4Core')
    copyFrom(LIB_DIR / 'dnYara' / 'Libraries' / 'dnYara.Interop', PROJ_ROOT / 'dnYara4Core.Interop')
    (PROJ_ROOT / 'legal' / 'dnYARA.LICENSE.txt').write_bytes((LIB_DIR / 'dnYara' / 'LICENSE').read_bytes())

    shutil.copy(LIB_DIR / 'dnYara' / 'build' / 'x64' / 'bin' / 'Release' / 'libyara.dll',
                CORE_PROJ_DIR / 'libyara.dll')


if __name__ == '__main__':
    bcfg = BBBuildConfig()
    bcfg.load()
    buildDNYara(bcfg)
