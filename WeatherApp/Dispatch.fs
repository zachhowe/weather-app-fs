namespace WeatherApp

open CoreFoundation

module Dispatch =
    let mainAsync (x: unit -> unit) = 
        DispatchQueue.MainQueue.DispatchAsync x
