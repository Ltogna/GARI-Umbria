using AgronicaNetCore.Base.Models;
using InData.Log.AgronicaLogInvio;

namespace AgronicaNetCore.Logging.DAL.DataLayer.AgronicaLogInvio.AgronicaLogInvioAgenda
{
    public interface IAgronicaLogInvioAgenda
    {
        public Task<bool> WriteAsync(WriteAgronicaLogInvioAgenda writeAgronicaLogInvioAgenda, AgronicaCoreParametri objParams);
    }
}
