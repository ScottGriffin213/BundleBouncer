'''
Public-Access Buildsystem

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
import os
import shutil
import subprocess
from pathlib import Path
from typing import List

from lxml import etree

from _common import BBBuildConfig, cmd

import logging #isort: skip
log = logging.getLogger(__name__)

PROJ_ROOT = Path(__file__).parent.parent
CORE_PROJ_DIR = PROJ_ROOT / 'BundleBouncer'
SHITLIST_PROJ_DIR = PROJ_ROOT / 'BundleBouncer.Shitlist'

LIB_DIR: Path = PROJ_ROOT / 'lib'
DIST_DIR = PROJ_ROOT / 'dist'
ILREPACK: Path = LIB_DIR / 'ilrepack' / 'ILRepack' / 'bin' / 'Release' / 'ILRepack.exe'
MERGE_ASSEMBLIES = [
    'BundleBouncer.dll',  # self, must come first
    'AssetsTools.NET.dll',
    'BouncyCastle.Crypto.dll',
    'BundleBouncer.Validation.dll',
    'Mono.Cecil.Mdb.dll',
    'Mono.Cecil.Pdb.dll',
    'Mono.Cecil.Rocks.dll',
    'Mono.Cecil.dll',
    'dnYara4Core.Interop.dll',
    'dnYara4Core.dll',
]
MOD_DEPENDENCIES = [
    'UIExpansionKit.dll',
    'VRChatUtilityKit.dll',
]
BREAKS_SHIT = frozenset(['Newtonsoft.Json'])

MANAGED_DIR: Path = None
MODS_DIR: Path = None

GENERATED_SRC_DIR = Path.cwd().absolute() / 'src' / 'Generated'

def idiotCheck(bcfg: BBBuildConfig) -> None:
    global MANAGED_DIR, MODS_DIR
    assert bcfg.VRCHAT_DIR.is_dir(), 'buildcfg.yml has an invalid paths.vrchat: Directory does not exist'

    MANAGED_DIR = bcfg.VRCHAT_DIR / 'MelonLoader' / 'Managed'
    assert MANAGED_DIR.is_dir(), 'You need to install MelonLoader and run VRChat once to generate dependencies.'
    MODS_DIR = bcfg.VRCHAT_DIR / 'Mods'
    assert MODS_DIR.is_dir() or any([(not (MODS_DIR / dllname).is_file()) for dllname in DLL_DEPENDENCIES]), 'You need to install UIX and VRCUK before building.'

    assert bcfg.GRADLE is not None, 'gradle executable not found (required for ILRepack), install gradle'
    log.info(f'GRADLE={bcfg.GRADLE}')
    assert bcfg.CMAKE is not None, 'cmake executable not found (required for dnYara), install CMake with VS2019'
    log.info(f'CMAKE={bcfg.CMAKE}')
    assert bcfg.JAVA is not None, 'java executable not found (required for ILRepack), install a JDK'
    log.info(f'JAVA={bcfg.JAVA}')
    assert bcfg.DOTNET is not None, 'dotnet executable not found (required), install dotnet 5/6'
    log.info(f'DOTNET={bcfg.DOTNET}')

    dotnet_version: List[str] = subprocess.check_output([str(bcfg.DOTNET), '--version'], executable=bcfg.DOTNET).decode('utf-8').strip().split('.')
    dotnet_version: List[int] = list(map(int, dotnet_version))
    assert dotnet_version[0] >= 5, 'dotnet version {dotnet_version!r} < 5.0'

def configureProjects(bcfg: BBBuildConfig) -> None:
    configureProject(bcfg, Path('BundleBouncer') / 'BundleBouncer.csproj')

def configureProject(bcfg: BBBuildConfig, csproj_path: Path) -> None:
    log.info(f'Configuring {csproj_path}...')
    tree: etree._ElementTree = etree.parse(str(csproj_path.with_suffix('.csproj.in')))
    refgroup = None
    for ig in tree.findall('ItemGroup'):
        if len(ig.getchildren()) == 0:
            refgroup = ig
            break
    dlls = list(MANAGED_DIR.glob('*.dll'))
    dlls.append(bcfg.VRCHAT_DIR / 'MelonLoader' / 'MelonLoader.dll')
    dlls += [(MODS_DIR / x) for x in MOD_DEPENDENCIES]
    for dllpath in sorted(dlls, key=lambda x: x.stem):
        if dllpath.stem in BREAKS_SHIT:
            continue
        '''
        <Reference Include="Accessibility">
            <HintPath>G:\SteamLibrary\steamapps\common\VRChat\MelonLoader\Managed\Accessibility.dll</HintPath>
            <Private>false</Private>
        </Reference>
        '''
        ref = etree.SubElement(refgroup, 'Reference', {'Include': dllpath.stem})
        etree.SubElement(ref, 'HintPath', {}).text = str(dllpath.absolute())
        etree.SubElement(ref, 'Private', {}).text = 'false'

    etree.indent(tree)
    with open(csproj_path, 'wb') as f:
        tree.write(f, encoding='utf-8', xml_declaration=False, pretty_print=True)

def buildILRepack(bcfg: BBBuildConfig) -> None:
    oldcwd = Path.cwd().absolute()
    try:
        os.chdir(PROJ_ROOT / 'lib' / 'ilrepack')
        cmd(['cmd', '/c' , 'gradlew.bat', 'msbuild'])
        '''
        # Emulate gradlew.bat
        env = os.environ.copy()
        env['APP_NAME'] = 'Gradle'
        env['APP_BASE_NAME'] = 'ilrepack'
        env['APP_HOME'] = str(APP_HOME := Path.cwd().absolute())
        env['DEFAULT_JVM_OPTS'] = str(DEFAULT_JVM_OPTS := ["-Xmx64m", "-Xms64m"])
        env['CLASSPATH'] = (CLASSPATH := str(APP_HOME.relative_to(Path.cwd())/'gradle'/'wrapper'/'gradle_wrapper.jar'))
        
        cmd([str(bcfg.JAVA)]+DEFAULT_JVM_OPTS+[f'-Dorg.gradle.appname=ilrepack', 
                                               '-classpath', CLASSPATH, 
                                               'org.gradle.wrapper.GradleWrapperMain',
                                               'msbuild'], env=env)
        '''                                       
    finally:
        os.chdir(oldcwd)

def dotnetBuild(bcfg: BBBuildConfig, csproj: Path) -> None:
    cmd([str(bcfg.DOTNET),
        'build',
        '-c', 'Release', # Setting this to debug isn't going to help much.
        str(csproj)])

def main() -> None:
    bcfg = BBBuildConfig()

    bcfg.load()

    idiotCheck(bcfg)
    
    shutil.rmtree(DIST_DIR, ignore_errors=True)
    DIST_DIR.mkdir(parents=True, exist_ok=True)
    MELON_DIR = bcfg.VRCHAT_DIR / 'MelonLoader'
    MELON_MANAGED_DIR = MELON_DIR / 'Managed'

    buildILRepack(bcfg)
    configureProjects(bcfg)

    dotnetBuild(bcfg, CORE_PROJ_DIR / 'BundleBouncer.csproj')
    outfile = DIST_DIR / 'BundleBouncer.dll'
    cmd([str(ILREPACK),
        f'--out={outfile}',
        '--internalize',
        f'--lib={MELON_DIR}',
        f'--lib={MELON_MANAGED_DIR}',
        ]+[str(CORE_PROJ_DIR / 'dist' / 'net472' / x) for x in MERGE_ASSEMBLIES])

    dotnetBuild(bcfg, SHITLIST_PROJ_DIR / 'BundleBouncer.Shitlist.csproj')

if __name__ == '__main__':
    main()
