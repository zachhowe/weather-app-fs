namespace WeatherApp

open FSharp.Control.Reactive
open Foundation
open System
open System.Reactive.Disposables
open UIKit
open WeatherApp.Core

module CityListViewUi =
    open UITableViewExtensions

    type CityTableViewCellModel = {
        Name: string
        Status: string
        Temperature: string
    }

    type CityTableViewCellModel with
        static member FromCityWeather(cityWeather: CityWeather) : CityTableViewCellModel =
            { Name = cityWeather.City.Name
              Temperature = (sprintf "%d K" (cityWeather.Weather.Temp |> Decimal.ToInt32))
              Status = "Cloudy" }

    [<Sealed>]
    [<Register("CityTableViewCell")>]
    type CityTableViewCell(handle: IntPtr) as this =
        inherit UITableViewCell(handle)

        let nameLabel: UILabel =
            let label = new UILabel()
            label.Font <- UIFont.BoldSystemFontOfSize (nfloat (float 20))
            label.TranslatesAutoresizingMaskIntoConstraints <- false
            label

        let statusLabel: UILabel =
            let label = new UILabel()
            label.Font <- UIFont.SystemFontOfSize (nfloat (float 14))
            label.TranslatesAutoresizingMaskIntoConstraints <- false
            label

        let temperatureLabel: UILabel =
            let label = new UILabel()
            label.Font <- UIFont.BoldSystemFontOfSize (nfloat (float 20))
            label.TranslatesAutoresizingMaskIntoConstraints <- false
            label

        do
            this.BackgroundColor <- UIColor.White
            this.ContentView.AddSubview nameLabel
            this.ContentView.AddSubview statusLabel
            this.ContentView.AddSubview temperatureLabel

            NSLayoutConstraint.ActivateConstraints 
                [|
                    nameLabel.TopAnchor.ConstraintEqualTo(this.ContentView.TopAnchor, (nfloat (float 12)))
                    nameLabel.LeadingAnchor.ConstraintEqualTo(this.ContentView.LeadingAnchor, (nfloat (float 12))) 
                    nameLabel.TrailingAnchor.ConstraintEqualTo(this.ContentView.TrailingAnchor, (nfloat (float -12)))
        
                    statusLabel.TopAnchor.ConstraintEqualTo(nameLabel.BottomAnchor, (nfloat (float 8)))
                    statusLabel.LeadingAnchor.ConstraintEqualTo(this.ContentView.LeadingAnchor, (nfloat (float 12))) 
                    statusLabel.BottomAnchor.ConstraintEqualTo(this.ContentView.BottomAnchor, (nfloat (float -12)))
        
                    temperatureLabel.CenterYAnchor.ConstraintEqualTo(this.ContentView.CenterYAnchor)
                    temperatureLabel.TrailingAnchor.ConstraintEqualTo(this.ContentView.TrailingAnchor, (nfloat (float -12))) 
                    temperatureLabel.BottomAnchor.ConstraintEqualTo(this.ContentView.BottomAnchor, (nfloat (float -12)))
                |]
    
        member __.Configure(viewModel: CityTableViewCellModel) =
            nameLabel.Text <- viewModel.Name
            statusLabel.Text <- viewModel.Status
            temperatureLabel.Text <- viewModel.Temperature

    [<Sealed>]
    [<Register("CityListView")>]
    type CityListView() as this =
        inherit UIView()

        let tableView: UITableView = 
            let tableView = new UITableView()
            tableView.TranslatesAutoresizingMaskIntoConstraints <- false
            tableView
        
        do
            this.BackgroundColor <- UIColor.White
            this.AddSubview tableView

            NSLayoutConstraint.ActivateConstraints 
                [|
                    tableView.TopAnchor.ConstraintEqualTo(this.TopAnchor)
                    tableView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor) 
                    tableView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor)
                    tableView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor)
                |]
    
        member __.TableView = tableView

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
                        let cityWeather = weather.[indexPath.Row]
                        let cell: CityTableViewCell = tableView.DequeueCell(indexPath)
                        cell.Configure (CityTableViewCellModel.FromCityWeather cityWeather)
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

    let public create (dataSource: CityWeatherDataSource) =
        let cityListView = new CityListView()
        let cityListViewController = new CityListViewController(cityListView, dataSource)
        cityListViewController