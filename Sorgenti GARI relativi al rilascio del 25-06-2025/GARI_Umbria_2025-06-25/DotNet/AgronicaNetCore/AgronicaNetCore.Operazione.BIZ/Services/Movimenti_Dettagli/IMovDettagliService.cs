using InData.Agenda;
using AgronicaNetCore.Base.Models;
using InData.Anagrafica;

namespace AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli
{
    public interface IMovDettagliService
    {
        public Task<int> ScriviModificaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovDettagli> MovDettagli, AgronicaCoreParametri objP);
        Task ScriviMovimentoDettaglioAsync(Movimento_Dettaglio movDett, AgronicaCoreParametri objServer, bool documentoPrevisionale = false, bool usaDataModifica = false);
    }
}
