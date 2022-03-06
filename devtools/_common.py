
import os
import shlex
import shutil
import subprocess
from pathlib import Path
from typing import Dict, List, Optional

from click import UsageError
from ruamel.yaml import YAML as Yaml

yaml = Yaml(typ='rt')

import logging #isort: skip
log = logging.getLogger(__name__)

def cmd(cmdline: List[str], env: Optional[Dict[str,str]]=None) -> None:
    if env is None:
        env = os.environ
    echostr = list(map(shlex.quote, cmdline))
    log.info(f'$> {echostr}')
    subprocess.check_call(cmdline, env=env)

BUILDCONFIG_PATH = Path.cwd() / '.buildcfg.yml'
class BBBuildConfig:
    def __init__(self) -> None:
        self.CMAKE: Optional[Path] = None
        self.GRADLE: Optional[Path] = None
        self.DOTNET: Optional[Path] = None
        self.JAVA: Optional[Path] = None
        self.VRCHAT_DIR: Optional[Path] = None

    def load(self) -> None:
        if not BUILDCONFIG_PATH.is_file():
            self.detectPaths()
            self.save()
            raise UsageError(f'{BUILDCONFIG_PATH.absolute()} does not exist.  We created it for you with our best guesses, but you must edit it.')

        with open(BUILDCONFIG_PATH, 'r') as f:
            self.deserialize(yaml.load(f))

    def detectPaths(self) -> None:
        self.CMAKE = self.detectPathOfCommand('cmake', 'paths.cmake')
        self.GRADLE = self.detectPathOfCommand('gradle', 'paths.gradle')
        self.DOTNET = self.detectPathOfCommand('dotnet', 'paths.dotnet')
        self.JAVA = self.detectPathOfCommand('java', 'paths.java', warn_on_fail=False)
        if self.JAVA is None:
            if 'JAVA_HOME' in os.environ:
                JAVA_HOME = Path(os.environ['JAVA_HOME'])
                possibilities = [
                    JAVA_HOME / 'bin' / 'java',
                    JAVA_HOME / 'bin' / 'java.exe',
                    JAVA_HOME / 'jre' / 'sh' / 'java',
                ]
                for jpath in possibilities:
                    if jpath.is_file():
                        self.JAVA = jpath
                        log.info(f' -> java found at {self.JAVA}. Disregard previous message.')
                        break
        if self.JAVA is None:
            log.warning(f' ! Could not find command \'java\'.  Please set paths.java in buildcfg.yml.')
        self.VRCHAT_DIR = Path('.')

    def detectPathOfCommand(self, cmd: str, cfgpath: str, warn_on_fail: bool=True) -> Optional[str]:
        log.info(f'Looking for {cmd}...')
        if (guess := shutil.which(cmd)) is not None:
            log.info(f' -> {cmd} found at {guess}!')
            return Path(guess)
        if warn_on_fail:
            log.warning(f' ! Could not find command {cmd!r}.  Please set {cfgpath} in buildcfg.yml.')
        return None

    def save(self) -> None:
        with open(BUILDCONFIG_PATH, 'w') as f:
            yaml.dump(self.serialize(), f)

    def _getOptionalPath(self, data: dict, key: str) -> Optional[Path]:
        v = data.get(key)
        return Path(v) if v is not None else v

    def deserialize(self, data: dict) -> None:
        paths = data['paths']
        self.CMAKE = self._getOptionalPath(paths, 'cmake')
        self.DOTNET = self._getOptionalPath(paths, 'dotnet')
        self.GRADLE = self._getOptionalPath(paths, 'gradle')
        self.JAVA = self._getOptionalPath(paths, 'java')
        self.VRCHAT_DIR = self._getOptionalPath(paths, 'vrchat')

    def dump(self) -> str:
        kv = {
            'CMAKE':      self.CMAKE,
            'DOTNET':     self.DOTNET,
            'GRADLE':     self.GRADLE,
            'JAVA':       self.JAVA,
            'VRCHAT_DIR': self.VRCHAT_DIR,
        }
        o = ''
        for k,v in sorted(kv.items()):
            o += f'{k:<10}: {v}\n'
        return o

    def serialize(self) -> dict:
        return {
            'paths': {
                'cmake':  str(self.CMAKE) if self.CMAKE is not None else None,
                'dotnet': str(self.DOTNET) if self.DOTNET is not None else None,
                'gradle': str(self.GRADLE) if self.GRADLE is not None else None,
                'java': str(self.JAVA) if self.JAVA is not None else None,
                'vrchat': str(self.VRCHAT_DIR) if self.VRCHAT_DIR is not None else None,
            }
        }

