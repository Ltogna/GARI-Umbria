using System.Data;
using AgronicaNetCore.Base.Models;
using InData.DefaultPianiColturali;
using InData.SpecieVegetali;
using OutData.DefaultPianiColturali;

namespace AgronicaNetCore.DefaultPianiColturali.BIZ.Services;

public interface IDefaultPianiColturaliService
{
    public Task<DefaultDistintaProduzione_Out> LeggiDefaultDistintaDiProduzioneAsync(string piva, int vegCod, AgronicaCoreParametri objParams);
    public Task<DataTable> LeggiDefaultGeneraleSpecieAsync(string piva, AgronicaCoreParametri objParams);
    public Task<bool> ScriviSpecieVegetaleAsync(SpecieVegetali_In body, AgronicaCoreParametri objParams);
    public Task<bool> CancellaSpecieVegetaleAsync(string piva, int vegCod, AgronicaCoreParametri objParams);

    public Task<bool> ScriviDefaultGeneraliInizialiAsync(DefaultGeneraliColtura_In body, AgronicaCoreParametri objParams);

    public Task<bool> ScriviDefaultSpecieInizialiAsync(DefaultGeneraliColtura_In body, AgronicaCoreParametri objParams);
    public Task<DataTable> LeggiDefaultGeneraliAsync(string piva, AgronicaCoreParametri objParams);
    public Task<bool> ScriviDefaultGeneraliAsync(DefaultGenerali_In body, AgronicaCoreParametri objParams);
    public Task<bool> CancellaDefaultGeneraleAsync(string piva, int vegCod, int culCod, AgronicaCoreParametri objParams);
    public Task<bool> SalvaDefaultDistintaDiProduzioneAsync(DistintaProduzione_In body, AgronicaCoreParametri objParams);
}