module UITableViewExtensions

open UIKit
open Foundation

type UITableView with
    member x.RegisterCell<'a when 'a :> UITableViewCell>(cellType: System.Type) =
        x.RegisterClassForCellReuse(cellType, cellType.Name)
        
    member x.DequeueCell<'a when 'a :> UITableViewCell>(indexPath: NSIndexPath) : 'a =
        x.DequeueReusableCell(typedefof<'a>.Name, indexPath) :?> 'a
