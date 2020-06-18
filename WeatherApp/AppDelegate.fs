﻿namespace WeatherApp

open Foundation
open UIKit

[<Register("AppDelegate")>]
type AppDelegate() =
    inherit UIApplicationDelegate()

    override val Window = null with get, set

    // This method is invoked when the application is ready to run.
    override this.FinishedLaunching(app, options) =
        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- new CityListViewController(new CityListView())
        this.Window.MakeKeyAndVisible()

        true
