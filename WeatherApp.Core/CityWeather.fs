namespace WeatherApp.Core

[<Struct>]
type public CityWeather = {
    City: City
    Weather: Weather option
}
