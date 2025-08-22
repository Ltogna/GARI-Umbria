using AgronicaCoreDTOStd.InData.ConfrontoCatasto;
using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgronicaCoreDTOStd.InData.Widgets;

namespace AgronicaNetCore.Widgets.BIZ.Services.WidgetStatistiche
{
    public interface IWidgetStatisticheService
    {
        Task<DataTable?> GetCountriesAsync(int year, AgronicaCoreParametri objParametri);

        Task<DataTable?> GetGeneralStatisticsAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMappedFarmersAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMovedMappedFarmersPriorWeekAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMovedMappedFarmersCampaignBeginAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUser);
        Task<DataTable?> GetCropMapAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetFarmersHarvestSowingDataAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetPlotsHarvestSowingDataAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetTargetHAAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetFarmerxRegionxRangeAsync(Widget_Statistics_IN input, AgronicaCoreParametri objParametri);
    }
}
