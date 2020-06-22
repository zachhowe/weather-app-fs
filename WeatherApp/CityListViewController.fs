namespace WeatherApp

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

    let dataSource = CityWeatherDataSource()
    let disposeBag = new CompositeDisposable()
    
    let mutable Weather: CityWeather list = List.empty
    
    override this.LoadView() =
        this.View <- ui
        
    override this.ViewDidLoad() = 
        base.ViewDidLoad()
        
        this.ConfigureNavigationItem()
        this.ConfigureTableView()
        this.ConfigureBindings()
        
        dataSource.AddCitiesFromQuery (CityIDs [ 5368361; 5391811 ])
        
    member private this.ConfigureNavigationItem() =
        this.NavigationItem.Title <- "Weather"

    member private this.ConfigureTableView() =
        ui.TableView.RegisterClassForCellReuse(typedefof<CityTableViewCell>, "CityTableViewCell")
        ui.TableView.Source <- { new UITableViewSource() with
            member __.RowsInSection(_tableView: UITableView, _section: nint) : nint =
                nint (Weather |> List.length)
            member __.GetCell(tableView: UITableView, indexPath: NSIndexPath) : UITableViewCell =
                let city = Weather.[indexPath.Row]
                let cell = tableView.DequeueReusableCell("CityTableViewCell", indexPath) :?> CityTableViewCell
                cell.Configure { Name = city.City.Name
                                 Temperature = (sprintf "%d K" (city.Weather.Temp |> Decimal.ToInt32))
                                 Status = "Cloudy" }
                cell :> UITableViewCell
        }
    
    member private this.ConfigureBindings() =
        dataSource.CityWeather
        |> Observable.subscribeSafeWithError this.AddCells this.ErrorLoadingWeather
        |> Disposable.disposeWith disposeBag
    
    member private __.AddCells(cityWeathers: CityWeather list) =
        printfn "Got data"
        Weather <- cityWeathers
        Dispatch.mainAsync ui.TableView.ReloadData
    
    member private __.ErrorLoadingWeather(error) =
        printfn "Error getting weather: %s" (string error)