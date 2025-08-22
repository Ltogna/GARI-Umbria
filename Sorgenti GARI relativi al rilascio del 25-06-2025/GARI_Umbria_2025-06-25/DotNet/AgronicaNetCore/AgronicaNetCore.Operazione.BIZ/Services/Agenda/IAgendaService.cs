using InData.Agenda;
using AgronicaNetCore.Base.Models;
using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreDTOStd.InData.Agenda.OperazioneAgenda_Temp;

namespace AgronicaNetCore.Operazione.BIZ.Services.Agenda
{
    public interface IAgendaService
    {
        public Task<int> ScriviModificaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP);
        public Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP);
        public Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP);
        public Task<bool> EliminaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        //public Task<bool> EliminaAssociateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP);
        Task ScriviListaAttivitaAgendaAsync(List<(Attivita attivita, OperazioneAgenda agenda)> agendaActivityList, AgronicaCoreParametri objServer);
        Task<int> ScriviAttivitaAgendaAsync((Attivita attivita, OperazioneAgenda agenda) agendaActivityList, AgronicaCoreParametri objServer);
    }
}
