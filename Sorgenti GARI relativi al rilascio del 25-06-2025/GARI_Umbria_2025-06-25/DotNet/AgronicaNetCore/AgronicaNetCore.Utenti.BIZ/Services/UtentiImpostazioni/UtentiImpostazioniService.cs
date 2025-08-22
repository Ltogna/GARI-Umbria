using AgronicaCoreDTOStd.OutData;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.BIZ.Resources;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioni;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.Utenti.BIZ.Services.UtentiImpostazioni
{
    public class UtentiImpostazioniService : BaseServiceUtentiBIZ, IUtentiImpostazioniService
    {
        private readonly IUtentiImpostazioni _utentiImpostazioniDAL;

        public UtentiImpostazioniService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _utentiImpostazioniDAL = _serviceProvider.GetRequiredService<IUtentiImpostazioni>();
        }

        public async Task<DataTable?> Read_User_Then_SuperUserAsync(int cod, AgronicaCoreParametri objParametriUser)
        {
            DataTable? res = null;

            try
            {
                res = await _utentiImpostazioniDAL.Read_User_Then_SuperUserAsync(cod, objParametriUser);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
            }

            return res;
        }

        public async Task<DataTable?> ReadAsync(int cod, int User1_SuperUser2, AgronicaCoreParametri objParametriUser)
        {
            DataTable? res = null;

            try
            {
                res = await _utentiImpostazioniDAL.ReadAsync(cod, User1_SuperUser2, objParametriUser);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
            }

            return res;
        }

        public async Task<DataInizioEFine> LeggiAnnataAgrariaAsync(AgronicaCoreParametri objParametriUser)
        {
            var dataInizioEFine = new DataInizioEFine();
            try
            {
                dataInizioEFine = await _utentiImpostazioniDAL.CropYearAsync(DateTime.Now, objParametriUser);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
            }

            return dataInizioEFine;
        }

    }
}
