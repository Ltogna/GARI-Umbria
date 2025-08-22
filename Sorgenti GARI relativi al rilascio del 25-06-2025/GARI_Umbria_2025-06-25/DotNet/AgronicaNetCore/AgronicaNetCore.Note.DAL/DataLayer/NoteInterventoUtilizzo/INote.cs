using System.Data;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Note.DAL.DataLayer.NoteInterventoUtilizzo;

public interface INote
{
    public Task<DataTable> LeggiNoteInterventoUtilizzoAsync(int notaUtilizzoCod, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiProfilazioneNoteAsync(int notaUtilizzoCod, int notaGruppoCod, bool soloNoteConUtilizzo,
        Visibilita visibilita, string filtroAggiuntivo, AgronicaCoreParametri objParams);

    public Task<string> LeggiNotaDesFromNotaCodAsync(int notaCod, AgronicaCoreParametri objParams);
    public Task<string> LeggiNotaUtilizzoDesFromNotaUtilizzoCodAsync(int i, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiGruppoNoteAsync(int notaGruppoCod, string filtroAggiuntivo, string orderBy,
        AgronicaCoreParametri objParams);

    public Task<bool> SalvaGruppoNoteAsync(int notaGruppoCod, string notaGruppoDes, DateTime validitaInizio, DateTime validitaFine,
        AgronicaCoreParametri objParams);

    public Task<int> NoteInterventoGruppiNewIdAsync(AgronicaCoreParametri objParams);

    public Task<bool> AggiornaGruppoNoteAsync(int bodyNotaGruppoCod, string bodyNotaGruppoDes, DateTime finestraTemporaleInizio,
        DateTime finestraTemporaleFine, bool visibile, AgronicaCoreParametri objParams);

    public Task<bool> CancellaGruppoNoteAsync(int gruppoNoteCod, AgronicaCoreParametri objParams);

    public Task<bool> SalvaNotaAsync(int notaCod, string notaDes, int notaGruppoCod, DateTime validitaInizio,
        DateTime validitaFine, AgronicaCoreParametri objParams);

    public Task<int> NoteNewIdAsync(AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiNoteInterventoUtilizzoGruppiAsync(int notaGruppoCod, int notaUtilizzoCod,
        AgronicaCoreParametri objParams);

    public Task<bool> CancellaGruppoNoteUtilizzoAsync(int notaGruppoCod, int notaUtilizzoCod, AgronicaCoreParametri objParams);

    public Task<bool> ScriviNoteUtilizzoAsync(int notaGruppoCod, int utilizzoCod, DateTime dtInizio, DateTime dtFine,
        AgronicaCoreParametri objParams);

    public Task<bool> ModificaVisibilitaGruppoAsync(int notaGruppoCod, bool visible, AgronicaCoreParametri objParams);
    public Task<bool> ModificaVisibilitaNotaAsync(int notaCod, bool visible, AgronicaCoreParametri objParams);
    public Task<bool> CancellaNotaAsync(int notaCod, AgronicaCoreParametri objParams);
}