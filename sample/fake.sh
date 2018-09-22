#!/usr/bin/env bash
Target=${1:-Build}
BuildPackages="./.fake"
FakeExe=$"BuildPackages/fake"

if [ ! -f $FakeExe ]; then
    dotnet tool install fake-cli --tool-path $BuildPackages
fi

# if [ -f "build.fsx.lock" ]; then 
    # rm "build.fsx.lock"
# fi

$FakeExe run build.fsx --target $Target