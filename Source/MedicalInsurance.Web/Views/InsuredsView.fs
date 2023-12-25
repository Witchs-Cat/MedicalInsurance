module MedicalInsurance.Views.InsuredsView
open Giraffe
open Giraffe.ViewEngine

open MedicalInsurance.DataAccessLayer
open MedicalInsurance.Views.Shared

let toStringOrEmpty (str : string Option) = 
    match str with 
    | Some someStr -> someStr
    | None -> ""

let personsHeader text = 
     th [_class "persons-tabel__header"] [encodedText text]

let personData (personId: int) text = 
    td [_class "persons-tabel__data"] [
            a [_class "persons-tabel__data__link"; _href $"/insureds/{personId}"] 
                [encodedText text]
        ]

let personToView (insured: InsuredPerson) = 
    tr [_class "persons-tabel__row"] [
        insured.LastName |> personData insured.Id
        insured.FirstName |> personData insured.Id 
        insured.MiddleName |> toStringOrEmpty  |> personData insured.Id
        insured.Passport.Series.ToString() |> personData insured.Id
        insured.Passport.Number.ToString() |> personData insured.Id
        insured.PhoneNumber |> personData insured.Id
        insured.BirthDay.ToString("d") |> personData insured.Id
        insured.Residency |> personData insured.Id
        td [_class "persons-tabel__data"]
            [a [_class "red-rounded-button"; _href $"/insureds/delete/{insured.Id}"] [encodedText "удалить"]]
    ]

let personsTabel insureds =
    tr [_class "persons-tabel__row"] [
         "Фамилия" |> personsHeader
         "Имя" |> personsHeader
         "Отчество" |> personsHeader
         "Серия" |> personsHeader
         "Номер паспорта" |> personsHeader
         "Номер телефона" |> personsHeader
         "Дата рождения" |> personsHeader
         "Место жительства" |> personsHeader
         "Операция" |> personsHeader
    ]
    :: [for insured in insureds -> 
        personToView insured]
    |> table [_class "persons-tabel"] 


         

let render (insureds: InsuredPerson list) : XmlNode =
    [
        Head.defaultHead()
        body [_class "background-image"] [
            [
                insureds
                |> personsTabel
                Ui.blueButtonLink "/insureds/create" "Добавить"
            ]
            |> Layout.whiteContainer
            |> Layout.maxWidthWrapper
            |> Layout.centeredWrapper


         ]
     ]
     |> html []
    