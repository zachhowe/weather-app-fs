namespace WeatherApp

open CoreFoundation

module Dispatch =
    let mainAsync (x: unit -> unit) = 
        DispatchQueue.MainQueue.DispatchAsync x

    let backgroundAsync (x: unit -> unit) =
        DispatchQueue.GetGlobalQueue(DispatchQualityOfService.Background).DispatchAsync x
