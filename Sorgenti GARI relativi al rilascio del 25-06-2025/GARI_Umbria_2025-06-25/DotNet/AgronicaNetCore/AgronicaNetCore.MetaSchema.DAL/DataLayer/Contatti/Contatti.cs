using System.Data;
using System.Text;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.Resources;
using Microsoft.Extensions.Localization;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Contatti;

public class Contatti : BaseDALMetaschema, IContatti
{
    public Contatti(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
    {
    }

    public async Task<DataTable> LeggiAsync(string visibilityFilter, AgronicaCoreParametri objParams)
    {
        var stbQuery = new StringBuilder();
        var sqlParams = new Dictionary<string, object>();

        const int rapprLegale = 5000;

        stbQuery.AppendLine(
            " SELECT I.Rag_Soc AS Impresa, C.Piva, C.Sa_Cod, C.Cod_Contatto, C.Codice_Fiscale, C.Id_CF, C.Rag_Soc, C.Convenevoli, ");
        stbQuery.AppendLine(" C.Nome, C.Cognome, C.Data_Nascita, C.Sesso, C.Cod_Contatto_Referente, ");
        stbQuery.AppendLine(" RU.Sa_Cod AS Sa_Cod_2, RU.Cod_RisUm, RU.Validita_Inizio, RU.Validita_Fine, ");
        stbQuery.AppendLine(" RU.Settore_Des, RU.Attivita_Des, RU.Corrispettivo_Mensile, RU.Corrispettivo_Orario, ");
        stbQuery.AppendLine(" RU.Occasionale, RU.Ore_Settimanali, RU.Giorni_Ferie, RU.Ferie_Godute, ");
        stbQuery.AppendLine(
            " RU.Giorni_Malattia, RU.Patentino, RU.Data_Rilascio_Patentino, RU.Data_Scadenza_Patentino, ");
        stbQuery.AppendLine(
            " RC.Piva AS Piva_SuperUser, RC.Sa_Cod AS Sa_Cod_3, RC.Cod_Rapporto, RC.Rapporto_Des, RC.Cliente,  ");
        stbQuery.AppendLine(" RC.Fornitore, RC.Dipendente, RC.Terzista, RC.Legale, ");
        stbQuery.AppendLine("ISNULL ((SELECT TOP 1   val_cod  ");
        stbQuery.AppendLine(" FROM Contatti_Codici CC ");
        stbQuery.AppendLine(
            " WHERE C.PIVA = CC.PIVA AND C.Cod_Contatto = CC.Cod_Contatto AND CC.id_cod = @IdCod), '0') AS Modifica ");
        stbQuery.AppendLine(" FROM Contatti C");
        stbQuery.AppendLine(" INNER JOIN Risorse_Umane RU ON C.Cod_Contatto = RU.Cod_Contatto AND C.Piva = RU.Piva ");
        stbQuery.AppendLine(" INNER JOIN Rapporti_Contabili RC ON RU.Cod_Rapporto = RC.Cod_Rapporto ");
        stbQuery.AppendLine(
            " INNER JOIN UtentiXImprese UI ON RC.Piva = UI.[USER] AND RU.Piva = UI.PIVA ");
        stbQuery.AppendLine(" INNER JOIN Imprese I ON I.Piva = C.Piva ");
        stbQuery.AppendLine(" WHERE (RC.Piva = @piva) ");
        stbQuery.AppendLine(" AND ( RU.Validita_Inizio <= @dtFine) ");
        stbQuery.AppendLine(" AND ( RU.Validita_Fine >= @dtInizio) ");
        stbQuery.AppendLine(" AND  RU.Cod_RisUm_Origine = 0 ");
        stbQuery.AppendLine(" AND  RU.Cod_RisUm_Origine = 0 ");
        stbQuery.AppendLine(" AND ( (RC.Cod_Rapporto IN (-1,-4,-5,-6,-12)) or RC.Dipendente=1 or RC.Terzista=1 ) ");
        stbQuery.AppendLine(visibilityFilter);
        stbQuery.AppendLine(" ORDER BY C.Sa_Cod desc, C.Rag_Soc, cognome, nome ASC");

        sqlParams.Add("@dtFine", objParams.FinestraTemporaleFine);
        sqlParams.Add("@dtInizio", objParams.FinestraTemporaleInizio);
        sqlParams.Add("@piva", objParams.PivaSuperUser.Trim());
        sqlParams.Add("@IdCod", rapprLegale);

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

    public async Task<DataTable> LeggiFromCodRisUmAsync(string codRisUm, AgronicaCoreParametri objParams)
    {
        var sqlParams = new Dictionary<string, object>();
        var query = @"SELECT rag_soc, nome, cognome, rapporto_des  FROM Contatti (NOLOCK)
                  INNER JOIN Risorse_Umane (NOLOCK) ON Contatti.Cod_Contatto = Risorse_Umane.Cod_Contatto AND Contatti.Piva = Risorse_Umane.Piva 
                  INNER JOIN  Rapporti_Contabili (NOLOCK) ON Risorse_Umane.Cod_Rapporto = Rapporti_Contabili.Cod_Rapporto 
                  WHERE Risorse_Umane.Cod_risum = @codRisUm AND Contatti.PIVA= @piva ";

        switch (objParams.FlagVisibilita)
        {
            case AgronicaCoreParametri.enumVisibilita.visibilita_SoloNonInviati:
                query += " AND   Contatti.Inviato >= 0 ";
                break;
            case AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati:
                query += " AND   Contatti.Inviato =-1 ";
                break;
            case AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        sqlParams.Add("@codRisUm", codRisUm);
        sqlParams.Add("@piva", objParams.PivaSuperUser);

        try
        {
            return await GetDataProvider(objParams).ExecuteReadAsync(query, sqlParams);
        }
        catch (Exception ex)
        {
            LogError(ex.Message, objParams, ex);
            throw;
        }
    }
}