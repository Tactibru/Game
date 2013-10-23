rm -rf bin/

mkdir -p bin/build/Windows
mkdir -p bin/build/Linux/x86
mkdir bin/build/Linux/x86_64
mkdir bin/build/Mac
mkdir "bin/build/Web Player"
mkdir bin/build/log

"C:\Program Files (x86)\Unity\Editor\Unity.exe" -batchmode -quit -nographics -logFile "bin/build/BuildLog" -buildWebPlayer "bin/build/Web Player" -buildWindowsPlayer "bin/build/Windows" -buildOSXPlayer "bin/build/Mac" -buildLinux32Player "bin/build/Linux/x86" -buildLinux64Player "bin/build/Linux/x86_64"