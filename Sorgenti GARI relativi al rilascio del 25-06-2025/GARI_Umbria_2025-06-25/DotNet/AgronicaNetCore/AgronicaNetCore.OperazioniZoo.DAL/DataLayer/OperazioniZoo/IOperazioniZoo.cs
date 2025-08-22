using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.OperazioniZoo.DAL.DataLayer.OperazioniZoo
{
    public interface IOperazioniZoo
    {
        public Task<DataTable> LeggiCentriZooAsync(string piva, AgronicaCoreParametri objParams);

        public Task<DataTable> LeggiStalleZooAsync(string piva, int centro, AgronicaCoreParametri objParams);

        public Task<DataTable> LeggiStalleRaggruppamentiZooAsync(string piva, int centro, int stanum, AgronicaCoreParametri objParams);
        public Task<DataTable> CaricaAgendaZooAsync(string piva, int sa_Cod, int sta_Num, DateTime validita_Inizio, DateTime validita_Fine, DateTime validita_FineGiacenze, List<string> filtroCentri, int livelloCompatibilita, AgronicaCoreParametri objParams);

        Task<DataTable> Leggi_GiacenzeAsync(
           string Piva,
           int Sa_Cod,
           int STA_NUM,
           int Raggruppamento_Cod,
           int Cod_Animale,
           DateTime Data,
           AgronicaCoreParametri objParametri,
           bool bAll = false,
           bool Filtro_Visibilita_Utente = false,
           List<int> listCod_Animali = null,
           bool filtraGiacenze1 = true,
           bool filtraFornitori = false,
           bool mostraPesate = false,
           bool mostraAnomalie = false,
           //string xFiltroAggiuntivo = "",
           string Matricola = "",
           bool MostraGGPrimoCaricamento = false,
           string CFproprietario = "",
           List<string> listMatricola_Animali = null, 
           bool leggiUltimaPesata = false);

        public Task<DataTable> LeggiTrattamentiAsync(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            string matricola,
            DateTime dataInizio,
            DateTime dataFine,
            bool filtroVisibilitaUtente,
            List<int>? listCodAnimali,
            int[]? farmCatList,
            int[]? farmCatSemplList,
            AgronicaCoreParametri objParametri);
        Task<DataTable> LeggiCapiSenzaTrattamentiAsync
            (
                string piva,
                int saCod,
                int staNum,
                int raggruppamentoCod,
                int codAnimale,
                DateTime data,
                int giorniSenzaTrattamenti,
                bool filtroVisibilitaUtente,
                List<int>? listCodAnimali,
                int[]? farmCatList,
                int[]? farmCatSemplList,
                AgronicaCoreParametri objParametri,
                int compatibilityLevel,
                bool mostraGGPrimoCaricamento = false
            );

        Task<DataTable> LeggiStazionamentoZooAsync
           (
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data,
            int giorniStazionamento,
            bool filtroVisibilitaUtente,
            List<int>? listCod_Animali,
            AgronicaCoreParametri objParametri,
            int compatibilityLevel
           );
    }
}
