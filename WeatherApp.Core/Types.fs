namespace WeatherApp.Core

type public City = { 
    Name: string
    ID: int
}

type public Weather = { 
    Temp: float
    FeelsLike: float
}

type public CityWeather = {
    City: City;
    Weather: Weather
}
