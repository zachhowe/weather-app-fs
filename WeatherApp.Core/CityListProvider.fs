namespace WeatherApp.Core

open FSharp.Data
open System

module public CityListProvider =
    let private apiKey = "2095b1e3413a7e02ad01ee92b227a889"
    
    type private CityWeatherType = 
        JsonProvider<"""
                    {"coord":{"lon":80.99,"lat":7.04},"weather":[{"id":803,"main":"Clouds","description":"broken clouds","icon":"04d"}],"base":"stations","main":{"temp":302.89,"feels_like":304.65,"temp_min":302.89,"temp_max":302.89,"pressure":1006,"humidity":51,"sea_level":1006,"grnd_level":910},"wind":{"speed":1.79,"deg":27},"clouds":{"all":63},"dt":1592031842,"sys":{"country":"LK","sunrise":1592007624,"sunset":1592052720},"timezone":19800,"id":1250615,"name":"Badulla","cod":200}
                    """>

    let mutable CityIds : int list = List.empty

    let queryWeather query =
        async {
            try
                let uriBuilder = UriBuilder("https://api.openweathermap.org/data/2.5/weather") 
                uriBuilder.Query <- HttpQueryBuilder.formatQueryString [ 
                    ("appid", apiKey)
                    ("q", query)
                ]
                let url = uriBuilder.Uri.AbsolutePath
                let! result = CityWeatherType.AsyncLoad url
                return Success result
            with
                ex ->
                    printfn "Exception: %s" ex.Message
                    return Failure ex
        }
        
    let public addCityByQuery query =
        async {
            let! result = queryWeather query

            CityIds <- result
                    |> Result.value
                    |> Option.toList
                    |> List.map (fun (city) -> city.Id)
                    |> List.append CityIds
        }
