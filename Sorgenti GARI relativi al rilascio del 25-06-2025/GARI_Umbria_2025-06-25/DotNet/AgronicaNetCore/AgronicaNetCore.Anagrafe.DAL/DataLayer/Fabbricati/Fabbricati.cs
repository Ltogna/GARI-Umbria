using System.Text;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Anagrafe.DAL.Base;
using System.Data;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Fabbricati
{
    public class Fabbricati : DAL_Base, IFabbricati
    {
        public Fabbricati(IServiceProvider provider, bool securityBypass = false) : base(provider, securityBypass)
        {
        }

        public async Task<int> ReadFabbCodAsync(string Piva, int Sa_Cod, int Tipo_Fabb, AgronicaCoreParametri objP)
        {
            if(string.IsNullOrEmpty(Piva))
                throw new ArgumentException("Piva non può essere vuota.");
            if(Sa_Cod == 0)
                throw new ArgumentException("Sa_Cod non può essere vuoto.");

            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            stbQuery.AppendLine("SELECT * ")
                .AppendLine("FROM Fabbricati  ")
                .AppendLine("WHERE PIVA = @piva AND Sa_Cod = @saCod ");

            sqlParams.TryAdd("@piva", Piva);
            sqlParams.TryAdd("@saCod", Sa_Cod);

            if (Tipo_Fabb != 0)
            {
                sqlParams.TryAdd("@tipo", Tipo_Fabb);
                stbQuery.AppendLine("    AND Tipo_Fabbricato_Cod = @tipo ");
            }
            sqlParams.TryAdd("@inizio", objP.FinestraTemporaleInizio);
            sqlParams.TryAdd("@fine", objP.FinestraTemporaleFine);
            stbQuery.AppendLine("    AND Validita_Inizio <= @inizio ")
                .AppendLine("    AND Validita_Fine >= @fine ")
                .AppendLine("ORDER BY Fabbricato_Cod DESC ");

            try
            {
                var dtFabb = await GetDataProvider(objP).ExecuteReadAsync(stbQuery.ToString(), sqlParams);

                if (dtFabb.Rows.Count == 0)
                    return 0;

                int fabbCod = (int)dtFabb.Rows[0]["Fabbricato_Cod"];
                return fabbCod;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<DataTable> ReadAsync(string Piva, int Sa_Cod, int Fabbricato_Cod, AgronicaCoreParametri objP)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            stbQuery.AppendLine("SELECT * ")
                .AppendLine("FROM Fabbricati  ")
                .AppendLine("WHERE 1 = 1 ");

            if (Piva != "")
            {
                stbQuery.AppendLine(" AND PIVA = @piva ");
                sqlParams.TryAdd("@piva", Piva);
            }

            if (Sa_Cod != 0)
            {
                stbQuery.AppendLine(" AND Sa_Cod = @saCod ");
                sqlParams.TryAdd("@saCod", Sa_Cod);
            }

            if (Fabbricato_Cod != 0)
            {
                sqlParams.TryAdd("@Fabbricato_Cod", Fabbricato_Cod);
                stbQuery.AppendLine("    AND Fabbricato_Cod = @Fabbricato_Cod ");
            }
            sqlParams.TryAdd("@inizio", objP.FinestraTemporaleInizio);
            sqlParams.TryAdd("@fine", objP.FinestraTemporaleFine);
            stbQuery.AppendLine("    AND Validita_Inizio <= @inizio ")
                .AppendLine("    AND Validita_Fine >= @fine ")
                .AppendLine("ORDER BY Fabbricato_Cod DESC ");

            try
            {
                var dtFabb = await GetDataProvider(objP).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
                return dtFabb;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

    }
}
