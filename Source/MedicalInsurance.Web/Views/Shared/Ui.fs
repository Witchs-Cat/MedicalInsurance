module MedicalInsurance.Views.Shared.Ui

open Giraffe.ViewEngine

let blueButtonLink link text = 
    a [_class "blue-button"; _href link] [encodedText text]

