using System.Data;
using InData.Agenda;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti
{
    public interface IMovimenti
    {
        public Task<DataTable> ReadAsync(string Piva, int Id_Agenda, int Id_Mov, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null);
        public Task<bool> ExistAsync(int Id_Mov, AgronicaCoreParametri objP);

        public Task<bool> CreateAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(string Piva, int Id_Agenda, int Id_Mov, AgronicaCoreParametri objP);

        public Task<int> ScriviModificaAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovimenti> Movimenti, AgronicaCoreParametri objP);
    }
}
