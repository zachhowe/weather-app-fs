module Tests

open Xunit
open WeatherApp.Core
open FsCheck
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Testing

[<Fact>]
let ``My test`` () =
    Assert.True(true)
