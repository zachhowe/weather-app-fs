namespace WeatherApp.Core

[<Struct>]
type public Coordinate = {
    Latitude: decimal
    Longitude: decimal
}

[<Struct>]
type public City = {
    ID: int
    Name: string
    Coordinate: Coordinate
}
