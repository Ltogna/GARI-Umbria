using System.Data;
using AgronicaNetCore.Base.Models;
using InData.Note;
using InData.ProfilazioneImprese;

namespace AgronicaNetCore.ProfilazioneImprese.BIZ.Services;

public interface IProfilazioneImpreseService
{
    public Task<DataTable> LeggiAsync(string piva, int idProfiloDati, string codiceChiave, string idGruppo, int lavCod,
        int vegCod, bool leggiSoloNoteConLavorazioneNullSeLavCodZero, AgronicaCoreParametri objParams);

    public Task<bool> SalvaProfilazioneAsync(SalvaProfilazione_In body, AgronicaCoreParametri objParams);
    
    public Task<bool> SalvaNoteAsync(SalvaNote_In body, AgronicaCoreParametri objParams);

    public Task<bool> ScriviInserisceOAggiornaAsync(string piva, string codiceChiave, string idGruppo, string quesito,
        string valoreSalvato, DateTime validitaInizio, DateTime validitaFine, int lavCod, int vegCod,
        AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiProfilazioneMacchineXContattiAsync(string piva, int vegCod, AgronicaCoreParametri objParams);

    public Task<bool> CancellaAsync(string piva, int notaUtilizzoCod, string idGruppo, int lavCod, int vegCod,
        AgronicaCoreParametri objParams);

    public Task<bool> PropagaSuTutteLeOperazioniAsync(string piva, int lavCod, int vegCod, AgronicaCoreParametri objParams);
    public Task<bool> AggiornaOreMinutiAsync(AggiornaOreMinuti_In body, AgronicaCoreParametri objParams);
}