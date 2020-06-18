namespace WeatherApp

open Foundation
open UIKit

[<Register("CityListView")>]
type CityListView() as this =
    inherit UIView()

    let tableView: UITableView = 
        let tableView = new UITableView()
        tableView.TranslatesAutoresizingMaskIntoConstraints <- false
        tableView
        
    do
        this.BackgroundColor <- UIColor.White
        this.AddSubview tableView

        NSLayoutConstraint.ActivateConstraints 
            [|
                tableView.TopAnchor.ConstraintEqualTo(this.TopAnchor)
                tableView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor) 
                tableView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor)
                tableView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor)
            |]
    
    member x.TableView = tableView