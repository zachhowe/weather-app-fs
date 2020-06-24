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
type CityListViewController(ui: CityListView, dataSource: CityWeatherDataSource) =
    inherit UIViewController(null, null)

    let ui = ui

    member val private DataSource = dataSource with get
    member val private DisposeBag = new CompositeDisposable() with get
    member val private Weather: CityWeather list = List.empty with get, set
    
    override this.LoadView() =
        this.View <- ui
        
    override this.ViewDidLoad() = 
        base.ViewDidLoad()
        
        this.ConfigureNavigationItem()
        this.ConfigureTableView()
        this.ConfigureBindings()
        
        this.DataSource.AddCitiesFromQuery (CityIDs [ 5368361; 5391811 ])
        
    member private this.ConfigureNavigationItem() =
        this.NavigationItem.Title <- "Weather"

    member private this.ConfigureTableView() =
        ui.TableView.RegisterCell typedefof<CityTableViewCell>
        ui.TableView.Source <- { new UITableViewSource() with
            member __.RowsInSection(_tableView: UITableView, _section: nint) : nint =
                nint (this.Weather |> List.length)
            member __.GetCell(tableView: UITableView, indexPath: NSIndexPath) : UITableViewCell =
                let city = this.Weather.[indexPath.Row]
                let cell: CityTableViewCell = tableView.DequeueCell(indexPath)
                cell.Configure { Name = city.City.Name
                                 Temperature = (sprintf "%d K" (city.Weather.Temp |> Decimal.ToInt32))
                                 Status = "Cloudy" }
                cell :> UITableViewCell
        }
    
    member private this.ConfigureBindings() =
        this.DataSource.CityWeather
        |> Observable.subscribeSafeWithError this.AddCells this.ErrorLoadingWeather
        |> Disposable.disposeWith this.DisposeBag
    
    member private this.AddCells(cityWeathers: CityWeather list) =
        printfn "Got data: %d" (cityWeathers |> List.length)
        this.Weather <- cityWeathers
        Dispatch.mainAsync ui.TableView.ReloadData
    
    member private __.ErrorLoadingWeather(error) =
        printfn "Error getting weather: %s" (string error)