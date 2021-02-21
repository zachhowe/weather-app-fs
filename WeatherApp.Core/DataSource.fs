namespace WeatherApp.Core

open FSharp.Control
open FSharp.Control.Reactive
open System
open System.Reactive.Disposables
open System.Reactive.Subjects

module DataSource =
    open Provider
    
    type public CityWeatherDataSource = {
        CityWeather: IObservable<CityWeather list>
        AddCitiesFromQuery: WeatherQuery -> unit
    }
        
    let create provider =
        let cityWeather = new BehaviorSubject<CityWeather list>(List.empty)
        let disposeBag = new CompositeDisposable()
        
        let addCitiesFromQuery query =
            provider.GetCityWeather query
            |> Observable.combineLatest (cityWeather |> Observable.take 1)
            |> Observable.subscribeSafe (fun (oldWeathers, newWeathers) -> cityWeather.OnNext(oldWeathers |> List.append newWeathers))
            |> Disposable.disposeWith disposeBag
        
        {
            CityWeather = cityWeather
            AddCitiesFromQuery = addCitiesFromQuery
        }
