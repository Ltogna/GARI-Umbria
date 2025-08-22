using AgronicaCoreDTOStd.Identity;
using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaCoreModelsSTD.metaschema.utilizzi;
using AgronicaNetCore.Anagrafe.DAL.Resources;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Impianti
{
    public class Impianti : BaseDALAnagrafe, IImpianti
    {
        public Impianti(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable?> GetPianoColturalePerConfrontoCatastoAsync(string partitaIva, DateTime dataInizio, DateTime dataFine, AgronicaCoreParametri objParametri, bool origine = true, bool ShowCatasto = false, bool showVarieta = false)
        {
            var strSql = "";
            var parSql = new Dictionary<string, object>();
            DataTable? result = null;

            try
            {
                if (string.IsNullOrEmpty(partitaIva))
                    throw new Exception("Specificare la partita iva");


                strSql += "select \n";
                if (ShowCatasto)
                    strSql += "     Prov, Prov_Des, Com, Com_Des, Sezione, Foglio, Numero, Subalterno, \n";
                strSql += "     Veg_cod, Veg_Des,\n";
                if (showVarieta)
                    strSql += "     Cul_Cod, Cul_Des,\n";
                strSql += "     id_Cod, \n";
                if (origine)
                {
                    strSql += "    sum(area) as Superficie_Ori \n";
                }
                else
                {
                    strSql += "    sum(area) as Superficie_Act \n";
                }
                strSql += "from( \n";
                strSql += "     select \n";
                strSql += "     d.Prov as Prov, \n";
                strSql += "     coalesce(lp.COMUNI_PROV, '') as Prov_Des, \n";
                strSql += "     d.Com as Com, \n";
                strSql += "     coalesce(lp.LOCALITA, '') as Com_Des, \n";
                strSql += "     d.Sezione as Sezione, \n";
                strSql += "     d.Foglio as Foglio, \n";
                strSql += "     d.Numero as Numero, \n";
                strSql += "     d.Subalterno as Subalterno, \n";
                strSql += "     coalesce(f.Veg_Cod,0) as Veg_Cod, \n";
                strSql += "     COALESCE(sv.Veg_Des, ca.descrizione) as Veg_Des, \n";
                strSql += "     coalesce(f.Cul_Cod,0) as Cul_Cod, \n";
                strSql += "     coalesce(f.Cul_Des,'') as Cul_Des, \n";
                strSql += "     coalesce(ric.id_cod,0) as id_Cod, \n";
                strSql += "     coalesce(d.area,b.sup_imp) as area \n";
                strSql += "\n";
                strSql += "     from imprese_progetti a \n";
                strSql += "\n";
                strSql += "     inner join Reg_Impianti b \n";
                strSql += "     on(a.piva= b.piva and a.SA_COD= b.SA_COD and a.APPEZZA= b.APPEZZA and a.id_Reg= b.id_reg) \n";
                strSql += "\n";
                strSql += "     inner join Appezzamento c \n";
                strSql += "     on(b.piva= c.piva and b.SA_COD= c.SA_COD and b.APPEZZA= c.APPEZZA) \n";
                strSql += "\n";
                strSql += "     left join Reg_Impianti_Codici ric \n";
                strSql += "     on(b.piva= ric.piva and b.SA_COD= ric.SA_COD and b.APPEZZA= ric.APPEZZA and b.id_Reg= ric.id_reg and ric.id_cod between 3000 and 3999) \n";
                strSql += "\n";
                strSql += "     left join AppezzamentiXParticelle d \n";
                strSql += "     on(c.piva= d.piva and c.SA_COD= d.SA_COD and c.APPEZZA= d.APPEZZA) \n";
                strSql += "\n";
                strSql += "     left join Cultivar f \n";
                strSql += "     on(b.cul_cod = f.cul_cod) \n";
                strSql += "\n";
                strSql += "     left join SpecieVegetali sv \n";
                strSql += "    on(sv.Veg_Cod = f.Veg_Cod) \n";
                strSql += "\n";
                strSql += "     left join codici_anagrafe ca ON (ric.id_cod = ca.codice) \n";
                strSql += "\n";
                strSql += "     left join ISTAT lp on(lp.PROV = d.Prov AND lp.COM = d.Com) \n";
                strSql += "\n";
                strSql += "     where a.piva=@piva \n";
                if (dataInizio > new DateTime(1900, 1, 1, 0, 0, 0))
                    strSql += "     and a.Validita_Inizio>=@dtStart \n";
                if (dataFine < new DateTime(2100, 12, 31, 0, 0, 0))
                    strSql += "     and a.Validita_Fine<=@dtEnd \n";
                if (ShowCatasto)
                    strSql += "     and d.Prov is not null \n";
                strSql += ") src \n";
                strSql += "group by \n";
                if (ShowCatasto)
                    strSql += " Prov, Prov_Des, Com, Com_Des, Sezione, Foglio, Numero, Subalterno, \n";
                strSql += " Veg_Cod, Veg_Des, \n";
                if (showVarieta)
                    strSql += " Cul_Cod, Cul_Des,\n";
                strSql += " id_Cod \n";

                parSql.Add("@piva", partitaIva);
                if (dataInizio > new DateTime(1900, 1, 1, 0, 0, 0))
                    parSql.Add("@dtStart", dataInizio);
                if (dataFine < new DateTime(2100, 12, 31, 0, 0, 0))
                    parSql.Add("@dtEnd", dataFine);

                result = await GetDataProvider(objParametri).ExecuteReadAsync(strSql, parSql);

                if (ShowCatasto && showVarieta)
                {
                    var pk = new List<DataColumn>();
                    foreach (DataColumn col in result.Columns)
                    {
                        switch (col.ColumnName)
                        {
                            case "Prov":
                            case "Com":
                            case "Sezione":
                            case "Foglio":
                            case "Numero":
                            case "Subalterno":
                            case "Veg_cod":
                            case "Cul_Cod":
                            case "id_Cod":
                                pk.Add(col);
                                break;

                        }
                    }
                    result.PrimaryKey = pk.ToArray();
                }
                else
                {
                    if (ShowCatasto)
                    {
                        var pk = new List<DataColumn>();
                        foreach (DataColumn col in result.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "Prov":
                                case "Com":
                                case "Sezione":
                                case "Foglio":
                                case "Numero":
                                case "Subalterno":
                                case "Veg_cod":
                                case "id_Cod":
                                    pk.Add(col);
                                    break;

                            }
                        }
                        result.PrimaryKey = pk.ToArray();
                    }
                    else
                    {
                        if (showVarieta)
                        {
                            var pk = new List<DataColumn>();
                            foreach (DataColumn col in result.Columns)
                            {
                                switch (col.ColumnName)
                                {
                                    case "Cul_Cod":
                                    case "Veg_cod":
                                    case "id_Cod":
                                        pk.Add(col);
                                        break;

                                }
                            }
                            result.PrimaryKey = pk.ToArray();
                        }
                        else
                        {
                            var pk = new List<DataColumn>();
                            foreach (DataColumn col in result.Columns)
                            {
                                switch (col.ColumnName)
                                {
                                    case "Veg_cod":
                                    case "id_Cod":
                                        pk.Add(col);
                                        break;

                                }
                            }
                            result.PrimaryKey = pk.ToArray();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                result = null;
            }
            return result;
        }

        public async Task<DataSet> GetExistsContributiACAAsync(AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataSet ds = new();

            try
            {
                stbQuery.AppendLine(" SELECT COUNT(0) esistenza FROM Imprese_ProgettiXContributi WHERE ContributoTipo = 1 ");
                stbQuery.AppendLine("");
                stbQuery.AppendLine(" SELECT ContributoCod, ContributoDes ");
                stbQuery.AppendLine(" FROM Contributi ");
                stbQuery.AppendLine(" WHERE TIPO = 1 --Fisso ACA ");

                ds = await GetDataProvider(objParametri).ExecuteMultipleReadAsync(stbQuery.ToString(), "contributi"); 
                             

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                ds = null;
            }
            return ds;
        }

        public async Task<DataTable?> LeggiConCodiciAsync(
            AgronicaCoreParametri objParametri,
            string piva = "", int saCod = 0, int appezza = 0, int idReg = 0,
            int culCod = 0, int progCod = -1, int idCod = 0, string valCod = ""
        )
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("SELECT");
                stbQuery.AppendLine("  Reg_Impianti_Codici.PIVA, Reg_Impianti_Codici.sa_cod,");
                stbQuery.AppendLine("  Reg_Impianti_Codici.appezza, Reg_Impianti_Codici.Id_Reg,");
                stbQuery.AppendLine("  Reg_Impianti.Sup_Imp, Reg_Impianti.cul_cod,");
                stbQuery.AppendLine("  Reg_Impianti_Codici.id_cod, Reg_Impianti_Codici.val_cod,");
                stbQuery.AppendLine("  Reg_Impianti.Validita_Inizio, Reg_Impianti.Validita_Fine");
                stbQuery.AppendLine("FROM Reg_Impianti_Codici");
                stbQuery.AppendLine("INNER JOIN Reg_Impianti ON");
                stbQuery.AppendLine("  Reg_Impianti_Codici.PIVA = Reg_impianti.PIVA");
                stbQuery.AppendLine("  AND Reg_Impianti_Codici.sa_cod = Reg_Impianti.sa_cod");
                stbQuery.AppendLine("  AND Reg_Impianti_Codici.appezza = Reg_Impianti.appezza");
                stbQuery.AppendLine("  AND Reg_Impianti_Codici.Id_Reg = Reg_Impianti.Id_Reg");
                stbQuery.AppendLine("WHERE 1 = 1");

                if (piva != "")
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.piva = @piva ");
                    sqlParams.TryAdd("@piva", piva);
                }
                if (saCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.sa_cod = @saCod ");
                    sqlParams.TryAdd("@saCod", saCod);
                }
                if (appezza != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.appezza = @appezza ");
                    sqlParams.TryAdd("@appezza", appezza);
                }
                if (idReg != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.Id_Reg = @idReg ");
                    sqlParams.TryAdd("@idReg", idReg);
                }
                if (culCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.cul_cod = @culCod ");
                    sqlParams.TryAdd("@culCod", culCod);
                }
                if (culCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.cul_cod = @culCod ");
                    sqlParams.TryAdd("@culCod", culCod);
                }
                if (progCod != -1)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.Progetto_Cod = @progCod ");
                    sqlParams.TryAdd("@progCod", progCod);
                }
                if (idCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.Id_Cod = @idCod ");
                    sqlParams.TryAdd("@idCod", idCod);
                }
                if (valCod != "")
                {
                    stbQuery.AppendLine(" AND Reg_Impianti_Codici.Val_Cod = @valCod ");
                    sqlParams.TryAdd("@valCod", valCod);
                }

                return await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                return null;
            }
        }

        public async Task<DataTable?> LeggiInfoVarietaAsync(string piva, int saCod, int appezza, int idReg, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("SELECT * ");
                stbQuery.AppendLine("FROM Reg_Impianti");
                stbQuery.AppendLine("LEFT JOIN Cultivar ON Reg_Impianti.CUL_COD = Cultivar.Cul_Cod");
                stbQuery.AppendLine("LEFT JOIN SpecieVegetali ON Cultivar.Veg_Cod = SpecieVegetali.Veg_Cod");
                stbQuery.AppendLine("LEFT JOIN GruppoVegetale ON SpecieVegetali.Gru_Cod = GruppoVegetale.Gru_Cod");
                stbQuery.AppendLine("WHERE 1 = 1");

                if (piva != "")
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.piva = @piva ");
                    sqlParams.TryAdd("@piva", piva);
                }
                if (saCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.sa_cod = @saCod ");
                    sqlParams.TryAdd("@saCod", saCod);
                }
                if (appezza != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.appezza = @appezza ");
                    sqlParams.TryAdd("@appezza", appezza);
                }
                if (idReg != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.Id_Reg = @idReg ");
                    sqlParams.TryAdd("@idReg", idReg);
                }

                return await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                return null;
            }
        }

        public async Task<DataTable?> LeggiAsync(AgronicaCoreParametri objParametri, string piva = "", int saCod = 0, int appezza = 0, int idReg = 0, int progCod = -1)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            try
            {
                stbQuery.AppendLine("SELECT * ");
                stbQuery.AppendLine("FROM Reg_Impianti");
                stbQuery.AppendLine("LEFT JOIN Imprese_Progetti ON reg_impianti.PIVA = Imprese_Progetti.Piva");
                stbQuery.AppendLine("  AND reg_impianti.SA_COD = Imprese_Progetti.Sa_Cod");
                stbQuery.AppendLine("  AND reg_impianti.APPEZZA = Imprese_Progetti.Appezza");
                stbQuery.AppendLine("  AND reg_impianti.ID_REG = Imprese_Progetti.Id_Reg");
                stbQuery.AppendLine("WHERE 1 = 1");

                if (piva != "")
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.piva = @piva ");
                    sqlParams.TryAdd("@piva", piva);
                }
                if (saCod != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.sa_cod = @saCod ");
                    sqlParams.TryAdd("@saCod", saCod);
                }
                if (appezza != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.appezza = @appezza ");
                    sqlParams.TryAdd("@appezza", appezza);
                }
                if (idReg != 0)
                {
                    stbQuery.AppendLine(" AND Reg_Impianti.Id_Reg = @idReg ");
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
