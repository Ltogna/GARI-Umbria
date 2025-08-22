using InData.Agenda;
using Newtonsoft.Json;
using System.Transactions;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Base.Constants;
using AgronicaCoreDTOStd.InData.Budget;
using Microsoft.Extensions.DependencyInjection;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agronica_Log_Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico_Extra;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti;
using System.Data;
using AgronicaDataProvider6.Extensions;
using System.Linq;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Zoo;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Operazione.BIZ.Resources;
using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreDTOStd.InData.Agenda.OperazioneAgenda_Temp;
using AgronicaCoreDTOStd.Identity;
using AgronicaNetCore.Operazione.DAL.Resources.UtilityAgenda;
using AgronicaCoreModelsSTD.attivita.Info;
using InData.Anagrafica;
using InData.Log.AgronicaLogInvio;
using System.Diagnostics.CodeAnalysis;

namespace AgronicaNetCore.Operazione.BIZ.Services.Agenda
{
    public class AgendaService : BaseServiceOperazioneBIZ, IAgendaService
    {
        private readonly IAgenda _agendaDal;
        private readonly IAgronica_Log_Agenda _logOpAgenda;
        
        private readonly IMovimenti _movimentiDal;
        private readonly IMovimentiService _movimentiBiz;

        private readonly IMovimenti_Dettagli _movDettagliDal;
        //private readonly IMovDettagliService _movDettagliBiz;

        private readonly IMov_Destinazioni _movDestinazioniDal;
        //private readonly IMovDestinazioniService _movDestinazioniBiz;
        
        private readonly IMovimenti_Zoo _movZooDal;
        //private readonly IMovZooService _movZooBiz;
        
        private readonly IMov_Dettaglio_Tecnico _movDettTecDal;
        private readonly IMov_Dettaglio_Tecnico_Extra _movDettTecExtraDal;
        private readonly IAgro_Sequence _sequenceDal;
        private readonly IUtilityAgendaClassInitializer _agendaClassInitializer;

        public AgendaService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _agendaDal = provider.GetRequiredService<IAgenda>();

            _logOpAgenda = provider.GetRequiredService<IAgronica_Log_Agenda>();
            
            _movimentiDal = provider.GetRequiredService<IMovimenti>();
            _movimentiBiz = provider.GetRequiredService<IMovimentiService>();

            _movDettagliDal = provider.GetRequiredService<IMovimenti_Dettagli>();
            //_movDettagliBiz = provider.GetRequiredService<IMovDettagliService>();

            _movDestinazioniDal = provider.GetRequiredService<IMov_Destinazioni>();
            //_movDestinazioniBiz = provider.GetRequiredService<IMovDestinazioniService>();
            
            _movZooDal = provider.GetRequiredService<IMovimenti_Zoo>();
            //_movZooBiz = provider.GetRequiredService<IMovZooService>();

            _movDettTecDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico>();

            _movDettTecExtraDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico_Extra>();
            
            _sequenceDal = provider.GetRequiredService<IAgro_Sequence>();
            _agendaClassInitializer = provider.GetRequiredService<IUtilityAgendaClassInitializer>();
        }

        #region Operazioni Agenda QdC
        private async Task<int> Scrivi(OperazioneAgenda agenda, AgronicaCoreParametri objServer)
        {
            int idAgenda = agenda.Id_Agenda;
            if (idAgenda <= 0)
            {
                idAgenda = await _sequenceDal.NuovoId_TabellaAsync("Agenda", agenda.BaseCode, agenda.TopCode, objServer);
            }
            agenda.Id_Agenda = idAgenda;
            await _agendaDal.CreateAsync(agenda.ToWriteAgenda(), objServer);

            DateTime data = agenda.Data;

            // TODO write agenda log
            // TODO write note if agenda.note > 0
            // TODO write movimenti
            if (agenda.Movimenti.Any())
            {
                foreach (Movimento movimento in agenda.Movimenti)
                {
                    movimento.Id_Agenda = idAgenda;
                    await _movimentiBiz.ScriviMovimentoAsync(movimento, objServer);
                }
            }
            // TODO write movimenti dett rif 

            return idAgenda;
        }

        /// <summary>
        /// Scritta sulla bas di scriviAgenda
        /// </summary>
        private async Task<int> ScriviAttivita(Attivita attivita, OperazioneAgenda agenda, InfoOperazione info, AgronicaCoreParametri objServer)
        {
            int idAgenda = await Scrivi(agenda, objServer);

            // TODO if attivita.codice == 0
            // TODO if info.isrilievo
            // TODO if attivita.risorse > 0 && is QdC

            return idAgenda;
        }

        /// <exception cref="NotImplementedException"></exception>
        private async Task<int> ScriviOperazioneAgenda(Attivita attivita, OperazioneAgenda agenda, InfoOperazione info, AgronicaCoreParametri objServer)
        {
            int idAgenda = 0;
            List<Movimento_Dettaglio_Tecnico> techDetails = new();

            //if (info.IsRaccolta && attivita.tipo == Attivita.Tipo_Attivita.QuadernoDiCampagna)
            //{
            //    // PostOperazioe. modifica anagrafiche
            //}

            //if esegui solo verifiche conformita then return idAgenda
            if (attivita.tipo == Attivita.Tipo_Attivita.QuadernoDiCampagna)
            {
                idAgenda = await ScriviAttivita(attivita, agenda, info, objServer);
            }
            else
            {
                throw new NotImplementedException();
            }

            return idAgenda;
        }

        private async Task<int> ScriviAttivitaToAgenda(
            Attivita attivita, OperazioneAgenda agenda, AgronicaCoreParametri objServer)
        {
            int idAgenda = 0;

            // TODO gestire operazione esistente
            // TODO gestire parametri aggiuntivi

            InfoOperazione info = _agendaClassInitializer.GetInfoOperazione(agenda.Lav_Cod, Attivita.Tipo_Attivita.QuadernoDiCampagna);

            if (info.IsZoo)
            {
                // TODO chiamare funzione inerente
            } else
            {
                return await ScriviOperazioneAgenda(attivita, agenda, info, objServer);
            }

            return idAgenda;
        }
        #endregion

        #region Scrittura
        public async Task<int> ScriviModificaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            try
            {
                int tipoOp;
                bool isNew = false;
                
                if (dtoAgenda.Id_Agenda == 0)
                {
                    dtoAgenda.Id_Agenda = await _sequenceDal.NuovoId_TabellaAsync("agenda", 0, 2000000000, objP);
                    isNew = true;
                }
                else
                    isNew = !await _agendaDal.ExistAsync(dtoAgenda.Id_Agenda, objP);

                if (isNew)
                {
                    tipoOp = (int)enum_TipoOperazioneDB.Scrittura;
                    await _agendaDal.CreateAsync(dtoAgenda, objP);
                }
                else
                {
                    tipoOp = (int)enum_TipoOperazioneDB.Modifica;
                    await _agendaDal.UpdateAsync(dtoAgenda, objP);
                }

                var jobjAgenda = 
                    JsonConvert.SerializeObject(dtoAgenda, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local });
                WriteLogAgenda dtoLogAgenda = new()
                {
                    Piva = dtoAgenda.Piva,
                    Sa_Cod = dtoAgenda.Sa_Cod,
                    Id_Agenda = dtoAgenda.Id_Agenda,
                    Lav_Cod = dtoAgenda.Lav_Cod,
                    Id_Servizio = 5,
                    Des_Lib = dtoAgenda.Des_Lib,
                    Data_Ora_Lavorazione = dtoAgenda.Validita_Inizio,
                    SuperUser = objP.PivaSuperUser,
                    Utente = objP.UsernameOperazione,
                    Tipo_Operazione = tipoOp,
                    Data_Ora_RegistrazioneLog = DateTime.Now,
                    Object_Data = jobjAgenda,
                    Origine = -1,
                    Raccoglitore_Cod = 0
                };
                await _logOpAgenda.CreateAsync(dtoLogAgenda, objP);

                return dtoAgenda.Id_Agenda;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        /// <summary>
        /// Scritta sulla base di ScriviListaAttivitaToRaccoglitore
        /// </summary>
        public async Task ScriviListaAttivitaAgendaAsync(List<(Attivita attivita, OperazioneAgenda agenda)> agendaActivityList, AgronicaCoreParametri objServer)
        {
            try
            {
                await OpenConnectionAsync(objServer);

                // TODO eliminare definitivamente le agende che in un salvataggio multicentro
                // non sono state riconfermate (causa deselezione propri impianti/centri)
                // TODO gestione carico/scarico

                Attivita last = agendaActivityList.Last().attivita;

                foreach (var item in agendaActivityList)
                {
                    bool isLast = item.attivita == last;
                    string currActivityDes = item.attivita.job.descrizione;
                    int idAgenda = await ScriviAttivitaToAgenda(item.attivita, item.agenda, objServer);

                }
            }
            catch (Exception ex)
            {
                CloseTransaction(objServer, true);
                LogError(ex.Message, objServer, ex);
                throw;
            }
            finally
            {
                CloseConnection(objServer);
            }
        }

        public async Task<int> ScriviAttivitaAgendaAsync((Attivita attivita, OperazioneAgenda agenda) agendaActivityList, AgronicaCoreParametri objServer)
        {
            try
            {
                await OpenConnectionAsync(objServer);

                // TODO eliminare definitivamente le agende che in un salvataggio multicentro
                // non sono state riconfermate (causa deselezione propri impianti/centri)
                // TODO gestione carico/scarico

                int idAgenda = await ScriviAttivitaToAgenda(agendaActivityList.attivita, agendaActivityList.agenda, objServer);
                return idAgenda;
            }
            catch (Exception ex)
            {
                CloseTransaction(objServer, true);
                LogError(ex.Message, objServer, ex);
                throw;
            }
            finally
            {
                CloseConnection(objServer);
            }
        }
        #endregion

        #region Blocco/Sblocco
        public async Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP)
        {
            try
            {
                WriteAgenda dtoAgenda = new(Piva, Sa_Cod, Id_Agenda)
                {
                    Blocco_Flag = 1,
                    Blocco_Data = DateTime.Now,
                    Blocco_Username = objP.UsernameOperazione
                };
                return await _agendaDal.UpdateAsync(dtoAgenda, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, int Id_Agenda, AgronicaCoreParametri objP)
        {
            try
            {
                WriteAgenda dtoAgenda = new(Piva, Sa_Cod, Id_Agenda)
                {
                    Blocco_Flag = 0,
                    Blocco_Data = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO),
                    Blocco_Username = objP.UsernameOperazione
                };
                return await _agendaDal.UpdateAsync(dtoAgenda, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }
        
        public async Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0)))
            {
                try
                {
                    foreach (int Id_Agenda in Operazioni)
                    {
                        WriteAgenda dtoAgenda = new(Piva, Sa_Cod, Id_Agenda)
                        {
                            Blocco_Flag = 1,
                            Blocco_Data = DateTime.Now,
                            Blocco_Username = objP.UsernameOperazione
                        };
                        await _agendaDal.UpdateAsync(dtoAgenda, objP);
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, objP, ex);
                    throw;
                }
                finally
                {
                    ts.Dispose();
                }
            }

            return true;
        }

        public async Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0)))
            {
                try
                {
                    foreach (int Id_Agenda in Operazioni)
                    {
                        WriteAgenda dtoAgenda = new(Piva, Sa_Cod, Id_Agenda)
                        {
                            Blocco_Flag = 0,
                            Blocco_Data = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO),
                            Blocco_Username = objP.UsernameOperazione
                        };
                        await _agendaDal.UpdateAsync(dtoAgenda, objP);
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, objP, ex);
                    throw;
                }
                finally
                {
                    ts.Dispose();
                }
            }

            return true;
        }
        #endregion

        #region Eliminazione
        public async Task<bool> EliminaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            try
            {
                return await _agendaDal.DeleteAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, dtoAgenda.Sa_Cod ?? 0, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

    //    public async Task<bool> EliminaAssociateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
    //    {
    //        using (TransactionScope ts = new(TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0)))
    //        {
    //            try
    //            {
    //                //mov dett tec extra

    //                //mov dett tec

    //                /*-- MOV DESTINAZIONI --*/
    //                DataTable dtMovDest = await _movDestinazioniDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, 0, objP);
    //                if (dtMovDest.Rows.Count > 0)
    //                {
    //                    var dtosMovDest = dtMovDest.ToDictionaryList().Select(row => new WriteMovDestinazioni()
    //                    {
    //                        Piva = Convert.ToString(row["Piva"]),
    //                        Id_Agenda = (int)row["Id_Agenda"],
    //                        Id_Mov = (int)row["Id_Mov"],
    //                        Id_Mov_Det = (int)row["Id_Mov_Det"],
    //                        Id_Destinazione = (int)row["Id_Destinazione"]
    //                    }).ToList();
    //                    await _movDestinazioniBiz.EliminaAsync(dtosMovDest, objP);
    //                }


    //                /*-- MOVIMENTI DETTAGLI --*/
    //                DataTable dtMovDett = await _movDettagliDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, objP);
    //                if (dtMovDett.Rows.Count > 0)
    //                {
    //                    var dtosMovDett = dtMovDett.ToDictionaryList().Select(row => new WriteMovDettagli()
    //                    {
    //                        Piva = Convert.ToString(row["Piva"]),
    //                        Id_Agenda = (int)row["Id_Agenda"],
    //                        Id_Mov = (int)row["Id_Mov"],
    //                        Id_Mov_Det = (int)row["Id_Mov_Det"]
    //                    }).ToList();
    //                    await _movDettagliBiz.EliminaAsync(dtosMovDett, objP);
    //                }

    //                /*-- MOVIMENTI ZOO --*/
    //                DataTable dtMovZoo = await _movZooDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, objP);
    //                if (dtMovZoo.Rows.Count > 0)
    //                {
    //                    var dtosMovZoo = dtMovZoo.ToDictionaryList().Select(row => new WriteMovimentiZoo()
    //                    {
    //                        Piva = Convert.ToString(row["Piva"]),
    //                        Id_Agenda = (int)row["Id_Agenda"],
    //                        Id_Mov = (int)row["Id_Mov"]
    //                    }).ToList();
    //                    await _movZooBiz.EliminaAsync(dtosMovZoo, objP);
    //                }

    //                /*-- MOVIMENTI --*/
    //                DataTable dtMov = await _movimentiDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, objP);
    //                if(dtMov.Rows.Count > 0)
    //                {
    //                    var dtosMov = dtMov.ToDictionaryList().Select(row => new WriteMovimenti()
    //                    {
    //                        Piva = Convert.ToString(row["Piva"]),
    //                        Id_Agenda = (int)row["Id_Agenda"],
    //                        Id_Mov = (int)row["Id_Mov"]
    //                    }).ToList();
    //                    await _movimentiBiz.EliminaAsync(dtosMov, objP);
    //                }

    //                /*-- AGENDA --*/
    //                await _agendaDal.DeleteAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, dtoAgenda.Sa_Cod ?? 0, objP);
                    
    //                ts.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                LogError(ex.Message, objP, ex);
    //                throw;
    //            }
    //            finally
    //            {
    //                ts.Dispose();
    //            }
    //        }
            
        //    return true;
        //}
        #endregion
    }
}
