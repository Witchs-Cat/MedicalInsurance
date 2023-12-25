namespace MedicalInsurance.DataAccessLayer

open System
open System.Data.OleDb

type Passport = {
    Series: int
    Number: int}

type InsuredPerson = {
    Id: int
    LastName: string
    FirstName: string 
    MiddleName: string Option
    Passport: Passport
    BirthDay: DateTime
    PhoneNumber: string
    Residency: string}

module InsuredPerson =
    let read (reader : OleDbDataReader) = 
        let toStrOrNone (convertedValue: obj) = 
            match convertedValue with
                | :? string as str -> Some str  
                | _ -> None

        if reader.Read()
            then Some { 
                Id = reader["НомерЗастрахованногоЛица"]:?> int32
                LastName = reader["Фамилия"]:?>string
                FirstName = reader["Имя"]:?>string
                MiddleName =  toStrOrNone reader["Отчество"]
                Passport = { 
                    Series = reader["СерияПаспорта"]:?> int32
                    Number = reader["НомерПаспорта"]:?> int32}
                PhoneNumber = reader["НомерТелефона"]:?>string
                BirthDay = reader["ДатаРождения"]:?>DateTime
                Residency = reader["МестоЖительства"]:?>string
            }
            else None


