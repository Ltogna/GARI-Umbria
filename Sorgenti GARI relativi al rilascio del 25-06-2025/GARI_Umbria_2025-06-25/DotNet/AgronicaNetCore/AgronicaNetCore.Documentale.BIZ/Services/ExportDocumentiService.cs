using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.DataLayer.Security;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Documentale.BIZ.Resources;
using AgronicaNetCore.Documentale.DAL.DataLayer;
using AgronicaNetCore.Utility.DAL.DataLayer.ObjectUtility;
using InData.DataExchange;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OutData.DataExchange;
using System.Data;

namespace AgronicaNetCore.Documentale.BIZ.Services
{
    public class ExportDocumentiService : BaseServiceExportDocumentiBIZ, IExportDocumentiService

    {
        private readonly ISecurityLayer _securityLayer;
        private readonly ICompressioneDecompressione _compressioneDecompressione;
        private readonly IExportDocumenti _exportDocumenti;

        public ExportDocumentiService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _securityLayer = _serviceProvider.GetRequiredService<ISecurityLayer>();
            _compressioneDecompressione = _serviceProvider.GetRequiredService<ICompressioneDecompressione>();
            _exportDocumenti = _serviceProvider.GetRequiredService<IExportDocumenti>();
        }

        public async Task<string> GetDocumentiExportAsync(ExportDocumenti_In ExportDocumenti_IN, AgronicaCoreParametri objParametriServer)
        {
            JsonSerializerSettings tzh = new() { DateFormatString = "yyyy-MM-ddT00:00:00Z" };
            List<ExportDocumenti_Out> DocumentiExport = new();

            string datiCompressi = "";

            try
            {
                DataTable result = await _exportDocumenti.LeggiDocumentiExportAsync(ExportDocumenti_IN.CUAA, ExportDocumenti_IN.Id_Tipologia, ExportDocumenti_IN.DataOra_Rif_Documenti, objParametriServer);
                result = await LeggiAllegatoDaFileSystemAsync(result, objParametriServer);

                if (result.Rows.Count > 0)
                {

                    foreach (DataRow row in result.Rows)
                    {

                        ExportDocumenti_Out Documento = new();

                        Documento.Id_Documento = (int)row["ID_Elenco"];
                        Documento.Id_Tipologia = (int)row["ID_Tipologia"];

                        bool cancellato = Convert.ToBoolean((int)row["Cancellato"]);
                        Documento.Cancellato = cancellato;

                        if (!cancellato)
                        {
                            Documento.Descrizione = row["Descrizione"].ToString();
                            Documento.Data_Scadenza = DateTime.Parse(row["Data_Scadenza"].ToString()!);
                            Documento.FileName = row["Allegati_Documenti_NomeFile"].ToString();
                            Documento.FileByte = (byte[])row["File_Allegato_DB"];
                        }

                        DocumentiExport.Add(Documento);
                    }
                    datiCompressi = _compressioneDecompressione.CompressioneBase64(1, JsonConvert.SerializeObject(DocumentiExport, tzh));
                }
            }

            catch (Exception ex)
            {
                datiCompressi = "";
                LogError(ex.Message, objParametriServer, ex);
                throw new Exception(ex.Message);
            }

            return datiCompressi;
        }

        public async Task<DataTable> LeggiAllegatoDaFileSystemAsync(DataTable dt, AgronicaCoreParametri objParametri_Server)
        {
            string percorsoAllegatyRepository = await GetGestioneAllegatiRepository(objParametri_Server, objParametri_Server);

            //Aggiungere controllo sull'esistenza del file
            //se non esiste --> response KO con id_elenco problematico
            //rompiamo tutto oppure no? verificare con scatto

            if (dt.Rows.Count > 0)
            {
                bool bFS = true;

                foreach (DataRow row in dt.Rows)
                {

                    if (!Convert.ToBoolean((int)row["Cancellato"]))
                    {

                        // Controllo Tipo Salvataggio
                        if (row["File_Allegato_DB"] != DBNull.Value)
                        {
                            if (row["File_Allegato_DB"].ToString() == "System.Byte[]")
                            {
                                // Salvataggio su DB
                                bFS = false;
                            }
                        }

                        if (bFS)
                        {
                            if (!string.IsNullOrEmpty(row["Sottocartella"].ToString()))
                            {
                                percorsoAllegatyRepository = Path.Combine(percorsoAllegatyRepository,
                                           row["Sottocartella"].ToString()!);
                            }

                            string fileName = Path.Combine(percorsoAllegatyRepository, row["Allegati_Documenti_NomeFile"].ToString()!);

                            // Lettura file su file system
                            byte[] fileByteArray = File.ReadAllBytes(fileName);

                            // Aggiorna la colonna con il contenuto del file
                            row["File_Allegato_DB"] = fileByteArray;

                        }
                    }

                }
            }

            return dt;
        }

        private async Task<string> GetGestioneAllegatiRepository(AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Super_Server)
        {
            return await _securityLayer.LeggiConfigurazioneSitiScalareAsync("GestioneAllegati_Repository", objP_Server, objP_Super_Server);
        }

    }
}
