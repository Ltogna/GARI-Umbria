using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Gis.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Gis.DAL.DataLayer.Gis
{
    public class Gis : BaseDALGis, IGis
    {
        public Gis(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) 
        {
        }

        public async Task<DataTable> LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            stbQuery.AppendLine(" SELECT     Algoritmo");
            stbQuery.AppendLine(" FROM       GIS_ProcessingAlgorithms_Cleaning_Algorithm");
            stbQuery.AppendLine(" WHERE      Validita_Inizio <= @dtFine");
            stbQuery.AppendLine(" AND        Validita_Fine >= @dtInizio");

            if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati)
                stbQuery.AppendLine(" AND        Inviato >= 0 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati)
                stbQuery.AppendLine(" AND        Inviato = -1 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti)
            {
                //nessun filtro da aggiungere
            }
            else
                throw new Exception("Parametro non corretto nella query (FlagVisibilita)");

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
    }
}
