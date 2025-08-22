using AgronicaCoreAPI.adapters;
using AgronicaCoreAPI.models;
using AgronicaCoreDataProviderSTD;
using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreUtilityStd;
using AgronicaCoreVarieBizSTD;
using AgronicaCoreWebServiceSTD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AgronicaCoreDataProviderSTD.TipiEnumerativi;
using Attivita = AgronicaCoreModelsSTD.attivita.Attivita;
using static AgronicaCoreDataProviderSTD.CostantiPersonalizzate;
using AgronicaCoreModelloSTD;
using InData.Anagrafica;
using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaCoreModelsSTD.exceptions;
using System.Data;
using Fabbricato = AgronicaCoreModelsSTD.anagrafiche.Fabbricato;
using System.Net.Http;
using AgronicaCoreDTOStd.InData.Anagrafica;
using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaCoreModelsSTD.metaschema.utilizzi;
using AgronicaCoreAPI.models.entities;
using AgronicaNetCore.Operazione.BIZ.Services.OperazioneCausale;
using AgronicaNetCore.Base.Utility;
using Microsoft.Extensions.Options;
using AgronicaDataProvider6.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using AgronicaCoreAPI.Resources;

namespace AgronicaCoreAPI.Controllers
{
    public class SincroController : BaseController
    {
        public SincroController(IServiceProvider provider, IConfiguration config, IHttpClientFactory httpClientFactory, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, config, httpClientFactory, securitySettings, localizer)
        {
        }


        [HttpGet]
        [Route("Imprese")]
        public ObjectResult GetImprese(string gerarchia, string tipologie, string ricerca, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<List<ImpresaEntity>> result = new RispostaStandard<List<ImpresaEntity>>();

            try
            {
                var adapter = new ModelloAdapter();
                var ws = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request);
                var filtro = new FiltroAziendeAPP() { gerarchia = gerarchia, tipologie = tipologie, ricerca = ricerca };
                var r = ws.LeggiImpreseAPP(getRequest(filtro));
                if (!r.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, r);

                // result = ws.LeggiImpreseModello(new CoreWS_Imprese(objP_super_server, objP_server, objP_utenti, tipo));
                // if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

                result.RispostaOK = true;
                result.RispostaStringa = adapter.leggiAziende(r.RispostaStringa);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        [Route("DatiApp")]
        public ObjectResult GetDatiApp(string tipo, string piva, string data, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<DatiApp> result = new RispostaStandard<DatiApp>();

            try
            {
                var dati = new DatiApp();
                var ws = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request);
                var r = ws.LeggiDatiApp(new CoreWS_Dati_App(objP_super_server, objP_server, objP_utenti, tipo, piva, data));
                var dt = JsonConvert.DeserializeObject<DataTable>(r.RispostaStringa);

                foreach (DataRow row in dt.Rows)
                {
                    string id = row["ID"].ToString();
                    string d = row["Dati"].ToString();
                    string t = row["Tipo"].ToString();

                    enum_Dati_App type = (enum_Dati_App)Enum.Parse(typeof(enum_Dati_App), t);

                    if (type == enum_Dati_App.Manutenzioni)
                    {
                        var manutenzione = JsonConvert.DeserializeObject<Manutenzione>(d);
                        if (manutenzione != null)
                        {
                            manutenzione.guid = id;
                            dati.manutenzioni.Add(manutenzione);
                        }
                    }
                    else if (type == enum_Dati_App.Movimenti)
                    {
                        var movimento = JsonConvert.DeserializeObject<MovimentoDiMagazzino>(d);
                        if (movimento != null)
                        {
                            movimento.guid = id;
                            dati.movimenti.Add(movimento);
                        }
                    }
                    else if (type == enum_Dati_App.Acquisti)
                    {
                        var acquisto = JsonConvert.DeserializeObject<Acquisto>(d);
                        if (acquisto != null)
                        {
                            acquisto.guid = id;
                            dati.acquisti.Add(acquisto);
                        }
                    }
                    else if (type == enum_Dati_App.Documenti)
                    {
                        var documento = JsonConvert.DeserializeObject<DocumentoPerScarico>(d);
                        if (documento != null)
                        {
                            documento.guid = id;
                            dati.documenti.Add(documento);
                        }
                    }
                    else if (type == enum_Dati_App.Visite)
                    {
                        var adapter = new VisiteAdapter();
                        var visita = JsonConvert.DeserializeObject<VisitePerScarico>(d);
                        Attivita attivita = null;
                        try
                        {
                            attivita = adapter.leggiAttivita(visita);
                            if (visita.VisiteRilievi != null)
                            {
                                AttivitaAdapter rilieviAdapter = new AttivitaAdapter();
                                attivita = rilieviAdapter.leggiAttivita(visita.VisiteRilievi);
                            }
                        }
                        catch (Exception ex)
                        {
                            logErrore(Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex));
                        }
                        if (attivita != null)
                        {
                            attivita.guid = id;
                            dati.visite.Add(attivita);
                            if (visita.VisiteDocumenti != null && visita.VisiteDocumenti.Count > 0)
                            {
                                var documento = visita.VisiteDocumenti[0];
                                documento.guid = id;
                                dati.documenti.Add(documento);
                            }
                        }
                    }
                    else if (type == enum_Dati_App.Attivita || type == enum_Dati_App.AttivitaCdG || type == enum_Dati_App.Rilievi)
                    {
                        var adapter = new AttivitaAdapter();
                        var ricetta = JsonConvert.DeserializeObject<RicettePerScarico>(d);
                        List<Attivita> attivita = new List<Attivita>();
                        try
                        {
                            if (type == enum_Dati_App.Rilievi && ricetta.Ricette == null)
                            {
                                attivita.Add(JsonConvert.DeserializeObject<Attivita>(d));
                            }
                            else if (type == enum_Dati_App.Attivita && ricetta.Ricette.Count > 1)
                            {
                                attivita = adapter.leggiAttivitaMiste(ricetta);
                            }
                            else
                            {
                                attivita.Add(adapter.leggiAttivita(ricetta));
                            }
                        }
                        catch (Exception ex)
                        {
                            logErrore(Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex));
                        }
                        if (attivita != null && attivita.Count > 0)
                        {
                            foreach (Attivita a in attivita)
                            {
                                a.guid = id;
                                switch (type)
                                {
                                    // case enum_Dati_App.Ricette: dati.attivitaPianificate.Add(attivita); break;
                                    case enum_Dati_App.AttivitaCdG: dati.attivita.Add(a); break;
                                    case enum_Dati_App.Attivita: dati.attivita.Add(a); break;
                                    case enum_Dati_App.Rilievi: dati.rilievi.Add(a); break;
                                }
                                if (ricetta.Documenti != null && ricetta.Documenti.Count > 0)
                                {
                                    var documento = ricetta.Documenti[0];
                                    documento.guid = id;
                                    dati.documenti.Add(documento);
                                }
                            }
                        }
                    }
                }

                result.RispostaStringa = dati;
                result.RispostaOK = true;

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        [Route("QDCA")]
        public ObjectResult GetStoricoQDCA(string tipo, string piva, string data, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<DatiApp> result = new RispostaStandard<DatiApp>();

            try
            {
                var dati = new DatiApp();
                var ws = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request);
                var r = ws.LeggiDatiAppStorico(new CoreWS_Dati_App(objP_super_server, objP_server, objP_utenti, tipo, piva, data));
                var dt = JsonConvert.DeserializeObject<DataTable>(r.RispostaStringa);

                foreach (DataRow row in dt.Rows)
                {
                    string id = row["ID"].ToString();
                    string d = row["Dati"].ToString();
                    string t = row["Tipo"].ToString();

                    enum_Dati_App type = (enum_Dati_App)Enum.Parse(typeof(enum_Dati_App), t);

                    if (type == enum_Dati_App.Manutenzioni)
                    {
                        var manutenzione = JsonConvert.DeserializeObject<Manutenzione>(d);
                        if (manutenzione != null)
                        {
                            manutenzione.guid = id;
                            dati.manutenzioni.Add(manutenzione);
                        }
                    }
                    else if (type == enum_Dati_App.Movimenti)
                    {
                        var movimento = JsonConvert.DeserializeObject<MovimentoDiMagazzino>(d);
                        if (movimento != null)
                        {
                            movimento.guid = id;
                            dati.movimenti.Add(movimento);
                        }
                    }
                    else if (type == enum_Dati_App.Acquisti)
                    {
                        var acquisto = JsonConvert.DeserializeObject<Acquisto>(d);
                        if (acquisto != null)
                        {
                            acquisto.guid = id;
                            dati.acquisti.Add(acquisto);
                        }
                    }
                    else if (type == enum_Dati_App.Documenti)
                    {
                        var documento = JsonConvert.DeserializeObject<DocumentoPerScarico>(d);
                        if (documento != null)
                        {
                            documento.guid = id;
                            dati.documenti.Add(documento);
                        }
                    }
                    else if (type == enum_Dati_App.Visite)
                    {
                        var adapter = new VisiteAdapter();
                        var visita = JsonConvert.DeserializeObject<VisitePerScarico>(d);
                        Attivita attivita = null;
                        try
                        {
                            attivita = adapter.leggiAttivita(visita);
                            if (visita.VisiteRilievi != null)
                            {
                                AttivitaAdapter rilieviAdapter = new AttivitaAdapter();
                                attivita = rilieviAdapter.leggiAttivita(visita.VisiteRilievi);
                            }
                        }
                        catch (Exception ex)
                        {
                            logErrore(Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex));
                        }
                        if (attivita != null)
                        {
                            attivita.guid = id;
                            dati.visite.Add(attivita);
                            if (visita.VisiteDocumenti != null && visita.VisiteDocumenti.Count > 0)
                            {
                                var documento = visita.VisiteDocumenti[0];
                                documento.guid = id;
                                dati.documenti.Add(documento);
                            }
                        }
                    }
                    else if (type == enum_Dati_App.Attivita || type == enum_Dati_App.AttivitaCdG || type == enum_Dati_App.Rilievi)
                    {
                        var adapter = new AttivitaAdapter();
                        var ricetta = JsonConvert.DeserializeObject<RicettePerScarico>(d);
                        List<Attivita> attivita = new List<Attivita>();
                        try
                        {
                            if (type == enum_Dati_App.Rilievi && ricetta.Ricette == null)
                            {
                                attivita.Add(JsonConvert.DeserializeObject<Attivita>(d));
                            }
                            else if (type == enum_Dati_App.Attivita && ricetta.Ricette.Count > 1)
                            {
                                attivita = adapter.leggiAttivitaMiste(ricetta);
                            }
                            else
                            {
                                attivita.Add(adapter.leggiAttivita(ricetta));
                            }
                        }
                        catch (Exception ex)
                        {
                            logErrore(Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex));
                        }
                        if (attivita != null && attivita.Count > 0)
                        {
                            foreach (Attivita a in attivita)
                            {
                                a.guid = id;
                                switch (type)
                                {
                                    // case enum_Dati_App.Ricette: dati.attivitaPianificate.Add(attivita); break;
                                    case enum_Dati_App.AttivitaCdG: dati.attivita.Add(a); break;
                                    case enum_Dati_App.Attivita: dati.attivita.Add(a); break;
                                    case enum_Dati_App.Rilievi: dati.rilievi.Add(a); break;
                                }
                                if (ricetta.Documenti != null && ricetta.Documenti.Count > 0)
                                {
                                    var documento = ricetta.Documenti[0];
                                    documento.guid = id;
                                    dati.documenti.Add(documento);
                                }
                            }
                        }
                    }
                }

                result.RispostaStringa = dati;
                result.RispostaOK = true;

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        [Route("DatiComuni")]
        public ObjectResult GetDatiComuni(string parametri, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<DatiComuniEntity> result = new RispostaStandard<DatiComuniEntity>();

            try
            {
                var r = new RispostaStandard();
                var dati = new DatiComuniEntity();
                var adapter = new ModelloAdapter();
                var ws = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request);
                var operazioneCausaleService = _serviceProvider.GetRequiredService<IOperazioneCausaleService>();

                DatiComuniRequest request = JsonConvert.DeserializeObject<DatiComuniRequest>(parametri);
                var permessi = new PermessiApp(request.permessi);

                Task[] tasks = new  Task[3] {
                    Task.Run(async () => {

                        // lavorazioni / attività cdg
                        if (permessi.attivita || permessi.rilievi || permessi.visite) {
                            r = ws.LeggiOperazioni(new CoreWS_Operazioni(objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.lavorazioni = adapter.leggiLavorazioni(r.RispostaStringa);
                            r = ws.LeggiAttivita(new CoreWS_Attivita(pivaSuperUser, objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.attivitaCDG = adapter.leggiAttivitaCDG(r.RispostaStringa);
                            r = ws.LeggiAttivitaOperazioni(new CoreWS_AttivitaXOperazioni(pivaSuperUser, objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.lavorazioniAttivitaCDG = adapter.leggiLavorazioniAttivitaCDG(r.RispostaStringa);
                            r = ws.LeggiOperazioniCombinazioni(new CoreWS_Operazioni(objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.operazioniCombinazioni = adapter.leggiOperazioniCombinazioni(r.RispostaStringa);
                        }

                        if (permessi.attivita || permessi.rilievi || permessi.visite) {
                            try
                            {
                                var param = new InData.Operazione.LeggiOperazione();
                                param.LavCod = -1;
                                param.Data = null;
                                var dt = await operazioneCausaleService.OperazioneCausale_LeggiAsync(param, UtilityAgronica.convertStringtoOBJparametri(objP_server, _securitySettings),  UtilityAgronica.convertStringtoOBJparametri(objP_utenti, _securitySettings));
                                if (dt != null && dt.Rows.Count > 0) dati.operazioniCausali = adapter.leggiOperazioniCausali(dt);
                            } 
                            catch (Exception ex) {dati.operazioniCausali = new List<OperazioneCausaleEntity>();}
                            
                        }

                        // avversità / unità di misura / tipi macchine                          
                        if (request.anagMetaschema) {

                            // avversità
                            if (permessi.attivita || permessi.rilievi) {
                                r = ws.LeggiAvversita(new CoreWS_Avversita(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.avversita = adapter.leggiAvversita(r.RispostaStringa);
                                r = ws.LeggiGruppoAvversita(new CoreWS_GruppoAvversita(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.gruppiAvversita = adapter.leggiGruppoAvversita(r.RispostaStringa);
                                r = ws.LeggiGruppoAvversitaAttive(new CoreWS_GruppoAvversitaAttive(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.gruppiAvversitaAttive = adapter.leggiGruppoAvversitaAttive(r.RispostaStringa);
                                r = ws.LeggiAvversitaSpecie(new CoreWS_Avversita_Specie(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.avversitaSpecie = adapter.leggiAvversitaSpecie(r.RispostaStringa);
                                r = ws.LeggiErbeInfestantiAttive(new CoreWS_Infestanti_Attive(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.infestantiAttive = adapter.leggiInfestantiAttive(r.RispostaStringa);
                            }

                            // unità di misura
                            if (permessi.attivita || permessi.rilievi) {
                                r = ws.LeggiCategorieUnitaMisura(new CoreWS_CategorieXUnitaMisura(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.categorieUnitaMisura = adapter.leggiCategorieUnitaMisura(r.RispostaStringa);
                            }

                            // tipi macchine
                            if (permessi.macchine) {
                                r = ws.LeggiTipiMacchine(new APICallsBasic(objP_super_server, objP_server, objP_utenti));
                                if (r != null && r.RispostaOK) dati.tipiMacchine = adapter.leggiTipiMacchine(r.RispostaStringa);
                            }

                        }
                        
                        // tipologie documenti
                        r = ws.LeggiTipologie(new CoreWS_Tipologie(objP_super_server, objP_server, objP_utenti));
                        if (r != null && r.RispostaOK) adapter.leggiTipologieDocumento(r.RispostaStringa, dati);
                        
                        // contatti / macchine
                        if (permessi.attivita || permessi.macchine) {
                            r = ws.LeggiContatti(new CoreWS_Contatti(objP_super_server, objP_server, objP_utenti, ""));
                            if (r != null && r.RispostaOK) adapter.leggiContatti(r.RispostaStringa, dati);
                            r = ws.LeggiMacchine(new CoreWS_Parco_Macchine(objP_super_server, objP_server, objP_utenti, ""));
                            if (r != null && r.RispostaOK) adapter.leggiMacchine(r.RispostaStringa, dati);
                        }
                        
                        // fornitori
                        if (request.anagFornitori && permessi.magazzini) {
                            r = ws.LeggiFornitori(new CoreWS_Contatti(objP_super_server, objP_server, objP_utenti, ""));
                            if (r != null && r.RispostaOK) adapter.leggiFornitori(r.RispostaStringa, dati);
                        }

                        // sincro anagrafica prodotti
                        if (permessi.attivita || permessi.magazzini)
                        {
                            // trasformati vegetali
                            if (request.anagTrasformatiVegetali) {
                                r = ws.LeggiProdottiGias(new CoreWS_Prodotti_GIAS(objP_super_server, objP_server, objP_utenti, "", (int)enum_CategorieMagazzino.TRASFORMATI_VEGETALI, "", ""));
                                if (r != null && r.RispostaOK) dati.prodotti = adapter.leggiProdottiOnline(r.RispostaStringa, "");
                            }
                        
                            // sementi
                            if (request.anagSementi) {
                                r = ws.LeggiProdottiGias(new CoreWS_Prodotti_GIAS(objP_super_server, objP_server, objP_utenti, "", (int)enum_CategorieMagazzino.SEMENTI, "", ""));
                                if (r != null && r.RispostaOK) dati.prodotti.AddRange(adapter.leggiProdottiOnline(r.RispostaStringa, ""));
                            }

                            // nazione default
                            string nazione = string.IsNullOrEmpty(request.anagNazioni)?"IT":request.anagNazioni.Split(',').FirstOrDefault();

                            // formulati
                            if (request.anagFormulati) {
                                r = ws.LeggiProdottiSenzaGiacenza(new CoreWS_ProdottiSenzaGiacenza(objP_super_server, objP_server, objP_utenti, "", (int)enum_CategorieMagazzino.FORMULATI, "", request.specieProdotti, "", nazione));
                                if (r != null && r.RispostaOK) dati.prodotti.AddRange(adapter.leggiProdottiOnline(r.RispostaStringa, ""));
                            }

                            // fertilizzanti
                            if (request.anagFertilizzanti) {
                                r = ws.LeggiProdottiSenzaGiacenza(new CoreWS_ProdottiSenzaGiacenza(objP_super_server, objP_server, objP_utenti, "", (int)enum_CategorieMagazzino.FERTILIZZANTI, "", "", "", nazione));
                                if (r != null && r.RispostaOK) dati.prodotti.AddRange(adapter.leggiProdottiOnline(r.RispostaStringa, ""));
                            }
                        }                        

                        // codifica prodotti aziendali
                        if (request.anagCodificaProdotti && permessi.magazzini) {
                            r = ws.LeggiCodificaProdotti(new CoreWS_Codifica_Prodotti(objP_super_server, objP_server, objP_utenti, pivaSuperUser, 0, (int)enum_CategorieMagazzino.SEMENTI));
                            if (r != null && r.RispostaOK) dati.codificaProdotti = adapter.leggiCodificaProdotti(r.RispostaStringa);
                        }

                        // disciplinari
                        if (!string.IsNullOrEmpty(request.anagDisciplinari)) {
                            var r4 = ws.LeggiDisciplinari(getRequest(new LeggiDisciplinari() { specie = new Specie(0), data = new DateTime(), privato = true }));
                            if (r4 != null && r4.RispostaOK) adapter.leggiDisciplinari(r4.RispostaStringa, dati);
                        }

                        // nazioni, province, comuni
                        if (request.anagMetaschema && !string.IsNullOrEmpty(request.anagNazioni)) {
                            if (permessi.aziende || permessi.pianocolturale) {
                                var nazioni = request.anagNazioni.Split(',').ToList();
                                var r1 = ws.LeggiNazioni(getRequest(""));
                                if (r1 != null && r1.RispostaOK) dati.nazioni = adapter.leggiNazioni(r1.RispostaStringa, nazioni);

                                var filtroNazioni = "|" + request.anagNazioni;
                                GetProvincie parametri = new GetProvincie();
                                parametri.stato = filtroNazioni;
                                parametri.regione = "";
                                var r2 = ws.LeggiProvince(getRequest(parametri));

                                if (r2 != null && r2.RispostaOK) {
                                    dati.regioni = adapter.leggiRegioni(r2.RispostaStringa);
                                    dati.province = adapter.leggiProvince(r2.RispostaStringa);
                                }
                                var r3 = ws.LeggiComuni(getRequest(filtroNazioni));
                                if (r3 != null && r3.RispostaOK) dati.comuni = adapter.leggiComuni(r3.RispostaStringa);
                            }
                        }

                        // specie zootecniche
                        if (request.anagMetaschema && permessi.visite) {
                            var r = ws.LeggiRisorseZootecniche(getRequest((object)null));
                            if (r != null && r.RispostaOK) dati.specieZootecniche = adapter.leggiSpecieZootecniche(r.RispostaStringa);
                        }

                        // tipologie gerarchia imprese
                        /* if (permessi.aziende) {
                            var r = ws.LeggiTipologieGerarchiaImprese(new APICallsBasic(objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.tipologieGerarchiaImprese = adapter.leggiTipologieGerarchiaImprese(r.RispostaStringa);
                        } */

                    }),
                    Task.Factory.StartNew(() => {
                        
                        // specie, varietà, finalità e destinazioni uso
                        if (request.anagMetaschema && !string.IsNullOrEmpty(request.anagUtilizziTerreno)) {
                            if (permessi.pianocolturale || permessi.visite) {
                                List<int> listaSpecie = request.anagUtilizziTerreno?.Split(',')?.Select(int.Parse)?.ToList();
                                if (listaSpecie != null && listaSpecie.Contains(0)) {
                                    var r0 = ws.LeggiDestinazioniUso(getRequest(new LeggiDestinazioniUso()));
                                    if (r0 != null && r0.RispostaOK) dati.destinazioniUso = adapter.leggiDestinazioniUso(r0.RispostaStringa);
                                    if (request.anagUtilizziTerreno == "0") listaSpecie = null; // usa filtro impostazioni utente
                                }
                                bool cache = false;
                                var r1 = ws.LeggiSpecie(getRequest(new LeggiSpecie() { cache = cache }));
                                if (r1 != null && r1.RispostaOK) dati.specie = adapter.leggiSpecie(r1.RispostaStringa, listaSpecie);
                                if (permessi.pianocolturale) {
                                    listaSpecie = (from x in dati.specie select x.codice).ToList();
                                    var r2 = ws.LeggiVarieta(getRequest(new LeggiCultivar() { specie = new Specie(0), listaSpecie = listaSpecie, cache = cache }));
                                    if (r2 != null && r2.RispostaOK) dati.varieta = adapter.leggiVarieta(r2.RispostaStringa);
                                    var r3 = ws.LeggiGruppoFinalita(getRequest(new LeggiFinalita() { specie = new Specie(0), listaSpecie = listaSpecie, cache = cache }));
                                    if (r3 != null && r3.RispostaOK) dati.finalita = adapter.leggiFinalita(r3.RispostaStringa);
                                    /* foreach (var specie in dati.specie) {
                                        var r2 = ws.LeggiVarieta(getRequest(new LeggiCultivar() { specie = new Specie(specie.codice) }));
                                        if (r2 != null && r2.RispostaOK) dati.varieta.AddRange(adapter.leggiVarieta(r2.RispostaStringa));
                                        var r3 = ws.LeggiGruppoFinalita(getRequest(new LeggiFinalita() { specie = new Specie(specie.codice) }));
                                        if (r3 != null && r3.RispostaOK) dati.finalita.AddRange(adapter.leggiFinalita(r3.RispostaStringa));
                                    } */
                                }
                            }
                        }

                    }),
                    Task.Factory.StartNew(() => {

                        // misure, indici stadi crescita per rilievi
                        if (request.anagMetaschema && permessi.rilievi) {
                            r = ws.LeggiRilievi(objP_super_server, objP_server, objP_utenti, CostantiPersonalizzate.LAVCOD_FASI_FENOLOGICHE.ToString(), request.anagPersonalizzataRilievoFasiFenologiche);
                            if (r != null && r.RispostaOK) dati.specieVegetaliStadiCrescita = adapter.leggiSpecieVegetaliStadiCrescita(r.RispostaStringa);
                            r = ws.LeggiRilievi(objP_super_server, objP_server, objP_utenti, CostantiPersonalizzate.LAVCOD_RILIEVO_INDICI_MATURITA.ToString(), request.anagPersonalizzataRilievoIndiciMaturita);
                            if (r != null && r.RispostaOK) adapter.leggiIndiciMaturita(r.RispostaStringa, dati);
                            r = ws.LeggiRilievi(objP_super_server, objP_server, objP_utenti, CostantiPersonalizzate.LAVCOD_RILIEVO_AVVERSITA_CAMPO.ToString(), request.anagPersonalizzataRilievoAvversita);
                            if (r != null && r.RispostaOK) adapter.leggiMisureAvversita(r.RispostaStringa, dati);
                            r = ws.LeggiRilievi(objP_super_server, objP_server, objP_utenti, CostantiPersonalizzate.LAVCOD_DANNI_RACCOLTA.ToString(), request.anagPersonalizzataRilievoDanniRaccolta);
                            if (r != null && r.RispostaOK) adapter.leggiDanniRaccolte(r.RispostaStringa, dati);
                            //r = ws.LeggiRilievi(objP_super_server, objP_server, objP_utenti, CostantiPersonalizzate.LAVCOD_RILIEVO_ERBE_INFESTANTI.ToString(), request.anagPersonalizzataRilievoErbeInfestanti);
                            //if (r != null && r.RispostaOK) adapter.leggiErbeInfestanti(r.RispostaStringa, dati);
                        }

                    }),
                };

                Task.WaitAll(tasks);

                // result.Compressa = true;
                // result.RispostaCompressa = zip(JsonConvert.SerializeObject(dati));
                result.RispostaStringa = dati;
                result.RispostaOK = true;
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        [Route("DatiAzienda")]
        public ObjectResult GetDatiAzienda(string piva, string data, string parametri, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            DatiAziendaRequest request = null;
            RispostaStandard<DatiAziendaEntity> result = new RispostaStandard<DatiAziendaEntity>();

            try
            {
                var r = new RispostaStandard();
                var dati = new DatiAziendaEntity();
                var adapter = new ModelloAdapter();
                var ws = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request);

                if (!string.IsNullOrEmpty(parametri)) request = JsonConvert.DeserializeObject<DatiAziendaRequest>(parametri);
                var permessi = new PermessiApp(request != null ? request.permessi : null);

                Task[] tasks = new Task[2] {
                    Task.Factory.StartNew(() => {

                        // dati azienda
                        var r0 = ws.LeggiImpresaModello(getRequest(new LeggiImpresa(piva)));
                        if (r0 != null && r0.RispostaOK) dati.azienda =  adapter.leggiImpresaModello(r0.RispostaStringa);

                        // centri aziendali
                        var r1 = ws.LeggiCentriAziendaliModello(new CoreWS_Centri_Aziendali(objP_super_server, objP_server, objP_utenti, piva, data));
                        if (r1 != null && r1.RispostaOK) dati.centriAziendali = adapter.leggiCentriAziendaliModello(r1.RispostaStringa);
                        
                        // magazzini
                        if (permessi.attivita || permessi.magazzini) {
                            var r2 = ws.LeggiMagazziniModello(getRequest(new LeggiMagazzini(piva, true)));
                            if (r2 != null && r2.RispostaOK) dati.magazzini = adapter.leggiMagazziniModello(r2.RispostaStringa);
                        }

                        // piano colturale (appezzamenti, impianti, esercizi, utilizzi terreno)
                        if (permessi.attivita || permessi.rilievi || permessi.visite || permessi.pianocolturale) {
                            if (request!=null && request.anagPianoColturale) {
                                var r = ws.LeggiAppezzamentiModello(new CoreWS_Appezzamenti(objP_super_server, objP_server, objP_utenti, piva, data));
                                if (r != null && r.RispostaOK) dati.pianoColturale = adapter.leggiPianoColturale(r.RispostaStringa);
                            } else {
                                r = ws.LeggiImpianti(new CoreWS_Impianti(objP_super_server, objP_server, objP_utenti, piva, data));
                                if (r != null && r.RispostaOK) dati.pianoColturale = adapter.leggiPianoColturale(r.RispostaStringa);
                                // dati.centriAziendali.AddRange(dati.pianoColturale.centriAziendali);
                            }
                        }
                        
                        // contatti / macchine
                        if (permessi.attivita || permessi.macchine) {
                            r = ws.LeggiContatti(new CoreWS_Contatti(objP_super_server, objP_server, objP_utenti, piva));
                            if (r != null && r.RispostaOK) adapter.leggiContatti(r.RispostaStringa, dati);
                            r = ws.LeggiMacchine(new CoreWS_Parco_Macchine(objP_super_server, objP_server, objP_utenti, piva));
                            if (r != null && r.RispostaOK) adapter.leggiMacchine(r.RispostaStringa, dati);
                        }

                        // fornitori
                        if (request !=null && request.anagFornitori && permessi.magazzini) {
                            r = ws.LeggiFornitori(new CoreWS_Contatti(objP_super_server, objP_server, objP_utenti, piva));
                            if (r != null && r.RispostaOK) adapter.leggiFornitori(r.RispostaStringa, dati);
                        }

                        // progetti / attività x centri aziendali
                        if (permessi.attivita) {
                            r = ws.LeggiProgetti(new CoreWS_Imputazione_Fasi(piva, objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.progetti = adapter.leggiProgetti(r.RispostaStringa, piva);
                            r = ws.LeggiAttivitaCentriAziendali(new CoreWS_AttivitaXCentri_Aziendali(piva, objP_super_server, objP_server, objP_utenti));
                            if (r != null && r.RispostaOK) dati.centriAziendaliAttivitaCDG = adapter.leggiCentriAziendaliAttivitaCDG(r.RispostaStringa, piva);
                        }

                        // impostazioni
                        r = ws.LeggiImpreseImpostazioni(new CoreWS_Imprese_Impostazioni(piva, objP_super_server, objP_server, objP_utenti));
                        if (r != null && r.RispostaOK) dati.impostazioni = adapter.leggiImpostazioni(r.RispostaStringa, piva);

                    }),
                    Task.Factory.StartNew(() => {
                        
                        // prodotti / giacenze / magazzini
                        if (permessi.attivita || permessi.magazzini) {
                            r = ws.LeggiProdotti(objP_super_server, objP_server, objP_utenti, piva, data, enum_CategorieMagazzino.FORMULATI, true);
                            if (r != null && r.RispostaOK) adapter.leggiProdotti(r.RispostaStringa, dati);
                            r = ws.LeggiProdotti(objP_super_server, objP_server, objP_utenti, piva, data, enum_CategorieMagazzino.FERTILIZZANTI, true);
                            if (r != null && r.RispostaOK) adapter.leggiProdotti(r.RispostaStringa, dati);
                            r = ws.LeggiProdotti(objP_super_server, objP_server, objP_utenti, piva, data, enum_CategorieMagazzino.SEMENTI, false);
                            if (r != null && r.RispostaOK) adapter.leggiProdotti(r.RispostaStringa, dati);
                            r = ws.LeggiProdotti(objP_super_server, objP_server, objP_utenti, piva, data, enum_CategorieMagazzino.TRASFORMATI_VEGETALI, false);
                            if (r != null && r.RispostaOK) adapter.leggiProdotti(r.RispostaStringa, dati);
                            r = ws.LeggiProdotti(objP_super_server, objP_server, objP_utenti, piva, data, enum_CategorieMagazzino.INSETTI, false);
                            if (r != null && r.RispostaOK) adapter.leggiProdotti(r.RispostaStringa, dati);
                            r = ws.LeggiProdottiGiacenze(new CoreWS_Prodotti_Giacenze(objP_super_server, objP_server, objP_utenti, piva, data));
                            if (r != null && r.RispostaOK) adapter.leggiProdottiGiacenze(r.RispostaStringa, dati);
                        }

                        // anagrafica prodotti: trasformati vegetali / sementi                        
                        if (request !=null && request.anagTrasformatiVegetali && (permessi.attivita || permessi.magazzini)) {
                            r = ws.LeggiProdottiGias(new CoreWS_Prodotti_GIAS(objP_super_server, objP_server, objP_utenti, piva, (int)enum_CategorieMagazzino.TRASFORMATI_VEGETALI, "", ""));
                            if (r != null && r.RispostaOK) dati.prodotti.AddRange(adapter.leggiProdottiOnline(r.RispostaStringa, piva));
                        }
                        if (request !=null && request.anagSementi && (permessi.attivita || permessi.magazzini)) {
                            r = ws.LeggiProdottiGias(new CoreWS_Prodotti_GIAS(objP_super_server, objP_server, objP_utenti, piva, (int)enum_CategorieMagazzino.SEMENTI, "", ""));
                            if (r != null && r.RispostaOK) dati.prodotti.AddRange(adapter.leggiProdottiOnline(r.RispostaStringa, piva));
                        }
                    })
                };

                Task.WaitAll(tasks);

                result.RispostaOK = true;
                result.RispostaStringa = dati;
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Attivita")]
        public ObjectResult PostAttivita([FromBody] Attivita attivita, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                AttivitaAdapter adapter = new AttivitaAdapter();
                RicettePerScarico dati = adapter.initDati();
                dati.cancellato = attivita.cancellato;
                dati.guid = attivita.guid;

                if (!attivita.cancellato)
                {
                    Job job = attivita.job;

                    // attività principale
                    adapter.scriviAttivita(dati, attivita, job, user);

                    // posizione
                    if (!adapter.isZero(attivita.latitude) && !adapter.isZero(attivita.longitude))
                    {
                        dati.posizione = (attivita.latitude + "|" + attivita.longitude).Replace(",", ".");
                    }

                    // scrive dati comuni
                    adapter.scriviDatiComuni(dati, username);
                }

                var request = getRequest(dati);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviAttivita(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("AttivitaMiste")]
        public ObjectResult PostAttivitaMiste([FromBody] List<Attivita> attivita, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                AttivitaAdapter adapter = new AttivitaAdapter();
                RicettePerScarico dati = adapter.initDati();

                dati.cancellato = attivita[0].cancellato;
                dati.guid = attivita[0].guid;

                if (!attivita[0].cancellato)
                {
                    foreach (Attivita a in attivita)
                    {
                        Job job = a.job;

                        // attività principale
                        adapter.scriviAttivita(dati, a, job, user);

                        // posizione
                        if (string.IsNullOrEmpty(dati.posizione) && !adapter.isZero(a.latitude) && !adapter.isZero(a.longitude))
                        {
                            dati.posizione = (a.latitude + "|" + a.longitude).Replace(",", ".");
                        }

                    }

                    // scrive dati comuni
                    adapter.scriviDatiComuni(dati, username);
                }

                var request = getRequest(dati);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviAttivita(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Rilievo")]
        public ObjectResult PostRilievo([FromBody] Attivita rilievo, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviRilievo(getRequest(rilievo));
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        [Route("Visite")]
        public ObjectResult GetVisite(string piva, [FromHeader] string Authorization)
        {
            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<List<Attivita>> result = new RispostaStandard<List<Attivita>>();

            try
            {
                // var ws = new CoreWSController(hc, coreWSBaseURL, tokenChiamateWS, Request);
                // var request = new LeggiVisite() { centro_aziendale = new CentroAziendale() { primaryKey = new CentroAziendale.PK(0, piva) } };
                // result = ws.LeggiVisite(getRequest(request));

                CoreWS_LeggiVisite request = new CoreWS_LeggiVisite(piva, objP_super_server, objP_server, objP_utenti);
                var r = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).LeggiVisite(request);

                if (!r.RispostaOK)
                {
                    result.RispostaOK = false;
                    result.Errore = r.Errore;
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }

                VisiteAdapter adapter = new VisiteAdapter();
                List<Attivita> visite = adapter.convertiAttivita(r.RispostaStringa);

                result.RispostaOK = true;
                result.RispostaStringa = visite;
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Visite")]
        public ObjectResult PostVisite([FromBody] Attivita attivita, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                VisiteAdapter adapter = new VisiteAdapter();
                VisitePerScarico dati = adapter.initDati();
                dati.cancellato = attivita.cancellato;
                dati.guid = attivita.guid;

                if (!attivita.cancellato)
                {
                    Job job = attivita.job;
                    Job job2 = null;

                    if (job.getTipo() == TipiJob.JOB_COMPOSITE)
                    {
                        var jobComposito = (JobComposito)attivita.job;
                        job = jobComposito.lavorazione;
                        job2 = jobComposito.attivitaCDG;
                    }

                    // scrive attività visita
                    adapter.scriviAttivita(dati, attivita, job2 != null ? job2.getCodice() : 0, user);
                    adapter.scriviDatiComuni(dati, username);

                    // gestione rilievo associato alla visita
                    if (job.getCodice() != LAVCOD_VISITA && !attivita.cancellato)
                    {

                        AttivitaAdapter rilieviAdapter = new AttivitaAdapter();
                        RicettePerScarico rilievi = rilieviAdapter.initDati();

                        // patch per passare la posizione rilievo
                        if (!adapter.isZero(attivita.latitude) && !adapter.isZero(attivita.longitude))
                        {
                            rilievi.posizione = (attivita.latitude + "|" + attivita.longitude).Replace(",", ".");
                        }

                        rilieviAdapter.scriviAttivita(rilievi, attivita, attivita.job, user);
                        rilieviAdapter.scriviDatiComuni(rilievi, username);

                        dati.VisiteRilievi = rilievi;
                    }

                }

                var request = getRequest(dati);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviVisite(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                // result.Errore = getErrorMessage(ex);
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Documenti")]
        public ObjectResult PostDocumenti([FromBody] DocumentoPerScarico documenti, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(documenti);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviDocumenti(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Impresa")]
        public ObjectResult PostImpresa([FromBody] Impresa impresa, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<Impresa> result = new RispostaStandard<Impresa>();

            try
            {
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviImpresa(getRequest(impresa));
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Centro")]
        public ObjectResult PostCentro([FromBody] CentroAziendale centro, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<CentroAziendale> result = new RispostaStandard<CentroAziendale>();

            try
            {
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviCentro(getRequest(centro));
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Appezzamento")]
        public ObjectResult PostAppezzamento([FromBody] Appezzamento appezzamento, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            if (appezzamento == null) return StatusCode(StatusCodes.Status400BadRequest, "Oggetto appezzamento non valorizzato");

            RispostaStandard<Appezzamento> result = new RispostaStandard<Appezzamento>();

            try
            {
                var request = getRequest(appezzamento);
                if (appezzamento.primaryKey.codice == 0 && !string.IsNullOrEmpty(appezzamento.guid))
                {
                    result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviAppezzamento(request);
                }
                else
                {
                    result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviAppezzamentiModello(request);
                }
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Magazzini")]
        public ObjectResult PostMagazzini([FromBody] List<Fabbricato> magazzini, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(magazzini);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviMagazzini(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Movimenti")]
        public ObjectResult PostMovimenti([FromBody] List<MovimentoDiMagazzino> movimenti, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(movimenti);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviMovimenti(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }


        [HttpPost]
        [Route("Acquisto")]
        public ObjectResult PostAcquisto([FromBody] Acquisto acquisto, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(acquisto);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviAcquisto(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Macchine")]
        public ObjectResult PostMacchine([FromBody] List<ParcoMacchine> macchine, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(macchine);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviMacchine(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Manutenzioni")]
        public ObjectResult PostManutenzioni([FromBody] List<Manutenzione> manutenzioni, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(manutenzioni);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviManutenzioni(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("EntrateUscite")]
        public ObjectResult PostEntrateUscite([FromBody] List<LogEventiEntity> eventi, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var adapter = new ModelloAdapter();
                var request = new CoreWS_LogEventiScrivi_APP(objP_super_server, objP_server, objP_utenti, adapter.convertiEventi(eventi, username));
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviEntrateUscite(request);

                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("Coordinate")]
        public ObjectResult PostCoordinate([FromBody] List<TrackingGPSEntity> coordinate, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var adapter = new ModelloAdapter();
                var request = new CoreWS_CoordinateScrivi_APP(objP_super_server, objP_server, objP_utenti, adapter.convertiCoordinate(coordinate, username));
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ScriviCoordinate(request);

                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("ControllaModalitaDemetra")]
        public ObjectResult ControllaModalitaDemetra([FromBody] string controllaModalitaDemetra, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(controllaModalitaDemetra);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ControllaModalitaDemetra(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost]
        [Route("ConsultaSincroDatiApp")]
        public ObjectResult ConsultaSincroDatiApp([FromBody] ConsultaSincroDatiApp consultaSincroDatiApp, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard result = new RispostaStandard();

            try
            {
                var request = getRequest(consultaSincroDatiApp);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ConsultaSincroDatiApp(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }


        [HttpPost]
        [Route("ConsultaLogInterscambio")]
        public ObjectResult ConsultaLogInterscambio([FromBody] ConsultaSincroDatiApp consultaLogInterscambio, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);

            RispostaStandard<OutData.Logging.MonitorLogInterscambio_OUT> result = new RispostaStandard<OutData.Logging.MonitorLogInterscambio_OUT>();

            try
            {
                var request = getRequest(consultaLogInterscambio);
                result = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).ConsultaLogInterscambio(request);
                if (!result.RispostaOK) return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(coreWsNotAuthenticatedException);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            }
            catch (Exception ex)
            {
                result.RispostaOK = false;
                result.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}
