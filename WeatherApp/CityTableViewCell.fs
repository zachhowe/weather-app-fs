namespace WeatherApp

open Foundation
open UIKit

[<Register("CityTableViewCell")>]
type CityTableViewCell(style, reuseId : string) as this =
    inherit UITableViewCell(style, reuseId)

    let tableView: UITableView = 
        let tableView = new UITableView()
        tableView.TranslatesAutoresizingMaskIntoConstraints <- false
        tableView
        
    do
        this.BackgroundColor <- UIColor.White
        this.AddSubview tableView

        NSLayoutConstraint.ActivateConstraints 
            [|
                tableView.TopAnchor.ConstraintEqualTo(this.SafeAreaLayoutGuide.TopAnchor)
                tableView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor) 
                tableView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor)
                tableView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor)
            |]
