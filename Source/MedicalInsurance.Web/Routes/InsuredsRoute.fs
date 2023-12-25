module MedicalInsurance.Routes.InsuredsRoute

open Giraffe;
open MedicalInsurance.Controllers.InsuredsController

let users = "/insureds"

let cleanRoutes: HttpHandler = 
    (choose [
        // views
        GET >=> routex "(/?)" >=> getAll
        GET >=> routef "/%i" getPerson
        GET >=> route "/create" >=> createPersonView
        //api
        POST >=> routef "/%i" updatePerson
        POST >=> routex "(/?)" >=> createPerson
        GET >=> routef "/delete/%i" deletePerson 
        ])

let routes: HttpHandler = 
    subRoute users cleanRoutes
    
