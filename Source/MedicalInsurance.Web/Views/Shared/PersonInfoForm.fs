module MedicalInsurance.Views.Shared.PersonInfoForm

open Giraffe.ViewEngine
open MedicalInsurance.DataAccessLayer

let toStringOrEmpty (str : string Option) = 
    match str with 
    | Some someStr -> someStr
    | None -> ""

let label text = 
    label [_class "person-form__label"] [encodedText text] 

let input attrs = 
    (_class "person-form__input")::attrs
    |> input 

let render (personInfo: InsuredPerson) (method) (action): XmlNode = 
    form [_class "person-form"; _method method; _action action; _acceptCharset "UTF-8"] [
        "Фамилия" |> label
        input [_value personInfo.LastName; _required; _name "LastName"; _id "LastName"]
        "Имя" |> label
        input [_value personInfo.FirstName; _required; _name "FirstName"; _id "FirstName"]
        "Отчество" |> label
        input [_value (toStringOrEmpty personInfo.MiddleName); _name "MiddleName"; _id "MiddleName"]
        "Серия" |> label
        input [_type "number"; _value (personInfo.Passport.Series.ToString()); _name "PassportSeries"; _id "PassportSeries"; _required]
        "Номер паспорта" |> label
        input [_type "number"; _value (personInfo.Passport.Number.ToString()); _name "PassportNumber"; _id "PassportNumber";_required]
        "Номер телефона" |> label
        input [_type "tel"; _value personInfo.PhoneNumber; _name "PhoneNumber"; _id "PhoneNumber"]
        "Дата рождения" |> label
        input [_type "date"; _value (personInfo.BirthDay.ToString("yyyy-MM-dd")); _required; _name "BirthDay"; _id "BirthDay"]
        "Место жительства" |> label
        input [_type "text"; _value personInfo.Residency; _required; _name "Residency"; _id "Residency"]

        button [_class "blue-button"; _type "submit"] [encodedText "Сохранить"]
    ]
    
