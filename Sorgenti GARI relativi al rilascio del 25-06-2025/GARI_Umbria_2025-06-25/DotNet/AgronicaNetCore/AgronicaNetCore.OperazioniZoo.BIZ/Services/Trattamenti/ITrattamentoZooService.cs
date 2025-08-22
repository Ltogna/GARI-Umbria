using OutData.Zoo;
using AgronicaNetCore.Base.Models;
using AgronicaCoreModelsSTD.attivita;
using InData.Zoo;

namespace AgronicaNetCore.OperazioniZoo.BIZ.Services.Trattamenti
{
    public interface ITrattamentoZooService
    {
        public interface ITrattamentoResult { }

        Task<SomministrazioneProdotti> LeggiSomministrazioneProdottiAsync(string sommNumero, AgronicaCoreParametri objParams);

        /// <summary>
        /// Dato il Protocollo da cui partire, ribalta il Protocollo in una x nuove Prescrizioni (Indicazioni Terapeutiche), in base al campo Numero_Somm delle Righe del Protocollo, 
        /// contenenti i dettagli mancanti per il Trattamento, con le rispettive Righe di Prescrizione.
        /// </summary>
        /// <param name="Id_Protocollo">Protocollo da cui si sta creando il trattamento (corrisponde a Ricette_Zoo.IdRicetta)</param>
        /// <param name="somministrazioni">Prime somministrazioni da effettuare (contiene anche i dati delle somministrazioni seguenti in base all'intervallo impostato su Ricette_Zoo_Agenda)</param>
        /// <param name="objP"></param>
        /// <param name="objP_Utenti"></param>
        /// <returns></returns>
        Task<ITrattamentoResult> CreaTrattamentoDaProtocollo(int Id_Protocollo, List<Attivita> somministrazioni, AgronicaCoreParametri objP, AgronicaCoreParametri objP_Utenti);

        /// <summary>
        /// Crea o modifica una Somministrazione dall'oggetto Attivita passato
        /// </summary>
        /// <param name="somministrazione"></param>
        /// <param name="objP_Server"></param>
        /// <param name="objP_Utenti"></param>
        /// <returns></returns>
        Task<ITrattamentoResult> ScriviModificaSomministrazione(Attivita somministrazione, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti);

        /// <summary>
        /// Elimina una Somministrazione (futura o confermata)
        /// </summary>
        /// <param name="dtoDelete"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        Task<bool> EliminaSomministrazione(DeleteSomministrazione dtoDelete, AgronicaCoreParametri objP);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Piva"></param>
        /// <param name="Id_Agenda"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        Task<Attivita?> GetAttivitaFromAgenda(string Piva, int Id_Agenda, AgronicaCoreParametri objP);
    }
}
