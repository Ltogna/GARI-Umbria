using System.Data;
using InData.Agenda;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni
{
    public interface IMov_Destinazioni
    {
        public Task<DataTable> ReadAsync(string Piva, int Id_Agenda, int Id_Mov, int Id_Mov_Det, int Id_Dest, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null);
        public Task<bool> ExistAsync(string Piva, int Id_Agenda, int Id_Mov, int Id_Mov_Det, int Id_Destinazione, AgronicaCoreParametri objP);
        
        public Task<bool> CreateAsync(WriteMovDestinazioni dtoMovDestinazioni, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteMovDestinazioni dtoMovDestinazioni, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(string Piva, int Id_Agenda, int Id_Mov, int Id_Mov_Det, int Id_Dest, AgronicaCoreParametri objP);

        public Task<int> ScriviModificaAsync(WriteMovDestinazioni dtoMovDest, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovDestinazioni dtoMovDest, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovDestinazioni> MovDestinazioni, AgronicaCoreParametri objP);
    }
}
