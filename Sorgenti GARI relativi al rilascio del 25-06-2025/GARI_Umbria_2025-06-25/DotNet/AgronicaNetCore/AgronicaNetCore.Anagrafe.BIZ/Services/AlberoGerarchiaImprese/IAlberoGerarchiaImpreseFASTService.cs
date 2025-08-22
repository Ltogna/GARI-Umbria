using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.AlberoGerarchiaImprese
{
    public interface IAlberoGerarchiaImpreseFASTService
    {
        Task<KendoHierarchicalDataSource> GetNodesGearchiaObjAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtenti);
    }
}
