namespace MedicalInsurance.DataAccessLayer

module InsuranceCasesCollection = 
   let getAll context = 
        let query = "SELECT * FROM СтраховыеСлучаи"

        AccessDbContext.openConnetion context

        let request = AccessDbContext.request context query 

        let rec iter () =  
            match InsuranceCase.read request with
            | Some(value) -> value :: (iter ())
            | None -> []  

        let result = iter ()

        AccessDbContext.closeConnetion context
        result


