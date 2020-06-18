namespace WeatherApp.Core

open FSharp.Data
open System

module UriQueryBuilder =
    type QueryItem = string * string

    let formatQueryString (items: QueryItem list) =
        items
        |> List.map (fun (one, two) -> (Uri.EscapeUriString one) + "=" + (Uri.EscapeUriString two))
        |> String.join "&"
