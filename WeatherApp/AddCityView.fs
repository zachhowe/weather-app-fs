namespace WeatherApp

open Foundation
open UIKit
open System

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
    