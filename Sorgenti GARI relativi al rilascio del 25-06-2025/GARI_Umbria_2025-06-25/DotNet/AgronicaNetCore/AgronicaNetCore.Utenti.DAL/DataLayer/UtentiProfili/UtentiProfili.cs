using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili
{
    public class UtentiProfili : BaseDALUtenti, IUtentiProfili
    {
        public UtentiProfili(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable?> ReadAsync(AgronicaCoreParametri objParametriUser, int idServizio = 0)

        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataTable? result;

            try
            {

                stbQuery.AppendLine(" SELECT * ");
                stbQuery.AppendLine(" FROM Utenti_Profili (NOLOCK) ");
                stbQuery.AppendLine(" WHERE 1 = 1 ");
                stbQuery.AppendLine(" AND Utente_Profilo = @SuperUserUsername ");
                stbQuery.AppendLine(" AND Utente = @Username ");

                if (idServizio != 0)
                {
                    stbQuery.AppendLine(" AND Id_Servizio = @idServizio");
                    parSql.Add("@idServizio", idServizio);
                }

                parSql.Add("@SuperUserUsername", objParametriUser.SuperUserUsername);
                parSql.Add("@Username", objParametriUser.UtenteUsername);

                result = await GetDataProvider(objParametriUser).ExecuteReadAsync(stbQuery.ToString(), parSql);           
                            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
                result = null;
            }
            return result;
        }

        private async Task<bool> VisibilitaTotale(AgronicaCoreParametri objParametriUser, int idServizio = 0)
        {
            bool result = false;
            DataTable dt = await ReadAsync(objParametriUser, idServizio);
            if (dt == null || dt.Rows.Count == 0)
            {
                return result;
            }
            else
            {
                if (dt.Rows[0]["Descrizione_2"] == "")
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> VisibilitaTotaleGiasOnline(AgronicaCoreParametri objParametriUser)
        {
            return await VisibilitaTotale(objParametriUser, 5);
        }
    }
}
