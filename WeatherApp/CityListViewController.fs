namespace WeatherApp

open FSharp.Control.Reactive
open Foundation
open System
open System.Reactive.Disposables
open UIKit
open WeatherApp.Core
open UITableViewExtensions

[<Sealed>]
[<Register("CityListViewController")>]
type CityListViewController(ui: CityListView, dataSource: CityWeatherDataSource) as this =
    inherit UIViewController(null, null)

    let ui = ui
    let dataSource = dataSource
    let disposeBag = new CompositeDisposable()

    let mutable weather: CityWeather list = List.empty
    
    let refresh = ui.TableView.ReloadData

    let configureNavigationItem =
        fun () ->
            this.NavigationItem.Title <- "Weather"
    
    let configureBindings =
        let onWeatherLoaded cityWeathers =
            printfn "Got data: %d" (cityWeathers |> List.length)
            weather <- cityWeathers
            Dispatch.mainAsync refresh
            
        let onWeatherLoadError error =
            printfn "Error getting weather: %s" (string error)

        fun () ->
            dataSource.CityWeather
            |> Observable.subscribeSafeWithError onWeatherLoaded onWeatherLoadError
            |> Disposable.disposeWith disposeBag

    let configureTableView = 
        fun () ->
            ui.TableView.RegisterCell typedefof<CityTableViewCell>
            ui.TableView.Source <- { new UITableViewSource() with
                member __.RowsInSection(_tableView: UITableView, _section: nint) : nint =
                    nint (weather |> List.length)
                member __.GetCell(tableView: UITableView, indexPath: NSIndexPath) : UITableViewCell =
                    let city = weather.[indexPath.Row]
                    let cell: CityTableViewCell = tableView.DequeueCell(indexPath)
                    cell.Configure { Name = city.City.Name
                                     Temperature = (sprintf "%d K" (city.Weather.Temp |> Decimal.ToInt32))
                                     Status = "Cloudy" }
                    cell :> UITableViewCell
            }
            
    let loadView =
        this.View <- ui

    let viewDidLoad =
        base.ViewDidLoad()
        
        configureNavigationItem()
        configureTableView()
        configureBindings()
        
        dataSource.AddCitiesFromQuery (CityIDs [ 5368361; 5391811 ])

    override __.LoadView() = loadView
    override __.ViewDidLoad() = viewDidLoad
