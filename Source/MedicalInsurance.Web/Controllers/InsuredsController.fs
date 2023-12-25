module MedicalInsurance.Controllers.InsuredsController

open System

open Giraffe
open Giraffe.ViewEngine
open Microsoft.AspNetCore.Http

open MedicalInsurance.Views
open MedicalInsurance.DataAccessLayer
open Microsoft.Extensions.Logging
open System.Text
open System.Buffers
open System.Web

let private readBody (context: HttpContext)  = 
    let body = context.ReadBodyFromRequestAsync().GetAwaiter().GetResult();
    HttpUtility.UrlDecode(body)

let private middleNameOrNone (arr:string array) = 
    if arr.Length > 1 
        then Some arr[1]
        else None

//LastName=Андреев&FirstName=Даниил&MiddleName=&PassportSeries=1111&PassportNumber=122221&PhoneNumber=89003343148&BirthDay=2002-02-18&Residency=Цивильск
let private toIsuredPerson personId (value: string) = 
    let arr = value.Split("&")

    {   Id = personId
        LastName = arr[0].Split("=")[1]
        FirstName = arr[1].Split("=")[1]
        MiddleName = middleNameOrNone (arr[2].Split("="))
        Passport = 
            {   Series = int (arr[3].Split("=")[1])
                Number = int (arr[4].Split("=")[1]) }
        PhoneNumber = arr[5].Split("=")[1]
        BirthDay = DateTime.Parse(arr[6].Split("=")[1])
        Residency = arr[7].Split("=")[1] }


let getAll: HttpHandler = 
    fun (next : HttpFunc) (context: HttpContext) -> 
        context.GetService<AccessDbContext>() 
        |> InsuredPersonsCollection.getAll
        |> InsuredsView.render
        |> context.WriteHtmlViewAsync

let getPerson (personId:int) : HttpHandler =  
    fun (next : HttpFunc) (context: HttpContext) -> 
        personId
        |> InsuredPersonsCollection.getById (context.GetService<AccessDbContext>())
        |> UpdatePersonView.render
        |> context.WriteHtmlViewAsync

let createPersonView : HttpHandler= 
    fun (next : HttpFunc) (context: HttpContext) -> 
        CreatePersonView.render ()
        |> context.WriteHtmlViewAsync


let createPerson : HttpHandler= 
    fun (next : HttpFunc) (context: HttpContext) -> 
        let logger = context.GetLogger()
        logger.LogInformation("Добавление персоны")

        let strBody = readBody context
        toIsuredPerson -1 strBody
        |> InsuredPersonsCollection.insert (context.GetService<AccessDbContext>())

        redirectTo true "/insureds" next context

let updatePerson (personId :int): HttpHandler=
    fun (next : HttpFunc) (context: HttpContext) -> 
        let logger = context.GetLogger()
        logger.LogInformation("Обновление персоны")

        let strBody = readBody context
        
        toIsuredPerson personId strBody
        |> InsuredPersonsCollection.update (context.GetService<AccessDbContext>())

        redirectTo true "/insureds" next context

let deletePerson (personId :int): HttpHandler= 
    fun (next : HttpFunc) (context: HttpContext) ->
        personId
        |> InsuredPersonsCollection.delete (context.GetService<AccessDbContext>())
        redirectTo true "/insureds" next context