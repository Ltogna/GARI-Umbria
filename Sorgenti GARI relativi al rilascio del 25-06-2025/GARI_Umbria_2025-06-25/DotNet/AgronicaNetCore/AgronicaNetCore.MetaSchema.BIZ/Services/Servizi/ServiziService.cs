using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.BIZ.Resources;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Servizi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.Servizi
{
    public class ServiziService : BaseServiceMetaschemaBIZ, IServiziService
    {
        private readonly IServizi _serviziDAL;

        public ServiziService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _serviziDAL = _serviceProvider.GetRequiredService<IServizi>();
        }

        public async Task<DataTable> LeggiServiziEffettivamenteUsatiAsync(LeggiServizi_IN leggiServizi_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                dt = await _serviziDAL.LeggiServiziEffettivamenteUsatiAsync(leggiServizi_IN, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return dt;
        }

        public async Task<DataTable> LeggiServizi_StatiAsync(LeggiServiziStati_IN leggiServiziStati_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                dt = await _serviziDAL.LeggiServizi_StatiAsync(leggiServiziStati_IN, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return dt;
        }
    }
}
