namespace MedicalInsurance.DataAccessLayer 

open System.Data.OleDb

type AccessDbContext = { DbConnection : OleDbConnection}

module AccessDbContext = 
    let request context query = 
        let command = new OleDbCommand (query, context.DbConnection)
        command.ExecuteReader ()

    let createDbCntext connextionString = 
        { DbConnection = new OleDbConnection(connextionString) }

    let openConnetion context = 
        context.DbConnection.Open ()
    
    let closeConnetion context = 
        context.DbConnection.Close ()
