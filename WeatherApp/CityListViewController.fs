namespace WeatherApp

open Foundation
open UIKit

[<Register("CityListViewController")>]
type CityListViewController(ui: CityListView) =
    inherit UIViewController(null, null)

    let ui = ui

    override x.LoadView() = x.View <- ui

    override x.ViewDidLoad() = 
        base.ViewDidLoad()
    
        let result = CityListProvider.getWeather "Mar Vista"
                     |> Async.RunSynchronously
        
        printfn "Result: %s" result.Value.Name
    