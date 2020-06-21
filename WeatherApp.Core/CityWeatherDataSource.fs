module WeatherApp.Core.CityWeatherDataSource

open FSharp.Control
open FSharp.Control.Reactive
open System.Reactive.Disposables
open System.Reactive.Subjects

type CityWeatherDataSource() =
    
    member val private DisposeBag = new CompositeDisposable()
    
    member val private CityWeather = new BehaviorSubject<CityWeather list>(List.empty)
    
    member val private Cities = new BehaviorSubject<City list>(List.empty)
    
    member this.AddCitiesFromQuery(query: WeatherQuery) =
        let currentCityWeathers = this.CityWeather.Value
        
        CityWeatherProvider.getCityWeather query
        |> Observable.subscribeSafe (fun x -> this.CityWeather.OnNext(currentCityWeathers |> List.append x))
        |> Disposable.disposeWith this.DisposeBag
        
    
    member __.RefreshAllCities() = ()
    
    
    member __.RefreshCity(city: City) = ()
        