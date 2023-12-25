module MedicalInsurance.Views.Shared.Layout

open Giraffe.ViewEngine

let whiteContainer nodes = 
    div [_class "white-container"] nodes

let centeredWrapper node = 
    div [_class "centered-wrapper"] [node]

let maxWidthWrapper node =
    div [_class "max-width-wrapper"] [node]