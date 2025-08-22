using AgronicaNetCore.Base.Models;
using AgronicaNetCore.UtilityDB.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.UtilityDB.DAL.DataLayer.Utenti_CodiciGiasPro
{
    public class Utenti_CodiciGiasPro : BaseDALUtilityDB, IUtenti_CodiciGiasPro
    {
        public Utenti_CodiciGiasPro(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<int> LeggiProgressivoGIASAsync(AgronicaCoreParametri objParametri)
        {
            var strQuery = "";
            var parametriSql = new Dictionary<string, object>();
            DataTable result;


            strQuery = @"Select
                            ProgressivoGIAS
                         from Utenti_CodiciGiasPro
                         where
                            UserName=@p1
                        ";

            parametriSql.Add("@p1", objParametri.SuperUserUsername);

            try
            {
                result = await GetDataProvider(objParametri).ExecuteReadAsync(strQuery, parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return (result!= null && result.Rows.Count>0 ? (int)result.Rows[0]["ProgressivoGIAS"] : 0);
        }
    }
}
