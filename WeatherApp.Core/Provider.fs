namespace WeatherApp.Core

open System
open FSharp.Control
open FSharp.Control.Reactive

module Provider =
    open Api
    
    type CityWeatherProvider = {
        GetCityWeather: WeatherQuery -> IObservable<CityWeather list>
    }
    
    let create appId =
        let toLoadCityRequest (request: CityWeatherRequest) =
            request.LoadWeather appId

        let requestOfQuery query =
            match query with
            | WeatherQuery.Query(query) -> Query query |> List.singleton
            | WeatherQuery.CityID(cityId) -> CityID cityId |> List.singleton
            | WeatherQuery.CityIDs(cityIds) -> cityIds |> List.map CityID
            | WeatherQuery.Location(lat, lon) -> Location (lat, lon) |> List.singleton

        let getCityWeather (query: WeatherQuery) = 
            query
            |> requestOfQuery
            |> List.map toLoadCityRequest
            |> Observable.combineLatestSeq
            |> Observable.map List.ofSeq
    
        {
            GetCityWeather = getCityWeather
        }
