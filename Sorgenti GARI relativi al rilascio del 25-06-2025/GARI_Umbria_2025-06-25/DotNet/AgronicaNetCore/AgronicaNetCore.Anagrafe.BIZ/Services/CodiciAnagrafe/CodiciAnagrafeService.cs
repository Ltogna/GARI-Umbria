using AgronicaCoreDTOStd.InData.Anagrafica;
using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Anagrafe.BIZ.Resources;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.CodiciAnagrafe;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.CodiciAnagrafe
{
    public class CodiciAnagrafeService : BaseServiceAnagrafeBIZ, ICodiciAnagrafeService
    {
        private readonly ICodiciAnagrafe _codiciAnagrafeDal;

        public CodiciAnagrafeService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _codiciAnagrafeDal = _serviceProvider.GetRequiredService<ICodiciAnagrafe>();
        }

        public async Task<List<CodiceAnagrafeBase>> LeggiCodiciUsatixEntitaAsync(LeggiCodiciUsatixEntitaAnagrafe_IN LeggiCodiciUsatixEntitaAnagrafe, AgronicaCoreParametri objParametri)
        {
            List<CodiceAnagrafeBase> codiciAnagrafe;
            try
            {
                codiciAnagrafe = await _codiciAnagrafeDal.LeggiCodiciUsatixEntitaUsatixEntitaAsync(LeggiCodiciUsatixEntitaAnagrafe.entita, LeggiCodiciUsatixEntitaAnagrafe.idBudget, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return codiciAnagrafe;
        }

        public async Task<DataTable> LeggiCodiciUsatixEntitaDestinazioniDUsoAsync(AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                dt = await _codiciAnagrafeDal.LeggiCodiciUsatixEntitaDestinazioniDUsoAsync(objParametri);
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
