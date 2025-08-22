using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaCoreDTOStd.OutData.FiltroRicerca;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.Operazioni
{
    public interface IOperazioniService
    {
        Task<DtConVisibilita_OUT> Operazioni_GestioneFiltroUtente_LeggiAsync(LeggiOperazioni_IN leggiOperazioni, AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);
        Task<DataTable> LeggiOperazioniPerTipoAsync(string tipoGruppoOperazione, AgronicaCoreParametri objParametriServer);
    }
}
