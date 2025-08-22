using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Apezzamenti
{
    public interface IAppezzamenti
    {
        Task<bool> BloccaSbloccaAsync(bool block, Appezzamento.PK appezzamento, AgronicaCoreParametri objParametri);
        Task<bool> IsBlockedAsync(Appezzamento.PK appezzamento, DateTime atDate, AgronicaCoreParametri objParametri);
    }
}