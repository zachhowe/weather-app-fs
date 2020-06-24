namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open System.Reactive.Disposables
open System.Reactive.Subjects

[<Sealed>]
type public CityWeatherDataSource(provider: CityWeatherProvider) =
    let provider = provider

    member val private DisposeBag = new CompositeDisposable()

    member val CityWeather = new BehaviorSubject<CityWeather list>(List.empty)
    
    member this.AddCitiesFromQuery(query: WeatherQuery) =
        provider.GetCityWeather query
        |> Observable.combineLatest (this.CityWeather |> Observable.take 1)
        |> Observable.subscribeSafe (fun (oldWeathers, newWeathers) -> this.CityWeather.OnNext(oldWeathers |> List.append newWeathers))
        |> Disposable.disposeWith this.DisposeBag
    
    member __.RefreshAll() =
        ()
        