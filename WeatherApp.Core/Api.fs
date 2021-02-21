namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive

module Api =
    type City with
        static member private FromWireType (wire: CityWeatherType.Root) =
            { City = { ID = wire.Id
                       Name = wire.Name
                       Coordinate = { Latitude = wire.Coord.Lat
                                      Longitude = wire.Coord.Lon } }
              Weather = Some { Temp = wire.Main.Temp
                               FeelsLike = wire.Main.FeelsLike } }

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
        
        member public this.LoadWeather appId =
            CityWeatherType.AsyncLoad (this.UrlStringForRequest appId)
            |> Observable.ofAsync
            |> Observable.map City.FromWireType
