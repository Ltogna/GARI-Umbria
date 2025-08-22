using AgronicaNetCore.Anagrafe.DAL.Resources;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.GerarchiaImprese
{
    public class GerarchiaImprese : BaseDALAnagrafe, IGerarchiaImprese
    {
        public GerarchiaImprese(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable> LeggixGerarchiaAlberoImprese_VisibilitaAsync(bool applicaVisibilita, AgronicaCoreParametri objParametriServer)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable dt;

            stbQuery.AppendLine(" SELECT DISTINCT a.Padre, a.rag_soc, a.PIVA, ISNULL(IC.Val_Cod, '') AS CUAA, a.Foglia, a.Validazione, a.TipoImpresaGerarchia, a.Blk_Flag, a.validita_fine ");
            stbQuery.AppendLine(" FROM");
            stbQuery.AppendLine(" ( ");
            stbQuery.AppendLine("   SELECT      GerarchiaImprese.Padre, Imprese.rag_soc, Imprese.PIVA, GerarchiaImprese.Foglia, Imprese.Validazione, Imprese.TipoImpresaGerarchia , Imprese.Blk_Flag, Imprese.validita_fine  ");
            stbQuery.AppendLine("   FROM        GerarchiaImprese (NOLOCK) ");
            stbQuery.AppendLine("   INNER JOIN  Imprese (NOLOCK) ");
            stbQuery.AppendLine("   ON          GerarchiaImprese.Figlio = Imprese.PIVA   ");

            if (applicaVisibilita)
            {
                stbQuery.AppendLine("   LEFT JOIN   Utenti_Visibilita_Appoggio uvap (NOLOCK)  ");
                stbQuery.AppendLine("   ON          GerarchiaImprese.Padre = uvap.Piva");
                stbQuery.AppendLine("   AND         uvap.Entita_Cod = 1 ");
                stbQuery.AppendLine("   AND         uvap.sa_cod = 0  ");
                stbQuery.AppendLine("   AND         uvap.Username = @utenteUsername ");

                stbQuery.AppendLine("   LEFT JOIN   Utenti_Visibilita_Appoggio uvaf (NOLOCK)  ");
                stbQuery.AppendLine("   ON          GerarchiaImprese.Figlio = uvaf.Piva");
                stbQuery.AppendLine("   AND         uvaf.Entita_Cod = 1 ");
                stbQuery.AppendLine("   AND         uvaf.sa_cod = 0  ");
                stbQuery.AppendLine("   AND         uvaf.Username = @utenteUsername ");

                parametriSql.Add("@utenteUsername", objParametriServer.UtenteUsername);
            }

            stbQuery.AppendLine("   WHERE       1 = 1");

            if (applicaVisibilita)
            {
                stbQuery.AppendLine("   AND NOT     (uvap.Piva IS NULL AND uvaf.Piva is NULL) ");
            }
            stbQuery.AppendLine("     ) as a ");

            stbQuery.AppendLine("     LEFT JOIN Imprese_Codici IC ON IC.Piva = a.Piva AND IC.id_cod = " + (int)Enum_CodiciAnagrafe.CodiceCUAA + " ");

            stbQuery.AppendLine(" ORDER BY        a.rag_soc ");

            try
            {
                dt = await GetDataProvider(objParametriServer).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

            return dt;
        }
    }
}
