namespace WeatherApp.Core

open System

module String =
    let join (separator : string) (strings : string list) =
        String.Join(separator, strings)

    let toCharArray (s: string) =
        s.ToCharArray()
        
    let split (separator : string) (stringToSplit : string) =
        stringToSplit.Split (separator |> toCharArray)
        