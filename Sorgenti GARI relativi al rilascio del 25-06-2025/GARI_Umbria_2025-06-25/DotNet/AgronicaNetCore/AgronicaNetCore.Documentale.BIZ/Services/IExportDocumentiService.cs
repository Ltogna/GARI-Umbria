using AgronicaNetCore.Base.Models;
using InData.DataExchange;
using OutData.DataExchange;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Documentale.BIZ.Services
{
    public interface IExportDocumentiService
    {

        Task<string> GetDocumentiExportAsync(ExportDocumenti_In ExportDocumenti_IN, AgronicaCoreParametri objParametriServer);

    }
}
