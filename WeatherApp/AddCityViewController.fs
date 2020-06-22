namespace WeatherApp

open Foundation
open UIKit
open WeatherApp.Core
open System.Reactive.Disposables

[<Sealed>]
[<Register("AddCityViewController")>]
type AddCityViewController(ui: AddCityView, dataSource: CityWeatherDataSource) =
    inherit UIViewController(null, null)

    let ui = ui

    member val DataSource = dataSource with get
    member val DisposeBag = new CompositeDisposable() with get
    
    override this.LoadView() =
        this.View <- ui
