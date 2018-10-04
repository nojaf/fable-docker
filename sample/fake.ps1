param (
    [string]$target = "Build"
)

$BuildPackages = ".\.fake"

if (-Not (Test-Path "$BuildPackages\fake.exe")) {
    & dotnet tool install fake-cli --tool-path "$BuildPackages"
}

& "$BuildPackages/fake.exe" run build.fsx --target $target