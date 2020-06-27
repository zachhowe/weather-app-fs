namespace WeatherApp.Core

type public Coordinate = {
    Latitude: decimal
    Longitude: decimal
}

type public City = {
    ID: int
    Name: string
    Coordinate: Coordinate
}
