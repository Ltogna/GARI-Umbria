using InData.Agenda;
using AgronicaNetCore.Base.Models;
using InData.Anagrafica;

namespace AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni
{
    public interface IMovDestinazioniService
    {
        public Task<int> ScriviModificaAsync(WriteMovDestinazioni dtoMovDest, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteMovDestinazioni dtoMovDest, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(List<WriteMovDestinazioni> MovDestinazioni, AgronicaCoreParametri objP);
        Task ScriviMovimentoDestinazioneAsync(Movimento_Destinazione movDest, AgronicaCoreParametri objServer);
    }
}
