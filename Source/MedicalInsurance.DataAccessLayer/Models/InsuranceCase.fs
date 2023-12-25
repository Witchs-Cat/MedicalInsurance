namespace MedicalInsurance.DataAccessLayer

open System
open System.Data.OleDb

type InsuranceCase = {
    Id: int
    PaymentAmount: decimal
    PaymentDate: DateTime
    ContractId: int
    IncidentDate: DateTime
    NatureId: int
}

module InsuranceCase = 
    let read (reader : OleDbDataReader) = 
        if reader.Read()
            then Some { 
                Id = reader["НомерСтраховогоСлучая"] :?> int
                PaymentAmount = reader["РазмерВыплаты"] :?> decimal
                ContractId = reader["НомерСтраховогоДоговра"] :?> int
                PaymentDate = reader["ДатаВыплаты"] :?> DateTime
                IncidentDate = reader["ДатаПроисшествия"] :?> DateTime
                NatureId = reader["ХарактерПроисшествия"] :?> int}
            else None
