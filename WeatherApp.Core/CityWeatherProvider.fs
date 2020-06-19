namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Builders

module internal CityWeatherRequest =
    type Request =
    | Query of string
    | CityID of int
    | Location of float * float

    let private urlStringForRequest appId request =
        let uriString = "https://api.openweathermap.org/data/2.5/weather"
        let apiKey = ("appid", appId)
        match request with
        | Query(query) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("q", query) ]
        | CityID(cityId) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("id", (string cityId)) ]
        | Location(lat, lon) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("lat", (string lat)); ("lon", (string lon))]

    let private convertWireTypeToLocalType (city: CityWeatherType.Root) =
        { City = { ID = city.Id; Name = city.Name }
          Weather = { Temp = city.Main.Temp; FeelsLike = city.Main.FeelsLike } }

    let loadWeather appId query = observe {
        let url = urlStringForRequest appId query
        let request = CityWeatherType.AsyncLoad url
                       |> Observable.ofAsync
                       |> Observable.map convertWireTypeToLocalType
        yield! request
    }
    
module public CityWeatherProvider =
    open CityWeatherRequest
    

    let private appId = "2095b1e3413a7e02ad01ee92b227a889"

    let private toLoadCityRequest = (fun cityId -> loadWeather appId (Request.CityID cityId))

    let private queryMultipleCities (cityIds: int list) = observe {
        let cityRequests = cityIds |> List.map toLoadCityRequest

        for req in cityRequests do
            yield! req
    }

    let getCityWeather (q: WeatherQuery) = 
        match q with
        | WeatherQuery.Query(query) -> loadWeather appId (Request.Query query)
        | WeatherQuery.CityID(cityId) -> loadWeather appId (Request.CityID cityId)
        | WeatherQuery.CityIDs(cityIds) -> queryMultipleCities cityIds
        | WeatherQuery.Location(lat, lon) -> loadWeather appId (Request.Location (lat, lon))
    
