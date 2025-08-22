using InData.Agenda;
using AgronicaNetCore.Base.Models;
using InData.Anagrafica;

namespace AgronicaNetCore.Operazione.BIZ.Services.Movimenti
{
    public interface IMovimentiService
    {
        public Task<int> ScriviModificaAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovimenti dtoMovimenti, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovimenti> Movimenti, AgronicaCoreParametri objP);
        Task ScriviMovimentoAsync(Movimento movimento, AgronicaCoreParametri objServer);
    }
}
