using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiVisibilitaAppoggio
{
    public class UtentiVisibilitaAppoggio : BaseDALUtenti, IUtentiVisibilitaAppoggio
    {
        public UtentiVisibilitaAppoggio(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable?> ReadAsync(int entity, AgronicaCoreParametri objParametriUser, string piva = "")

        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataTable? result;

            try
            {
                stbQuery.AppendLine(" SELECT * ");
                stbQuery.AppendLine(" FROM Utenti_Visibilita_Appoggio WITH (NOLOCK) ");
                stbQuery.AppendLine(" WHERE PivaSuperUser = @Piva_SuperUser ");
                stbQuery.AppendLine(" AND Username = @Username ");

                if (entity > 0)
                    stbQuery.AppendLine(" AND Entita_Cod = @entity ");

                if (piva != string.Empty)
                {
                    stbQuery.AppendLine(" AND Piva = @Piva ");
                    parSql.Add("@Piva", piva);
                }

                parSql.Add("@entity", entity);
                parSql.Add("@Piva_SuperUser", objParametriUser.PivaSuperUser);
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

        public async Task<DataTable?> ReadVisibilitaCentriAsync(string? piva, AgronicaCoreParametri objParametriUser)

        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataTable? result;

            try
            {
                stbQuery.AppendLine(" SELECT * ");
                stbQuery.AppendLine(" FROM Utenti_Visibilita_Appoggio WITH (NOLOCK) ");
                stbQuery.AppendLine(" WHERE PivaSuperUser = @Piva_SuperUser ");
                stbQuery.AppendLine(" AND Username = @Username ");
                stbQuery.AppendLine(" AND Entita_Cod = @Entity ");

                if (!string.IsNullOrEmpty(piva))
                {
                    stbQuery.AppendLine("AND Piva = @Piva");
                }

                parSql.Add("@Entity", TipiEnumerativi.Enum_TipoEntita.Centro);
                parSql.Add("@Piva_SuperUser", objParametriUser.PivaSuperUser);
                parSql.Add("@Username", objParametriUser.UtenteUsername);
                if (!string.IsNullOrEmpty(piva))
                {
                    parSql.Add("@Piva", piva);
                }

                result = await GetDataProvider(objParametriUser).ExecuteReadAsync(stbQuery.ToString(), parSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
                result = null;
            }

            return result;
        }
    }
}
