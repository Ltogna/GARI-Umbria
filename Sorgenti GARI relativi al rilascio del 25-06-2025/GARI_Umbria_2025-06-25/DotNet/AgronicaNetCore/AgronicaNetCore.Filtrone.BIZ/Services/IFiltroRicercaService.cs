using AgronicaCoreDTOStd.InData.FiltroRicerca;
using AgronicaNetCore.Base.Models;
using AgronicaCoreDTOStd.OutData.FiltroRicerca;

namespace AgronicaNetCore.FiltroRicerca.BIZ.Services
{
    public interface IFiltroRicercaService
    {
        Task<CriteriRicerca_OUT> GetResultAsync(CriteriRicerca_IN criteriRicerca_IN, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti, AgronicaCoreParametri objP_Super_Server);
    }
}