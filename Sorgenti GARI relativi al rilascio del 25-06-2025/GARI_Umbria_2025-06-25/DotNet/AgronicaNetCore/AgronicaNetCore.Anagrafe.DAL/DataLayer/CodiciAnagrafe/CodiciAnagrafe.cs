using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Anagrafe.DAL.Resources;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.CodiciAnagrafe
{
    public class CodiciAnagrafe : BaseDALAnagrafe, ICodiciAnagrafe
    {
        private readonly string _impreseCodici = "Imprese_Codici";
        private readonly string _centriAziendaliCodici = "Centri_Aziendali_Codici";
        private readonly string _tabellaCampiCodici = "Campi_Codici";
        private readonly string _tabellaAppezzamentiCodici = "Appezzamento_Codici";
        private readonly string _tabellaRegImpiantiCodici = "Reg_Impianti_Codici";
        private readonly string _tabellaFabbricatiCodici = "Fabbricati_Codici";

        public CodiciAnagrafe(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable> LeggiCodiciUsatixEntitaDestinazioniDUsoAsync(AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable dt;

            stbQuery.AppendLine(" SELECT   Codice, Descrizione");
            stbQuery.AppendLine(" FROM     Codici_Anagrafe ");
            stbQuery.AppendLine(" WHERE    Validita_Inizio <= @dtFine");
            stbQuery.AppendLine(" AND      Validita_Fine >= @dtInizio");
            stbQuery.AppendLine(" AND      Codice >= 3000");
            stbQuery.AppendLine(" AND      Codice < 4000");


            parametriSql.Add("@dtFine", objParametri.FinestraTemporaleFine);
            parametriSql.Add("dtInizio", objParametri.FinestraTemporaleInizio);

            try
            {
                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return dt;
        }

        public async Task<List<CodiceAnagrafeBase>> LeggiCodiciUsatixEntitaUsatixEntitaAsync(int entitaLetturaCodici, int idBudget, AgronicaCoreParametri objParametri)
        {

            //Aziende, Centri e Fabbricati NON esistono in modalità budget

            if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Azienda)
            {
                return await LeggiCodiciUsatixEntitaAsync(_impreseCodici, 0, objParametri);
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.CentroAziendale)
            {
                return await LeggiCodiciUsatixEntitaAsync(_centriAziendaliCodici, 0, objParametri);
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Campo)
            {
                return await LeggiCodiciUsatixEntitaAsync(_tabellaCampiCodici, idBudget, objParametri);
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Appezzamento)
            {
                return await LeggiCodiciUsatixEntitaAsync(_tabellaAppezzamentiCodici, idBudget, objParametri, "Codici_Anagrafe.codice <> " + (int)Enum_CodiciAnagrafe.MetodoDiProduzione);
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Impianto)
            {
                return await LeggiCodiciUsatixEntitaAsync(_tabellaRegImpiantiCodici, idBudget, objParametri, "Progetto_Cod = 0");
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Esercizio)
            {
                return await LeggiCodiciUsatixEntitaAsync(_tabellaRegImpiantiCodici, idBudget, objParametri, "Progetto_Cod <> 0");
            }
            else if (entitaLetturaCodici == (int)Enum_Entita_FiltroRicerca.Fabbricato)
            {
                return await LeggiCodiciUsatixEntitaAsync(_tabellaFabbricatiCodici, 0, objParametri);
            }
            else
                throw new NotImplementedException();
        }


        /// <summary>
        /// Effettua la lettura dei codici (NON RENDERE PUBBLICO, sarebbe suscettibile al sql injection)
        /// </summary>
        /// <param name="objParametri"></param>
        /// <param name="tabellaCodice"></param>
        /// <param name="filtroAggiuntivo"></param>
        /// <returns></returns>
        private async Task<List<CodiceAnagrafeBase>> LeggiCodiciUsatixEntitaAsync(string tabellaCodice, int idBudget, AgronicaCoreParametri objParametri, string filtroAggiuntivo = "")
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable dt;

            if (idBudget > 0)
                tabellaCodice = "Budget_" + tabellaCodice;

            stbQuery.AppendLine(" SELECT   DISTINCT Codice, Descrizione, TipoControllo_Cod");
            stbQuery.AppendLine(" FROM     Codici_Anagrafe WITH (NOLOCK) ");
            stbQuery.AppendLine($" JOIN     {tabellaCodice} tabellaCodice WITH (NOLOCK)");
            stbQuery.AppendLine(" ON     tabellaCodice.id_cod = Codici_Anagrafe.codice");

            if (idBudget > 0)
                stbQuery.AppendLine(" AND     tabellaCodice.Id_Budget = @idBudget");

            stbQuery.AppendLine(" WHERE    1 = 1 ");

            if (CodiciSkip.Length > 0)
                stbQuery.AppendLine(" AND      Codice NOT IN (" + string.Join(",", CodiciSkip) + ") ");

            //Escludo destinazioni d'uso (codice <3000 or codice >= 4000) e i codici cliente (codice <2000)
            stbQuery.AppendLine(" AND      (Codice < 2000 OR Codice >= 4000) ");
            stbQuery.AppendLine(" AND      (Codice <= 10000) ");

            if (!string.IsNullOrEmpty(filtroAggiuntivo))
                stbQuery.AppendLine($" AND ({filtroAggiuntivo})");

            parametriSql.Add("@idBudget", idBudget);

            try
            {
                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            var codiciAnagrafeBase = new List<CodiceAnagrafeBase>();

            foreach (DataRow row in dt.Rows)
            {
                var codiceAnagrafe = new CodiceAnagrafeBase((int)row["Codice"], (string)row["Descrizione"])
                {
                    TipoControllo_Cod = (int)row["TipoControllo_Cod"]
                };
                codiciAnagrafeBase.Add(codiceAnagrafe);
            }

            return codiciAnagrafeBase;
        }
    }
}
