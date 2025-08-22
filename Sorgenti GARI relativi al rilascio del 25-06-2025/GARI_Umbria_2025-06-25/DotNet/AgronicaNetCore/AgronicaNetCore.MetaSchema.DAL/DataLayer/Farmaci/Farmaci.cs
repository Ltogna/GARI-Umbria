using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Farmaci
{
    public class Farmaci : BaseDALMetaschema, IFarmaci
    {
        public Farmaci(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
        }

        public async Task<DataTable> LeggiFarmaciAsync(int Farm_Cod, string Aic, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();

            stbQuery.AppendLine(" SELECT * ");
            stbQuery.AppendLine(" FROM [dbo].[Farmaci] ");
            stbQuery.AppendLine(" WHERE 1 = 1 ");

            if (Farm_Cod != 0)
            {
                stbQuery.AppendLine(" AND Farm_Cod = @farmCod ");
                sqlParams.TryAdd("@farmCod", Farm_Cod);
            }

            if (Aic != "")
            {
                stbQuery.AppendLine(" AND AIC = @aic ");
                sqlParams.TryAdd("@aic", Aic);
            }

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

        }

        public async Task<DataTable> LeggiFarmaciListAICAsync(int Farm_Cod, List<string> Aic, List<string> FamigliaAic, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            Dictionary<string, Dictionary<Type, List<object>>> parSqlIn = new();

            stbQuery.AppendLine(" SELECT * ");
            stbQuery.AppendLine(" FROM [dbo].[Farmaci] ");
            stbQuery.AppendLine(" WHERE 1 = 1 ");

            if (Farm_Cod != 0)
            {
                stbQuery.AppendLine(" AND Farm_Cod = @farmCod ");
                sqlParams.TryAdd("@farmCod", Farm_Cod);
            }

            if (Aic != null && Aic.Count > 0)
            {
                parSqlIn.Add("@AIC", FormatClauseIn(Aic));
                stbQuery.AppendLine(" AND AIC IN (@AIC) ");
            }
            if (FamigliaAic != null && FamigliaAic.Count > 0)
            {
                //SUBSTRING(AIC, 0, 7)
                parSqlIn.Add("@FamAIC", FormatClauseIn(FamigliaAic));
                stbQuery.AppendLine(" AND SUBSTRING(AIC, 0, 7) IN (@FamAIC) ");
            }

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), sqlParams, parSqlIn);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

        }
    }
}
