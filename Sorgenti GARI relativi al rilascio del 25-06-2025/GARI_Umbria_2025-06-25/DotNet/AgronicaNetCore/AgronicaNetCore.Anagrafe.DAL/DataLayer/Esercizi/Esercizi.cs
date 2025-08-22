using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;
using AgronicaNetCore.Anagrafe.DAL.Resources;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Esercizi
{
    public class Esercizi : BaseDALAnagrafe, IEsercizi
    {
        public Esercizi(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable?> LeggiAsync(
            AgronicaCoreParametri objParametri,
            string piva = "", int saCod = 0, int appezza = 0, int idReg = 0, int progCod = -1,
            DateTime? inizio = null, DateTime? fine = null
        )
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("SELECT * FROM Imprese_Progetti");
                stbQuery.AppendLine("WHERE 1 = 1");
                if (inizio != null)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Validita_Fine >= @inizio ");
                    sqlParams.TryAdd("@inizio", inizio);
                }
                if (fine != null)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Validita_Inizio <= @fine ");
                    sqlParams.TryAdd("@fine", fine);
                }
                if (piva != "")
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Piva = @piva ");
                    sqlParams.TryAdd("@piva", piva);
                }
                if (saCod != 0)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Sa_Cod = @saCod ");
                    sqlParams.TryAdd("@saCod", saCod);
                }
                if (appezza != 0)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Appezza = @appezza ");
                    sqlParams.TryAdd("@appezza", appezza);
                }
                if (idReg != 0)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Id_Reg = @idReg ");
                    sqlParams.TryAdd("@idReg", idReg);
                }
                if (progCod != -1)
                {
                    stbQuery.AppendLine(" AND Imprese_Progetti.Progetto_Cod = @progCod ");
                    sqlParams.TryAdd("@progCod", progCod);
                }
                return await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                return null;
            }
        }
    }
}
