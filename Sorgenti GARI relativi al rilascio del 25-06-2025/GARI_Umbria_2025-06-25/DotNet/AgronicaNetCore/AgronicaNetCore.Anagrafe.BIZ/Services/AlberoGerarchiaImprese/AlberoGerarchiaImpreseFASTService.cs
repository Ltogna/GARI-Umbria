using AgronicaNetCore.Anagrafe.BIZ.Resources;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.GerarchiaImprese;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiDettagli;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.AlberoGerarchiaImprese
{
    public class AlberoGerarchiaImpreseFASTService : BaseServiceAnagrafeBIZ, IAlberoGerarchiaImpreseFASTService
    {
        private IUtentiDettagli _utentiDettagli;
        private IUtentiProfili _utentiProfili;
        private IGerarchiaImprese _gerarchiaImprese;

        public AlberoGerarchiaImpreseFASTService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _utentiDettagli = _serviceProvider.GetRequiredService<IUtentiDettagli>();
            _utentiProfili = _serviceProvider.GetRequiredService<IUtentiProfili>();
            _gerarchiaImprese = _serviceProvider.GetRequiredService<IGerarchiaImprese>();
        }

        public async Task<KendoHierarchicalDataSource> GetNodesGearchiaObjAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtenti)
        {

            var livelloImpresa = new List<AjaxTreeNodeFASTJsonObject>();
            var utentiDettagliDT = await _utentiDettagli.LeggiAsync(5, objParametriUtenti);

            string testo;
            int tipoUtente;

            if (utentiDettagliDT.Rows.Count > 0)
            {
                var primaRiga = utentiDettagliDT.Rows[0];
                tipoUtente = (int)primaRiga["Flag_Azienda_Persona"];
                if (tipoUtente == 1)
                    testo = (primaRiga["Rag_Soc"] as string)!;
                else
                    testo = string.Format("{0} {1}", primaRiga["Cognome"], primaRiga["Nome"]);
            }
            else
            {
                testo = objParametriServer.UtenteUsername;
            }

            var text = string.Format("{0}{1}", CostantiPersonalizzate.AgroPrefix_Utente, testo);
            AjaxTreeNodeFASTJsonObject radice = new AjaxTreeNodeFASTJsonObject(text, TipoNodo.Utente, children: livelloImpresa);

            radice.State.Opened = false;

            var utentiProfiloDt = await _utentiProfili.ReadAsync(objParametriUtenti, 5);

            bool filtroVisibilitaUtente = true;

            if (utentiProfiloDt!.Rows.Count > 0)
            {
                var sqlPermessi = utentiProfiloDt.Rows[0]["Descrizione_2"] as string;
                if (string.IsNullOrEmpty(sqlPermessi))
                    filtroVisibilitaUtente = false;
            }

            DataTable gerarchieDt;

            try
            {
                gerarchieDt = await _gerarchiaImprese.LeggixGerarchiaAlberoImprese_VisibilitaAsync(filtroVisibilitaUtente, objParametriServer);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

            var results = new List<AjaxTreeNodeFASTJsonObject>();
            var pivaPadre = string.Empty;

            RestituisciNodoImpresa(results, gerarchieDt, pivaPadre);
            radice.Children!.AddRange(results);

            var listaRadice = new List<AjaxTreeNodeFASTJsonObject>();
            listaRadice.Add(radice);

            var kendoHierarchical = new KendoHierarchicalDataSource();
            AjaxTreeNodeJsonObjectConverter.AjaxTreeNodeJsonObject_KendoHierarchical(listaRadice, kendoHierarchical);

            return kendoHierarchical;
        }


        private void RestituisciNodoImpresa(List<AjaxTreeNodeFASTJsonObject> results, DataTable gerarchieDt, string pivaPadre)
        {
            int tipoImpresaGerarchia;
            var xPiva = string.Empty;
            var xRag_Soc = string.Empty;
            var xCUAA = string.Empty;
            int? certificatiBloccati = null;

            var DR = gerarchieDt.Select(string.Format("Padre = '{0}'", pivaPadre));
            if (pivaPadre == string.Empty && !DR.Any())
            {
                var padriDt = gerarchieDt.DefaultView.ToTable(true, "Padre");
                foreach (DataRow row in padriDt.Rows)
                {
                    var padre = (row[0] as string)!;
                    var eFiglio = gerarchieDt.Select(string.Format("Piva = '{0}'", padre));

                    if (eFiglio.Length == 0)
                        RestituisciNodoImpresa(results, gerarchieDt, padre);
                }
                return;
            }

            foreach (var row in DR)
            {
                bool foglia;

                if ((short)row["Foglia"] == 1)
                    foglia = true;
                else
                {
                    foglia = false;
                    var piva = row["Piva"] as string;
                    if (!string.IsNullOrEmpty(piva))
                    {
                        var padreDR = gerarchieDt.Select(string.Format("Padre = '{0}'", piva));
                        if (padreDR.Length == 0)
                        {
                            continue;
                        }
                    }
                }

                xPiva = (row["Piva"] as string)!;
                xRag_Soc = (row["Rag_Soc"] as string)!;
                xCUAA = (row["CUAA"] as string)!;

                if (row["Blk_Flag"] is not null)
                    certificatiBloccati = (int)row["Blk_Flag"];

                tipoImpresaGerarchia = (int)row["TipoImpresaGerarchia"];

                if (certificatiBloccati == -1)
                    xRag_Soc = "--SOSPESA--" + xRag_Soc;

                var tipoImpresa = GetTipoImpresa(tipoImpresaGerarchia);

                var livello2 = new List<AjaxTreeNodeFASTJsonObject>();
                if (foglia)
                    livello2 = null;
                else
                    RestituisciNodoImpresa(livello2, gerarchieDt, xPiva);

                AjaxTreeNodeFASTJsonObject impresa;
                if (livello2 is null)
                    impresa = new AjaxTreeNodeFASTJsonObject(xRag_Soc, tipoImpresa, xPiva, pivaPadre, xCUAA);
                else
                    impresa = new AjaxTreeNodeFASTJsonObject(xRag_Soc, tipoImpresa, xPiva, pivaPadre, xCUAA, livello2);

                if (pivaPadre == string.Empty)
                    impresa.State.Opened = true;
                else
                    impresa.State.Opened = false;

                impresa.State.Selected = false;
                results.Add(impresa);
            }
        }

        private TipoNodo GetTipoImpresa(int tipoImpresaGerarchia)
        {
            if (tipoImpresaGerarchia == 1)
                return TipoNodo.Impresa;
            else if (tipoImpresaGerarchia == 2)
                return TipoNodo.x_Cooperativa;
            else if (tipoImpresaGerarchia == 3)
                return TipoNodo.x_Consorzio;
            else if (tipoImpresaGerarchia == 4)
                return TipoNodo.x_OP;
            else
                return TipoNodo.Utente;
        }


    }

    public enum TipoNodo
    {
        Indefinito = 0,
        Utente = 1,
        Impresa = 2,
        x_Cooperativa = 38,
        x_Consorzio = 39,
        x_OP = 40,
    }
}
