using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaCoreDTOStd.OutData.FiltroRicerca;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.SpecieVegetali
{
    public interface ISpecieVegetaliService
    {
        Task<DtConVisibilita_OUT> SpecieVegetali_GestioneFiltroUtente_LeggiAsync(LeggiSpecieVegetali_IN leggiSpecie_IN, AgronicaCoreParametri objParametri);

        Task<DataTable> Cultivar_GestioneFiltroUtente_LeggiAsync(Cultivar_GestioneFiltroUtente_Leggi_IN leggiCultivar_IN, AgronicaCoreParametri objParametri);

        Task<DataTable> GruppoVegetale_GestioneFiltroUtente_LeggiAsync(int gru_cod, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiGruppiVarietaliAsync(LeggiGruppiVarietali_IN leggiGruppiVarietali_IN, AgronicaCoreParametri objParametri);
        Task<DataTable> LeggiVarietaAsync(int culCod, int vegCod, AgronicaCoreParametri objParams);
        Task<DataTable> LeggiSpecieAziendaliAsync(string piva, AgronicaCoreParametri objParams);
        Task<DataTable> LeggiVarietaFilteredAsync(string piva, int culCod, int vegCod, AgronicaCoreParametri objParams);
    }
}
