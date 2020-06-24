﻿namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
    
module public CityWeatherProvider =
    open CityWeatherApi
   
    let private appId = "2095b1e3413a7e02ad01ee92b227a889"

    let private toLoadCityRequest (request: CityWeatherRequest) =
        request.LoadWeather appId
        
    let private requestOfQuery query =
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
