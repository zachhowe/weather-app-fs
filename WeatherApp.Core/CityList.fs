namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Builders
open System

module public CityList =
    type private Request =
    | Query of string
    | CityID of int
    | Location of float * float

    let private urlStringForRequest appId request =
        let uriBuilder = UriBuilder("https://api.openweathermap.org/data/2.5/weather")
        let apiKey = ("appid", appId)
        match request with
        | Query(query) -> uriBuilder.Query <- UriQueryBuilder.formatQueryString [ apiKey; ("q", query) ]
        | CityID(cityId) -> uriBuilder.Query <- UriQueryBuilder.formatQueryString [ apiKey; ("id", (string cityId)) ]
        | Location(lat, lon) -> uriBuilder.Query <- UriQueryBuilder.formatQueryString [ apiKey; ("lat", (string lat)); ("lon", (string lon))]
        uriBuilder.Uri.AbsolutePath
    
    let private convertWireTypeToLocalType (city: CityWeatherType.Root) =
        { ID = city.Id; Name = city.Name }

    let private loadWeather appId query = observe {
        let url = urlStringForRequest appId query

        yield! CityWeatherType.AsyncLoad url
        |> Observable.ofAsync
        |> Observable.map convertWireTypeToLocalType
    }
    
    let private appId = "2095b1e3413a7e02ad01ee92b227a889"
    
    type public WeatherQuery =
    | Query of string
    | CityID of int
    | CityIDs of int list
    | Location of float * float

    let queryMultipleCities (cityIds: int list) = observe {
        let toLoadCityRequest = (fun cityId -> loadWeather appId (Request.CityID cityId))
        let cityRequests = cityIds |> List.map toLoadCityRequest

        for req in cityRequests do
            yield! req
    }

    let getCityWeather (q: WeatherQuery) = 
        match q with
        | Query(query) -> loadWeather appId (Request.Query query) |> Observable.toList
        | CityID(cityId) -> loadWeather appId (Request.CityID cityId) |> Observable.toList
        | CityIDs(cityIds) -> queryMultipleCities cityIds |> Observable.toList
        | Location(lat, lon) -> loadWeather appId (Request.Location (lat, lon)) |> Observable.toList
    
