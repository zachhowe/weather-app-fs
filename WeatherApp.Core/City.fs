namespace WeatherApp.Core

type CityIdentifier = private CityIdentifier of int

type public City = {
    ID: CityIdentifier
    Name: string
}
