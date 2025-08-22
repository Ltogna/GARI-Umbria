using System.Data;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using InData.Note;

namespace AgronicaNetCore.Note.BIZ.Services;

public interface INoteService
{
    public Task<DataTable> LeggiNoteInterventoUtilizzoAsync(int notaUtilizzoCod, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiProfilazioneNoteAsync(int notaUtilizzoCod, int notaGruppoCod, bool soloNoteConUtilizzo,
        string filtroAggiuntivo, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiProfilazioneNoteAsync(string piva, string codiceChiave, int lavCod, int vegCod,
        bool filtraVegCodAncheSeZero, bool leggiSoloNoteConLavorazioneNullSeLavCodZero,
        AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiNoteAsync(string piva, int inputLavCod, int inputVegCod, AgronicaCoreParametri objParams);
    public Task<DataTable> LeggiGruppoNoteAsync(AgronicaCoreParametri objParams);
    public Task<bool> SalvaGruppoNoteAsync(SalvaGruppoNote_In body, AgronicaCoreParametri objParams);
    public Task<bool> AggiornaGruppoNoteAsync(SalvaGruppoNote_In body, AgronicaCoreParametri objParams);
    public Task<bool> CancellaGruppoNoteAsync(int gruppoNoteCod, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiNoteXUtilizzoAsync(int notaUtilizzoCod, int notaGruppoCod, AgronicaCoreParametri objParams);

    public Task<bool> AggiungiNotaAsync(string notaDes, int notaGruppoCod, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiNoteInterventoUtilizzoGruppiAsync(int notaGruppoCod, int notaUtilizzoCod,
        AgronicaCoreParametri objParams);

    public Task<bool> SalvaGruppoNoteCompletoAsync(SalvaGruppoNoteCompleto_In body, AgronicaCoreParametri objParams);
    public Task<bool> CancellaNotaAsync(int notaCod, AgronicaCoreParametri objParams);
}