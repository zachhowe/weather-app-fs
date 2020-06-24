namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive

open CityWeatherApi

type public CityWeatherProvider(appId) =
    let appId = appId

    let toLoadCityRequest (request: CityWeatherRequest) =
        request.LoadWeather appId

    let requestOfQuery query =
        match query with
        | WeatherQuery.Query(query) -> Query query |> List.ofOne
        | WeatherQuery.CityID(cityId) -> CityID cityId |> List.ofOne
        | WeatherQuery.CityIDs(cityIds) -> cityIds |> List.map CityID
        | WeatherQuery.Location(lat, lon) -> Location (lat, lon) |> List.ofOne

    let getCityWeather (query: WeatherQuery) = 
        query
        |> requestOfQuery
        |> List.map toLoadCityRequest
        |> Observable.combineLatestSeq
        |> Observable.map List.ofSeq

    member __.GetCityWeather = getCityWeather
