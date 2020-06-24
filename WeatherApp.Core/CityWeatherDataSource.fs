namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open System.Reactive.Disposables
open System.Reactive.Subjects

[<Sealed>]
type public CityWeatherDataSource(provider: CityWeatherProvider) =
    let provider = provider

    let disposeBag = new CompositeDisposable()
    
    // TODO: ensure duplicate cities cannot be added
    let cityWeather = new BehaviorSubject<CityWeather list>(List.empty)

    let addCitiesFromQuery query =
        provider.GetCityWeather query
        |> Observable.combineLatest (cityWeather |> Observable.take 1)
        |> Observable.subscribeSafe (fun (oldWeathers, newWeathers) -> cityWeather.OnNext(oldWeathers |> List.append newWeathers))
        |> Disposable.disposeWith disposeBag
    
    // TODO: implement refresh
    let refreshAll = ()

    member val CityWeather = cityWeather

    member __.AddCitiesFromQuery = addCitiesFromQuery
    
    member __.RefreshAll() = refreshAll
        