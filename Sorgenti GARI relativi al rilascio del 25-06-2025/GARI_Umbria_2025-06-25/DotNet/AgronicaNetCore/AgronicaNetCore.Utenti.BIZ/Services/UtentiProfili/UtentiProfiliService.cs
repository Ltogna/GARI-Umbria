using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.BIZ.Resources;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.Utenti.BIZ.Services.UtentiProfili
{
    public class UtentiProfiliService : BaseServiceUtentiBIZ, IUtentiProfiliService
    {
        private readonly IUtentiProfili _utentiProfiliDAL;

        public UtentiProfiliService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _utentiProfiliDAL = _serviceProvider.GetRequiredService<IUtentiProfili>();
        }

        public async Task<string?> ReadAsync(AgronicaCoreParametri objParametriUser, int idServizio = 0)
        {
            string? res = null;
            DataTable? result = null;
            try
            {
                result = await _utentiProfiliDAL.ReadAsync( objParametriUser, idServizio);

                if (result != null && result.Rows.Count > 0)
                {
                    res = result.Rows[0]["Descrizione_2"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
            }

            return res;
        }    
    }
}
