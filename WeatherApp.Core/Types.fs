namespace WeatherApp.Core

type public City = {
    ID: int
    Name: string
}

type public Weather = { 
    Temp: decimal
    FeelsLike: decimal
}

type public CityWeather = {
    City: City
    Weather: Weather
}

type public WeatherQuery =
| Query of string
| CityID of int
| CityIDs of int list
| Location of float * float
