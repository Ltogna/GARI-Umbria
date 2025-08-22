using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.Imprese
{
    public interface IImpresaService
    {
        Task<AgronicaCoreModelsSTD.anagrafiche.Impresa?> LeggiImpresaAsync(AgronicaCoreParametri objParametri, string partiIva);
        Task<DataTable> Leggi2Async(AgronicaCoreParametri objParametri, string partiIva);
        Task<DataTable> LeggiPadriAsync(AgronicaCoreParametri objParametri);
        Task<DataTable> LeggiImpreseAsync(string piva, AgronicaCoreParametri objParametriServer);
        Task<DataTable> TestClausolaINAsync(List<string> elencoPiva, List<int> elencoVegCod, int varieta, AgronicaCoreParametri objParametri);
    }
}
