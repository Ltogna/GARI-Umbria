using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.GerarchiaImprese
{
    public interface IGerarchiaImprese
    {
        Task<DataTable> LeggixGerarchiaAlberoImprese_VisibilitaAsync(bool applicaVisibilita, AgronicaCoreParametri objParametriServer);
    }
}
