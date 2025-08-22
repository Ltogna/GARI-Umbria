using System.Data;
using InData.Agenda;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Ricette
{
    public interface IRicette
    {
        Task<DataTable> ReadAsync(string Piva, int Sa_Cod, int Ricetta_Cod, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null);
        public Task<bool> ExistAsync(int Ricetta_Cod, AgronicaCoreParametri objP);

        public Task<bool> CreateAsync(WriteRicette dtoRicetta, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteRicette dtoRicetta, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(int Ricetta_Cod, AgronicaCoreParametri objP);

        public Task<int> ScriviModificaAsync(WriteRicette dtoRicetta, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, int Ricetta_Cod, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Ricette, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, int Ricetta_Cod, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Ricette, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteRicette dtoRicetta, AgronicaCoreParametri objP);
        public Task<bool> EliminaAssociateAsync(WriteRicette dtoRicetta, AgronicaCoreParametri objP);
    }
}
