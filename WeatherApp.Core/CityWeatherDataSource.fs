namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open System.Reactive.Disposables
open System.Reactive.Subjects

[<Sealed>]
type public CityWeatherDataSource() =
    
    member val private DisposeBag = new CompositeDisposable()
    
    member val CityWeather = new BehaviorSubject<CityWeather list>(List.empty)
    
    member this.AddCitiesFromQuery(query: WeatherQuery) =
        let currentCityWeathers = this.CityWeather.Value
        
        CityWeatherProvider.getCityWeather query
        |> Observable.subscribeSafe (fun x -> this.CityWeather.OnNext(currentCityWeathers |> List.append x))
        |> Disposable.disposeWith this.DisposeBag
    
    member __.RefreshAll() =
        ()
        