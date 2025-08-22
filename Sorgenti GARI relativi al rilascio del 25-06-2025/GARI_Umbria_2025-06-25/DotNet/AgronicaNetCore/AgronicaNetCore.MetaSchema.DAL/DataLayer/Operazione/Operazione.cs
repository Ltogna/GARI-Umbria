using System.Data;
using System.Text;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.Resources;
using Microsoft.Extensions.Localization;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Operazione;

public class Operazione : BaseDALMetaschema, IOperazione
{
    public Operazione(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
    {
    }

    public async Task<string?> LavorazioneDesFromLavorazioneCodAsync(int lavCod, AgronicaCoreParametri objParams)
    {
        var sqlParams = new Dictionary<string, object>();
        const string sqlQuery = @"SELECT coalesce(LL.lav_des, O.lav_des) as lav_des 
                                  FROM  Operazioni O
                                    LEFT JOIN Operazioni_XLingue LL 
                                      on O.Lav_Cod = LL.Lav_COD AND LL.Lingua_cod = @linguaCod 
                                  WHERE O.Validita_inizio <= @dtInizio 
                                    AND O.Validita_Fine >= @dtFine 
                                    AND O.Lav_Cod = @lavCod";

        sqlParams.TryAdd("@linguaCod", objParams.Lingua_Cod);
        sqlParams.TryAdd("@dtInizio", objParams.FinestraTemporaleInizio);
        sqlParams.TryAdd("@dtFine", objParams.FinestraTemporaleFine);
        sqlParams.TryAdd("@lavCod", lavCod);

        try
        {
            var datatable = await GetDataProvider(objParams).ExecuteReadAsync(sqlQuery, sqlParams);
            return datatable.Rows.Count > 0 ? datatable.Rows[0][0].ToString() : null;
        }
        catch
            (Exception ex)
        {
            LogError(ex.Message, objParams, ex);
            throw;
        }
    }

    public async Task<DataTable> LeggiAsync(int lavCod, string tipo, string xOrderBy, AgronicaCoreParametri objParams)
    {
        var strSql = new StringBuilder();
        var sqlParams = new Dictionary<string, object>();

        strSql.AppendLine(
            " SELECT  Operazioni.LAV_COD, coalesce ( operazioni_XLingue.lav_des, Operazioni.LAV_DES) as lav_des, Operazioni.P, Operazioni.Validita_Inizio, Operazioni.Validita_Fine, Operazioni.GRU_OP,GruppoOperazioni.GRU_COD,  ");
        strSql.AppendLine("         GruppoOperazioni.GRU_DES, GruppoOperazioni.Tipo, GruppoOperazioni.ATT_COD  ");
        strSql.AppendLine(" FROM    Operazioni  ");
        strSql.AppendLine(" INNER JOIN GruppoOperazioni ON Operazioni.GRU_OP = GruppoOperazioni.GRU_COD ");
        strSql.AppendLine(
            " LEFT JOIN operazioni_XLingue ON Operazioni.LAV_COD = operazioni_XLingue.LAV_COD AND operazioni_XLingue.Lingua_COD = @linguaCod ");
        strSql.AppendLine(" WHERE   Operazioni.Validita_Inizio <= @dtInizio ");
        strSql.AppendLine(" AND     Operazioni.Validita_Fine >= @dtFine ");

        sqlParams.Add("@linguaCod", objParams.Lingua_Cod);
        sqlParams.TryAdd("@dtInizio", objParams.FinestraTemporaleInizio);
        sqlParams.TryAdd("@dtFine", objParams.FinestraTemporaleFine);

        if (lavCod != 0)
        {
            strSql.AppendLine(" AND Operazioni.Lav_Cod = @lavCod ");
            sqlParams.TryAdd("@lavCod", lavCod);
        }

        if (tipo != "")
        {
            strSql.AppendLine(" AND GruppoOperazioni.Tipo = @tipo ");
            sqlParams.TryAdd("@tipo", tipo);
        }

        switch (objParams.FlagVisibilita)
        {
            case AgronicaCoreParametri.enumVisibilita.visibilita_SoloNonInviati:
                strSql.AppendLine(" AND   Operazioni.Inviato = 0 ");
                break;
            case AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati:
                strSql.AppendLine(" AND   Operazioni.Inviato =-1 ");
                break;
            case AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti:
                break;
            default:
                throw new Exception("Parametro non corretto nella query (FlagVisibilita)");
        }

        if (xOrderBy != "")
        {
            strSql.AppendLine(" ORDER BY " + xOrderBy);
        }
        else
        {
            // 'Nota:
            //     Questo ordinamento è importante per la gestione del campo.
            //     'Viene letto l'impianto più RECENTE dell'appezzamento associato al campo
            strSql.AppendLine(
                " ORDER BY GruppoOperazioni.GRU_DES, coalesce(operazioni_XLingue.lav_des, Operazioni.LAV_DES) ");
        }

        try
        {
            return await GetDataProvider(objParams).ExecuteReadAsync(strSql.ToString(), sqlParams);
        }
        catch
            (Exception ex)
        {
            LogError(ex.Message, objParams, ex);
            throw;
        }
    }
}