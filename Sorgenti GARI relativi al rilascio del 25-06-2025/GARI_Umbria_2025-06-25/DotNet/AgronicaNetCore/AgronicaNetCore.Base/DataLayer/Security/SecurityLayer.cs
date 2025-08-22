using AgronicaNetCore.Anagrafe.DAL.Base;
using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Base.DataLayer.Security
{
    public class SecurityLayer : DAL_Base, ISecurityLayer
    {

        public SecurityLayer(IServiceProvider provider) : base(provider, true)
        {
        }

        [Obsolete("Usare la versione asincrona del metodo")]
        public DataTable LeggiConnessioni(AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            stbQuery.AppendLine("SELECT * FROM Connessioni (NOLOCK) ");

            try
            {
                return GetDataProvider(objParams).ExecuteRead(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }

        public async Task<DataTable> LeggiConnessioniAsync(AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            stbQuery.AppendLine("SELECT * FROM Connessioni (NOLOCK) ");

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }

        [Obsolete("Usare la versione asincrona del metodo")]
        public DataTable LeggiConfigurazioneSiti(string chiave, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            CreateQueryLeggiConfigurazioneSiti(stbQuery, parametriSql, chiave);

            try
            {
                return GetDataProvider(objParams).ExecuteRead(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }

        public async Task<DataTable> LeggiConfigurazioneSitiAsync(string chiave, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            CreateQueryLeggiConfigurazioneSiti(stbQuery, parametriSql, chiave);

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }

        public async Task<string> LeggiConfigurazioneSitiScalareAsync(string chiave, AgronicaCoreParametri objParams_Server, AgronicaCoreParametri objParams_Super_Server)
        {

            string valore = string.Empty;

            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();

            CreateQueryLeggiConfigurazioneSiti(stbQuery, parametriSql, chiave);

            try
            {

                //Leggo prima dal Server
                DataTable dt = await GetDataProvider(objParams_Server).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
          
                if (dt.Rows.Count == 0)
                {
                    //Se non trovo nulla leggo dal SuperServer
                    dt = await GetDataProvider(objParams_Super_Server).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
                }

                if (dt.Rows.Count > 0)
                {
                    valore = dt.Rows[0]["Valore"].ToString()!;
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams_Server, ex);
                throw;
            }

            return valore;

        }

        private void CreateQueryLeggiConfigurazioneSiti(StringBuilder stbQuery, Dictionary<string, object> parametriSql, string chiave)
        {
            stbQuery.AppendLine("SELECT * FROM Configurazione_Siti (NOLOCK) ");

            if (!string.IsNullOrEmpty(chiave))
            {
                stbQuery.AppendLine("WHERE Chiave=@Chiave");
                parametriSql.TryAdd("@Chiave", chiave);
            }
        }
    }
}
