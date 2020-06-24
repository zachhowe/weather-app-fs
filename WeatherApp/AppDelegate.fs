namespace WeatherApp

open Foundation
open UIKit
open WeatherApp.Core

[<Sealed>]
[<Register("AppDelegate")>]
type AppDelegate() =
    inherit UIApplicationDelegate()

    override val Window = null with get, set

    override this.FinishedLaunching(_app, _options) =
        let cityListView = new CityListView()
        let dataSource = CityWeatherProvider "2095b1e3413a7e02ad01ee92b227a889" |> CityWeatherDataSource
        let cityListViewController = new CityListViewController(cityListView, dataSource)
        let navigationController = new UINavigationController(cityListViewController)
        
        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- navigationController
        this.Window.MakeKeyAndVisible()

        true
