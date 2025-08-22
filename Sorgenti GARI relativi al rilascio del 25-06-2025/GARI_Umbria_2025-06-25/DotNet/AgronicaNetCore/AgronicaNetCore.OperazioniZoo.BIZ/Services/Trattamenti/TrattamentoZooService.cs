using System.Data;
using OutData.Zoo;
using InData.Agenda;
using OutData.Kendo;
using AgronicaNetCore.Base.Models;
using AgronicaCoreModelsSTD.attivita;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.OperazioniZoo.BIZ.Resources;
using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaCoreModelsSTD.attivita.risorse;
using AgronicaCoreModelsSTD.attivita.dettagli;
using Microsoft.Extensions.DependencyInjection;
using AgronicaCoreModelsSTD.attivita.centri_di_costo;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Farmaci;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Operazione;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_ZooxAgenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agenda;
using AgronicaCoreModelsSTD.metaschema;
using AgronicaCoreModelsSTD.baseClass;
using InData.Zoo;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo;
using AutoMapper;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Agenda;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Movimenti;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Dettagli;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Destinazioni;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using AgronicaNetCore.Zoo.DAL.DataLayer.Prescrizioni;

using static AgronicaCoreModelsSTD.attivita.Attivita;
using static AgronicaNetCore.OperazioniZoo.BIZ.Services.Trattamenti.ITrattamentoZooService;
using AgronicaNetCore.OperazioniZoo.DAL.DataLayer.OperazioniZoo;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Fabbricati;

namespace AgronicaNetCore.OperazioniZoo.BIZ.Services.Trattamenti
{
    public class TrattamentoZooService : BaseServiceOperazioniZooBIZ, ITrattamentoZooService
    {
        private static readonly DateTime AGRODATAINIZIO = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO);
        private static readonly DateTime AGRODATAFINE = DateTime.Parse(CostantiPersonalizzate.AGRODATAFINE);

        private IMapper _mapper;

        private readonly IFarmaci _farmaciDal;
        private readonly IOperazione _operazioneDal;
        private readonly IOperazioniZoo _operazioniZooDal;
        private readonly IPrescrizioni _prescrizioniDal;

        private readonly IFabbricati _fabbricatiDal;

        private readonly IRicette_Zoo _ricetteZooDal;
        private readonly IRicette_Zoo_Agenda _ricetteZooAgDal;
        private readonly IRicette_ZooxAgenda _ricetteZooxAgDal;
        private readonly IRicette_Zoo_Movimenti _ricetteZooMovDal;
        private readonly IRicette_Zoo_Dettagli _ricetteZooDettDal;
        private readonly IRicette_Zoo_Destinazioni _ricetteZooDestDal;
        private readonly IRicette_Zoo_Dettaglio_Tecnico _ricetteZooDettTecDal;

        private readonly IAgenda _agendaDal;
        private readonly IMovimenti _movimentiDal;
        private readonly IMovimenti_Zoo _movimentiZooDal;
        private readonly IMovimenti_Dettagli _movDettagliDal;
        private readonly IMov_Dettaglio_Tecnico _movDettTecnicoDal;
        private readonly IMov_Destinazioni _movDestinazioniDal;

        public TrattamentoZooService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _mapper = provider.GetRequiredService<IMapper>();

            _farmaciDal = provider.GetRequiredService<IFarmaci>();
            _operazioneDal = provider.GetRequiredService<IOperazione>();
            _operazioniZooDal = provider.GetRequiredService<IOperazioniZoo>();

            _fabbricatiDal = provider.GetRequiredService<IFabbricati>();

            _ricetteZooDal = provider.GetRequiredService<IRicette_Zoo>();
            _ricetteZooAgDal = provider.GetRequiredService<IRicette_Zoo_Agenda>();
            _ricetteZooMovDal = provider.GetRequiredService<IRicette_Zoo_Movimenti>();
            _ricetteZooDettDal = provider.GetRequiredService<IRicette_Zoo_Dettagli>();
            _ricetteZooDestDal = provider.GetRequiredService<IRicette_Zoo_Destinazioni>();
            _ricetteZooDettTecDal = provider.GetRequiredService<IRicette_Zoo_Dettaglio_Tecnico>();

            _prescrizioniDal = provider.GetRequiredService<IPrescrizioni>();

            _ricetteZooxAgDal = provider.GetRequiredService<IRicette_ZooxAgenda>();
            _agendaDal = provider.GetRequiredService<IAgenda>();
            _movimentiDal = provider.GetRequiredService<IMovimenti>();
            _movimentiZooDal = provider.GetRequiredService<IMovimenti_Zoo>();
            _movDettagliDal = provider.GetRequiredService<IMovimenti_Dettagli>();
            _movDettTecnicoDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico>();
            _movDestinazioniDal = provider.GetRequiredService< IMov_Destinazioni> ();
        }

        #region Utilities

        public class CreaTrattamentoDaProtocollo_Resp : ITrattamentoResult
        {
            /// <summary>
            /// Gruppo di Prescrizioni creato dal ribaltamento del Protocollo
            /// </summary>
            public int Gruppo_Prescrizione { get; set; }
            /// <summary>
            /// Prima testata di Prescrizione 
            /// </summary>
            public int Id_Prescrizione { get; set; }
            /// <summary>
            /// Lista delle Somministrazioni create post-ribaltamento
            /// </summary>
            public List<int> Somministrazioni { get; set; }

            public CreaTrattamentoDaProtocollo_Resp()
            {
                Gruppo_Prescrizione = 0;
                Id_Prescrizione = 0;
                Somministrazioni = new List<int>();
            }
        }

        public class ScriviModificaSomministrazione_Resp : ITrattamentoResult
        {
            /// <summary>
            /// Gruppo di Prescrizioni (Ricette_Zoo.Gruppo_Ricetta)
            /// </summary>
            public int Gruppo_Prescrizione { get; set; }
            /// <summary>
            /// Id della testata di Prescrizione (Ricette_Zoo.IdRicetta)
            /// </summary>
            public int Id_Prescrizione { get; set; }
            /// <summary>
            /// Id della riga di Prescrizione (Ricette_Zoo_Agenda.IdAgenda)
            /// </summary>
            public int Id_Riga_Prescrizione { get; set; }
            /// <summary>
            /// Id_Agenda della Somministrazione (Agenda.Id_Agenda)
            /// </summary>
            public int Id_Somministrazione { get; set; }

            public ScriviModificaSomministrazione_Resp()
            {
                Gruppo_Prescrizione = 0;
                Id_Prescrizione = 0;
                Id_Riga_Prescrizione = 0;
                Id_Somministrazione = 0;
            }
        }

        /// <summary>
        ///  Utility per Parse di campi string to int
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int ParseRequiredInt(string objectName, string fieldName, string fieldValue, string errorMessage = "")
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                throw new Exception($"Il campo '{fieldName}' dell'oggetto '{objectName}' non è stato valorizzato.{errorMessage}");
            if (!int.TryParse(fieldValue, out int result))
                throw new Exception($"Il campo '{fieldName}' dell'oggetto '{objectName}' deve essere numerico.{errorMessage}");
            return result;
        }

        private static string GetAlimentoDesFromCod(int alimentoCod)
        {
            return alimentoCod switch
            {
                1 => "CARNE",
                2 => "LATTE",
                _ => string.Empty,
            };
        }

        private static FiltroAggiuntivo GetFiltroAggiuntivoGruppoRic(int Gruppo_Ricetta)
        {
            if (Gruppo_Ricetta == 0)
                throw new ArgumentException($"'{nameof(Gruppo_Ricetta)}' non può essere 0.", nameof(Gruppo_Ricetta));

            FiltroAggiuntivo filtroAggiuntivo = new();
            filtroAggiuntivo.AddFilter("Gruppo_Ricetta", "@gruRic", Comparison_Operators.Equal, Boolean_Operators.First, Gruppo_Ricetta, typeof(int));
            return filtroAggiuntivo;
        }

        private static FiltroAggiuntivo GetFiltroAggiuntivoCauMov(string Cau_Mov) 
        {
            if (string.IsNullOrEmpty(Cau_Mov))
                throw new ArgumentException($"'{nameof(Cau_Mov)}' non può essere null o vuoto.", nameof(Cau_Mov));

            FiltroAggiuntivo filtroAggiuntivo = new();
            filtroAggiuntivo.AddFilter("Cau_Mov", "@cauMov", Comparison_Operators.Equal, Boolean_Operators.First, Cau_Mov, typeof(string));
            return filtroAggiuntivo;
        }
        
        private static FiltroAggiuntivo GetFiltroAggiuntivoProCod(int Pro_Cod) 
        {
            if (Pro_Cod == 0)
                throw new ArgumentException($"'{nameof(Pro_Cod)}' non può essere 0.", nameof(Pro_Cod));

            FiltroAggiuntivo filtroAggiuntivo = new();
            filtroAggiuntivo.AddFilter("Pro_Cod", "@proCod", Comparison_Operators.Equal, Boolean_Operators.First, Pro_Cod, typeof(int));
            return filtroAggiuntivo;
        }
        
        private static FiltroAggiuntivo GetFiltroAggiuntivoDettCod(int Dett_Cod) 
        {
            if (Dett_Cod == 0)
                throw new ArgumentException($"'{nameof(Dett_Cod)}' non può essere 0.", nameof(Dett_Cod));

            FiltroAggiuntivo filtroAggiuntivo = new();
            filtroAggiuntivo.AddFilter("Dett_Cod", "@dettcod", Comparison_Operators.Equal, Boolean_Operators.First, Dett_Cod, typeof(int));
            return filtroAggiuntivo;
        }

        #endregion

        #region Leggi Attivita

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione"></param>
        /// <param name="listCodAnimale"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<List<CapoAnimaleCDC>> GetListaCapoAnimaleCdc(Attivita somministrazione, List<int> listCodAnimale, AgronicaCoreParametri objP)
        {
            List<CapoAnimaleCDC> animali = new();

            string piva = somministrazione.centroAziendale.primaryKey.partitaIva;
            int saCod = somministrazione.centroAziendale.primaryKey.codice;
            int staNum = somministrazione.fabbricatoCod;

            var dtGiacenze = await _operazioniZooDal.Leggi_GiacenzeAsync(piva, saCod, staNum, 0, 0, DateTime.Now, objP, listCod_Animali: listCodAnimale);
            if (dtGiacenze != null && dtGiacenze.Rows.Count > 0)
                foreach (var row in dtGiacenze.AsEnumerable())
                {
                    int codAnimale = (int)row["Cod_Animale"];
                    string matricola = (string)row["Matricola"];
                    int razCod = (int)row["RAZ_COD"];
                    string razDes = (string)row["RAZ_DES"];
                    int raggrCod = (int)row["Raggruppamento_Cod"];
                    string raggrDes = (string)row["Raggruppamento_Des"];
                    int statoCod = (int)row["Stato_Cod"];
                    string statoDes = (string)row["Stato_Des"];
                    DateTime inizio = (DateTime)row["Validita_Inizio"];
                    DateTime fine = (DateTime)row["Validita_Fine"];

                    animali.Add(new CapoAnimaleCDC()
                    {
                        capoAnimale = new(piva, codAnimale, matricola)
                        {
                            razza = new(razCod, razDes),
                            validita = new(inizio, fine),
                            statiAccrescimento = new List<StatoAccrescimento>()
                            {
                                new(statoCod, statoDes),
                            },
                        },
                        sottogruppoStalla_ingresso = new()
                        {
                            codice = raggrCod,
                            nome = raggrDes,
                        }
                    });
                }

            return animali;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoAgenda"></param>
        /// <param name="somministrazione"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task PopolaDettagliSomministrazione(WriteAgenda dtoAgenda, Attivita somministrazione, AgronicaCoreParametri objP)
        {
            DataTable? dtMovimenti = await _movimentiDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, objP, GetFiltroAggiuntivoCauMov(CAU_MOV.CAU_TRATTAMENTO_ZOO));
            if (dtMovimenti == null || dtMovimenti.Rows.Count == 0)
                return;
            var dtoMovimenti = _mapper.Map<WriteMovimenti>(new MovimentiRow(dtMovimenti.Rows[0]));
            
            DataTable? dtMovDett = await _movDettagliDal.ReadAsync(dtoMovimenti.Piva, dtoMovimenti.Id_Agenda, dtoMovimenti.Id_Mov, 0, objP);
            if (dtMovDett == null || dtMovDett.Rows.Count == 0)
                return;

            foreach (var dtoMovDett in dtMovDett.AsEnumerable().Select(row => _mapper.Map<WriteMovDettagli>(new MovDettagliRow(row))))
            {
                // Aggiunta dettagli Prodotto della Riga di Prescrizione
                int farmCod = dtoMovDett.Pro_Cod ?? 0;
                string aic = "";
                string farmDes = "";

                if (farmCod != 0)
                {
                    DataTable? dtFarmaco = await _farmaciDal.LeggiFarmaciAsync(farmCod, "", objP);
                    if (dtFarmaco != null && dtFarmaco.Rows.Count > 0)
                    {
                        aic = (string)dtFarmaco.Rows[0]["AIC"];
                        farmDes = $"{(string)dtFarmaco.Rows[0]["Denominazione"]} - {(string)dtFarmaco.Rows[0]["Confezione"]}";
                    }
                }
                
                int udmCod = dtoMovDett.Udm_Cod ?? 0;
                string udmDes = "";
                string udmSim = "";
                if (udmCod != 0)
                {
                    switch ((enum_UnitaMisura)udmCod)
                    {
                        case enum_UnitaMisura.Grammi:
                            udmDes = "Grammi";
                            udmSim = "g";
                            break;
                        case enum_UnitaMisura.Millilitri:
                            udmDes = "Millilitri";
                            udmSim = "ml";
                            break;
                        case enum_UnitaMisura.KG:
                            udmDes = "Chilogrammi";
                            udmSim = "kg";
                            break;
                        case enum_UnitaMisura.Litri:
                            udmDes = "Litri";
                            udmSim = "l";
                            break;
                    }
                }

                string trattNumero = dtoMovDett.Rif_Esterno;
                string sommNumero = dtoMovDett.Extra_Str;
                string regScoNum = dtoMovDett.RegSco_Numero;
                int elemCod = dtoMovDett.Elem_Cod ?? 0;
                double qta = dtoMovDett.Qta_Extra_Totale ?? 0;

                var dettaglioSomm = new DettaglioRegistroSomministrazioni()
                {
                    codice = sommNumero,
                    prodotto = new(farmCod, elemCod)
                    {
                        descrizione = farmDes,
                        codice_alfanumerico = aic,
                    },
                    codiceAIC = aic,
                    numTrattamento = trattNumero,
                    dataPrescrizione = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO),
                    quantitaTotaleReale = (decimal)qta,
                    regSco_Numero = regScoNum,
                    unitaDiMisura = new(udmCod)
                    {
                        tipoControllo = new BaseCodeDescr(udmCod, udmDes),
                        simbolo = udmSim,
                    },
                    validita = new(somministrazione.inizio, somministrazione.fine),
                    durataTrattamento = (int)(somministrazione.fine - somministrazione.inizio).TotalDays + 1,
                };

                DataTable? dtoMovDettTec = await _movDettTecnicoDal.ReadAsync(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, dtoMovDett.Id_Mov_Det, 0, objP);
                if (dtoMovDettTec != null && dtoMovDettTec.Rows.Count > 0)
                {
                    List<TempiSospensione> tempiSospensione = new();
                    foreach (var dtoMovDte in dtoMovDettTec.AsEnumerable().Select(row => _mapper.Map<WriteMovDettTecnico>(new MovDettTecnicoRow(row))))
                    {
                        int alimCod = dtoMovDte.Dett_Cod ?? 0;
                        int durataSosp = dtoMovDte.Extra_Int ?? 0;
                        tempiSospensione.Add(new TempiSospensione()
                        {
                            Alimento = new BaseCodeDescr(alimCod, GetAlimentoDesFromCod(alimCod)),
                            tempoSospensione = durataSosp,
                        });
                    }
                    dettaglioSomm.sospensione = tempiSospensione.ToArray();
                }
                somministrazione.risorse.Add(dettaglioSomm);

                DataTable? dtMovDest = await _movDestinazioniDal.ReadAsync(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, dtoMovDett.Id_Mov_Det, 0, objP);
                if (dtMovDest != null && dtMovDest.Rows.Count > 0)
                {
                    List<int> listaCodAnimale = dtMovDest.AsEnumerable()
                        .Select(row => _mapper.Map<WriteMovDestinazioni>(new MovDestinazioniRow(row)))
                        .Select(dto => dto.Id_Destinazione).ToList();

                    // Leggere capo da Zoo_Animali
                    var capiAnimale = await GetListaCapoAnimaleCdc(somministrazione, listaCodAnimale, objP);
                    foreach (var dtoDest in dtMovDest.AsEnumerable().Select(row => _mapper.Map<WriteMovDestinazioni>(new MovDestinazioniRow(row))))
                    {
                        var capoCdc = capiAnimale.Find(capoCdc => capoCdc.capoAnimale.codice == dtoDest.Id_Destinazione);
                        if (capoCdc != null)
                            capoCdc.qtaSomministrata = dtoDest.Qta ?? 0;

                    }
                    somministrazione.centriDiCosto.AddRange(capiAnimale);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="somministrazione"></param>
        /// <param name="objP_Server"></param>
        /// <returns></returns>
        private async Task PopolaDettagliScaricoProdotti(string piva, int idAgenda, Attivita somministrazione, AgronicaCoreParametri objP_Server)
        {
            DataTable? dtMovimenti = await _movimentiDal.ReadAsync(piva, idAgenda, 0, objP_Server, GetFiltroAggiuntivoCauMov(CAU_MOV.CAU_SCARICO));
            if (dtMovimenti == null || dtMovimenti.Rows.Count == 0)
                return;

            int idMov = (int)dtMovimenti.Rows[0]["Id_Mov"];

            DataTable? dtMovDett = await _movDettagliDal.ReadAsync(piva, idAgenda, idMov, 0, objP_Server);
            if (dtMovDett == null || dtMovDett.Rows.Count == 0)
                return;

            foreach (var dtoMovDett in dtMovDett.AsEnumerable().Select(row => _mapper.Map<WriteMovDettagli>(new MovDettagliRow(row))))
            {
                int idMovDet = dtoMovDett.Id_Mov_Det;

                DataTable? dtMovDest = await _movDestinazioniDal.ReadAsync(piva, idAgenda, idMov, idMovDet, 0, objP_Server);
                if (dtMovDest == null || dtMovDest.Rows.Count == 0)
                    return;

                string Piva_Magazzino = "";
                int Sa_Cod_Magazzino = 0;
                int Fabbricato_Cod_Magazzino = 0;
                string Fabbricato_Des_Magazzino = "";

                if ((int)dtMovDest.Rows[0]["Tipo_Destinazione"] == TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_MAGAZZINO)
                {
                    Piva_Magazzino = (string)dtMovDest.Rows[0]["Piva"];
                    Sa_Cod_Magazzino = (int)dtMovDest.Rows[0]["Sa_Cod"];
                    Fabbricato_Cod_Magazzino = (int)dtMovDest.Rows[0]["Id_Destinazione"];
                    var dtMagazzino = await _fabbricatiDal.ReadAsync(Piva_Magazzino, Sa_Cod_Magazzino, Fabbricato_Cod_Magazzino, objP_Server);
                    if (dtMagazzino == null || dtMagazzino.Rows.Count > 0)
                        Fabbricato_Des_Magazzino = (string)dtMagazzino.Rows[0]["Fabbricato_Des"];
                }

                // Aggiunta dettagli Prodotto della Riga di Prescrizione
                int farmCod = dtoMovDett.Pro_Cod ?? 0;
                string aic = "";
                DataTable? dtFarmaco = await _farmaciDal.LeggiFarmaciAsync(farmCod, "", objP_Server);
                if(dtFarmaco != null && dtFarmaco.Rows.Count > 0)
                    aic = (string)dtFarmaco.Rows[0]["AIC"];

                string trattNumero = dtoMovDett.Rif_Esterno;
                string sommNumero = dtoMovDett.Extra_Str;
                string regScoNum = dtoMovDett.RegSco_Numero;
                int udmCod = dtoMovDett.Udm_Cod ?? 0;
                int elemCod = dtoMovDett.Elem_Cod ?? 0;
                decimal qta = decimal.Parse(dtoMovDett.Qta_Extra_Totale?.ToString() ?? throw new ArgumentNullException("Qta_Extra_Totale is null"));

                var scaricoProd = new RisorsaProdotto()
                {
                    prodotto = new(farmCod, elemCod)
                    {
                        tipo = new TipoRisorsa(elemCod),
                        codice_alfanumerico = aic,
                    },
                    quantitaTotaleReale = qta,
                    unitaDiMisura = new UnitaDiMisura(udmCod),
                    MagazziniMovimentazioni = new()
                    {
                        new()
                        {
                            Magazzino = new()
                            {
                                primaryKey = new Fabbricato.PK(Piva_Magazzino, Sa_Cod_Magazzino, Fabbricato_Cod_Magazzino),
                                tipo = (int)enum_TipoFabbricato.Magazzino_Aziendale,                                
                                descrizione = Fabbricato_Des_Magazzino
                            },
                            Lotto = dtoMovDett.Lotto,
                        }
                    },
                };
                somministrazione.risorse.Add(scaricoProd);
            }
        }

        /// <summary>
        /// Converte una riga di Agenda (Somministrazione) in un oggetto Attivita Zoo 
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <param name="dtoAgenda"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<Attivita> DtAgendaToAttivita(int idAgenda, WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            string piva = dtoAgenda.Piva;
            DateTime dataInizio = dtoAgenda.Validita_Inizio;
            DateTime dataFine = dtoAgenda.Validita_Fine;

            int idRigaRicetta = 0;
            DataTable? dtRicetteZxAgenda = await _ricetteZooxAgDal.ReadAsync(0, 0, idAgenda, objP);
            if (dtRicetteZxAgenda != null && dtRicetteZxAgenda.Rows.Count == 1)
                idRigaRicetta = (int)dtRicetteZxAgenda.Rows[0]["Id_RigaRicetta"];

            Attivita somministrazione = new()
            {
                codice = idAgenda.ToString(),
                tipo = Tipo_Attivita.QuadernoDiCampagna,
                codiceOperazioneRicetta = idRigaRicetta.ToString(),
                job = new Zootecnia(LAV_COD.LAVCOD_CUREMEDICAMENTI_ANIMALI, "Cure e Medicamenti"),
                centroAziendale = new CentroAziendale(new CentroAziendale.PK(dtoAgenda.Sa_Cod ?? 0, piva)),
                fabbricatoCod = dtoAgenda.Sta_Num ?? 0,
                inizio = dataInizio,
                fine = dataFine,
                risorse = new List<Risorsa>(),
                centriDiCosto = new List<CentroDiCosto>(),
            };

            await PopolaDettagliSomministrazione(dtoAgenda, somministrazione, objP);            
            await PopolaDettagliScaricoProdotti(piva, idAgenda, somministrazione, objP);

            return somministrazione;
        }

        /// <summary>
        /// Popola l'oggetto Attivita a partire da un Trattamento Zoo
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda">Agenda.Id_Agenda</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<Attivita> PopulateAttivitaZooFromAgenda(string piva, int idAgenda, AgronicaCoreParametri objP)
        {
            DataTable? dtAgenda = await _agendaDal.ReadAsync(piva, idAgenda, objP);

            if (dtAgenda == null || dtAgenda.Rows.Count == 0)
                throw new Exception("Operazione non trovata. Impossibile generare l'oggetto Attivita.");
            if (dtAgenda.Rows.Count != 1)
                throw new Exception("Piu' di un'Operazione trovata. Impossibile generare l'oggetto Attivita.");

            Attivita attivita = await DtAgendaToAttivita(idAgenda, _mapper.Map<WriteAgenda>(new AgendaRow(dtAgenda.Rows[0])), objP);

            return attivita;
        }

        public async Task<Attivita?> GetAttivitaFromAgenda(string Piva, int Id_Agenda, AgronicaCoreParametri objP)
        {
            Attivita? res = null;

            try
            {
                res = await PopulateAttivitaZooFromAgenda(Piva, Id_Agenda, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
            }

            return res;
        }

        #endregion

        #region Scrittura Attivita

        #region Crea Prescrizione GIAS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoProtRZDS"></param>
        /// <param name="dtoIndTerapRZD"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        //private async Task<WriteRicetteZooDestinazioni> CreateRicZooDestinazioniFromProtocollo(WriteRicetteZooDestinazioni dtoProtRZDS, WriteRicetteZooDettagli dtoIndTerapRZD, AgronicaCoreParametri objP)
        //{
        //    var dtoIndTerapRZDS = _mapper.Map<WriteRicetteZooDestinazioni>(dtoProtRZDS);

        //    dtoIndTerapRZDS.IdRicetta = dtoIndTerapRZD.IdRicetta;
        //    dtoIndTerapRZDS.IdAgenda = dtoIndTerapRZD.IdAgenda;
        //    dtoIndTerapRZDS.IdMov = dtoIndTerapRZD.IdMov;
        //    dtoIndTerapRZDS.IdDettaglio = dtoIndTerapRZD.IdDettaglio;
        //    dtoIndTerapRZDS.IdDestinazione = 0;

        //    dtoIndTerapRZDS.IdDestinazione = await _ricetteZooDestBiz.ScriviModificaAsync(dtoIndTerapRZDS, objP);

        //    return dtoIndTerapRZDS;
        //}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoProtRZDTT"></param>
        /// <param name="dtoIndTerapRZD"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZooDettaglioTecnico> CreateRicZooDettaglioTecnicoFromProtocollo(WriteRicetteZooDettaglioTecnico dtoProtRZDTT, WriteRicetteZooDettagli dtoIndTerapRZD, AgronicaCoreParametri objP)
        {
            var dtoIndTerapRZDTT = _mapper.Map<WriteRicetteZooDettaglioTecnico>(dtoProtRZDTT);

            dtoIndTerapRZDTT.Id_Ricetta = dtoIndTerapRZD.IdRicetta;
            dtoIndTerapRZDTT.Id_Agenda = dtoIndTerapRZD.IdAgenda;
            dtoIndTerapRZDTT.Id_Mov = dtoIndTerapRZD.IdMov;
            dtoIndTerapRZDTT.Id_Mov_Det = dtoIndTerapRZD.IdDettaglio;
            dtoIndTerapRZDTT.Id_Reg_Dettaglio = 0;

            dtoIndTerapRZDTT.Id_Reg_Dettaglio = await _ricetteZooDettTecDal.ScriviModificaAsync(dtoIndTerapRZDTT, objP);

            return dtoIndTerapRZDTT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoProtRZD"></param>
        /// <param name="dtoIndTerapRZM"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZooDettagli> CreateRicZooDettagliFromProtocollo(WriteRicetteZooDettagli dtoProtRZD, WriteRicetteZooMovimenti dtoIndTerapRZM, AgronicaCoreParametri objP)
        {
            var dtoIndTerapRZD = _mapper.Map<WriteRicetteZooDettagli>(dtoProtRZD);

            dtoIndTerapRZD.IdRicetta = dtoIndTerapRZM.IdRicetta;
            dtoIndTerapRZD.IdAgenda = dtoIndTerapRZM.IdAgenda;
            dtoIndTerapRZD.IdMov = dtoIndTerapRZM.IdMov;
            dtoIndTerapRZD.IdDettaglio = 0;

            dtoIndTerapRZD.IdDettaglio = await _ricetteZooDettDal.ScriviModificaAsync(dtoIndTerapRZD, objP);

            //var dtRicZooDest = await _ricetteZooDestDal.ReadAsync("", 0, dtoProtRZD.IdRicetta, dtoProtRZD.IdAgenda, dtoProtRZD.IdMov, dtoProtRZD.IdDettaglio, 0, objP);
            //if (dtRicZooDest != null && dtRicZooDest.Rows.Count > 0)
            //    foreach (DataRow rowRZDS in dtRicZooDest.Rows)
            //    {
            //        var dtoProtRZDS = _mapper.Map<WriteRicetteZooDestinazioni>(new RicetteZooDestinazioniRow(rowRZDS));
            //        var dtoIndTerapRZDS = await CreateRicZooDestinazioniFromProtocollo(dtoProtRZDS, dtoIndTerapRZD, objP);
            //    }

            var dtRicZooDettTec = await _ricetteZooDettTecDal.ReadAsync("", 0, dtoProtRZD.IdRicetta, dtoProtRZD.IdAgenda, dtoProtRZD.IdMov, dtoProtRZD.IdDettaglio, 0, objP);
            if (dtRicZooDettTec != null && dtRicZooDettTec.Rows.Count > 0)
                foreach (DataRow rowRZDTT in dtRicZooDettTec.Rows)
                {
                    var dtoProtRZDTT = _mapper.Map<WriteRicetteZooDettaglioTecnico>(new RicetteZooDettTecnicoRow(rowRZDTT));
                    var dtoIndTerapRZDTT = await CreateRicZooDettaglioTecnicoFromProtocollo(dtoProtRZDTT, dtoIndTerapRZD, objP);
                }

            return dtoIndTerapRZD;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoProtRZM"></param>
        /// <param name="dtoIndTerapRZA"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZooMovimenti> CreateRicZooMovimentiFromProtocollo(WriteRicetteZooMovimenti dtoProtRZM, WriteRicetteZooAgenda dtoIndTerapRZA, AgronicaCoreParametri objP)
        {
            var dtoIndTerapRZM = _mapper.Map<WriteRicetteZooMovimenti>(dtoProtRZM);

            dtoIndTerapRZM.IdRicetta = dtoIndTerapRZA.IdRicetta;
            dtoIndTerapRZM.IdAgenda = dtoIndTerapRZA.IdAgenda;
            dtoIndTerapRZM.IdMov = 0;

            dtoIndTerapRZM.IdMov = await _ricetteZooMovDal.ScriviModificaAsync(dtoIndTerapRZM, objP);

            var dtRicZooDett = await _ricetteZooDettDal.ReadAsync("", 0, dtoProtRZM.IdRicetta, dtoProtRZM.IdAgenda, dtoProtRZM.IdMov, 0, objP);
            if (dtRicZooDett != null && dtRicZooDett.Rows.Count > 0)
                foreach (DataRow rowRZD in dtRicZooDett.Rows)
                {
                    var dtoProtRZD = _mapper.Map<WriteRicetteZooDettagli>(new RicetteZooDettagliRow(rowRZD));
                    var dtoIndTerapRZD = await CreateRicZooDettagliFromProtocollo(dtoProtRZD, dtoIndTerapRZM, objP);
                }   

            return dtoIndTerapRZM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoProtRZA"></param>
        /// <param name="idIndTerap"></param>
        /// <param name="sommNum">Somministrazione Numero x (in base al numero di Somministrazioni impostate nella Riga di Protocollo)</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZooAgenda> CreateRicZooAgendaFromProtocollo(WriteRicetteZooAgenda dtoProtRZA, int idIndTerap, int sommNum, AgronicaCoreParametri objP)
        {
            var dtoIndTerapRZA = _mapper.Map<WriteRicetteZooAgenda>(dtoProtRZA);
            dtoIndTerapRZA.IdRicetta = idIndTerap;
            dtoIndTerapRZA.IdAgenda = 0;
            dtoIndTerapRZA.Numero = "";
            dtoIndTerapRZA.Note = sommNum.ToString() + " di " + dtoProtRZA.Numero_Somm.ToString();
            dtoIndTerapRZA.Numero_Somm = sommNum;

            dtoIndTerapRZA.IdAgenda = await _ricetteZooAgDal.ScriviModificaAsync(dtoIndTerapRZA, objP);

            var dtRicZooMov = await _ricetteZooMovDal.ReadAsync("", 0, dtoProtRZA.IdRicetta, dtoProtRZA.IdAgenda, 0, objP);
            if (dtRicZooMov != null && dtRicZooMov.Rows.Count > 0)
                foreach (DataRow rowRZM in dtRicZooMov.Rows)
                {
                    var dtoProtRZM = _mapper.Map<WriteRicetteZooMovimenti>(new RicetteZooMovimentiRow(rowRZM));
                    var dtoIndTerapRZM = await CreateRicZooMovimentiFromProtocollo(dtoProtRZM, dtoIndTerapRZA, objP);
                }

            return dtoIndTerapRZA;
        }

        /// <summary>
        /// Scrive una nuova riga di Ricette_Zoo (Indicazione Terapeutica Da Procollo GIAS) partendo da una riga di Ricette_Zoo (Protocollo)
        /// </summary>
        /// <param name="dtoProtocollo"></param>
        /// <param name="idGruppoPres"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<WriteRicetteZoo> CreateRicetteZooFromProtocollo(WriteRicetteZoo dtoProtocollo, int idGruppoPres, AgronicaCoreParametri objP)
        {
            var dtoIndTerap = _mapper.Map<WriteRicetteZoo>(dtoProtocollo);

            dtoIndTerap.IdRicetta = 0;
            dtoIndTerap.Numero = "";
            dtoIndTerap.Pin = "";
            dtoIndTerap.StatoCodice = 0;
            dtoIndTerap.TipoCodice = (int)enum_TipoPrescrizione.Da_Protocollo_GIAS;
            dtoIndTerap.Id_Protocollo = dtoProtocollo.IdRicetta;
            dtoIndTerap.ProtocolloCodice = dtoProtocollo.Numero;
            dtoIndTerap.Validita_Inizio = DateTime.Now;
            dtoIndTerap.Gruppo_Ricetta = idGruppoPres;

            dtoIndTerap.IdRicetta = await _ricetteZooDal.ScriviModificaAsync(dtoIndTerap, objP);

            return dtoIndTerap;
        }

        /// <summary>
        /// Ribalta un Protocollo su una nuova Ricetta_Zoo (Tipo = Da Protocollo GIAS)
        /// </summary>
        /// <param name="idProtocollo"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZoo> ProjectProtocolloToIndTerapeutica(int idProtocollo, AgronicaCoreParametri objP)
        {
            // IdRicetta della prima Prescrizione da associare alla Somministrazione in creazione
            WriteRicetteZoo dtoFirstPres = new();

            try
            {
                var dtProt = await _ricetteZooDal.ReadAsync("", 0, 0, idProtocollo, "", objP);
                if (dtProt == null || dtProt.Rows.Count == 0)
                    throw new Exception($"Protocollo {idProtocollo} non trovato.");

                var dtoProtocollo = _mapper.Map<WriteRicetteZoo>(new RicetteZooRow(dtProt.Rows[0]));

                int idGruppoPres = await _ricetteZooDal.NuovoId_GruppoRicettaZoo(objP);

                var dtProtRows = await _ricetteZooAgDal.ReadAsync("", 0, dtoProtocollo.IdRicetta, 0, "", objP);
                //if (dtRicZooA != null && dtRicZooA.Rows.Count > 0)
                if (dtProtRows != null && dtProtRows.Rows.Count == 1)
                    foreach (var dtoRowProt in dtProtRows.AsEnumerable().Select(row => _mapper.Map<WriteRicetteZooAgenda>(new RicetteZooAgendaRow(row))))
                    {
                        for (int sommN = 1; sommN <= dtoRowProt.Numero_Somm; sommN++)
                        {
                            var dtoIndTerap = await CreateRicetteZooFromProtocollo(dtoProtocollo, idGruppoPres, objP);                            
                            if (sommN == 1)
                                dtoFirstPres = _mapper.Map<WriteRicetteZoo>(dtoIndTerap);

                            var dtoRicZooAg = await CreateRicZooAgendaFromProtocollo(dtoRowProt, dtoIndTerap.IdRicetta, sommN, objP);
                        }
                    }
            }
            catch (Exception)
            {
                throw;
            }

            return dtoFirstPres;
        }

        #endregion

        #region Creazione/Modifica Somministrazione

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteAgenda> GetDtoAgendaFromAttivitaZoo(Attivita somministrazione, AgronicaCoreParametri objP)
        {
            int lavCod = !string.IsNullOrEmpty(somministrazione.job?.primaryKey?.codice) ? int.Parse(somministrazione.job.primaryKey.codice) : 0;
            WriteAgenda dto = new(somministrazione.centroAziendale.primaryKey.partitaIva, somministrazione.centroAziendale.primaryKey.codice, 0)
            {
                Lav_Cod = lavCod,
                Sta_Num = somministrazione.fabbricatoCod,
                Des_Lib = await _operazioneDal.LavorazioneDesFromLavorazioneCodAsync(lavCod, objP),
                Blocco_Data = AGRODATAINIZIO,
                Validita_Inizio = somministrazione.inizio.Date,
                Validita_Fine = somministrazione.fine.Date,
                Origine = "",
                Inviato = 0
            };

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoFirstRicZoo"></param>
        /// <param name="sommNum"></param>
        /// <param name="idAgenda"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<List<WriteRicetteZooAgenda>> WriteRighePrescrizioneFromSomministrazione(Attivita somministrazione, int gruppoRicetta, int sommNum, int idAgenda, AgronicaCoreParametri objP)
        {
            List<WriteRicetteZooAgenda> result = new();

            if (gruppoRicetta == 0)
                throw new Exception($"Non è stato passato il Gruppo Ricetta. Impossibile proseguire nell'aggiornamento delle Righe di Prescrizione.");

            // Lettura delle testate di Ricetta_Zoo
            var dtRicZoo = await _ricetteZooDal.ReadAsync("", 0, 0, 0, "", objP, GetFiltroAggiuntivoGruppoRic(gruppoRicetta));
            if (dtRicZoo == null || dtRicZoo.Rows.Count == 0)
                throw new Exception($"Testate di Prescrizione non trovate. Impossibile proseguire nell'aggiornamento delle Righe di Prescrizione.");

            List<WriteRicetteZooAgenda> righePrescrizione = new();
            foreach (DataRow row in dtRicZoo.Rows)
            {
                // Lettura Righe della Testata di Prescrizione
                int idRicetta = (int)row["IdRicetta"];
                var dtRicZooAg = await _ricetteZooAgDal.ReadAsync("", 0, idRicetta, 0, "", objP);

                if (dtRicZooAg == null || dtRicZooAg.Rows.Count == 0)
                    throw new Exception($"Non sono state trovate le Righe della Prescrizione {idRicetta} (Id Gruppo {gruppoRicetta}) da associare alla Somministrazione.");

                righePrescrizione.AddRange(dtRicZooAg.AsEnumerable().Select(row => _mapper.Map<WriteRicetteZooAgenda>(new RicetteZooAgendaRow(row))));
            }

            DateTime lastInsertedDate = somministrazione.inizio.Date;
            // Ordina le Righe di Prescrizione (i possibili gruppi) per Numero_Somm
            foreach (var rowRza in righePrescrizione.OrderBy(row => row.Numero_Somm))
            {
                bool isFirst = rowRza.Numero_Somm == 1;

                int intervalloSomm = rowRza.Intervallo_Somm ?? 0;
                rowRza.DataInizioTrattamento = lastInsertedDate;
                rowRza.DataFineTrattamento = lastInsertedDate;
                rowRza.DurataTrattamento = intervalloSomm;

                int idRigaPres = await _ricetteZooAgDal.ScriviModificaAsync(rowRza, objP);
                rowRza.IdAgenda = idRigaPres;

                lastInsertedDate = lastInsertedDate.AddDays(intervalloSomm);

                // Scrittura del link tra Riga di Prescrizione e Somministrazione per la prima somministrazione (operazione di Agenda) 
                if (isFirst)
                {
                    WriteRicetteZooxAgenda dtoRZxA = new(rowRza.IdRicetta, rowRza.IdAgenda, idAgenda)
                    {
                        Validita_Inizio = somministrazione.inizio,
                        Validita_Fine = somministrazione.fine,
                    };
                    await _ricetteZooxAgDal.ScriviModificaAsync(dtoRZxA, objP);
                }

                result.Add(rowRza);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoAgenda"></param>
        /// <param name="cauMov">In base a questo compone un Movimenti per lo Scarico del Farmaco su Animale o da Magazzino</param>
        /// <returns></returns>
        private static WriteMovimenti GetDtoMovimenti(Attivita somministrazione, WriteAgenda dtoAgenda, int cauMov)
        {
            WriteMovimenti dto = new(somministrazione.centroAziendale.primaryKey.partitaIva, dtoAgenda.Id_Agenda, 0)
            {
                Mov_Desc = dtoAgenda.Des_Lib,
                Cau_Mov = cauMov,
                Data_Movimento = somministrazione.inizio.Date,
                Scadenza = AGRODATAFINE,
                Data_Registrazione = DateTime.Now, // TODO Sostituire con Data_Movimento??
                Validita_Inizio = somministrazione.inizio,
                Validita_Fine = AGRODATAFINE
            };

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="agenda"></param>
        /// <param name="cauMov">In base a questo scrive una riga di Movimenti per lo Scarico del Farmaco su Animale o da Magazzino</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteMovimenti> WriteMovimentiFromAttivitaZoo(Attivita somministrazione, WriteAgenda agenda, string cauMov, AgronicaCoreParametri objP)
        {
            WriteMovimenti dtoMov = GetDtoMovimenti(somministrazione, agenda, int.Parse(cauMov));

            var dtMov = await _movimentiDal.ReadAsync(agenda.Piva, agenda.Id_Agenda, 0, objP, GetFiltroAggiuntivoCauMov(cauMov));
            if(dtMov != null && dtMov.Rows.Count == 1)
                dtoMov.Id_Mov = (int)dtMov.Rows[0]["Id_Mov"];

            int idMov = await _movimentiDal.ScriviModificaAsync(dtoMov, objP);
            dtoMov.Id_Mov = idMov;

            return dtoMov;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRicetta">Ricette_Zoo.IdRicetta</param>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco su Animale</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteMovimentiZoo> GetDtoMovimentiZooFromAttivitaZoo(int idRicetta, Attivita somministrazione, WriteMovimenti dtoMov, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            int presTipo = 0;
            string presNum = "";
            string presRigaNum = "";

            int idAgenda = dtoMov.Id_Agenda;
            int idRigaRic = int.Parse(somministrazione.codiceOperazioneRicetta);

            var dtRz = await _prescrizioniDal.ReadPrescrizioniAsync(new(idRicetta), objP_Server, objP_Utenti);
            if (dtRz != null && dtRz.Rows.Count > 0)
            {
                presNum = (string)dtRz.Rows[0]["Numero"];
                presTipo = (int)dtRz.Rows[0]["TipoCodice"];

                var dtRzA = await _prescrizioniDal.ReadRighePrescrizioneAsync(idRicetta, objP_Server);
                if (dtRzA != null && dtRzA.Rows.Count > 0)
                    foreach (var row in dtRzA.Rows)
                        if ((int)dtRzA.Rows[0]["IdAgenda"] == idRigaRic)
                        {
                            presRigaNum = (string)dtRzA.Rows[0]["Numero"];
                            break;
                        }
            }

            WriteMovimentiZoo dto = new(somministrazione.centroAziendale.primaryKey.partitaIva, idAgenda, dtoMov.Id_Mov)
            {
                Pres_Numero = presNum,
                PresRiga_Numero = presRigaNum,
                Tipo_Trattamento = presTipo, 
                Stato_Trattamento = 0, // TODO Aggiungere enum?
                Validita_Inizio = somministrazione.inizio,
                Validita_Fine = AGRODATAFINE,
            };

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRicetta">Ricette_Zoo.IdRicetta</param>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco su Animale</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<bool> WriteMovimentiZooFromAttivitaZoo(int idRicetta, Attivita somministrazione, WriteMovimenti dtoMov, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            WriteMovimentiZoo dtoMovZ = await GetDtoMovimentiZooFromAttivitaZoo(idRicetta, somministrazione, dtoMov, objP_Server, objP_Utenti);
            return await _movimentiZooDal.ScriviModificaAsync(dtoMovZ, objP_Server);
        }

        /// <summary>
        /// Elimina Movimenti e collegate
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task DeleteOldMovimenti(string piva, int idAgenda, AgronicaCoreParametri objP)
        {
            var dtMov = await _movimentiDal.ReadAsync(piva, idAgenda, 0, objP);
            var movimentiToDelete = dtMov.AsEnumerable().Select(row => _mapper.Map<WriteMovimenti>(new MovimentiRow(row))).ToList();

            foreach (var dtoMov in movimentiToDelete)
            {
                // Delete Movimenti_Zoo associati
                await DeleteOldMovimentiZoo(piva, idAgenda, dtoMov.Id_Mov, objP);

                // Delete Movimenti_Dettagli associati
                await DeleteOldMovDettagli(piva, idAgenda, dtoMov.Id_Mov, objP);
            }

            if (movimentiToDelete.Any())
                await _movimentiDal.EliminaAsync(movimentiToDelete, objP);
        }

        /// <summary>
        /// Elimina Movimenti_Zoo
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="idMov"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task DeleteOldMovimentiZoo(string piva, int idAgenda, int idMov, AgronicaCoreParametri objP)
        {
            var dtMovZ = await _movimentiZooDal.ReadAsync(piva, idAgenda, idMov, objP);
            var movimentiZooToDelete = dtMovZ.AsEnumerable().Select(row => _mapper.Map<WriteMovimentiZoo>(new MovimentiZooRow(row))).ToList();
            
            if (movimentiZooToDelete.Any())
                await _movimentiZooDal.EliminaAsync(movimentiZooToDelete, objP);
        }

        /// <summary>
        /// Elimina Movimenti_dettagli e collegate
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="idMov"></param>
        /// <param name="objP"></param>
        private async Task DeleteOldMovDettagli(string piva, int idAgenda, int idMov, AgronicaCoreParametri objP)            
        {
            var dtMovDett = await _movDettagliDal.ReadAsync(piva, idAgenda, idMov, 0, objP);
            var movDettagliToDelete = dtMovDett.AsEnumerable().Select(row => _mapper.Map<WriteMovDettagli>(new MovDettagliRow(row))).ToList();
            
            foreach (var dtoDett in movDettagliToDelete)
            {
                // Delete Mov_Dettaglio_Tecnico associati
                await DeleteOldMovDettTecnico(piva, idAgenda, idMov, dtoDett.Id_Mov_Det, objP);

                // Delete Mov_Destinazioni associati
                await DeleteOldMovDestinazioni(piva, idAgenda, idMov, dtoDett.Id_Mov_Det, objP);
            }

            if (movDettagliToDelete.Any())
                await _movDettagliDal.EliminaAsync(movDettagliToDelete, objP);
        }

        /// <summary>
        /// Elimina Mov_Destinazioni
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="idMov"></param>
        /// <param name="idMovDet"></param>
        /// <param name="objP"></param>
        private async Task DeleteOldMovDestinazioni(string piva, int idAgenda, int idMov, int idMovDet, AgronicaCoreParametri objP)
        {
            var dtMovDest = await _movDestinazioniDal.ReadAsync(piva, idAgenda, idMov, idMovDet, 0, objP);
            var movDesToDelete = dtMovDest.AsEnumerable().Select(row => _mapper.Map<WriteMovDestinazioni>(new MovDestinazioniRow(row))).ToList();

            if (movDesToDelete.Count > 0)
                await _movDestinazioniDal.EliminaAsync(movDesToDelete, objP);
        }

        /// <summary>
        /// Elimina Mov_Dettaglio_Tecnici 
        /// </summary>
        /// <param name="piva"></param>
        /// <param name="idAgenda"></param>
        /// <param name="idMov"></param>
        /// <param name="idMovDet"></param>
        /// <param name="objP"></param>
        private async Task DeleteOldMovDettTecnico(string piva, int idAgenda, int idMov, int idMovDet, AgronicaCoreParametri objP)
        {
            var dtMovDettTec = await _movDettTecnicoDal.ReadAsync(piva, idAgenda, idMov, idMovDet, 0, objP);
            var movDetTecToDelete = dtMovDettTec.AsEnumerable().Select(row => _mapper.Map<WriteMovDettTecnico>(new MovDettTecnicoRow(row))).ToList();

            if (movDetTecToDelete.Count > 0)
                await _movDettTecnicoDal.EliminaAsync(movDetTecToDelete, objP);
        }

        /// <summary>
        /// Aggiunge i dati mancanti dei Dettagli della Somministrazione alla riga di Ricette_Zoo_Dettagli corrispondente
        /// </summary>
        /// <param name="dettagliSomm"></param>
        /// <param name="dtoRza"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteRicetteZooDettagli> UpdateRicetteZooFromDettagliSomm(DettaglioRegistroSomministrazioni dettagliSomm, WriteRicetteZooAgenda dtoRza, AgronicaCoreParametri objP)
        {
            WriteRicetteZooDettagli? dtoRzdtt = null;

            try
            {
                var dtRzm = await _ricetteZooMovDal.ReadAsync("", 0, dtoRza.IdRicetta, dtoRza.IdAgenda, 0, objP);
                if (dtRzm != null && dtRzm.Rows.Count > 0)
                    foreach (DataRow row in dtRzm.Rows)
                    {
                        var rowRzm = _mapper.Map<WriteRicetteZooMovimenti>(new RicetteZooMovimentiRow(row));

                        var dtRzdtt = await _ricetteZooDettDal.ReadAsync("", 0, rowRzm.IdRicetta, rowRzm.IdAgenda, rowRzm.IdMov, 0, objP);
                        if (dtRzdtt != null && dtRzdtt.Rows.Count > 0)
                        {
                            dtoRzdtt = dtRzdtt.AsEnumerable()
                                    .Select(row => _mapper.Map<WriteRicetteZooDettagli>(new RicetteZooDettagliRow(row)))
                                    .Where(row => row.ProdottoAic != "")
                                    .FirstOrDefault(row => dettagliSomm.codiceAIC.StartsWith(row.ProdottoAic));
                            
                            if (dtoRzdtt != null) break;
                        }
                    }

                if (dtoRzdtt == null)
                    throw new Exception($"Non è stata trovata il Dettaglio della Riga {dtoRza.IdAgenda} (Prescrizione {dtoRza.IdRicetta}) da associare alla al Dettaglio della Somministrazione.");

                //Scrivo la famiglia del farmaco, non il farmaco specifico
                dtoRzdtt.Pro_Cod = 0;
                dtoRzdtt.ProdottoAic = dettagliSomm.codiceAIC.Substring(0, 6);

                await _ricetteZooDettDal.ScriviModificaAsync(dtoRzdtt, objP);
            }
            catch (Exception)
            {
                throw;
            }

            return dtoRzdtt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco su Animale</param>
        /// <param name="dettagliSomm"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<WriteMovDettagli> GetDtoMovDettagliFromSomministrazione(Attivita somministrazione, WriteMovimenti dtoMov, DettaglioRegistroSomministrazioni dettagliSomm, AgronicaCoreParametri objP)
        {
            int farmCod = dettagliSomm.prodotto.codice;

            var dtFarmaco = await _farmaciDal.LeggiFarmaciAsync(farmCod, "", objP);
            if (dtFarmaco != null && dtFarmaco.Rows.Count > 0)
            {
                //int selUdm;
                //decimal qtaConv;
                int udmCod = dettagliSomm.unitaDiMisura.codice;

                //switch ((enum_UnitaMisura)udmCod)
                //{
                //    case enum_UnitaMisura.Grammi:
                //        selUdm = udmCod;
                //        udmCod = (int)enum_UnitaMisura.KG; 
                //        qtaConv = dettagliSomm.quantitaTotaleReale / 1000;
                //        break;

                //    case enum_UnitaMisura.Millilitri:
                //        selUdm = udmCod;
                //        udmCod = (int)enum_UnitaMisura.Litri;
                //        qtaConv = dettagliSomm.quantitaTotaleReale / 1000;
                //        break;

                //    default:
                //        selUdm = udmCod;
                //        qtaConv = dettagliSomm.quantitaTotaleReale;
                //        break;
                //}

                WriteMovDettagli dto = new(somministrazione.centroAziendale.primaryKey.partitaIva, dtoMov.Id_Agenda, dtoMov.Id_Mov, 0)
                {
                    Pro_Cod = farmCod,
                    Elem_Cod = ELEM_COD.FARMACI,
                    Mov_Det_Des = $"{dtFarmaco.Rows[0]["Denominazione"]} ({dtFarmaco.Rows[0]["Confezione"]} - {dettagliSomm.quantitaTotaleReale})",
                    Udm_Cod = udmCod,
                    Qta = (double)dettagliSomm.quantitaTotaleReale,
                    Qta_Extra_Totale = (double)dettagliSomm.quantitaTotaleReale,
                    Contabilizzato = DETTAGLI_CONTABILI.NON_CONTABILE,
                    RegSco_Numero = dettagliSomm.regSco_Numero,
                    Rif_Esterno = dettagliSomm.numTrattamento, // Tratt_Numero
                    Extra_Str = dettagliSomm.codice, // Somm_Numero
                    Extra_Int = udmCod,
                    Extra_Date = dettagliSomm.dataPrescrizione,
                    Validita_Inizio = AGRODATAINIZIO,
                    Validita_Fine = AGRODATAFINE,
                };

                return dto;
            }
            else
            {
                throw new Exception("Farmaco non configurato.");
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco su Animale</param>
        /// <param name="dettagliSomm"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteMovDettagli> WriteMovDettagliFromSomministrazione(Attivita somministrazione, WriteMovimenti dtoMov, DettaglioRegistroSomministrazioni dettagliSomm, AgronicaCoreParametri objP)
        {
            WriteMovDettagli dtoMovDett = await GetDtoMovDettagliFromSomministrazione(somministrazione, dtoMov, dettagliSomm, objP);

            var dtMovDett = await _movDettagliDal.ReadAsync(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, 0, objP, 
                GetFiltroAggiuntivoProCod(dtoMovDett.Pro_Cod.GetValueOrDefault(0)));
            if (dtMovDett != null && dtMovDett.Rows.Count == 1)
                dtoMovDett.Id_Mov_Det = (int)dtMovDett.Rows[0]["Id_Mov_Det"];

            int idMovDett = await _movDettagliDal.ScriviModificaAsync(dtoMovDett, objP);
            dtoMovDett.Id_Mov_Det = idMovDett;

            return dtoMovDett;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoMovDett">Movimenti_dettagli legato allo Scarico del Farmaco su Animale</param>
        /// <param name="sospensione"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<int> WriteMovDettTecnicoFromSospensione(WriteMovDettagli dtoMovDett, TempiSospensione sospensione, AgronicaCoreParametri objP)
        {
            WriteMovDettTecnico dtoMovDettTec = new(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, dtoMovDett.Id_Mov_Det, 0)
            {
                Dett_Cod = sospensione.Alimento.codice,
                Extra_Int = sospensione.tempoSospensione,
                Validita_Inizio = AGRODATAINIZIO,
                Validita_Fine = AGRODATAFINE
            };

            var dtMovDettTec = await _movDettTecnicoDal.ReadAsync(dtoMovDettTec.Piva, dtoMovDettTec.Id_Agenda, dtoMovDettTec.Id_Mov, dtoMovDettTec.Id_Mov_Det, 0, objP, GetFiltroAggiuntivoDettCod(dtoMovDettTec.Dett_Cod.GetValueOrDefault(0)));
            if (dtMovDettTec != null && dtMovDettTec.Rows.Count == 1)
                dtoMovDettTec.Id_Reg_Dettaglio = (int)dtMovDettTec.Rows[0]["Id_Reg_Dettaglio"];

            int idMovRegDet = await _movDettTecnicoDal.ScriviModificaAsync(dtoMovDettTec, objP);

            return idMovRegDet;
        }

        /// <summary>
        /// Scrive un record di Ricette_Zoo_Destinazioni con i dati del Capo coinvolto per ogni Riga della Prescrizione 
        /// creata (riferimenti alla prima somministrazione e programmate) poiche' al ribaltamento da Protocollo non sono create
        /// </summary>
        /// <param name="dtosRicZooAg"></param>
        /// <param name="capo"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task WriteRicetteZooDestFromCapo(List<WriteRicetteZooAgenda> dtosRicZooAg, CapoAnimale capo, AgronicaCoreParametri objP)
        {
            // Righe della Prescrizione ribaltata dal Protocollo            
            foreach (WriteRicetteZooAgenda dtoRza in dtosRicZooAg)
            {
                var dtRzm = await _ricetteZooMovDal.ReadAsync(dtoRza.Piva, dtoRza.Sa_Cod ?? 0, dtoRza.IdRicetta, dtoRza.IdAgenda, 0, objP);
                if (dtRzm == null || dtRzm.Rows.Count == 0)
                    throw new Exception($"Non sono state trovate i record di Ricette_Zoo_Movimenti della Prescrizione {dtoRza.IdRicetta} (Prescrizione ribaltata dal Protocollo di origine).");

                foreach (var dtoRzm in dtRzm.AsEnumerable().Select(row => _mapper.Map<WriteRicetteZooMovimenti>(new RicetteZooMovimentiRow(row))))
                {
                    var dtRzdtt = await _ricetteZooDettDal.ReadAsync(dtoRzm.Piva, dtoRzm.Sa_Cod ?? 0, dtoRzm.IdRicetta, dtoRzm.IdAgenda, dtoRzm.IdMov, 0, objP);
                    if (dtRzdtt == null || dtRzdtt.Rows.Count == 0)
                        throw new Exception($"Non sono state trovate le righe di Dettagli della Prescrizione {dtoRza.IdRicetta} (Prescrizione ribaltata dal Protocollo di origine).");

                    foreach (var dtoRzdtt in dtRzdtt.AsEnumerable().Select(row => _mapper.Map<WriteRicetteZooDettagli>(new RicetteZooDettagliRow(row))))
                    {
                        WriteRicetteZooDestinazioni dtoRzdest = new(dtoRzdtt.Piva, dtoRzdtt.Sa_Cod ?? 0, dtoRzdtt.IdRicetta, dtoRzdtt.IdAgenda, dtoRzdtt.IdMov, dtoRzdtt.IdDettaglio)
                        {
                            IdDestinazione = 0,
                            CodAnimale = capo.codice,
                            Matricola = capo.matricola,
                            Sesso = capo.sesso,
                            Validita_Fine = AGRODATAFINE
                        };
                        int idRzdst = await _ricetteZooDestDal.ScriviModificaAsync(dtoRzdest, objP);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dettagliSomm"></param>
        /// <param name="capoCDC"></param>
        /// <param name="dtoMovDett">Movimenti_dettagli legato allo Scarico del Farmaco su Animale</param>
        /// <param name="dtosRicZooAg"></param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<int> WriteMovDestinazioneFromCapo(Attivita somministrazione, DettaglioRegistroSomministrazioni dettagliSomm, CapoAnimaleCDC capoCDC, WriteMovDettagli dtoMovDett, List<WriteRicetteZooAgenda>? dtosRicZooAg, AgronicaCoreParametri objP)
        {
            CapoAnimale capo = capoCDC.capoAnimale;
            double sommPercentage = Math.Round(((double)((decimal)capoCDC.qtaSomministrata / (decimal)dtoMovDett.Qta.GetValueOrDefault(0))), 2);

            WriteMovDestinazioni dtoMovDest = new(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, dtoMovDett.Id_Mov_Det, capo.codice)
            {
                Tipo_Destinazione = TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE,
                Qta = capoCDC.qtaSomministrata,
                Validita_Inizio = somministrazione.inizio,
                Validita_Fine = AGRODATAFINE,
                Mov_Destinazioni_GraphicKey = capo.matricola,
                QuotaDistribuzione = sommPercentage
            };

            await _movDestinazioniDal.ScriviModificaAsync(dtoMovDest, objP);

            // Caso scrittura prima Somministrazione da Protocollo: necessario aggiungere le righe di Ricette_Zoo_Destinazioni poichè mancanti
            if (dtosRicZooAg != null)
                await WriteRicetteZooDestFromCapo(dtosRicZooAg, capo, objP);

            return dtoMovDest.Id_Destinazione;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco da Magazzino</param>
        /// <param name="scaricoProd"></param>
        /// <param name="dettagliSomm">Dettagli della Somministrazioni legati al Farmaco</param>
        /// <param name="movMagazzino">Movimento di Magazzino</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<WriteMovDettagli> GetDtoMovDettagliFromScaricoProd(Attivita somministrazione, WriteMovimenti dtoMov, RisorsaProdotto scaricoProd, DettaglioRegistroSomministrazioni dettagliSomm, RilevamentoDiMagazzino movMagazzino, AgronicaCoreParametri objP)
        {
            int farmCod = scaricoProd.prodotto.codice;
            var dtFarmaco = await _farmaciDal.LeggiFarmaciAsync(farmCod, "", objP);
            if (dtFarmaco != null && dtFarmaco.Rows.Count > 0)
            {
                WriteMovDettagli dto = new(somministrazione.centroAziendale.primaryKey.partitaIva, dtoMov.Id_Agenda, dtoMov.Id_Mov, 0)
                {
                    Sa_Cod = somministrazione.centroAziendale.primaryKey.codice,
                    Pro_Cod = farmCod,
                    Elem_Cod = ELEM_COD.FARMACI,
                    Mov_Det_Des = $"{dtFarmaco.Rows[0]["Denominazione"]} ({dtFarmaco.Rows[0]["Confezione"]} - {movMagazzino.Qta:F2})",
                    Udm_Cod = (int)movMagazzino.udm.codice,
                    Qta = (double)movMagazzino.Qta,
                    Qta_Extra_Totale = (double)movMagazzino.QtaTot,
                    Contabilizzato = DETTAGLI_CONTABILI.NON_CONTABILE,
                    Rif_Esterno = dettagliSomm.numTrattamento, // Tratt_Numero
                    Extra_Str = scaricoProd.prodotto.codice_alfanumerico, // AIC
                    Extra_Int = (int)movMagazzino.QtaTot, // (int)somministrazione.quantitaTotaleReale,
                    Lotto = movMagazzino.Lotto,
                    Validita_Inizio = AGRODATAINIZIO,
                    Validita_Fine = AGRODATAFINE,
                };

                return dto;
            }
            else
            {
                throw new Exception("Farmaco non configurato.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMov">Movimenti legato allo Scarico del Farmaco da Magazzino</param>
        /// <param name="scaricoProd">Dettagli della Somministrazioni legati al Farmaco</param>
        /// <param name="dettagliSomm"></param>
        /// <param name="movMagazzino">Movimento di Magazzino</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private async Task<WriteMovDettagli> WriteMovDettagliFromScaricoProd(Attivita somministrazione, WriteMovimenti dtoMov, RisorsaProdotto scaricoProd, RilevamentoDiMagazzino movMagazzino, DettaglioRegistroSomministrazioni dettagliSomm, AgronicaCoreParametri objP)
        {
            WriteMovDettagli dtoMovDett = await GetDtoMovDettagliFromScaricoProd(somministrazione, dtoMov, scaricoProd, dettagliSomm, movMagazzino, objP);

            var dtMovDett = await _movDettagliDal.ReadAsync(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, 0, objP, GetFiltroAggiuntivoProCod((int)dtoMovDett.Pro_Cod.GetValueOrDefault(0)));
            if (dtMovDett != null && dtMovDett.Rows.Count == 1)
                dtoMovDett.Id_Mov_Det = (int)dtMovDett.Rows[0]["Id_Mov_Det"];

            int idMovDett = await _movDettagliDal.ScriviModificaAsync(dtoMovDett, objP);
            dtoMovDett.Id_Mov_Det = idMovDett;

            return dtoMovDett;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attivita">Attivita collegata all'Attivita di tipo Ricetta</param>
        /// <param name="dtoMovDett">Movimenti_dettagli legato allo Scarico del Farmaco da Magazzino</param>
        /// <param name="movMagazzino">Movimento di Magazzino</param>
        /// <param name="objP"></param>
        /// <returns></returns>
        private static WriteMovDestinazioni GetDtoMovDestinazioneFromScaricoProd(Attivita attivita, WriteMovDettagli dtoMovDett, RilevamentoDiMagazzino movMagazzino, AgronicaCoreParametri objP)
        {
            Fabbricato magazzino = movMagazzino.Magazzino;

            WriteMovDestinazioni dto = new(dtoMovDett.Piva, dtoMovDett.Id_Agenda, dtoMovDett.Id_Mov, dtoMovDett.Id_Mov_Det, magazzino.primaryKey.codice)
            {
                Sa_Cod = magazzino.primaryKey.centroAziendalePK.codice,
                Tipo_Destinazione = magazzino.tipo,
                Qta = dtoMovDett.Qta,
                Validita_Inizio = attivita.inizio,
                Validita_Fine = AGRODATAFINE,
            };

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione"></param>
        /// <param name="objP_Server"></param>
        /// <param name="objP_Utenti"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<ScriviModificaSomministrazione_Resp> ConfermaSomministrazioneFutura(Attivita somministrazione, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            var resp = new ScriviModificaSomministrazione_Resp();

            int idPres = ParseRequiredInt(nameof(Attivita), nameof(somministrazione.codiceOperazioneRicetta), somministrazione.codiceOperazioneRicetta, " Impossibile creare/modificare la Somministrazione.");
           
            // Lettura Riga della Prescrizione legata alla Somministrazione futura
            var dtRicZooAg = await _ricetteZooAgDal.ReadAsync("", 0, idPres, 0, "", objP_Server);
            if (dtRicZooAg == null || dtRicZooAg.Rows.Count == 0)
                throw new Exception($"Non è stata trovata la Riga di Prescrizione legata alla Somministrazione {idPres}.");
            else if (dtRicZooAg.Rows.Count > 1)
                throw new Exception($"Sono state trovate più Righe di Prescrizione legate alla Somministrazione {idPres}.");
            var dtoRicZA = _mapper.Map<WriteRicetteZooAgenda>(new RicetteZooAgendaRow(dtRicZooAg.Rows[0]));

            int idRigaPres = dtoRicZA.IdAgenda;

            // Scrittura Agenda
            WriteAgenda dtoSommAgenda = await GetDtoAgendaFromAttivitaZoo(somministrazione, objP_Server);
            dtoSommAgenda.Id_Agenda = await _agendaDal.ScriviModificaAsync(dtoSommAgenda, objP_Server);

            // Scrittura link Ricette_ZooxAgenda
            WriteRicetteZooxAgenda dtoRZxA = new(dtoRicZA.IdRicetta, dtoRicZA.IdAgenda, dtoSommAgenda.Id_Agenda)
            {
                Validita_Inizio = somministrazione.inizio,
                Validita_Fine = somministrazione.fine,
            };
            await _ricetteZooxAgDal.ScriviModificaAsync(dtoRZxA, objP_Server);

            // Scrittura Movimenti - Scarico del Farmaco sul Capo
            WriteMovimenti dtoMov = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoSommAgenda, CAU_MOV.CAU_TRATTAMENTO_ZOO, objP_Server);

            // Scrittura testata (Mov Zoo) per dati della Prescrizione
            bool resMovZ = await WriteMovimentiZooFromAttivitaZoo(idPres, somministrazione, dtoMov, objP_Server, objP_Utenti);

            foreach (var risorsa in somministrazione.risorse)
            {
                /*** Dati relativi allo scarico del farmaco sul capo ***/
                if (risorsa is DettaglioRegistroSomministrazioni dettaglioSomm)
                {
                    // scrittura riga di Movimenti_dettagli (dettagli del farmaco)
                    WriteMovDettagli dtoMovDett = await WriteMovDettagliFromSomministrazione(somministrazione, dtoMov, dettaglioSomm, objP_Server);

                    // Scrittura Mov_Dettaglio_Tecnico (tempi sospensione per alimento)  
                    if (dettaglioSomm.sospensione != null)
                        foreach (TempiSospensione sosp in dettaglioSomm.sospensione)
                            await WriteMovDettTecnicoFromSospensione(dtoMovDett, sosp, objP_Server);

                    // Scrittura Mov_Destinazioni e Ricette_Zoo_Destinazioni (dati del capo)
                    foreach (CapoAnimaleCDC capoCDC in somministrazione.centriDiCosto.Where(cdc => cdc.classType == nameof(CapoAnimaleCDC)).OfType<CapoAnimaleCDC>())
                        await WriteMovDestinazioneFromCapo(somministrazione, dettaglioSomm, capoCDC, dtoMovDett, null, objP_Server);

                }
                /*** Dati relativi allo scarico del farmaco dalle giacenze di magazzino ***/
                else if (risorsa is RisorsaProdotto scaricoProd)
                {
                    // Verifica se esiste la somministrazione (scarico) animale da associare allo scarico di magazzino
                    if (somministrazione.risorse.Find(r => r is DettaglioRegistroSomministrazioni somm && somm.prodotto.codice == scaricoProd.prodotto.codice)
                        is DettaglioRegistroSomministrazioni dettSomm)
                    {
                        // Scrittura Movimenti Scarico 
                        WriteMovimenti dtoMovScarico = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoSommAgenda, CAU_MOV.CAU_SCARICO, objP_Server);

                        foreach (RilevamentoDiMagazzino movMagazzino in scaricoProd.MagazziniMovimentazioni)
                        {
                            // Scrittura Movimenti_dettagli Scarico
                            WriteMovDettagli dtoMovDettScarico = await WriteMovDettagliFromScaricoProd(somministrazione, dtoMovScarico, scaricoProd, movMagazzino, dettSomm, objP_Server);

                            // Scrittura Mov_Destinazioni Scarico
                            WriteMovDestinazioni dtoMovDestScarico = GetDtoMovDestinazioneFromScaricoProd(somministrazione, dtoMovDettScarico, movMagazzino, objP_Server);
                            int idDestScarico = await _movDestinazioniDal.ScriviModificaAsync(dtoMovDestScarico, objP_Server);
                        }
                    }
                }
            }

            resp.Id_Somministrazione = dtoSommAgenda.Id_Agenda;
            resp.Id_Prescrizione = idPres;
            resp.Id_Riga_Prescrizione = idRigaPres;

            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="somministrazione"></param>
        /// <param name="idAgenda"></param>
        /// <param name="objP_Server"></param>
        /// <param name="objP_Utenti"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<ScriviModificaSomministrazione_Resp> UpdateSomministrazione(Attivita somministrazione, int idAgenda, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            var resp = new ScriviModificaSomministrazione_Resp();

            // Modifica Agenda
            var dtAgenda = await _agendaDal.ReadAsync("", idAgenda, objP_Server);
            if (dtAgenda == null || dtAgenda.Rows.Count == 0)
                throw new Exception($"Operazione di Agenda {idAgenda} non trovata.");
            if (dtAgenda.Rows.Count != 1)
                throw new Exception($"Piu' di un'Operazione trovata per Id_Agenda = {idAgenda}.");

            var dtoAgenda = _mapper.Map<WriteAgenda>(new AgendaRow(dtAgenda.Rows[0]));
            dtoAgenda.Validita_Inizio = somministrazione.inizio.Date;
            dtoAgenda.Validita_Fine = somministrazione.fine.Date;
            await _agendaDal.ScriviModificaAsync(dtoAgenda, objP_Server);

            // TODO Valutare se necessario: caso di prima somministrazione potrebbe essere necessario
            // Lettura legame Somministrazione - Prescrizione
            var dtRicZxA = await _ricetteZooxAgDal.ReadAsync(0, 0, idAgenda, objP_Server);
            if (dtRicZxA == null || dtRicZxA.Rows.Count == 0)
                throw new Exception($"Non è stata trovata la Riga di Prescrizione legata alla Somministrazione {idAgenda}.");
            else if (dtRicZxA.Rows.Count > 1)
                throw new Exception($"Sono state trovate più Righe di Prescrizione legate alla Somministrazione {idAgenda}.");

            var dtoRicZxA = _mapper.Map<WriteRicetteZooxAgenda>(new RicetteZooxAgendaRow(dtRicZxA.Rows[0]));
            int idPres = dtoRicZxA.Id_Ricetta;
            int idRigaPres = dtoRicZxA.Id_RigaRicetta;

            /** Logica di Modifica: Elimina vecchi record da Movimenti in poi e crea i nuovi record come per la creazione **/

            // Elimina le righe di tabelle Agenda collegate (Movimenti, Movimenti_dettagli, Mov_Destinazioni e Mov_Dettaglio_Tecnico) 
            await DeleteOldMovimenti(dtoAgenda.Piva, dtoAgenda.Id_Agenda, objP_Server);

            // Scrittura Movimenti - Scarico del Farmaco sul Capo
            WriteMovimenti dtoMov = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoAgenda, CAU_MOV.CAU_TRATTAMENTO_ZOO, objP_Server);

            // Scrittura testata (Mov Zoo) per dati della Prescrizione
            bool resMovZ = await WriteMovimentiZooFromAttivitaZoo(idPres, somministrazione, dtoMov, objP_Server, objP_Utenti);

            foreach (var risorsa in somministrazione.risorse)
            {
                /*** Dati relativi allo scarico del farmaco sul capo ***/
                if (risorsa is DettaglioRegistroSomministrazioni dettaglioSomm)
                {
                    // scrittura riga di Movimenti_dettagli (dettagli del farmaco)
                    WriteMovDettagli dtoMovDett = await WriteMovDettagliFromSomministrazione(somministrazione, dtoMov, dettaglioSomm, objP_Server);

                    // Scrittura Mov_Dettaglio_Tecnico (tempi sospensione per alimento)  
                    if (dettaglioSomm.sospensione != null)
                        foreach (TempiSospensione sosp in dettaglioSomm.sospensione)
                            await WriteMovDettTecnicoFromSospensione(dtoMovDett, sosp, objP_Server);

                    // Scrittura Mov_Destinazioni e Ricette_Zoo_Destinazioni (dati del capo)
                    foreach (CapoAnimaleCDC capoCDC in somministrazione.centriDiCosto.Where(cdc => cdc.classType == nameof(CapoAnimaleCDC)).OfType<CapoAnimaleCDC>())
                        await WriteMovDestinazioneFromCapo(somministrazione, dettaglioSomm, capoCDC, dtoMovDett, null, objP_Server);

                }
                /*** Dati relativi allo scarico del farmaco dalle giacenze di magazzino ***/
                else if (risorsa is RisorsaProdotto scaricoProd)
                {
                    // Verifica se esiste la somministrazione (scarico) animale da associare allo scarico di magazzino
                    if (somministrazione.risorse.Find(r => r is DettaglioRegistroSomministrazioni somm && somm.prodotto.codice == scaricoProd.prodotto.codice)
                        is DettaglioRegistroSomministrazioni dettSomm)
                    {
                        // Scrittura Movimenti Scarico 
                        WriteMovimenti dtoMovScarico = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoAgenda, CAU_MOV.CAU_SCARICO, objP_Server);

                        foreach (RilevamentoDiMagazzino movMagazzino in scaricoProd.MagazziniMovimentazioni)
                        {
                            // Scrittura Movimenti_dettagli Scarico
                            WriteMovDettagli dtoMovDettScarico = await WriteMovDettagliFromScaricoProd(somministrazione, dtoMovScarico, scaricoProd, movMagazzino, dettSomm, objP_Server);

                            // Scrittura Mov_Destinazioni Scarico
                            WriteMovDestinazioni dtoMovDestScarico = GetDtoMovDestinazioneFromScaricoProd(somministrazione, dtoMovDettScarico, movMagazzino, objP_Server);
                            int idDestScarico = await _movDestinazioniDal.ScriviModificaAsync(dtoMovDestScarico, objP_Server);
                        }
                    }
                }
            }

            resp.Id_Somministrazione = dtoAgenda.Id_Agenda;
            resp.Id_Prescrizione = idPres;
            resp.Id_Riga_Prescrizione = idRigaPres;

            return resp;
        }

        #endregion

        public async Task<SomministrazioneProdotti> LeggiSomministrazioneProdottiAsync(string sommNumero, AgronicaCoreParametri objParams)
        {
            SomministrazioneProdotti somministrazioneProdotti = new SomministrazioneProdotti();

            try
            {
                if (sommNumero == "0")
                {
                    somministrazioneProdotti.result = await _farmaciDal.LeggiFarmaciAsync(0, "", objParams);
                    somministrazioneProdotti.kendoColumns = new List<KendoColumn>
                    {
                        new KendoColumn { Field = "AIC", Hidden = false, Title = "AIC", DataType = "string" },
                        new KendoColumn { Field = "Confezione", Hidden = false, Title = "Confezione", DataType = "string" },
                        new KendoColumn { Field = "Denominazione", Hidden = false, Title = "Denominazione", DataType = "string" },
                        new KendoColumn { Field = "Codice_GTIN", Hidden = false, Title = "CodiceGTIN", DataType = "string" },
                        new KendoColumn { Field = "ModalitaPrescrizione", Hidden = false, Title = "ModalitaPresc", DataType = "string" },
                        new KendoColumn { Field = "InfoAggiuntive", Hidden = false, Title = "InfoAggiuntive", DataType = "string" }
                    };
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
            return somministrazioneProdotti;
        }

        public async Task<ITrattamentoResult> CreaTrattamentoDaProtocollo(int Id_Protocollo, List<Attivita> somministrazioni, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            var resp = new CreaTrattamentoDaProtocollo_Resp();

            try
            {
                await OpenConnectionAsync(objP_Server, true, IsolationLevel.ReadUncommitted);
                //await OpenTransactionAsync(objP);

                // Ribalta il Protocollo in x (Ricette_Zoo_Agenda.Numero_Somm) nuove testate di Prescrizione (Ricette_Zoo);
                // la prima viene legata alla nuova Somministrazione
                var dtoRicZoo = await ProjectProtocolloToIndTerapeutica(Id_Protocollo, objP_Server);

                int gruppoPrescrizione = dtoRicZoo.Gruppo_Ricetta ?? 0;
                int idPrescrizione = dtoRicZoo.IdRicetta;

                // Per ora si suppone che il Protocollo abbia sempre e solo un Prodotto
                // Scrive per Data Inizio crescente le Somministrazioni passate
                int sommNum = 1;
                foreach (Attivita somministrazione in somministrazioni.OrderBy(act => act.inizio))
                {
                    // Scrittura Agenda
                    WriteAgenda dtoSommAgenda = await GetDtoAgendaFromAttivitaZoo(somministrazione, objP_Server);
                    dtoSommAgenda.Id_Agenda = await _agendaDal.ScriviModificaAsync(dtoSommAgenda, objP_Server);

                    // Aggiorna le date di inizio e fine di ogni Riga di Prescrizione (Ricette_Zoo_Agenda) , in base ai campi Intervallo_Somm e Numero_Somm
                    var dtoRicZooAgs = await WriteRighePrescrizioneFromSomministrazione(somministrazione, gruppoPrescrizione, sommNum, dtoSommAgenda.Id_Agenda, objP_Server);
                    somministrazione.codiceOperazioneRicetta = dtoRicZooAgs[0].IdAgenda.ToString();

                    // Scrittura Movimenti - Scarico del Farmaco sul Capo
                    WriteMovimenti dtoMov = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoSommAgenda, CAU_MOV.CAU_TRATTAMENTO_ZOO, objP_Server);

                    // Scrittura testata (Mov Zoo) per dati della Prescrizione
                    bool resMovZ = await WriteMovimentiZooFromAttivitaZoo(dtoRicZoo.IdRicetta, somministrazione, dtoMov, objP_Server, objP_Utenti);

                    // OLD Elimina le righe di tabelle Agenda collegate (Movimenti, Movimenti_dettagli, Mov_Destinazioni e Mov_Dettaglio_Tecnico) 
                    //await DeleteOldMovimenti(dtoSommAgenda.Piva, dtoSommAgenda.Id_Agenda, objP);
                    
                    foreach (var risorsa in somministrazione.risorse)
                    {
                        /*** Dati relativi allo scarico del farmaco sul capo ***/
                        if (risorsa is DettaglioRegistroSomministrazioni dettaglioSomm)
                        {
                            // TODO Valutare se necessario 
                            //if (string.IsNullOrEmpty(dettaglioSomm.codiceAIC)) continue;

                            // OLD Update di Ricette_Zoo_Dettagli
                            // farmCod = somm.prodotto.codice 
                            //List<WriteRicetteZooDettagli> dtoRicZooDttArr = new List<WriteRicetteZooDettagli>();
                            //foreach (WriteRicetteZooAgenda dtoRicZooAgenda in dtoRicZooAg)
                            //{
                            //    WriteRicetteZooDettagli dtoRicZooDtt = await UpdateRicetteZooFromDettagliSomm(dettaglioSomm, dtoRicZooAgenda, objP);
                            //    dtoRicZooDttArr.Add(dtoRicZooDtt);
                            //}

                            // Scrittura Movimenti_dettagli (dettagli del farmaco) 
                            WriteMovDettagli dtoMovDett = await WriteMovDettagliFromSomministrazione(somministrazione, dtoMov, dettaglioSomm, objP_Server);

                            // Scrittura Mov_Dettaglio_Tecnico (tempi sospensione per alimento)  
                            if (dettaglioSomm.sospensione != null)
                                foreach (TempiSospensione sosp in dettaglioSomm.sospensione)
                                    await WriteMovDettTecnicoFromSospensione(dtoMovDett, sosp, objP_Server);

                            // Scrittura Mov_Destinazioni e Ricette_Zoo_Destinazioni (dati del capo)
                            foreach (CapoAnimaleCDC capoCDC in somministrazione.centriDiCosto.Where(cdc => cdc.classType == nameof(CapoAnimaleCDC)).OfType<CapoAnimaleCDC>())
                                await WriteMovDestinazioneFromCapo(somministrazione, dettaglioSomm, capoCDC, dtoMovDett, dtoRicZooAgs, objP_Server);

                        }
                        /*** Dati relativi allo scarico del farmaco dalle giacenze di magazzino ***/
                        else if (risorsa is RisorsaProdotto scaricoProd)
                        {
                            // Verifica se esiste la somministrazione (scarico) animale da associare allo scarico di magazzino
                            if (somministrazione.risorse.Find(r => r is DettaglioRegistroSomministrazioni somm && somm.prodotto.codice == scaricoProd.prodotto.codice) 
                                is DettaglioRegistroSomministrazioni dettSomm)
                            {
                                // Scrittura Movimenti Scarico 
                                WriteMovimenti dtoMovScarico = await WriteMovimentiFromAttivitaZoo(somministrazione, dtoSommAgenda, CAU_MOV.CAU_SCARICO, objP_Server);

                                foreach (RilevamentoDiMagazzino movMagazzino in scaricoProd.MagazziniMovimentazioni)
                                {
                                    // Scrittura Movimenti_dettagli Scarico
                                    WriteMovDettagli dtoMovDettScarico = await WriteMovDettagliFromScaricoProd(somministrazione, dtoMovScarico, scaricoProd, movMagazzino, dettSomm, objP_Server);

                                    // Scrittura Mov_Destinazioni Scarico
                                    WriteMovDestinazioni dtoMovDestScarico = GetDtoMovDestinazioneFromScaricoProd(somministrazione, dtoMovDettScarico, movMagazzino, objP_Server);
                                    int idDestScarico = await _movDestinazioniDal.ScriviModificaAsync(dtoMovDestScarico, objP_Server);
                                }
                            }
                        }
                    }

                    sommNum++;
                    resp.Somministrazioni.Add(dtoSommAgenda.Id_Agenda);
                }

                resp.Gruppo_Prescrizione = gruppoPrescrizione;
                resp.Id_Prescrizione = idPrescrizione;
            }
            catch (Exception ex)
            {
                CloseTransaction(objP_Server, true);
                LogError(ex.Message, objP_Server, ex);
                throw;
            }
            finally
            {
                CloseConnection(objP_Server);
            }

            return resp;
        }
        
        public async Task<ITrattamentoResult> ScriviModificaSomministrazione(Attivita somministrazione, AgronicaCoreParametri objP_Server, AgronicaCoreParametri objP_Utenti)
        {
            var resp = new ScriviModificaSomministrazione_Resp();

            try
            {
                await OpenConnectionAsync(objP_Server, true, IsolationLevel.ReadUncommitted);

                int idAgenda = ParseRequiredInt(nameof(Attivita), nameof(somministrazione.codice), somministrazione.codice, " Impossibile creare/modificare la Somministrazione.");
                
                if (idAgenda == 0)
                    /*** Conferma di una Somministrazione futura ***/
                    resp = await ConfermaSomministrazioneFutura(somministrazione, objP_Server, objP_Utenti);
                else
                    /*** Modifica di una Somministrazione esistente ***/
                    resp = await UpdateSomministrazione(somministrazione, idAgenda, objP_Server, objP_Utenti);
            }
            catch (Exception ex)
            {
                CloseTransaction(objP_Server, true);
                LogError(ex.Message, objP_Server, ex);
                throw;
            }
            finally
            {
                CloseConnection(objP_Server);
            }

            return resp;
        }

        public async Task<bool> EliminaSomministrazione(DeleteSomministrazione dtoDelete, AgronicaCoreParametri objP)
        {
            bool result = false;

            // Somministrazione futura
            if (dtoDelete.Programmata)
            {
                if (dtoDelete.Id_Ricetta == null || dtoDelete.Id_Ricetta == 0)
                    throw new Exception($"La Somministrazione da eliminare è programmata, è necessario valorizzare Id_Ricetta.");
                if (dtoDelete.Id_Riga_Ricetta == null || dtoDelete.Id_Riga_Ricetta == 0)
                    throw new Exception($"La Somministrazione da eliminare è programmata, è necessario valorizzare Id_Riga_Ricetta.");

                int idRicetta = dtoDelete.Id_Ricetta ?? 0;
                int idRigaRicetta = dtoDelete.Id_Riga_Ricetta ?? 0;

                var dtRicZooxAg = await _ricetteZooxAgDal.ReadAsync(idRicetta, idRigaRicetta, 0, objP);
                if (dtRicZooxAg != null && dtRicZooxAg.Rows.Count > 0)
                    throw new Exception($"La Somministrazione selezionata è già stata confermata.");

                var dtRicZoo = await _ricetteZooDal.ReadAsync("", 0, 0, idRicetta, "", objP);
                if (dtRicZoo == null || dtRicZoo.Rows.Count == 0)
                    throw new Exception($"Testata di Prescrizione non trovata (Id_Ricetta = {idRicetta}).");
                var dtoRicZoo = _mapper.Map<WriteRicetteZoo>(new RicetteZooRow(dtRicZoo.Rows[0]));

                if (dtoRicZoo.TipoCodice != (int)enum_TipoPrescrizione.Da_Protocollo_GIAS)
                    throw new Exception($"La Prescrizione selezionata non è del tipo corretto (Id_Ricetta = {idRicetta}, Tipo = {dtoRicZoo.TipoCodice}).");

                // Elimina i record di Ricette_Zoo e associate che rappresentano la Somministrazione futura
                result = await _ricetteZooDal.EliminaAssociateAsync(dtoRicZoo, objP);

            }
            // Somministrazione confermata
            else
            {
                if (dtoDelete.Id_Agenda == null || dtoDelete.Id_Agenda == 0)
                    throw new Exception($"La Somministrazione da eliminare è confermata, è necessario valorizzare Id_Agenda.");

                var dtAgenda = await _agendaDal.ReadAsync("", dtoDelete.Id_Agenda ?? 0, objP);
                if (dtAgenda == null || dtAgenda.Rows.Count == 0)
                    throw new Exception($"Somminstrazione non trovata (Id_Agenda = {dtoDelete.Id_Agenda}).");
                var dtoAgenda = _mapper.Map<WriteAgenda>(new AgendaRow(dtAgenda.Rows[0]));

                result = await _agendaDal.EliminaAssociateAsync(dtoAgenda, objP);
            }

            return result;
        }

        #endregion
    }
}
