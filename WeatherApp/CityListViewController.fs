namespace WeatherApp

open Foundation
open UIKit
open WeatherApp.Core

[<Register("CityListViewController")>]
type CityListViewController(ui: CityListView) =
    inherit UIViewController(null, null)

    let ui = ui

    override x.LoadView() = x.View <- ui

    override _.ViewDidLoad() = 
        base.ViewDidLoad()

    