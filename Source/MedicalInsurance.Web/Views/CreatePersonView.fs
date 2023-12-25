module MedicalInsurance.Views.CreatePersonView

open System
open Giraffe.ViewEngine

open MedicalInsurance.Views.Shared
open MedicalInsurance.DataAccessLayer


let render (): XmlNode =
    let emptyValue =   
        {   Id = -1
            LastName = "" 
            FirstName = "" 
            MiddleName = None
            Passport = 
                {   Series = 0
                    Number = 0}
            BirthDay = DateTime.Now
            PhoneNumber = ""
            Residency = ""}

    [
        Head.defaultHead()
        body [] [   
            PersonInfoForm.render emptyValue "post" $"/insureds"
            |> Layout.maxWidthWrapper
            |> Layout.centeredWrapper
         ]
     ]
     |> html []