using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.OperazioniZoo.DAL.Resources;
using AgronicaNetCore.Utility.DAL.DataLayer.FiltriTabelle;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;

namespace AgronicaNetCore.OperazioniZoo.DAL.DataLayer.OperazioniZoo
{
    public class OperazioniZoo : BaseDALOperazioniZoo, IOperazioniZoo
    {
        readonly IAgro_Sequence _agrosequences;
        //readonly IMemoryCache _memoryCache;
        readonly ITmpAgenda _tempAgenda;

        public OperazioniZoo(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _agrosequences = provider.GetRequiredService<IAgro_Sequence>();
            //_memoryCache = provider.GetRequiredService<IMemoryCache>();
            _tempAgenda = provider.GetRequiredService<ITmpAgenda>();
        }

        public async Task<DataTable> LeggiCentriZooAsync(string piva, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();

            stbQuery.AppendLine(" SELECT c.sa_cod, sa_nome ");
            stbQuery.AppendLine(" FROM Centri_Aziendali c INNER JOIN Fabbricati f ON c.PIVA = f.PIVA AND c.sa_cod = f.SA_COD ");
            stbQuery.AppendLine(" WHERE c.PIVA = @piva ");
            stbQuery.AppendLine(" AND Tipo_Fabbricato_Cod >= @tipoFabbricatoDa AND Tipo_Fabbricato_Cod <= @tipoFabbricatoA ");
            stbQuery.AppendLine(" ORDER BY sa_nome ASC ");

            sqlParams.TryAdd("@piva", piva);
            sqlParams.TryAdd("@tipoFabbricatoDa", 170);
            sqlParams.TryAdd("@tipoFabbricatoA", 179);

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

        }

        public async Task<DataTable> LeggiStalleZooAsync(string piva, int centro, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();

            stbQuery.AppendLine(" SELECT s.* ");
            stbQuery.AppendLine(" FROM Stalla s INNER JOIN Centri_Aziendali c ON c.PIVA = s.PIVA AND c.sa_cod = s.sa_cod ");
            stbQuery.AppendLine(" WHERE c.PIVA = @piva ");
            stbQuery.AppendLine(" AND c.sa_cod = @centro ");
            stbQuery.AppendLine(" ORDER BY s.STA_DES ASC ");

            sqlParams.TryAdd("@piva", piva);
            sqlParams.TryAdd("@centro", centro);

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

        }

        public async Task<DataTable> LeggiStalleRaggruppamentiZooAsync(string piva, int centro, int stanum, AgronicaCoreParametri objParams)
        {
            var stbQuery = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();

            stbQuery.AppendLine(" SELECT * ");
            stbQuery.AppendLine(" FROM Stalla_Raggruppamenti ");
            stbQuery.AppendLine(" WHERE PIVA = @piva ");
            stbQuery.AppendLine(" AND sa_cod = @centro ");
            stbQuery.AppendLine(" AND STA_NUM = @stanum ");
            stbQuery.AppendLine(" ORDER BY Raggruppamento_Cod ASC ");

            sqlParams.TryAdd("@piva", piva);
            sqlParams.TryAdd("@centro", centro);
            sqlParams.TryAdd("@stanum", stanum);

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(stbQuery.ToString(), sqlParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

        }

        public async Task<DataTable> CaricaAgendaZooAsync(string piva, int sa_Cod, int sta_Num, DateTime validita_Inizio, DateTime validita_Fine, DateTime validita_FineGiacenze, List<string> filtroCentri, int livelloCompatibilita, AgronicaCoreParametri objParams)
        {
            var Stb = new StringBuilder();
            var sqlParams = new Dictionary<string, object>();
            var sqlParamsIn = new Dictionary<string, Dictionary<Type, List<object>>>();

            var listaLavCodCaricoSpostamenti = new List<int>
            {
                LAV_COD.LAVCOD_NASCITA_ANIMALI,
                LAV_COD.LAVCOD_INCREMENTO_CONSISTENZE_ZOO,
                LAV_COD.LAVCOD_ACQUISTO_ANIMALI,
                LAV_COD.LAVCOD_SPOSTAMENTI_ZOO
            };

            var listaLavCodCarichiScarichi = new List<int>
            {
               LAV_COD.LAVCOD_NASCITA_ANIMALI,
               LAV_COD.LAVCOD_INCREMENTO_CONSISTENZE_ZOO,
               LAV_COD.LAVCOD_DECREMENTO_CONSISTENZE_ZOO,
               LAV_COD.LAVCOD_MORTE_ANIMALI,
               LAV_COD.LAVCOD_MACELLAZIONE_ANIMALI,
               LAV_COD.LAVCOD_ACQUISTO_ANIMALI,
               LAV_COD.LAVCOD_VENDITA_ANIMALI,
               LAV_COD.LAVCOD_TRASFERIMENTO_ANIMALI
            };

            var listaCauMovCarichiScarichi = new List<string>
            {
                CAU_MOV.CAU_CARICO_CAPO,
                CAU_MOV.CAU_SCARICO_CAPO
            };

            var listaLavCodTrattamentiAlimentazione = new List<int>
            {
                LAV_COD.LAVCOD_SOSTITUZIONE_MARCA,
                LAV_COD.LAVCOD_PRODUZIONI_LATTE_BOVINO,
                LAV_COD.LAVCOD_PREPARAZIONE_MUNGITURA,
                LAV_COD.LAVCOD_MUNGITURA_SECCHIO_POSTA,
                LAV_COD.LAVCOD_MUNGITURA_GRUPPI_POSTA,
                LAV_COD.LAVCOD_MUNGITURA_SALA_LATTE,
                LAV_COD.LAVCOD_LAVAGGIO_IMPIANTI_MUNGITURA,
                LAV_COD.LAVCOD_LAVAGGIO_SALA_LATTE,
                LAV_COD.LAVCOD_SISTEMAZIONE_PAGLIA_LETTIERE,
                LAV_COD.LAVCOD_RIMOZIONE_DEIEZIONI_MANUALE,
                LAV_COD.LAVCOD_RIMOZIONE_DEIEZIONI_NASTRO_TRASPORTATORE,
                LAV_COD.LAVCOD_SISTEMAZIONE_LETAME_CONCIMAIA,
                LAV_COD.LAVCOD_ALIMENTAZIONE_PULIZIA_IMPIANTI,
                LAV_COD.LAVCOD_ALIMENTAZIONE_DISTRIBUZIONE_MANUALE_FORAGGI,
                LAV_COD.LAVCOD_ALIMENTAZIONE_DISTRIBUZIONE_MECCANICA_FORAGGI,
                LAV_COD.LAVCOD_ALIMENTAZIONE_DISTRIBUZIONE_MANUALE_MANGIMI,
                LAV_COD.LAVCOD_ALIMENTAZIONE_DISTRIBUZIONE_MECCANICA_MANGIMI,
                LAV_COD.LAVCOD_ALIMENTAZIONE_CONTROLLO_REGOLAZIONE_SISTEMI,
                LAV_COD.LAVCOD_ASSISTENZA_PARTI,
                LAV_COD.LAVCOD_CONTROLLI_FECONDAZIONI_BOVINE,
                LAV_COD.LAVCOD_VACCINAZIONI_ANIMALI,
                LAV_COD.LAVCOD_CUREMEDICAMENTI_ANIMALI,
                LAV_COD.LAVCOD_ASSISTENZA_VETERINARIO,
                LAV_COD.LAVCOD_RACCOLTA_UOVA,
                LAV_COD.LAVCOD_RACCOLTA_MIELE,
                LAV_COD.LAVCOD_ALTRE_LAVORAZIONI_ZOO,
                LAV_COD.LAVCOD_TRASFERIMENTO_ANIMALI
            };

            var listaCauMovTrattamentiAlimentazione = new List<string>
            {
                CAU_MOV.CAU_TRATTAMENTO_ZOO,
                CAU_MOV.CAU_MACELLAZIONE,
                CAU_MOV.CAU_ALIMENTAZIONE,
                CAU_MOV.CAU_LAVORAZIONE_ZOO
            };

            var listaLavCodPesiAnalisi = new List<int>
            {
                LAV_COD.LAVCOD_PESATURA_ANIMALI,
                LAV_COD.LAVCOD_ANALISI_LATTE_SINGOLA,
                LAV_COD.LAVCOD_ANALISI_LATTE_MASSA
            };

            var listaCauMovPesiAnalisi = new List<string>
            {
                CAU_MOV.CAU_PESATURA_ANIMALI,
                CAU_MOV.CAU_EVENTI,
                CAU_MOV.CAU_ANALISI_LATTE,
                CAU_MOV.CAU_ANIMALE
            };


            //Movimenti di carico e spostamenti dei capi in giacenza
            Stb.AppendLine("--Movimenti di carico e spostamenti dei capi in giacenza");
            Stb.AppendLine(" SELECT ").
                AppendLine(" mdes.Piva, ").
                AppendLine(" mdes.Sa_Cod, ").
                AppendLine(" mdes.Id_Destinazione, ").
                AppendLine(" md.Cod_Progetto, ").
                AppendLine(" m.Cau_Mov, ").
                AppendLine(" m.Id_Mov, ").
                AppendLine(" m.Data_Movimento ").
                AppendLine(" INTO #movspos ").
                AppendLine(" FROM Agenda a (NOLOCK) ").
                AppendLine(" INNER JOIN Movimenti m (NOLOCK) ON m.PIVA = a.PIVA AND m.Id_Agenda = a.Id_Agenda ").
                AppendLine(" INNER JOIN Movimenti_dettagli md (NOLOCK) ON md.PIVA = m.Piva AND md.Id_Agenda = m.Id_Agenda AND md.Id_Mov = m.Id_Mov ").
                AppendLine(" INNER JOIN Mov_Destinazioni mdes (NOLOCK) ON mdes.PIVA = md.Piva AND mdes.Id_Agenda = md.Id_Agenda AND mdes.Id_Mov = md.Id_Mov AND mdes.Id_Mov_Det = md.Id_Mov_Det ").
                AppendLine(" INNER JOIN Zoo_Animali ON md.Piva = Zoo_Animali.Piva AND md.Cod_Progetto = Zoo_Animali.Cod_Progetto AND Zoo_Animali.Sa_Cod = 0 ").
                AppendLine(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(piva)) Stb.AppendLine(" AND a.PIVA = @Piva ");

            if (sa_Cod != 0) Stb.AppendLine(" AND mdes.sa_cod = @SaCod ");

            Stb.AppendLine(" AND a.Lav_Cod IN (@ListaLavCodCaricoSpostamenti) ").
                AppendLine(" AND m.Cau_Mov = @CAU_CARICO_CAPO ").
                AppendLine(" AND m.Data_Movimento <= Convert(Datetime,@ValiditaFineGiacenze,120) ").
                AppendLine(" AND md.Elem_Cod = @ZOO_CONSISTENZA ").
                AppendLine(" AND md.Jolly_Int = 0 ").
                AppendLine(" AND Zoo_Animali.Validita_Fine >= Convert(Datetime,@ValiditaInizio,120) ").
                AppendLine(" AND mdes.Tipo_Destinazione = @TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA ");

            Stb.AppendLine(" ALTER TABLE #movspos alter column Piva nvarchar(25) NOT NULL ");
            Stb.AppendLine(" ALTER TABLE #movspos alter column Cod_Progetto int NOT NULL ");
            Stb.AppendLine(" ALTER TABLE #movspos alter column Id_Mov int NOT NULL ");
            Stb.AppendLine(" ALTER TABLE #movspos ADD PRIMARY KEY CLUSTERED (Piva, Cod_Progetto, Id_Mov) ");

            if (!string.IsNullOrEmpty(piva)) sqlParams.TryAdd("@Piva", piva);
            if (sa_Cod != 0) sqlParams.TryAdd("@SaCod", sa_Cod);
            if (sta_Num != 0) sqlParams.TryAdd("@sta_Num", sta_Num);
            sqlParamsIn.TryAdd("@ListaLavCodCaricoSpostamenti", FormatClauseIn(listaLavCodCaricoSpostamenti));
            sqlParams.TryAdd("@CAU_CARICO_CAPO", CAU_MOV.CAU_CARICO_CAPO);
            sqlParams.TryAdd("@ValiditaFineGiacenze", validita_FineGiacenze);
            sqlParams.TryAdd("@ZOO_CONSISTENZA", ELEM_COD.ZOO_CONSISTENZA);
            sqlParams.TryAdd("@ValiditaInizio", validita_Inizio);
            sqlParams.TryAdd("@TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA", TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA);


            //Ricava l'ultimo carico o spostamento effettuato su ogni capo
            Stb.AppendLine("--Ricava l'ultimo carico o spostamento effettuato su ogni capo ");
            Stb.AppendLine(" SELECT ").
                AppendLine(" movspos.PIVA, ").
                AppendLine(" movspos.Sa_Cod, ").
                AppendLine(" movspos.Id_Destinazione, ").
                AppendLine(" movspos.Cod_Progetto, ").
                AppendLine(" movspos.Cau_Mov, ").
                AppendLine(" movspos.Data_Movimento, ").
                AppendLine(" ROW_NUMBER() OVER (PARTITION BY movspos.Cod_Progetto ORDER BY movspos.Data_Movimento DESC) as rn ").
                AppendLine(" INTO #last_movspos ").
                AppendLine(" FROM #movspos movspos ");

            Stb.AppendLine(" ALTER TABLE #last_movspos alter column Cod_Progetto int NOT NULL ");
            Stb.AppendLine(" ALTER TABLE #last_movspos alter column rn int NOT NULL ");
            Stb.AppendLine(" ALTER TABLE #last_movspos ADD PRIMARY KEY CLUSTERED (Cod_Progetto, rn) ");


            //JOIN su centri, stalle e raggruppamenti degli ultimi movimenti ricavati
            Stb.AppendLine("--JOIN su centri, stalle e raggruppamenti degli ultimi movimenti ricavati ");
            Stb.AppendLine(" SELECT ").
                     AppendLine(" last_movspos.Cod_Progetto, ").
                     AppendLine(" last_movspos.Sa_Cod, ").
                     AppendLine(" last_movspos.Cau_Mov, ").
                     AppendLine(" ca.sa_nome, ").
                     AppendLine(" last_movspos.Data_Movimento, ").
                     AppendLine(" sta.STA_NUM, ").
                     AppendLine(" sta.STA_DES ").
                     AppendLine(" INTO #agn_cte ").
                     AppendLine(" FROM #last_movspos last_movspos ").
                     AppendLine(" INNER JOIN Centri_Aziendali ca (NOLOCK) ON ca.PIVA = last_movspos.Piva COLLATE SQL_Latin1_General_CP850_CI_AS AND ca.sa_cod = last_movspos.Sa_Cod ").
                     AppendLine(" INNER JOIN Stalla_Raggruppamenti stra (NOLOCK) ON last_movspos.Id_Destinazione = stra.Raggruppamento_Cod ").
                     AppendLine(" INNER JOIN Stalla sta (NOLOCK) ON sta.PIVA = stra.PIVA AND sta.sa_cod= stra.sa_cod AND sta.STA_NUM = stra.STA_NUM ").
                     AppendLine(" WHERE last_movspos.rn = 1 ");
            //if (sa_Cod != 0) Stb.AppendLine(" AND ca.Sa_Cod = @SaCod ");
            if (sta_Num != 0) Stb.AppendLine(" AND sta.Sta_Num = @sta_Num ");

            Stb.AppendLine(" ALTER TABLE #agn_cte alter column Cod_Progetto int NOT NULL ");
            //Stb.AppendLine(" ALTER TABLE #agn_cte alter column Data_Movimento datetime NOT NULL ");
            //Stb.AppendLine(" ALTER TABLE #agn_cte ADD PRIMARY KEY CLUSTERED (Cod_Progetto, Data_Movimento) ");
            Stb.AppendLine(" ALTER TABLE #agn_cte ADD PRIMARY KEY CLUSTERED (Cod_Progetto) ");

            //Tabella temporanea con Spostamenti
            Stb.AppendLine("--Tabella spostamenti ");
            Stb.AppendLine(" SELECT  ");
            Stb.AppendLine("     Agenda.Piva   ");
            Stb.AppendLine("   , Centri_Aziendali.Sa_Cod   ");
            Stb.AppendLine("   , Agenda.Id_Agenda   ");
            Stb.AppendLine("   , Agenda.Id_Agenda  as ID ");
            Stb.AppendLine("   , Agenda.Lav_Cod   ");
            Stb.AppendLine("   , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("   , Agenda.Blocco_Flag ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data] ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data2] ");
            Stb.AppendLine("   , Movimenti.Ora   ");
            Stb.AppendLine("   , 0 as ID_Mov_Det ");
            Stb.AppendLine("   , '' as Info ");
            Stb.AppendLine("   , '' as Dettagli ");
            Stb.AppendLine("   , 0 as Veg_Cod ");
            Stb.AppendLine("   , Agenda.Username_Creazione   ");
            Stb.AppendLine("   , 'N.D.' AS Tecnico   ");
            Stb.AppendLine("   , -1 AS Tipo_Destinazione   ");
            Stb.AppendLine("   , 0 AS Elem_Cod   ");
            Stb.AppendLine("   , 0 AS Mat_Cod   ");
            Stb.AppendLine("   , 0 AS Pro_Cod   ");
            Stb.AppendLine("   , '' AS Mat_Des   ");
            Stb.AppendLine("   , '' AS Cod_Articolo   ");
            Stb.AppendLine("   , Centri_Aziendali.sa_nome  ");
            Stb.AppendLine("   , i.rag_soc   ");
            Stb.AppendLine("   , Operazioni.lav_des   ");
            Stb.AppendLine("   , Operazioni.lav_des  as [Operazione_DES] ");
            //Stb.AppendLine("   , GruppoOperazioni.gru_Des   ");
            //Stb.AppendLine("   , ISNULL(GruppoOperazioni.tipo, 'C') as tipo   ");
            Stb.AppendLine("   , '' as tipo_colore ");
            Stb.AppendLine("   , 0 AS contabilizzato   ");
            Stb.AppendLine("   , '' as LottiProduzione ");
            Stb.AppendLine("   , '' AS Nota_Des   ");
            Stb.AppendLine("   , '' AS Note ");
            Stb.AppendLine("   , '' as Costi_Operatori ");
            Stb.AppendLine("   , '' as Costi_Macchine ");
            Stb.AppendLine("   , 0 as Sup_Trattata ");
            Stb.AppendLine("   , '' as LottiImpianto ");
            Stb.AppendLine("   , '' as Descrizione_Unica ");
            Stb.AppendLine("   , '' as Prodotti ");
            Stb.AppendLine("   , ISNULL(Attivita.Sigla, '') AS AttivitaSigla   ");
            Stb.AppendLine("   , ISNULL(Attivita.[Desc], '') AS AttivitaDesc   ");
            Stb.AppendLine("   , '' AS RifDdtFatture  ");
            Stb.AppendLine("   , Utenti.[user] as Creatore_Intervento ");
            Stb.AppendLine("   , Stalla.STA_DES ");
            Stb.AppendLine("   , Stalla.STA_NUM ");
            Stb.AppendLine("   , Stalla_Raggruppamenti.Raggruppamento_Des ");
            Stb.AppendLine("   , STRING_AGG(CAST(Zoo_Animali.Matricola as nvarchar(max)), ', ') as Matricole ");
            Stb.AppendLine("   , STRING_AGG(CAST(IIF(Zoo_Animali.Matricola IS NULL, Movimenti_dettagli.Mov_Det_Des, NULL) AS NVARCHAR(MAX)), ', ') AS Segnalazioni ");
            Stb.AppendLine("   , '' As PermessoModifica ");
            Stb.AppendLine("   , COUNT(*) as NumeroCapi ");
            Stb.AppendLine("   INTO #spostamenti ");
            Stb.AppendLine(" FROM Agenda (NOLOCK)  ");
            Stb.AppendLine(" JOIN Movimenti (NOLOCK)  ON Agenda.Id_Agenda = Movimenti.ID_Agenda ");
            Stb.AppendLine(" LEFT JOIN Movimenti (NOLOCK)  Mov_Lav ON Agenda.Id_Agenda = Mov_Lav.ID_Agenda AND Mov_Lav.Cau_Mov = '4000' ");
            Stb.AppendLine(" JOIN dbo.Operazioni (NOLOCK)  On Agenda.Lav_Cod = Operazioni.Lav_Cod ");
            //Stb.AppendLine(" JOIN dbo.GruppoOperazioni (NOLOCK)  On Operazioni.GRU_OP = GruppoOperazioni.GRU_COD ");
            Stb.AppendLine(" JOIN imprese (NOLOCK)  i On i.piva = Agenda.PIVA ");
            Stb.AppendLine(" LEFT JOIN Attivita (NOLOCK)  ON Agenda.Id_Attivita = Attivita.Id_Attivita ");
            Stb.AppendLine(" LEFT JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.ID_Agenda = Movimenti_dettagli.Id_Agenda  ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Elem_Cod = 300 ");
            Stb.AppendLine(" JOIN Mov_Destinazioni (NOLOCK)  ON Movimenti_dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Id_Mov = Mov_Destinazioni.Id_Mov ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det ");
            Stb.AppendLine("                    AND Mov_Destinazioni.Tipo_Destinazione = 21 ");
            Stb.AppendLine("  JOIN Stalla_Raggruppamenti (NOLOCK)  ON Mov_Destinazioni.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod ");
            Stb.AppendLine("  JOIN Stalla (NOLOCK)  ON Stalla_Raggruppamenti.Piva = Stalla.PIVA ");
            Stb.AppendLine("            AND Stalla_Raggruppamenti.sa_cod = Stalla.sa_cod ");
            Stb.AppendLine("            AND Stalla_Raggruppamenti.STA_NUM = Stalla.STA_NUM ");
            Stb.AppendLine(" JOIN Centri_Aziendali (NOLOCK)  ON Stalla.Piva = Centri_Aziendali.Piva AND Stalla.Sa_Cod = Centri_Aziendali.sa_cod ");
            Stb.AppendLine(" LEFT JOIN Zoo_Animali (NOLOCK)  ON Movimenti_dettagli.Piva = Zoo_Animali.Piva AND Movimenti_dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto AND Zoo_Animali.Sa_Cod = 0 ");
            Stb.AppendLine(" JOIN Utenti (NOLOCK)  ON Agenda.Username_Creazione = Utenti.CODICE_FISCALE ");

            Stb.AppendLine(" WHERE (Agenda.Lav_Cod = @LAVCOD_SPOSTAMENTI_ZOO) ");
            Stb.AppendLine("  AND Movimenti.Cau_Mov = @CAU_CARICO_CAPO ");

            if (!string.IsNullOrEmpty(piva)) Stb.AppendLine(" AND i.PIVA = @Piva ");

            if (filtroCentri.Any()) Stb.AppendLine(" AND Centri_Aziendali.Sa_Cod IN (@FiltroCentri) ");

            if (sa_Cod != 0) Stb.AppendLine(" AND Centri_Aziendali.Sa_Cod = @SaCod ");
            
            if (sta_Num != 0) Stb.AppendLine(" AND Stalla.Sta_Num = @sta_Num ");

            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) >= Convert(Datetime,@ValiditaInizio,120) ");
            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) <= Convert(Datetime,@ValiditaFine,120) ");

            Stb.AppendLine(" AND Movimenti.Data_Movimento >= Convert(Datetime,@FinestraTemporaleInizio,120) ");
            Stb.AppendLine(" AND Movimenti.Data_Movimento <= Convert(Datetime,@FinestraTemporaleFine,120) ");

            Stb.AppendLine("  GROUP BY Agenda.Piva  ");
            Stb.AppendLine("  , Centri_Aziendali.Sa_Cod  ");
            Stb.AppendLine("  , Agenda.Id_Agenda  ");
            Stb.AppendLine("  , Agenda.Lav_Cod  ");
            Stb.AppendLine("  , Movimenti.Data_Movimento  ");
            Stb.AppendLine("  , Movimenti.Ora  ");
            Stb.AppendLine("  , Mov_Lav.Extra_Str ");
            Stb.AppendLine("  , Agenda.Username_Creazione  ");
            Stb.AppendLine("  , Agenda.Blocco_Flag  ");
            Stb.AppendLine("  , Centri_Aziendali.sa_nome ");
            Stb.AppendLine("  , i.rag_soc  ");
            Stb.AppendLine("  , Operazioni.lav_des  ");
            //Stb.AppendLine("  , GruppoOperazioni.gru_Des  ");
            //Stb.AppendLine("  , GruppoOperazioni.tipo ");
            Stb.AppendLine("  , Attivita.Sigla ");
            Stb.AppendLine("  , Attivita.[Desc] ");
            Stb.AppendLine("  , Utenti.[user] ");
            Stb.AppendLine("  , Stalla_Raggruppamenti.Raggruppamento_Des ");
            Stb.AppendLine("  , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("  , Stalla.STA_DES ");
            Stb.AppendLine("  , Stalla.STA_NUM ");
            Stb.AppendLine("   ");

            sqlParams.TryAdd("@LAVCOD_SPOSTAMENTI_ZOO", LAV_COD.LAVCOD_SPOSTAMENTI_ZOO);
            if (filtroCentri.Any()) sqlParamsIn.TryAdd("@FiltroCentri", FormatClauseIn(filtroCentri)); //TODO: verificare se è una stringa concatenata
            sqlParams.TryAdd("@ValiditaFine", validita_Fine);
            sqlParams.TryAdd("@FinestraTemporaleInizio", objParams.FinestraTemporaleInizio);
            sqlParams.TryAdd("@FinestraTemporaleFine", objParams.FinestraTemporaleFine);


            //Carichi/Scarichi
            Stb.AppendLine(" --Carichi/Scarichi  ");
            Stb.AppendLine(" SELECT  ");
            Stb.AppendLine("     Agenda.Piva   ");
            Stb.AppendLine("   , Centri_Aziendali.Sa_Cod   ");
            Stb.AppendLine("   , Agenda.Id_Agenda   ");
            Stb.AppendLine("   , Agenda.Id_Agenda  as ID ");
            Stb.AppendLine("   , Agenda.Lav_Cod   ");
            Stb.AppendLine("   , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("   , Agenda.Blocco_Flag ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data] ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data2] ");
            Stb.AppendLine("   , Movimenti.Ora   ");
            Stb.AppendLine("   , 0 as ID_Mov_Det ");
            Stb.AppendLine("   , '' as Info ");
            Stb.AppendLine("   , '' as Dettagli ");
            Stb.AppendLine("   , 0 as Veg_Cod ");
            Stb.AppendLine("   , Agenda.Username_Creazione   ");
            Stb.AppendLine("   , Agenda.Blocco_Flag   ");
            Stb.AppendLine("   , 'N.D.' AS Tecnico   ");
            Stb.AppendLine("   , -1 AS Tipo_Destinazione   ");
            Stb.AppendLine("   , 0 AS Elem_Cod   ");
            Stb.AppendLine("   , 0 AS Mat_Cod   ");
            Stb.AppendLine("   , 0 AS Pro_Cod   ");
            Stb.AppendLine("   , '' AS Mat_Des   ");
            Stb.AppendLine("   , '' AS Cod_Articolo   ");
            Stb.AppendLine("   , Centri_Aziendali.sa_nome  ");
            Stb.AppendLine("   , i.rag_soc   ");
            Stb.AppendLine("   , Operazioni.lav_des   ");
            Stb.AppendLine("   , Operazioni.lav_des  as [Operazione_DES] ");
            //Stb.AppendLine("   , GruppoOperazioni.gru_Des   ");
            //Stb.AppendLine("   , ISNULL(GruppoOperazioni.tipo, 'C') as tipo   ");
            Stb.AppendLine("   , '' as tipo_colore ");
            Stb.AppendLine("   , 0 AS contabilizzato   ");
            Stb.AppendLine("   , '' as LottiProduzione ");
            Stb.AppendLine("   , '' AS Nota_Des   ");
            Stb.AppendLine("   , '' AS Note ");
            Stb.AppendLine("   , '' as Costi_Operatori ");
            Stb.AppendLine("   , '' as Costi_Macchine ");
            Stb.AppendLine("   , 0 as Sup_Trattata ");
            Stb.AppendLine("   , '' as LottiImpianto ");
            Stb.AppendLine("   , '' as Descrizione_Unica ");
            Stb.AppendLine("   , '' as Prodotti ");
            Stb.AppendLine("   , ISNULL(Attivita.Sigla, '') AS AttivitaSigla   ");
            Stb.AppendLine("   , ISNULL(Attivita.[Desc], '') AS AttivitaDesc   ");
            Stb.AppendLine("   , '' AS RifDdtFatture  ");
            Stb.AppendLine("   , Utenti.[user] as Creatore_Intervento ");
            Stb.AppendLine("   , IIF(Mov_Lav.Extra_Str IS NULL OR TRIM(Mov_Lav.Extra_Str) = '', Stalla.STA_DES + ' (N. Capi:' + CAST(COUNT(*) as varchar(250)) + ' ) ' + ': ' + STRING_AGG(CAST(Zoo_Animali.Matricola as nvarchar(max)), ', '), Stalla.STA_DES + ' (N. Capi:' + CAST(COUNT(*) as varchar(250)) + ' ) ' + ': ' + COALESCE(Mov_Lav.Extra_Str + ': ', '') + STRING_AGG(CAST(Zoo_Animali.Matricola as nvarchar(max)), ', ')) As Dettaglio_Tecnico ");
            Stb.AppendLine("   , '' AS Segnalazioni ");
            Stb.AppendLine("   , '' As PermessoModifica ");
            Stb.AppendLine("   , Stalla.STA_DES ");
            Stb.AppendLine("   , Stalla.STA_NUM ");

            Stb.AppendLine(" FROM Agenda (NOLOCK)  ");
            Stb.AppendLine(" JOIN Movimenti (NOLOCK)  ON Agenda.Id_Agenda = Movimenti.ID_Agenda ");
            Stb.AppendLine(" LEFT JOIN Movimenti (NOLOCK)  Mov_Lav ON Agenda.Id_Agenda = Mov_Lav.ID_Agenda AND Mov_Lav.Cau_Mov = '4000' ");
            Stb.AppendLine(" JOIN dbo.Operazioni (NOLOCK)  On Agenda.Lav_Cod = Operazioni.Lav_Cod ");
            //Stb.AppendLine(" JOIN dbo.GruppoOperazioni (NOLOCK)  On Operazioni.GRU_OP = GruppoOperazioni.GRU_COD ");
            Stb.AppendLine(" JOIN imprese (NOLOCK)  i On i.piva = Agenda.PIVA ");
            Stb.AppendLine(" LEFT JOIN Attivita (NOLOCK)  ON Agenda.Id_Attivita = Attivita.Id_Attivita ");
            Stb.AppendLine(" JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.ID_Agenda = Movimenti_dettagli.Id_Agenda  ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Elem_Cod = 300 ");
            Stb.AppendLine(" JOIN Mov_Destinazioni  (NOLOCK) ON Movimenti_dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Id_Mov = Mov_Destinazioni.Id_Mov ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det ");
            Stb.AppendLine("                    AND Mov_Destinazioni.Tipo_Destinazione = 21 ");
            Stb.AppendLine("  JOIN Stalla_Raggruppamenti (NOLOCK)  ON Mov_Destinazioni.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod ");
            Stb.AppendLine("  JOIN Stalla (NOLOCK)  ON Stalla_Raggruppamenti.Piva = Stalla.PIVA ");
            Stb.AppendLine("            AND Stalla_Raggruppamenti.sa_cod = Stalla.sa_cod ");
            Stb.AppendLine("            AND Stalla_Raggruppamenti.STA_NUM = Stalla.STA_NUM ");
            Stb.AppendLine(" JOIN Centri_Aziendali (NOLOCK)  ON Stalla.Piva = Centri_Aziendali.Piva AND Stalla.Sa_Cod = Centri_Aziendali.sa_cod ");
            Stb.AppendLine(" JOIN Zoo_Animali (NOLOCK)  ON Movimenti_dettagli.Piva = Zoo_Animali.Piva AND Movimenti_dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto AND Zoo_Animali.Sa_Cod = 0 ");
            Stb.AppendLine(" JOIN Utenti (NOLOCK)  ON Agenda.Username_Creazione = Utenti.CODICE_FISCALE ");

            Stb.AppendLine(" WHERE (Agenda.Lav_Cod IN (@ListaLavCodCarichiScarichi)) ");
            Stb.AppendLine(" AND Movimenti.Cau_Mov IN (@ListaCauMovCarichiScarichi) ");

            if (!string.IsNullOrEmpty(piva)) Stb.AppendLine(" AND i.PIVA = @Piva ");

            if (filtroCentri.Any()) Stb.AppendLine(" AND Centri_Aziendali.Sa_Cod IN (@FiltroCentri) ");

            if (sa_Cod != 0) Stb.AppendLine(" AND Centri_Aziendali.Sa_Cod = @SaCod ");

            if (sta_Num != 0) Stb.AppendLine(" AND Stalla.Sta_Num = @sta_Num ");

            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) >= Convert(Datetime,@ValiditaInizio,120) ");
            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) <= Convert(Datetime,@ValiditaFine,120) ");

            Stb.AppendLine(" AND Movimenti.Data_Movimento >= Convert(Datetime,@FinestraTemporaleInizio,120) ");
            Stb.AppendLine(" AND Movimenti.Data_Movimento <= Convert(Datetime,@FinestraTemporaleFine,120) ");

            Stb.AppendLine("  GROUP BY Agenda.Piva  ");
            Stb.AppendLine("  , Centri_Aziendali.Sa_Cod  ");
            Stb.AppendLine("  , Agenda.Id_Agenda  ");
            Stb.AppendLine("  , Agenda.Lav_Cod  ");
            Stb.AppendLine("  , Movimenti.Data_Movimento  ");
            Stb.AppendLine("  , Movimenti.Ora  ");
            Stb.AppendLine("  , Movimenti.Ora  ");
            Stb.AppendLine("  , Mov_Lav.Extra_Str ");
            Stb.AppendLine("  , Agenda.Username_Creazione  ");
            Stb.AppendLine("  , Agenda.Blocco_Flag  ");
            Stb.AppendLine("  , Centri_Aziendali.sa_nome ");
            Stb.AppendLine("  , i.rag_soc  ");
            Stb.AppendLine("  , Operazioni.lav_des  ");
            //Stb.AppendLine("  , GruppoOperazioni.gru_Des  ");
            //Stb.AppendLine("  , GruppoOperazioni.tipo ");
            Stb.AppendLine("  , Attivita.Sigla ");
            Stb.AppendLine("  , Attivita.[Desc] ");
            Stb.AppendLine("  , Utenti.[user] ");
            Stb.AppendLine("   , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("   , Stalla.STA_DES ");
            Stb.AppendLine("   , Stalla.STA_NUM ");
            Stb.AppendLine("   ");

            sqlParamsIn.Add("@ListaLavCodCarichiScarichi", FormatClauseIn(listaLavCodCarichiScarichi));
            sqlParamsIn.Add("@ListaCauMovCarichiScarichi", FormatClauseIn(listaCauMovCarichiScarichi));

            Stb.AppendLine(" UNION ");


            //Spostamenti
            Stb.AppendLine(" --Spostamenti  ");
            Stb.AppendLine(" SELECT PIVA, sa_cod, Id_Agenda, ID, Lav_Cod, Tipo_Accettazione, Blocco_Flag,   ");
            Stb.AppendLine(" --Tipo_Destinazione,  ");
            Stb.AppendLine(" [Data], Data2, Ora, ID_Mov_Det, Info, Dettagli, Veg_Cod,  ");
            Stb.AppendLine(" CAST(Username_Creazione as nvarchar(25)) as Username_Creazione,  ");
            Stb.AppendLine(" Blocco_Flag,  ");
            Stb.AppendLine(" Tecnico, Tipo_Accettazione, Elem_Cod, Mat_Cod, Pro_Cod, Mat_Des, Cod_Articolo, sa_nome, rag_soc, LAV_DES, Operazione_DES,  ");
            //Stb.AppendLine(" GRU_DES, tipo, ");
            Stb.AppendLine(" tipo_colore, contabilizzato, LottiProduzione, Nota_Des, Note,  ");
            Stb.AppendLine(" Costi_Operatori, Costi_Macchine, Sup_Trattata, LottiImpianto, Descrizione_Unica, Prodotti, AttivitaSigla, AttivitaDesc, RifDdtFatture,  ");
            Stb.AppendLine(" Creatore_Intervento,   ");
            Stb.AppendLine(" STA_DES + ' - Destinazione: ' + STRING_AGG(Raggruppamento_Des, ',') + ' ' + ' (N. Capi:' + CAST(SUM(NumeroCapi) as varchar(250)) + ' ) ' + ': ' + STRING_AGG(CAST(Matricole as nvarchar(max)), ', '),  ");
            Stb.AppendLine(" COALESCE(Segnalazioni, ''), PermessoModifica, STA_DES, STA_NUM ");
            Stb.AppendLine(" FROM #spostamenti ");
            Stb.AppendLine(" GROUP BY PIVA, sa_cod, Id_Agenda, ID, lav_Cod, Tipo_Accettazione, Blocco_Flag, Tipo_Destinazione, [Data], Data2, Ora, ID_Mov_Det, Info, Dettagli, Veg_Cod, Username_Creazione, Tecnico, Tipo_Accettazione, Elem_Cod, Mat_Cod, Pro_Cod, Mat_Des, Cod_Articolo, sa_nome, rag_soc, LAV_DES, Operazione_DES,  ");
            //Stb.AppendLine(" GRU_DES, tipo,  ");
            Stb.AppendLine(" tipo_colore, contabilizzato, LottiProduzione, Nota_Des, Note, Costi_Operatori, Costi_Macchine, Sup_Trattata, LottiImpianto, Descrizione_Unica, Prodotti, AttivitaSigla, AttivitaDesc, RifDdtFatture, Creatore_Intervento, STA_DES, STA_NUM, Segnalazioni, PermessoModifica ");

            Stb.AppendLine("  UNION ");


            //Trattamenti/Alimentazione
            Stb.AppendLine(" --Trattamenti/Alimentazione  ");
            Stb.AppendLine(" SELECT  ");
            Stb.AppendLine("     Agenda.Piva   ");
            Stb.AppendLine("   , g.Sa_Cod   ");
            Stb.AppendLine("   , Agenda.Id_Agenda   ");
            Stb.AppendLine("   , Agenda.Id_Agenda  as ID ");
            Stb.AppendLine("   , Agenda.Lav_Cod   ");
            Stb.AppendLine("   , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("   , Agenda.Blocco_Flag ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data] ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data2] ");
            Stb.AppendLine("   , Movimenti.Ora   ");
            Stb.AppendLine("   , 0 as ID_Mov_Det ");
            Stb.AppendLine("   , '' as Info ");
            Stb.AppendLine("   , '' as Dettagli ");
            Stb.AppendLine("   , 0 as Veg_Cod ");
            Stb.AppendLine("   , Agenda.Username_Creazione   ");
            Stb.AppendLine("   , Agenda.Blocco_Flag   ");
            Stb.AppendLine("   , 'N.D.' AS Tecnico   ");
            Stb.AppendLine("   , -1 AS Tipo_Destinazione   ");
            Stb.AppendLine("   , 0 AS Elem_Cod   ");
            Stb.AppendLine("   , 0 AS Mat_Cod   ");
            Stb.AppendLine("   , COALESCE(dettagli_farmaci.pro_Cod, dettagli_mat.pro_cod, 0) AS Pro_Cod   ");
            Stb.AppendLine("   , COALESCE(Farmaci.Denominazione, Materie_Prime.Mat_Des, '') AS Mat_Des   ");
            Stb.AppendLine("   , '' AS Cod_Articolo   ");
            Stb.AppendLine("   , g.sa_nome  ");
            Stb.AppendLine("   , i.rag_soc   ");
            Stb.AppendLine("   , Operazioni.lav_des   ");
            Stb.AppendLine("   , Operazioni.lav_des  as [Operazione_DES] ");
            //Stb.AppendLine("   , GruppoOperazioni.gru_Des   ");
            //Stb.AppendLine("   , ISNULL(GruppoOperazioni.tipo, 'C') as tipo   ");
            Stb.AppendLine("   , '' as tipo_colore ");
            Stb.AppendLine("   , 0 AS contabilizzato   ");
            Stb.AppendLine("   , '' as LottiProduzione ");
            Stb.AppendLine("   , '' AS Nota_Des   ");
            Stb.AppendLine("   , '' AS Note ");
            Stb.AppendLine("   , '' as Costi_Operatori ");
            Stb.AppendLine("   , '' as Costi_Macchine ");
            Stb.AppendLine("   , 0 as Sup_Trattata ");
            Stb.AppendLine("   , '' as LottiImpianto ");
            Stb.AppendLine("   , '' as Descrizione_Unica ");
            Stb.AppendLine("   , COALESCE(Farmaci.Denominazione, Materie_Prime.Mat_Des, '') as Prodotti  ");
            Stb.AppendLine("   , ISNULL(Attivita.Sigla, '') AS AttivitaSigla   ");
            Stb.AppendLine("   , ISNULL(Attivita.[Desc], '') AS AttivitaDesc   ");
            Stb.AppendLine("   , '' AS RifDdtFatture  ");
            Stb.AppendLine("   , Utenti.[user] as Creatore_Intervento ");
            Stb.AppendLine("   , COALESCE(Ricette_Zoo_Agenda.Note, '') + ' ' + IIF(g.STA_DES IS NULL, '', g.STA_DES + ' (N. Capi:' + CAST(COUNT(*) as varchar(250)) + ' ) ' + ': ')  + STRING_AGG(CAST(Zoo_Animali.Matricola as nvarchar(max)), ', ') as Dettaglio_Tecnico ");
            Stb.AppendLine("   , '' AS Segnalazioni ");
            Stb.AppendLine("   , '' As PermessoModifica ");
            Stb.AppendLine("   , g.STA_DES ");
            Stb.AppendLine("   , g.STA_NUM ");

            Stb.AppendLine(" FROM Agenda (NOLOCK)  ");
            Stb.AppendLine(" JOIN Movimenti (NOLOCK)  ON Agenda.Id_Agenda = Movimenti.ID_Agenda AND Agenda.Piva = Movimenti.Piva ");
            Stb.AppendLine(" JOIN dbo.Operazioni (NOLOCK)  On Agenda.Lav_Cod = Operazioni.Lav_Cod ");
            //Stb.AppendLine(" JOIN dbo.GruppoOperazioni (NOLOCK)  On Operazioni.GRU_OP = GruppoOperazioni.GRU_COD ");
            Stb.AppendLine(" JOIN imprese (NOLOCK)  i On i.piva = Agenda.PIVA ");
            Stb.AppendLine(" LEFT JOIN Centri_Aziendali (NOLOCK)  ON Agenda.Piva = Centri_Aziendali.Piva AND Agenda.Sa_Cod = Centri_Aziendali.sa_cod ");
            Stb.AppendLine(" LEFT JOIN Attivita (NOLOCK)  ON Agenda.Id_Attivita = Attivita.Id_Attivita ");
            Stb.AppendLine(" LEFT JOIN Movimenti_dettagli (NOLOCK)  dettagli_farmaci ON Movimenti.Piva = dettagli_farmaci.Piva AND Movimenti.ID_Agenda = dettagli_farmaci.Id_Agenda  ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = dettagli_farmaci.Id_Mov ");
            Stb.AppendLine("                    AND dettagli_farmaci.Elem_Cod = 307 ");
            Stb.AppendLine(" LEFT JOIN Farmaci (NOLOCK)  ON dettagli_farmaci.Pro_Cod = Farmaci.Farm_Cod ");
            Stb.AppendLine(" LEFT JOIN Movimenti_dettagli (NOLOCK)  dettagli_mat ON Movimenti.Piva = dettagli_mat.Piva AND Movimenti.ID_Agenda = dettagli_mat.Id_Agenda  ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = dettagli_mat.Id_Mov ");
            Stb.AppendLine("                    AND dettagli_mat.Elem_Cod <> 307 ");
            Stb.AppendLine(" LEFT JOIN Materie_Prime (NOLOCK)  ON dettagli_mat.Mat_Cod = Materie_Prime.Mat_Cod ");
            Stb.AppendLine(" JOIN Mov_Destinazioni (NOLOCK)  ON Movimenti.Piva = Mov_Destinazioni.Piva AND Movimenti.ID_Agenda = Mov_Destinazioni.Id_Agenda ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = Mov_Destinazioni.Id_Mov ");
            Stb.AppendLine("                    AND Mov_Destinazioni.Tipo_Destinazione = 1 ");
            Stb.AppendLine("                    AND Mov_Destinazioni.Id_Destinazione > 0 ");
            Stb.AppendLine(" JOIN Zoo_Animali (NOLOCK)  ON Mov_Destinazioni.ID_Destinazione = Zoo_Animali.Cod_Progetto AND Mov_Destinazioni.Piva = Zoo_Animali.Piva AND Zoo_Animali.Sa_Cod = 0 ");
            Stb.AppendLine(" JOIN #agn_cte g ON g.Cod_Progetto = Zoo_Animali.Cod_Progetto  ");
            Stb.AppendLine(" JOIN Utenti (NOLOCK)  ON Agenda.Username_Creazione = Utenti.CODICE_FISCALE ");

            Stb.AppendLine(" LEFT JOIN Ricette_ZooxAgenda ON Agenda.Id_Agenda = Ricette_ZooxAgenda.Id_Agenda ");
            Stb.AppendLine(" LEFT JOIN Ricette_Zoo_Agenda ON Ricette_ZooxAgenda.Id_Ricetta = Ricette_Zoo_Agenda.IdRicetta AND Ricette_ZooxAgenda.Id_RigaRicetta = Ricette_Zoo_Agenda.IdAgenda ");

            Stb.AppendLine(" WHERE (Agenda.Lav_Cod IN (@ListaLavCodTrattamentiAlimentazione)) ");
            Stb.AppendLine(" AND Movimenti.Cau_Mov IN (@ListaCauMovTrattamentiAlimentazione) ");

            if (!string.IsNullOrEmpty(piva)) Stb.AppendLine(" AND i.Piva = @Piva ");

            if (filtroCentri.Any()) Stb.AppendLine(" AND g.Sa_Cod IN (@FiltroCentri) ");

            if (sa_Cod != 0) Stb.AppendLine(" AND g.Sa_Cod = @SaCod ");

            if (sta_Num != 0) Stb.AppendLine(" AND g.Sta_Num = @sta_num ");

            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) >= Convert(Datetime,@ValiditaInizio,120) ");
            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) <= Convert(Datetime,@ValiditaFine,120) ");

            Stb.AppendLine(" AND Movimenti.Data_Movimento >= Convert(Datetime,@FinestraTemporaleInizio,120) ");
            Stb.AppendLine(" AND Movimenti.Data_Movimento <= Convert(Datetime,@FinestraTemporaleFine,120) ");

            Stb.AppendLine("  GROUP BY Agenda.Piva  ");
            Stb.AppendLine("  , g.Sa_Cod  ");
            Stb.AppendLine("  , Agenda.Id_Agenda  ");
            Stb.AppendLine("  , Agenda.Lav_Cod  ");
            Stb.AppendLine("  , Movimenti.Data_Movimento  ");
            Stb.AppendLine("  , Movimenti.Ora  ");
            Stb.AppendLine("  , dettagli_farmaci.pro_Cod ");
            Stb.AppendLine("  , dettagli_mat.pro_cod ");
            Stb.AppendLine("  , Farmaci.Denominazione ");
            Stb.AppendLine("  , Materie_Prime.Mat_Des ");
            Stb.AppendLine("  , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("  , Agenda.Username_Creazione  ");
            Stb.AppendLine("  , Agenda.Blocco_Flag  ");
            Stb.AppendLine("  , g.sa_nome  ");
            Stb.AppendLine("  , i.rag_soc  ");
            Stb.AppendLine("  , Operazioni.lav_des  ");
            //Stb.AppendLine("  , GruppoOperazioni.gru_Des  ");
            //Stb.AppendLine("  , GruppoOperazioni.tipo ");
            Stb.AppendLine("  , Attivita.Sigla ");
            Stb.AppendLine("  , Attivita.[Desc] ");
            Stb.AppendLine("  , Utenti.[user] ");
            Stb.AppendLine("  , g.STA_DES ");
            Stb.AppendLine("  , g.STA_NUM ");
            Stb.AppendLine("  , Ricette_Zoo_Agenda.Note ");
            Stb.AppendLine(" ");

            sqlParamsIn.Add("@ListaLavCodTrattamentiAlimentazione", FormatClauseIn(listaLavCodTrattamentiAlimentazione));
            sqlParamsIn.Add("@ListaCauMovTrattamentiAlimentazione", FormatClauseIn(listaCauMovTrattamentiAlimentazione));

            Stb.AppendLine("  UNION ");


            //Pesatura/Altre Lavorazioni
            Stb.AppendLine(" --Pesatura/Altre Lavorazioni  ");
            Stb.AppendLine(" SELECT  ");
            Stb.AppendLine("     Agenda.Piva   ");
            Stb.AppendLine("   , g.Sa_Cod   ");
            Stb.AppendLine("   , Agenda.Id_Agenda   ");
            Stb.AppendLine("   , Agenda.Id_Agenda  as ID ");
            Stb.AppendLine("   , Agenda.Lav_Cod   ");
            Stb.AppendLine("   , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("   , Agenda.Blocco_Flag ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data] ");
            Stb.AppendLine("   , Movimenti.Data_Movimento as [Data2] ");
            Stb.AppendLine("   , Movimenti.Ora   ");
            Stb.AppendLine("   , 0 as ID_Mov_Det ");
            Stb.AppendLine("   , '' as Info ");
            Stb.AppendLine("   , '' as Dettagli ");
            Stb.AppendLine("   , 0 as Veg_Cod ");
            Stb.AppendLine("   , Agenda.Username_Creazione   ");
            Stb.AppendLine("   , Agenda.Blocco_Flag   ");
            Stb.AppendLine("   , 'N.D.' AS Tecnico   ");
            Stb.AppendLine("   , -1 AS Tipo_Destinazione   ");
            Stb.AppendLine("   , 0 AS Elem_Cod   ");
            Stb.AppendLine("   , 0 AS Mat_Cod   ");
            Stb.AppendLine("   , 0 AS Pro_Cod   ");
            Stb.AppendLine("   , '' AS Mat_Des   ");
            Stb.AppendLine("   , '' AS Cod_Articolo   ");
            Stb.AppendLine("   , g.sa_nome  ");
            Stb.AppendLine("   , i.rag_soc   ");
            Stb.AppendLine("   , Operazioni.lav_des   ");
            Stb.AppendLine("   , Operazioni.lav_des  as [Operazione_DES] ");
            //Stb.AppendLine("   , GruppoOperazioni.gru_Des   ");
            //Stb.AppendLine("   , ISNULL(GruppoOperazioni.tipo, 'C') as tipo   ");
            Stb.AppendLine("   , '' as tipo_colore ");
            Stb.AppendLine("   , 0 AS contabilizzato   ");
            Stb.AppendLine("   , '' as LottiProduzione ");
            Stb.AppendLine("   , '' AS Nota_Des   ");
            Stb.AppendLine("   , '' AS Note ");
            Stb.AppendLine("   , '' as Costi_Operatori ");
            Stb.AppendLine("   , '' as Costi_Macchine ");
            Stb.AppendLine("   , 0 as Sup_Trattata ");
            Stb.AppendLine("   , '' as LottiImpianto ");
            Stb.AppendLine("   , '' as Descrizione_Unica ");
            Stb.AppendLine("   , '' as Prodotti ");
            Stb.AppendLine("   , ISNULL(Attivita.Sigla, '') AS AttivitaSigla   ");
            Stb.AppendLine("   , ISNULL(Attivita.[Desc], '') AS AttivitaDesc   ");
            Stb.AppendLine("   , '' AS RifDdtFatture  ");
            Stb.AppendLine("   , Utenti.[user] as Creatore_Intervento ");
            Stb.AppendLine("   , IIF(g.STA_DES IS NULL, '' + + 'N. Capi:' + CAST(COUNT(*) as varchar(250)) + ' : ', g.STA_DES + ' (N. Capi:' + CAST(COUNT(*) as varchar(250)) + ' ) ' + ': ')  + STRING_AGG(CAST(Zoo_Animali.Matricola as nvarchar(max)), ', ') As Dettaglio_Tecnico ");
            Stb.AppendLine("   , '' AS Segnalazioni ");
            Stb.AppendLine("   , '' As PermessoModifica ");
            Stb.AppendLine("   , g.STA_DES ");
            Stb.AppendLine("   , g.STA_NUM ");

            Stb.AppendLine(" FROM Agenda (NOLOCK)  ");
            Stb.AppendLine(" JOIN Movimenti  (NOLOCK) ON Agenda.Id_Agenda = Movimenti.ID_Agenda ");
            Stb.AppendLine(" JOIN dbo.Operazioni (NOLOCK)  On Agenda.Lav_Cod = Operazioni.Lav_Cod ");
            //Stb.AppendLine(" JOIN dbo.GruppoOperazioni (NOLOCK)  On Operazioni.GRU_OP = GruppoOperazioni.GRU_COD ");
            Stb.AppendLine(" JOIN imprese (NOLOCK)  i On i.piva = Agenda.PIVA ");
            Stb.AppendLine(" LEFT JOIN Centri_Aziendali (NOLOCK)  ON Agenda.Piva = Centri_Aziendali.Piva AND Agenda.Sa_Cod = Centri_Aziendali.sa_cod ");
            Stb.AppendLine(" LEFT JOIN Attivita (NOLOCK)  ON Agenda.Id_Attivita = Attivita.Id_Attivita ");
            Stb.AppendLine(" LEFT JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.ID_Agenda = Movimenti_dettagli.Id_Agenda  ");
            Stb.AppendLine("                    AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov ");
            Stb.AppendLine("                    AND Movimenti_dettagli.Elem_Cod = 300 ");
            Stb.AppendLine(" JOIN Zoo_Animali (NOLOCK)  ON Movimenti_dettagli.Piva = Zoo_Animali.Piva AND Movimenti_dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto AND Zoo_Animali.sa_Cod = 0 ");
            Stb.AppendLine(" JOIN #agn_cte g ON g.Cod_Progetto = Zoo_Animali.Cod_Progetto ");
            Stb.AppendLine(" JOIN Utenti (NOLOCK)  ON Agenda.Username_Creazione = Utenti.CODICE_FISCALE ");
            Stb.AppendLine(" WHERE (Agenda.Lav_Cod IN (@ListaLavCodPesiAnalisi)) ");
            Stb.AppendLine(" AND Movimenti.Cau_Mov IN (@ListaCauMovPesiAnalisi) ");

            if (!string.IsNullOrEmpty(piva)) Stb.AppendLine(" AND i.Piva = @Piva ");

            if (filtroCentri.Any()) Stb.AppendLine(" AND g.Sa_Cod IN (@FiltroCentri) ");

            if (sa_Cod != 0) Stb.AppendLine(" AND g.Sa_Cod = @SaCod ");
            if (sta_Num != 0) Stb.AppendLine(" AND g.Sta_Num = @sta_num ");

            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) >= Convert(Datetime,@ValiditaInizio,120) ");
            Stb.AppendLine(" AND CAST(Movimenti.Data_Movimento as date) <= Convert(Datetime,@ValiditaFine,120) ");

            Stb.AppendLine(" AND Movimenti.Data_Movimento >= Convert(Datetime,@FinestraTemporaleInizio,120) ");
            Stb.AppendLine(" AND Movimenti.Data_Movimento <= Convert(Datetime,@FinestraTemporaleFine,120) ");

            Stb.AppendLine("  GROUP BY Agenda.Piva  ");
            Stb.AppendLine("  , g.Sa_Cod  ");
            Stb.AppendLine("  , Agenda.Id_Agenda  ");
            Stb.AppendLine("  , Agenda.Lav_Cod  ");
            Stb.AppendLine("  , Movimenti.Data_Movimento  ");
            Stb.AppendLine("  , Movimenti.Ora  ");
            Stb.AppendLine("  , Agenda.Username_Creazione  ");
            Stb.AppendLine("  , Agenda.Blocco_Flag  ");
            Stb.AppendLine("  , g.sa_nome ");
            Stb.AppendLine("  , Agenda.Tipo_Accettazione ");
            Stb.AppendLine("  , i.rag_soc  ");
            Stb.AppendLine("  , Operazioni.lav_des  ");
            //Stb.AppendLine("  , GruppoOperazioni.gru_Des  ");
            //Stb.AppendLine("  , GruppoOperazioni.tipo ");
            Stb.AppendLine("  , Attivita.Sigla ");
            Stb.AppendLine("  , Attivita.[Desc] ");
            Stb.AppendLine("  , Utenti.[user] ");
            Stb.AppendLine("  , g.STA_DES  ");
            Stb.AppendLine("  , g.STA_NUM  ");

            sqlParamsIn.Add("@ListaLavCodPesiAnalisi", FormatClauseIn(listaLavCodPesiAnalisi));
            sqlParamsIn.Add("@ListaCauMovPesiAnalisi", FormatClauseIn(listaCauMovPesiAnalisi));

            if (livelloCompatibilita >= 150) Stb.AppendLine(" OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) ");


            Stb.AppendLine(" DROP TABLE #spostamenti ");
            Stb.AppendLine(" DROP TABLE #movspos ");
            Stb.AppendLine(" DROP TABLE #last_movspos ");
            Stb.AppendLine(" DROP TABLE #agn_cte ");

            try
            {
                return await GetDataProvider(objParams).ExecuteReadAsync(Stb.ToString(), sqlParams, sqlParamsIn);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }

        public async Task<DataTable> Leggi_GiacenzeAsync(
           string piva,
           int saCod,
           int staNum,
           int raggruppamentoCod,
           int codAnimale,
           DateTime data,
           AgronicaCoreParametri objParametri,
           bool bAll = false,
           bool Filtro_Visibilita_Utente = false,
           List<int> listCod_Animali = null,
           bool filtraGiacenze1 = true,
           bool filtraFornitori = false,
           bool mostraPesate = false,
           bool mostraAnomalie = false,
           string matricola = "",
           bool mostraGGPrimoCaricamento = false,
           string CFproprietario = "",
           List<string> listMatricola_Animali = null,
           bool leggiUltimaPesata = false)
        {
            string messaggioErrore = "";
            StringBuilder stb = new StringBuilder();
            DataTable? dt;
            var sqlParams = new Dictionary<string, object>();
            try
            {
                sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
                sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
                sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
                sqlParams.TryAdd($"@{nameof(raggruppamentoCod)}", raggruppamentoCod);
                sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
                sqlParams.TryAdd($"@{nameof(data)}Param", data.Date.ToString("yyyy-MM-dd"));
                sqlParams.TryAdd($"@{nameof(data)}Ora", data.ToString("yyyy-MM-dd hh:mm:ss"));
                sqlParams.TryAdd($"@MinDate", new DateTime(1900, 1, 1).Date.ToString("yyyy-MM-dd"));
                sqlParams.TryAdd($"@MaxDate", new DateTime(2100, 12, 31).Date.ToString("yyyy-MM-dd"));

                int idTestataTemp = await _agrosequences.NuovoId_TabellaAsync("__Tmp_Agenda", 0, int.MaxValue, objParametri);
                int idTestataTempMatricola = await _agrosequences.NuovoId_TabellaAsync("__Tmp_Agenda", 0, int.MaxValue, objParametri);

                if (listCod_Animali != null && listCod_Animali.Any())
                {

                    foreach (var animaleCod in listCod_Animali)
                    {
                        await _tempAgenda.ScriviAsync(idTestataTemp, string.Empty, animaleCod, 0, objParametri);
                    }
                }

                if (listMatricola_Animali != null && listMatricola_Animali.Any())
                {
                    foreach (var mat in listMatricola_Animali)
                    {
                        await _tempAgenda.ScriviAsync(idTestataTempMatricola, mat, 0, 0, objParametri);
                    }
                }

                stb.Length = 0;
                var agn_TT = CreaAgn_TT(piva, saCod, staNum, codAnimale, data, sqlParams, ref objParametri, bAll, Filtro_Visibilita_Utente, listCod_Animali, filtraGiacenze1, idTestataTemp);
                stb.AppendLine(agn_TT);
                var giac_TT = CreaGiac_TT(piva, saCod, staNum, sqlParams, filtraGiacenze1, mostraAnomalie);
                stb.AppendLine(giac_TT);
                if (mostraGGPrimoCaricamento)
                {
                    var ZooMatricoleList_TT = CreaZooMatricoleList_TT();
                    stb.AppendLine(ZooMatricoleList_TT);
                    var DataPirmoCaricamento_TT = CreaDataPrimoCaricamento_TT(piva, sqlParams);
                    stb.AppendLine(DataPirmoCaricamento_TT);
                }
                if (mostraPesate)
                {
                    string pesate_TT = CreaPesate_TT(data, sqlParams, bAll, leggiUltimaPesata);
                    stb.AppendLine(pesate_TT);
                    string maxPesate_TT = CreaMaxPesate_TT(sqlParams);
                    stb.AppendLine(maxPesate_TT);
                    string minPesate_TT = CreaMinPesate_TT(sqlParams);
                    stb.AppendLine(minPesate_TT);
                }
                if (mostraAnomalie)
                {
                    string anomalie_TT = CreaAnomalie_TT();
                    stb.AppendLine(anomalie_TT);
                }
                stb.AppendLine("SELECT giac_cte.Piva + '_' + CAST(giac_cte.Sa_Cod as varchar(100)) + '_' + CAST(giac_cte.Cod_Animale as varchar(100)) as chiave,  ");
                stb.AppendLine("giac_cte.*,  ");

                stb.AppendLine("Lista_Specie_Animali.SPE_DES,  ");
                stb.AppendLine("Lista_Razze_Animali.RAZ_DES,  ");
                stb.AppendLine("Zoo_Animali_Lista_Tipi.Tipo_Des,  ");
                stb.AppendLine("Lista_IndirizziProd_Animali.IPRO_DES,  ");
                if (filtraGiacenze1)
                {
                    stb.AppendLine("Zoo_Animali_Distinte.Cod_Progetto,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Progetto_Des,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Progetto_Nome,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Codice_Distinta,  ");
                    stb.AppendLine("Zoo_AnimalixStati_Accrescimento.Stato_Cod,  ");
                    stb.AppendLine("Zoo_Animali_Lista_Stati_Accrescimento.Stato_Des,  ");
                }

                if (filtraFornitori)
                {
                    stb.AppendLine("COALESCE(Contatti_FornFatt.Cod_Contatto, '') AS CF_FornFatt,  ");
                    stb.AppendLine("COALESCE(Contatti_FornFatt.Rag_Soc + Contatti_FornFatt.cognome + ' ' + Contatti_FornFatt.Nome, '') AS RagSoc_FornFatt,  ");

                    stb.AppendLine("COALESCE(Contatti_FornProv.Cod_Contatto, '') AS CF_FornProv,  ");
                    stb.AppendLine("COALESCE(Contatti_FornProv.Rag_Soc + Contatti_FornProv.cognome + ' ' + Contatti_FornProv.Nome, '') AS RagSoc_FornProv,  ");
                }

                stb.AppendLine(" COALESCE(Contatti.Rag_Soc, '') as Rag_Soc,  ");
                stb.AppendLine(" COALESCE(Contatti.Cod_Contatto, '') as Cod_Contatto, ");
                stb.AppendLine(" Zoo_Animali.RAZ_COD,  ");
                stb.AppendLine("    Zoo_Animali.IPRO_COD,  ");
                stb.AppendLine("    COALESCE(Zoo_Animali.CF_Fornitore, '') as CF_Fornitore,  ");
                stb.AppendLine("   Zoo_Animali.Mat_Madre,   ");
                stb.AppendLine("   Zoo_Animali.Mat_Padre,   ");
                stb.AppendLine($"   CASE Zoo_Animali.Metodo_Produzione WHEN 1 THEN 'Integrato' WHEN 2 THEN 'InConversione' WHEN 3 THEN 'Biologico' END as Metodo_Produzione,   "); //TODO: DEMETRIO Integrato, InConversione, Biologico in resx
                stb.AppendLine("   Zoo_Animali.Razza_Padre AS RazCod_Padre,   ");
                stb.AppendLine("   razza_madre.RAZ_DES as RazDes_Madre,   ");
                stb.AppendLine("   Zoo_Animali.Razza_Madre AS RazCod_Madre,   ");
                stb.AppendLine("   razza_padre.RAZ_DES as RazDes_Padre,   ");
                stb.AppendLine("   Zoo_Animali.Lotto_Fornitore, ");
                stb.AppendLine("   Zoo_Animali.Matricola,   ");
                stb.AppendLine("   RIGHT(Zoo_Animali.Matricola, 5) AS Matricola_Breve,   ");
                stb.AppendLine("   Zoo_Animali.GEN_COD,   ");
                stb.AppendLine("   Zoo_Animali.SPE_COD,   ");
                stb.AppendLine("   Zoo_Animali.TIPO_COD,   ");
                stb.AppendLine("   Zoo_Animali.Progetto,   ");
                stb.AppendLine("   CAST(Zoo_Animali.Validita_Inizio as date) as Validita_Inizio,   ");
                stb.AppendLine("   CAST(Zoo_Animali.Validita_Fine as date) as Validita_Fine,   ");
                stb.AppendLine("   Zoo_Animali.Nome,   ");
                stb.AppendLine("   Zoo_Animali.Sesso,   ");
                stb.AppendLine("   CASE WHEN Zoo_Animali.Validato = 1 THEN 'Si' ELSE 'No' END AS Validato,   "); //TODO: DEMETRIO Si, No in resx
                stb.AppendLine("   Zoo_Animali.Modello4_Ingresso,   ");
                stb.AppendLine("   Zoo_Animali.Modello4_Uscita,   ");
                stb.AppendLine("   ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Creazione), ZOO_Animali.Username_Creazione) AS Utente_Creazione,   ");
                stb.AppendLine("   ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Modifica), ZOO_Animali.Username_Modifica) AS Utente_Modifica,   ");
                stb.AppendLine("   Zoo_Animali.Data_Creazione,   ");
                stb.AppendLine("   Zoo_Animali.Data_Modifica,   ");
                stb.AppendLine("   Zoo_Animali.Dat_Nascita,   ");
                stb.AppendLine("   Zoo_Animali.Id_Capo_BDN,   ");
                stb.AppendLine("   Zoo_Animali.CF_PROPRIETARIO,   ");
                stb.AppendLine("   Zoo_Animali.CF_DETENTORE,   ");
                stb.AppendLine("   COALESCE(Zoo_Animali.AUSL_AZI_NASCITA, '') as AUSL_AZI_NASCITA,   ");
                stb.AppendLine("   CASE WHEN Zoo_Animali.Id_Capo_BDN = 0 THEN 'Si' ELSE 'No' END AS FlagBDN,   ");
                stb.AppendLine("   Zoo_Animali.Certificato  ");

                stb.AppendLine("   , COALESCE(Zoo_Animali.Modello4_Ingresso_Numero, '') as Modello4_Ingresso_Numero  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Modello4_Uscita_Numero, '') as Modello4_Uscita_Numero ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Modello4_Ingresso_Prenotazione, '') as Modello4_Ingresso_Prenotazione  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Modello4_Uscita_Prenotazione, '') as Modello4_Uscita_Prenotazione  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Codice_Azienda_Uscita, '') as Codice_Azienda_Uscita  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Data_Documento_Ingresso, CONVERT(datetime, @MinDate, 120)) as Data_Documento_Ingresso  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Data_Documento_Uscita, CONVERT(datetime, @MaxDate, 120)) as Data_Documento_Uscita  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Fornitore_Provenienza, '') as Fornitore_Provenienza  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.N_Bolla_Fornitore, '') as N_Bolla_Fornitore  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.N_Bolla_Uscita, '') as N_Bolla_Uscita  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Data_DDT_Ingresso, CONVERT(datetime, @MinDate, 120)) as Data_DDT_Ingresso  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Data_DDT_Uscita, CONVERT(datetime, @MaxDate, 120)) as Data_DDT_Uscita  ");
                stb.AppendLine("   , COALESCE(Zoo_Animali.Codice_Azienda_Fornitore, '') as Codice_Azienda_Fornitore  ");
                stb.AppendLine("   , COALESCE(Contatti_StallaSvezz.Cod_Contatto, '') AS CF_StallaSvezz  ");
                stb.AppendLine("   , COALESCE(Contatti_StallaSvezz.Rag_Soc + Contatti_StallaSvezz.cognome + ' ' + Contatti_StallaSvezz.Nome, '') AS RagSoc_StallaSvezz  ");
                stb.AppendLine("   , Zoo_Animali.Stalla_Svezzamento  ");
                stb.AppendLine("   , Zoo_Animali.Note ");
                stb.AppendLine("   , Zoo_Animali.Anomalie_Note ");
                stb.AppendLine("   , Zoo_Animali.Id_Patologia AS Patologia_Cod ");
                stb.AppendLine("   , COALESCE(Lista_Patologie.Patologia_Des, '') AS Patologia_Des ");
                stb.AppendLine("   , Zoo_Animali.Incremento_Teorico AS Incremento_Teorico ");
                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine($"   , DATEDIFF(day, DataPrimoCaricamento.Data_Movimento, CONVERT(DateTime,@{nameof(data)}Param,120)) as giorni_stalla_primo_caricamento ");
                    stb.AppendLine("   , DataPrimoCaricamento.Data_Movimento as data_primo_caricamento ");
                }
                if (filtraGiacenze1)
                {
                    stb.AppendLine($"  ,  DATEDIFF(day, Zoo_Animali.Validita_Inizio, CONVERT(DateTime,@{nameof(data)}Param,120)) as giorni_in_stalla ");
                    stb.AppendLine($"   , DATEDIFF(day, Zoo_Animali.DAT_NASCITA, CONVERT(DateTime,@{nameof(data)}Param,120)) As Eta_Giorni_TOTALI ");
                    stb.AppendLine($"   , IIF(DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) , Zoo_Animali.Dat_Nascita), CONVERT(DateTime,@{nameof(data)}Param,120)) >= 0,  ");
                    stb.AppendLine($"      DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) ");
                    stb.AppendLine($"   , DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) -1)  as eta_mesi ");
                    stb.AppendLine($"   , IIF(DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) , Zoo_Animali.Dat_Nascita), CONVERT(DateTime,@{nameof(data)}Param,120)) >= 0 ");
                    stb.AppendLine($"   , DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) , Zoo_Animali.Dat_Nascita), CONVERT(DateTime,@{nameof(data)}Param,120)) ");
                    stb.AppendLine($"   , DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)}Param,120)) - 1, Zoo_Animali.Dat_Nascita), CONVERT(DateTime,@{nameof(data)}Param,120))) as eta_giorni ");
                }
                if (mostraPesate)
                {
                    stb.AppendLine("  , COALESCE(min_pesate_ct.Data_Peso, CONVERT(datetime, @MinDate, 120)) as Data_Prima_Pesata ");
                    stb.AppendLine("  , ROUND(min_pesate_ct.Qta, 2) as Qta_Prima_Pesata ");
                    stb.AppendLine("  , COALESCE(max_pesate_ct.Data_Peso, CONVERT(datetime, @MinDate, 120)) as Data_Ultima_Pesata ");
                    stb.AppendLine("  , ROUND(max_pesate_ct.Qta, 2) as Qta_Ultima_Pesata ");
                    stb.AppendLine($"  , ROUND(((DATEDIFF(day, COALESCE(min_pesate_ct.Data_Peso, CONVERT(datetime, @MinDate, 120)), CONVERT(DateTime,@{nameof(data)}Param,120))) * Zoo_Animali.Incremento_Teorico) + min_pesate_ct.Qta, 2) AS Incremento_Teorico_Calcolato ");
                }
                if (mostraAnomalie)
                {
                    stb.AppendLine("   , COALESCE(anomalie_ct.Anomalie, '') as Anomalie ");
                    stb.AppendLine("   , COALESCE(anomalie_ct.Anomalie_Str, '') as Anomalie_Str ");
                }
                stb.AppendLine("   , COALESCE(Zoo_Animali.Anomalie_Note, '') as Anomalie_Note ");
                stb.AppendLine("FROM #giac_cte giac_cte ");
                stb.AppendLine(" INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale  ");
                if (listMatricola_Animali != null && listMatricola_Animali.Count > 0)
                    stb.AppendLine($"   INNER Join __Tmp_Agenda tmp_agenda (NOLOCK) ON Zoo_Animali.Matricola = tmp_agenda.Piva AND tmp_agenda.id_agenda = 0 AND tmp_agenda.IDTestataTemp = {idTestataTempMatricola} ");

                if (mostraGGPrimoCaricamento)
                    stb.AppendLine(" INNER JOIN #DataPrimoCaricamento DataPrimoCaricamento ON Zoo_Animali.Matricola = DataPrimoCaricamento.Matricola ");
                if (filtraGiacenze1)
                {
                    stb.AppendLine(" INNER JOIN Zoo_Animali_Distinte (NOLOCK) ON Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ");
                    if (bAll)
                    {
                        stb.AppendLine($"                                         AND CAST(Zoo_Animali_Distinte.Validita_Inizio as date) <= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                        stb.AppendLine($"                                         And CAST(Zoo_Animali_Distinte.Validita_Fine as date) >= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                    }
                    else
                    {
                        stb.AppendLine($"                                         AND CAST(Zoo_Animali_Distinte.Validita_Inizio as datetime) <= CONVERT(DateTime,@{nameof(data)}Ora,120) ");
                        stb.AppendLine($"                                         And CAST(Zoo_Animali_Distinte.Validita_Fine as datetime) >= CONVERT(DateTime,@{nameof(data)}Ora,120) ");
                    }
                }
                if (filtraGiacenze1)
                {
                    stb.AppendLine(" INNER JOIN Zoo_AnimalixStati_Accrescimento (NOLOCK)  ON Zoo_AnimalixStati_Accrescimento.Cod_Progetto  = Zoo_Animali.Cod_Progetto  ");
                    stb.AppendLine("                                                      AND Zoo_AnimalixStati_Accrescimento.PIVA  = Zoo_Animali.PIVA ");
                    stb.AppendLine("                                                      AND Zoo_AnimalixStati_Accrescimento.sa_cod  = Zoo_Animali.sa_cod ");
                    stb.AppendLine($"                                                     AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Inizio as date) <= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                    stb.AppendLine($"                                                     AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Fine as date) >= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                }
                if (filtraGiacenze1)
                {
                    stb.AppendLine("INNER JOIN Zoo_Animali_Lista_Stati_Accrescimento (NOLOCK) ON Zoo_AnimalixStati_Accrescimento.GEN_COD = Zoo_Animali_Lista_Stati_Accrescimento.GEN_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.SPE_COD = Zoo_Animali_Lista_Stati_Accrescimento.SPE_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.STATO_COD = Zoo_Animali_Lista_Stati_Accrescimento.STATO_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.TIPO_COD = Zoo_Animali_Lista_Stati_Accrescimento.TIPO_COD  ");
                }
                stb.AppendLine("INNER JOIN Lista_Specie_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Specie_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Specie_Animali.SPE_COD  ");
                stb.AppendLine("INNER JOIN Lista_Razze_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Razze_Animali.SPE_COD And Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali razza_madre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_madre.GEN_COD And Zoo_Animali.SPE_COD = razza_madre.SPE_COD And Zoo_Animali.Razza_Madre = razza_madre.RAZ_COD   ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali razza_padre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_padre.GEN_COD And Zoo_Animali.SPE_COD = razza_padre.SPE_COD And Zoo_Animali.Razza_Padre = razza_padre.RAZ_COD   ");
                stb.AppendLine("INNER JOIN Zoo_Animali_Lista_Tipi (NOLOCK) ON Zoo_Animali.GEN_COD = Zoo_Animali_Lista_Tipi.GEN_COD And Zoo_Animali.SPE_COD = Zoo_Animali_Lista_Tipi.SPE_COD And Zoo_Animali.TIPO_COD = Zoo_Animali_Lista_Tipi.TIPO_COD  ");
                stb.AppendLine("INNER JOIN Lista_IndirizziProd_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_IndirizziProd_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_IndirizziProd_Animali.SPE_COD And Zoo_Animali.IPRO_COD = Lista_IndirizziProd_Animali.IPRO_COD  ");
                if (mostraPesate)
                {
                    stb.AppendLine(" LEFT JOIN #min_pesate_ct min_pesate_ct ON Zoo_Animali.Cod_Progetto = min_pesate_ct.Cod_Progetto ");
                    stb.AppendLine(" LEFT JOIN #max_pesate_ct max_pesate_ct ON Zoo_Animali.Cod_Progetto = max_pesate_ct.Cod_Progetto ");
                }
                if (mostraAnomalie)
                    stb.AppendLine(" LEFT JOIN #anomalie_ct anomalie_ct ON anomalie_ct.Cod_Animale = giac_cte.Cod_Animale ");
                stb.AppendLine("LEFT  JOIN Contatti (NOLOCK) ON Zoo_Animali.CF_Fornitore = Contatti.Cod_Contatto AND Zoo_Animali.Piva = Contatti.Piva ");
                stb.AppendLine("  LEFT JOIN Lista_Patologie (NOLOCK)  ON Lista_Patologie.Patologia_Cod = Zoo_Animali.Id_Patologia  ");
                if (filtraFornitori)
                {
                    stb.AppendLine("OUTER APPLY (SELECT TOP 1 * FROM Contatti  (NOLOCK) ");
                    stb.AppendLine("             WHERE Contatti.Cod_Contatto = Zoo_Animali.CF_Fornitore AND ");
                    stb.AppendLine("                (Contatti.Piva = Zoo_Animali.Piva OR Contatti.Sa_Cod = -1)) Contatti_FornFatt");

                    stb.AppendLine("OUTER APPLY (SELECT TOP 1 * FROM Contatti  (NOLOCK) ");
                    stb.AppendLine("             WHERE Contatti.Cod_Contatto = Zoo_Animali.Fornitore_Provenienza AND ");
                    stb.AppendLine("                (Contatti.Piva = Zoo_Animali.Piva OR Contatti.Sa_Cod = -1)) Contatti_FornProv");
                }

                // Stalla svezzamento
                stb.AppendLine("OUTER APPLY (SELECT TOP 1 * FROM Contatti  (NOLOCK) ");
                stb.AppendLine("             WHERE Contatti.Cod_Contatto = Zoo_Animali.Stalla_Svezzamento AND ");
                stb.AppendLine("                (Contatti.Piva = Zoo_Animali.Piva OR Contatti.Sa_Cod = -1)) Contatti_StallaSvezz");

                stb.AppendLine(" WHERE 1=1 ");
                if (filtraGiacenze1)
                {
                    if (bAll)
                    {
                        stb.AppendLine($" And CAST(Zoo_Animali.Validita_Inizio as Date) <= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                        stb.AppendLine($" And CAST(Zoo_Animali.Validita_Fine as Date) >= CONVERT(DateTime,@{nameof(data)}Param,120) ");
                    }
                    else
                    {
                        stb.AppendLine($" AND CAST(Zoo_Animali_Distinte.Validita_Inizio as datetime) <=    CONVERT(DateTime,@{nameof(data)}Ora,120) ");
                        stb.AppendLine($" And CAST(Zoo_Animali_Distinte.Validita_Fine as datetime) >=    CONVERT(DateTime,@{nameof(data)}Ora,120) ");
                    }
                }

                if (piva != "")
                    stb.AppendLine($" And giac_cte.Piva = @{nameof(piva)}      ");

                if (saCod != 0)
                    stb.AppendLine($" And giac_cte.sa_cod = @{nameof(saCod)}      ");

                if (staNum != 0)
                    stb.AppendLine($" AND giac_cte.Sta_NUM = @{nameof(staNum)}  ");

                if (raggruppamentoCod != 0)
                    stb.AppendLine($" AND giac_cte.Raggruppamento_Cod = @{nameof(raggruppamentoCod)}  ");

                if (codAnimale != 0)
                    stb.AppendLine($" AND giac_cte.Cod_Animale = @{nameof(codAnimale)}  ");

                if (matricola != "")
                {
                    sqlParams.TryAdd($"@{nameof(matricola)}", matricola);
                    stb.AppendLine($" AND Zoo_Animali.Matricola = @{nameof(matricola)}  ");
                }

                if (CFproprietario != "")
                {
                    sqlParams.TryAdd($"@{nameof(CFproprietario)}", CFproprietario);
                    stb.AppendLine($" AND Zoo_Animali.CF_PROPRIETARIO = @{nameof(CFproprietario)}  ");
                }

                stb.AppendLine(" DROP TABLE #agn_cte ");
                stb.AppendLine(" DROP TABLE #giac_cte ");

                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine(" DROP TABLE #ZooMatricoleList ");
                    stb.AppendLine(" DROP TABLE #DataPrimoCaricamento ");
                }

                if (mostraPesate)
                {
                    stb.AppendLine(" DROP TABLE #pesate_cte ");
                    stb.AppendLine(" DROP TABLE #max_pesate_ct ");
                    stb.AppendLine(" DROP TABLE #min_pesate_ct ");
                }

                if (mostraAnomalie)
                    stb.AppendLine(" DROP TABLE #anomalie_ct ");


                // ------------------------------------------------------------------------------------------------------

                var sqlParamIn = new Dictionary<string, Dictionary<Type, List<object>>>();


                var inParams = sqlParams
                    .Where(x => x.Value is Dictionary<Type, List<object>>)
                    .ToList();
                for (var i = 0; i < inParams.Count; i++)
                {
                    sqlParamIn.Add(inParams[i].Key, (Dictionary<Type, List<object>>)inParams[i].Value);
                    sqlParams.Remove(inParams[i].Key);
                }

                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stb.ToString(), sqlParams, sqlParamIn);

                // ------------------------------------------------------------------------------------------------------

                if (listCod_Animali != null && listCod_Animali.Any() && idTestataTemp != 0)
                {
                    await _tempAgenda.CancellaRecordDaIDTestataTempAsync(idTestataTemp, objParametri);
                }


                if (listMatricola_Animali != null && listMatricola_Animali.Any() && idTestataTempMatricola != 0)
                {
                    await _tempAgenda.CancellaRecordDaIDTestataTempAsync(idTestataTempMatricola, objParametri);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                ex.Data.Add("FunctionName", System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiTrattamentiAsync(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            string matricola,
            DateTime dataInizio,
            DateTime dataFine,
            bool filtroVisibilitaUtente,
            List<int>? listCodAnimali,
            int[]? farmCatList,
            int[]? farmCatSemplList,
            AgronicaCoreParametri objParametri)
        {

            string nomeRoutine = "AgronicaCoreAnagrafeBIZ.Zoo.Leggi_Trattamenti()";
            string messaggioErrore = "";
            var stb = new StringBuilder();
            DataTable dt;
            var sqlParams = new Dictionary<string, object>();

            try
            {
                int idTestataTemp = await _agrosequences.NuovoId_TabellaAsync("__Tmp_Agenda", 0, int.MaxValue, objParametri);

                if (listCodAnimali != null && listCodAnimali.Any())
                {
                    foreach (var animaleCod in listCodAnimali)
                    {
                        await _tempAgenda.ScriviAsync(idTestataTemp, string.Empty, animaleCod, 0, objParametri);
                    }
                }

                var lavCodesIn = new List<string>
                {
                    $"{LAV_COD.LAVCOD_ACQUISTO_ANIMALI}",
                    $"{LAV_COD.LAVCOD_INCREMENTO_CONSISTENZE_ZOO}",
                    $"{LAV_COD.LAVCOD_NASCITA_ANIMALI}"
                };

                var cauMoves = new List<string>
                {
                    $"{CAU_MOV.CAU_CARICO_CAPO}",
                    $"{CAU_MOV.CAU_SCARICO_CAPO}",
                    $"{CAU_MOV.CAU_CARICO}",
                    $"{CAU_MOV.CAU_SCARICO}"
                };

                var cauMovesOut = new List<string>
                {
                    $"{CAU_MOV.CAU_SCARICO_CAPO}",
                    $"{CAU_MOV.CAU_SCARICO}"
                };

                var tipiDest = new List<int>
                {
                    TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA,
                    TIPO_DESTINAZIONE.STALLA
                };
                var minDate = new DateTime(1900, 1, 1).Date;
                var maxDate = new DateTime(2100, 12, 31).Date;

                sqlParams.TryAdd($"@{nameof(cauMoves)}In", FormatClauseIn(cauMoves));
                sqlParams.TryAdd($"@{nameof(cauMovesOut)}In", FormatClauseIn(cauMovesOut));
                sqlParams.TryAdd($"@{nameof(tipiDest)}In", FormatClauseIn(tipiDest));
                sqlParams.TryAdd($"@{nameof(lavCodesIn)}", FormatClauseIn(lavCodesIn));

                sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
                sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
                sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
                sqlParams.TryAdd($"@{nameof(raggruppamentoCod)}", raggruppamentoCod);
                sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
                sqlParams.TryAdd($"@{nameof(dataInizio)}", dataInizio);
                sqlParams.TryAdd($"@{nameof(dataFine)}", dataFine);
                sqlParams.TryAdd($"@{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)}", LAV_COD.LAVCOD_NASCITA_ANIMALI);
                sqlParams.TryAdd($"@{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)}", LAV_COD.MAX_OPERAZIONE_ZOO);
                sqlParams.TryAdd($"@{nameof(LAV_COD.LAVCOD_CUREMEDICAMENTI_ANIMALI)}", LAV_COD.LAVCOD_CUREMEDICAMENTI_ANIMALI);
                sqlParams.TryAdd($"@{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA)}", TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA);
                sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}", CAU_MOV.CAU_TRATTAMENTO_ZOO);
                sqlParams.TryAdd($"@{nameof(DETT_TEC.SOSPENSIONE_CARNE)}", DETT_TEC.SOSPENSIONE_CARNE);
                sqlParams.TryAdd($"@{nameof(DETT_TEC.SOSPENSIONE_LATTE)}", DETT_TEC.SOSPENSIONE_LATTE);
                sqlParams.TryAdd($"@{nameof(minDate)}", new DateTime(1900, 1, 1).Date);
                sqlParams.TryAdd($"@{nameof(maxDate)}", new DateTime(2100, 12, 31).Date);
                sqlParams.TryAdd($"@{nameof(idTestataTemp)}", idTestataTemp);
                sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);
                sqlParams.TryAdd($"@{nameof(IMPRESE_CODICI.CUAA)}", IMPRESE_CODICI.CUAA);



                stb.Length = 0;
                stb.AppendLine("  with  ");
                stb.AppendLine(" agn_cte as ");
                stb.AppendLine("  ( ");
                stb.AppendLine("  select a.Id_Agenda,  ");
                stb.AppendLine("    a.PIVA,  ");
                stb.AppendLine("    md.Cod_Progetto, ");
                stb.AppendLine("    md.Lotto, ");
                stb.AppendLine("    md.Udm_Cod, ");
                stb.AppendLine("    UnitaMisura.Udm_Des, ");
                stb.AppendLine("    UnitaMisura.Udm_Sim, ");
                stb.AppendLine("    mdes.Sa_Cod, ");
                stb.AppendLine("    mdes.Id_Destinazione, ");
                stb.AppendLine("    mdes.Tipo_Destinazione, ");
                stb.AppendLine("    mdes.Qta, ");
                stb.AppendLine("    md.Id_Mov, ");
                stb.AppendLine("    md.Id_Mov_Det, ");
                stb.AppendLine("    m.Cau_Mov, ");
                stb.AppendLine("    i.rag_soc, ");
                stb.AppendLine("    ic.val_cod as cuaa, ");
                stb.AppendLine("    Centri_Aziendali.sa_nome,");
                stb.AppendLine("    m.Data_Movimento");
                stb.AppendLine("    from agenda a (NOLOCK)  ");
                stb.AppendLine("    inner join imprese i (NOLOCK) on i.PIVA = a.PIVA ");
                stb.AppendLine($"   inner join imprese_codici ic (NOLOCK) on i.piva = ic.piva and ic.id_cod = @{nameof(IMPRESE_CODICI.CUAA)}");
                stb.AppendLine("    inner join Movimenti m (NOLOCK) on  a.Id_Agenda = m.Id_Agenda  ");
                stb.AppendLine("    inner join Movimenti_dettagli md (NOLOCK) on md.Id_Agenda = m.Id_Agenda  AND md.Id_Mov = m.Id_Mov  ");
                stb.AppendLine("    inner join mov_destinazioni mdes (NOLOCK) on mdes.Id_Agenda = md.Id_Agenda and mdes.Id_Mov = md.Id_Mov and mdes.Id_Mov_Det = md.Id_Mov_Det ");
                stb.AppendLine("    inner join Centri_Aziendali (NOLOCK) ON mdes.Piva = Centri_Aziendali.Piva and mdes.Sa_Cod=Centri_Aziendali.sa_cod  ");
                stb.AppendLine("    INNER Join UnitaMisura (NOLOCK) ON UnitaMisura.Udm_Cod = md.Udm_Cod ");
                if (listCodAnimali is not null && listCodAnimali.Any())
                {
                    stb.AppendLine($"   INNER Join __Tmp_Agenda (NOLOCK) ON md.Cod_Progetto = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");
                }
                stb.AppendLine("    where  1 = 1 ");
                if (!string.IsNullOrEmpty(piva))
                {
                    stb.AppendLine($" And a.Piva = @{nameof(piva)}      ");
                }

                if (saCod != 0)
                {
                    stb.AppendLine($" And mdes.sa_cod = @{nameof(saCod)}");
                }

                if (codAnimale != 0)
                {
                    stb.AppendLine($" AND md.Cod_Progetto = @{nameof(codAnimale)}  ");
                }
                stb.AppendLine($"   and a.lav_Cod >= @{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)} And a.lav_cod < @{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)} ");
                stb.AppendLine($"   And m.Cau_Mov In (@{nameof(cauMoves)}In)     ");
                stb.AppendLine($"   And m.Data_Movimento >=  CONVERT(DateTime,@{nameof(minDate)},120)     ");
                stb.AppendLine("    And md.Elem_Cod = 300  ");
                stb.AppendLine("    And md.Jolly_Int = 0  ");
                stb.AppendLine($"   And mdes.Tipo_Destinazione IN (@{nameof(tipiDest)}In)  ");
                stb.AppendLine($"   And a.Lav_Cod in (@{nameof(lavCodesIn)}) ");
                stb.AppendLine(" ), ");
                stb.AppendLine(" giac_cte as ( ");
                stb.AppendLine("    SELECT   ");
                stb.AppendLine("   agn_cte.Piva, agn_cte.Rag_Soc As Impresa,  ");
                stb.AppendLine("   agn_cte.cuaa, Stalla.BDN_Codice_Azienda,  ");
                stb.AppendLine("   agn_cte.Cod_Progetto AS Cod_Animale,  ");
                stb.AppendLine("   agn_cte.Lotto,  ");
                stb.AppendLine("   agn_cte.Udm_Cod,  ");
                stb.AppendLine("   agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
                stb.AppendLine("   agn_cte.Sa_Cod,  ");
                stb.AppendLine("   agn_cte.sa_nome,  ");
                stb.AppendLine("   agn_cte.Id_Destinazione,  ");
                stb.AppendLine("   agn_cte.Tipo_Destinazione,  ");
                stb.AppendLine("   Lista_AUSL.denominazione AS AUSL_DES,  ");
                stb.AppendLine("   MIN(agn_cte.Data_Movimento) as mindata,");
                stb.AppendLine($"   IIF (MAX(agn_cte.Data_Movimento) = MIN(agn_cte.Data_Movimento), CONVERT(datetime, @{nameof(maxDate)}, 120), MAX(agn_cte.Data_Movimento)) as maxdata,");
                stb.AppendLine("   COALESCE(Fabbricati.Fabbricato_Des, '') As STA_DES,  ");
                stb.AppendLine("    COALESCE(Fabbricati.Fabbricato_Cod, 0) As STA_NUM,  ");
                stb.AppendLine($"   Convert(INTEGER, SUM( Case When agn_cte.CAU_MOV In (@{nameof(cauMovesOut)}In)              ");
                stb.AppendLine("           THEN -(agn_cte.qta)             ");
                stb.AppendLine("           Else agn_cte.qta             ");
                stb.AppendLine("           End)) As Giacenza   ");
                stb.AppendLine("  From agn_cte  ");
                stb.AppendLine("  INNER Join Stalla_Raggruppamenti (NOLOCK) ON agn_cte.Sa_Cod = Stalla_Raggruppamenti.sa_cod  ");
                stb.AppendLine("                                 AND agn_cte.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod  ");
                stb.AppendLine($"                                 AND agn_cte.Tipo_Destinazione = @{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA)}  ");
                stb.AppendLine("  INNER JOIN Fabbricati (NOLOCK) ON Stalla_Raggruppamenti.PIVA = Fabbricati.Piva AND Stalla_Raggruppamenti.sa_cod = Fabbricati.SA_COD AND Stalla_Raggruppamenti.STA_NUM = Fabbricati.Fabbricato_Cod ");
                stb.AppendLine("  INNER JOIN Stalla (NOLOCK) ON Stalla.PIVA = Fabbricati.Piva AND Stalla.sa_cod = Fabbricati.SA_COD AND Stalla.STA_NUM = Fabbricati.Fabbricato_Cod  ");
                stb.AppendLine("  INNER JOIN Indirizzi (NOLOCK) ON Fabbricati.Indirizzo_Cod = Indirizzi.cod_indirizzo ");
                stb.AppendLine("  LEFT JOIN IstatxDistretti (NOLOCK) ON IstatxDistretti.pro_cod = Indirizzi.pro_cod_istat AND IstatxDistretti.com_cod = Indirizzi.com_cod_istat ");
                stb.AppendLine("  LEFT JOIN Lista_Distretti (NOLOCK) ON Lista_Distretti.distretto_id = IstatxDistretti.distretto_id ");
                stb.AppendLine("  LEFT JOIN Lista_AUSL (NOLOCK) ON Lista_Distretti.asl_id = Lista_AUSL.asl_id ");
                stb.AppendLine("   where 1=1  ");
                if (!string.IsNullOrEmpty(piva))
                {
                    stb.AppendLine($" And agn_cte.Piva = @{nameof(piva)}     ");
                }
                if (saCod != 0)
                {
                    stb.AppendLine($" And agn_cte.sa_cod = @{nameof(saCod)}      ");
                }

                if (staNum != 0)
                {
                    stb.AppendLine($" AND Fabbricati.Fabbricato_Cod = @{nameof(staNum)}  ");
                }
                stb.AppendLine("  GROUP BY agn_cte.Piva, agn_cte.Rag_Soc, agn_cte.cuaa, Stalla.BDN_Codice_Azienda, ");
                stb.AppendLine("  agn_cte.Cod_Progetto,  ");
                stb.AppendLine("  agn_cte.Lotto,  ");
                stb.AppendLine("  agn_cte.Udm_Cod, ");
                stb.AppendLine("  Fabbricati.Fabbricato_Des, ");
                stb.AppendLine("  Fabbricati.Fabbricato_Cod, ");
                stb.AppendLine("  agn_cte.Sa_Cod,  ");
                stb.AppendLine("  agn_cte.sa_nome, ");
                stb.AppendLine("  agn_cte.Id_Destinazione,  ");
                stb.AppendLine("  agn_cte.Tipo_Destinazione,  ");
                stb.AppendLine("  agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
                stb.AppendLine("  Lista_AUSL.denominazione  ");
                stb.AppendLine($"  --HAVING Convert(INTEGER, SUM(Case When agn_cte.Cau_Mov In (@{nameof(cauMovesOut)}In) THEN -(agn_cte.qta) ELSE agn_cte.qta END)) <> 0   "); //Query commentata?
                stb.AppendLine(" ), ");
                stb.AppendLine(" Categorie_Farmaci_Con_Semplificati as  ( ");
                stb.AppendLine(" SELECT ");
                stb.AppendLine("    Farmaci_Categorie.ID,  ");
                stb.AppendLine("    Farmaci_Categorie.Categoria_Codice,  ");
                stb.AppendLine("    Farmaci_Categorie.Categoria_Descrizione, ");
                stb.AppendLine("    CONCAT('|' ,STRING_AGG(Farmaci_Categorie_Semplificate.ID, '|') , '|') as Categorie_Semplificate_Cod, ");
                stb.AppendLine("    COALESCE(STRING_AGG(Farmaci_Categorie_Semplificate.Descrizione, ','), '') as Categorie_Semplificate_Des ");
                stb.AppendLine(" FROM Farmaci_Categorie ");
                stb.AppendLine(" LEFT JOIN Farmaci_CategoriexFarmaci_Categorie_Semplificate ON Farmaci_Categorie.ID = Farmaci_CategoriexFarmaci_Categorie_Semplificate.ID_Categoria ");
                stb.AppendLine(" LEFT JOIN Farmaci_Categorie_Semplificate ON Farmaci_CategoriexFarmaci_Categorie_Semplificate.ID_Categoria_Semplificata = Farmaci_Categorie_Semplificate.ID ");
                stb.AppendLine(" GROUP BY Farmaci_Categorie.ID, Farmaci_Categorie.Categoria_Codice, Farmaci_Categorie.Categoria_Descrizione ");
                stb.AppendLine(" ) ");
                stb.AppendLine(" SELECT ");
                stb.AppendLine("    CAST(Mov_Destinazioni.Id_Agenda as varchar(250)) + '_' + CAST(Mov_Destinazioni.Id_Destinazione as varchar(250)) as ID, ");
                stb.AppendLine("    Imprese.Piva,  ");
                stb.AppendLine("    Imprese.rag_soc,  ");
                stb.AppendLine("    Imprese_Codici.val_cod as CF_Impresa, ");
                stb.AppendLine("    giac_cte.BDN_Codice_Azienda,  ");
                stb.AppendLine("    giac_cte.Sa_Nome, ");
                stb.AppendLine("    giac_cte.Sa_Cod, ");
                stb.AppendLine("    giac_cte.STA_DES, ");
                stb.AppendLine("    giac_cte.STA_NUM, ");
                stb.AppendLine("    giac_cte.AUSL_DES, ");
                stb.AppendLine("    Movimenti_dettagli.Extra_Date as Data_Prescrizione, ");
                stb.AppendLine("    Movimenti_Dettagli.Rif_Esterno as Num_Trattamento, ");
                stb.AppendLine("    Movimenti_Dettagli.Qta_Extra_Totale as QtaTotale, ");
                stb.AppendLine("    Movimenti_Dettagli.extra_int as Udm_Cod, ");
                stb.AppendLine("    UnitaMisura.UDM_SIM as Unita_Misura, ");
                stb.AppendLine("    Agenda.Validita_Inizio as Data_Inizio_Trattamento, ");
                stb.AppendLine("    Agenda.Validita_Fine as Data_Fine_Trattamento, ");
                stb.AppendLine("    Zoo_Animali.Matricola as Matricola, ");
                stb.AppendLine("    Zoo_Animali.Modello4_Uscita_Numero,  ");
                stb.AppendLine("    Zoo_Animali.Modello4_Uscita_Prenotazione,  ");
                stb.AppendLine("    giac_cte.Cod_Animale, ");
                stb.AppendLine("    Zoo_Animali_Distinte.Cod_Progetto, ");
                stb.AppendLine("    Zoo_Animali_Distinte.Codice_Distinta, ");
                stb.AppendLine("    Zoo_Animali_Distinte.Progetto_Des, ");
                stb.AppendLine("    Zoo_Animali_Distinte.Codice_Distinta, ");
                stb.AppendLine("    CAST(Movimenti_Dettagli.Qta_Extra_Totale * Mov_Destinazioni.QuotaDistribuzione as decimal(10, 4)) as Qta_Capo, ");
                stb.AppendLine("    SospensioneCarne.Extra_Int as giorniSospensioneCarne, ");
                stb.AppendLine("    DATEADD(day, SospensioneCarne.Extra_Int, Agenda.Validita_Fine) AS DataSospensioneCarne, ");
                stb.AppendLine("    SospensioneLatte.Extra_Int as giorniSospensioneLatte, ");
                stb.AppendLine("    DATEADD(day, SospensioneLatte.Extra_Int, Agenda.Validita_Fine) AS DataSospensioneLatte, ");
                stb.AppendLine("    Farmaci.AIC, ");
                stb.AppendLine("    Farmaci.Denominazione, ");
                stb.AppendLine("    Farmaci.Confezione, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categoria_Codice, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categoria_Descrizione, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Cod, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Des, ");
                stb.AppendLine("    COALESCE(STRING_AGG(PrincipiAttivi.Pa_Des, ', '), '') as PrincipiAttivi ");
                stb.AppendLine(" FROM Agenda");
                stb.AppendLine($" JOIN Movimenti ON Agenda.Id_Agenda = Movimenti.Id_Agenda AND Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}");
                stb.AppendLine(" JOIN Movimenti_Dettagli ON Movimenti_Dettagli.Id_Agenda = Movimenti.Id_Agenda AND Movimenti_Dettagli.Id_Mov = Movimenti.Id_Mov");
                stb.AppendLine(" LEFT JOIN Mov_Dettaglio_Tecnico as SospensioneCarne ON Movimenti_Dettagli.Piva = SospensioneCarne.Piva ");
                stb.AppendLine("                                                    AND Movimenti_Dettagli.Id_Agenda = SospensioneCarne.Id_Agenda ");
                stb.AppendLine("                                                    AND Movimenti_Dettagli.Id_Mov = SospensioneCarne.Id_Mov ");
                stb.AppendLine("                                                    AND Movimenti_Dettagli.Id_Mov_Det = SospensioneCarne.Id_Mov_Det ");
                stb.AppendLine($"                                                   AND SospensioneCarne.Dett_Cod = @{nameof(DETT_TEC.SOSPENSIONE_CARNE)} ");
                stb.AppendLine("  LEFT JOIN Mov_Dettaglio_Tecnico as SospensioneLatte ON Movimenti_Dettagli.Id_Agenda = SospensioneLatte.Id_Agenda ");
                stb.AppendLine("                                                    AND Movimenti_Dettagli.Id_Mov = SospensioneLatte.Id_Mov ");
                stb.AppendLine("                                                    AND Movimenti_Dettagli.Id_Mov_Det = SospensioneLatte.Id_Mov_Det ");
                stb.AppendLine($"                                                   AND SospensioneLatte.Dett_Cod = @{nameof(DETT_TEC.SOSPENSIONE_LATTE)} ");
                stb.AppendLine(" JOIN Mov_Destinazioni ON Movimenti_Dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda AND Movimenti_Dettagli.Id_Mov = Mov_Destinazioni.Id_Mov AND Movimenti_Dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det");
                stb.AppendLine(" INNER JOIN giac_cte ON Mov_Destinazioni.Id_Destinazione = giac_cte.Cod_Animale ");
                stb.AppendLine("        AND giac_cte.maxdata >= Movimenti.Data_Movimento");
                stb.AppendLine("        AND giac_cte.mindata <= Movimenti.Data_Movimento");
                stb.AppendLine("  INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale  ");
                stb.AppendLine("  INNER JOIN Zoo_Animali_Distinte (NOLOCK) ON Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ");
                stb.AppendLine("                                          AND CAST(Zoo_Animali_Distinte.Validita_Inizio as datetime) <= Movimenti.Data_Movimento ");
                stb.AppendLine("                                          And CAST(Zoo_Animali_Distinte.Validita_Fine as datetime) >=   Movimenti.Data_Movimento ");
                stb.AppendLine("  INNER JOIN Zoo_AnimalixStati_Accrescimento ON Zoo_AnimalixStati_Accrescimento.Cod_Progetto  = Zoo_Animali.Cod_Progetto  ");
                stb.AppendLine("                                            AND Zoo_AnimalixStati_Accrescimento.PIVA  = Zoo_Animali.PIVA  ");
                stb.AppendLine("                                            AND Zoo_AnimalixStati_Accrescimento.sa_cod  = Zoo_Animali.sa_cod  ");
                stb.AppendLine("                                            AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Inizio as datetime) <=  Movimenti.Data_Movimento ");
                stb.AppendLine("                                            And CAST(Zoo_AnimalixStati_Accrescimento.Validita_Fine as datetime) >=  Movimenti.Data_Movimento ");
                stb.AppendLine(" INNER JOIN Zoo_Animali_Lista_Stati_Accrescimento (NOLOCK) ON Zoo_AnimalixStati_Accrescimento.GEN_COD = Zoo_Animali_Lista_Stati_Accrescimento.GEN_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.SPE_COD = Zoo_Animali_Lista_Stati_Accrescimento.SPE_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.STATO_COD = Zoo_Animali_Lista_Stati_Accrescimento.STATO_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.TIPO_COD = Zoo_Animali_Lista_Stati_Accrescimento.TIPO_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Specie_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Specie_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Specie_Animali.SPE_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Razze_Animali.SPE_COD And Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD  ");
                stb.AppendLine("  INNER JOIN Lista_Razze_Animali razza_madre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_madre.GEN_COD And Zoo_Animali.SPE_COD = razza_madre.SPE_COD And Zoo_Animali.Razza_Madre = razza_madre.RAZ_COD   ");
                stb.AppendLine("  INNER JOIN Lista_Razze_Animali razza_padre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_padre.GEN_COD And Zoo_Animali.SPE_COD = razza_padre.SPE_COD And Zoo_Animali.Razza_Padre = razza_padre.RAZ_COD   ");
                stb.AppendLine(" INNER JOIN Zoo_Animali_Lista_Tipi (NOLOCK) ON Zoo_Animali.GEN_COD = Zoo_Animali_Lista_Tipi.GEN_COD And Zoo_Animali.SPE_COD = Zoo_Animali_Lista_Tipi.SPE_COD And Zoo_Animali.TIPO_COD = Zoo_Animali_Lista_Tipi.TIPO_COD  ");
                stb.AppendLine(" INNER JOIN Lista_IndirizziProd_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_IndirizziProd_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_IndirizziProd_Animali.SPE_COD And Zoo_Animali.IPRO_COD = Lista_IndirizziProd_Animali.IPRO_COD  ");
                stb.AppendLine(" LEFT  JOIN Contatti (NOLOCK) ON Zoo_Animali.CF_Fornitore = Contatti.Cod_Contatto AND Zoo_Animali.Piva = Contatti.Piva ");
                stb.AppendLine(" JOIN Imprese ON Agenda.PIVA = Imprese.PIVA");
                stb.AppendLine($" JOIN Imprese_Codici ON Imprese.Piva = Imprese_Codici.Piva AND Imprese_Codici.id_cod = @{nameof(IMPRESE_CODICI.CUAA)}");
                stb.AppendLine(" JOIN Farmaci ON Movimenti_Dettagli.Pro_Cod = Farmaci.Farm_Cod");
                stb.AppendLine(" JOIN FarmacixCategorie ON Farmaci.Farm_Cod = FarmacixCategorie.Farm_Cod");
                stb.AppendLine(" JOIN Categorie_Farmaci_Con_Semplificati ON FarmacixCategorie.Cat_Cod = Categorie_Farmaci_Con_Semplificati.ID ");
                stb.AppendLine(" LEFT JOIN FarmacixPrincipiAttivi ON Farmaci.Farm_Cod = FarmacixPrincipiAttivi.Farm_Cod ");
                stb.AppendLine(" LEFT JOIN PrincipiAttivi ON FarmacixPrincipiAttivi.PA_Cod = PrincipiAttivi.Pa_Cod ");
                stb.AppendLine(" JOIN UnitaMisura ON UnitaMisura.UDM_COD = Movimenti_Dettagli.Extra_Int");
                if (filtroVisibilitaUtente)
                {
                    stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Agenda.Piva = p.piva AND p.Sa_Cod = giac_cte.Sa_Cod AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
                }
                stb.AppendLine($" WHERE Agenda.Lav_Cod = @{nameof(LAV_COD.LAVCOD_CUREMEDICAMENTI_ANIMALI)} ");

                if (!string.IsNullOrEmpty(piva))
                {
                    stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
                }

                if (saCod != 0)
                {
                    stb.AppendLine($" And giac_cte.sa_cod = @{nameof(saCod)}      ");
                }

                if (staNum != 0)
                {
                    stb.AppendLine($" AND giac_cte.Sta_NUM =  @{nameof(staNum)}  ");
                }

                if (raggruppamentoCod != 0)
                {
                    stb.AppendLine($" AND giac_cte.Raggruppamento_Cod = @{nameof(raggruppamentoCod)}  ");
                }

                if (codAnimale != 0)
                {
                    stb.AppendLine($" AND giac_cte.Cod_Animale = @{nameof(codAnimale)}  ");
                }

                if (!string.IsNullOrEmpty(matricola))
                {
                    sqlParams.TryAdd($"@{nameof(matricola)}", matricola);
                    stb.AppendLine($" AND Zoo_Animali.Matricola = @{nameof(matricola)} ");
                }

                if (dataInizio > minDate)
                {
                    stb.AppendLine($" AND Movimenti.Data_Movimento >= CONVERT(datetime, @{nameof(dataInizio)}, 120) ");
                }

                if (farmCatList != null && farmCatList.Any())
                {
                    sqlParams.TryAdd($"@{nameof(farmCatList)}In", FormatClauseIn(farmCatList.ToList()));
                    stb.AppendLine($" AND Categorie_Farmaci_Con_Semplificati.ID IN (@{nameof(farmCatList)}In) ");
                }

                if (farmCatSemplList != null && farmCatSemplList.Any())
                {
                    stb.AppendLine(" AND ( ");
                    int i = 0;
                    foreach (var cat_sempl in farmCatSemplList)
                    {
                        if (i != 0)
                        {
                            stb.Append(" OR ");
                        }
                        stb.Append(" Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Cod like '%|" + cat_sempl + "|%' ");
                        i = i + 1;
                    }
                    stb.Append(" ) ");
                }

                if (dataFine < maxDate)
                {
                    dataFine = dataFine.AddHours(24d);
                    dataFine = dataFine.AddSeconds(-1);
                    stb.AppendLine($" AND   Movimenti.Data_Movimento <= CONVERT(datetime, @{nameof(maxDate)}, 120) ");
                }

                stb.AppendLine(" GROUP BY  ");
                stb.AppendLine("    Mov_Destinazioni.Id_Agenda, ");
                stb.AppendLine("    Mov_Destinazioni.Id_Destinazione, ");
                stb.AppendLine("    Mov_Destinazioni.QuotaDistribuzione, ");
                stb.AppendLine("    Imprese.Piva,   ");
                stb.AppendLine("    Imprese.rag_soc,   ");
                stb.AppendLine("    Imprese_Codici.val_cod,  ");
                stb.AppendLine("    giac_cte.BDN_Codice_Azienda,   ");
                stb.AppendLine("    giac_cte.Sa_Nome,  ");
                stb.AppendLine("    giac_cte.Sa_Cod,  ");
                stb.AppendLine("    giac_cte.STA_DES,  ");
                stb.AppendLine("    giac_cte.STA_NUM,  ");
                //stb.AppendLine("      giac_cte.Raggruppamento_Des,  ");
                //stb.AppendLine("      giac_cte.Raggruppamento_Cod,  ");
                stb.AppendLine("    giac_cte.AUSL_DES,  ");
                stb.AppendLine("    Movimenti.Data_Movimento,  ");
                stb.AppendLine("    Movimenti_dettagli.Extra_Date,  ");
                stb.AppendLine("    Movimenti_Dettagli.Rif_Esterno,  ");
                stb.AppendLine("    Movimenti_Dettagli.Qta_Extra_Totale,  ");
                stb.AppendLine("    Movimenti_Dettagli.extra_int,  ");
                stb.AppendLine("    UnitaMisura.UDM_SIM,  ");
                stb.AppendLine("    Agenda.Validita_Inizio,  ");
                stb.AppendLine("    Agenda.Validita_Fine,  ");
                stb.AppendLine("    Zoo_Animali.Matricola,  ");
                stb.AppendLine("    Zoo_Animali.Modello4_Uscita_Numero,  ");
                stb.AppendLine("    Zoo_Animali.Modello4_Uscita_Prenotazione,  ");
                stb.AppendLine("    giac_cte.Cod_Animale,  ");
                stb.AppendLine("    Zoo_Animali_Distinte.Cod_Progetto,  ");
                stb.AppendLine("    Zoo_Animali_Distinte.Progetto_Des, ");
                stb.AppendLine("  Zoo_Animali_Distinte.Codice_Distinta, ");
                stb.AppendLine("    SospensioneCarne.Extra_Int,  ");
                stb.AppendLine("    SospensioneLatte.Extra_Int,  ");
                stb.AppendLine("    Farmaci.AIC,  ");
                stb.AppendLine("    Farmaci.Denominazione,  ");
                stb.AppendLine("    Farmaci.Confezione,  ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categoria_Codice,  ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categoria_Descrizione, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Cod, ");
                stb.AppendLine("    Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Des ");

                // ------------------------------------------------------------------------------------------------------

                var sqlParamIn = new Dictionary<string, Dictionary<Type, List<object>>>();

                var inParams = sqlParams
                    .Where(x => x.Value is Dictionary<Type, List<object>>)
                    .ToList();
                for (var i = 0; i < inParams.Count; i++)
                {
                    sqlParamIn.Add(inParams[i].Key, (Dictionary<Type, List<object>>)inParams[i].Value);
                    sqlParams.Remove(inParams[i].Key);
                }
                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stb.ToString(), sqlParams, sqlParamIn);
                // ------------------------------------------------------------------------------------------------------

                if (listCodAnimali != null && listCodAnimali.Any() && idTestataTemp != 0)
                {
                    await _tempAgenda.CancellaRecordDaIDTestataTempAsync(idTestataTemp, objParametri);
                }
            }

            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                ex.Data.Add("FunctionName", System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                throw;
            }

            return dt;

        }
        public async Task<DataTable> LeggiStazionamentoZooAsync(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data,
            int giorniStazionamento,
            bool filtroVisibilitaUtente,
            List<int>? listCodAnimali,
            AgronicaCoreParametri objParametri,
            int compatibilityLevel)
        {

            const string nomeRoutine = "AgronicaCoreAnagrafeBIZ.Zoo.Leggi_Stazionamento_Capi()";

            string messaggioErrore = "";
            var stb = new StringBuilder();
            DataTable dt;
            var sqlParams = new Dictionary<string, object>();
            try
            {
                int idTestataTemp = 0;
                // CancellaRecordDaIDTestataTemp
                bool filtraGiacenze1 = true;
                bool bAll = false;

                if (listCodAnimali is not null && listCodAnimali.Any())
                {
                    foreach (var itemAnimaleCod in listCodAnimali)
                        await _tempAgenda.ScriviAsync(idTestataTemp, string.Empty, itemAnimaleCod, 0, objParametri);
                }

                var cauMoves = new List<string>
                {
                    $"{CAU_MOV.CAU_CARICO_CAPO}",
                    $"{CAU_MOV.CAU_SCARICO_CAPO}",
                    $"{CAU_MOV.CAU_CARICO}",
                    $"{CAU_MOV.CAU_SCARICO}"
                };

                var cauScarico = new List<string>
                {
                    $"{CAU_MOV.CAU_SCARICO_CAPO}",
                    $"{CAU_MOV.CAU_SCARICO}"
                };

                var tipiDest = new List<int>
                {
                    TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA,
                    TIPO_DESTINAZIONE.STALLA
                };

                var minDate = new DateTime(1900, 1, 1).Date;
                var maxDate = new DateTime(2100, 12, 31).Date;

                sqlParams.TryAdd($"@{nameof(cauMoves)}In", FormatClauseIn(cauMoves));
                sqlParams.TryAdd($"@{nameof(cauScarico)}In", FormatClauseIn(cauScarico));
                sqlParams.TryAdd($"@{nameof(tipiDest)}In", FormatClauseIn(tipiDest));
                sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
                sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
                sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
                sqlParams.TryAdd($"@{nameof(raggruppamentoCod)}", raggruppamentoCod);
                sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
                sqlParams.TryAdd($"@{nameof(data)}", data.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlParams.TryAdd($"@{nameof(giorniStazionamento)}", giorniStazionamento);
                sqlParams.TryAdd($"@{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)}", LAV_COD.LAVCOD_NASCITA_ANIMALI);
                sqlParams.TryAdd($"@{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)}", LAV_COD.MAX_OPERAZIONE_ZOO);
                sqlParams.TryAdd($"@{nameof(minDate)}", minDate.ToString("yyyy-MM-dd"));
                sqlParams.TryAdd($"@{nameof(maxDate)}", maxDate.ToString("yyyy-MM-dd"));
                sqlParams.TryAdd($"@{nameof(ELEM_COD.ZOO_CONSISTENZA)}", ELEM_COD.ZOO_CONSISTENZA);
                sqlParams.TryAdd($"@{nameof(IMPRESE_CODICI.CUAA)}", IMPRESE_CODICI.CUAA);
                sqlParams.TryAdd($"@{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA)}", TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA);
                sqlParams.TryAdd($"@{nameof(ELEM_COD.ZOO_CONSISTENZA)}", ELEM_COD.ZOO_CONSISTENZA);

                stb.Length = 0;
                stb.AppendLine(" with  ");
                stb.AppendLine(" agn_cte as ");
                stb.AppendLine(" ( ");
                stb.AppendLine(" select a.Id_Agenda,  ");
                stb.AppendLine("    a.PIVA,  ");
                stb.AppendLine("    md.Cod_Progetto, ");
                stb.AppendLine("    md.Lotto, ");
                stb.AppendLine("    md.Udm_Cod, ");
                stb.AppendLine("    UnitaMisura.Udm_Des, ");
                stb.AppendLine("    UnitaMisura.Udm_Sim, ");
                stb.AppendLine("    mdes.Sa_Cod, ");
                stb.AppendLine("    mdes.Id_Destinazione, ");
                stb.AppendLine("    mdes.Tipo_Destinazione, ");
                stb.AppendLine("    mdes.Qta, ");
                stb.AppendLine("    md.Id_Mov, ");
                stb.AppendLine("    md.Id_Mov_Det, ");
                stb.AppendLine("    m.Cau_Mov, ");
                stb.AppendLine("    i.rag_soc, ");
                stb.AppendLine("    ic.val_cod as cuaa, ");
                stb.AppendLine("    Centri_Aziendali.sa_nome ");
                stb.AppendLine("    from agenda a (NOLOCK)  ");
                stb.AppendLine("    inner join imprese i (NOLOCK) on i.PIVA = a.PIVA ");
                stb.AppendLine("    inner join imprese_codici ic (NOLOCK) on i.piva = ic.piva and ic.id_cod = 1010 ");
                stb.AppendLine("    inner join Movimenti m (NOLOCK) on  a.Id_Agenda = m.Id_Agenda  ");
                stb.AppendLine("    inner join Movimenti_dettagli md (NOLOCK) on md.Id_Agenda = m.Id_Agenda  AND md.Id_Mov = m.Id_Mov  ");
                stb.AppendLine("    inner join mov_destinazioni mdes (NOLOCK) on mdes.Id_Agenda = md.Id_Agenda and mdes.Id_Mov = md.Id_Mov and mdes.Id_Mov_Det = md.Id_Mov_Det ");
                stb.AppendLine("    inner join Centri_Aziendali (NOLOCK) ON mdes.Piva = Centri_Aziendali.Piva and mdes.Sa_Cod=Centri_Aziendali.sa_cod  ");
                stb.AppendLine("    INNER Join UnitaMisura (NOLOCK) ON UnitaMisura.Udm_Cod = md.Udm_Cod ");
                if (listCodAnimali is not null && listCodAnimali.Any())
                {
                    stb.AppendLine($"   INNER Join __Tmp_Agenda (NOLOCK) ON md.Cod_Progetto = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");
                }
                // If Filtro_Visibilita_Utente And Piva = "" Then
                // stb.AppendLine(" inner join piva_visibili p (NOLOCK) ON a.Piva = p.piva  ")
                // End If
                stb.AppendLine("    where  1 = 1 ");
                if (piva != "")
                {
                    stb.AppendLine($" And a.Piva = @{nameof(piva)}      ");
                }

                if (saCod != 0)
                {
                    stb.AppendLine($" And mdes.sa_cod = @{nameof(saCod)}      ");
                }

                if (codAnimale != 0)
                {
                    stb.AppendLine($" AND md.Cod_Progetto =@{nameof(codAnimale)}   ");
                }

                stb.AppendLine($"   and a.lav_Cod >= @{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)} And a.lav_cod <= @{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)} ");
                stb.AppendLine($"   And m.Cau_Mov In (@{nameof(cauMoves)}In)     ");
                stb.AppendLine($"   And m.Data_Movimento >=  CONVERT(DateTime,@{nameof(minDate)},120)     ");

                if (filtraGiacenze1)
                {
                    switch (bAll)
                    {
                        case false:
                            {
                                stb.AppendLine($"   And m.Data_Movimento <=   CONVERT(DateTime,@{nameof(data)},120)     ");
                                break;
                            }
                        case true:
                            {
                                stb.AppendLine($"   And m.Data_Movimento <=   CONVERT(DateTime,@{nameof(data)},120)     ");
                                break;
                            }
                    }
                }

                stb.AppendLine("    And md.Elem_Cod = 300  ");
                stb.AppendLine("    And md.Jolly_Int = 0  ");
                stb.AppendLine("    And mdes.Tipo_Destinazione IN (15, 21)  ");
                stb.AppendLine("), ");
                stb.AppendLine("giac_cte as ( ");
                stb.AppendLine("    SELECT   ");
                stb.AppendLine("  agn_cte.Piva, agn_cte.Rag_Soc As Impresa,  ");
                stb.AppendLine("  agn_cte.cuaa, Stalla.BDN_Codice_Azienda,  ");
                stb.AppendLine("  agn_cte.Cod_Progetto AS Cod_Animale,  ");
                stb.AppendLine("  agn_cte.Lotto,  ");
                stb.AppendLine("  agn_cte.Udm_Cod,  ");
                stb.AppendLine("  agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
                stb.AppendLine("  agn_cte.Sa_Cod,  ");
                stb.AppendLine("  agn_cte.sa_nome,  ");
                stb.AppendLine("  agn_cte.Id_Destinazione,  ");
                stb.AppendLine("  agn_cte.Tipo_Destinazione,  ");
                stb.AppendLine("  COALESCE(Fabbricati.Fabbricato_Des, '') As STA_DES,  ");
                stb.AppendLine("    COALESCE(Fabbricati.Fabbricato_Cod, 0) As STA_NUM,  ");
                stb.AppendLine("  COALESCE(Stalla_Raggruppamenti.Raggruppamento_Des, '') as Raggruppamento_Des,  ");
                stb.AppendLine("  COALESCE(Stalla_Raggruppamenti.Raggruppamento_Cod, 0) as Raggruppamento_Cod,  ");
                stb.AppendLine($"  Convert(INTEGER, SUM( Case When agn_cte.CAU_MOV In (@{nameof(cauScarico)}In)              ");
                stb.AppendLine("          THEN -(agn_cte.qta)             ");
                stb.AppendLine("          Else agn_cte.qta             ");
                stb.AppendLine("          End)) As Giacenza   ");
                stb.AppendLine(" From agn_cte  ");
                stb.AppendLine(" INNER Join Stalla_Raggruppamenti (NOLOCK) ON agn_cte.Sa_Cod = Stalla_Raggruppamenti.sa_cod  ");
                stb.AppendLine("                                AND agn_cte.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod  ");
                stb.AppendLine("                                AND agn_cte.Tipo_Destinazione = 21  ");
                stb.AppendLine(" INNER JOIN Fabbricati (NOLOCK) ON Stalla_Raggruppamenti.PIVA = Fabbricati.Piva AND Stalla_Raggruppamenti.sa_cod = Fabbricati.SA_COD AND Stalla_Raggruppamenti.STA_NUM = Fabbricati.Fabbricato_Cod ");
                stb.AppendLine(" INNER JOIN Stalla (NOLOCK) ON Stalla.PIVA = Fabbricati.Piva AND Stalla.sa_cod = Fabbricati.SA_COD AND Stalla.STA_NUM = Fabbricati.Fabbricato_Cod  ");
                stb.AppendLine("  where 1=1  ");
                if (saCod != 0)
                {
                    stb.AppendLine($" And agn_cte.sa_cod = @{nameof(saCod)}      ");
                }

                if (staNum != 0)
                {
                    stb.AppendLine($" AND Fabbricati.Fabbricato_Cod = @{nameof(staNum)}  ");
                }

                stb.AppendLine(" GROUP BY agn_cte.Piva, agn_cte.Rag_Soc, agn_cte.cuaa, Stalla.BDN_Codice_Azienda, ");
                stb.AppendLine(" agn_cte.Cod_Progetto,  ");
                stb.AppendLine(" agn_cte.Lotto,  ");
                stb.AppendLine(" agn_cte.Udm_Cod, ");
                stb.AppendLine(" Fabbricati.Fabbricato_Des, ");
                stb.AppendLine(" Fabbricati.Fabbricato_Cod, ");
                stb.AppendLine(" agn_cte.Sa_Cod,  ");
                stb.AppendLine(" agn_cte.sa_nome, ");
                stb.AppendLine(" agn_cte.Id_Destinazione,  ");
                stb.AppendLine(" agn_cte.Tipo_Destinazione,  ");
                stb.AppendLine(" agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
                stb.AppendLine(" Stalla_Raggruppamenti.Raggruppamento_Des,  ");
                stb.AppendLine(" Stalla_Raggruppamenti.Raggruppamento_Cod  ");
                if (filtraGiacenze1)
                {
                    stb.AppendLine($" HAVING Convert(INTEGER, SUM(Case When agn_cte.Cau_Mov In (@{nameof(cauScarico)}In) THEN -(agn_cte.qta) ELSE agn_cte.qta END)) <> 0   ");
                }

                stb.AppendLine("), ");
                stb.AppendLine(" ZooMatricoleList as ( ");
                stb.AppendLine("    SELECT Zoo_Animali.Matricola ");
                stb.AppendLine("    FROM giac_cte  ");
                stb.AppendLine("     INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale   ");
                stb.AppendLine("     WHERE 1=1  ");
                stb.AppendLine(" ),  ");
                stb.AppendLine(" DataPrimoCaricamento as ( ");
                stb.AppendLine(" SELECT ZooMatricoleList.Matricola, MIN(Movimenti.Data_Movimento) as Data_Movimento ");
                stb.AppendLine(" FROM Agenda (NOLOCK) ");
                stb.AppendLine(" JOIN Movimenti (NOLOCK) ON Agenda.ID_Agenda = Movimenti.ID_Agenda ");
                stb.AppendLine($" JOIN Movimenti_Dettagli (NOLOCK) ON Movimenti.ID_Agenda = Movimenti_Dettagli.ID_Agenda AND Movimenti.Id_Mov = Movimenti_Dettagli.Id_Mov AND Movimenti_dettagli.Elem_Cod = @{nameof(ELEM_COD.ZOO_CONSISTENZA)} ");
                stb.AppendLine(" JOIN Zoo_Animali (NOLOCK) ON Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto ");
                stb.AppendLine(" JOIN ZooMatricoleList  ON Zoo_Animali.Matricola = ZooMatricoleList.Matricola ");
                stb.AppendLine(" WHERE 1 = 1 ");
                if (piva != "")
                {
                    stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
                }

                stb.AppendLine(" GROUP BY ZooMatricoleList.Matricola ");
                stb.AppendLine(" ) ");
                stb.AppendLine("SELECT giac_cte.Piva + '_' + CAST(giac_cte.Sa_Cod as varchar(100)) + '_' + CAST(giac_cte.Cod_Animale as varchar(100)) as chiave,  ");
                stb.AppendLine("giac_cte.*,  ");

                stb.AppendLine("Lista_Specie_Animali.SPE_DES,  ");
                stb.AppendLine("Lista_Razze_Animali.RAZ_DES,  ");
                stb.AppendLine("Zoo_Animali_Lista_Tipi.Tipo_Des,  ");
                stb.AppendLine("Lista_IndirizziProd_Animali.IPRO_DES,  ");
                if (filtraGiacenze1)
                {
                    stb.AppendLine("Zoo_Animali_Distinte.Cod_Progetto,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Progetto_Des,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Progetto_Nome,  ");
                    stb.AppendLine("Zoo_Animali_Distinte.Codice_Distinta,  ");
                    stb.AppendLine("Zoo_AnimalixStati_Accrescimento.Stato_Cod,  ");
                    stb.AppendLine("Zoo_Animali_Lista_Stati_Accrescimento.Stato_Des,  ");
                }

                stb.AppendLine("Contatti.Rag_Soc,  ");
                stb.AppendLine("Contatti.Cod_Contatto, ");
                stb.AppendLine(" Zoo_Animali.RAZ_COD,  ");
                stb.AppendLine("    Zoo_Animali.IPRO_COD,  ");
                stb.AppendLine("    Zoo_Animali.CF_Fornitore,  ");
                stb.AppendLine("   Zoo_Animali.Mat_Madre,   ");
                stb.AppendLine("   Zoo_Animali.Mat_Padre,   ");
                stb.AppendLine("   CASE Zoo_Animali.Metodo_Produzione WHEN 1 THEN 'Integrato' WHEN 2 THEN 'In Conversione' WHEN 3 THEN 'Biologico' END as Metodo_Produzione,   ");
                stb.AppendLine("   Zoo_Animali.Razza_Padre AS RazCod_Padre,   ");
                stb.AppendLine("   razza_madre.RAZ_DES as RazDes_Madre,   ");
                stb.AppendLine("   Zoo_Animali.Razza_Madre AS RazCod_Madre,   ");
                stb.AppendLine("   razza_padre.RAZ_DES as RazDes_Padre,   ");
                stb.AppendLine("   Zoo_Animali.Lotto_Fornitore, ");
                stb.AppendLine("   Zoo_Animali.Matricola,   ");
                stb.AppendLine("   Zoo_Animali.GEN_COD,   ");
                stb.AppendLine("   Zoo_Animali.SPE_COD,   ");
                stb.AppendLine("   Zoo_Animali.TIPO_COD,   ");
                stb.AppendLine("   Zoo_Animali.Progetto,   ");
                stb.AppendLine("   CAST(Zoo_Animali.Validita_Inizio as date) as Validita_Inizio,   ");
                stb.AppendLine("   CAST(Zoo_Animali.Validita_Fine as date) as Validita_Fine,   ");
                stb.AppendLine("   Zoo_Animali.Nome,   ");
                stb.AppendLine("   Zoo_Animali.Sesso,   ");
                stb.AppendLine("   Zoo_Animali.Modello4_Ingresso,   ");
                stb.AppendLine("   Zoo_Animali.Modello4_Uscita,   ");
                stb.AppendLine("   ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Creazione), ZOO_Animali.Username_Creazione) AS Utente_Creazione,   ");
                stb.AppendLine("   ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Modifica), ZOO_Animali.Username_Modifica) AS Utente_Modifica,   ");
                stb.AppendLine("   Zoo_Animali.Data_Creazione,   ");
                stb.AppendLine("   Zoo_Animali.Data_Modifica,   ");
                stb.AppendLine("   Zoo_Animali.Dat_Nascita,   ");
                stb.AppendLine("   Zoo_Animali.Id_Capo_BDN,   ");
                stb.AppendLine("   Zoo_Animali.CF_PROPRIETARIO,   ");
                stb.AppendLine("   Zoo_Animali.CF_DETENTORE,   ");
                stb.AppendLine("   Zoo_Animali.AUSL_AZI_NASCITA,   ");
                stb.AppendLine("   CASE WHEN Zoo_Animali.Id_Capo_BDN = 0 THEN 'No' ELSE 'Si' END AS FlagBDN,   ");
                stb.AppendLine("   Zoo_Animali.Certificato  ");

                stb.AppendLine("   , Zoo_Animali.Modello4_Ingresso_Numero  ");
                stb.AppendLine("   , Zoo_Animali.Modello4_Uscita_Numero  ");
                stb.AppendLine("   , Zoo_Animali.Modello4_Ingresso_Prenotazione  ");
                stb.AppendLine("   , Zoo_Animali.Modello4_Uscita_Prenotazione  ");
                stb.AppendLine("   , Zoo_Animali.Codice_Azienda_Uscita  ");
                stb.AppendLine("   , Zoo_Animali.Data_Documento_Ingresso  ");
                stb.AppendLine("   , Zoo_Animali.Data_Documento_Uscita  ");

                stb.AppendLine($"   , DATEDIFF(day, Zoo_Animali.Validita_Inizio, CONVERT(Date,@{nameof(data)},120)) as giorni_stalla ");

                stb.AppendLine($"   , DATEDIFF(day, DataPrimoCaricamento.Data_Movimento,  CONVERT(Date,@{nameof(data)},120)) as giorni_stalla_primo_caricamento ");
                stb.AppendLine("   , DataPrimoCaricamento.Data_Movimento as data_primo_caricamento ");
                stb.AppendLine($"   , DATEADD(day, @{nameof(giorniStazionamento)} , DataPrimoCaricamento.Data_Movimento) as data_giorni_da_primo_caricamento, ");
                stb.AppendLine(Calcolo_Mesi("Zoo_Animali.Dat_Nascita", $"DATEADD(day, @{nameof(giorniStazionamento)}, DataPrimoCaricamento.Data_Movimento)") + " as eta_mesi_da_primo_caricamento, ");
                stb.AppendLine(Calcolo_Giorni("Zoo_Animali.Dat_Nascita", $"DATEADD(day, @{nameof(giorniStazionamento)}, DataPrimoCaricamento.Data_Movimento)") + " as eta_giorni_da_primo_caricamento ");

                stb.AppendLine($"   , DATEADD(day, @{nameof(giorniStazionamento)}, Zoo_Animali.Validita_Inizio) as Uscita_Data,  ");
                stb.AppendLine($"    DATEDIFF(day, Zoo_Animali.Dat_Nascita, CONVERT(Date,@{nameof(data)},120)) as eta_giorni_totali,");
                stb.AppendLine(Calcolo_Mesi("Zoo_Animali.Dat_Nascita", $"CONVERT(Date,@{nameof(data)},120)") + " as eta_mesi,");
                stb.AppendLine(Calcolo_Giorni("Zoo_Animali.Dat_Nascita", $"CONVERT(Date,@{nameof(data)},120)") + " as eta_giorni,");

                stb.AppendLine($"    DATEDIFF(day, Zoo_Animali.Dat_Nascita, DATEADD(day, @{nameof(giorniStazionamento)}, Zoo_Animali.Validita_Inizio)) as eta_giorni_totali_uscita,");
                stb.AppendLine(Calcolo_Mesi("Zoo_Animali.Dat_Nascita", $"DATEADD(day, @{nameof(giorniStazionamento)} , Zoo_Animali.Validita_Inizio)") + " as eta_mesi_uscita,");
                stb.AppendLine(Calcolo_Giorni("Zoo_Animali.Dat_Nascita", $"DATEADD(day, @{nameof(giorniStazionamento)}, Zoo_Animali.Validita_Inizio)") + " as eta_giorni_uscita");

                stb.AppendLine("FROM giac_cte ");
                stb.AppendLine(" INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale  ");
                stb.AppendLine(" INNER JOIN DataPrimoCaricamento ON Zoo_Animali.Matricola = DataPrimoCaricamento.Matricola ");
                if (filtraGiacenze1)
                {
                    stb.AppendLine(" INNER JOIN Zoo_Animali_Distinte (NOLOCK) ON Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ");
                    switch (bAll)
                    {
                        case false:
                            {
                                stb.AppendLine($"                                         AND CAST(Zoo_Animali_Distinte.Validita_Inizio as datetime) <=    CONVERT(DateTime,@{nameof(data)},120) ");
                                stb.AppendLine($"                                         And CAST(Zoo_Animali_Distinte.Validita_Fine as datetime) >=    CONVERT(DateTime,@{nameof(data)},120) ");
                                break;
                            }
                        case true:
                            {
                                stb.AppendLine($"                                         AND CAST(Zoo_Animali_Distinte.Validita_Inizio as date) <=    CONVERT(Date,@{nameof(data)},120) ");
                                stb.AppendLine($"                                         And CAST(Zoo_Animali_Distinte.Validita_Fine as date) >=    CONVERT(Date,@{nameof(data)},120) ");
                                break;
                            }
                    }
                }

                if (filtraGiacenze1)
                {
                    stb.AppendLine(" INNER JOIN Zoo_AnimalixStati_Accrescimento ON Zoo_AnimalixStati_Accrescimento.Cod_Progetto  = Zoo_Animali.Cod_Progetto  ");
                    switch (bAll)
                    {
                        case false:
                            {
                                stb.AppendLine($"                                           AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Inizio as datetime) <=    CONVERT(DateTime,@{nameof(data)},120) ");
                                stb.AppendLine($"                                           And CAST(Zoo_AnimalixStati_Accrescimento.Validita_Fine as datetime) >=   CONVERT(DateTime,@{nameof(data)},120) ");
                                break;
                            }
                        case true:
                            {
                                stb.AppendLine($"                                           AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Inizio as date) <=    CONVERT(Date,@{nameof(data)},120) ");
                                stb.AppendLine($"                                           And CAST(Zoo_AnimalixStati_Accrescimento.Validita_Fine as date) >=   CONVERT(Date,@{nameof(data)},120) ");
                                break;
                            }
                    }

                }

                if (filtraGiacenze1)
                {
                    stb.AppendLine("INNER JOIN Zoo_Animali_Lista_Stati_Accrescimento (NOLOCK) ON Zoo_AnimalixStati_Accrescimento.GEN_COD = Zoo_Animali_Lista_Stati_Accrescimento.GEN_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.SPE_COD = Zoo_Animali_Lista_Stati_Accrescimento.SPE_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.STATO_COD = Zoo_Animali_Lista_Stati_Accrescimento.STATO_COD  ");
                    stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.TIPO_COD = Zoo_Animali_Lista_Stati_Accrescimento.TIPO_COD  ");
                }

                stb.AppendLine("INNER JOIN Lista_Specie_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Specie_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Specie_Animali.SPE_COD  ");
                stb.AppendLine("INNER JOIN Lista_Razze_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Razze_Animali.SPE_COD And Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali razza_madre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_madre.GEN_COD And Zoo_Animali.SPE_COD = razza_madre.SPE_COD And Zoo_Animali.Razza_Madre = razza_madre.RAZ_COD   ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali razza_padre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_padre.GEN_COD And Zoo_Animali.SPE_COD = razza_padre.SPE_COD And Zoo_Animali.Razza_Padre = razza_padre.RAZ_COD   ");
                stb.AppendLine("INNER JOIN Zoo_Animali_Lista_Tipi (NOLOCK) ON Zoo_Animali.GEN_COD = Zoo_Animali_Lista_Tipi.GEN_COD And Zoo_Animali.SPE_COD = Zoo_Animali_Lista_Tipi.SPE_COD And Zoo_Animali.TIPO_COD = Zoo_Animali_Lista_Tipi.TIPO_COD  ");
                stb.AppendLine("INNER JOIN Lista_IndirizziProd_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_IndirizziProd_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_IndirizziProd_Animali.SPE_COD And Zoo_Animali.IPRO_COD = Lista_IndirizziProd_Animali.IPRO_COD  ");
                stb.AppendLine("LEFT  JOIN Contatti (NOLOCK) ON Zoo_Animali.CF_Fornitore = Contatti.Cod_Contatto AND Zoo_Animali.Piva = Contatti.Piva ");
                if (filtroVisibilitaUtente)
                {
                    stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Zoo_Animali.Piva = p.piva AND p.Sa_Cod = giac_cte.Sa_Cod AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
                }

                stb.AppendLine(" WHERE 1=1 ");
                if (filtraGiacenze1)
                {
                    switch (bAll)
                    {
                        case false:
                            {
                                break;
                            }

                        case true:
                            {
                                stb.AppendLine($" And CAST(Zoo_Animali.Validita_Inizio as Date) <= CONVERT(Date,@{nameof(data)},120)   ");
                                stb.AppendLine($" And CAST(Zoo_Animali.Validita_Fine as Date) >= CONVERT(Date,@{nameof(data)},120)   ");
                                break;
                            }
                    }
                }

                if (piva != "")
                {
                    stb.AppendLine($" And giac_cte.Piva = @{nameof(piva)}      ");
                }

                if (saCod != 0)
                {
                    stb.AppendLine($" And giac_cte.sa_cod = @{nameof(saCod)}      ");
                }

                if (staNum != 0)
                {
                    stb.AppendLine($" AND giac_cte.Sta_NUM = @{nameof(staNum)}  ");
                }

                if (raggruppamentoCod != 0)
                {
                    stb.AppendLine($" AND giac_cte.Raggruppamento_Cod = @{nameof(raggruppamentoCod)}  ");
                }

                if (codAnimale != 0)
                {
                    stb.AppendLine($" AND giac_cte.Cod_Animale = @{nameof(codAnimale)}  ");
                }

                if (compatibilityLevel >= 150)
                {
                    stb.AppendLine(" OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) ");
                }
                // stb.AppendLine("ORDER BY giac_cte.Impresa ")

                // ------------------------------------------------------------------------------------------------------

                var sqlParamIn = new Dictionary<string, Dictionary<Type, List<object>>>();

                var inParams = sqlParams
                    .Where(x => x.Value is Dictionary<Type, List<object>>)
                    .ToList();
                for (var i = 0; i < inParams.Count; i++)
                {
                    sqlParamIn.Add(inParams[i].Key, (Dictionary<Type, List<object>>)inParams[i].Value);
                    sqlParams.Remove(inParams[i].Key);
                }
                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stb.ToString(), sqlParams, sqlParamIn);
                // ------------------------------------------------------------------------------------------------------

                if (listCodAnimali != null && listCodAnimali.Any() && idTestataTemp != 0)
                {
                    await _tempAgenda.CancellaRecordDaIDTestataTempAsync(idTestataTemp, objParametri);
                }
            }

            catch (Exception ex)
            {
                messaggioErrore = ex.Message;
                LogError(messaggioErrore, objParametri, ex);
                dt = default;
                throw new Exception("[" + nomeRoutine + "] : " + messaggioErrore);
            }

            return dt;
        }

        public async Task<DataTable> LeggiCapiSenzaTrattamentiAsync(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data,
            int giorniSenzaTrattamenti,
            bool filtroVisibilitaUtente,
            List<int>? listCodAnimali,
            int[]? farmCatList,
            int[]? farmCatSemplList,
            AgronicaCoreParametri objParametri,
            int compatibilityLevel,
            bool mostraGGPrimoCaricamento = false
            )
        {
            const string nomeRoutine = "AgronicaCoreAnagrafeBIZ.Zoo.Leggi_Capi_Senza_Trattamenti()";

            string messaggioErrore = "";
            var stb = new StringBuilder();
            DataTable dt;
            var sqlParams = new Dictionary<string, object>();
            try
            {
                sqlParams.TryAdd($"@{nameof(piva)}", piva);
                sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
                sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
                sqlParams.TryAdd($"@{nameof(raggruppamentoCod)}", raggruppamentoCod);
                sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
                sqlParams.TryAdd($"@{nameof(data)}", data);
                sqlParams.TryAdd($"@{nameof(giorniSenzaTrattamenti)}", giorniSenzaTrattamenti);
                sqlParams.TryAdd($"@{nameof(filtroVisibilitaUtente)}", filtroVisibilitaUtente);

                if (farmCatSemplList != null && farmCatSemplList.Any())
                {
                    sqlParams.TryAdd($"@{nameof(farmCatSemplList)}In", FormatClauseIn(farmCatSemplList.ToList()));
                }
                sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);

                var minDate = new DateTime(1900, 1, 1).Date;
                var maxDate = new DateTime(2100, 12, 31).Date;
                sqlParams.TryAdd($"@{nameof(minDate)}", minDate);
                sqlParams.TryAdd($"@{nameof(maxDate)}", maxDate);
                var tipiDest = new List<int>
                {
                    {TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA},
                    {TIPO_DESTINAZIONE.STALLA}
                };
                sqlParams.TryAdd($"@{nameof(tipiDest)}In", FormatClauseIn(tipiDest));


                int idTestataTemp = await _agrosequences.NuovoId_TabellaAsync("__Tmp_Agenda", 0, int.MaxValue, objParametri);
                if (listCodAnimali != null && listCodAnimali.Any())
                {
                    sqlParams.TryAdd($"@{nameof(listCodAnimali)}In", FormatClauseIn(listCodAnimali.ToList()));
                }

                stb.Length = 0;
                stb.AppendLine("  --Creazione agn_cte  ");
                var agenda_cte = crea_agn_cte(piva, saCod, staNum, raggruppamentoCod, codAnimale, data, giorniSenzaTrattamenti, filtroVisibilitaUtente, listCodAnimali, idTestataTemp, sqlParams, objParametri);
                stb.AppendLine(agenda_cte);

                stb.AppendLine("  --Creazione Categorie_Farmaci_Con_Semplificati  ");
                var categorie_farmaci_con_Semplificati_cte = crea_Categorie_Farmaci_Con_Semplificati();
                stb.AppendLine(categorie_farmaci_con_Semplificati_cte);

                stb.AppendLine("  --Creazione sospensioneCarne_cte");
                var sospensioneCarne_cte = crea_sospensioneCarne_cte(piva, filtroVisibilitaUtente, sqlParams, objParametri);
                stb.AppendLine(sospensioneCarne_cte);

                stb.AppendLine("  --Creazione sospensioneLatte_cte");
                var sospensioneLatte_cte = crea_sospensioneLatte_cte(piva, filtroVisibilitaUtente, sqlParams, objParametri);
                stb.AppendLine(sospensioneLatte_cte);

                stb.AppendLine("  --Creazione Trattamenti_cte");
                var trattamenti_cte = crea_trattamenti_cte(piva, saCod, staNum, raggruppamentoCod, codAnimale, data, giorniSenzaTrattamenti, listCodAnimali,
                                                            farmCatList, farmCatSemplList, idTestataTemp, filtroVisibilitaUtente, sqlParams, objParametri);
                stb.AppendLine(trattamenti_cte);

                stb.AppendLine("  --Creazione giac_cte");
                var giac_cte = crea_giac_cte(piva, saCod, staNum, sqlParams);
                stb.AppendLine(giac_cte);

                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine(" --Creazione ZooMatricoleList");
                    var zooMatricoleList_cte = crea_zooMatricoleList();
                    stb.AppendLine(zooMatricoleList_cte);

                    stb.AppendLine(" --Creazione DataPrimoCaricamento");
                    var DataPrimoCaricamento_cte = crea_DataPrimoCaricamento(piva, sqlParams);
                    stb.AppendLine(DataPrimoCaricamento_cte);
                }

                stb.AppendLine(" SELECT giac_cte.Piva + '_' + CAST(giac_cte.Sa_Cod as varchar(100)) + '_' + CAST(giac_cte.Cod_Animale as varchar(100)) as chiave,  ");
                stb.AppendLine(" giac_cte.*,  ");
                stb.AppendLine(" Lista_Specie_Animali.SPE_DES,  ");
                stb.AppendLine(" Lista_Razze_Animali.RAZ_DES,  ");
                stb.AppendLine(" Zoo_Animali_Lista_Tipi.Tipo_Des,  ");
                stb.AppendLine(" Lista_IndirizziProd_Animali.IPRO_DES,  ");
                stb.AppendLine(" Zoo_Animali_Distinte.Cod_Progetto,  ");
                stb.AppendLine(" Zoo_Animali_Distinte.Progetto_Des,  ");
                stb.AppendLine(" Zoo_Animali_Distinte.Progetto_Nome,  ");
                stb.AppendLine(" Zoo_Animali_Distinte.Codice_Distinta,  ");
                stb.AppendLine(" Zoo_AnimalixStati_Accrescimento.Stato_Cod,  ");
                stb.AppendLine(" Zoo_Animali_Lista_Stati_Accrescimento.Stato_Des,  ");
                stb.AppendLine(" Contatti.Rag_Soc,  ");
                stb.AppendLine(" Contatti.Cod_Contatto, ");
                stb.AppendLine("  Zoo_Animali.RAZ_COD,  ");
                stb.AppendLine("    Zoo_Animali.IPRO_COD,  ");
                stb.AppendLine("    Zoo_Animali.CF_Fornitore,  ");
                stb.AppendLine("    Zoo_Animali.Mat_Madre,   ");
                stb.AppendLine("    Zoo_Animali.Mat_Padre,   ");
                stb.AppendLine("    CASE Zoo_Animali.Metodo_Produzione WHEN 1 THEN 'Integrato' WHEN 2 THEN 'In Conversione' WHEN 3 THEN 'Biologico' END as Metodo_Produzione,   ");
                stb.AppendLine("    Zoo_Animali.Razza_Padre AS RazCod_Padre,   ");
                stb.AppendLine("    razza_madre.RAZ_DES as RazDes_Madre,   ");
                stb.AppendLine("    Zoo_Animali.Razza_Madre AS RazCod_Madre,   ");
                stb.AppendLine("    razza_padre.RAZ_DES as RazDes_Padre,   ");
                stb.AppendLine("    Zoo_Animali.Lotto_Fornitore, ");
                stb.AppendLine("    Zoo_Animali.Matricola,   ");
                stb.AppendLine("    Zoo_Animali.GEN_COD,   ");
                stb.AppendLine("    Zoo_Animali.SPE_COD,   ");
                stb.AppendLine("    Zoo_Animali.TIPO_COD,   ");
                stb.AppendLine("    Zoo_Animali.Progetto,   ");
                stb.AppendLine("    Zoo_Animali.Validita_Inizio,   ");
                stb.AppendLine("    Zoo_Animali.Validita_Fine,   ");
                stb.AppendLine("    Zoo_Animali.Nome,   ");
                stb.AppendLine("    Zoo_Animali.Sesso,   ");
                stb.AppendLine("    Zoo_Animali.Modello4_Ingresso,   ");
                stb.AppendLine("    Zoo_Animali.Modello4_Uscita,   ");
                stb.AppendLine("    ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Creazione), ZOO_Animali.Username_Creazione) AS Utente_Creazione,   ");
                stb.AppendLine("    ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Modifica), ZOO_Animali.Username_Modifica) AS Utente_Modifica,   ");
                stb.AppendLine("    Zoo_Animali.Data_Creazione,   ");
                stb.AppendLine("    Zoo_Animali.Data_Modifica,   ");
                stb.AppendLine("    Zoo_Animali.Dat_Nascita,   ");
                stb.AppendLine($"    DATEDIFF(day, Zoo_Animali.Dat_Nascita, CONVERT(DateTime,@{nameof(data)},120)) as eta_giorni_totali,");

                stb.AppendLine(Calcolo_Mesi("Zoo_Animali.Dat_Nascita", $"CONVERT(DateTime,@{nameof(data)},120)") + " as eta_mesi,");

                stb.AppendLine(Calcolo_Giorni("Zoo_Animali.Dat_Nascita", $"CONVERT(DateTime,@{nameof(data)},120)") + " as eta_giorni,");


                stb.AppendLine($"    DATEDIFF(day, Zoo_Animali.Validita_Inizio, CONVERT(DateTime,@{nameof(data)},120)) as giorni_totali_stalla,");
                stb.AppendLine(Calcolo_Mesi("Zoo_Animali.Validita_Inizio", $"CONVERT(DateTime,@{nameof(data)},120)") + " as mesi_stalla,");
                stb.AppendLine(Calcolo_Giorni("Zoo_Animali.Validita_Inizio", $"CONVERT(DateTime,@{nameof(data)},120)") + " as giorni_stalla,");

                stb.AppendLine("    Zoo_Animali.Id_Capo_BDN,   ");
                stb.AppendLine("    Zoo_Animali.CF_PROPRIETARIO,   ");
                stb.AppendLine("    Zoo_Animali.CF_DETENTORE,   ");
                stb.AppendLine("    Zoo_Animali.AUSL_AZI_NASCITA,   ");
                stb.AppendLine("    CASE WHEN Zoo_Animali.Id_Capo_BDN = 0 THEN 'No' ELSE 'Si' END AS FlagBDN,   ");
                stb.AppendLine("    Zoo_Animali.Certificato  ");
                stb.AppendLine("    , Zoo_Animali.Modello4_Ingresso_Numero  ");
                stb.AppendLine("    , Zoo_Animali.Modello4_Uscita_Numero  ");
                stb.AppendLine("    , Zoo_Animali.Modello4_Ingresso_Prenotazione  ");
                stb.AppendLine("    , Zoo_Animali.Modello4_Uscita_Prenotazione  ");
                stb.AppendLine("    , Zoo_Animali.Codice_Azienda_Uscita  ");
                stb.AppendLine("    , Zoo_Animali.Data_Documento_Ingresso  ");
                stb.AppendLine("    , Zoo_Animali.Data_Documento_Uscita ");
                stb.AppendLine("    , Zoo_Animali.Note ");
                stb.AppendLine("    , Lista_Patologie.Patologia_Des ");
                stb.AppendLine("    , trattamenti_cte.Data_Ultimo_Trattamento");
                stb.AppendLine($"    , DATEADD(DAY, @{nameof(giorniSenzaTrattamenti)}, trattamenti_cte.Data_Ultimo_Trattamento) as Data_Disponibilita ");
                stb.AppendLine("    , trattamenti_cte.Data_Sospensione_Carne");
                stb.AppendLine("    , trattamenti_cte.Data_Sospensione_Latte");
                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine($"   , DATEDIFF(day, DataPrimoCaricamento.Data_Movimento,   CONVERT(DateTime,@{nameof(data)},120)  ) as giorni_stalla_primo_caricamento ");
                    stb.AppendLine("   , DataPrimoCaricamento.Data_Movimento as data_primo_caricamento ");
                }
                stb.AppendLine(" FROM #giac_cte giac_cte ");
                stb.AppendLine("  INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale  ");
                stb.AppendLine("  INNER JOIN Zoo_Animali_Distinte (NOLOCK) ON Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ");
                stb.AppendLine($"                                          AND CAST(Zoo_Animali_Distinte.Validita_Inizio as datetime) <=     CONVERT(DateTime,@{nameof(data)},120)  ");
                stb.AppendLine($"                                          And CAST(Zoo_Animali_Distinte.Validita_Fine as datetime) >=     CONVERT(DateTime,@{nameof(data)},120) ");
                stb.AppendLine("  INNER JOIN Zoo_AnimalixStati_Accrescimento ON Zoo_AnimalixStati_Accrescimento.Cod_Progetto  = Zoo_Animali.Cod_Progetto  ");
                stb.AppendLine($"                                            AND CAST(Zoo_AnimalixStati_Accrescimento.Validita_Inizio as datetime) <=     CONVERT(DateTime,@{nameof(data)},120)  ");
                stb.AppendLine($"                                            And CAST(Zoo_AnimalixStati_Accrescimento.Validita_Fine as datetime) >=    CONVERT(DateTime,@{nameof(data)},120)  ");
                stb.AppendLine(" INNER JOIN Zoo_Animali_Lista_Stati_Accrescimento (NOLOCK) ON Zoo_AnimalixStati_Accrescimento.GEN_COD = Zoo_Animali_Lista_Stati_Accrescimento.GEN_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.SPE_COD = Zoo_Animali_Lista_Stati_Accrescimento.SPE_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.STATO_COD = Zoo_Animali_Lista_Stati_Accrescimento.STATO_COD  ");
                stb.AppendLine("                                                AND Zoo_AnimalixStati_Accrescimento.TIPO_COD = Zoo_Animali_Lista_Stati_Accrescimento.TIPO_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Specie_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Specie_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Specie_Animali.SPE_COD  ");
                stb.AppendLine(" INNER JOIN Lista_Razze_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_Razze_Animali.SPE_COD And Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD  ");
                stb.AppendLine("  INNER JOIN Lista_Razze_Animali razza_madre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_madre.GEN_COD And Zoo_Animali.SPE_COD = razza_madre.SPE_COD And Zoo_Animali.Razza_Madre = razza_madre.RAZ_COD   ");
                stb.AppendLine("  INNER JOIN Lista_Razze_Animali razza_padre (NOLOCK) ON Zoo_Animali.GEN_COD = razza_padre.GEN_COD And Zoo_Animali.SPE_COD = razza_padre.SPE_COD And Zoo_Animali.Razza_Padre = razza_padre.RAZ_COD   ");
                stb.AppendLine(" INNER JOIN Zoo_Animali_Lista_Tipi (NOLOCK) ON Zoo_Animali.GEN_COD = Zoo_Animali_Lista_Tipi.GEN_COD And Zoo_Animali.SPE_COD = Zoo_Animali_Lista_Tipi.SPE_COD And Zoo_Animali.TIPO_COD = Zoo_Animali_Lista_Tipi.TIPO_COD  ");
                stb.AppendLine(" INNER JOIN Lista_IndirizziProd_Animali (NOLOCK) ON Zoo_Animali.GEN_COD = Lista_IndirizziProd_Animali.GEN_COD And Zoo_Animali.SPE_COD = Lista_IndirizziProd_Animali.SPE_COD And Zoo_Animali.IPRO_COD = Lista_IndirizziProd_Animali.IPRO_COD  ");
                stb.AppendLine(" LEFT  JOIN Contatti (NOLOCK) ON Zoo_Animali.CF_Fornitore = Contatti.Cod_Contatto AND Zoo_Animali.Piva = Contatti.Piva ");
                stb.AppendLine(" LEFT JOIN #trattamenti_cte trattamenti_cte ON giac_cte.Cod_Animale = trattamenti_cte.Cod_Animale");
                stb.AppendLine("  LEFT JOIN Lista_Patologie ON Lista_Patologie.Patologia_Cod = Zoo_Animali.Id_Patologia  ");
                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine(" INNER JOIN #DataPrimoCaricamento DataPrimoCaricamento ON Zoo_Animali.Matricola = DataPrimoCaricamento.Matricola ");
                }
                if (filtroVisibilitaUtente)
                {
                    stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON giac_cte.Piva = p.piva AND p.Sa_Cod = giac_cte.Sa_Cod AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
                }
                stb.AppendLine("  WHERE 1=1 ");
                if (!string.IsNullOrEmpty(piva))
                {
                    stb.AppendLine($"  And giac_cte.Piva = @{nameof(piva)}      ");
                }
                if (saCod != 0)
                {
                    stb.AppendLine($"  And giac_cte.sa_cod = @{nameof(saCod)}      ");
                }

                if (raggruppamentoCod != 0)
                {
                    stb.AppendLine($"  And giac_cte.Raggruppamento_Cod = @{nameof(raggruppamentoCod)}      ");
                }

                stb.AppendLine($"   AND Zoo_Animali.Validita_Fine >= CONVERT(DateTime,@{nameof(data)},120) ");

                if (compatibilityLevel >= 150)
                {
                    stb.AppendLine(" OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) ");
                }

                stb.AppendLine("DROP TABLE #agn_cte ");
                stb.AppendLine("DROP TABLE #giac_cte ");
                stb.AppendLine("DROP TABLE #Categorie_Farmaci_Con_Semplificati ");
                stb.AppendLine("DROP TABLE #sospensioneCarne_cte ");
                stb.AppendLine("DROP TABLE #sospensioneLatte_cte ");
                stb.AppendLine("DROP TABLE #trattamenti_cte ");
                if (mostraGGPrimoCaricamento)
                {
                    stb.AppendLine("DROP TABLE #DataPrimoCaricamento ");
                    stb.AppendLine("DROP TABLE #ZooMatricoleList ");
                }

                // ------------------------------------------------------------------------------------------------------
                var sqlParamIn = new Dictionary<string, Dictionary<Type, List<object>>>();
                var inParams = sqlParams
                    .Where(x => x.Value is Dictionary<Type, List<object>>)
                    .ToList();
                for (var i = 0; i < inParams.Count; i++)
                {
                    sqlParamIn.Add(inParams[i].Key, (Dictionary<Type, List<object>>)inParams[i].Value);
                    sqlParams.Remove(inParams[i].Key);
                }
                dt = await GetDataProvider(objParametri).ExecuteReadAsync(stb.ToString(), sqlParams, sqlParamIn);
                // ------------------------------------------------------------------------------------------------------

                if (listCodAnimali is not null && listCodAnimali.Any() && idTestataTemp != 0)
                {
                    await _tempAgenda.CancellaRecordDaIDTestataTempAsync(idTestataTemp, objParametri);
                }
            }

            catch (Exception ex)
            {
                messaggioErrore = ex.Message;
                LogError(messaggioErrore, objParametri, ex);
                dt = default;
                throw new Exception("[" + nomeRoutine + "] : " + messaggioErrore);
            }

            return dt;
        }

        private string crea_agn_cte(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data,
            int giorniSenzaTrattamenti,
            bool filtroVisibilitaUtente,
            List<int> listCodAnimali,
            int idTestataTemp,
            Dictionary<string, object> sqlParams,
            AgronicaCoreParametri objParametri)
        {
            var stb = new StringBuilder();
            sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
            sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
            sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
            sqlParams.TryAdd($"@{nameof(raggruppamentoCod)}", raggruppamentoCod);
            sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
            sqlParams.TryAdd($"@{nameof(data)}", data);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);
            sqlParams.TryAdd($"@{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)}", LAV_COD.LAVCOD_NASCITA_ANIMALI);
            sqlParams.TryAdd($"@{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)}", LAV_COD.MAX_OPERAZIONE_ZOO);

            var cauMoves = new List<string>
            {
                $"{CAU_MOV.CAU_CARICO_CAPO}",
                $"{CAU_MOV.CAU_SCARICO_CAPO}",
                $"{CAU_MOV.CAU_CARICO}",
                $"{CAU_MOV.CAU_SCARICO}"
            };
            sqlParams.TryAdd($"@{nameof(cauMoves)}In", FormatClauseIn(cauMoves));

            var minDate = new DateTime(1900, 1, 1).Date;
            var maxDate = new DateTime(2100, 12, 31).Date;
            sqlParams.TryAdd($"@{nameof(minDate)}", new DateTime(1900, 1, 1).Date);
            sqlParams.TryAdd($"@{nameof(maxDate)}", new DateTime(2100, 12, 31).Date);
            var tipiDest = new List<int>
            {
                {TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA},
                {TIPO_DESTINAZIONE.STALLA}
            };
            sqlParams.TryAdd($"@{nameof(tipiDest)}In", FormatClauseIn(tipiDest));

            stb.AppendLine("  select a.Id_Agenda,  ");
            stb.AppendLine("    a.PIVA,  ");
            stb.AppendLine("    md.Cod_Progetto, ");
            stb.AppendLine("    md.Lotto, ");
            stb.AppendLine("    md.Udm_Cod, ");
            stb.AppendLine("    UnitaMisura.Udm_Des, ");
            stb.AppendLine("    UnitaMisura.Udm_Sim, ");
            stb.AppendLine("    mdes.Sa_Cod, ");
            stb.AppendLine("    mdes.Id_Destinazione, ");
            stb.AppendLine("    mdes.Tipo_Destinazione, ");
            stb.AppendLine("    mdes.Qta, ");
            stb.AppendLine("    md.Id_Mov, ");
            stb.AppendLine("    md.Id_Mov_Det, ");
            stb.AppendLine("    m.Cau_Mov, ");
            stb.AppendLine("    i.rag_soc, ");
            stb.AppendLine("    ic.val_cod as cuaa, ");
            stb.AppendLine("    Centri_Aziendali.sa_nome ");
            stb.AppendLine("    INTO #agn_cte  ");
            stb.AppendLine("    from agenda a (NOLOCK)  ");
            stb.AppendLine("    inner join imprese i (NOLOCK) on i.PIVA = a.PIVA ");
            stb.AppendLine("    inner join imprese_codici ic (NOLOCK) on i.piva = ic.piva and ic.id_cod = 1010 ");
            stb.AppendLine("    inner join Movimenti m (NOLOCK) on  a.Id_Agenda = m.Id_Agenda  ");
            stb.AppendLine("    inner join Movimenti_dettagli md (NOLOCK) on md.Id_Agenda = m.Id_Agenda  AND md.Id_Mov = m.Id_Mov  ");
            stb.AppendLine("    inner join mov_destinazioni mdes (NOLOCK) on mdes.Id_Agenda = md.Id_Agenda and mdes.Id_Mov = md.Id_Mov and mdes.Id_Mov_Det = md.Id_Mov_Det ");
            stb.AppendLine("    inner join Centri_Aziendali (NOLOCK) ON mdes.Piva = Centri_Aziendali.Piva and mdes.Sa_Cod=Centri_Aziendali.sa_cod  ");
            stb.AppendLine("    INNER Join UnitaMisura (NOLOCK) ON UnitaMisura.Udm_Cod = md.Udm_Cod ");


            if (listCodAnimali is not null && listCodAnimali.Any())
            {
                stb.AppendLine($"   INNER Join __Tmp_Agenda (NOLOCK) ON md.Cod_Progetto = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");
            }
            if (string.IsNullOrWhiteSpace(piva) && filtroVisibilitaUtente)
            {
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Agenda.Piva = p.piva AND p.Sa_Cod = giac_cte.Sa_Cod AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
            }
            stb.AppendLine("    where  1 = 1 ");
            if (!string.IsNullOrEmpty(piva))
            {
                stb.AppendLine($" And a.Piva = @{nameof(piva)}      ");
            }

            if (saCod != 0)
            {
                stb.AppendLine($" And mdes.sa_cod = @{nameof(saCod)}      ");
            }

            if (codAnimale != 0)
            {
                stb.AppendLine($" AND md.Cod_Progetto  = @{nameof(codAnimale)}  ");
            }
            stb.AppendLine($"   and a.lav_Cod >= @{nameof(LAV_COD.LAVCOD_NASCITA_ANIMALI)} And a.lav_cod <= @{nameof(LAV_COD.MAX_OPERAZIONE_ZOO)} ");
            stb.AppendLine($"   And m.Cau_Mov In (@{nameof(cauMoves)}In)     ");
            stb.AppendLine($"   And m.Data_Movimento >=  CONVERT(DateTime,@{nameof(minDate)},120)     ");
            stb.AppendLine($"   And m.Data_Movimento <=  CONVERT(DateTime,@{nameof(data)},120)     ");
            stb.AppendLine("    And md.Elem_Cod = 300  ");
            stb.AppendLine("    And md.Jolly_Int = 0  ");
            stb.AppendLine($"   And mdes.Tipo_Destinazione in (@{nameof(tipiDest)}In)  ");

            return stb.ToString();
        }
        private string crea_Categorie_Farmaci_Con_Semplificati()
        {
            var stb = new StringBuilder();

            stb.AppendLine(" SELECT ");
            stb.AppendLine("    Farmaci_Categorie.ID,  ");
            stb.AppendLine("    Farmaci_Categorie.Categoria_Codice,  ");
            stb.AppendLine("    Farmaci_Categorie.Categoria_Descrizione, ");
            stb.AppendLine("    CONCAT('|' ,STRING_AGG(Farmaci_Categorie_Semplificate.ID, '|') , '|') as Categorie_Semplificate_Cod, ");
            stb.AppendLine("    COALESCE(STRING_AGG(Farmaci_Categorie_Semplificate.Descrizione, ','), '') as Categorie_Semplificate_Des ");
            stb.AppendLine(" INTO #Categorie_Farmaci_Con_Semplificati ");
            stb.AppendLine(" FROM Farmaci_Categorie ");
            stb.AppendLine(" LEFT JOIN Farmaci_CategoriexFarmaci_Categorie_Semplificate ON Farmaci_Categorie.ID = Farmaci_CategoriexFarmaci_Categorie_Semplificate.ID_Categoria ");
            stb.AppendLine(" LEFT JOIN Farmaci_Categorie_Semplificate ON Farmaci_CategoriexFarmaci_Categorie_Semplificate.ID_Categoria_Semplificata = Farmaci_Categorie_Semplificate.ID ");
            stb.AppendLine(" GROUP BY Farmaci_Categorie.ID, Farmaci_Categorie.Categoria_Codice, Farmaci_Categorie.Categoria_Descrizione ");

            return stb.ToString();
        }
        private string crea_sospensioneCarne_cte(string piva, bool filtroVisibilitaUtente, Dictionary<string, object> sqlParams, AgronicaCoreParametri objParametri)
        {
            var stb = new StringBuilder();
            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}", CAU_MOV.CAU_TRATTAMENTO_ZOO);
            sqlParams.TryAdd($"@{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE)}", TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE);
            sqlParams.TryAdd($"@{nameof(DETT_TEC.SOSPENSIONE_CARNE)}", DETT_TEC.SOSPENSIONE_CARNE);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);

            stb.AppendLine("    SELECT Mov_Destinazioni.Id_Destinazione as Cod_Animale,");
            stb.AppendLine("        MAX(IIF(mdt_Carne.Extra_Int > 0, DATEADD(DAY, mdt_Carne.Extra_Int, Agenda.Validita_Fine), '')) AS Data_Sospensione_Carne ");
            stb.AppendLine("    INTO #sospensioneCarne_cte ");
            stb.AppendLine("    FROM Agenda  (NOLOCK)  ");
            stb.AppendLine($"    JOIN Movimenti  (NOLOCK) ON Agenda.Id_Agenda = Movimenti.Id_Agenda AND Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}");
            stb.AppendLine("    JOIN Movimenti_Dettagli  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Movimenti.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Movimenti.Id_Mov ");
            stb.AppendLine("    JOIN Mov_Destinazioni  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Mov_Destinazioni.Id_Mov ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det ");
            stb.AppendLine($"        AND Mov_Destinazioni.Tipo_Destinazione =  @{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE)}");
            stb.AppendLine("    JOIN Mov_Dettaglio_Tecnico AS mdt_Carne  (NOLOCK) ON mdt_Carne.Id_Agenda = Movimenti_Dettagli.Id_Agenda ");
            stb.AppendLine("        AND mdt_Carne.Id_Mov = Movimenti_Dettagli.Id_Mov ");
            stb.AppendLine("        AND mdt_Carne.Id_Mov_Det = Movimenti_Dettagli.Id_Mov_Det ");
            if (string.IsNullOrWhiteSpace(piva) && filtroVisibilitaUtente)
            {
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Agenda.Piva = p.piva AND p.Entita_Cod = 1 AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
            }
            else
            {
                sqlParams.TryAdd($"@{nameof(piva)}", piva);
                stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
            }
            stb.AppendLine($"    WHERE mdt_Carne.Dett_Cod = @{nameof(DETT_TEC.SOSPENSIONE_CARNE)} ");
            stb.AppendLine("    GROUP BY Mov_Destinazioni.Id_Destinazione ");

            return stb.ToString();
        }
        private string crea_sospensioneLatte_cte(string piva, bool filtroVisibilitaUtente, Dictionary<string, object> sqlParams, AgronicaCoreParametri objParametri)
        {
            var stb = new StringBuilder();

            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}", CAU_MOV.CAU_TRATTAMENTO_ZOO);
            sqlParams.TryAdd($"@{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE)}", TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);
            sqlParams.TryAdd($"@{nameof(piva)}", piva);

            stb.AppendLine("    SELECT Mov_Destinazioni.Id_Destinazione as Cod_Animale,");
            stb.AppendLine("        MAX(IIF(mdt_Latte.Extra_Int > 0, DATEADD(DAY, mdt_Latte.Extra_Int, Agenda.Validita_Fine), '')) AS Data_Sospensione_Latte ");
            stb.AppendLine("    INTO #sospensioneLatte_cte ");
            stb.AppendLine("    FROM Agenda  (NOLOCK) ");
            stb.AppendLine($"    JOIN Movimenti (NOLOCK)  ON Agenda.Id_Agenda = Movimenti.Id_Agenda AND Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}");
            stb.AppendLine("    JOIN Movimenti_Dettagli  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Movimenti.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Movimenti.Id_Mov ");
            stb.AppendLine("    JOIN Mov_Destinazioni  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Mov_Destinazioni.Id_Mov ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det ");
            stb.AppendLine($"        AND Mov_Destinazioni.Tipo_Destinazione = @{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE)} ");
            stb.AppendLine("    JOIN Mov_Dettaglio_Tecnico AS mdt_Latte  (NOLOCK) ON mdt_Latte.Id_Agenda = Movimenti_Dettagli.Id_Agenda ");
            stb.AppendLine("        AND mdt_Latte.Id_Mov = Movimenti_Dettagli.Id_Mov ");
            stb.AppendLine("        AND mdt_Latte.Id_Mov_Det = Movimenti_Dettagli.Id_Mov_Det ");
            if (string.IsNullOrEmpty(piva) && filtroVisibilitaUtente)
            {
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Agenda.Piva = p.piva AND p.Entita_Cod = 1 AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
            }
            stb.AppendLine("    WHERE mdt_Latte.Dett_Cod = 2 ");

            if (!string.IsNullOrEmpty(piva))
            {
                stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
            }

            stb.AppendLine("    GROUP BY Mov_Destinazioni.Id_Destinazione  ");

            return stb.ToString();
        }
        private string crea_trattamenti_cte(string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data, int
            giorniSenzaTrattamenti,
            List<int> listCodAnimali,
            int[]? farmCatList,
            int[]? farmCatSemplList,
            int idTestataTemp,
            bool filtroVisibilitaUtente,
            Dictionary<string, object> sqlParams,
            AgronicaCoreParametri objParametri)
        {

            var stb = new StringBuilder();
            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}", CAU_MOV.CAU_TRATTAMENTO_ZOO);
            sqlParams.TryAdd($"@{nameof(piva)}", piva);
            sqlParams.TryAdd($"@{nameof(idTestataTemp)}", idTestataTemp);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", objParametri.UtenteUsername);

            sqlParams.TryAdd($"@{nameof(data)}", data);
            sqlParams.TryAdd($"@{nameof(giorniSenzaTrattamenti)}", giorniSenzaTrattamenti * -1);

            var minDate = new DateTime(1900, 1, 1).Date;
            var maxDate = new DateTime(2100, 12, 31).Date;
            sqlParams.TryAdd($"@{nameof(minDate)}", new DateTime(1900, 1, 1).Date);
            sqlParams.TryAdd($"@{nameof(maxDate)}", new DateTime(2100, 12, 31).Date);
            var tipiDest = new List<int>
            {
                {TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA},
                {TIPO_DESTINAZIONE.STALLA}
            };
            sqlParams.TryAdd($"@{nameof(tipiDest)}In", FormatClauseIn(tipiDest));


            stb.AppendLine("    SELECT Mov_Destinazioni.Id_Destinazione as Cod_Animale, ");
            stb.AppendLine("    MAX(Agenda.Validita_Fine) as Data_Ultimo_Trattamento, ");
            stb.AppendLine("    sospensioneCarne_cte.Data_Sospensione_Carne AS Data_Sospensione_Carne, ");
            stb.AppendLine("    sospensioneLatte_cte.Data_Sospensione_Latte AS Data_Sospensione_Latte ");
            stb.AppendLine("    INTO #trattamenti_cte ");
            stb.AppendLine("    FROM Agenda (NOLOCK) ");
            stb.AppendLine($"   JOIN Movimenti  (NOLOCK) ON Agenda.Id_Agenda = Movimenti.Id_Agenda AND Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_TRATTAMENTO_ZOO)}");
            stb.AppendLine("    JOIN Movimenti_Dettagli  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Movimenti.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Movimenti.Id_Mov ");
            stb.AppendLine("    JOIN Mov_Destinazioni  (NOLOCK) ON Movimenti_Dettagli.Id_Agenda = Mov_Destinazioni.Id_Agenda ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov = Mov_Destinazioni.Id_Mov ");
            stb.AppendLine("        AND Movimenti_Dettagli.Id_Mov_Det = Mov_Destinazioni.Id_Mov_Det ");
            stb.AppendLine($"       AND Mov_Destinazioni.Tipo_Destinazione = @{nameof(TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_ANIMALE)} ");
            stb.AppendLine("    LEFT JOIN #sospensioneCarne_cte sospensioneCarne_cte ON Mov_Destinazioni.Id_Destinazione = sospensioneCarne_cte.Cod_Animale");
            stb.AppendLine("    LEFT JOIN #sospensioneLatte_cte sospensioneLatte_cte ON Mov_Destinazioni.Id_Destinazione = sospensioneLatte_cte.Cod_Animale");
            stb.AppendLine("    JOIN Farmaci  (NOLOCK) ON Movimenti_Dettagli.Pro_Cod = Farmaci.Farm_Cod");
            stb.AppendLine("    JOIN FarmacixCategorie  (NOLOCK) ON FarmacixCategorie.Farm_Cod = Farmaci.Farm_Cod");
            stb.AppendLine("    JOIN #Categorie_Farmaci_Con_Semplificati Categorie_Farmaci_Con_Semplificati ON FarmacixCategorie.Cat_Cod = Categorie_Farmaci_Con_Semplificati.ID ");
            if (string.IsNullOrEmpty(piva) && filtroVisibilitaUtente)
            {
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p (NOLOCK) ON Agenda.Piva = p.piva AND p.Entita_Cod = 1 AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
            }
            if (listCodAnimali is not null && listCodAnimali.Count > 0)
            {
                stb.AppendLine($"   INNER Join __Tmp_Agenda (NOLOCK) ON Mov_Destinazioni.id_Destinazione = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");
            }
            stb.AppendLine("    WHERE 1 = 1");

            if (!string.IsNullOrEmpty(piva))
            {
                stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
            }

            if (farmCatList is not null && farmCatList.Any())
            {
                sqlParams.TryAdd($"@{nameof(farmCatList)}In", FormatClauseIn(farmCatList.ToList()));
                stb.AppendLine($" And Categorie_Farmaci_Con_Semplificati.ID IN (@{nameof(farmCatList)}In) ");
            }

            if (farmCatSemplList is not null && farmCatSemplList.Any())
            {
                stb.AppendLine(" AND ( ");
                int i = 0;
                foreach (var cat_sempl in farmCatSemplList)
                {
                    if (i != 0)
                    {
                        stb.Append(" OR ");
                    }
                    stb.Append(" Categorie_Farmaci_Con_Semplificati.Categorie_Semplificate_Cod like '%|" + cat_sempl + "|%' ");
                    i = i + 1;
                }
                stb.Append(" ) ");
            }

            stb.AppendLine($"   AND Movimenti.Data_Movimento <= Convert(Datetime,@{nameof(data)},120) ");
            stb.AppendLine($"   AND Movimenti.Data_Movimento >= DATEADD(day, @{nameof(giorniSenzaTrattamenti)}, CONVERT(DateTime,@{nameof(data)},120))");
            stb.AppendLine("    GROUP BY Mov_Destinazioni.Id_Destinazione, sospensioneCarne_cte.Data_Sospensione_Carne, sospensioneLatte_cte.Data_Sospensione_Latte ");

            return stb.ToString();
        }

        private string crea_giac_cte(
            string piva,
            int saCod,
            int staNum,
            Dictionary<string, object> sqlParams
            )
        {
            StringBuilder stb = new StringBuilder();

            sqlParams.TryAdd($"@{nameof(piva)}", piva);
            sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
            sqlParams.TryAdd($"@{nameof(staNum)}", staNum);

            var cauCarico = new List<string>
            {
                $"{CAU_MOV.CAU_SCARICO_CAPO}",
                $"{CAU_MOV.CAU_SCARICO}"
            };
            sqlParams.TryAdd($"@{nameof(cauCarico)}In", FormatClauseIn(cauCarico.ToList()));

            stb.AppendLine("    SELECT   ");
            stb.AppendLine("   agn_cte.Piva, agn_cte.Rag_Soc As Impresa,  ");
            stb.AppendLine("   agn_cte.cuaa, Stalla.BDN_Codice_Azienda,  ");
            stb.AppendLine("   agn_cte.Cod_Progetto AS Cod_Animale,  ");
            stb.AppendLine("   agn_cte.Lotto,  ");
            stb.AppendLine("   agn_cte.Udm_Cod,  ");
            stb.AppendLine("   agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
            stb.AppendLine("   agn_cte.Sa_Cod,  ");
            stb.AppendLine("   agn_cte.sa_nome,  ");
            stb.AppendLine("   agn_cte.Id_Destinazione,  ");
            stb.AppendLine("   agn_cte.Tipo_Destinazione,  ");
            stb.AppendLine("   Lista_AUSL.denominazione AS AUSL_DES,  ");
            stb.AppendLine("   COALESCE(Fabbricati.Fabbricato_Des, '') As STA_DES,  ");
            stb.AppendLine("   COALESCE(Fabbricati.Fabbricato_Cod, 0) As STA_NUM,  ");
            stb.AppendLine("   COALESCE(Stalla_Raggruppamenti.Raggruppamento_Des, '') as Raggruppamento_Des,  ");
            stb.AppendLine("   COALESCE(Stalla_Raggruppamenti.Raggruppamento_Cod, 0) as Raggruppamento_Cod,  ");
            stb.AppendLine($"   Convert(INTEGER, SUM( Case When agn_cte.CAU_MOV In (@{nameof(cauCarico)}In)              ");
            stb.AppendLine("           THEN -(agn_cte.qta)             ");
            stb.AppendLine("           Else agn_cte.qta             ");
            stb.AppendLine("           End)) As Giacenza   ");
            stb.AppendLine("  INTO #giac_cte");
            stb.AppendLine("  From #agn_cte agn_cte  ");
            stb.AppendLine("  INNER Join Stalla_Raggruppamenti (NOLOCK) ON agn_cte.Sa_Cod = Stalla_Raggruppamenti.sa_cod  ");
            stb.AppendLine("                                 AND agn_cte.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod  ");
            stb.AppendLine("                                 AND agn_cte.Tipo_Destinazione = 21  ");
            stb.AppendLine("  INNER JOIN Fabbricati (NOLOCK) ON Stalla_Raggruppamenti.PIVA = Fabbricati.Piva AND Stalla_Raggruppamenti.sa_cod = Fabbricati.SA_COD AND Stalla_Raggruppamenti.STA_NUM = Fabbricati.Fabbricato_Cod ");
            stb.AppendLine("  INNER JOIN Stalla (NOLOCK) ON Stalla.PIVA = Fabbricati.Piva AND Stalla.sa_cod = Fabbricati.SA_COD AND Stalla.STA_NUM = Fabbricati.Fabbricato_Cod  ");
            stb.AppendLine("  INNER JOIN Indirizzi (NOLOCK) ON Fabbricati.Indirizzo_Cod = Indirizzi.cod_indirizzo ");
            stb.AppendLine("  LEFT JOIN IstatxDistretti (NOLOCK) ON IstatxDistretti.pro_cod = Indirizzi.pro_cod_istat AND IstatxDistretti.com_cod = Indirizzi.com_cod_istat ");
            stb.AppendLine("  LEFT JOIN Lista_Distretti (NOLOCK) ON Lista_Distretti.distretto_id = IstatxDistretti.distretto_id ");
            stb.AppendLine("  LEFT JOIN Lista_AUSL (NOLOCK) ON Lista_Distretti.asl_id = Lista_AUSL.asl_id ");
            stb.AppendLine("   where 1=1  ");

            if (piva != "")
            {
                stb.AppendLine($" And agn_cte.Piva = @{nameof(piva)}     ");
            }

            if (saCod != 0)
            {
                stb.AppendLine($" And agn_cte.sa_cod = @{nameof(saCod)}      ");
            }

            if (staNum != 0)
            {
                stb.AppendLine($" AND Fabbricati.Fabbricato_Cod = @{nameof(staNum)}  ");
            }

            stb.AppendLine("  GROUP BY agn_cte.Piva, agn_cte.Rag_Soc, agn_cte.cuaa, Stalla.BDN_Codice_Azienda, ");
            stb.AppendLine("  agn_cte.Cod_Progetto,  ");
            stb.AppendLine("  agn_cte.Lotto,  ");
            stb.AppendLine("  agn_cte.Udm_Cod, ");
            stb.AppendLine("  Fabbricati.Fabbricato_Des, ");
            stb.AppendLine("  Fabbricati.Fabbricato_Cod, ");
            stb.AppendLine("  agn_cte.Sa_Cod,  ");
            stb.AppendLine("  agn_cte.sa_nome, ");
            stb.AppendLine("  agn_cte.Id_Destinazione,  ");
            stb.AppendLine("  agn_cte.Tipo_Destinazione,  ");
            stb.AppendLine("  agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
            stb.AppendLine("  Lista_AUSL.denominazione,  ");
            stb.AppendLine("  Stalla_Raggruppamenti.Raggruppamento_Des,  ");
            stb.AppendLine("  Stalla_Raggruppamenti.Raggruppamento_Cod  ");
            stb.AppendLine($"  HAVING Convert(INTEGER, SUM(Case When agn_cte.Cau_Mov In (@{nameof(cauCarico)}In) THEN -(agn_cte.qta) ELSE agn_cte.qta END)) <> 0   ");

            return stb.ToString();
        }

        private string crea_zooMatricoleList()
        {
            var stb = new StringBuilder();

            stb.AppendLine("    SELECT Zoo_Animali.Matricola  ");
            stb.AppendLine("    INTO #ZooMatricoleList  ");
            stb.AppendLine("    FROM #giac_cte giac_cte   ");
            stb.AppendLine("    INNER Join Zoo_Animali (NOLOCK) ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale    ");
            stb.AppendLine("    WHERE 1=1   ");

            return stb.ToString();
        }

        private string crea_DataPrimoCaricamento(string piva, Dictionary<string, object> sqlParams)
        {
            var stb = new StringBuilder();
            sqlParams.TryAdd($"@{nameof(piva)}", piva);
            stb.AppendLine("  SELECT ZooMatricoleList.Matricola, MIN(Movimenti.Data_Movimento) as Data_Movimento  ");
            stb.AppendLine("  INTO #DataPrimoCaricamento  ");
            stb.AppendLine("  FROM Agenda (NOLOCK)  ");
            stb.AppendLine("  JOIN Movimenti (NOLOCK) ON Agenda.ID_Agenda = Movimenti.ID_Agenda  ");
            stb.AppendLine("  JOIN Movimenti_Dettagli (NOLOCK) ON Movimenti.ID_Agenda = Movimenti_Dettagli.ID_Agenda AND Movimenti.Id_Mov = Movimenti_Dettagli.Id_Mov AND Movimenti_dettagli.Elem_Cod = 300  ");
            stb.AppendLine("  JOIN Zoo_Animali (NOLOCK) ON Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto  ");
            stb.AppendLine("  JOIN #ZooMatricoleList ZooMatricoleList  ON Zoo_Animali.Matricola = ZooMatricoleList.Matricola  ");
            stb.AppendLine("  WHERE 1 = 1  ");
            if (!string.IsNullOrEmpty(piva))
            {
                stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
            }
            stb.AppendLine("  GROUP BY ZooMatricoleList.Matricola  ");

            return stb.ToString();
        }

        private string Calcolo_Mesi(string campo_from, string campo_to)
        {
            string strResult = "";
            strResult += " IIF(DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, " + campo_from + ", " + campo_to + ") , " + campo_from + "), " + campo_to + ") >= 0, " + Environment.NewLine;
            strResult += "     DATEDIFF(month, " + campo_from + ", " + campo_to + "), " + Environment.NewLine;
            strResult += "     DATEDIFF(month, " + campo_from + ", " + campo_to + ") -1) ";
            return strResult;
        }
        private string Calcolo_Giorni(string campo_from, string campo_to)
        {
            string strResult = "";
            strResult += " IIF(DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, " + campo_from + ", " + campo_to + ") , " + campo_from + "), " + campo_to + ") >= 0," + Environment.NewLine;
            strResult += "     DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, " + campo_from + ", " + campo_to + ") , " + campo_from + "), " + campo_to + ")," + Environment.NewLine;
            strResult += "     DATEDIFF(day, DATEADD(MONTH, DATEDIFF(month, " + campo_from + ", " + campo_to + ") - 1, " + campo_from + "), " + campo_to + "))";
            return strResult;
        }
        private string CreaAgn_TT(string piva,
            int saCod,
            int staNum,
            int codAnimale,
            DateTime data,
            Dictionary<string, object> sqlParams,
            ref AgronicaCoreParametri objParametri,
            bool bAll = false,
            bool Filtro_Visibilita_Utente = false,
            List<int> listCod_Animali = null,
            bool filtraGiacenze1 = true,
            int idTestataTemp = 0)
        {
            var username = (objParametri.UtenteUsername != null) ? $"{objParametri.UtenteUsername}" : "NULL";
            var cauMoves = new List<string>
            {
                $"{CAU_MOV.CAU_CARICO_CAPO}",
                $"{CAU_MOV.CAU_SCARICO_CAPO}",
                $"{CAU_MOV.CAU_CARICO}",
                $"{CAU_MOV.CAU_SCARICO}"
            };

            var tipiDest = new List<int>
            {
                {TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA},
                {TIPO_DESTINAZIONE.STALLA}
            };

            sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
            sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
            sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
            sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
            sqlParams.TryAdd($"@{nameof(data)}Param", data.Date.ToString("yyyy-MM-dd"));
            sqlParams.TryAdd($"@{nameof(data)}Ora", data.ToString("yyyy-MM-dd hh:mm:ss"));
            sqlParams.TryAdd($"@{nameof(data)}Max", data.Date.ToString("yyyy-MM-dd"));
            sqlParams.TryAdd($"@{nameof(data)}Min", DateTime.Parse(COSTANTI_GENERALI.AGRODATAINIZIO).ToString("yyyy-MM-dd hh:mm:ss"));
            sqlParams.TryAdd($"@{nameof(idTestataTemp)}", idTestataTemp);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", username);
            sqlParams.TryAdd($"@lavCodMin", LAV_COD.MIN_OPERAZIONE_ZOO);
            sqlParams.TryAdd($"@lavCodMax", LAV_COD.MAX_OPERAZIONE_ZOO);
            sqlParams.TryAdd("@cauMovesIn", FormatClauseIn(cauMoves));
            sqlParams.TryAdd("@tipiDestIn", FormatClauseIn(tipiDest));
            sqlParams.TryAdd($"@{nameof(ELEM_COD.ZOO_CONSISTENZA)}", ELEM_COD.ZOO_CONSISTENZA);
            sqlParams.TryAdd($"@{nameof(ENTITA_VISIBILITA.CENTRO)}", ENTITA_VISIBILITA.CENTRO);


            StringBuilder stb = new StringBuilder();
            stb.AppendLine(" select a.Id_Agenda,  ");
            stb.AppendLine("    a.PIVA,  ");
            stb.AppendLine("    md.Cod_Progetto, ");
            stb.AppendLine("    md.Lotto, ");
            stb.AppendLine("    md.Udm_Cod, ");
            stb.AppendLine("    UnitaMisura.Udm_Des, ");
            stb.AppendLine("    UnitaMisura.Udm_Sim, ");
            stb.AppendLine("    mdes.Sa_Cod, ");
            stb.AppendLine("    mdes.Id_Destinazione, ");
            stb.AppendLine("    mdes.Tipo_Destinazione, ");
            stb.AppendLine("    mdes.Qta, ");
            stb.AppendLine("    md.Id_Mov, ");
            stb.AppendLine("    md.Id_Mov_Det, ");
            stb.AppendLine("    m.Cau_Mov, ");
            stb.AppendLine("    i.rag_soc, ");
            stb.AppendLine("    ic.val_cod as cuaa, ");
            stb.AppendLine("    Centri_Aziendali.sa_nome ");
            stb.AppendLine("    INTO #agn_cte ");
            stb.AppendLine("    from agenda a ");
            stb.AppendLine("    inner join imprese i on i.PIVA = a.PIVA ");
            stb.AppendLine("    inner join imprese_codici ic on i.piva = ic.piva and ic.id_cod = 1010 ");
            stb.AppendLine("    inner join Movimenti m on a.Id_Agenda = m.Id_Agenda  ");
            stb.AppendLine("    inner join Movimenti_dettagli md on md.Id_Agenda = m.Id_Agenda  AND md.Id_Mov = m.Id_Mov  ");
            stb.AppendLine("    inner join mov_destinazioni mdes on mdes.Id_Agenda = md.Id_Agenda and mdes.Id_Mov = md.Id_Mov and mdes.Id_Mov_Det = md.Id_Mov_Det ");
            stb.AppendLine("    inner join Centri_Aziendali ON mdes.Piva = Centri_Aziendali.Piva and mdes.Sa_Cod=Centri_Aziendali.sa_cod  ");
            stb.AppendLine("    INNER Join UnitaMisura ON UnitaMisura.Udm_Cod = md.Udm_Cod ");
            if (listCod_Animali != null && listCod_Animali.Count > 0)
                stb.AppendLine($"   INNER Join __Tmp_Agenda ON md.Cod_Progetto = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");

            if (Filtro_Visibilita_Utente)
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p ON a.Piva = p.piva AND p.Sa_Cod = mdes.Sa_Cod  AND p.entita_Cod = @{nameof(ENTITA_VISIBILITA.CENTRO)} AND p.Appezza = 0 AND p.Id_Reg = 0 AND p.Username = @{nameof(objParametri.UtenteUsername)} ");

            stb.AppendLine("    where  1 = 1 ");
            if (piva != "")
                stb.AppendLine($" And a.Piva = @{nameof(piva)} ");

            if (saCod != 0)
                stb.AppendLine($" And mdes.sa_cod = @{nameof(saCod)} ");

            if (codAnimale != 0)
                stb.AppendLine($" AND md.Cod_Progetto = @{nameof(codAnimale)} ");

            stb.AppendLine("    and a.lav_Cod >= @lavCodMin And a.lav_cod <= @lavCodMax ");
            stb.AppendLine("    And m.Cau_Mov In (@cauMovesIn)     ");
            stb.AppendLine($"   And m.Data_Movimento >=  CONVERT(DateTime,@{nameof(data)}Min,120)     "); //TODO: DEMETRIO possible deprecation

            if (filtraGiacenze1)
            {
                if (bAll)
                {
                    //TODO: DEMETRIO, with <= possible 23:59 hours lost 
                    stb.AppendLine($"   And m.Data_Movimento <= CONVERT(DateTime,@{nameof(data)}Max,120)     ");
                }
                else
                {
                    stb.AppendLine($"   And m.Data_Movimento <= CONVERT(DateTime,@{nameof(data)}Ora,120)     ");
                }

            }

            stb.AppendLine($"   And md.Elem_Cod = @{nameof(ELEM_COD.ZOO_CONSISTENZA)}  ");
            stb.AppendLine("    And md.Jolly_Int = 0  ");
            stb.AppendLine($"   And mdes.Tipo_Destinazione IN (@{nameof(tipiDest)}In)  ");
            return stb.ToString();
        }
        private string CreaGiac_TT(
            string piva,
            int saCod,
            int staNum,
            Dictionary<string, object> sqlParams,
            bool filtraGiacenze1 = true,
            bool mostraAnomalie = false)
        {
            var cauMovesOut = new List<string>
            {
                $"{CAU_MOV.CAU_SCARICO_CAPO}",
                $"{CAU_MOV.CAU_SCARICO}"
            };

            sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
            sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
            sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
            sqlParams.TryAdd("@cauMovesOut", FormatClauseIn(cauMovesOut));
            //sqlParams.TryAdd("@cauMovIn", FormatClauseIn(cauMoves));

            StringBuilder stb = new StringBuilder();

            stb.AppendLine("    SELECT   ");
            stb.AppendLine("  agn_cte.Piva, agn_cte.Rag_Soc As Impresa,  ");
            stb.AppendLine("  agn_cte.cuaa, Stalla.BDN_Codice_Azienda,  ");
            stb.AppendLine("  agn_cte.Cod_Progetto AS Cod_Animale,  ");
            stb.AppendLine("  agn_cte.Lotto,  ");
            stb.AppendLine("  agn_cte.Udm_Cod,  ");
            stb.AppendLine("  agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
            stb.AppendLine("  agn_cte.Sa_Cod,  ");
            stb.AppendLine("  agn_cte.sa_nome,  ");
            stb.AppendLine("  agn_cte.Id_Destinazione,  ");
            stb.AppendLine("  agn_cte.Tipo_Destinazione,  ");
            stb.AppendLine("  COALESCE(Fabbricati.Fabbricato_Des, '') As STA_DES,  ");
            stb.AppendLine("    COALESCE(Fabbricati.Fabbricato_Cod, 0) As STA_NUM,  ");
            stb.AppendLine("  COALESCE(Stalla_Raggruppamenti.Raggruppamento_Des, '') as Raggruppamento_Des,  ");
            stb.AppendLine("  COALESCE(Stalla_Raggruppamenti.Raggruppamento_Cod, 0) as Raggruppamento_Cod,  ");
            stb.AppendLine($"  Convert(INTEGER, SUM( Case When agn_cte.CAU_MOV In (@{nameof(cauMovesOut)})              ");
            stb.AppendLine("          THEN -(agn_cte.qta)             ");
            stb.AppendLine("          Else agn_cte.qta             ");
            stb.AppendLine("          End)) As Giacenza   ");
            stb.AppendLine(" INTO #giac_cte ");
            stb.AppendLine(" From #agn_cte agn_cte  ");
            stb.AppendLine(" INNER Join Stalla_Raggruppamenti ON agn_cte.Sa_Cod = Stalla_Raggruppamenti.sa_cod  ");
            stb.AppendLine("                                AND agn_cte.Id_Destinazione = Stalla_Raggruppamenti.Raggruppamento_Cod  ");
            stb.AppendLine("                                AND agn_cte.Tipo_Destinazione = 21  ");
            stb.AppendLine(" INNER JOIN Fabbricati ON Stalla_Raggruppamenti.PIVA = Fabbricati.Piva AND Stalla_Raggruppamenti.sa_cod = Fabbricati.SA_COD AND Stalla_Raggruppamenti.STA_NUM = Fabbricati.Fabbricato_Cod ");
            stb.AppendLine(" INNER JOIN Stalla ON Stalla.PIVA = Fabbricati.Piva AND Stalla.sa_cod = Fabbricati.SA_COD AND Stalla.STA_NUM = Fabbricati.Fabbricato_Cod  ");
            if (mostraAnomalie)
                stb.AppendLine(" LEFT Join Zoo_AnimalixAnomalie ON Zoo_AnimalixAnomalie.Cod_Animale = agn_cte.Cod_Progetto  ");
            stb.AppendLine("  where 1=1  ");
            if (saCod != 0)
                stb.AppendLine($" And agn_cte.sa_cod = @{nameof(saCod)}      ");

            if (staNum != 0)
                stb.AppendLine($" AND Fabbricati.Fabbricato_Cod = @{nameof(staNum)}  ");
            stb.AppendLine(" GROUP BY agn_cte.Piva, agn_cte.Rag_Soc, agn_cte.cuaa, Stalla.BDN_Codice_Azienda, ");
            stb.AppendLine(" agn_cte.Cod_Progetto,  ");
            stb.AppendLine(" agn_cte.Lotto,  ");
            stb.AppendLine(" agn_cte.Udm_Cod, ");
            stb.AppendLine(" Fabbricati.Fabbricato_Des, ");
            stb.AppendLine(" Fabbricati.Fabbricato_Cod, ");
            stb.AppendLine(" agn_cte.Sa_Cod,  ");
            stb.AppendLine(" agn_cte.sa_nome, ");
            stb.AppendLine(" agn_cte.Id_Destinazione,  ");
            stb.AppendLine(" agn_cte.Tipo_Destinazione,  ");
            stb.AppendLine(" agn_cte.Udm_Des, agn_cte.Udm_Sim,  ");
            stb.AppendLine(" Stalla_Raggruppamenti.Raggruppamento_Des,  ");
            stb.AppendLine(" Stalla_Raggruppamenti.Raggruppamento_Cod  ");
            if (filtraGiacenze1)
                stb.AppendLine($" HAVING Convert(INTEGER, SUM(Case When agn_cte.Cau_Mov In (@{nameof(cauMovesOut)}) THEN -(agn_cte.qta) ELSE agn_cte.qta END)) <> 0   ");

            return stb.ToString();
        }

        private string CreaZooMatricoleList_TT()
        {
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("    SELECT Zoo_Animali.Matricola  ");
            stb.AppendLine("    INTO #ZooMatricoleList  ");
            stb.AppendLine("    FROM #giac_cte giac_cte   ");
            stb.AppendLine("    INNER Join Zoo_Animali ON Zoo_Animali.Cod_Progetto = giac_cte.Cod_Animale    ");
            stb.AppendLine("    WHERE 1=1   ");
            return stb.ToString();
        }

        private string CreaDataPrimoCaricamento_TT(string piva, Dictionary<string, object> sqlParams)
        {
            sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
            sqlParams.TryAdd($"@{nameof(ELEM_COD.ZOO_CONSISTENZA)}", ELEM_COD.ZOO_CONSISTENZA);

            // DataPrimoCaricamento
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("  SELECT ZooMatricoleList.Matricola, MIN(Movimenti.Data_Movimento) as Data_Movimento  ");
            stb.AppendLine("  INTO #DataPrimoCaricamento  ");
            stb.AppendLine("  FROM Agenda  ");
            stb.AppendLine("  JOIN Movimenti ON Agenda.ID_Agenda = Movimenti.ID_Agenda  ");
            stb.AppendLine($"  JOIN Movimenti_Dettagli ON Movimenti.ID_Agenda = Movimenti_Dettagli.ID_Agenda AND Movimenti.Id_Mov = Movimenti_Dettagli.Id_Mov AND Movimenti_dettagli.Elem_Cod = @{nameof(ELEM_COD.ZOO_CONSISTENZA)}  ");
            stb.AppendLine("  JOIN Zoo_Animali ON Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto  ");
            stb.AppendLine("  JOIN #ZooMatricoleList ZooMatricoleList  ON Zoo_Animali.Matricola = ZooMatricoleList.Matricola  ");
            stb.AppendLine("  WHERE 1 = 1  ");
            if (piva != "")
                stb.AppendLine($" And Agenda.Piva = @{nameof(piva)}      ");
            stb.AppendLine("  GROUP BY ZooMatricoleList.Matricola  ");
            return stb.ToString();
        }

        private string CreaPesate_TT(DateTime Data, Dictionary<string, object> sqlParams, bool bAll = false, bool leggiUltimaPesata = false)
        {
            var stb = new StringBuilder();
            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)}", CAU_MOV.CAU_PESATURA_ANIMALI);
            sqlParams.TryAdd($"@{nameof(Data)}PesataTime", Data.AddMinutes(1d));
            sqlParams.TryAdd($"@{nameof(Data)}PesataDate", Data.Date);
            stb.AppendLine("    SELECT Movimenti_dettagli.Cod_Progetto, MAX(Data_Movimento) as Data_Peso_Max, MIN(Data_Movimento) as Data_Peso_Min  ");
            stb.AppendLine("    INTO #pesate_cte");
            stb.AppendLine("    FROM Movimenti (NOLOCK)  ");
            stb.AppendLine("    JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.Id_Agenda = Movimenti_dettagli.Id_Agenda AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov ");
            stb.AppendLine("    JOIN #giac_cte giac_cte ON Movimenti_dettagli.Cod_Progetto = giac_cte.Cod_Animale ");
            stb.AppendLine($"   WHERE Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)} "); // " AND Agenda.Lav_Cod = " & LAVCOD_PESATURA_ANIMALI & " ")
            if (!leggiUltimaPesata)
            {
                stb.AppendLine($"   And CAST(Data_Movimento as Date) <=  Convert(Date, @{nameof(Data)}PesataTime, 120)     ");
            }
            stb.AppendLine("    GROUP BY Movimenti_dettagli.Cod_Progetto ");
            return stb.ToString();
        }


        private string CreaMaxPesate_TT(Dictionary<string, object> sqlParams)
        {
            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)}", CAU_MOV.CAU_PESATURA_ANIMALI);
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("    SELECT Movimenti_dettagli.Qta, Movimenti_dettagli.Cod_Progetto, Movimenti.Data_Movimento as Data_Peso ");
            stb.AppendLine("    INTO #max_pesate_ct  ");
            stb.AppendLine("    FROM Movimenti (NOLOCK)  ");
            stb.AppendLine("    JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.Id_Agenda = Movimenti_dettagli.Id_Agenda AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov   ");
            stb.AppendLine("    JOIN #pesate_cte pesate_cte ON pesate_cte.Cod_Progetto = Movimenti_dettagli.Cod_Progetto AND Movimenti.Data_Movimento = pesate_cte.Data_Peso_Max   ");
            stb.AppendLine($"   WHERE Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)} ");
            return stb.ToString();
        }

        private string CreaMinPesate_TT(Dictionary<string, object> sqlParams)
        {
            sqlParams.TryAdd($"@{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)}", CAU_MOV.CAU_PESATURA_ANIMALI);
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("    SELECT Movimenti_dettagli.Qta, Movimenti_dettagli.Cod_Progetto, Movimenti.Data_Movimento as Data_Peso ");
            stb.AppendLine("    INTO #min_pesate_ct  ");
            stb.AppendLine("    FROM Movimenti (NOLOCK)  ");
            stb.AppendLine("    JOIN Movimenti_dettagli (NOLOCK)  ON Movimenti.Id_Agenda = Movimenti_dettagli.Id_Agenda AND Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov   ");
            stb.AppendLine("    JOIN #pesate_cte pesate_cte ON pesate_cte.Cod_Progetto = Movimenti_dettagli.Cod_Progetto AND Movimenti.Data_Movimento = pesate_cte.Data_Peso_Min   ");
            stb.AppendLine($"   WHERE Movimenti.Cau_Mov = @{nameof(CAU_MOV.CAU_PESATURA_ANIMALI)} ");
            return stb.ToString();
        }

        private string CreaAnomalie_TT()
        {
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("    SELECT giac_cte.cod_animale, COALESCE(STRING_AGG(Zoo_AnimalixAnomalie.Codice, ','), '') as Anomalie, COALESCE(STRING_AGG(Zoo_Animali_Anomalie.descrizione, ','), '') as Anomalie_Str   ");
            stb.AppendLine("    INTO #anomalie_ct  ");
            stb.AppendLine("    FROM Zoo_AnimalixAnomalie (NOLOCK)  ");
            stb.AppendLine("    JOIN #giac_cte giac_cte ON Zoo_AnimalixAnomalie.Cod_Animale = giac_cte.Cod_Animale   ");
            stb.AppendLine("    JOIN Zoo_Animali_Anomalie (NOLOCK)  ON Zoo_AnimalixAnomalie.Codice = Zoo_Animali_Anomalie.Codice   ");
            stb.AppendLine("    GROUP BY giac_cte.cod_animale   ");
            return stb.ToString();
        }

        private string crea_agn_cte(
            string piva,
            int saCod,
            int staNum,
            int raggruppamentoCod,
            int codAnimale,
            DateTime data,
            int giorniSenzaTrattamenti,
            bool filtroVisibilitaUtente,
            List<int> listCodAnimali,
            int idTestataTemp,
            Dictionary<string, object> sqlParams,
            ref AgronicaCoreParametri objParametri)
        {
            StringBuilder stb = new StringBuilder();

            var username = (objParametri.UtenteUsername != null) ? $"'{objParametri.UtenteUsername}'" : "NULL";
            var cauMoves = new List<string>
            {
                $"'{CAU_MOV.CAU_CARICO_CAPO}'",
                $"'{CAU_MOV.CAU_SCARICO_CAPO}'",
                $"'{CAU_MOV.CAU_CARICO}'",
                $"'{CAU_MOV.CAU_SCARICO}'"
            };

            var tipiDest = new List<int>
            {
                {TIPO_DESTINAZIONE.TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA},
                {TIPO_DESTINAZIONE.STALLA}
            };

            sqlParams.TryAdd($"@{nameof(piva)}", (piva == null) ? "NULL" : $"{piva}");
            sqlParams.TryAdd($"@{nameof(saCod)}", saCod);
            sqlParams.TryAdd($"@{nameof(staNum)}", staNum);
            sqlParams.TryAdd($"@{nameof(codAnimale)}", codAnimale);
            sqlParams.TryAdd($"@{nameof(data)}Param", data.Date);
            sqlParams.TryAdd($"@{nameof(data)}Ora", data);
            sqlParams.TryAdd($"@{nameof(data)}Min", DateTime.Parse(COSTANTI_GENERALI.AGRODATAINIZIO).Date);
            sqlParams.TryAdd($"@{nameof(data)}Max", data);
            sqlParams.TryAdd($"@{nameof(idTestataTemp)}", idTestataTemp);
            sqlParams.TryAdd($"@{nameof(objParametri.UtenteUsername)}", username);
            sqlParams.TryAdd($"@lavCodMin", LAV_COD.MIN_OPERAZIONE_ZOO);
            sqlParams.TryAdd($"@lavCodMax", LAV_COD.MAX_OPERAZIONE_ZOO);
            sqlParams.TryAdd("@cauMovesIn", FormatClauseIn(cauMoves));
            sqlParams.TryAdd("@tipiDestIn", FormatClauseIn(tipiDest));
            //sqlParams.TryAdd("@cauMovIn", string.Join(",", cauMoves));
            //sqlParams.TryAdd("@tipiDest", string.Join(",", tipiDest));
            sqlParams.TryAdd($"@{nameof(ELEM_COD.ZOO_CONSISTENZA)}", ELEM_COD.ZOO_CONSISTENZA);
            sqlParams.TryAdd($"@{nameof(ENTITA_VISIBILITA.CENTRO)}", ENTITA_VISIBILITA.CENTRO);

            stb.AppendLine("  select a.Id_Agenda,  ");
            stb.AppendLine("    a.PIVA,  ");
            stb.AppendLine("    md.Cod_Progetto, ");
            stb.AppendLine("    md.Lotto, ");
            stb.AppendLine("    md.Udm_Cod, ");
            stb.AppendLine("    UnitaMisura.Udm_Des, ");
            stb.AppendLine("    UnitaMisura.Udm_Sim, ");
            stb.AppendLine("    mdes.Sa_Cod, ");
            stb.AppendLine("    mdes.Id_Destinazione, ");
            stb.AppendLine("    mdes.Tipo_Destinazione, ");
            stb.AppendLine("    mdes.Qta, ");
            stb.AppendLine("    md.Id_Mov, ");
            stb.AppendLine("    md.Id_Mov_Det, ");
            stb.AppendLine("    m.Cau_Mov, ");
            stb.AppendLine("    i.rag_soc, ");
            stb.AppendLine("    ic.val_cod as cuaa, ");
            stb.AppendLine("    Centri_Aziendali.sa_nome ");
            stb.AppendLine("    INTO #agn_cte  ");
            stb.AppendLine("    from agenda a  ");
            stb.AppendLine("    inner join imprese i on i.PIVA = a.PIVA ");
            stb.AppendLine("    inner join imprese_codici ic on i.piva = ic.piva and ic.id_cod = 1010 ");
            stb.AppendLine("    inner join Movimenti m on   a.Id_Agenda = m.Id_Agenda  ");
            stb.AppendLine("    inner join Movimenti_dettagli md on md.Id_Agenda = m.Id_Agenda  AND md.Id_Mov = m.Id_Mov  ");
            stb.AppendLine("    inner join mov_destinazioni mdes on mdes.Id_Agenda = md.Id_Agenda and mdes.Id_Mov = md.Id_Mov and mdes.Id_Mov_Det = md.Id_Mov_Det ");
            stb.AppendLine("    inner join Centri_Aziendali ON mdes.Piva = Centri_Aziendali.Piva and mdes.Sa_Cod=Centri_Aziendali.sa_cod  ");
            stb.AppendLine("    INNER Join UnitaMisura ON UnitaMisura.Udm_Cod = md.Udm_Cod ");
            if (listCodAnimali != null && listCodAnimali.Count > 0)
                stb.AppendLine($"   INNER Join __Tmp_Agenda ON md.Cod_Progetto = __Tmp_Agenda.id_agenda AND __Tmp_Agenda.Piva = '' AND __Tmp_Agenda.IDTestataTemp = @{nameof(idTestataTemp)} ");

            if (piva == "" && filtroVisibilitaUtente)
                stb.AppendLine($" inner join utenti_Visibilita_Appoggio p ON a.Piva = p.piva AND p.Entita_Cod = 1 AND p.Username = @{nameof(objParametri.UtenteUsername)} ");
            stb.AppendLine("    where  1 = 1 ");

            if (piva != "")
            {
                stb.AppendLine($" And a.Piva = @{nameof(piva)}      ");
            }

            if (saCod != 0)
                stb.AppendLine($" And mdes.sa_cod = @{nameof(saCod)}      ");

            if (codAnimale != 0)
                stb.AppendLine($" AND md.Cod_Progetto = @{nameof(codAnimale)}  ");
            stb.AppendLine("    and a.lav_Cod >= lavCodMin And a.lav_cod <= @lavCodMax ");
            stb.AppendLine("    And m.Cau_Mov In (@cauMovesIn)     ");
            stb.AppendLine($"   And m.Data_Movimento >=  @{nameof(data)}Min     ");
            stb.AppendLine($"   And m.Data_Movimento <=  @{nameof(data)}Max  ");
            stb.AppendLine($"   And md.Elem_Cod = @{nameof(ELEM_COD.ZOO_CONSISTENZA)}  ");
            stb.AppendLine("    And md.Jolly_Int = 0  ");
            stb.AppendLine("    And mdes.Tipo_Destinazione IN (@tipiDestIn)  ");

            return stb.ToString();
        }
    }
}
