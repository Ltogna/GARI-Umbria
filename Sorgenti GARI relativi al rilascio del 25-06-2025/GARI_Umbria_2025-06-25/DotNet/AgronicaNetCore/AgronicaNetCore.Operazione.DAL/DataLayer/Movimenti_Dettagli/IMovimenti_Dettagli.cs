using System.Data;
using InData.Agenda;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli
{
    public interface IMovimenti_Dettagli
    {
        public Task<DataTable> ReadAsync(string Piva, int Id_Agenda, int Id_Mov, int Id_Mov_Det, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null);
        public Task<bool> ExistAsync(int Id_Mov_Det, AgronicaCoreParametri objP);

        public Task<bool> CreateAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(string Piva, int Id_Agenda, int Id_Mov, int Id_Mov_Det, AgronicaCoreParametri objP);

        public Task<int> ScriviModificaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovDettagli> MovDettagli, AgronicaCoreParametri objP);
    }
}
