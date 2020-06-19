namespace WeatherApp.Core

open FSharp.Control.Reactive.Builders
    
module public CityWeatherProvider =
    open CityWeatherRequest
   
    let private appId = "2095b1e3413a7e02ad01ee92b227a889"

    let private toLoadCityRequest = 
        (fun cityId -> loadWeather appId (Request.CityID cityId))

    let private queryMultipleCityIds (cityIds: int list) = observe {
        let cityRequests = cityIds |> List.map toLoadCityRequest

        for req in cityRequests do
            yield! req
    }

    let getCityWeather (query: WeatherQuery) = 
        match query with
        | WeatherQuery.Query(query) -> loadWeather appId (Request.Query query)
        | WeatherQuery.CityID(cityId) -> loadWeather appId (Request.CityID cityId)
        | WeatherQuery.CityIDs(cityIds) -> queryMultipleCityIds cityIds
        | WeatherQuery.Location(lat, lon) -> loadWeather appId (Request.Location (lat, lon))
