using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Servizi
{
    public class Servizi : BaseDALMetaschema, IServizi
    {
        public Servizi(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable> LeggiServiziEffettivamenteUsatiAsync(LeggiServizi_IN leggiServizi_IN, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            stbQuery.AppendLine(" SELECT     DISTINCT s.Servizio_Cod, s.Servizio_Des");
            stbQuery.AppendLine(" FROM       Servizi s");
            stbQuery.AppendLine(" JOIN       Pratiche p");
            stbQuery.AppendLine(" ON         s.Servizio_Cod = p.Servizio_Cod");
            stbQuery.AppendLine(" WHERE      s.Validita_Inizio <= @dtFine");
            stbQuery.AppendLine(" AND        s.Validita_Fine >= @dtInizio");

            if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati)
                stbQuery.AppendLine(" AND        s.Inviato >= 0 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati)
                stbQuery.AppendLine(" AND        s.Inviato = -1 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti)
            {
                //nessun filtro da aggiungere
            }
            else
                throw new Exception("Parametro non corretto nella query (FlagVisibilita)");

            if (leggiServizi_IN.Servizio_Cod != 0)
            {
                stbQuery.AppendLine(" AND        s.Servizio_Cod = @servizioCod");
                parametriSql.Add("@servizioCod", leggiServizi_IN.Servizio_Cod);
            }

            if (!string.IsNullOrEmpty(leggiServizi_IN.Servizio_Des))
            {
                stbQuery.AppendLine(" AND        s.Servizio_Des LIKE CONCAT('%', @servizioDes, '%')");
                parametriSql.Add("@servizioDes", leggiServizi_IN.Servizio_Des);
            }

            stbQuery.AppendLine(" ORDER BY   s.Servizio_Cod");

            parametriSql.Add("@dtInizio", objParametri.FinestraTemporaleInizio);
            parametriSql.Add("@dtFine", objParametri.FinestraTemporaleFine);

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

        public async Task<DataTable> LeggiServizi_StatiAsync(LeggiServiziStati_IN leggiServiziStati_IN, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            stbQuery.AppendLine(" SELECT * FROM ( ");

            stbQuery.AppendLine("   SELECT     WAnagraficaStati.WAnagraficaStati_Cod AS Stato_Cod, WAnagraficaStati.WAnagraficaStati_Des AS Stato_Des, WTransizioniDiStatoConfigurazione.Servizio_Cod ");
            stbQuery.AppendLine("   FROM       WTransizioniDiStatoConfigurazione ");
            stbQuery.AppendLine("   JOIN       WAnagraficaStati ON WTransizioniDiStatoConfigurazione.Stato_Origine_Cod = WAnagraficaStati.WAnagraficaStati_Cod ");

            stbQuery.AppendLine("   UNION ");

            stbQuery.AppendLine("   SELECT     WAnagraficaStati.WAnagraficaStati_Cod AS Stato_Cod, WAnagraficaStati.WAnagraficaStati_Des AS Stato_Des, WTransizioniDiStatoConfigurazione.Servizio_Cod ");
            stbQuery.AppendLine("   FROM       WTransizioniDiStatoConfigurazione ");
            stbQuery.AppendLine("   JOIN       WAnagraficaStati ON WTransizioniDiStatoConfigurazione.Stato_Destinazione_cod = WAnagraficaStati.WAnagraficaStati_Cod ");
       
            stbQuery.AppendLine(" ) X ");

            stbQuery.AppendLine(" WHERE      1 = 1 ");

            if (leggiServiziStati_IN.Stato_Cod != 0)
            {
                stbQuery.AppendLine(" AND        Stato_Cod = @statoCod");
                parametriSql.Add("@statoCod", leggiServiziStati_IN.Stato_Cod);
            }

            if (leggiServiziStati_IN.Servizio_Cod != 0)
            {
                stbQuery.AppendLine(" AND        Servizio_Cod = @servizioCod");
                parametriSql.Add("@servizioCod", leggiServiziStati_IN.Servizio_Cod);
            }

            if (!string.IsNullOrEmpty(leggiServiziStati_IN.Stato_Des))
            {
                stbQuery.AppendLine(" AND        Stato_Des LIKE CONCAT('%', @statoDes, '%')");
                parametriSql.Add("@statoDes", leggiServiziStati_IN.Stato_Des);
            }

            stbQuery.AppendLine(" ORDER BY        Stato_Cod");

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
    }
}
