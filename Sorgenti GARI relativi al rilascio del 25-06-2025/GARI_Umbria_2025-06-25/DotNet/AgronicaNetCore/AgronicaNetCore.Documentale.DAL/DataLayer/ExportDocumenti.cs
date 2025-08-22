
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Documentale.DAL.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Numerics;
using System.Text;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;
using static AgronicaNetCore.Base.Constants.CostantiPersonalizzate;

namespace AgronicaNetCore.Documentale.DAL.DataLayer
{
    public class ExportDocumenti : BaseDALExportDocumenti, IExportDocumenti
    {
        public ExportDocumenti(IServiceProvider serviceProvider, IStringLocalizer<Messages> localizer) : base(serviceProvider, localizer)
        {
        }

        public async Task<DataTable> LeggiDocumentiExportAsync(string CUAA, int Tipologia_Cod, DateTime DataRiferimento, AgronicaCoreParametri objParametriServer)
        {

            Dictionary<string, object> parSql = new();
            var stbQuery = new StringBuilder();

            stbQuery.AppendLine(" SELECT ");
            stbQuery.AppendLine("     Alert_Elenco.ID_Elenco ");
            stbQuery.AppendLine("   , Alert_Tipologia.ID_Tipologia ");
            stbQuery.AppendLine("   , CASE WHEN Alert_Elenco.Data_Scadenza IS NULL THEN CAST('2100-12-31' AS Date) ELSE Alert_Elenco.Data_Scadenza END AS Data_Scadenza");
            stbQuery.AppendLine("   , Allegati_Documenti.Allegati_Documenti_NomeFile ");
            stbQuery.AppendLine("   , Alert_Elenco.Descrizione_Scadenza AS Descrizione ");
            stbQuery.AppendLine("   , Allegati_Documenti.File_Allegato_DB ");
            stbQuery.AppendLine("   , ISNULL(CategorieDocumenti.Sottocartella, '') AS Sottocartella ");
            stbQuery.AppendLine("   , 0 AS Cancellato ");
            stbQuery.AppendLine(" FROM Alert_Elenco ");
            stbQuery.AppendLine(" JOIN Alert_Entita ON Alert_Elenco.ID_Alert_Entita = Alert_Entita.Id_Alert_Entita ");
            stbQuery.AppendLine(" JOIN Allegati_Documenti ON ");
            stbQuery.AppendLine("     Alert_Entita.Piva = Allegati_Documenti.Allegati_Documenti_Piva ");
            stbQuery.AppendLine(" AND Allegati_Documenti.Allegati_Documenti_Cod = Alert_Entita.Allegati_Documenti_Cod ");
            stbQuery.AppendLine(" JOIN Imprese_Codici ON ");
            stbQuery.AppendLine("     Allegati_Documenti.Allegati_Documenti_Piva = Imprese_Codici.Piva ");
            stbQuery.AppendLine($" AND id_cod = {((int)Enum_CodiciAnagrafe.CodiceCUAA).ToString()} AND val_cod = @CUAA ");
            stbQuery.AppendLine(" INNER JOIN Alert_Tipologia ON ");
            stbQuery.AppendLine("     Alert_Tipologia.ID_Tipologia = Alert_Elenco.ID_Tipologia ");
            stbQuery.AppendLine(" LEFT JOIN CategorieDocumenti ON ");
            stbQuery.AppendLine("     Alert_Tipologia.Cat_Cod = CategorieDocumenti.Cat_Cod ");

            stbQuery.AppendLine(" WHERE 1 = 1 ");

            if (Tipologia_Cod != 0)
            {
                stbQuery.AppendLine(" AND Alert_Elenco.ID_Tipologia = @Tipologia_Cod");
            }

            if (DataRiferimento != DateTime.Parse(AGRODATAINIZIO))
            {
                stbQuery.AppendLine(" AND Alert_Elenco.Data_Modifica >= @DataRiferimento ");
            }

            if (1 == 2)
            {
                //TO DO 
                // Sulla Alert_Log bisogna aggiungere le colonne id_elenco, piva, id_tipologia per poter identificare correttamente i documenti cancellati
                //Sulla colonna piva bisognerà aggiungere la join con il CUAA

                stbQuery.AppendLine("");

                stbQuery.AppendLine("UNION ");

                stbQuery.AppendLine("");

                stbQuery.AppendLine(" SELECT ");
                stbQuery.AppendLine("     0 AS ID_Elenco ");
                stbQuery.AppendLine("   , 0 AS ID_Tipologia ");
                stbQuery.AppendLine("   , NULL AS Data_Scadenza ");
                stbQuery.AppendLine("   , '' AS Allegati_Documenti_NomeFile ");
                stbQuery.AppendLine("   , '' AS Descrizione ");
                stbQuery.AppendLine("   , NULL AS File_Allegato_DB ");
                stbQuery.AppendLine("   , '' AS Sottocartella ");
                stbQuery.AppendLine("   , 1 AS Cancellato ");
                stbQuery.AppendLine(" FROM Alert_Log ");
                stbQuery.AppendLine(" JOIN Imprese_Codici ON ");
                stbQuery.AppendLine("     Allegati_Documenti.Allegati_Documenti_Piva = Imprese_Codici.Piva ");
                stbQuery.AppendLine($" AND id_cod = {((int)Enum_CodiciAnagrafe.CodiceCUAA).ToString()} AND val_cod = @CUAA ");

                stbQuery.AppendLine("WHERE 1 = 1 ");

                if (Tipologia_Cod != 0)
                {
                    stbQuery.AppendLine(" AND Alert_Log.ID_Tipologia = @Tipologia_Cod");
                }

                if (DataRiferimento != DateTime.Parse(AGRODATAINIZIO))
                {
                    stbQuery.AppendLine(" AND Data_Creazione >= @DataRiferimento ");
                }

            }

            if (Tipologia_Cod != 0)
            {
                parSql.Add("@Tipologia_Cod", Tipologia_Cod);
            }

            if (DataRiferimento != DateTime.Parse(AGRODATAINIZIO))
            {
                parSql.Add("@DataRiferimento", DataRiferimento);
            }

            parSql.Add("@CUAA", CUAA);


            return await GetDataProvider(objParametriServer).ExecuteReadAsync(stbQuery.ToString(), parSql);

        }
    }
}
