﻿namespace WeatherApp.Core

module List =
    let ofOne (value : 'a) =
        [ value ]

module Array =
    let ofOne (value : 'a) =
        [| value |]