#r "paket: groupref Build //"
#load @".fake/build.fsx/intellisense.fsx"

#if !FAKE
  #r "netstandard"
#endif

open System.IO

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators

let srcGlob = "src/**/*.?sproj"
let testsGlob = "tests/**/*.?sproj"

Target.create "Clean" (fun _ ->
    ["bin"; "temp"]
    |> Shell.cleanDirs

    !! srcGlob
    ++ testsGlob
    |> Seq.collect(fun p ->
        ["bin";"obj"]
        |> Seq.map(fun sp ->
            Path.GetDirectoryName p @@ sp)
        )
    |> Shell.cleanDirs)

Target.create "Restore" (fun _ ->
    DotNet.restore id "NetCorePaketFake.sln")

Target.create "Build" (fun _ ->
    DotNet.build id "NetCorePaketFake.sln")

"Clean"
==> "Restore"
==> "Build"

Target.runOrDefault "Build"