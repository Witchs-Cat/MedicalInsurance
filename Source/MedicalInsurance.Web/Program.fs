module MedicalInsurance.Program

open System
open System.Threading.Tasks
open System.IO

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection

open Giraffe

open MedicalInsurance.Routes
open MedicalInsurance.DataAccessLayer

let webApp = choose[
    InsuredsRoute.routes
    InsuredsRoute.cleanRoutes
]

let connectionString = ("Provider=Microsoft.ACE.OLEDB.16.0;" + 
    "Data Source=C:\Junk\Solutions\MedicalInsurance\Source\MedicalInsurance.DataAccessLayer\MMI.Db.accdb;" +
    "Persist Security Info=False;")

// ---------------------------------
// Error handler
// ---------------------------------
let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------
let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
            .UseCors(configureCors)
            .UseStaticFiles()
            .UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    let dbContext = AccessDbContext.createDbCntext connectionString
    AccessDbContext.openConnetion dbContext
    services
        .AddTransient<AccessDbContext>(Func<IServiceProvider, AccessDbContext> (fun _ -> dbContext))
        .AddCors()
        .AddGiraffe() 
        |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder
        .AddConsole()
        .AddDebug() 
        |> ignore


[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")

    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(webRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0