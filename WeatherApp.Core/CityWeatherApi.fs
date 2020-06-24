namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Builders

module public CityWeatherApi =
    type City with
        static member private FromWireType (city: CityWeatherType.Root) =
            { City = { ID = city.Id; Name = city.Name }
              Weather = { Temp = city.Main.Temp; FeelsLike = city.Main.FeelsLike } }

    type CityWeatherRequest =
    | Query of string
    | CityID of int
    | Location of float * float
        member private this.UrlStringForRequest appId =
            let uriString = "https://api.openweathermap.org/data/2.5/weather"
            let apiKey = ("appid", appId)
            match this with
            | Query(query) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("q", query) ]
            | CityID(cityId) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("id", (string cityId)) ]
            | Location(lat, lon) -> uriString + "?" + UriQueryBuilder.formatQueryString [ apiKey; ("lat", (string lat)); ("lon", (string lon))]
        
        member public this.LoadWeather appId = observe {
            let url = this.UrlStringForRequest appId
            let request = CityWeatherType.AsyncLoad url
                            |> Observable.ofAsync
                            |> Observable.map City.FromWireType
            yield! request
        }
