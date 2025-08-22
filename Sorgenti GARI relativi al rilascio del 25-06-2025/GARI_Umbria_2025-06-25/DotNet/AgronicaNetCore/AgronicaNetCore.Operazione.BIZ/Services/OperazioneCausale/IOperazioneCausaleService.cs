using AgronicaNetCore.Base.Models;
using InData.Operazione;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Operazione.BIZ.Services.OperazioneCausale;

public interface IOperazioneCausaleService
{
    Task<DataTable> OperazioneCausale_LeggiAsync(LeggiOperazione leggiOperazione, AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);
    Task<DataTable> Leggi_CausaleDes_From_CausaleId_Async(int id, AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);
    Task<bool> OperazioneCausale_ScriviModificaAsync(OperazioneCausale_In body, AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtente);
    public Task<bool> OperazioneCausale_CancellaAsync(int id, int lavCod, AgronicaCoreParametri objParams);
}
