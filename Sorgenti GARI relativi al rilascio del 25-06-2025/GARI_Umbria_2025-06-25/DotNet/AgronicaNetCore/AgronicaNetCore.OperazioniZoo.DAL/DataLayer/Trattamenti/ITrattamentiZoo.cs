using AgronicaNetCore.Base.Models;
using AgronicaNetCore.OperazioniZoo.DAL.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.OperazioniZoo.DAL.DataLayer.Trattamenti
{
    public interface ITrattamentiZoo
    {
        public Task<bool> ScriviAgendaAsync(string Piva, int Sa_Cod, int Sta_Num, int Id_Agenda, DateTime inizio, DateTime fine, AgronicaCoreParametri objP);

        public Task<bool> ScriviAgroLogAgendaAsync(int idAgenda, string piva, int saCod, DateTime inizio, int tipoOp, AgronicaCoreParametri objP);
        
        public Task<bool> ScriviMovimentiAsync(string piva, int idAgenda, DateTime inizio, DateTime registrazione, AgronicaCoreParametri objP);

    }
}
