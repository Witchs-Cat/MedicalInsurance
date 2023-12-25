namespace MedicalInsurance.DataAccessLayer

open System.Globalization

module InsuredPersonsCollection = 
    let getAll context = 
        //AccessDbContext.openConnetion context
        let query = "SELECT * FROM ЗастрахованныеЛица"
        let request = AccessDbContext.request context query 

        let rec iter () =  
            match InsuredPerson.read request with
            | Some(value) -> value :: (iter ())
            | None -> []  

        iter ()
        // AccessDbContext.closeConnetion context

    let getById context (id:int) = 
        $"""SELECT * 
            FROM ЗастрахованныеЛица
            WHERE ЗастрахованныеЛица.НомерЗастрахованногоЛица = {id}"""
        |> AccessDbContext.request context
        |> InsuredPerson.read

    let delete context  (id:int) = 
        $"""DELETE * 
            FROM ЗастрахованныеЛица
            WHERE ЗастрахованныеЛица.НомерЗастрахованногоЛица = {id}"""
        |> AccessDbContext.request context
        |> ignore

    let private toStrOrNull str = 
        match str with 
        | None -> "\"\""
        | Some str -> $"\"{str}\""

    let update context  (person:InsuredPerson) = 
        $"""UPDATE ЗастрахованныеЛица 
            SET ЗастрахованныеЛица.Фамилия="{person.LastName}",
            ЗастрахованныеЛица.Имя="{person.FirstName}",
            ЗастрахованныеЛица.Отчество={toStrOrNull person.MiddleName},
            ЗастрахованныеЛица.СерияПаспорта={person.Passport.Series},
            ЗастрахованныеЛица.НомерПаспорта={person.Passport.Number},
            ЗастрахованныеЛица.НомерТелефона="{person.PhoneNumber}",
            ЗастрахованныеЛица.ДатаРождения=#{person.BirthDay.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}#,
            ЗастрахованныеЛица.МестоЖительства="{person.Residency}"
            WHERE (((ЗастрахованныеЛица.НомерЗастрахованногоЛица)={person.Id}));
        """
        |> AccessDbContext.request context
        |> ignore

    let insert context  (person:InsuredPerson) = 
        $"""INSERT INTO ЗастрахованныеЛица (Фамилия, Имя, Отчество, СерияПаспорта, НомерПаспорта, НомерТелефона, ДатаРождения, МестоЖительства) 
             VALUES ("{person.LastName}", "{person.FirstName}", {toStrOrNull person.MiddleName},
                {person.Passport.Series}, {person.Passport.Number}, "{person.PhoneNumber}",
                #{person.BirthDay.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}#, "{person.Residency}")
        """
        |>AccessDbContext.request context
        |> ignore