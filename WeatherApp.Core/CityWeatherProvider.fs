namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
    
module public CityWeatherProvider =
    open CityWeatherRequest
   
    let private appId = "2095b1e3413a7e02ad01ee92b227a889"

    let private toLoadCityRequest request =
        loadWeather appId request
        
    let private requestOfQuery query =
        match query with
        | WeatherQuery.Query(query) -> Request.Query query |> List.ofOne
        | WeatherQuery.CityID(cityId) -> Request.CityID cityId |> List.ofOne
        | WeatherQuery.CityIDs(cityIds) -> cityIds |> List.map Request.CityID
        | WeatherQuery.Location(lat, lon) -> Request.Location (lat, lon) |> List.ofOne

    let getCityWeather (query: WeatherQuery) = 
        query
        |> requestOfQuery
        |> List.map toLoadCityRequest
        |> Observable.combineLatestSeq
        |> Observable.map List.ofSeq
