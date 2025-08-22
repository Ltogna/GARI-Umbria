using AgronicaCoreModelsSTD.anagrafiche;

namespace OutData.Zoo
{
    public class ProtocolloInCorso
    {
        public int IdProt { get; set; }
        public string Numero { get; set; }

        public string Piva { get; set; }
        public string Impresa { get; set; }
        public int SaCod { get; set; }
        public string SaDes { get; set; }
        public int StaNum { get; set; }
        public string StaDes { get; set; }
        public int RaggrCod { get; set; }
        public string RaggrDes { get; set; }

        public string FamigliaAic { get; set; }
        public string FarmacoDes { get; set; }

        public CapoAnimale Capo { get; set; }
        public double PesoStimato { get; set; }

        public ProtocolloInCorso()
        {
            Capo = new CapoAnimale();
        }

        public ProtocolloInCorso(int idProt, string numero, string piva, string impresa, int saCod, string saDes, int staNum, string staDes, int raggrCod, string raggrDes, string famigliaAic, string farmacoDes, CapoAnimale capo, double pesoStimato)
        {
            this.IdProt = idProt;
            this.Numero = numero;
            this.Piva = piva;
            this.Impresa = impresa;
            this.SaCod = saCod;
            this.SaDes = saDes;
            this.StaNum = staNum;
            this.StaDes = staDes;
            this.RaggrCod = raggrCod;
            this.RaggrDes = raggrDes;
            this.FamigliaAic = famigliaAic;
            this.FarmacoDes = farmacoDes;
            this.Capo = capo;
            this.PesoStimato = pesoStimato;
        }
    }
}
