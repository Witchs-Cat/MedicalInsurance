module MedicalInsurance.Views.UpdatePersonView

open Giraffe.ViewEngine

open MedicalInsurance.Views.Shared
open MedicalInsurance.DataAccessLayer


let personData (person: InsuredPerson option)  = 
    match person with 
    | Some (value) -> PersonInfoForm.render value "post" $"/insureds/{value.Id}"
    | None -> encodedText "person not found"


let render (person: InsuredPerson option): XmlNode =
    [
        Head.defaultHead()
        body [] [   
            person
            |> personData
            |> Layout.maxWidthWrapper
            |> Layout.centeredWrapper
         ]
     ]
     |> html []