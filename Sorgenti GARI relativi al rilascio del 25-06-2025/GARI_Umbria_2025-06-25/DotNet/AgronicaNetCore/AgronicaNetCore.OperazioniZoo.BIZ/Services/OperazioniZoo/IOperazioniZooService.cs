using AgronicaCoreDTOStd.InData.Zoo;
using AgronicaCoreModelsSTD.attivita;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.OperazioniZoo.BIZ.Services.OperazioniZoo
{
    public interface IOperazioniZooService
    {
        Task<DataTable> LeggiCentriAziendaliZooAsync(string piva, AgronicaCoreParametri objParametriServer);

        Task<DataTable> LeggiStalleZooAsync(string piva, int centro, AgronicaCoreParametri objParametriServer);

        Task<DataTable> LeggiStalleRaggruppamentiZooAsync(string piva, int centro, int stanum, AgronicaCoreParametri objParams);

        Task<List<Zootecnia>> LeggiOperazioniZooAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);

        Task<List<Zootecnia>> LeggiOperazioniZooPreferiteAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);

        Task<DataTable> LeggiOperazioniAgendaZooAsync(string piva, int sa_Cod, int sta_Num, DateTime validita_Inizio, DateTime validita_Fine, AgronicaCoreParametri objParams);
        Task<DataTable> LeggiGiacenzeZooAsync(
            LeggiGiacenzeZooDto paramsLeggiGiacenze,
            AgronicaCoreParametri objParametriServer,
            AgronicaCoreParametri objParametriUtente,
            string? superUserUsername = null);

        Task<DataTable> LeggiTrattamentiZooAsync(
             GetTrattamentiZooDto filter,
             AgronicaCoreParametri objParametriUtente,
             AgronicaCoreParametri objParametriServer);

        Task<DataTable> LeggiCapiSenzaTrattamentiAsync(
            GetSenzaTrattamentiZooDto filter,
            AgronicaCoreParametri objParametriUtente,
            AgronicaCoreParametri objParametriServer);

        Task<DataTable> LeggiStazionamentoZooAsync(
            GetStazionamentoZooDto filter,
            AgronicaCoreParametri objParametriUtente,
            AgronicaCoreParametri objParametriServer);
    }
}
