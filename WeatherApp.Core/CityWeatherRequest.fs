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

    let loadWeather appId request = observe {
        let url = urlStringForRequest appId request
        let request = CityWeatherType.AsyncLoad url
                      |> Observable.ofAsync
                      |> Observable.map convertWireTypeToLocalType
        yield! request
    }
    