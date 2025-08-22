using AgronicaCoreDTOStd.InData.Agenda.OperazioneAgenda_Temp;
using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreModelsSTD.attivita.dettagli;
using AgronicaCoreModelsSTD.attivita.Info;
using AgronicaCoreModelsSTD.attivita.risorse;
using AgronicaCoreModelsSTD.costanti;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Operazione.BIZ.Resources;
using AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni.Factory;
using AgronicaNetCore.Operazione.DAL.Resources.UtilityAgenda;
using InData.Anagrafica;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli.Factory
{
    public class MovDettagliFactory : BaseServiceOperazioneBIZ, IMovDettagliFactory
    {
        private readonly IUtilityAgenda _utils;
        private readonly IMovDestinazioniFactory _movDestinazioniFactory;

        public MovDettagliFactory(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _utils = provider.GetRequiredService<IUtilityAgenda>();
            _movDestinazioniFactory = provider.GetRequiredService<IMovDestinazioniFactory>();
        }

        private Movimento_Dettaglio GetBaseMov(OperazioneAgenda agenda, InfoOperazione info)
        {
            Movimento_Dettaglio baseMov = new();
            baseMov.Id_Agenda = agenda.Id_Agenda;
            baseMov.Piva = agenda.Piva;
            baseMov.Sa_Cod = agenda.Sa_Cod;
            baseMov.Data = agenda.Data;
            baseMov.Lav_Cod = agenda.Lav_Cod;
            baseMov.Elem_Cod = info.Elem_Cod;
            baseMov.Contabilizzato = CostantiPersonalizzate.NONCONTABILE;
            baseMov.BaseCode = info.BaseCode;
            baseMov.TopCode = info.BaseCode;
            baseMov.Mezzo_Det = -1;
            return baseMov;
        }

        private List<Movimento_Dettaglio> GetMovDettaglioRilievo()
        {
            throw new NotImplementedException();
        }

        private bool HandleProductDetails(Attivita attivita, List<Movimento_Dettaglio> details, List<RisorsaProdotto> resources, InfoOperazione info)
        {
            if (!resources.Any()) return false;

            bool found = false;

            if (info.IsRaccolta && attivita.tipoRaccolta != Attivita.Tipo_Raccolta.Fast &&
                (((DettaglioRaccolta)resources.First()).Opzioni_Raccolta.GenerazioneLotto == Opzioni_Raccolta.enum_Generazione_Lotto_Raccolta.DA_ESERCIZO ||
                ((DettaglioRaccolta)resources.First()).Opzioni_Raccolta.GenerazioneLotto == Opzioni_Raccolta.enum_Generazione_Lotto_Raccolta.MANUALE))
            {
                throw new NotImplementedException();
            }

            IEnumerable<RisorsaProdotto> productResources = resources.Where(res => res.prodotto != null && res.prodotto.codice != 0);
            foreach (var resource in productResources)
            {
                throw new NotImplementedException();
            }
            return found;
        }

        private void HandleIrrigationDetails() { /* TODO */ }

        private void HandleVisitDetails() { /* TODO */ }

        private void HandleResourceWithoutProduct(
            List<Movimento_Dettaglio> movimentiDettagli, bool foundResouce,
            OperazioneAgenda agenda, InfoOperazione info)
        {
            Movimento_Dettaglio movDett = GetBaseMov(agenda, info);
            if (info.IsLavorazione || info.isNonUtilizzo || info.IsAbbattimento)
            {
                movimentiDettagli.Add(movDett);
            }
            if (info.IsSemina && !foundResouce)
            {
                throw new NotImplementedException();
            }
            if (info.IsRaccolta && !foundResouce)
            {
                movDett.Mov_Det_Des = _localizer.GetString("DettagliProdottiAziendaliRaccolta");
                movDett.Pendente = 3;
                movDett.Anno = 1900;
                movDett.Lotto = "";
                if (movDett.Cal_Cod == null)
                    movDett.Cal_Cod = 0;
                movimentiDettagli.Add(movDett);
            }
        }

        private Dictionary<(int, string, string, string), QuantitaSuImpianto> RepartHarvestedQuantity(
            List<DettaglioRaccolta> products, Func<QuantitaSuImpianto, string> lottoMapper)
        {
            Dictionary<(int, string, string, string), QuantitaSuImpianto> dict = new();
            foreach (var resouce in products)
            {
                int productCode = resouce.prodotto.codice;
                decimal totalArea = resouce.QuantitaSuImpianti.Sum(q => q.esercizioCDC.superficieTrattata);
                decimal totalPlants = resouce.QuantitaSuImpianti.Sum(q => 
                    q.esercizioCDC.superficieTrattata * (decimal)q.esercizioCDC.esercizio.piante_Ha);
                string lastLot = "";
                decimal qta;
                foreach (var plant in resouce.QuantitaSuImpianti)
                {
                    string esercizioK = plant.esercizioCDC.esercizio.GetKey("_");
                    string magazzinoK = plant.Magazzino.GetKey("_");
                    string lotto = lottoMapper(plant) ?? lastLot;

                    if (resouce.Opzioni_Raccolta.Ripartizione == Opzioni_Raccolta.enum_Ripartizione_Raccolta.MANUALE)
                    {
                        qta = plant.Qta;
                    } else if (resouce.Opzioni_Raccolta.Ripartizione == Opzioni_Raccolta.enum_Ripartizione_Raccolta.AUTO_PIANTE && totalPlants > 0)
                    {
                        decimal plants = (decimal)plant.esercizioCDC.esercizio.piante_Ha * plant.esercizioCDC.superficieTrattata;
                        qta = resouce.quantitaTotaleReale * plants / totalPlants;
                    } else
                    {
                        qta = resouce.quantitaTotaleReale * plant.esercizioCDC.superficieTrattata / totalArea;
                    }

                    // Evito di aggiungere gli impianti per cui non è stata specificata una quantità
                    if (qta == 0 && productCode != 0) continue; 

                    if (dict.ContainsKey((productCode, esercizioK, magazzinoK, lotto.ToUpper())))
                    {
                        qta += dict[(productCode, esercizioK, magazzinoK, lotto.ToUpper())].Qta;
                        dict.Remove((productCode, esercizioK, magazzinoK, lotto.ToUpper()));
                    }

                    QuantitaSuImpianto newItem = new();
                    newItem.Qta = qta;
                    newItem.esercizioCDC = plant.esercizioCDC;
                    newItem.Magazzino = plant.Magazzino == null ? new("", 0, 0, "") : plant.Magazzino;
                    newItem.Lotto = lotto;
                    newItem.Prodotto = resouce.prodotto;
                    dict.Add((productCode, esercizioK, magazzinoK, lotto.ToUpper()), newItem);
                    lastLot = lotto;
                }
            }
            return dict;
        }

        private void AddMovDestinazione(Movimento_Dettaglio mov, Attivita attivita, OperazioneAgenda agenda, InfoOperazione info, decimal area, List<RisorsaProdotto> resources)
        {
            decimal doseHaTransformed = 0;
            if (info.IsSemina || info.IsRaccolta || info.Elem_Cod == ELEM_COD.TRAPPOLE)
            {
                doseHaTransformed = _utils.GetDoseTrasformata(mov.Qta, mov.Extra_Int) / area;
            } else
            {
                doseHaTransformed = _utils.GetDoseTrasformata(mov.Qta, mov.Extra_Int);
            }

            if (info.IsRaccolta)
            {
                mov.Movimenti_Destinazioni = _movDestinazioniFactory.GetMovDestinazioniRaccolta(mov, attivita, agenda, info, area, doseHaTransformed, resources);

            } else if (!info.IsIrrigazione && info.TipoCentroDiCosto != AgronicaCoreModelsSTD.attivita.centri_di_costo.Tipo.ProdottoDaTrattare)
            {
                mov.Movimenti_Destinazioni = _movDestinazioniFactory.GetMovDestinazioni(attivita, agenda, info, area, doseHaTransformed);
            }
        }

        private List<Movimento_Dettaglio_Riferimento> GetMovDettagliRiferiemnto(Attivita attivita, InfoOperazione info)
        {
            if (!info.IsVisita || attivita.attivitaCollegate == null)
                return new();
            throw new NotImplementedException();
        }

        public List<Movimento_Dettaglio> GetMovDettagliList(Attivita attivita, OperazioneAgenda agenda, InfoOperazione info, decimal totalTreatedArea)
        {
            if (info.IsRilievo)
                return GetMovDettaglioRilievo();

            List<Movimento_Dettaglio> movDet = new();
            List<RisorsaProdotto> resources = attivita.risorse.Where(res =>
                res.classType == ClassType.DettaglioTrattamento || res.classType == ClassType.DettaglioFertilizzazione ||
                res.classType == ClassType.DettaglioSemina || res.classType == ClassType.DettaglioRaccolta
            ).Cast<RisorsaProdotto>().ToList();

            bool foundRisorsa = HandleProductDetails(attivita, movDet, resources, info);
            HandleIrrigationDetails();
            HandleVisitDetails();
            HandleResourceWithoutProduct(movDet, foundRisorsa, agenda, info);

            if (info.IsRaccolta && attivita.tipoRaccolta != Attivita.Tipo_Raccolta.Leggera_Con_Dettagli_Magazzino)
            {
                movDet.ForEach(mov => mov.Cal_Cod = 0);
            }
            if (totalTreatedArea > 0)
            {
                movDet.ForEach(mov => AddMovDestinazione(mov, attivita, agenda, info, totalTreatedArea, resources));
            }
            movDet.ForEach(mov => mov.Movimenti_Dettagli_Riferimenti = GetMovDettagliRiferiemnto(attivita, info));

            if (info.TipoCentroDiCosto == AgronicaCoreModelsSTD.attivita.centri_di_costo.Tipo.ProdottoDaTrattare)
            {
                throw new NotImplementedException();
            }

            return movDet;
        }
    }
}
