using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.BIZ.Resources;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.UnitaMisuraConversione;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.UnitaMisuraConversione
{
    public class UnitaMisuraConversioneService : BaseServiceMetaschemaBIZ, IUnitaMisuraConversioneService
    {
        public UnitaMisuraConversioneService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
        }

        public async Task<DataTable> ReadAsync(UnitaMisuraConversione_IN leggiUnitaMisuraConversioneIN, AgronicaCoreParametri objParametri)
        {
            DataTable res;

            try
            {
                var unitaMisuraConversioneDAL = _serviceProvider.GetRequiredService<IUnitaMisuraConversione>();
                res = await unitaMisuraConversioneDAL.ReadAsync(leggiUnitaMisuraConversioneIN, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return res;
        }
    }
}
