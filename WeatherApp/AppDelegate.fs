namespace WeatherApp

open Foundation
open UIKit

[<Register("AppDelegate")>]
type AppDelegate() =
    inherit UIApplicationDelegate()

    override val Window = null with get, set
    
    override this.FinishedLaunching(_app, _options) =
        let cityListViewController = new CityListViewController(new CityListView())
        let navigationController = new UINavigationController(cityListViewController)
        
        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- navigationController
        this.Window.MakeKeyAndVisible()

        true
