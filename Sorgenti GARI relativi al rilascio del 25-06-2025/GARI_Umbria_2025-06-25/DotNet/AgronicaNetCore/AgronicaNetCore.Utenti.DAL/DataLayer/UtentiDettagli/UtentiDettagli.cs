using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiDettagli
{
    public class UtentiDettagli : BaseDALUtenti, IUtentiDettagli
    {
        public UtentiDettagli(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable> LeggiAsync(int idServizio, AgronicaCoreParametri objParametriUtenti)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            stbQuery.AppendLine(" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            stbQuery.AppendLine(" SELECT        Utenti_Dettagli.UserName, Utenti_Dettagli.Flag_Azienda_Persona, Utenti_Dettagli.Cognome, ");
            stbQuery.AppendLine("               Utenti_Dettagli.Nome, Utenti_Dettagli.PIVA, Utenti_Dettagli.CodFisc, Utenti_Dettagli.Rag_Soc, ");
            stbQuery.AppendLine("               Utenti_Dettagli.Data_Creazione, Utenti_Dettagli.Data_Modifica, Utenti_Dettagli.UserNameCommerciale, ");
            stbQuery.AppendLine("               Utenti_Profili.Utente, Utenti_Profili.Utente_Profilo, Utenti_dettagli.email, Utenti_dettagli.Tel ");
            stbQuery.AppendLine(" FROM          Utenti_Dettagli (NOLOCK)");
            stbQuery.AppendLine(" INNER JOIN    Utenti_Profili (NOLOCK)");
            stbQuery.AppendLine(" ON            Utenti_Dettagli.UserName = Utenti_Profili.Utente ");
            stbQuery.AppendLine(" WHERE         Utenti_Profili.Utente_Profilo = @superUserUsername");

            if (idServizio != 0)
            {
                stbQuery.AppendLine(" AND           Utenti_Profili.Id_Servizio = @idServizio");
                parametriSql.Add("@idServizio", idServizio);
            }

            if (objParametriUtenti.UtenteUsername != string.Empty)
            {
                stbQuery.AppendLine(" AND           Utenti_Dettagli.UserName = @utenteUsername");
                parametriSql.Add("@utenteUsername", objParametriUtenti.UtenteUsername);
            }

            stbQuery.AppendLine(" ORDER BY      Utenti_Dettagli.UserName ");

            parametriSql.Add("@superUserUsername", objParametriUtenti.SuperUserUsername);

            try
            {
                result = await GetDataProvider(objParametriUtenti).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUtenti, ex);
                throw;
            }

            return result;
        }
    }
}
