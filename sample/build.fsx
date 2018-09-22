#r "paket: 
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Process
nuget Fake.Core.Target
nuget Fake.JavaScript.Npm //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open System
open System.IO
open Fake.IO
open Fake.DotNet
open Fake.JavaScript

let paketExe = Path.Combine(__SOURCE_DIRECTORY__, ".paket", "paket")

// Default target
Target.create "Install" (fun _ ->
  Trace.trace "FAKE installed deps"
)

Target.create "InstallPaket" (fun _ ->
    if not (File.exists paketExe) then
        DotNet.exec id "tool" "install --tool-path \".paket\" Paket --version 5.182.0-alpha001 --add-source https://api.nuget.org/v3/index.json"
        |> ignore
    else
        printfn "paket already installed"
)

Target.create "Restore" (fun _ ->
    Process.directExec (fun p -> { p with FileName = paketExe ; Arguments = "restore" })
    |> ignore

    DotNet.restore (DotNet.Options.withWorkingDirectory "src") "App.fsproj"

    Npm.install id
)

Target.create "Build" (fun _ ->
  DotNet.exec (DotNet.Options.withWorkingDirectory "src") "fable" "webpack --port free -- -p"
  |> ignore
)

Target.create "Watch" (fun _ ->
  async {
      let result =
          DotNet.exec
              (DotNet.Options.withWorkingDirectory "src")
              "fable"
              "webpack-dev-server --port free"
      if not result.OK then failwithf "dotnet fable failed with code %i" result.ExitCode
  }
  |> Async.RunSynchronously
)

"InstallPaket" ==> "Restore" ==> "Build"

"Watch"
    <== [ "Restore" ] 
 
// start build
Target.runOrDefault "Install"