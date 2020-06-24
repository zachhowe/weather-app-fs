namespace WeatherApp.Core

type public WeatherQuery =
| Query of string
| CityID of int
| CityIDs of int list
| Location of float * float
