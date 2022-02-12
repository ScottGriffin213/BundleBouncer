:: Run git submodule update --init --recursive first.
:: You also need to set this:
set VRCHAT_DIR=G:\SteamLibrary\steamapps\common\VRChat\MelonLoader
echo VRCHAT_DIR=%VRCHAT_DIR%

cd lib\ilrepack
call gradlew.bat msbuild
cd ..\..
dotnet build -c Release BundleBouncer\BundleBouncer.csproj
dotnet build -c Release BundleBouncer.Shitlist\BundleBouncer.Shitlist.csproj
copy /Y BundleBouncer.Shitlist\dist\net472\BundleBouncer.Shitlist.dll dist\
lib\ilrepack\ILRepack\bin\Release\ILRepack.exe /out:dist\BundleBouncer.dll /internalize ^
    /lib:%VRCHAT_DIR% ^
    /lib:%VRCHAT_DIR%\Managed ^
    BundleBouncer\dist\net472\BundleBouncer.dll ^
    BundleBouncer\dist\net472\BouncyCastle.Crypto.dll ^
    BundleBouncer\dist\net472\dnYARA.dll ^
    BundleBouncer\dist\net472\dnYARA.Interop.dll ^
    BundleBouncer\dist\net472\Mono.Posix.NETStandard.dll ^
    BundleBouncer\dist\net472\System.Diagnostics.DiagnosticSource.dll
