namespace WeatherApp

open Foundation
open UIKit

[<Register("AppDelegate")>]
type AppDelegate() =
    inherit UIApplicationDelegate()

    override val Window = null with get, set
    
    override this.FinishedLaunching(_app, _options) =
        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- new CityListViewController(new CityListView())
        this.Window.MakeKeyAndVisible()

        true
