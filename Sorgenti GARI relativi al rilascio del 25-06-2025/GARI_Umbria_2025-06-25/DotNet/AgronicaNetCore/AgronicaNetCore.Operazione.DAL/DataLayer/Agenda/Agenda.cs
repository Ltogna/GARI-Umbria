using System.Text;
using InData.Agenda;
using System.Dynamic;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Anagrafe.DAL.Base;
using static AgronicaNetCore.Base.Models.AgronicaCoreParametri;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Operazione.DAL.Resources;
using AgronicaDataProvider6.Extensions;
using InData.Zoo;
using AgronicaCoreDTOStd.InData.Budget;
using AgronicaDataProvider6.Extensions;
using Newtonsoft.Json;
using System.Transactions;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agronica_Log_Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.RicettexAgenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico_Extra;

namespace AgronicaNetCore.Operazione.DAL.DataLayer.Agenda
{
    public class Agenda : BaseDALOperazione, IAgenda
    {
        private readonly IAgronica_Log_Agenda _logOpAgenda;

        private readonly IRicettexAgenda _ricettexAgDal;
        private readonly IMovimenti _movimentiDal;
        private readonly IMovimenti_Zoo _movZooDal;
        private readonly IMovimenti_Dettagli _movDettagliDal;
        private readonly IMov_Destinazioni _movDestinazioniDal;
        private readonly IMov_Dettaglio_Tecnico _movDettTecDal;
        private readonly IMov_Dettaglio_Tecnico_Extra _movDettTecExtraDal;

        private readonly IAgro_Sequence _sequenceDal;

	public Agenda(IServiceProvider provider, IStringLocalizer<Messages> localizer, bool securityBypass = false) : base(provider,localizer, securityBypass)
        {
            _logOpAgenda = provider.GetRequiredService<IAgronica_Log_Agenda>();

            _ricettexAgDal = provider.GetRequiredService<IRicettexAgenda>();
            _movimentiDal = provider.GetRequiredService<IMovimenti>();
            _movZooDal = provider.GetRequiredService<IMovimenti_Zoo>();
            _movDettagliDal = provider.GetRequiredService<IMovimenti_Dettagli>();
            _movDestinazioniDal = provider.GetRequiredService<IMov_Destinazioni>();
            _movDettTecDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico>();
            _movDettTecExtraDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico_Extra>();

            _sequenceDal = provider.GetRequiredService<IAgro_Sequence>();
        }   

        private async Task<WriteAgenda> Valorizza(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            DateTime adInizio = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO);
            DateTime adFine = DateTime.Parse(CostantiPersonalizzate.AGRODATAFINE);

            dtoAgenda.Sa_Cod ??= 0;
            dtoAgenda.Sta_Num ??= 0;
            dtoAgenda.Linea_Cod ??= 0;
            dtoAgenda.Preparazione_Cod ??= 0;
            dtoAgenda.Id_Trasformazione ??= 0;
            dtoAgenda.Tipo_Accettazione ??= 0;
            dtoAgenda.Audit_Cod ??= 0;
            dtoAgenda.Stato_Export ??= 0;
            dtoAgenda.Stato_Export2 ??= 0;
            dtoAgenda.Tipo_Visibilita ??= 0;
            dtoAgenda.ChkCoge_Manuale ??= 0;
            dtoAgenda.Id_Attivita ??= 0;
            dtoAgenda.Modulo ??= 0;
            dtoAgenda.Raccoglitore_Cod ??= 0;
            dtoAgenda.Split ??= 0;
            dtoAgenda.Pratica_Cod ??= 0;
            dtoAgenda.Origine ??= "";
            dtoAgenda.Stato_Cod ??= 0;
            dtoAgenda.DaRemoto ??= 0;
            
            dtoAgenda.Blocco_Flag ??= 0;
            dtoAgenda.Blocco_Data ??= adInizio;
            dtoAgenda.Blocco_Username ??= "";
            dtoAgenda.Inviato ??= 0;

            if(!dtoAgenda.Validita_Inizio.IsInRange(adInizio, adFine))
                dtoAgenda.Validita_Inizio = adInizio;
            if(!dtoAgenda.Validita_Fine.IsInRange(adInizio, adFine))
                dtoAgenda.Validita_Fine = adFine;

            return dtoAgenda;
        }

        public async Task<DataTable> ReadAsync(string Piva, int Id_Agenda, AgronicaCoreParametri objP, FiltroAggiuntivo? xFiltroAggiuntivo = null)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            stbQuery.AppendLine("SELECT * FROM Agenda ")
                .AppendLine("WHERE 1=1 ");

            if (!string.IsNullOrEmpty(Piva))
            {
                sqlParams.TryAdd("@piva", Piva);
                stbQuery.AppendLine("    AND PIVA = @piva ");
            }
            if (Id_Agenda != 0)
            {
                sqlParams.TryAdd("@idAgenda", Id_Agenda);
                stbQuery.AppendLine("    AND Id_Agenda = @idAgenda ");
            }
            sqlParams.TryAdd("@inizio", objP.FinestraTemporaleFine);
            sqlParams.TryAdd("@fine", objP.FinestraTemporaleInizio);
            stbQuery.AppendLine("    AND Validita_Inizio <= @inizio ")
                .AppendLine("    AND Validita_Fine >= @fine ");

            if (xFiltroAggiuntivo != null)
                stbQuery.AppendLine(FormatFiltroAggiuntivo(xFiltroAggiuntivo, ref sqlParams));

            stbQuery.AppendLine("ORDER BY Id_Agenda DESC ");

            try
            {
                return await GetDataProvider(objP).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> ExistAsync(int Id_Agenda, AgronicaCoreParametri objP)
        {
            if (Id_Agenda == 0)
                throw new Exception("Id_Agenda non valorizzato.");

            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            sqlParams.TryAdd("@idAgenda", Id_Agenda);

            stbQuery.AppendLine("SELECT TOP(1) * FROM Agenda ")
                .AppendLine("WHERE Id_Agenda = @idAgenda ");

            try
            {
                var dt = await GetDataProvider(objP).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> CreateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            dtoAgenda = await Valorizza(dtoAgenda, objP);

            var stbQuery = new StringBuilder();
            stbQuery.AppendLine("INSERT INTO Agenda (PIVA, Sa_Cod, Sta_Num, Id_Agenda, Lav_Cod, des_lib, ")
                .AppendLine("    Linea_Cod, Preparazione_Cod, Id_Trasformazione, Tipo_Accettazione, Audit_Cod, Stato_Export, Stato_Export_2, Tipo_Visibilita, ChkCoge_Manuale, Id_Attivita, Modulo, ")
                .AppendLine("    Raccoglitore_Cod, Split, Pratica_Cod, Origine, Stato_Cod, DaRemoto, ")
                .AppendLine("    Blocco_Flag, Blocco_Data, Blocco_Username, inviato, ");
            if (dtoAgenda.Data_Invio != null)
            {
                stbQuery.AppendLine("    DataInvio, ");
            }
            stbQuery.AppendLine("    Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine) ");
            stbQuery.AppendLine("VALUES (@piva, @saCod, @staNum, @idAgenda, @lavCod, @desLib,  ")
                .AppendLine("    @lineaCod, @prepCod, @idTrasf, @tipoAcc, @auditCod, @statoExp, @statoExp2, @tipoVisib, @chkCogMan, @idAtt, @modulo, @raccCod, @split, @pratCod, @orig, @statoCod, @daRemoto, ")
                .AppendLine("    @bloccoFl, @bloccoDt, @bloccoUsr, @inviato, ");
            if (dtoAgenda.Data_Invio != null)
            {
                stbQuery.AppendLine("    @dtInvio, ");
            }
            stbQuery.AppendLine("    GETDATE(), GETDATE(), @userOp, @userOp, @inizio, @fine) ");

            var expandoObj = new ExpandoObject();
            expandoObj.TryAdd("@piva", dtoAgenda.Piva);
            expandoObj.TryAdd("@saCod", dtoAgenda.Sa_Cod);
            expandoObj.TryAdd("@staNum", dtoAgenda.Sta_Num);
            expandoObj.TryAdd("@idAgenda", dtoAgenda.Id_Agenda);
            expandoObj.TryAdd("@lavCod", dtoAgenda.Lav_Cod);
            expandoObj.TryAdd("@desLib", dtoAgenda.Des_Lib);

            expandoObj.TryAdd("@lineaCod", dtoAgenda.Linea_Cod);
            expandoObj.TryAdd("@prepCod", dtoAgenda.Preparazione_Cod);
            expandoObj.TryAdd("@idTrasf", dtoAgenda.Id_Trasformazione);
            expandoObj.TryAdd("@tipoAcc", dtoAgenda.Tipo_Accettazione);
            expandoObj.TryAdd("@auditCod", dtoAgenda.Audit_Cod);
            expandoObj.TryAdd("@statoExp", dtoAgenda.Stato_Export);
            expandoObj.TryAdd("@statoExp2", dtoAgenda.Stato_Export2);
            expandoObj.TryAdd("@tipoVisib", dtoAgenda.Tipo_Visibilita);
            expandoObj.TryAdd("@chkCogMan", dtoAgenda.ChkCoge_Manuale);
            expandoObj.TryAdd("@idAtt", dtoAgenda.Id_Attivita);
            expandoObj.TryAdd("@modulo", dtoAgenda.Modulo);
            expandoObj.TryAdd("@raccCod", dtoAgenda.Raccoglitore_Cod);
            expandoObj.TryAdd("@split", dtoAgenda.Split);
            expandoObj.TryAdd("@pratCod", dtoAgenda.Pratica_Cod);
            expandoObj.TryAdd("@orig", dtoAgenda.Origine);
            expandoObj.TryAdd("@statoCod", dtoAgenda.Stato_Cod);
            expandoObj.TryAdd("@daRemoto", dtoAgenda.DaRemoto);
            
            expandoObj.TryAdd("@bloccoFl", dtoAgenda.Blocco_Flag);
            expandoObj.TryAdd("@bloccoDt", dtoAgenda.Blocco_Data);
            expandoObj.TryAdd("@bloccoUsr", dtoAgenda.Blocco_Username);
            expandoObj.TryAdd("@inviato", dtoAgenda.Inviato);
            if (dtoAgenda.Data_Invio != null)
            {
                expandoObj.TryAdd("@dtInvio", dtoAgenda.Data_Invio);
            }
            expandoObj.TryAdd("@userOp", objP.UsernameOperazione);
            expandoObj.TryAdd("@inizio", dtoAgenda.Validita_Inizio);
            expandoObj.TryAdd("@fine", dtoAgenda.Validita_Fine);

            try
            {
                return await GetDataProvider(objP).Execute_WriteAsync(stbQuery.ToString(), expandoObj);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            if (dtoAgenda.Id_Agenda == 0)
                throw new Exception("Id_Agenda non valorizzato.");

            var expandoObj = new ExpandoObject();
            expandoObj.TryAdd("@piva", dtoAgenda.Piva);
            expandoObj.TryAdd("@idAgenda", dtoAgenda.Id_Agenda);
            expandoObj.TryAdd("@userOp", objP.UsernameOperazione);

            var excludedProperties = new HashSet<string>
            {
                "Piva",
                "Sa_Cod",
                "Sta_Num",
                "Lav_Cod",
                "Id_Agenda",
                "inviato",
                "DataInvio",
                "Blocco_Flag",
                "Blocco_Data",
                "Blocco_Username",
                "Username_Creazione",
                "Username_Modifica",
                "Data_Creazione",
                "Data_Modifica"
            };

            var setClauses = new List<string>();
            foreach (var property in typeof(WriteAgenda).GetProperties())
            {
                if (excludedProperties.Contains(property.Name)) continue;

                var value = property.GetValue(dtoAgenda);
                if (value != null)
                {
                    string paramName = "@" + property.Name;
                    setClauses.Add($"{property.Name} = {paramName}");
                    expandoObj.TryAdd(paramName, value);
                }
            }
            if (setClauses.Count == 0) return false;

            string updateQuery = 
                $@"UPDATE Agenda SET
                    {string.Join(", ", setClauses)}
                    , Username_Modifica = @userOp
                    , Data_Modifica = GETDATE()
                WHERE Piva = @piva AND Id_Agenda = @idAgenda";

            try
            {
                return await GetDataProvider(objP).Execute_WriteAsync(updateQuery, expandoObj);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string Piva, int Id_Agenda, int Sa_Cod, AgronicaCoreParametri objP)
        {
            if (Id_Agenda == 0)
                throw new Exception("Id_Agenda non valorizzato.");

            var stbQuery = new StringBuilder();
            var expandoObj = new ExpandoObject();
            expandoObj.TryAdd("@piva", Piva);
            expandoObj.TryAdd("@idAgenda", Id_Agenda);

            if (objP.FlagCancellazioneLogica == enumCancellazioneLogica.CancellazioneLogica)
            {
                stbQuery.AppendLine("UPDATE Agenda SET ")
                    .AppendLine("    Inviato = -1, ")
                    .AppendLine("    Username_Modifica = @userOp ")
                    .AppendLine("WHERE Inviato >= 0 ");
                expandoObj.TryAdd("@userOp", objP.UsernameOperazione);
            } else
            {
                stbQuery.AppendLine("DELETE FROM Agenda ")
                    .AppendLine("WHERE 1=1 ");
            }

            stbQuery.AppendLine("    AND Piva = @piva ")
                .AppendLine("    AND Id_Agenda = @idAgenda ");

            if (Sa_Cod != 0)
            {
                stbQuery.AppendLine("    AND Sa_Cod = @saCod ");
                expandoObj.TryAdd("@saCod", Sa_Cod);
            }

            try
            {
                return await GetDataProvider(objP).Execute_WriteAsync(stbQuery.ToString(), expandoObj);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

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
                    isNew = !await ExistAsync(dtoAgenda.Id_Agenda, objP);

                if (isNew)
                {
                    tipoOp = (int)enum_TipoOperazioneDB.Scrittura;
                    await CreateAsync(dtoAgenda, objP);
                }
                else
                {
                    tipoOp = (int)enum_TipoOperazioneDB.Modifica;
                    await UpdateAsync(dtoAgenda, objP);
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
                return await UpdateAsync(dtoAgenda, objP);
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
                return await UpdateAsync(dtoAgenda, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> BloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(objP.objTransazione != null ? TransactionScopeOption.Required : TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0), TransactionScopeAsyncFlowOption.Enabled))
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
                        await UpdateAsync(dtoAgenda, objP);
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
                    if (objP.objTransazione == null)
                        ts.Dispose();
                }
            }

            return true;
        }

        public async Task<bool> SbloccaAttivitaAsync(string Piva, int Sa_Cod, List<int> Operazioni, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(objP.objTransazione != null ? TransactionScopeOption.Required : TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0), TransactionScopeAsyncFlowOption.Enabled))
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
                        await UpdateAsync(dtoAgenda, objP);
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
                    if (objP.objTransazione == null)
                        ts.Dispose();
                }
            }

            return true;
        }

        public async Task<bool> EliminaAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            try
            {
                return await DeleteAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, dtoAgenda.Sa_Cod ?? 0, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAssociateAsync(WriteAgenda dtoAgenda, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(objP.objTransazione != null ? TransactionScopeOption.Required : TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0), TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    /*-- MOV DETTAGLIO TECNICO EXTRA --*/
                    DataTable dtMovDetTecEx = await _movDettTecExtraDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, 0, objP);
                    if (dtMovDetTecEx.Rows.Count > 0)
                    {
                        var dtosMovDettTec = dtMovDetTecEx.ToDictionaryList().Select(row => new WriteMovDettTecnicoExtra()
                        {
                            Piva = Convert.ToString(row["Piva"]),
                            Id_Agenda = (int)row["Id_Agenda"],
                            Id_Mov = (int)row["Id_Mov"],
                            Id_Mov_Det = (int)row["Id_Mov_Det"],
                            Id_Reg_Det = (int)row["Id_Reg_Det"]
                        }).ToList();
                        await _movDettTecExtraDal.EliminaAsync(dtosMovDettTec, objP);
                    }

                    /*-- MOV DETTAGLIO TECNICO --*/
                    DataTable dtMovDetTec = await _movDettTecDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, 0, objP);
                    if (dtMovDetTec.Rows.Count > 0)
                    {
                        var dtosMovDettTec = dtMovDetTec.ToDictionaryList().Select(row =>
                            new WriteMovDettTecnico(Convert.ToString(row["Piva"]), (int)row["Id_Agenda"], (int)row["Id_Mov"], (int)row["Id_Mov_Det"], (int)row["Id_Reg_Dettaglio"])).ToList(); 
                        await _movDettTecDal.EliminaAsync(dtosMovDettTec, objP);
                    }

                    /*-- MOV DESTINAZIONI --*/
                    DataTable dtMovDest = await _movDestinazioniDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, 0, objP);
                    if (dtMovDest.Rows.Count > 0)
                    {
                        var dtosMovDest = dtMovDest.ToDictionaryList()
                            .Select(row => new WriteMovDestinazioni(Convert.ToString(row["Piva"]), (int)row["Id_Agenda"], (int)row["Id_Mov"], (int)row["Id_Mov_Det"], (int)row["Id_Destinazione"]))
                            .ToList();
                        await _movDestinazioniDal.EliminaAsync(dtosMovDest, objP);
                    }

                    /*-- MOVIMENTI DETTAGLI --*/
                    DataTable dtMovDett = await _movDettagliDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, 0, objP);
                    if (dtMovDett.Rows.Count > 0)
                    {
                        var dtosMovDett = dtMovDett.ToDictionaryList()
                            .Select(row => new WriteMovDettagli(Convert.ToString(row["Piva"]), (int)row["Id_Agenda"], (int)row["Id_Mov"], (int)row["Id_Mov_Det"]))
                            .ToList();
                        await _movDettagliDal.EliminaAsync(dtosMovDett, objP);
                    }

                    /*-- MOVIMENTI ZOO --*/
                    DataTable dtMovZoo = await _movZooDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, objP);
                    if (dtMovZoo.Rows.Count > 0)
                    {
                        var dtosMovZoo = dtMovZoo.ToDictionaryList()
                            .Select(row => new WriteMovimentiZoo(Convert.ToString(row["Piva"]), (int)row["Id_Agenda"], (int)row["Id_Mov"]))
                            .ToList();
                        await _movZooDal.EliminaAsync(dtosMovZoo, objP);
                    }

                    /*-- MOVIMENTI --*/
                    DataTable dtMov = await _movimentiDal.ReadAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, 0, objP);
                    if (dtMov.Rows.Count > 0)
                    {
                        var dtosMov = dtMov.ToDictionaryList()
                            .Select(row => new WriteMovimenti(Convert.ToString(row["Piva"]), (int)row["Id_Agenda"], (int)row["Id_Mov"]))
                            .ToList();
                        await _movimentiDal.EliminaAsync(dtosMov, objP);
                    }

                    /*-- RICETTExAGENDA --*/
                    DataTable dtRxA = await _ricettexAgDal.ReadAsync(0, dtoAgenda.Id_Agenda, objP);
                    if (dtRxA.Rows.Count > 0)
                    {
                        var dtosRxA = dtRxA.ToDictionaryList().Select(row => new WriteRicettexAgenda()
                        {
                            Ricetta_Cod = (int)row["Ricetta_Cod"],
                            Id_Agenda = (int)row["Id_Agenda"]
                        }).ToList();
                        await _ricettexAgDal.EliminaAsync(dtosRxA, objP);
                    }

                    /*-- AGENDA --*/
                    await DeleteAsync(dtoAgenda.Piva, dtoAgenda.Id_Agenda, dtoAgenda.Sa_Cod ?? 0, objP);

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, objP, ex);
                    throw;
                }
                finally
                {
                    if (objP.objTransazione == null)
                        ts.Dispose();
                }
            }

            return true;
        }
    }
}
