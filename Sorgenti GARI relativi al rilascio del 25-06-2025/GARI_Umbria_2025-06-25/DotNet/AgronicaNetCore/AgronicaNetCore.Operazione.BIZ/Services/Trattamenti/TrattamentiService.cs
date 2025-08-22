using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Operazione.DAL.DataLayer.Trattamenti;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Operazione.BIZ.Services.Trattamenti
{
    public class TrattamentiService : BaseService, ITrattamentiService
    {
        private readonly ITrattamenti _trattamenti;

        public TrattamentiService(IServiceProvider provider) : base(provider)
        {
            _trattamenti = provider.GetRequiredService<ITrattamenti>();
        }

        public async Task<DataTable> LeggiTrattamentiPerAvversitaAsync(string Piva, int Sa_Cod, int APPEZZA, int ID_REG, int Av_Cod, int Intervallo, AgronicaCoreParametri objP)
        {
            DataTable dt;
            try
            {
                dt = await _trattamenti.LeggiTrattamentiPerAvversitaAsync(Piva, Sa_Cod, APPEZZA, ID_REG, Av_Cod, Intervallo, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }

            return dt;
        }
    }
}
