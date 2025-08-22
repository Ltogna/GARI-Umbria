using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Gis.DAL.DataLayer.Gis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;
using AgronicaNetCore.Gis.BIZ.Resources;

namespace AgronicaNetCore.Gis.BIZ.Services.Gis
{
    public class GisService : BaseServiceGisBIZ, IGisService
    {
        private readonly IGis _gisDAL;

        public GisService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _gisDAL = _serviceProvider.GetRequiredService<IGis>();            
        }

        public async Task<DataTable> LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                dt = await _gisDAL.LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(objParametri);
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
