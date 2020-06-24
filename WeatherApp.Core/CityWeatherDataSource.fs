namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open System.Reactive.Disposables
open System.Reactive.Subjects

[<Sealed>]
type public CityWeatherDataSource() =
    member val private DisposeBag = new CompositeDisposable()
    
    member val Cities = new BehaviorSubject<City list>(List.empty)
    member val CityWeather = new BehaviorSubject<CityWeather list>(List.empty)
    
    member this.AddCitiesFromQuery(query: WeatherQuery) =
        CityWeatherProvider.getCityWeather query
        |> Observable.combineLatest this.CityWeather
        |> Observable.subscribeSafe (fun (oldWeathers, newWeathers) -> this.CityWeather.OnNext(oldWeathers |> List.append newWeathers))
        |> Disposable.disposeWith this.DisposeBag
    
    member __.RefreshAll() =
        ()
        