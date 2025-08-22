using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Anagrafe.DAL.Resources;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Apezzamenti
{
    public class Appezzamenti : BaseDALAnagrafe, IAppezzamenti
    {
        public Appezzamenti(IServiceProvider provider, IStringLocalizer<Messages> localizer, bool securityBypass = false) : base(provider, localizer, securityBypass)
        {
        }

        /// <summary>
        /// Permette di blocca/sbloccare un appezzamento.
        /// </summary>
        /// <param name="block">True se l'appezzamento è da bloccare, False altrimenti.</param>
        /// <returns>True se l'operazione è andata a buon fine, False altrimenti.</returns>
        public async Task<bool> BloccaSbloccaAsync(bool block, Appezzamento.PK appezzamento, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("UPDATE Appezzamento SET ");
                stbQuery.AppendLine("  Username_Modifica = @user, ");
                stbQuery.AppendLine("  Data_Modifica = GETDATE(), ");
                if (block)
                {
                    stbQuery.AppendLine("  Blk_Flag = 1, ");
                    stbQuery.AppendLine("  Blk_Inizio_Data = GETDATE(), ");
                    stbQuery.AppendLine("  Blk_Inizio_Username = @user, ");
                    stbQuery.AppendLine("  Blk_Inizio_Note = '', ");
                    stbQuery.AppendLine("  Blk_Fine_Data = @agrofine, ");
                    stbQuery.AppendLine("  Blk_Fine_Username = @user, ");
                    stbQuery.AppendLine("  Blk_Fine_Note = '' ");
                }
                else
                {
                    stbQuery.AppendLine("  Blk_Flag = 0, ");
                    stbQuery.AppendLine("  Blk_Inizio_Data = @agroinizio, ");
                    stbQuery.AppendLine("  Blk_Inizio_Username = '', ");
                    stbQuery.AppendLine("  Blk_Inizio_Note = '', ");
                    stbQuery.AppendLine("  Blk_Fine_Data = @agrofine, ");
                    stbQuery.AppendLine("  Blk_Fine_Username = '', ");
                    stbQuery.AppendLine("  Blk_Fine_Note = '' ");
                }
                stbQuery.AppendLine("WHERE");
                stbQuery.AppendLine(" Appezzamento.piva = @piva ");
                stbQuery.AppendLine(" AND Appezzamento.sa_cod = @saCod ");
                stbQuery.AppendLine(" AND Appezzamento.appezza = @appezza ");

                if (appezzamento.centroAziendalePK.partitaIva == "" || appezzamento.centroAziendalePK.codice == 0 || appezzamento.codice == 0)
                {
                    throw new ArgumentException(_localizer.GetString("InvalidParameters").Value);
                }
                sqlParams.TryAdd("@user", objParametri.UsernameOperazione);
                sqlParams.TryAdd("@agrofine", CostantiPersonalizzate.AGRODATAFINE);
                sqlParams.TryAdd("@agroinizio", CostantiPersonalizzate.AGRODATAINIZIO);
                sqlParams.TryAdd("@piva", appezzamento.centroAziendalePK.partitaIva);
                sqlParams.TryAdd("@saCod", appezzamento.centroAziendalePK.codice);
                sqlParams.TryAdd("@appezza", appezzamento.codice);

                await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                return false;
            }
        }

        public async Task<bool> IsBlockedAsync(Appezzamento.PK appezzamento, DateTime atDate, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("SELECT ");
                stbQuery.AppendLine(" Blk_Flag, Blk_Inizio_Data, Blk_Fine_Data ");
                stbQuery.AppendLine("FROM Appezzamento ");
            
                stbQuery.AppendLine("WHERE");
                stbQuery.AppendLine(" Appezzamento.piva = @piva ");
                stbQuery.AppendLine(" AND Appezzamento.sa_cod = @saCod ");
                stbQuery.AppendLine(" AND Appezzamento.appezza = @appezza ");

                if (appezzamento.centroAziendalePK.partitaIva == "" || appezzamento.centroAziendalePK.codice == 0 || appezzamento.codice == 0)
                {
                    throw new ArgumentException(_localizer.GetString("InvalidParameters").Value);
                }
                sqlParams.TryAdd("@piva", appezzamento.centroAziendalePK.partitaIva);
                sqlParams.TryAdd("@saCod", appezzamento.centroAziendalePK.codice);
                sqlParams.TryAdd("@appezza", appezzamento.codice);

                DataTable dt = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new InvalidOperationException(_localizer.GetString("NoRowsFound").Value);
                }
                DataRow row = dt.Rows[0];
                return (int)row["Blk_Flag"] == 1 && (DateTime)row["Blk_Inizio_Data"] <= atDate && (DateTime)row["Blk_Fine_Data"] >= atDate;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
        }
    }
}
