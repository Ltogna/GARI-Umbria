using System.Text;
using System.Data;
using AgronicaNetCore.Base.Models;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Anagrafe.DAL.Resources;
using AgronicaCoreDTOStd.Identity;
using AgronicaNetCore.Base.Constants;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Impresa
{
    public class Impresa : BaseDALAnagrafe, IImpresa
    {
        public Impresa(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
        }

        public async Task<DataTable?> LeggiClausolaInAsync(List<string> elencoPiva,List<int> elencoVegCod,int varieta, AgronicaCoreParametri objParametri)
        {
            var strSql = "";
            var parSql = new Dictionary<string, object>();
            var parSqlIn = new Dictionary<string, Dictionary<Type,List<object>>>();
            DataTable? result = null;

            try
            {
                if (elencoPiva ==null || elencoPiva.Count<=0)
                    throw new Exception("Specificare l'elenco delle partita iva");

                strSql = @$"
                    Select
                        rag_soc,
                        a.piva,
                        sa_cod,
                        appezza,
                        id_reg,
                        a.validita_inizio,
                        a.validita_fine
                    from reg_impianti a inner join imprese b on (a.piva=b.piva)
                    where
                        a.piva in (@in_1)
                        and (cul_cod in (select cul_cod from cultivar where veg_cod in (@in_2) or cul_cod=@cod_1))
                        and b.piva in (@in_1)
                    ";


                parSqlIn.Add("@in_1", FormatClauseIn(elencoPiva));
                parSqlIn.Add("@in_2", FormatClauseIn(elencoVegCod));

                parSql.Add("@cod_1", varieta);

                result = await GetDataProvider(objParametri).ExecuteReadAsync(strSql, parSql,parSqlIn);

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                result = null;
            }
            return result;
        }

        public async Task<DataTable?> LeggiAsync(string piva, AgronicaCoreParametri objParametri)
        {

            var strSql = "";
            var parSql = new Dictionary<string, object>();
            DataTable? result = null;

            try
            {
                if (string.IsNullOrEmpty(piva))
                    throw new Exception("Specificare la partita iva");

                strSql = @$"
                    Select
                        piva,
                        rag_soc,
                        validita_inizio,
                        validita_fine
                    from Imprese
                    where
                        piva=@p1
                    ";
                parSql.Add("@p1", piva);

                result = await GetDataProvider(objParametri).ExecuteReadAsync(strSql, parSql);

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                result = null;
            }
            return result;
        }

        public async Task<DataTable> LeggiImpreseAsync(string piva, DataTable dtImpreseVisibili, AgronicaCoreParametri objParametriServer)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            var visibilitaLimitata = dtImpreseVisibili is not null && dtImpreseVisibili.Rows.Count > 0;

            stbQuery.AppendLine(" SELECT ");
            stbQuery.AppendLine("               i.Piva, i.Rag_Soc");
            stbQuery.AppendLine(" FROM ");
            stbQuery.AppendLine("               Imprese i");
            if (visibilitaLimitata)
            {
                stbQuery.AppendLine(" JOIN ");
                stbQuery.AppendLine("               Utenti_Visibilita_Appoggio uvap");
                stbQuery.AppendLine("               ON i.Piva = uvap.Piva");
            }

            stbQuery.AppendLine(" WHERE");
            stbQuery.AppendLine("               1 = 1");

            if (!string.IsNullOrEmpty(piva))
            {
                stbQuery.AppendLine("               and i.Piva = @piva");
                parametriSql.Add("@piva", piva);
            }

            if (visibilitaLimitata)
            {
                stbQuery.AppendLine("               and uvap.Entita_Cod = @entitaCod");
                stbQuery.AppendLine("               and uvap.Username = @username");
                stbQuery.AppendLine("               and uvap.PivaSuperUser = @pivaSuperUser");

                parametriSql.Add("@entitaCod", (int)Enum_TipoEntita.Impresa);
                parametriSql.Add("@username", objParametriServer.UtenteUsername);
                parametriSql.Add("@pivaSuperUser", objParametriServer.PivaSuperUser);
            }

            try
            {
                result = await GetDataProvider(objParametriServer).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

            return result;
        }

        public async Task<DataTable> LeggiPadriAsync(DataTable dtImpreseVisibili, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            var visibilitaLimitata = dtImpreseVisibili is not null && dtImpreseVisibili.Rows.Count > 0;

            stbQuery.AppendLine(" SELECT       Imprese.Piva, Imprese.Rag_Soc");
            stbQuery.AppendLine(" FROM         Imprese (NOLOCK)");
            stbQuery.AppendLine(" JOIN         UtentiXImprese (NOLOCK)");
            stbQuery.AppendLine(" ON           Imprese.Piva = UtentiXImprese.Piva");
            
            if (visibilitaLimitata)
            {
                stbQuery.AppendLine(" JOIN         Utenti_Visibilita_Appoggio  (NOLOCK) ");
                stbQuery.AppendLine(" ON           Imprese.Piva = Utenti_Visibilita_Appoggio.Piva ");
                stbQuery.AppendLine(" AND          Utenti_Visibilita_Appoggio.Entita_Cod = @entitaCod");
            }
            
            stbQuery.AppendLine(" JOIN         ImpresexIndirizzi  (NOLOCK) ");
            stbQuery.AppendLine(" ON           Imprese.PIVA = ImpresexIndirizzi.PIVA");
            stbQuery.AppendLine(" WHERE        Imprese.Validita_Inizio <= @dtFine");
            stbQuery.AppendLine(" AND          Imprese.Validita_Fine >= @dtInizio");
            stbQuery.AppendLine(" AND          UtentiXImprese.[User] = @pivaSuperUser");
            stbQuery.AppendLine(" AND          Imprese.TipoImpresaGerarchia <> 1");
            stbQuery.AppendLine(" AND          ImpresexIndirizzi.Tipo_Indirizzo = 1");
            
            if (visibilitaLimitata)
            {
                stbQuery.AppendLine(" AND          Utenti_Visibilita_Appoggio.Username = @username");
            }

            stbQuery.AppendLine(" ORDER BY     Imprese.Rag_Soc");

            parametriSql.Add("@dtInizio", objParametri.FinestraTemporaleInizio);
            parametriSql.Add("@dtFine", objParametri.FinestraTemporaleFine);
            parametriSql.Add("@pivaSuperUser", objParametri.PivaSuperUser);
            
            if (visibilitaLimitata)
            {
                parametriSql.Add("@entitaCod", (int)Enum_TipoEntita.Impresa);
                parametriSql.Add("@username", objParametri.UtenteUsername);
            }

            try
            {
                result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return result;
        }

        public async Task<string> PivaFromCuaaAsync(string cuaa, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            stbQuery.AppendLine("SELECT PIVA FROM Imprese_Codici");
            stbQuery.AppendLine("WHERE id_cod = @idCod AND val_cod = @cuaa");
            parametriSql.Add("@idCod", IMPRESE_CODICI.CUAA);
            parametriSql.Add("@cuaa", cuaa);

            try
            {
                DataTable result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
                if (result.Rows.Count > 0)
                {
                    return result.Rows[0]["PIVA"].ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return "";
        }
    }
}
