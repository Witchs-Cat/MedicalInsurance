module MedicalInsurance.Views.Shared.Head

open Giraffe.ViewEngine

let defaultHead () =
    head [] [
        title [] [encodedText "MedicalInsurance"]
        meta [_charset "UTF-8"]
        meta [_name "viewport"; _content "width=device-width"]
        link [_rel "stylesheet"; _href "/css/style.css"]
        link [_rel "stylesheet"; _href "/css/reset.css"]
    ]