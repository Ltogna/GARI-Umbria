using System.Data;
using InData.Agenda;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.RicettexAgenda
{
    public interface IRicettexAgenda
    {
        public Task<DataTable> ReadAsync(int Ricetta_Cod, int Id_Agenda, AgronicaCoreParametri objP, int RicettaOp_Cod = 0);
        public Task<bool> ExistAsync(int Ricetta_Cod, int Id_Agenda, AgronicaCoreParametri objP, int RicettaOp_Cod = 0);

        public Task<bool> CreateAsync(WriteRicettexAgenda dtoRxA, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteRicettexAgenda dtoRxA, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(int Ricetta_Cod, int Id_Agenda, AgronicaCoreParametri objP, int RicettaOp_Cod = 0);

        public Task<bool> ScriviModificaAsync(WriteRicettexAgenda dtoRicxAg, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteRicettexAgenda dtoRicxAg, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteRicettexAgenda> RicettexAgenda, AgronicaCoreParametri objP);
    }
}
