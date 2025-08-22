using InData.Agenda;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Agenda
{
    public interface IAgenda
    {
        public Task<DataTable> ReadAsync(string Piva, int Id_Agenda, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null);
        public Task<bool> ExistAsync(int Id_Agenda, AgronicaCoreParametri objP);

        public Task<bool> CreateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(string Piva, int Id_Agenda, int Sa_Cod, AgronicaCoreParametri objP);

        public Task<int> ScriviModificaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        public Task<bool> EliminaAssociateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
    }
}
