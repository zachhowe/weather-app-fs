namespace WeatherApp

open System
open Foundation
open UIKit

type CityTableViewCellModel = {
    Name: string
    Status: string
    Temperature: string
}

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
