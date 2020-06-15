namespace WeatherApp.Core

module HttpQueryBuilder =
    type QueryItem = string * string

    let formatQueryString (items: QueryItem list) =
        items
        |> List.map (fun (one, two) -> one + "=" + two)
        |> String.join "&"
