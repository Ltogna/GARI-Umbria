using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.BIZ.Resources;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.DivisioniAmministrative;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.DivisioniAmministrative
{
    public class DivisioniAmministrativeService : BaseServiceMetaschemaBIZ, IDivisioniAmministrativeService
    {
        private readonly IDivisioniAmministrative _divisioniAmministrativeDAL;

        public DivisioniAmministrativeService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _divisioniAmministrativeDAL = _serviceProvider.GetRequiredService<IDivisioniAmministrative>();
        }
        public async Task<DataTable> LeggiStatiAsync(string codice, AgronicaCoreParametri objParametri)
        {
            DataTable dt;

            try
            {
                dt = await _divisioniAmministrativeDAL.LeggiStatiAsync(codice, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiRegioniAsync(LeggiRegioni_IN leggiRegioni_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;

            try
            {
                dt = await _divisioniAmministrativeDAL.LeggiRegioniAsync(leggiRegioni_IN, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiProvinceAsync(LeggiProvince_IN leggiProvince_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;

            try
            {
                dt = await _divisioniAmministrativeDAL.LeggiProvinceAsync(leggiProvince_IN, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiComuniAsync(LeggiComuni_IN leggiComuni_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;

            try
            {
                dt = await _divisioniAmministrativeDAL.LeggiComuniAsync(leggiComuni_IN, objParametri);
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
