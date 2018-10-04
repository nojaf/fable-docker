#r "paket: 
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Process
nuget Fake.Core.Target
nuget Fake.DotNet.Paket
nuget Fake.JavaScript.Npm //"
#load "./.fake/build.fsx/intellisense.fsx"

#if !FAKE
  #r "netstandard"
#endif

open Fake.Core
open Fake.Core.TargetOperators
open System.IO
open Fake.IO
open Fake.DotNet
open Fake.JavaScript

let paketFile = if Environment.isLinux then "paket" else "paket.exe"
let paketExe = Path.Combine(__SOURCE_DIRECTORY__, ".paket", paketFile)

// Default target
Target.create "Install" (fun _ ->
  Trace.trace "FAKE installed deps"
)

// Target.create "InstallPaket" (fun _ ->
//     if not (File.exists paketExe) then
//         DotNet.exec id "tool" "install --tool-path \".paket\" Paket --version 5.182.0-alpha001 --add-source https://api.nuget.org/v3/index.json"
//         |> ignore
//     else
//         printfn "paket already installed"
// )

Target.create "Restore" (fun _ ->
    // Paket.restore id

    DotNet.restore (DotNet.Options.withWorkingDirectory "src") "App.fsproj"

    Npm.install id
)

Target.create "Build" (fun _ ->
  DotNet.exec (DotNet.Options.withWorkingDirectory "src") "fable" "webpack-cli --port free -- -p"
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

"Restore" ==> "Build"

"Watch"
    <== [ "Restore" ] 
 
// start build
Target.runOrDefault "Install"