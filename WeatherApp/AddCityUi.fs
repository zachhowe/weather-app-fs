namespace WeatherApp

open Foundation
open UIKit
open System
open System.Reactive.Disposables
open WeatherApp.Core

module AddCityUi =
    [<Sealed>]
    [<Register("AddCityView")>]
    type AddCityView() as this =
        inherit UIView()
    
        let textField: UITextField = 
            let textField = new UITextField()
            textField.TranslatesAutoresizingMaskIntoConstraints <- false
            textField
        
        do
            this.BackgroundColor <- UIColor.White
            this.AddSubview textField

            NSLayoutConstraint.ActivateConstraints 
                [|
                    textField.TopAnchor.ConstraintEqualTo(this.TopAnchor, (nfloat (float 12)))
                    textField.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, (nfloat (float 12))) 
                    textField.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, (nfloat (float -12)))
                |]
    
        member __.SearchQueryTextField = textField
    
    [<Sealed>]
    [<Register("AddCityViewController")>]
    type AddCityViewController(ui: AddCityView, dataSource: CityWeatherDataSource) =
        inherit UIViewController(null, null)

        let ui = ui

        let dataSource = dataSource
        let disposeBag = new CompositeDisposable()
    
        override this.LoadView() =
            this.View <- ui
