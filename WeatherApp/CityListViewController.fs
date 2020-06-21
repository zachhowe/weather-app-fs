﻿namespace WeatherApp

open FSharp.Control.Reactive
open Foundation
open System
open System.Reactive.Disposables
open UIKit
open WeatherApp.Core


[<Register("CityListViewController")>]
type CityListViewController(ui: CityListView) =
    inherit UIViewController(null, null)

    let ui = ui
    
    let disposeBag = new CompositeDisposable()
    
    let mutable Weather: CityWeather array = Array.empty
    
    let tableViewSource = { new UITableViewSource() with
                           member __.RowsInSection(_tableView: UITableView, _section: nint) : nint =
                               nint (Weather |> Array.length)

                           member __.GetCell(tableView: UITableView, indexPath: NSIndexPath) : UITableViewCell =
                               let city = Weather.[indexPath.Row]
                               let cell = tableView.DequeueReusableCell("CityTableViewCell", indexPath) :?> CityTableViewCell
                               cell.Configure { Name = city.City.Name
                                                Temperature = (sprintf "%d K" (city.Weather.Temp |> Decimal.ToInt32))
                                                Status = "Cloudy" }
                               cell :> UITableViewCell }

    override this.LoadView() =
        this.View <- ui
    
    member __.AddCells(cityWeathers: CityWeather array) =
        printfn "Got data"
        Weather <- cityWeathers
    
    member __.ErrorLoadingWeather(error) =
        printfn "Error getting weather: %s" (string error)
        
    member __.CompletedLoading() =
        printfn "Done getting weather"
        Dispatch.mainAsync ui.TableView.ReloadData
    
    override this.ViewDidLoad() = 
        base.ViewDidLoad()
        
        ui.TableView.RegisterClassForCellReuse(typedefof<CityTableViewCell>, "CityTableViewCell")
        ui.TableView.Source <- tableViewSource
        
        CityWeatherProvider.getCityWeather (CityIDs [ 5368361; 5391811 ])
        |> Observable.toArray
        |> Observable.subscribeSafeWithCallbacks this.AddCells this.ErrorLoadingWeather this.CompletedLoading
        |> Disposable.disposeWith disposeBag
