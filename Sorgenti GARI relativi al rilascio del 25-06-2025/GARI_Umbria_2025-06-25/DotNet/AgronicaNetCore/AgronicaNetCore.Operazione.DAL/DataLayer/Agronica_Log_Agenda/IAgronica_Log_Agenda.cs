using AgronicaNetCore.Base.Models;
using InData.Agenda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Agronica_Log_Agenda
{
    public interface IAgronica_Log_Agenda
    {
        //public Task<DataTable> ReadAsync(string int lavCod, DateTime? data, AgronicaCoreParametri objP);

        public Task<bool> CreateAsync(WriteLogAgenda dtoLogAgenda, AgronicaCoreParametri objP);
        public Task<bool> UpdateAsync(WriteLogAgenda dtoLogAgenda, AgronicaCoreParametri objP);
        public Task<bool> DeleteAsync(int ID, string Piva, int Id_Agenda, AgronicaCoreParametri objP);
    }
}
