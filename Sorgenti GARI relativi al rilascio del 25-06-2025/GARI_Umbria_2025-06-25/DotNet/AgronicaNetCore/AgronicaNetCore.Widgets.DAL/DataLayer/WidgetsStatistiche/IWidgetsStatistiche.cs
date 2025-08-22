using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Widgets.DAL.DataLayer.WidgetsStatistiche.WidgetsStatistiche
{
    public interface IWidgetsStatistiche
    {
        Task<DataTable?> GetCountriesAsync(int year, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetGeneralStatisticsAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMappedFarmersAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMovedMappedFarmersPriorWeekAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetMovedMappedFarmersCampaignBeginAsync(int year, string country, DateTime campaignBegin, DateTime campaignEnd, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetCropMapAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetFarmersHarvestSowingDataAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetPlotsHarvestSowingDataAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetTargetHAAsync(int year, string country, AgronicaCoreParametri objParametri);
        Task<DataTable?> GetFarmerxRegionxRangeAsync(int year, string country, AgronicaCoreParametri objParametri);
    }
}