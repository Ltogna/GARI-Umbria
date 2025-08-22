using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Documentale.DAL.DataLayer
{
    public interface IExportDocumenti
    {
        Task<DataTable> LeggiDocumentiExportAsync(string CUAA, int Tipologia_Cod, DateTime DataRiferimento, AgronicaCoreParametri objParametriServer);
    }
}



