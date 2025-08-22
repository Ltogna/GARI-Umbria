using System.Data;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.ProfilazioneImprese.DAL.DataLayer.ProfilazioneImprese;

public interface IProfilazioneImprese
{
    public Task<DataTable> LeggiProfilazioneImpreseAsync(string piva,
        int idProfiloDati,
        string codiceChiave,
        string idGruppo,
        string filtroAggiuntivo,
        int lavCod,
        int vegCod,
        bool leggiSoloNoteConLavorazioneNullSeLavCodZero,
        AgronicaCoreParametri objParametri
    );

    public Task<bool> ScriviAsync(string piva, int idProfiloDati, string codiceChiave, string quesito, string idGruppo,
        string valoreSalvato, DateTime validitaInizio, DateTime validitaFine, int lavCod, int vegCod,
        AgronicaCoreParametri objParams);

    public Task<bool> ModificaAsync(string quesito, string valoreSalvato, string piva, int idProfiloDati, string codiceChiave,
        string idGruppo, DateTime validitaInizio, DateTime validitaFine, int lavCod, int vegCod,
        AgronicaCoreParametri objParams);

    public Task<bool> CancellaAsync(string piva, string codiceChiave, string idGruppo, int lavCod, int vegCod,
        AgronicaCoreParametri objParams);

    public Task<bool> CancellaTutteLavorazioniAsync(string piva, string idGruppo, int vegCod, AgronicaCoreParametri objParams);
}