namespace WeatherApp.Core

open System

module String =
    let join (seperator : string) (strings : string list) =
        String.Join(seperator, strings)
