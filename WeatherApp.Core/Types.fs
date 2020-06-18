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
