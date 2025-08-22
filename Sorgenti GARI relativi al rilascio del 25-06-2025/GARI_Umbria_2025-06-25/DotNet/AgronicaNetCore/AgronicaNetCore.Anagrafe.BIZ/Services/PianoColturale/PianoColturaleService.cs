using AgronicaNetCore.Anagrafe.BIZ.Resources;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.PianoColturale;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.PianoColturale
{
    public class PianoColturaleService : BaseServiceAnagrafeBIZ, IPianoColturaleService
    {
        public PianoColturaleService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
        }

        public async Task<DataTable?> GetPlanningsAsync(string piva, AgronicaCoreParametri objParametri)
        {
            DataTable? res = null;
            try
            {
                res = await ReadPlanningsAsync(piva, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
            }
            return res;
        }

        private async Task<DataTable?> ReadPlanningsAsync(String piva, AgronicaCoreParametri objParametri)
        {
            try
            {
                var pianocolturaleDal = _serviceProvider.GetRequiredService<IPianoColturale>();

                if (pianocolturaleDal == null)
                    throw new Exception("Riferimento a servizi DAL per piano colturale o planning non valorizzati. impossibile proseguire");

                var DT_plans = await pianocolturaleDal.ReadPlanningsByCompanyAsync(piva, objParametri);

                //if (DT_plans == null)
                //    throw new Exception("Dati planning non valorizzati. verificare lettura");

                return DT_plans;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                return null;
            }

        }
    }
}
