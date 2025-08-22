
/*      AGGANCIO 133 APERTO IL 17/03/2025        */


:setvar MetaschemaDB GIAS_MetaSchema_16
:setvar PUADB GIAS_PianoConcimazione_PUA

:setvar ServerDB GIAS_Server
:setvar UtentiDB Utenti

:setvar VersioneAggancio 133

 
/* #################################################################################### */
/*                                                                                      */
/* 	A G G I O R N A R E   I L   T R A C C I A T O   D E L L A   V E R S I O N E         */
/*          Leggere l'ultima versione prima di rilasciare                               */
/*                                                                                      */
/* #################################################################################### */
/*                                                                                      */
/*                                                                                      */
/*                                                                                      */
/*	                          AGGANCIO a    $(MetaschemaDB)                             */
/*                                                                                      */
/*                                                                                      */
/*                                                                                      

	nuove viste su GIAS_Server:
	- 


	nuove viste su Utenti:
	- 
	
                                                                                        */
/*                                                                                      */
/* ###################################################################################  */
/* ##      CTRL+F FOR:                                                               ## */
/* ##      -- > --LAST ServerDB                                                      ## */




/* ########################################################################################## */
/* #############                  Database GIAS SERVER                      ################# */
/* ########################################################################################## */


USE $(ServerDB)
go

PRINT  CHAR(10) + ' ++ DB = $(ServerDB)'
GO

if not exists (select 1 from Versione_Database where versione = 'Aggancio $(MetaschemaDB) v.$(VersioneAggancio)')
	INSERT INTO Versione_Database  (Versione, Username_Creazione, Username_Modifica)
		    VALUES     ('Aggancio $(MetaschemaDB) v.$(VersioneAggancio)', '$(MetaschemaDB) v.$(VersioneAggancio)', '$(MetaschemaDB) v.$(VersioneAggancio)')
else
	UPDATE Versione_Database SET data_modifica = getDate() WHERE versione = 'Aggancio $(MetaschemaDB) v.$(VersioneAggancio)'
GO



-- ==============================================================================
-- =====
-- =====  Cancellazione delle VISTE esistenti (x Pulizia - non verranno ri-create)
-- =====
-- ==============================================================================
if exists (select 1 from sys.views where name = 'AAA_SpecieVegetali_CRPV')
	DROP VIEW AAA_SpecieVegetali_CRPV
GO

if exists (select 1 from sys.views where name = 'AAA_Disciplinari_CRPV')
	DROP VIEW AAA_Disciplinari_CRPV
GO

if exists (select 1 from sys.views where name = 'AAA_Trascodifica_SpecieVegetali')
	DROP VIEW AAA_Trascodifica_SpecieVegetali
GO

if exists (select 1 from sys.views where name = 'SpecieVegetali_Decoder_OOPP')
	DROP VIEW SpecieVegetali_Decoder_OOPP
GO

if exists (select 1 from sys.views where name = 'SpecieVegetali_OOPP')
	DROP VIEW SpecieVegetali_OOPP
GO

if exists (select 1 from sys.views where name = 'Generazioni_Anagrafe_Moduli')
	DROP VIEW Generazioni_Anagrafe_Moduli
GO

if exists (select 1 from sys.views where name = 'Generazioni_Anagrafe')
	DROP VIEW Generazioni_Anagrafe
GO

if exists (select 1 from sys.views where name = 'Linee_Produzioni_DPI')
	DROP VIEW Linee_Produzioni_DPI
GO

if exists (select 1 from sys.views where name = 'Linee_Produzioni_DPI_Parametri')
	DROP VIEW Linee_Produzioni_DPI_Parametri
GO

if exists (select 1 from sys.views where name = 'Metaschema')
	DROP VIEW Metaschema
GO

if exists (select 1 from sys.views where name = 'Metaschema_Parametri')
	DROP VIEW Metaschema_Parametri
GO

if exists (select 1 from sys.views where name = 'OriginiDati')
	DROP VIEW OriginiDati
GO

if exists (select 1 from sys.views where name = 'TipologieDitta')
	DROP VIEW TipologieDitta
GO

if exists (select 1 from sys.views where name = 'FormulatiXAllegati')
	DROP VIEW FormulatiXAllegati
GO

if exists (select 1 from sys.views where name = 'FormulatiXAllegatiNormative')
	DROP VIEW FormulatiXAllegatiNormative
GO

if exists (select 1 from sys.views where name = 'FormulatiXClassiTossicologiche')
	DROP VIEW FormulatiXClassiTossicologiche
GO

if exists (select 1 from sys.views where name = 'FormulatiXDitte')
	DROP VIEW FormulatiXDitte
GO

if exists (select 1 from sys.views where name = 'residuiOrganici')
	DROP VIEW residuiOrganici
GO

if exists (select 1 from sys.views where name = 'RegolamentiRMA')
	DROP VIEW RegolamentiRMA
GO

if exists (select 1 from sys.views where name = 'DerrateRMA')
	DROP VIEW DerrateRMA
GO

if exists (select 1 from sys.views where name = 'PrincipiAttiviRMA')
	DROP VIEW PrincipiAttiviRMA
GO

if exists (select 1 from sys.views where name = 'PrincipiAttivixPrincipiAttiviRMA')
	DROP VIEW PrincipiAttivixPrincipiAttiviRMA
GO

if exists (select 1 from sys.views where name = 'DerrateRMAxPrincipiAttiviRMA')
	DROP VIEW DerrateRMAxPrincipiAttiviRMA
GO

if exists (select 1 from sys.views where name = 'DerrateCodifica')
	DROP VIEW DerrateCodifica
GO

if exists (select 1 from sys.views where name = 'PrincipiAttiviXDerrateXResidui')
	DROP VIEW PrincipiAttiviXDerrateXResidui
GO

if exists (select 1 from sys.views where name = 'Audit_Codici_Segnalazioni')
	DROP VIEW Audit_Codici_Segnalazioni
GO

if exists (select 1 from sys.views where name = 'Audit_Stampe')
	DROP VIEW Audit_Stampe
GO

if exists (select 1 from sys.views where name = 'StadiXSpecieVegetali')
	DROP VIEW StadiXSpecieVegetali
GO

if exists (select 1 from sys.views where name = 'SpecieConcimazione')
	DROP VIEW SpecieConcimazione
GO

if exists (select 1 from sys.views where name = 'EfficienzaLiquami')
	DROP VIEW EfficienzaLiquami
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MacrostadiCrescita')
	DROP VIEW MacrostadiCrescita
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MacrostadiXStadiCrescita')
	DROP VIEW MacrostadiXStadiCrescita
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MisuraXIndiciMaturita_Anagrafiche')
	DROP VIEW MisuraXIndiciMaturita_Anagrafiche
GO

IF EXISTS (select 1 from sys.views where name = 'UMA_Macrousi')
	DROP VIEW UMA_Macrousi
GO

IF EXISTS (select 1 from sys.views where name = 'UMA_Lavorazioni')
	DROP VIEW UMA_Lavorazioni
GO

if exists (select 1 from sys.views where name = 'UMA_Allevamenti')
	DROP VIEW UMA_Allevamenti
GO

-- ==============================================================================
-- =====
-- =====  Cancellazione delle VISTE esistenti e ri-creazione
-- =====
-- ==============================================================================

if exists (select 1 from sys.views where name = 'WTransizioniDiStatoConfigurazione')
	DROP VIEW WTransizioniDiStatoConfigurazione
GO
CREATE VIEW WTransizioniDiStatoConfigurazione AS select * from $(MetaschemaDB).dbo.WTransizioniDiStatoConfigurazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'WWorkFlow')
	DROP VIEW WWorkFlow
GO
CREATE VIEW WWorkFlow AS select * from $(MetaschemaDB).dbo.WWorkFlow WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'WAnagraficaStati')
	DROP VIEW WAnagraficaStati
GO
CREATE VIEW WAnagraficaStati AS	select * from $(MetaschemaDB).dbo.WAnagraficaStati WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T001_TabellaCodiceDelleLingue')
	DROP VIEW ACCDAA_ANAG_T001_TabellaCodiceDelleLingue
GO
CREATE VIEW ACCDAA_ANAG_T001_TabellaCodiceDelleLingue AS select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T001_TabellaCodiceDelleLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T002_UnitaDiMisuraDellaDurataDelTrasporto')
	DROP VIEW ACCDAA_ANAG_T002_UnitaDiMisuraDellaDurataDelTrasporto
GO
CREATE VIEW ACCDAA_ANAG_T002_UnitaDiMisuraDellaDurataDelTrasporto AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T002_UnitaDiMisuraDellaDurataDelTrasporto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T011_TabellaCodiciMotiviCancellazione')
	DROP VIEW ACCDAA_ANAG_T011_TabellaCodiciMotiviCancellazione
GO
CREATE VIEW ACCDAA_ANAG_T011_TabellaCodiciMotiviCancellazione AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T011_TabellaCodiciMotiviCancellazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T003_TipologiaDiGaranzia')
	DROP VIEW ACCDAA_ANAG_T003_TipologiaDiGaranzia
GO
CREATE VIEW ACCDAA_ANAG_T003_TipologiaDiGaranzia AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T003_TipologiaDiGaranzia WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T013_TabellaCodiciUnitaDiMisura')
	DROP VIEW ACCDAA_ANAG_T013_TabellaCodiciUnitaDiMisura
GO
CREATE VIEW ACCDAA_ANAG_T013_TabellaCodiciUnitaDiMisura AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T013_TabellaCodiciUnitaDiMisura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T014_TabellaTipiMessaggio')
	DROP VIEW ACCDAA_ANAG_T014_TabellaTipiMessaggio
GO
CREATE VIEW ACCDAA_ANAG_T014_TabellaTipiMessaggio AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T014_TabellaTipiMessaggio WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T016_TabellaCodiciOrganizzazioneTrasporto')
	DROP VIEW ACCDAA_ANAG_T016_TabellaCodiciOrganizzazioneTrasporto
GO
CREATE VIEW ACCDAA_ANAG_T016_TabellaCodiciOrganizzazioneTrasporto AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T016_TabellaCodiciOrganizzazioneTrasporto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T019_TabellaCodiciZoneColtivazioneVino')
	DROP VIEW ACCDAA_ANAG_T019_TabellaCodiciZoneColtivazioneVino
GO
CREATE VIEW ACCDAA_ANAG_T019_TabellaCodiciZoneColtivazioneVino AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T019_TabellaCodiciZoneColtivazioneVino WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T020_TabellaCodiciTrattamentoVino')
	DROP VIEW ACCDAA_ANAG_T020_TabellaCodiciTrattamentoVino
GO
CREATE VIEW ACCDAA_ANAG_T020_TabellaCodiciTrattamentoVino AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T020_TabellaCodiciTrattamentoVino WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T021_CauMovCarico')
	DROP VIEW ACCDAA_ANAG_T021_CauMovCarico
GO
CREATE VIEW ACCDAA_ANAG_T021_CauMovCarico AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T021_CauMovCarico WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T021_CauMovScarico')
	DROP VIEW ACCDAA_ANAG_T021_CauMovScarico
GO
CREATE VIEW ACCDAA_ANAG_T021_CauMovScarico AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T021_CauMovScarico WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T023_TabellaEsitoGlobaleDellaRicezione')
	DROP VIEW ACCDAA_ANAG_T023_TabellaEsitoGlobaleDellaRicezione
GO
CREATE VIEW ACCDAA_ANAG_T023_TabellaEsitoGlobaleDellaRicezione AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T023_TabellaEsitoGlobaleDellaRicezione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T024_TabellaIndicatoreDiCaloOEccedenza')
	DROP VIEW ACCDAA_ANAG_T024_TabellaIndicatoreDiCaloOEccedenza
GO
CREATE VIEW ACCDAA_ANAG_T024_TabellaIndicatoreDiCaloOEccedenza AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T024_TabellaIndicatoreDiCaloOEccedenza WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T025_TabellaCodiciMotiviDiInsoddisfazione')
	DROP VIEW ACCDAA_ANAG_T025_TabellaCodiciMotiviDiInsoddisfazione
GO
CREATE VIEW ACCDAA_ANAG_T025_TabellaCodiciMotiviDiInsoddisfazione AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T025_TabellaCodiciMotiviDiInsoddisfazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T026_TabelllaFlagDiRigetto')
	DROP VIEW ACCDAA_ANAG_T026_TabelllaFlagDiRigetto
GO
CREATE VIEW ACCDAA_ANAG_T026_TabelllaFlagDiRigetto AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T026_TabelllaFlagDiRigetto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T027_TabellaMotivazioniDiRigettoOSegnalazione')
	DROP VIEW ACCDAA_ANAG_T027_TabellaMotivazioniDiRigettoOSegnalazione
GO
CREATE VIEW ACCDAA_ANAG_T027_TabellaMotivazioniDiRigettoOSegnalazione AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_T027_TabellaMotivazioniDiRigettoOSegnalazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA01_TipiRichiesta')
	DROP VIEW ACCDAA_ANAG_TA01_TipiRichiesta
GO
CREATE VIEW ACCDAA_ANAG_TA01_TipiRichiesta AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TA01_TipiRichiesta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA05_TipiDocumento')
	DROP VIEW ACCDAA_ANAG_TA05_TipiDocumento
GO
CREATE VIEW ACCDAA_ANAG_TA05_TipiDocumento AS select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TA05_TipiDocumento WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA06_Paesi')
	DROP VIEW ACCDAA_ANAG_TA06_Paesi
GO
CREATE VIEW ACCDAA_ANAG_TA06_Paesi AS select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TA06_Paesi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA07_TipiMovimentazione')
	DROP VIEW ACCDAA_ANAG_TA07_TipiMovimentazione
GO
CREATE VIEW ACCDAA_ANAG_TA07_TipiMovimentazione AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TA07_TipiMovimentazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TabellaErrori')
	DROP VIEW ACCDAA_ANAG_TabellaErrori
GO
CREATE VIEW ACCDAA_ANAG_TabellaErrori AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TabellaErrori WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TabellaErrori_Agronica')
	DROP VIEW ACCDAA_ANAG_TabellaErrori_Agronica
GO
CREATE VIEW ACCDAA_ANAG_TabellaErrori_Agronica AS
	select * from $(MetaschemaDB).dbo.ACCDAA_ANAG_TabellaErrori_Agronica WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'ModelliPrevisionaliXpiva_superUser_OperazioniAutorizzate')
	DROP VIEW ModelliPrevisionaliXpiva_superUser_OperazioniAutorizzate
GO
CREATE VIEW ModelliPrevisionaliXpiva_superUser_OperazioniAutorizzate AS
	select * from $(MetaschemaDB).dbo.ModelliPrevisionaliXpiva_superUser_OperazioniAutorizzate WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModelliPrevisionaliXpiva_OperazioniAutorizzate')
	DROP VIEW ModelliPrevisionaliXpiva_OperazioniAutorizzate
GO
CREATE VIEW ModelliPrevisionaliXpiva_OperazioniAutorizzate AS
	select * from $(MetaschemaDB).dbo.ModelliPrevisionaliXpiva_OperazioniAutorizzate WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'CBI_Causali')
	DROP VIEW CBI_Causali
GO
CREATE VIEW CBI_Causali AS select * from $(MetaschemaDB).dbo.CBI_Causali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'UnitaMisura_Conversione')
	DROP VIEW UnitaMisura_Conversione
GO
CREATE VIEW UnitaMisura_Conversione as SELECT * FROM $(MetaschemaDB).dbo.UnitaMisura_Conversione WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Tipo_Entita')
	DROP VIEW Tipo_Entita
GO
CREATE VIEW  Tipo_Entita  AS SELECT * FROM $(MetaschemaDB).dbo.Tipo_Entita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Tipo_Entita_Chiavi')
	DROP VIEW Tipo_Entita_Chiavi
GO
CREATE VIEW  Tipo_Entita_Chiavi  AS SELECT * FROM $(MetaschemaDB).dbo.Tipo_Entita_Chiavi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Apporti')
	DROP VIEW Apporti
GO
CREATE VIEW  Apporti  AS SELECT * FROM $(MetaschemaDB).dbo.Apporti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ApportiAmmendante')
	DROP VIEW ApportiAmmendante
GO
CREATE VIEW  ApportiAmmendante  AS SELECT * FROM $(MetaschemaDB).dbo.ApportiAmmendante WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ApportiAzotati')
	DROP VIEW ApportiAzotati
GO
CREATE VIEW  ApportiAzotati  AS SELECT * FROM $(MetaschemaDB).dbo.ApportiAzotati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Analisi_Parametri')
	DROP VIEW Analisi_Parametri
GO
CREATE VIEW  Analisi_Parametri AS SELECT * FROM $(MetaschemaDB).dbo.Analisi_Parametri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Asportazioni')
	DROP VIEW Asportazioni
GO
CREATE VIEW  Asportazioni  AS SELECT * FROM $(MetaschemaDB).dbo.Asportazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'AsportiArboree')
	DROP VIEW AsportiArboree
GO
CREATE VIEW  AsportiArboree  AS SELECT * FROM $(MetaschemaDB).dbo.AsportiArboree WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Ausiliari')
	DROP VIEW Ausiliari
GO
CREATE VIEW  Ausiliari  AS SELECT * FROM $(MetaschemaDB).dbo.Ausiliari WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Avversita')
	DROP VIEW Avversita
GO
CREATE VIEW  Avversita  AS SELECT * FROM $(MetaschemaDB).dbo.Avversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'AvversitaxGruppoAvversita')
	DROP VIEW AvversitaxGruppoAvversita
GO
CREATE VIEW  AvversitaxGruppoAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.AvversitaxGruppoAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Avvertenze')
	DROP VIEW Avvertenze
GO
CREATE VIEW  Avvertenze  AS SELECT * FROM $(MetaschemaDB).dbo.Avvertenze WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'BIO_Dati_MetodoProduzione')
	DROP VIEW BIO_Dati_MetodoProduzione
GO
CREATE VIEW  BIO_Dati_MetodoProduzione  AS SELECT * FROM $(MetaschemaDB).dbo.BIO_Dati_MetodoProduzione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BIO_Dati_OrganismiControllo')
	DROP VIEW BIO_Dati_OrganismiControllo
GO
CREATE VIEW  BIO_Dati_OrganismiControllo  AS SELECT * FROM $(MetaschemaDB).dbo.BIO_Dati_OrganismiControllo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BIO_Dati_OrientamentoProduttivo')
	DROP VIEW BIO_Dati_OrientamentoProduttivo
GO
CREATE VIEW  BIO_Dati_OrientamentoProduttivo  AS SELECT * FROM $(MetaschemaDB).dbo.BIO_Dati_OrientamentoProduttivo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BIO_Dati_TipologiaColtura')
	DROP VIEW BIO_Dati_TipologiaColtura
GO
CREATE VIEW  BIO_Dati_TipologiaColtura  AS SELECT * FROM $(MetaschemaDB).dbo.BIO_Dati_TipologiaColtura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BIO_Dati_TitoloPossesso')
	DROP VIEW BIO_Dati_TitoloPossesso
GO
CREATE VIEW  BIO_Dati_TitoloPossesso  AS SELECT * FROM $(MetaschemaDB).dbo.BIO_Dati_TitoloPossesso WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'CalibriFrutti')
	DROP VIEW CalibriFrutti
GO
CREATE VIEW  CalibriFrutti  AS SELECT * FROM $(MetaschemaDB).dbo.CalibriFrutti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CalibriFruttixSpecieVegetali')
	DROP VIEW CalibriFruttixSpecieVegetali
GO
CREATE VIEW  CalibriFruttixSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.CalibriFruttixSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Carburanti')
	DROP VIEW Carburanti
GO
CREATE VIEW  Carburanti  AS SELECT * FROM $(MetaschemaDB).dbo.Carburanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Categorie')
	DROP VIEW Categorie
GO
CREATE VIEW  Categorie  AS SELECT * FROM $(MetaschemaDB).dbo.Categorie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CategorieDocumenti')
	DROP VIEW CategorieDocumenti
GO
CREATE VIEW  CategorieDocumenti  AS SELECT * FROM $(MetaschemaDB).dbo.CategorieDocumenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CategorieMagazzino')
	DROP VIEW CategorieMagazzino
GO
CREATE VIEW  CategorieMagazzino  AS SELECT * FROM $(MetaschemaDB).dbo.CategorieMagazzino WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CategorieXUnitaMisura')
	DROP VIEW CategorieXUnitaMisura
GO
CREATE VIEW  CategorieXUnitaMisura  AS SELECT * FROM $(MetaschemaDB).dbo.CategorieXUnitaMisura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClasseTossicologica')
	DROP VIEW ClasseTossicologica
GO
CREATE VIEW  ClasseTossicologica  AS SELECT * FROM $(MetaschemaDB).dbo.ClasseTossicologica WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClassiFasiFeno')
	DROP VIEW ClassiFasiFeno
GO
CREATE VIEW  ClassiFasiFeno  AS SELECT * FROM $(MetaschemaDB).dbo.ClassiFasiFeno WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClassificazioniFertilizzanti')
	DROP VIEW ClassificazioniFertilizzanti
GO
CREATE VIEW  ClassificazioniFertilizzanti  AS SELECT * FROM $(MetaschemaDB).dbo.ClassificazioniFertilizzanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClassificazioniFormulati')
	DROP VIEW ClassificazioniFormulati
GO
CREATE VIEW  ClassificazioniFormulati  AS SELECT * FROM $(MetaschemaDB).dbo.ClassificazioniFormulati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClassificazioniPrincipiAttivi')
	DROP VIEW ClassificazioniPrincipiAttivi
GO
CREATE VIEW  ClassificazioniPrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.ClassificazioniPrincipiAttivi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Coadiuvante')
	DROP VIEW Coadiuvante
GO
CREATE VIEW  Coadiuvante  AS SELECT * FROM $(MetaschemaDB).dbo.Coadiuvante WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codici_Anagrafe')
	DROP VIEW Codici_Anagrafe
GO
CREATE VIEW  Codici_Anagrafe  AS SELECT * FROM $(MetaschemaDB).dbo.Codici_Anagrafe WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codici_attributi')
	DROP VIEW Codici_attributi
GO
CREATE VIEW  Codici_attributi  AS SELECT * FROM $(MetaschemaDB).dbo.Codici_attributi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codici_Persone')
	DROP VIEW Codici_Persone
GO
CREATE VIEW  Codici_Persone  AS SELECT * FROM $(MetaschemaDB).dbo.Codici_Persone WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CoefficienteIntegrativo')
	DROP VIEW CoefficienteIntegrativo
GO
CREATE VIEW  CoefficienteIntegrativo  AS SELECT * FROM $(MetaschemaDB).dbo.CoefficienteIntegrativo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CoefficienteMineralizzazione')
	DROP VIEW CoefficienteMineralizzazione
GO
CREATE VIEW  CoefficienteMineralizzazione  AS SELECT * FROM $(MetaschemaDB).dbo.CoefficienteMineralizzazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CoefficienteUtilizzazione')
	DROP VIEW CoefficienteUtilizzazione
GO
CREATE VIEW  CoefficienteUtilizzazione  AS SELECT * FROM $(MetaschemaDB).dbo.CoefficienteUtilizzazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Coefficienti')
	DROP VIEW Coefficienti
GO
CREATE VIEW  Coefficienti  AS SELECT * FROM $(MetaschemaDB).dbo.Coefficienti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ConduzioneTerrenoSuFila')
	DROP VIEW ConduzioneTerrenoSuFila
GO
CREATE VIEW  ConduzioneTerrenoSuFila  AS SELECT * FROM $(MetaschemaDB).dbo.ConduzioneTerrenoSuFila WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ConduzioneTerrenoTraFila')
	DROP VIEW ConduzioneTerrenoTraFila
GO
CREATE VIEW  ConduzioneTerrenoTraFila  AS SELECT * FROM $(MetaschemaDB).dbo.ConduzioneTerrenoTraFila WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Copertura')
	DROP VIEW Copertura
GO
CREATE VIEW  Copertura  AS SELECT * FROM $(MetaschemaDB).dbo.Copertura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CorrezioneAzoto')
	DROP VIEW CorrezioneAzoto
GO
CREATE VIEW  CorrezioneAzoto  AS SELECT * FROM $(MetaschemaDB).dbo.CorrezioneAzoto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CorrezioniPrecipitazioni')
	DROP VIEW CorrezioniPrecipitazioni
GO
CREATE VIEW  CorrezioniPrecipitazioni  AS SELECT * FROM $(MetaschemaDB).dbo.CorrezioniPrecipitazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Cultivar')
	DROP VIEW Cultivar
GO
CREATE VIEW  Cultivar  AS SELECT * FROM $(MetaschemaDB).dbo.Cultivar WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CultivarxGruppoVarietale')
	DROP VIEW CultivarxGruppoVarietale
GO
CREATE VIEW  CultivarxGruppoVarietale  AS SELECT * FROM $(MetaschemaDB).dbo.CultivarxGruppoVarietale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DanniPostRaccolta')
	DROP VIEW DanniPostRaccolta
GO
CREATE VIEW  DanniPostRaccolta  AS SELECT * FROM $(MetaschemaDB).dbo.DanniPostRaccolta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DanniRaccolta')
	DROP VIEW DanniRaccolta
GO
CREATE VIEW  DanniRaccolta  AS SELECT * FROM $(MetaschemaDB).dbo.DanniRaccolta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DanniRaccoltaxSpecieVegetali')
	DROP VIEW DanniRaccoltaxSpecieVegetali
GO
CREATE VIEW  DanniRaccoltaxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.DanniRaccoltaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DensitaApparente')
	DROP VIEW DensitaApparente
GO
CREATE VIEW  DensitaApparente  AS SELECT * FROM $(MetaschemaDB).dbo.DensitaApparente WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Derrate')
	DROP VIEW Derrate
GO
CREATE VIEW  Derrate  AS SELECT * FROM $(MetaschemaDB).dbo.Derrate WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Descrizioni_Tavole_Fisiche')
	DROP VIEW Descrizioni_Tavole_Fisiche
GO
CREATE VIEW  Descrizioni_Tavole_Fisiche  AS SELECT * FROM $(MetaschemaDB).dbo.Descrizioni_Tavole_Fisiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DisponibilitaAzoto')
	DROP VIEW DisponibilitaAzoto
GO
CREATE VIEW  DisponibilitaAzoto  AS SELECT * FROM $(MetaschemaDB).dbo.DisponibilitaAzoto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DistribuzioneConcime')
	DROP VIEW DistribuzioneConcime
GO
CREATE VIEW  DistribuzioneConcime  AS SELECT * FROM $(MetaschemaDB).dbo.DistribuzioneConcime WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Ditte')
	DROP VIEW Ditte
GO
CREATE VIEW  Ditte  AS SELECT * FROM $(MetaschemaDB).dbo.Ditte WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DittexInsettiUtili')
	DROP VIEW DittexInsettiUtili
GO
CREATE VIEW  DittexInsettiUtili  AS SELECT * FROM $(MetaschemaDB).dbo.DittexInsettiUtili WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Divieti')
	DROP VIEW Divieti
GO
CREATE VIEW  Divieti  AS SELECT * FROM $(MetaschemaDB).dbo.Divieti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DurataImpianto')
	DROP VIEW DurataImpianto
GO
CREATE VIEW  DurataImpianto  AS SELECT * FROM $(MetaschemaDB).dbo.DurataImpianto WITH(NOLOCK)
GO

--if exists (select 1 from sys.views where name = 'EfficienzaLiquami')
--	DROP VIEW EfficienzaLiquami
--GO
--CREATE VIEW  EfficienzaLiquami  AS SELECT * FROM $(MetaschemaDB).dbo.EfficienzaLiquami
--GO

if exists (select 1 from sys.views where name = 'Epoche')
	DROP VIEW Epoche
GO
CREATE VIEW  Epoche  AS SELECT * FROM $(MetaschemaDB).dbo.Epoche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Epoche_Raggruppamenti')
	DROP VIEW Epoche_Raggruppamenti
GO
CREATE VIEW  Epoche_Raggruppamenti AS SELECT * FROM $(MetaschemaDB).dbo.Epoche_Raggruppamenti WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Fabbricati_Tipi')
	DROP VIEW Fabbricati_Tipi
GO
CREATE VIEW  Fabbricati_Tipi  AS SELECT * FROM $(MetaschemaDB).dbo.Fabbricati_Tipi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FamigliePrincipiAttivi')
	DROP VIEW FamigliePrincipiAttivi
GO
CREATE VIEW  FamigliePrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.FamigliePrincipiAttivi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FasiFenologiche')
	DROP VIEW FasiFenologiche
GO
CREATE VIEW  FasiFenologiche  AS SELECT * FROM $(MetaschemaDB).dbo.FasiFenologiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FasiFenologichexFioriture')
	DROP VIEW FasiFenologichexFioriture
GO
CREATE VIEW  FasiFenologichexFioriture  AS SELECT * FROM $(MetaschemaDB).dbo.FasiFenologichexFioriture WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FasiFenologichexSpecieVegetali')
	DROP VIEW FasiFenologichexSpecieVegetali
GO
CREATE VIEW  FasiFenologichexSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.FasiFenologichexSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Fertilizzanti')
	DROP VIEW Fertilizzanti
GO
CREATE VIEW  Fertilizzanti  AS SELECT * FROM $(MetaschemaDB).dbo.Fertilizzanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixClassificazioni')
	DROP VIEW FertilizzantixClassificazioni
GO
CREATE VIEW  FertilizzantixClassificazioni  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixClassificazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixDitte')
	DROP VIEW FertilizzantixDitte
GO
CREATE VIEW  FertilizzantixDitte  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixDitte WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixFormulazioni')
	DROP VIEW FertilizzantixFormulazioni
GO
CREATE VIEW  FertilizzantixFormulazioni  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixFormulazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixPrecauzioni')
	DROP VIEW FertilizzantixPrecauzioni
GO
CREATE VIEW  FertilizzantixPrecauzioni  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixPrecauzioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixRequisiti')
	DROP VIEW FertilizzantixRequisiti
GO
CREATE VIEW  FertilizzantixRequisiti  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixRequisiti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantiXTipologie')
	DROP VIEW FertilizzantiXTipologie
GO
CREATE VIEW  FertilizzantiXTipologie  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantiXTipologie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FertilizzantixTipologieCE')
	DROP VIEW FertilizzantixTipologieCE
GO
CREATE VIEW  FertilizzantixTipologieCE  AS SELECT * FROM $(MetaschemaDB).dbo.FertilizzantixTipologieCE WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Finanziamenti')
	DROP VIEW Finanziamenti
GO
CREATE VIEW  Finanziamenti  AS SELECT * FROM $(MetaschemaDB).dbo.Finanziamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FormeAllevamento')
	DROP VIEW FormeAllevamento
GO
CREATE VIEW  FormeAllevamento  AS SELECT * FROM $(MetaschemaDB).dbo.FormeAllevamento WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FormeAllevamentoxSpecieVegetali')
	DROP VIEW FormeAllevamentoxSpecieVegetali
GO
CREATE VIEW  FormeAllevamentoxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.FormeAllevamentoxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Formulati')
	DROP VIEW Formulati
GO
CREATE VIEW  Formulati  AS SELECT * FROM $(MetaschemaDB).dbo.Formulati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FormulatixClassificazioni')
	DROP VIEW FormulatixClassificazioni
GO
CREATE VIEW  FormulatixClassificazioni  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixClassificazioni WITH(NOLOCK)
GO

--if exists (select 1 from sys.views where name = 'FormulatixAvvertenze')
--	DROP VIEW FormulatixAvvertenze
--GO
--CREATE VIEW  FormulatixAvvertenze  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixAvvertenze
--GO

--if exists (select 1 from sys.views where name = 'FormulatiXCoadiuvante')
--	DROP VIEW FormulatiXCoadiuvante
--GO
--CREATE VIEW  FormulatiXCoadiuvante  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatiXCoadiuvante
--GO

--if exists (select 1 from sys.views where name = 'FormulatiXDerrate')
--	DROP VIEW FormulatiXDerrate
--GO
--CREATE VIEW  FormulatiXDerrate  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatiXDerrate
--GO

--if exists (select 1 from sys.views where name = 'FormulatixDivieti')
--	DROP VIEW FormulatixDivieti
--GO
--CREATE VIEW  FormulatixDivieti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixDivieti
--GO

--if exists (select 1 from sys.views where name = 'FormulatixFitotossicita')
--	DROP VIEW FormulatixFitotossicita
--GO
--CREATE VIEW  FormulatixFitotossicita  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixFitotossicita
--GO

--if exists (select 1 from sys.views where name = 'FormulatiXFormulazioni')
--	DROP VIEW FormulatiXFormulazioni
--GO
--CREATE VIEW  FormulatiXFormulazioni  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatiXFormulazioni
--GO

--if exists (select 1 from sys.views where name = 'FormulatixInfestanti')
--	DROP VIEW FormulatixInfestanti
--GO
--CREATE VIEW  FormulatixInfestanti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixInfestanti
--GO

--if exists (select 1 from sys.views where name = 'FormulatixNocivita')
--	DROP VIEW FormulatixNocivita
--GO
--CREATE VIEW  FormulatixNocivita  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixNocivita
--GO

--if exists (select 1 from sys.views where name = 'FormulatixPrincipiAttivi')
--	DROP VIEW FormulatixPrincipiAttivi
--GO
--CREATE VIEW  FormulatixPrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixPrincipiAttivi
--GO

--if exists (select 1 from sys.views where name = 'FormulatixRegolamenti')
--	DROP VIEW FormulatixRegolamenti
--GO
--CREATE VIEW  FormulatixRegolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixRegolamenti
--GO

--if exists (select 1 from sys.views where name = 'FormulatixRischi')
--	DROP VIEW FormulatixRischi
--GO
--CREATE VIEW  FormulatixRischi  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixRischi
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpecieFitotossiche')
--	DROP VIEW FormulatixSpecieFitotossiche
--GO
--CREATE VIEW  FormulatixSpecieFitotossiche  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpecieFitotossiche
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpecieVegetali')
--	DROP VIEW FormulatixSpecieVegetali
--GO
--CREATE VIEW  FormulatixSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpecieVegetali
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpeciexAvversita')
--	DROP VIEW FormulatixSpeciexAvversita
--GO
--CREATE VIEW  FormulatixSpeciexAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpeciexAvversita
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpeciexAvversitaxDosi')
--	DROP VIEW FormulatixSpeciexAvversitaxDosi
--GO
--CREATE VIEW  FormulatixSpeciexAvversitaxDosi  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpeciexAvversitaxDosi
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpeciexInfestanti')
--	DROP VIEW FormulatixSpeciexInfestanti
--GO
--CREATE VIEW  FormulatixSpeciexInfestanti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpeciexInfestanti
--GO

--if exists (select 1 from sys.views where name = 'FormulatixSpeciexInfestantixDosi')
--	DROP VIEW FormulatixSpeciexInfestantixDosi
--GO
--CREATE VIEW  FormulatixSpeciexInfestantixDosi  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixSpeciexInfestantixDosi
--GO

--if exists (select 1 from sys.views where name = 'FormulatixStabilimenti')
--	DROP VIEW FormulatixStabilimenti
--GO
--CREATE VIEW  FormulatixStabilimenti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulatixStabilimenti
--GO

--if exists (select 1 from sys.views where name = 'Formulazioni')
--	DROP VIEW Formulazioni
--GO
--CREATE VIEW  Formulazioni  AS SELECT * FROM $(MetaschemaDB).dbo.Formulazioni
--GO

if exists (select 1 from sys.views where name = 'FormulazioniFertilizzanti')
	DROP VIEW FormulazioniFertilizzanti
GO
CREATE VIEW  FormulazioniFertilizzanti  AS SELECT * FROM $(MetaschemaDB).dbo.FormulazioniFertilizzanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Funzionalita')
	DROP VIEW Funzionalita
GO
CREATE VIEW  Funzionalita  AS SELECT * FROM $(MetaschemaDB).dbo.Funzionalita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoAvversita')
	DROP VIEW GruppoAvversita
GO
CREATE VIEW  GruppoAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoAvversitaAttive')
	DROP VIEW GruppoAvversitaAttive
GO
CREATE VIEW  GruppoAvversitaAttive  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoAvversitaAttive WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoAvversitaIspave')
	DROP VIEW GruppoAvversitaIspave
GO
CREATE VIEW  GruppoAvversitaIspave  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoAvversitaIspave WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoChimico')
	DROP VIEW GruppoChimico
GO
CREATE VIEW  GruppoChimico  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoChimico WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoColturale')
	DROP VIEW GruppoColturale
GO
CREATE VIEW  GruppoColturale  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoColturale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoColturaleXSpecieVegetali')
	DROP VIEW GruppoColturaleXSpecieVegetali
GO
CREATE VIEW  GruppoColturaleXSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoColturaleXSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoFinalita')
	DROP VIEW GruppoFinalita
GO
CREATE VIEW  GruppoFinalita  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoFinalita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoFinalitaxSpecieVegetali')
	DROP VIEW GruppoFinalitaxSpecieVegetali
GO
CREATE VIEW  GruppoFinalitaxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoFinalitaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoOperazioni')
	DROP VIEW GruppoOperazioni
GO
CREATE VIEW  GruppoOperazioni  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoOperazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoVarietale')
	DROP VIEW GruppoVarietale
GO
CREATE VIEW  GruppoVarietale  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVarietale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoVegetale')
	DROP VIEW GruppoVegetale
GO
CREATE VIEW  GruppoVegetale  AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVegetale WITH(NOLOCK)
GO

--if exists (select 1 from sys.views where name = 'GruppiPrincipiAttivi')
--	DROP VIEW GruppiPrincipiAttivi
--GO
--CREATE VIEW  GruppiPrincipiAttivi AS SELECT * FROM $(MetaschemaDB).dbo.GruppiPrincipiAttivi
--GO

--if exists (select 1 from sys.views where name = 'GruppiPrincipiAttivixPrincipiAttivi')
--	DROP VIEW GruppiPrincipiAttivixPrincipiAttivi
--GO
--CREATE VIEW  GruppiPrincipiAttivixPrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.GruppiPrincipiAttivixPrincipiAttivi
--GO

if exists (select 1 from sys.views where name = 'ImpiantiIrrigazioni')
	DROP VIEW ImpiantiIrrigazioni
GO
CREATE VIEW  ImpiantiIrrigazioni  AS SELECT * FROM $(MetaschemaDB).dbo.ImpiantiIrrigazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ImpiantiIrrigazionixSpecie')
	DROP VIEW ImpiantiIrrigazionixSpecie
GO
CREATE VIEW  ImpiantiIrrigazionixSpecie  AS SELECT * FROM $(MetaschemaDB).dbo.ImpiantiIrrigazionixSpecie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IndiciMaturita')
	DROP VIEW IndiciMaturita
GO
CREATE VIEW  IndiciMaturita  AS SELECT * FROM $(MetaschemaDB).dbo.IndiciMaturita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IndiciMaturitaxSpecieVegetali')
	DROP VIEW IndiciMaturitaxSpecieVegetali
GO
CREATE VIEW  IndiciMaturitaxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.IndiciMaturitaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'InfestantiAttive')
	DROP VIEW InfestantiAttive
GO
CREATE VIEW  InfestantiAttive  AS SELECT * FROM $(MetaschemaDB).dbo.InfestantiAttive WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'InfoMedicheFormulati')
	DROP VIEW InfoMedicheFormulati
GO
CREATE VIEW  InfoMedicheFormulati  AS SELECT * FROM $(MetaschemaDB).dbo.InfoMedicheFormulati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'InfoMedichePrincipiAttivi')
	DROP VIEW InfoMedichePrincipiAttivi
GO
CREATE VIEW  InfoMedichePrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.InfoMedichePrincipiAttivi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'InsettiUtili')
	DROP VIEW InsettiUtili
GO
CREATE VIEW  InsettiUtili  AS SELECT * FROM $(MetaschemaDB).dbo.InsettiUtili WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'InsettiUtilixAvversita')
	DROP VIEW InsettiUtilixAvversita
GO
CREATE VIEW  InsettiUtilixAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.InsettiUtilixAvversita WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'ISTAT')
	DROP VIEW ISTAT
GO
CREATE VIEW  ISTAT  AS SELECT * FROM $(MetaschemaDB).dbo.ISTAT WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ISTAT_Comuni')
	DROP VIEW ISTAT_Comuni
GO
CREATE VIEW  ISTAT_Comuni  AS SELECT * FROM $(MetaschemaDB).dbo.ISTAT_Comuni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'LavorazioneTerreno')
	DROP VIEW LavorazioneTerreno
GO
CREATE VIEW  LavorazioneTerreno  AS SELECT * FROM $(MetaschemaDB).dbo.LavorazioneTerreno WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_K2O')
	DROP VIEW Limiti_K2O
GO
CREATE VIEW  Limiti_K2O  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_K2O WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_K2OxRegolamenti')
	DROP VIEW Limiti_K2OxRegolamenti
GO
CREATE VIEW  Limiti_K2OxRegolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_K2OxRegolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_K2OxSpecieVegetali')
	DROP VIEW Limiti_K2OxSpecieVegetali
GO
CREATE VIEW  Limiti_K2OxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_K2OxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_N')
	DROP VIEW Limiti_N
GO
CREATE VIEW  Limiti_N  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_N WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_NxRegolamenti')
	DROP VIEW Limiti_NxRegolamenti
GO
CREATE VIEW  Limiti_NxRegolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_NxRegolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_NxSpecieVegetali')
	DROP VIEW Limiti_NxSpecieVegetali
GO
CREATE VIEW  Limiti_NxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_NxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_P2O5')
	DROP VIEW Limiti_P2O5
GO
CREATE VIEW  Limiti_P2O5  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_P2O5 WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_P2O5xRegolamenti')
	DROP VIEW Limiti_P2O5xRegolamenti
GO
CREATE VIEW  Limiti_P2O5xRegolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_P2O5xRegolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Limiti_P2O5xSpecieVegetali')
	DROP VIEW Limiti_P2O5xSpecieVegetali
GO
CREATE VIEW  Limiti_P2O5xSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.Limiti_P2O5xSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'LimitiConcimazioneFondo')
	DROP VIEW LimitiConcimazioneFondo
GO
CREATE VIEW  LimitiConcimazioneFondo  AS SELECT * FROM $(MetaschemaDB).dbo.LimitiConcimazioneFondo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'LimitiOrganica')
	DROP VIEW LimitiOrganica
GO
CREATE VIEW  LimitiOrganica  AS SELECT * FROM $(MetaschemaDB).dbo.LimitiOrganica WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'LimitiOrganicaxRegolamenti')
	DROP VIEW LimitiOrganicaxRegolamenti
GO
CREATE VIEW  LimitiOrganicaxRegolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.LimitiOrganicaxRegolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'LimitiOrganicaxSpecieVegetali')
	DROP VIEW LimitiOrganicaxSpecieVegetali
GO
CREATE VIEW  LimitiOrganicaxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.LimitiOrganicaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_attributi_stalla')
	DROP VIEW Lista_attributi_stalla
GO
CREATE VIEW  Lista_attributi_stalla  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_attributi_stalla WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Categorie_Animali')
	DROP VIEW Lista_Categorie_Animali
GO
CREATE VIEW  Lista_Categorie_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Categorie_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_CBT_Indiretta')
	DROP VIEW Lista_CBT_Indiretta
GO
CREATE VIEW  Lista_CBT_Indiretta  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_CBT_Indiretta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Clostridi_Indiretta')
	DROP VIEW Lista_Clostridi_Indiretta
GO
CREATE VIEW  Lista_Clostridi_Indiretta  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Clostridi_Indiretta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Codici_Razze')
	DROP VIEW Lista_Codici_Razze
GO
CREATE VIEW  Lista_Codici_Razze  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Codici_Razze WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Codici_Specie')
	DROP VIEW Lista_Codici_Specie
GO
CREATE VIEW  Lista_Codici_Specie  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Codici_Specie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Coefficienti_UBA')
	DROP VIEW Lista_Coefficienti_UBA
GO
CREATE VIEW  Lista_Coefficienti_UBA  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Coefficienti_UBA WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Eventi_Animali')
	DROP VIEW Lista_Eventi_Animali
GO
CREATE VIEW  Lista_Eventi_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Eventi_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Generi_Animali')
	DROP VIEW Lista_Generi_Animali
GO
CREATE VIEW  Lista_Generi_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Generi_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Giudizi_Caseificabilita')
	DROP VIEW Lista_Giudizi_Caseificabilita
GO
CREATE VIEW  Lista_Giudizi_Caseificabilita  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Giudizi_Caseificabilita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_IndirizziProd_Animali')
	DROP VIEW Lista_IndirizziProd_Animali
GO
CREATE VIEW  Lista_IndirizziProd_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_IndirizziProd_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_LDG')
	DROP VIEW Lista_LDG
GO
CREATE VIEW  Lista_LDG  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_LDG WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Morres')
	DROP VIEW Lista_Morres
GO
CREATE VIEW  Lista_Morres  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Morres WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Parametri')
	DROP VIEW Lista_Parametri
GO
CREATE VIEW  Lista_Parametri  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Parametri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_PFS')
	DROP VIEW Lista_PFS
GO
CREATE VIEW  Lista_PFS  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_PFS WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Province')
	DROP VIEW Lista_Province
GO
CREATE VIEW  Lista_Province  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Province WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Razze_Animali')
	DROP VIEW Lista_Razze_Animali
GO
CREATE VIEW  Lista_Razze_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Razze_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Ricerca_Inibenti')
	DROP VIEW Lista_Ricerca_Inibenti
GO
CREATE VIEW  Lista_Ricerca_Inibenti  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Ricerca_Inibenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Specie_Animali')
	DROP VIEW Lista_Specie_Animali
GO
CREATE VIEW  Lista_Specie_Animali  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Specie_Animali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_Codici_Razze')
	DROP VIEW Lista_Tipi_Codici_Razze
GO
CREATE VIEW  Lista_Tipi_Codici_Razze  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Codici_Razze WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_Codici_Specie')
	DROP VIEW Lista_Tipi_Codici_Specie
GO
CREATE VIEW  Lista_Tipi_Codici_Specie  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Codici_Specie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_dati')
	DROP VIEW Lista_Tipi_dati
GO
CREATE VIEW  Lista_Tipi_dati  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_dati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_Fabbricati')
	DROP VIEW Lista_Tipi_Fabbricati
GO
CREATE VIEW  Lista_Tipi_Fabbricati  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Fabbricati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_Latte')
	DROP VIEW Lista_Tipi_Latte
GO
CREATE VIEW  Lista_Tipi_Latte  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Latte WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipi_Stalla')
	DROP VIEW Lista_Tipi_Stalla
GO
CREATE VIEW  Lista_Tipi_Stalla  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Stalla WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Tipologie_Analisi')
	DROP VIEW Lista_Tipologie_Analisi
GO
CREATE VIEW  Lista_Tipologie_Analisi  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipologie_Analisi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Macchine')
	DROP VIEW Macchine
GO
CREATE VIEW  Macchine  AS SELECT * FROM $(MetaschemaDB).dbo.Macchine WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ManutenzioneTabelle')
	DROP VIEW ManutenzioneTabelle
GO
CREATE VIEW  ManutenzioneTabelle  AS SELECT * FROM $(MetaschemaDB).dbo.ManutenzioneTabelle WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'MisuraxAusiliari')
	DROP VIEW MisuraxAusiliari
GO
CREATE VIEW  MisuraxAusiliari  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxAusiliari WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxAvversita')
	DROP VIEW MisuraxAvversita
GO
CREATE VIEW  MisuraxAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxDanniRaccolta')
	DROP VIEW MisuraxDanniRaccolta
GO
CREATE VIEW  MisuraxDanniRaccolta  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxDanniRaccolta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraXFormulati')
	DROP VIEW MisuraXFormulati
GO
CREATE VIEW  MisuraXFormulati  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraXFormulati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxIndiciMaturita')
	DROP VIEW MisuraxIndiciMaturita
GO
CREATE VIEW  MisuraxIndiciMaturita  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxIndiciMaturita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxOperazioni')
	DROP VIEW MisuraxOperazioni
GO
CREATE VIEW  MisuraxOperazioni  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxOperazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxRilieviCalibri')
	DROP VIEW MisuraxRilieviCalibri
GO
CREATE VIEW  MisuraxRilieviCalibri  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxRilieviCalibri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxSeminaTrapianto')
	DROP VIEW MisuraxSeminaTrapianto
GO
CREATE VIEW  MisuraxSeminaTrapianto  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxSeminaTrapianto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraxTrappole')
	DROP VIEW MisuraxTrappole
GO
CREATE VIEW  MisuraxTrappole  AS SELECT * FROM $(MetaschemaDB).dbo.MisuraxTrappole WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModalitaDazionePrincipiAttivi')
	DROP VIEW ModalitaDazionePrincipiAttivi
GO
CREATE VIEW  ModalitaDazionePrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.ModalitaDazionePrincipiAttivi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModalitaImpiego')
	DROP VIEW ModalitaImpiego
GO
CREATE VIEW  ModalitaImpiego  AS SELECT * FROM $(MetaschemaDB).dbo.ModalitaImpiego WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModelliParametri')
	DROP VIEW ModelliParametri
GO
CREATE VIEW  ModelliParametri  AS SELECT * FROM $(MetaschemaDB).dbo.ModelliParametri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModelliPrevisionali')
	DROP VIEW ModelliPrevisionali
GO
CREATE VIEW  ModelliPrevisionali  AS SELECT * FROM $(MetaschemaDB).dbo.ModelliPrevisionali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ModelliXSpeciexAvversita')
	DROP VIEW ModelliXSpeciexAvversita
GO
CREATE VIEW  ModelliXSpeciexAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.ModelliXSpeciexAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Nocivita')
	DROP VIEW Nocivita
GO
CREATE VIEW  Nocivita  AS SELECT * FROM $(MetaschemaDB).dbo.Nocivita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Operazioni')
	DROP VIEW Operazioni
GO
CREATE VIEW  Operazioni  AS SELECT * FROM $(MetaschemaDB).dbo.Operazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OperazionixMovimenti_Tipo')
	DROP VIEW OperazionixMovimenti_Tipo
GO
CREATE VIEW  OperazionixMovimenti_Tipo  AS SELECT * FROM $(MetaschemaDB).dbo.OperazionixMovimenti_Tipo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ParametriAnalitici')
	DROP VIEW ParametriAnalitici
GO
CREATE VIEW  ParametriAnalitici  AS SELECT * FROM $(MetaschemaDB).dbo.ParametriAnalitici WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PartePianta')
	DROP VIEW PartePianta
GO
CREATE VIEW  PartePianta  AS SELECT * FROM $(MetaschemaDB).dbo.PartePianta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PartePiantaxSpecieVegetali')
	DROP VIEW PartePiantaxSpecieVegetali
GO
CREATE VIEW  PartePiantaxSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.PartePiantaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PerditeLisciviazione')
	DROP VIEW PerditeLisciviazione
GO
CREATE VIEW  PerditeLisciviazione  AS SELECT * FROM $(MetaschemaDB).dbo.PerditeLisciviazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Portinnesti')
	DROP VIEW Portinnesti
GO
CREATE VIEW  Portinnesti  AS SELECT * FROM $(MetaschemaDB).dbo.Portinnesti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PortinnestixSpecieVegetali')
	DROP VIEW PortinnestixSpecieVegetali
GO
CREATE VIEW  PortinnestixSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.PortinnestixSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Precauzioni')
	DROP VIEW Precauzioni
GO
CREATE VIEW  Precauzioni  AS SELECT * FROM $(MetaschemaDB).dbo.Precauzioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PrincipiAttivi')
	DROP VIEW PrincipiAttivi
GO
CREATE VIEW  PrincipiAttivi  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivi WITH(NOLOCK)
GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixAvvertenze')
--	DROP VIEW PrincipiAttivixAvvertenze
--GO
--CREATE VIEW  PrincipiAttivixAvvertenze  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixAvvertenze
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixClassificazioni')
--	DROP VIEW PrincipiAttivixClassificazioni
--GO
--CREATE VIEW  PrincipiAttivixClassificazioni  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixClassificazioni
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttiviXDerrate')
--	DROP VIEW PrincipiAttiviXDerrate
--GO
--CREATE VIEW  PrincipiAttiviXDerrate  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttiviXDerrate
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixDivieti')
--	DROP VIEW PrincipiAttivixDivieti
--GO
--CREATE VIEW  PrincipiAttivixDivieti  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixDivieti
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixFitotossicita')
--	DROP VIEW PrincipiAttivixFitotossicita
--GO
--CREATE VIEW  PrincipiAttivixFitotossicita  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixFitotossicita
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttiviXModalitaDazione')
--	DROP VIEW PrincipiAttiviXModalitaDazione
--GO
--CREATE VIEW  PrincipiAttiviXModalitaDazione  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttiviXModalitaDazione
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixNocivita')
--	DROP VIEW PrincipiAttivixNocivita
--GO
--CREATE VIEW  PrincipiAttivixNocivita  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixNocivita
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttiviXProdotti')
--	DROP VIEW PrincipiAttiviXProdotti
--GO
--CREATE VIEW  PrincipiAttiviXProdotti  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttiviXProdotti
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixRischi')
--	DROP VIEW PrincipiAttivixRischi
--GO
--CREATE VIEW  PrincipiAttivixRischi  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixRischi
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixSpecieFitotossiche')
--	DROP VIEW PrincipiAttivixSpecieFitotossiche
--GO
--CREATE VIEW  PrincipiAttivixSpecieFitotossiche  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixSpecieFitotossiche
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttiviXSpecieVegetali')
--	DROP VIEW PrincipiAttiviXSpecieVegetali
--GO
--CREATE VIEW  PrincipiAttiviXSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttiviXSpecieVegetali
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixSpeciexAvversita')
--	DROP VIEW PrincipiAttivixSpeciexAvversita
--GO
--CREATE VIEW  PrincipiAttivixSpeciexAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixSpeciexAvversita
--GO

--if exists (select 1 from sys.views where name = 'PrincipiAttivixSpeciexImpiego')
--	DROP VIEW PrincipiAttivixSpeciexImpiego
--GO
--CREATE VIEW  PrincipiAttivixSpeciexImpiego  AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivixSpeciexImpiego
--GO

if exists (select 1 from sys.views where name = 'QualitaCatasto')
	DROP VIEW QualitaCatasto
GO
CREATE VIEW  QualitaCatasto  AS SELECT * FROM $(MetaschemaDB).dbo.QualitaCatasto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Regolamenti')
	DROP VIEW Regolamenti
GO
CREATE VIEW  Regolamenti  AS SELECT * FROM $(MetaschemaDB).dbo.Regolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'RegolamentixFertilizzanti')
	DROP VIEW RegolamentixFertilizzanti
GO
CREATE VIEW  RegolamentixFertilizzanti  AS SELECT * FROM $(MetaschemaDB).dbo.RegolamentixFertilizzanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'RegolamentixFormulati')
	DROP VIEW RegolamentixFormulati
GO
CREATE VIEW  RegolamentixFormulati  AS SELECT * FROM $(MetaschemaDB).dbo.RegolamentixFormulati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Requisiti')
	DROP VIEW Requisiti
GO
CREATE VIEW  Requisiti  AS SELECT * FROM $(MetaschemaDB).dbo.Requisiti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Residui')
	DROP VIEW Residui
GO
CREATE VIEW  Residui  AS SELECT * FROM $(MetaschemaDB).dbo.Residui WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Rischi')
	DROP VIEW Rischi
GO
CREATE VIEW  Rischi  AS SELECT * FROM $(MetaschemaDB).dbo.Rischi WITH(NOLOCK)
GO

--if exists (select 1 from sys.views where name = 'SpecieConcimazione')
--	DROP VIEW SpecieConcimazione
--GO
--CREATE VIEW  SpecieConcimazione  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieConcimazione
--GO



if exists (select 1 from sys.views where name = 'SpecieVegetali')
	DROP VIEW SpecieVegetali
GO
CREATE VIEW  SpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetalixAusiliari')
	DROP VIEW SpecieVegetalixAusiliari
GO
CREATE VIEW  SpecieVegetalixAusiliari  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetalixAusiliari WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetalixAvversita')
	DROP VIEW SpecieVegetalixAvversita
GO
CREATE VIEW  SpecieVegetalixAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetalixAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetalixProdotti')
	DROP VIEW SpecieVegetalixProdotti
GO
CREATE VIEW  SpecieVegetalixProdotti  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetalixProdotti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetalixGruppoVarietale')
	DROP VIEW SpecieVegetalixGruppoVarietale
GO
CREATE VIEW  SpecieVegetalixGruppoVarietale  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetalixGruppoVarietale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Stadi_Crescita_BBCH')
	DROP VIEW Stadi_Crescita_BBCH
GO
CREATE VIEW Stadi_Crescita_BBCH AS SELECT * FROM  $(MetaschemaDB).dbo.Stadi_Crescita_BBCH WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'SpecieVegetaliXStadiCrescitaXEpoche')
	DROP VIEW SpecieVegetaliXStadiCrescitaXEpoche
GO
CREATE VIEW SpecieVegetaliXStadiCrescitaXEpoche as SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetaliXStadiCrescitaXEpoche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetaliXStadiCrescita')
	DROP VIEW SpecieVegetaliXStadiCrescita
GO
CREATE VIEW  SpecieVegetaliXStadiCrescita  AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetaliXStadiCrescita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'StadiXSpecieVegetali_XLingua')
	DROP VIEW StadiXSpecieVegetali_XLingua
GO
CREATE VIEW  StadiXSpecieVegetali_XLingua  AS SELECT * FROM $(MetaschemaDB).dbo.StadiXSpecieVegetali_XLingua WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'Stabilimenti')
	DROP VIEW Stabilimenti
GO
CREATE VIEW  Stabilimenti  AS SELECT * FROM $(MetaschemaDB).dbo.Stabilimenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'StimaEfficienza')
	DROP VIEW StimaEfficienza
GO
CREATE VIEW  StimaEfficienza  AS SELECT * FROM $(MetaschemaDB).dbo.StimaEfficienza WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Tipologie')
	DROP VIEW Tipologie
GO
CREATE VIEW  Tipologie  AS SELECT * FROM $(MetaschemaDB).dbo.Tipologie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieCE')
	DROP VIEW TipologieCE
GO
CREATE VIEW  TipologieCE  AS SELECT * FROM $(MetaschemaDB).dbo.TipologieCE WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementi')
	DROP VIEW TipologieSementi
GO
CREATE VIEW  TipologieSementi  AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementixSpecieVegetali')
	DROP VIEW TipologieSementixSpecieVegetali
GO
CREATE VIEW  TipologieSementixSpecieVegetali  AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementixSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Trappole')
	DROP VIEW Trappole
GO
CREATE VIEW  Trappole  AS SELECT * FROM $(MetaschemaDB).dbo.Trappole WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TrappolexAvversita')
	DROP VIEW TrappolexAvversita
GO
CREATE VIEW  TrappolexAvversita  AS SELECT * FROM $(MetaschemaDB).dbo.TrappolexAvversita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TrappolexDitta')
	DROP VIEW TrappolexDitta
GO
CREATE VIEW  TrappolexDitta  AS SELECT * FROM $(MetaschemaDB).dbo.TrappolexDitta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'UnitaMisura')
	DROP VIEW UnitaMisura
GO
CREATE VIEW  UnitaMisura  AS SELECT * FROM $(MetaschemaDB).dbo.UnitaMisura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'UnitaMisuraxCosti')
	DROP VIEW UnitaMisuraxCosti
GO
CREATE VIEW  UnitaMisuraxCosti  AS SELECT * FROM $(MetaschemaDB).dbo.UnitaMisuraxCosti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OrientamentoTecnicoEconomico')
	DROP VIEW OrientamentoTecnicoEconomico
GO
CREATE VIEW  OrientamentoTecnicoEconomico AS SELECT * FROM $(MetaschemaDB).dbo.OrientamentoTecnicoEconomico WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Versione_Database_MetaSchema')
	DROP VIEW Versione_Database_MetaSchema
GO
CREATE VIEW  Versione_Database_MetaSchema  AS SELECT * FROM $(MetaschemaDB).dbo.Versione_Database WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Massimali_Riconversione_Varietale')
	DROP VIEW Massimali_Riconversione_Varietale
GO
CREATE VIEW  Massimali_Riconversione_Varietale AS SELECT * FROM $(MetaschemaDB).dbo.Massimali_Riconversione_Varietale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'xDecoder_Specie_OP2007')
	DROP VIEW xDecoder_Specie_OP2007
GO
CREATE VIEW  xDecoder_Specie_OP2007  AS SELECT * FROM $(MetaschemaDB).dbo.xDecoder_Specie_OP2007 WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'xDecoder_Specie_OPInv')
	DROP VIEW xDecoder_Specie_OPInv
GO
CREATE VIEW  xDecoder_Specie_OPInv  AS SELECT * FROM $(MetaschemaDB).dbo.xDecoder_Specie_OPInv WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'xDecoder_Varieta_OP2007')
	DROP VIEW xDecoder_Varieta_OP2007
GO
CREATE VIEW  xDecoder_Varieta_OP2007  AS SELECT * FROM $(MetaschemaDB).dbo.xDecoder_Varieta_OP2007 WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Titoli_Tipo')
	DROP VIEW Titoli_Tipo
GO
CREATE VIEW Titoli_Tipo AS SELECT * FROM $(MetaschemaDB).dbo.Titoli_Tipo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Titoli_Origine')
	DROP VIEW Titoli_Origine
GO
CREATE VIEW Titoli_Origine AS SELECT * FROM $(MetaschemaDB).dbo.Titoli_Origine WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Titoli_Movimento')
	DROP VIEW Titoli_Movimento
GO
CREATE VIEW Titoli_Movimento AS SELECT * FROM $(MetaschemaDB).dbo.Titoli_Movimento WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Titoli_Stato')
	DROP VIEW Titoli_Stato
GO
CREATE VIEW Titoli_Stato AS SELECT * FROM $(MetaschemaDB).dbo.Titoli_Stato WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Sequenza_Progressivi_Tipi')
	DROP VIEW Sequenza_Progressivi_Tipi
GO
CREATE VIEW Sequenza_Progressivi_Tipi AS SELECT * FROM $(MetaschemaDB).dbo.Sequenza_Progressivi_Tipi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Macrousi')
	DROP VIEW Macrousi
GO
CREATE VIEW Macrousi AS SELECT * FROM $(MetaschemaDB).dbo.Macrousi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IVA_Aliquote')
	DROP VIEW IVA_Aliquote
GO
CREATE VIEW IVA_Aliquote AS SELECT * FROM $(MetaschemaDB).dbo.IVA_Aliquote WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Agro_Reportistica')
	DROP VIEW Agro_Reportistica
GO
CREATE VIEW Agro_Reportistica AS SELECT * FROM $(MetaschemaDB).dbo.Agro_Reportistica WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Agro_Reportistica_Tipi')
	DROP VIEW Agro_Reportistica_Tipi
GO
CREATE VIEW Agro_Reportistica_Tipi AS SELECT * FROM $(MetaschemaDB).dbo.Agro_Reportistica_Tipi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BIO_AgriBio_Consistenze_Codifica')
	DROP VIEW BIO_AgriBio_Consistenze_Codifica
GO
CREATE VIEW BIO_AgriBio_Consistenze_Codifica AS SELECT * FROM $(MetaschemaDB).dbo.BIO_AgriBio_Consistenze_Codifica WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_SpecieVegetali_Agea')
	DROP VIEW Codifica_SpecieVegetali_Agea
GO
CREATE VIEW Codifica_SpecieVegetali_Agea AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Agea WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_SpecieVegetali_Agrea')
	DROP VIEW Codifica_SpecieVegetali_Agrea
GO
CREATE VIEW Codifica_SpecieVegetali_Agrea AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Agrea WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_SpecieVegetali_Avepa')
	DROP VIEW Codifica_SpecieVegetali_Avepa
GO
CREATE VIEW Codifica_SpecieVegetali_Avepa AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Avepa WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_SpecieVegetali_Artea')
	DROP VIEW Codifica_SpecieVegetali_Artea
GO
CREATE VIEW Codifica_SpecieVegetali_Artea AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Artea WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'OGenerazioni_Anagrafe_Moduli')
	DROP VIEW OGenerazioni_Anagrafe_Moduli
GO
CREATE VIEW  OGenerazioni_Anagrafe_Moduli AS SELECT * FROM $(MetaschemaDB).dbo.OGenerazioni_Anagrafe_Moduli WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OGenerazioni_Anagrafe')
	DROP VIEW OGenerazioni_Anagrafe
GO
CREATE VIEW  OGenerazioni_Anagrafe AS SELECT * FROM $(MetaschemaDB).dbo.OGenerazioni_Anagrafe WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OLinee_Produzioni_DPI')
	DROP VIEW OLinee_Produzioni_DPI
GO
CREATE VIEW  OLinee_Produzioni_DPI AS SELECT * FROM $(MetaschemaDB).dbo.OLinee_Produzioni_DPI WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OLinee_Produzioni_DPI_Parametri')
	DROP VIEW OLinee_Produzioni_DPI_Parametri
GO
CREATE VIEW  OLinee_Produzioni_DPI_Parametri AS SELECT * FROM $(MetaschemaDB).dbo.OLinee_Produzioni_DPI_Parametri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OMetaschema')
	DROP VIEW OMetaschema
GO
CREATE VIEW  OMetaschema AS SELECT * FROM $(MetaschemaDB).dbo.OMetaschema WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'OMetaschema_Parametri')
	DROP VIEW OMetaschema_Parametri
GO
CREATE VIEW  OMetaschema_Parametri AS SELECT * FROM $(MetaschemaDB).dbo.OMetaschema_Parametri WITH(NOLOCK)
GO


--if exists (select 1 from sys.views where name = 'FormulatiXPeriodoSospensione')
--	DROP VIEW FormulatiXPeriodoSospensione
--GO
--CREATE VIEW  FormulatiXPeriodoSospensione AS SELECT * FROM $(MetaschemaDB).dbo.FormulatiXPeriodoSospensione
--GO

if exists (select 1 from sys.views where name = 'DPI_Regolamenti')
	DROP VIEW DPI_Regolamenti
GO
CREATE VIEW DPI_Regolamenti AS SELECT * FROM $(MetaschemaDB).dbo.DPI_Regolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'EnteTecnico')
	DROP VIEW EnteTecnico
GO
CREATE VIEW EnteTecnico AS SELECT * FROM $(MetaschemaDB).dbo.EnteTecnico WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Regioni')
	DROP VIEW Lista_Regioni
GO
CREATE VIEW Lista_Regioni AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Regioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Operazioni_XLingue')
	DROP VIEW Operazioni_XLingue
GO
CREATE VIEW Operazioni_XLingue AS SELECT     Lingua_Cod, LAV_COD, LAV_DES, DATA_AGG, inviato, datainvio, Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine FROM         $(MetaschemaDB).dbo.operazioni_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'gruppoOperazioni_XLingua')
	DROP VIEW gruppoOperazioni_XLingua
GO
CREATE VIEW gruppoOperazioni_XLingua AS SELECT     Lingua_COD, Gru_COD, Gru_DES, inviato, datainvio, Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine FROM         $(MetaschemaDB).dbo.gruppoOperazioni_XLingua
GO

if exists (select 1 from sys.views where name = 'SpecieVegetali_XLingue')
	DROP VIEW SpecieVegetali_XLingue
GO
CREATE VIEW SpecieVegetali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetali_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Cultivar_XLingue')
	DROP VIEW Cultivar_XLingue
GO
CREATE VIEW Cultivar_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Cultivar_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ClassificazioniFormulati_XLingue')
	DROP VIEW ClassificazioniFormulati_XLingue
GO
CREATE VIEW ClassificazioniFormulati_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.ClassificazioniFormulati_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoFinalita_XLingue')
	DROP VIEW GruppoFinalita_XLingue
GO
CREATE VIEW GruppoFinalita_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.GruppoFinalita_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Campi')
	DROP VIEW Audit_Campi
GO
CREATE VIEW Audit_Campi AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Campi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Codici')
	DROP VIEW Audit_Codici
GO
CREATE VIEW Audit_Codici AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Codici WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Codici_Dettagli_Coop')
	DROP VIEW Audit_Codici_Dettagli_Coop
GO
CREATE VIEW Audit_Codici_Dettagli_Coop AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Codici_Dettagli_Coop WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Codici_Riferimenti')
	DROP VIEW Audit_Codici_Riferimenti
GO
CREATE VIEW Audit_Codici_Riferimenti AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Codici_Riferimenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_CodiciXCodici')
	DROP VIEW Audit_CodiciXCodici
GO
CREATE VIEW Audit_CodiciXCodici AS SELECT * FROM $(MetaschemaDB).dbo.Audit_CodiciXCodici WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_CodicixDeroghe')
	DROP VIEW Audit_CodicixDeroghe
GO
CREATE VIEW Audit_CodicixDeroghe AS SELECT * FROM $(MetaschemaDB).dbo.Audit_CodicixDeroghe WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Disposizioni')
	DROP VIEW Audit_Disposizioni
GO
CREATE VIEW Audit_Disposizioni AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Disposizioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_DisposizioniXSezioni')
	DROP VIEW Audit_DisposizioniXSezioni
GO
CREATE VIEW Audit_DisposizioniXSezioni AS SELECT * FROM $(MetaschemaDB).dbo.Audit_DisposizioniXSezioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Domande_Interviste')
	DROP VIEW Audit_Domande_Interviste
GO
CREATE VIEW Audit_Domande_Interviste AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Domande_Interviste WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Domande_IntervisteXDisposizioni')
	DROP VIEW Audit_Domande_IntervisteXDisposizioni
GO
CREATE VIEW Audit_Domande_IntervisteXDisposizioni AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Domande_IntervisteXDisposizioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Regolamenti')
	DROP VIEW Audit_Regolamenti
GO
CREATE VIEW Audit_Regolamenti AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Regolamenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Sezioni')
	DROP VIEW Audit_Sezioni
GO
CREATE VIEW Audit_Sezioni AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Sezioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Stati')
	DROP VIEW Audit_Stati
GO
CREATE VIEW Audit_Stati AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Stati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Audit_Tipi')
	DROP VIEW Audit_Tipi
GO
CREATE VIEW Audit_Tipi AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Tipi WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA20_CodificaProdotti')
	DROP VIEW  ACCDAA_ANAG_TA20_CodificaProdotti 
GO
CREATE VIEW  ACCDAA_ANAG_TA20_CodificaProdotti AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_TA20_CodificaProdotti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA16_TipiStoccaggioAlcoli')
	DROP VIEW  ACCDAA_ANAG_TA16_TipiStoccaggioAlcoli 
GO
CREATE VIEW  ACCDAA_ANAG_TA16_TipiStoccaggioAlcoli AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_TA16_TipiStoccaggioAlcoli WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA15_TipiRegistroAlcoli')
	DROP VIEW  ACCDAA_ANAG_TA15_TipiRegistroAlcoli 
GO
CREATE VIEW  ACCDAA_ANAG_TA15_TipiRegistroAlcoli AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_TA15_TipiRegistroAlcoli WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_TA03_UfficiDA')
	DROP VIEW  ACCDAA_ANAG_TA03_UfficiDA 
GO
CREATE VIEW  ACCDAA_ANAG_TA03_UfficiDA AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_TA03_UfficiDA WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T018_TabellaCodiciCategorieVino')
	DROP VIEW  ACCDAA_ANAG_T018_TabellaCodiciCategorieVino 
GO
CREATE VIEW  ACCDAA_ANAG_T018_TabellaCodiciCategorieVino AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T018_TabellaCodiciCategorieVino WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T017_TabellaCodiciOrigineDellaSpedizione')
	DROP VIEW  ACCDAA_ANAG_T017_TabellaCodiciOrigineDellaSpedizione 
GO
CREATE VIEW  ACCDAA_ANAG_T017_TabellaCodiciOrigineDellaSpedizione AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T017_TabellaCodiciOrigineDellaSpedizione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T015_TabellaCodiciTipoDestinazione')
	DROP VIEW  ACCDAA_ANAG_T015_TabellaCodiciTipoDestinazione 
GO
CREATE VIEW  ACCDAA_ANAG_T015_TabellaCodiciTipoDestinazione AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T015_TabellaCodiciTipoDestinazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T012_TabellaCodiciProdottoAccisa')
	DROP VIEW  ACCDAA_ANAG_T012_TabellaCodiciProdottoAccisa 
GO
CREATE VIEW  ACCDAA_ANAG_T012_TabellaCodiciProdottoAccisa AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T012_TabellaCodiciProdottoAccisa WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T010_TabellaCodiciImballaggio')
	DROP VIEW  ACCDAA_ANAG_T010_TabellaCodiciImballaggio 
GO
CREATE VIEW  ACCDAA_ANAG_T010_TabellaCodiciImballaggio AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T010_TabellaCodiciImballaggio WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T009_TabellaCodiciUnitaDiTrasporto')
	DROP VIEW  ACCDAA_ANAG_T009_TabellaCodiciUnitaDiTrasporto 
GO
CREATE VIEW  ACCDAA_ANAG_T009_TabellaCodiciUnitaDiTrasporto AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T009_TabellaCodiciUnitaDiTrasporto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T008_TabellaCodiciModalitaDiTrasporto')
	DROP VIEW  ACCDAA_ANAG_T008_TabellaCodiciModalitaDiTrasporto
GO
CREATE VIEW  ACCDAA_ANAG_T008_TabellaCodiciModalitaDiTrasporto AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T008_TabellaCodiciModalitaDiTrasporto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_DAA_CalcoloBaseCauzione')
	DROP VIEW ACCDAA_DAA_CalcoloBaseCauzione
GO
CREATE VIEW ACCDAA_DAA_CalcoloBaseCauzione AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_DAA_CalcoloBaseCauzione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_DAA_CalcoloBaseCauzione_TipoTariffa')
	DROP VIEW ACCDAA_DAA_CalcoloBaseCauzione_TipoTariffa
GO
CREATE VIEW ACCDAA_DAA_CalcoloBaseCauzione_TipoTariffa AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_DAA_CalcoloBaseCauzione_TipoTariffa WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_DAA_CalcoloBaseCauzione_Valute')
	DROP VIEW ACCDAA_DAA_CalcoloBaseCauzione_Valute
GO
CREATE VIEW ACCDAA_DAA_CalcoloBaseCauzione_Valute AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_DAA_CalcoloBaseCauzione_Valute WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T004_TabellaCodiciStatiMembri')
	DROP VIEW ACCDAA_ANAG_T004_TabellaCodiciStatiMembri
GO
CREATE VIEW ACCDAA_ANAG_T004_TabellaCodiciStatiMembri AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T004_TabellaCodiciStatiMembri WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T005_TabellaCodiciPaesiTerziISO3166')
	DROP VIEW ACCDAA_ANAG_T005_TabellaCodiciPaesiTerziISO3166
GO
CREATE VIEW ACCDAA_ANAG_T005_TabellaCodiciPaesiTerziISO3166 AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T005_TabellaCodiciPaesiTerziISO3166 WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ACCDAA_ANAG_T007_TabellaCodiciTipoGarante')
	DROP VIEW ACCDAA_ANAG_T007_TabellaCodiciTipoGarante
GO
CREATE VIEW ACCDAA_ANAG_T007_TabellaCodiciTipoGarante AS SELECT * FROM $(MetaschemaDB).dbo.ACCDAA_ANAG_T007_TabellaCodiciTipoGarante WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Servizi')
	DROP VIEW Servizi
GO
CREATE VIEW Servizi AS SELECT * FROM $(MetaschemaDB).dbo.Servizi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Servizi_Stati')
	DROP VIEW Servizi_Stati
GO
CREATE VIEW Servizi_Stati AS SELECT * FROM $(MetaschemaDB).dbo.Servizi_Stati WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IVA_CodiciIvaProdottiAgricoli')
	DROP VIEW IVA_CodiciIvaProdottiAgricoli
GO
CREATE VIEW IVA_CodiciIvaProdottiAgricoli AS SELECT * FROM $(MetaschemaDB).dbo.IVA_CodiciIvaProdottiAgricoli WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'StampeReport')
	DROP VIEW StampeReport
GO
CREATE VIEW StampeReport as SELECT * FROM $(MetaschemaDB).dbo.StampeReport WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'StampeReportGruppi')
	DROP VIEW StampeReportGruppi
GO
CREATE VIEW StampeReportGruppi as SELECT * FROM $(MetaschemaDB).dbo.StampeReportGruppi WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Macchine_Caratteristiche')
	DROP VIEW Macchine_Caratteristiche
GO
CREATE VIEW Macchine_Caratteristiche AS SELECT * FROM $(MetaschemaDB).dbo.Macchine_Caratteristiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MacchinexCaratteristiche')
	DROP VIEW MacchinexCaratteristiche
GO
CREATE VIEW MacchinexCaratteristiche AS SELECT * FROM $(MetaschemaDB).dbo.MacchinexCaratteristiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MisuraXAvversita_XFaseFenologica')
	DROP VIEW MisuraXAvversita_XFaseFenologica
GO
CREATE VIEW MisuraXAvversita_XFaseFenologica AS SELECT * FROM $(MetaschemaDB).dbo.MisuraXAvversita_XFaseFenologica WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IVA_DescrizioneEstesaTipoOperazione')
	DROP VIEW IVA_DescrizioneEstesaTipoOperazione
GO
CREATE VIEW IVA_DescrizioneEstesaTipoOperazione as SELECT * FROM $(MetaschemaDB).dbo.IVA_DescrizioneEstesaTipoOperazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PROFIS_Decodifica_TipiIVA_LavCod_CodIVA')
	DROP VIEW PROFIS_Decodifica_TipiIVA_LavCod_CodIVA
GO
CREATE VIEW PROFIS_Decodifica_TipiIVA_LavCod_CodIVA as SELECT * FROM $(MetaschemaDB).dbo.PROFIS_Decodifica_TipiIVA_LavCod_CodIVA WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_SpecieVegetali_Dogane')
	DROP VIEW Codifica_SpecieVegetali_Dogane
GO
CREATE VIEW Codifica_SpecieVegetali_Dogane as SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Dogane WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FormeGiuridiche')
	DROP VIEW FormeGiuridiche
GO
CREATE VIEW FormeGiuridiche AS SELECT * FROM $(MetaschemaDB).dbo.FormeGiuridiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CategorieAmmortizzabili')
	DROP VIEW CategorieAmmortizzabili  
GO
CREATE VIEW  CategorieAmmortizzabili  AS SELECT * FROM $(MetaschemaDB).dbo.CategorieAmmortizzabili WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Testata')
	DROP VIEW TeleRegistri_Testata 
GO
CREATE VIEW TeleRegistri_Testata AS select * from $(MetaschemaDB).dbo.TeleRegistri_Testata WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Dettagli')
	DROP VIEW TeleRegistri_Dettagli
GO
CREATE VIEW TeleRegistri_Dettagli AS select * from $(MetaschemaDB).dbo.TeleRegistri_Dettagli WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Operazioni')
	DROP VIEW TeleRegistri_Operazioni
GO
CREATE VIEW TeleRegistri_Operazioni AS select * from $(MetaschemaDB).dbo.TeleRegistri_Operazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Nodi')
	DROP VIEW TeleRegistri_Nodi
GO
CREATE VIEW TeleRegistri_Nodi AS select * from $(MetaschemaDB).dbo.TeleRegistri_Nodi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Attributi')
	DROP VIEW TeleRegistri_Attributi
GO
CREATE VIEW TeleRegistri_Attributi AS select * from $(MetaschemaDB).dbo.TeleRegistri_Attributi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TeleRegistri_Matrice')
	DROP VIEW TeleRegistri_Matrice
GO
CREATE VIEW TeleRegistri_Matrice AS select * from $(MetaschemaDB).dbo.TeleRegistri_Matrice WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CatalogoEuropeoRifiuti')
	DROP VIEW CatalogoEuropeoRifiuti
GO
CREATE VIEW CatalogoEuropeoRifiuti AS select * from $(MetaschemaDB).dbo.CatalogoEuropeoRifiuti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MacroUsi_xMacroUsi_SQNPI')
	DROP VIEW MacroUsi_xMacroUsi_SQNPI
GO
CREATE VIEW MacroUsi_xMacroUsi_SQNPI AS select * from $(MetaschemaDB).dbo.MacroUsi_xMacroUsi_SQNPI WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ws_RegVino_CodiciRitorno')
	DROP VIEW ws_RegVino_CodiciRitorno
GO
CREATE VIEW ws_RegVino_CodiciRitorno AS select * from $(MetaschemaDB).dbo.ws_RegVino_CodiciRitorno WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'AGEACodificaVini')
	DROP VIEW AGEACodificaVini
GO
CREATE VIEW AGEACodificaVini AS select * from $(MetaschemaDB).dbo.AGEACodificaVini WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_FormeAllevamento')
	DROP VIEW Codifica_FormeAllevamento
GO
CREATE VIEW Codifica_FormeAllevamento AS select * from $(MetaschemaDB).dbo.Codifica_FormeAllevamento WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_Vini_Agea_Enti')
	DROP VIEW Codifica_Vini_Agea_Enti
GO
CREATE VIEW Codifica_Vini_Agea_Enti  AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Vini_Agea_Enti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'RuoliAziendali')
	DROP VIEW RuoliAziendali
GO
CREATE VIEW RuoliAziendali  AS SELECT * FROM $(MetaschemaDB).dbo.RuoliAziendali WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'Codifica_SpecieVegetali_Agea_2015_2020')
	DROP VIEW Codifica_SpecieVegetali_Agea_2015_2020
GO
CREATE VIEW Codifica_SpecieVegetali_Agea_2015_2020  AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Agea_2015_2020 WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'Codifica_SpecieVegetali_Enti_2015_2020')
	DROP VIEW Codifica_SpecieVegetali_Enti_2015_2020
GO
CREATE VIEW Codifica_SpecieVegetali_Enti_2015_2020  AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Enti_2015_2020 WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'Lista_Tipi_Raggruppamento_Stalla')
	DROP VIEW Lista_Tipi_Raggruppamento_Stalla
GO
CREATE VIEW Lista_Tipi_Raggruppamento_Stalla  AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Raggruppamento_Stalla WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'RegimiFiscali')
	DROP VIEW RegimiFiscali
GO
CREATE VIEW RegimiFiscali  AS SELECT * FROM $(MetaschemaDB).dbo.RegimiFiscali WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MenuBS_2017_PulsantiMenu')
	DROP VIEW MenuBS_2017_PulsantiMenu
GO
CREATE VIEW MenuBS_2017_PulsantiMenu  AS SELECT * FROM $(MetaschemaDB).dbo.MenuBS_2017_PulsantiMenu WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MenuBS_2017_Sezioni')
	DROP VIEW MenuBS_2017_Sezioni
GO
CREATE VIEW MenuBS_2017_Sezioni  AS SELECT * FROM $(MetaschemaDB).dbo.MenuBS_2017_Sezioni WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'MenuBS_2017_WidgetAllarmi')
	DROP VIEW MenuBS_2017_WidgetAllarmi
GO
CREATE VIEW MenuBS_2017_WidgetAllarmi  AS SELECT * FROM $(MetaschemaDB).dbo.MenuBS_2017_WidgetAllarmi WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ModelliPrevisionaliRaggruppamenti')
	DROP VIEW ModelliPrevisionaliRaggruppamenti
GO
CREATE VIEW ModelliPrevisionaliRaggruppamenti  AS SELECT * FROM $(MetaschemaDB).dbo.ModelliPrevisionaliRaggruppamenti WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ModelliPrevisionaliRaggruppamentiXModelliPrevisionali')
	DROP VIEW ModelliPrevisionaliRaggruppamentiXModelliPrevisionali
GO
CREATE VIEW ModelliPrevisionaliRaggruppamentiXModelliPrevisionali  AS SELECT * FROM $(MetaschemaDB).dbo.ModelliPrevisionaliRaggruppamentiXModelliPrevisionali WITH(NOLOCK)
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'Codifica_Macchine_Agea')
	DROP VIEW Codifica_Macchine_Agea
GO
CREATE VIEW Codifica_Macchine_Agea  AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Macchine_Agea WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Codifica_Varieta_OIPomodorodaIndustriaNordItalia')
	DROP VIEW Codifica_Varieta_OIPomodorodaIndustriaNordItalia
GO
CREATE VIEW Codifica_Varieta_OIPomodorodaIndustriaNordItalia AS select * from $(MetaschemaDB).dbo.Codifica_Varieta_OIPomodorodaIndustriaNordItalia WITH(NOLOCK)
GO



if exists (select 1 from sys.views where name = 'Avversita_XLingue')
	DROP VIEW Avversita_XLingue
GO
CREATE VIEW Avversita_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Avversita_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoAvversita_XLingue')
	DROP VIEW GruppoAvversita_XLingue
GO
CREATE VIEW GruppoAvversita_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.GruppoAvversita_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'CategorieMagazzino_XLingue')
	DROP VIEW CategorieMagazzino_XLingue
GO
CREATE VIEW CategorieMagazzino_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.CategorieMagazzino_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementi_XLingue')
	DROP VIEW TipologieSementi_XLingue
GO
CREATE VIEW TipologieSementi_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementi_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoVegetale_XLingue')
	DROP VIEW GruppoVegetale_XLingue
GO
CREATE VIEW GruppoVegetale_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVegetale_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoVarietale_XLingue')
	DROP VIEW GruppoVarietale_XLingue
GO
CREATE VIEW GruppoVarietale_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVarietale_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'FormeAllevamento_XLingue')
	DROP VIEW FormeAllevamento_XLingue
GO
CREATE VIEW FormeAllevamento_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.FormeAllevamento_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ImpiantiIrrigazioni_XLingue')
	DROP VIEW ImpiantiIrrigazioni_XLingue
GO
CREATE VIEW ImpiantiIrrigazioni_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.ImpiantiIrrigazioni_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Portinnesti_XLingue')
	DROP VIEW Portinnesti_XLingue
GO
CREATE VIEW Portinnesti_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Portinnesti_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Epoche_XLingue')
	DROP VIEW Epoche_XLingue
GO
CREATE VIEW Epoche_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Epoche_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Fabbricati_Tipi_XLingue')
	DROP VIEW Fabbricati_Tipi_XLingue
GO
CREATE VIEW Fabbricati_Tipi_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Fabbricati_Tipi_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Macchine_XLingue')
	DROP VIEW Macchine_XLingue
GO
CREATE VIEW Macchine_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Macchine_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'StampeReport_XLingue')
	DROP VIEW StampeReport_XLingue
GO
CREATE VIEW StampeReport_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.StampeReport_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'MenuBS_2017_Sezioni_XLingua')
	DROP VIEW MenuBS_2017_Sezioni_XLingua
GO
CREATE VIEW MenuBS_2017_Sezioni_XLingua AS SELECT * FROM $(MetaschemaDB).dbo.MenuBS_2017_Sezioni_XLingua WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Tipologie_XLingue')
	DROP VIEW Tipologie_XLingue
GO
CREATE VIEW Tipologie_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Tipologie_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Servizi_Configurazione')
	DROP VIEW Servizi_Configurazione
GO
CREATE VIEW Servizi_Configurazione AS SELECT * FROM $(MetaschemaDB).dbo.Servizi_Configurazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Prenotazione_Piante_Certificazione')
	DROP VIEW Prenotazione_Piante_Certificazione
GO
CREATE VIEW  Prenotazione_Piante_Certificazione AS SELECT * FROM $(MetaschemaDB).dbo.Prenotazione_Piante_Certificazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Prenotazione_Piante_Categoria')
	DROP VIEW Prenotazione_Piante_Categoria
GO
CREATE VIEW  Prenotazione_Piante_Categoria AS SELECT * FROM $(MetaschemaDB).dbo.Prenotazione_Piante_Categoria WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Prenotazione_Piante_Certificazione_XLingue')
	DROP VIEW Prenotazione_Piante_Certificazione_XLingue
GO
CREATE VIEW  Prenotazione_Piante_Certificazione_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Prenotazione_Piante_Certificazione_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Prenotazione_Piante_Categoria_XLingue')
	DROP VIEW Prenotazione_Piante_Categoria_XLingue
GO
CREATE VIEW  Prenotazione_Piante_Categoria_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Prenotazione_Piante_Categoria_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Analisi_Tipi')
	DROP VIEW Analisi_Tipi
GO
CREATE VIEW  Analisi_Tipi AS SELECT * FROM $(MetaschemaDB).dbo.Analisi_Tipi WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Analisi_Tipi_XLingue')
	DROP VIEW Analisi_Tipi_XLingue
GO
CREATE VIEW  Analisi_Tipi_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Analisi_Tipi_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'UnitaMisura_XLingue')
	DROP VIEW UnitaMisura_XLingue
GO
CREATE VIEW  UnitaMisura_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.UnitaMisura_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codici_Anagrafe_XLingue')
	DROP VIEW Codici_Anagrafe_XLingue
GO
CREATE VIEW  Codici_Anagrafe_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Codici_Anagrafe_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'IndiciMaturita_XLingue')
	DROP VIEW IndiciMaturita_XLingue
GO
CREATE VIEW  IndiciMaturita_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.IndiciMaturita_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'TipologieDocumento')
	DROP VIEW TipologieDocumento
GO
CREATE VIEW  TipologieDocumento AS SELECT * FROM $(MetaschemaDB).dbo.TipologieDocumento WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'MatriceProdottiSian')
	DROP VIEW MatriceProdottiSian
GO
CREATE VIEW  MatriceProdottiSian AS SELECT * FROM $(MetaschemaDB).dbo.MatriceProdottiSian WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'WTransizioniDiStatoAudit')
	DROP VIEW WTransizioniDiStatoAudit
GO
CREATE VIEW  WTransizioniDiStatoAudit AS SELECT * FROM $(MetaschemaDB).dbo.WTransizioniDiStatoAudit WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Audit_Label')
	DROP VIEW Audit_Label
GO
CREATE VIEW  Audit_Label AS SELECT * FROM $(MetaschemaDB).dbo.Audit_Label WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'ParticelleCatastali_Acclivita')
	DROP VIEW ParticelleCatastali_Acclivita
GO
CREATE VIEW  ParticelleCatastali_Acclivita AS SELECT * FROM $(MetaschemaDB).dbo.ParticelleCatastali_Acclivita WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ISTAT_Cod_Nazionale_Sezione')
	DROP VIEW ISTAT_Cod_Nazionale_Sezione
GO
CREATE VIEW ISTAT_Cod_Nazionale_Sezione AS SELECT * FROM $(MetaschemaDB).dbo.ISTAT_Cod_Nazionale_Sezione WITH(NOLOCK)
GO


------------------------------------------------------------------------------------------------- (anna)
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Sistemi_Esterni')
	DROP VIEW Sistemi_Esterni
GO
CREATE VIEW Sistemi_Esterni AS SELECT * FROM $(MetaschemaDB).dbo.Sistemi_Esterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_Operazioni_SistemiEsterni')
	DROP VIEW Codifica_Operazioni_SistemiEsterni
GO
CREATE VIEW Codifica_Operazioni_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Operazioni_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_UnitaMisura_SistemiEsterni')
	DROP VIEW Codifica_UnitaMisura_SistemiEsterni
GO
CREATE VIEW Codifica_UnitaMisura_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_UnitaMisura_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_FormeAllevamento_SistemiEsterni')
	DROP VIEW Codifica_FormeAllevamento_SistemiEsterni
GO
CREATE VIEW Codifica_FormeAllevamento_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_FormeAllevamento_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_Avversita_SistemiEsterni')
	DROP VIEW Codifica_Avversita_SistemiEsterni 
GO
CREATE VIEW Codifica_Avversita_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Avversita_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_Macchine_SistemiEsterni')
	DROP VIEW Codifica_Macchine_SistemiEsterni
GO
CREATE VIEW Codifica_Macchine_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Macchine_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_FasiFenologicheEpoche_SistemiEsterni')
	DROP VIEW Codifica_FasiFenologicheEpoche_SistemiEsterni
GO
CREATE VIEW Codifica_FasiFenologicheEpoche_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_FasiFenologicheEpoche_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_Coperture_SistemiEsterni')
	DROP VIEW Codifica_Coperture_SistemiEsterni
GO
CREATE VIEW Codifica_Coperture_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Coperture_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_ImpiantiIrrigui_SistemiEsterni')
	DROP VIEW Codifica_ImpiantiIrrigui_SistemiEsterni
GO
CREATE VIEW Codifica_ImpiantiIrrigui_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_ImpiantiIrrigui_SistemiEsterni WITH(NOLOCK)
GO


IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_SpecieVegetali_Clienti')
	DROP VIEW Codifica_SpecieVegetali_Clienti
GO
CREATE VIEW Codifica_SpecieVegetali_Clienti AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_Clienti WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_ImpiantiIrrigui_Clienti')
	DROP VIEW Codifica_ImpiantiIrrigui_Clienti
GO
CREATE VIEW Codifica_ImpiantiIrrigui_Clienti AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_ImpiantiIrrigui_Clienti WITH(NOLOCK)
GO
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_Coperture_Clienti')
	DROP VIEW Codifica_Coperture_Clienti
GO
CREATE VIEW Codifica_Coperture_Clienti AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_Coperture_Clienti WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Operazioni_Combinazioni')
	DROP VIEW Operazioni_Combinazioni
GO
CREATE VIEW Operazioni_Combinazioni AS SELECT * FROM $(MetaschemaDB).dbo.Operazioni_Combinazioni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_BDN_GeneriAnimali')
	DROP VIEW Codifica_BDN_GeneriAnimali
GO
CREATE VIEW Codifica_BDN_GeneriAnimali AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_BDN_GeneriAnimali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_BDN_SpecieAnimali')
	DROP VIEW Codifica_BDN_SpecieAnimali
GO
CREATE VIEW Codifica_BDN_SpecieAnimali AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_BDN_SpecieAnimali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_BDN_RazzeAnimali')
	DROP VIEW Codifica_BDN_RazzeAnimali
GO
CREATE VIEW Codifica_BDN_RazzeAnimali AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_BDN_RazzeAnimali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_BDN_CategorieAnimali')
	DROP VIEW Codifica_BDN_CategorieAnimali
GO
CREATE VIEW Codifica_BDN_CategorieAnimali AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_BDN_CategorieAnimali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GruppoAvversitaXGruppoAvversita')
	DROP VIEW GruppoAvversitaXGruppoAvversita
GO
CREATE VIEW GruppoAvversitaXGruppoAvversita AS SELECT * FROM $(MetaschemaDB).dbo.GruppoAvversitaXGruppoAvversita WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Farmaci')
	DROP VIEW Farmaci
GO
CREATE VIEW Farmaci AS SELECT * FROM $(MetaschemaDB).dbo.Farmaci WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'Widgets')
	DROP VIEW Widgets
GO
CREATE VIEW Widgets AS SELECT * FROM $(MetaschemaDB).dbo.Widgets WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'ContributiColtivazioni')
	DROP VIEW ContributiColtivazioni
GO
CREATE VIEW ContributiColtivazioni AS SELECT * FROM $(MetaschemaDB).dbo.ContributiColtivazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Farmaci_Categorie')
	DROP VIEW Farmaci_Categorie
GO
CREATE VIEW Farmaci_Categorie AS SELECT * FROM $(MetaschemaDB).dbo.Farmaci_Categorie WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'FarmacixCategorie')
	DROP VIEW FarmacixCategorie
GO
CREATE VIEW FarmacixCategorie AS SELECT * FROM $(MetaschemaDB).dbo.FarmacixCategorie WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Farmaci_ModalitaPrescrizione')
	DROP VIEW Farmaci_ModalitaPrescrizione
GO
CREATE VIEW Farmaci_ModalitaPrescrizione AS SELECT * FROM $(MetaschemaDB).dbo.Farmaci_ModalitaPrescrizione WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'FarmacixPrincipiAttivi')
	DROP VIEW FarmacixPrincipiAttivi
GO
CREATE VIEW FarmacixPrincipiAttivi AS SELECT * FROM $(MetaschemaDB).dbo.FarmacixPrincipiAttivi WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Fertilizzanti_GHG')
	DROP VIEW Fertilizzanti_GHG
GO
CREATE VIEW Fertilizzanti_GHG AS SELECT * FROM $(MetaschemaDB).dbo.Fertilizzanti_GHG WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Cultivar_GHG')
	DROP VIEW Cultivar_GHG
GO
CREATE VIEW Cultivar_GHG AS SELECT * FROM $(MetaschemaDB).dbo.Cultivar_GHG WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Formulati_GHG')
	DROP VIEW Formulati_GHG
GO
CREATE VIEW Formulati_GHG AS SELECT * FROM $(MetaschemaDB).dbo.Formulati_GHG WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Macchine_Consumi')
	DROP VIEW Macchine_Consumi
GO
CREATE VIEW Macchine_Consumi AS SELECT * FROM $(MetaschemaDB).dbo.Macchine_Consumi WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Carburanti_GHG')
	DROP VIEW Carburanti_GHG
GO
CREATE VIEW Carburanti_GHG AS SELECT * FROM $(MetaschemaDB).dbo.Carburanti_GHG WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Direttive')
	DROP VIEW Direttive
GO
CREATE VIEW Direttive AS SELECT * FROM $(MetaschemaDB).dbo.Direttive WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_AUSL')
	DROP VIEW Lista_AUSL
GO
CREATE VIEW Lista_AUSL AS select * from $(MetaschemaDB).dbo.Lista_AUSL WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Distretti')
	DROP VIEW Lista_Distretti
GO
CREATE VIEW Lista_Distretti AS select * from $(MetaschemaDB).dbo.Lista_Distretti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'IstatxDistretti')
	DROP VIEW IstatxDistretti
GO
CREATE VIEW IstatxDistretti AS select * from $(MetaschemaDB).dbo.IstatxDistretti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Causali_Morte')
	DROP VIEW Lista_Causali_Morte
GO
CREATE VIEW Lista_Causali_Morte AS select * from $(MetaschemaDB).dbo.Lista_Causali_Morte WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Codifica_SpecieVegetali_SistemiEsterni')
	DROP VIEW Codifica_SpecieVegetali_SistemiEsterni
GO
CREATE VIEW Codifica_SpecieVegetali_SistemiEsterni AS SELECT * FROM $(MetaschemaDB).dbo.Codifica_SpecieVegetali_SistemiEsterni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Tipo_Controlli')
	DROP VIEW Tipo_Controlli
GO
CREATE VIEW Tipo_Controlli AS SELECT * FROM $(MetaschemaDB).dbo.Tipo_Controlli WITH(NOLOCK)
GO


IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Servizi_Sottoscrizione_Notifiche')
	DROP VIEW Servizi_Sottoscrizione_Notifiche
GO
CREATE VIEW Servizi_Sottoscrizione_Notifiche AS SELECT * FROM $(MetaschemaDB).dbo.Servizi_Sottoscrizione_Notifiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Farmaci_Categorie_Semplificate')
	DROP VIEW Farmaci_Categorie_Semplificate
GO
CREATE VIEW Farmaci_Categorie_Semplificate AS SELECT * FROM $(MetaschemaDB).dbo.Farmaci_Categorie_Semplificate WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Farmaci_CategoriexFarmaci_Categorie_Semplificate')
	DROP VIEW Farmaci_CategoriexFarmaci_Categorie_Semplificate
GO
CREATE VIEW Farmaci_CategoriexFarmaci_Categorie_Semplificate AS SELECT * FROM $(MetaschemaDB).dbo.Farmaci_CategoriexFarmaci_Categorie_Semplificate WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GIS_LayerAnalysisConfig_Algorithm')
	DROP VIEW GIS_LayerAnalysisConfig_Algorithm
GO
CREATE VIEW GIS_LayerAnalysisConfig_Algorithm AS SELECT * FROM $(MetaschemaDB).dbo.GIS_LayerAnalysisConfig_Algorithm WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GIS_LayerAnalysisConfig_Algorithm_Params')
	DROP VIEW GIS_LayerAnalysisConfig_Algorithm_Params
GO
CREATE VIEW GIS_LayerAnalysisConfig_Algorithm_Params AS SELECT * FROM $(MetaschemaDB).dbo.GIS_LayerAnalysisConfig_Algorithm_Params WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GIS_LayerXConfig')
	DROP VIEW GIS_LayerXConfig
GO
CREATE VIEW GIS_LayerXConfig AS SELECT * FROM $(MetaschemaDB).dbo.GIS_LayerXConfig WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GIS_LayerAnalysisConfig_AlgorithmType')
	DROP VIEW GIS_LayerAnalysisConfig_AlgorithmType
GO
CREATE VIEW GIS_LayerAnalysisConfig_AlgorithmType AS SELECT * FROM $(MetaschemaDB).dbo.GIS_LayerAnalysisConfig_AlgorithmType WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'GIS_LayerAnalysisConfig_AlgorithmType_Param')
	DROP VIEW GIS_LayerAnalysisConfig_AlgorithmType_Param
GO
CREATE VIEW GIS_LayerAnalysisConfig_AlgorithmType_Param AS SELECT * FROM $(MetaschemaDB).dbo.GIS_LayerAnalysisConfig_AlgorithmType_Param WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'InsettiUtilixPrincipiAttivi')
	DROP VIEW InsettiUtilixPrincipiAttivi
GO
CREATE VIEW InsettiUtilixPrincipiAttivi AS SELECT * FROM $(MetaschemaDB).dbo.InsettiUtilixPrincipiAttivi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Tipologia_Budget')
	DROP VIEW Tipologia_Budget
GO
CREATE VIEW Tipologia_Budget AS select * from $(MetaschemaDB).dbo.Tipologia_Budget WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Tecnologie_Sementi')
	DROP VIEW Tecnologie_Sementi
GO
CREATE VIEW Tecnologie_Sementi AS SELECT * FROM $(MetaschemaDB).dbo.Tecnologie_Sementi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Valutazione_Gruppo')
	DROP VIEW Valutazione_Gruppo
GO
CREATE VIEW  Valutazione_Gruppo  AS SELECT * FROM $(MetaschemaDB).dbo.Valutazione_Gruppo WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Valutazione_Sezione')
	DROP VIEW Valutazione_Sezione
GO
CREATE VIEW  Valutazione_Sezione  AS SELECT * FROM $(MetaschemaDB).dbo.Valutazione_Sezione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Valutazione_Conto')
	DROP VIEW Valutazione_Conto
GO
CREATE VIEW  Valutazione_Conto  AS SELECT * FROM $(MetaschemaDB).dbo.Valutazione_Conto WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Prenotazione_Piante_Calibro')
	DROP VIEW Prenotazione_Piante_Calibro
GO
CREATE VIEW Prenotazione_Piante_Calibro AS SELECT * FROM $(MetaschemaDB).dbo.Prenotazione_Piante_Calibro WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'SpecieVegetali_DefaultGlobali')
	DROP VIEW SpecieVegetali_DefaultGlobali
GO
CREATE VIEW SpecieVegetali_DefaultGlobali AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetali_DefaultGlobali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'FasiCicloColturale_Anagrafiche')
	DROP VIEW FasiCicloColturale_Anagrafiche
GO
CREATE VIEW FasiCicloColturale_Anagrafiche AS SELECT * FROM $(MetaschemaDB).dbo.FasiCicloColturale_Anagrafiche WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'FasiCicloColturalexSpecieVegetali')
	DROP VIEW FasiCicloColturalexSpecieVegetali
GO
CREATE VIEW FasiCicloColturalexSpecieVegetali AS SELECT * FROM $(MetaschemaDB).dbo.FasiCicloColturalexSpecieVegetali WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'PrincipiAttiviXPrincipiAttivi_Contesto')
	DROP VIEW PrincipiAttiviXPrincipiAttivi_Contesto
GO
CREATE VIEW PrincipiAttiviXPrincipiAttivi_Contesto AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttiviXPrincipiAttivi_Contesto WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'PrincipiAttivi_Contesto')
	DROP VIEW PrincipiAttivi_Contesto
GO
CREATE VIEW PrincipiAttivi_Contesto AS SELECT * FROM $(MetaschemaDB).dbo.PrincipiAttivi_Contesto WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Lista_Patologie')
	DROP VIEW Lista_Patologie
GO
CREATE VIEW Lista_Patologie AS select * from $(MetaschemaDB).dbo.Lista_Patologie WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'BDN_Causali')
	DROP VIEW BDN_Causali
GO
CREATE VIEW BDN_Causali AS select * from $(MetaschemaDB).dbo.BDN_Causali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Zoo_Animali_Anomalie')
	DROP VIEW Zoo_Animali_Anomalie
GO
CREATE VIEW Zoo_Animali_Anomalie AS select * from $(MetaschemaDB).dbo.Zoo_Animali_Anomalie WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Zoo_Barcode')
    DROP VIEW Zoo_Barcode
GO
CREATE VIEW Zoo_Barcode AS select * from $(MetaschemaDB).dbo.Zoo_Barcode WITH(NOLOCK)
GO

-- ==============================================================================
-- =====  Viste Guida Impostazioni
-- ==============================================================================
IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni')
    DROP VIEW Guida_Impostazioni
GO
CREATE VIEW Guida_Impostazioni AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni_Sezioni')
    DROP VIEW Guida_Impostazioni_Sezioni
GO
CREATE VIEW Guida_Impostazioni_Sezioni AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni_Sezioni WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni_Valori')
    DROP VIEW Guida_Impostazioni_Valori
GO
CREATE VIEW Guida_Impostazioni_Valori AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni_Valori WITH(NOLOCK)
GO


IF EXISTS (select 1 from sys.views where name = 'GIS_LayerAnalysisConfig_Checklist_Type')
    DROP VIEW GIS_LayerAnalysisConfig_Checklist_Type
GO
CREATE VIEW GIS_LayerAnalysisConfig_Checklist_Type AS select * from $(MetaschemaDB).dbo.GIS_LayerAnalysisConfig_Checklist_Type WITH(NOLOCK)
GO

if EXISTS (select 1 from sys.views where name = 'Modalita_Applicazione_Globali')
	DROP VIEW Modalita_Applicazione_Globali
GO
CREATE VIEW Modalita_Applicazione_Globali AS SELECT * FROM $(MetaschemaDB).dbo.Modalita_Applicazione_Globali WITH(NOLOCK)
GO

if EXISTS (select 1 from sys.views where name = 'Modalita_Applicazione_Globali_Operazioni')
	DROP VIEW Modalita_Applicazione_Globali_Operazioni
GO
CREATE VIEW Modalita_Applicazione_Globali_Operazioni AS SELECT * FROM $(MetaschemaDB).dbo.Modalita_Applicazione_Globali_Operazioni WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Modalita_Applicazione_Globali_XLingue')
	DROP VIEW Modalita_Applicazione_Globali_XLingue
GO
CREATE VIEW  Modalita_Applicazione_Globali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Modalita_Applicazione_Globali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Categorie_Animali_XLingue')
	DROP VIEW Lista_Categorie_Animali_XLingue
GO
CREATE VIEW  Lista_Categorie_Animali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Categorie_Animali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Causali_Morte_XLingue')
	DROP VIEW Lista_Causali_Morte_XLingue
GO
CREATE VIEW  Lista_Causali_Morte_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Causali_Morte_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Generi_Animali_XLingue')
	DROP VIEW Lista_Generi_Animali_XLingue
GO
CREATE VIEW  Lista_Generi_Animali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Generi_Animali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_IndirizziProd_Animali_XLingue')
	DROP VIEW Lista_IndirizziProd_Animali_XLingue
GO
CREATE VIEW  Lista_IndirizziProd_Animali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_IndirizziProd_Animali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Patologie_XLingue')
	DROP VIEW Lista_Patologie_XLingue
GO
CREATE VIEW  Lista_Patologie_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Patologie_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Razze_Animali_XLingue')
	DROP VIEW Lista_Razze_Animali_XLingue
GO
CREATE VIEW  Lista_Razze_Animali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Razze_Animali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Specie_Animali_XLingue')
	DROP VIEW Lista_Specie_Animali_XLingue
GO
CREATE VIEW  Lista_Specie_Animali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Specie_Animali_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Lista_Tipi_Raggruppamento_Stalla_XLingue')
	DROP VIEW Lista_Tipi_Raggruppamento_Stalla_XLingue
GO
CREATE VIEW  Lista_Tipi_Raggruppamento_Stalla_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Lista_Tipi_Raggruppamento_Stalla_XLingue WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'Contributi')
	DROP VIEW Contributi
GO
CREATE VIEW  Contributi AS SELECT * FROM $(MetaschemaDB).dbo.Contributi WITH(NOLOCK)
GO

IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'FarmacixUnitaMisura')
	DROP VIEW FarmacixUnitaMisura
GO
CREATE VIEW  FarmacixUnitaMisura AS SELECT * FROM $(MetaschemaDB).dbo.FarmacixUnitaMisura WITH(NOLOCK)
GO

--LAST ServerDB (!! INSERIRE SOPRA ||)

-------------------------------------------------------------------------------------------------


/* ########################################################################################## */
/* ########################################################################################## */
/*	            AGGANCIO A Utenti_DB								        */
/* ########################################################################################## */
/* ########################################################################################## */


if exists (select 1 from sys.views where name = 'Lingue')
	DROP VIEW Lingue
GO
CREATE VIEW Lingue as SELECT * FROM $(UtentiDB).dbo.Lingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Utenti_Impostazioni_FiltroMono')
	DROP VIEW Utenti_Impostazioni_FiltroMono
GO
CREATE VIEW Utenti_Impostazioni_FiltroMono as SELECT * FROM $(UtentiDB).dbo.Utenti_Impostazioni_FiltroMono WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Last_Impresa_Selezionata_Dashboard')
    DROP VIEW Last_Impresa_Selezionata_Dashboard
GO
CREATE VIEW Last_Impresa_Selezionata_Dashboard AS SELECT * FROM $(UtentiDB).dbo.[Utenti_Navigazione_Aziende] WITH(NOLOCK)
GO

/* ################################################################################################################ */
/* ################################################################################################################ */
/*	            Viste legate alle lingue - viene fatto qui perchè viene utilizzata la tabella lingue                */
/* ################################################################################################################ */
/* ################################################################################################################ */


if exists (select 1 from sys.views where name = 'Tabelle_I18N')
BEGIN
	DROP VIEW Tabelle_I18N
END
GO

/* Creazione di tutte le viste XLingua_xx */

CREATE VIEW Tabelle_I18N AS SELECT * FROM $(MetaschemaDB).dbo.Tabelle_I18N WITH(NOLOCK)
GO

BEGIN

	DECLARE Lingue_Cursor CURSOR FOR SELECT Lingua_Cod,CodiceISO FROM Lingue 
									 WHERE Lingua_Cod != 1 -- 1 = Italiano

	DECLARE @Lingua  SMALLINT
	DECLARE @CodiceISO NVARCHAR(5)

	OPEN Lingue_Cursor
	FETCH NEXT FROM Lingue_Cursor INTO @Lingua, @CodiceISO

	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE Tabelle_I18N_Cursor CURSOR FOR 
				SELECT Nome_Tabella_Originale,Nome_Tabella_traduzioni,Campi_da_tradurre,clausola_join,nome_campo_lingua
				FROM $(MetaschemaDB).dbo.Tabelle_I18N
				WHERE clausola_join is not null And Crea_Su_DBServer = 1

		DECLARE @Nome_Tabella_Originale NVARCHAR(100)
		DECLARE @Nome_Tabella_traduzioni NVARCHAR(100)
		DECLARE @Campi_da_tradurre NVARCHAR(100)
		DECLARE @Clausola_Join NVARCHAR(MAX)
		DECLARE @Nome_Campo_Lingua NVARCHAR(100)

		OPEN Tabelle_I18N_Cursor
		FETCH NEXT FROM Tabelle_I18N_Cursor INTO @Nome_Tabella_Originale, @Nome_Tabella_traduzioni, @Campi_da_tradurre, @Clausola_Join, @Nome_Campo_Lingua

		WHILE @@FETCH_STATUS = 0
		BEGIN

			DECLARE @nomeColonna VARCHAR(100)
			DECLARE @listaColonne NVARCHAR(MAX)
			DECLARE @deleteVista NVARCHAR(100)
			DECLARE @creaVista NVARCHAR(MAX)
			DECLARE @primogiro VARCHAR(20)

			DECLARE nomeColonnaCursor CURSOR FOR
					SELECT cc.name FROM sys.tables tt INNER JOIN sys.columns cc ON tt.object_id=cc.object_id 
					WHERE tt.name =  @Nome_Tabella_Originale
					UNION
					SELECT cc.name FROM sys.views tt INNER JOIN sys.columns cc ON tt.object_id=cc.object_id 
					WHERE tt.name =  @Nome_Tabella_Originale 
        
			OPEN nomeColonnaCursor
			FETCH NEXT FROM nomeColonnaCursor INTO @nomeColonna

			SET @primogiro = 'true'
			SET @listaColonne =''

			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF @primogiro = 'false'
				BEGIN
					SET @listaColonne = @listaColonne + N' ,'
				END
       
				IF CHARINDEX( @nomeColonna ,@Campi_da_tradurre) <> 0
					SET @listaColonne = @listaColonne + N' ISNULL(' + @Nome_Tabella_traduzioni + '.'+@nomeColonna+','+@Nome_Tabella_Originale+'.'+@nomeColonna+') AS '+@nomeColonna
				ELSE
					SET @listaColonne = @listaColonne+ N' '+@Nome_Tabella_Originale+'.'+@nomeColonna
				SET @primogiro = 'false'
       
				FETCH NEXT FROM nomeColonnaCursor INTO @nomeColonna 
			END

			DEALLOCATE nomeColonnaCursor
			
			IF @listaColonne = ''
			BEGIN
				PRINT CHAR(10) + 'ERRORE: listaColonne è vuota per la tabella ' + @Nome_Tabella_Originale;
			END

			-- Cancello la vista
			IF EXISTS (select 1 from sys.views where name = + @Nome_Tabella_traduzioni + '_' + @CodiceISO)
			BEGIN
				SET @deleteVista = N'DROP VIEW [' + @Nome_Tabella_traduzioni + '_' + @CodiceISO  + ']'

				BEGIN TRY
					EXEC sp_executesql @deleteVista;
				END TRY
				BEGIN CATCH
					PRINT CHAR(10) + 'ERRORE DROP View: ' + ERROR_MESSAGE();
					PRINT @deleteVista;
				END CATCH
			END
				 
			SET @creaVista = N'CREATE VIEW [' + @Nome_Tabella_traduzioni + '_' + @CodiceISO + ']' +
							' AS (SELECT '+ @listaColonne+
							' FROM ['+@Nome_Tabella_Originale  + ']' +
							'  WITH(NOLOCK) LEFT OUTER JOIN ['+@Nome_Tabella_traduzioni+ ']  WITH(NOLOCK) ON '+ @Clausola_Join +  
							'  AND  ' + @Nome_Tabella_traduzioni + '.' + @Nome_Campo_Lingua + ' = '+
							CONVERT(VARCHAR, @Lingua)   
							+ ' )'
 
			BEGIN TRY
				--print(@creaVista)
				EXEC sp_executesql @creaVista;
			END TRY
			BEGIN CATCH
				PRINT CHAR(10) + 'ERRORE CREA View: ' + ERROR_MESSAGE();
				PRINT @creaVista;
			END CATCH				 

			FETCH NEXT FROM Tabelle_I18N_Cursor INTO @Nome_Tabella_Originale,@Nome_Tabella_traduzioni,@Campi_da_tradurre,@Clausola_Join, @Nome_Campo_Lingua

		END
		DEALLOCATE Tabelle_I18N_Cursor

		FETCH NEXT FROM Lingue_Cursor INTO @Lingua, @CodiceISO

	END
	DEALLOCATE Lingue_Cursor

END
GO






/* ########################################################################################## */
/* ########################################################################################## */
/*	            AGGANCIO A GIAS_PianoConcimazione_PUA								  	      */
/* ########################################################################################## */
/* ########################################################################################## */

if not exists (select 1 from Versione_Database where versione = 'Aggancio $(PUADB) v.$(VersioneAggancio)')
	INSERT INTO Versione_Database  (Versione, Username_Creazione, Username_Modifica)
		    VALUES     ('Aggancio $(PUADB) v.$(VersioneAggancio)', '$(PUADB) v.$(VersioneAggancio)', '$(PUADB) v.$(VersioneAggancio)')
else
	UPDATE Versione_Database SET data_modifica = getDate() WHERE versione = 'Aggancio $(PUADB) v.$(VersioneAggancio)'
GO


-- ==============================================================================
-- =====
-- =====  Viste Condivise tra Piano Concimazione e Pua
-- =====
-- ==============================================================================
if exists (select 1 from sys.views where name = 'FasiCicloColturalexGruppoFinalita')
	DROP VIEW FasiCicloColturalexGruppoFinalita
GO
CREATE VIEW FasiCicloColturalexGruppoFinalita AS SELECT * FROM $(PUADB).dbo.FasiCicloColturalexGruppoFinalita WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LimitiAzotoxSpecie')
	DROP VIEW LimitiAzotoxSpecie
GO
CREATE VIEW LimitiAzotoxSpecie AS SELECT * FROM $(PUADB).dbo.LimitiAzotoxSpecie WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'PUA_Regolamenti')
	DROP VIEW PUA_Regolamenti
GO
CREATE VIEW PUA_Regolamenti AS SELECT * FROM $(PUADB).dbo.PUA_Regolamenti WITH(NOLOCK)
GO

-- ==============================================================================
-- =====
-- =====  Viste del Piano Concimazione
-- =====
-- ==============================================================================

if exists (select 1 from sys.views where name = 'AnnixFasiDelCiclo')
	DROP VIEW AnnixFasiDelCiclo 
go
CREATE VIEW AnnixFasiDelCiclo AS SELECT * FROM $(PUADB).dbo.AnnixFasiDelCiclo WITH(NOLOCK)
go
 
if exists (select 1 from sys.views where name = 'CaratteristicheColture')
	DROP VIEW CaratteristicheColture 
go
CREATE VIEW CaratteristicheColture AS SELECT * FROM $(PUADB).dbo.CaratteristicheColture WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Celle_Meteo')
	DROP VIEW Celle_Meteo 
go
CREATE VIEW Celle_Meteo AS SELECT * FROM $(PUADB).dbo.Celle_Meteo WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'ClassiTessitura')
	DROP VIEW ClassiTessitura
go
CREATE VIEW ClassiTessitura AS SELECT * FROM $(PUADB).dbo.ClassiTessitura WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'CoefficenteAssorbimento')
	DROP VIEW CoefficenteAssorbimento 
go
CREATE VIEW CoefficenteAssorbimento AS SELECT * FROM $(PUADB).dbo.CoefficenteAssorbimento WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'CoefficenteEfficenza')
	DROP VIEW CoefficenteEfficenza
go
CREATE VIEW CoefficenteEfficenza AS SELECT * FROM $(PUADB).dbo.CoefficenteEfficenza WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'CoefficenteMinimoSoN')
	DROP VIEW CoefficenteMinimoSoN 
go
CREATE VIEW CoefficenteMinimoSoN AS SELECT * FROM $(PUADB).dbo.CoefficenteMinimoSoN WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'CoefficenteMinSal')
	DROP VIEW CoefficenteMinSal
go 
CREATE VIEW CoefficenteMinSal AS SELECT * FROM $(PUADB).dbo.CoefficenteMinSal WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'CoefficenteRec')
	DROP VIEW CoefficenteRec
go 
CREATE VIEW CoefficenteRec AS SELECT * FROM $(PUADB).dbo.CoefficenteRec WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'DoseLivelloConcime')
	DROP VIEW DoseLivelloConcime
go 
CREATE VIEW DoseLivelloConcime AS SELECT * FROM $(PUADB).dbo.DoseLivelloConcime WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EfficienzaLiquami')
	DROP VIEW EfficienzaLiquami 
go
CREATE VIEW EfficienzaLiquami AS SELECT * FROM $(PUADB).dbo.EfficienzaLiquami WITH(NOLOCK) 
go

if exists (select 1 from sys.views where name = 'EfficienzaLiquamixConcimazione')
	DROP VIEW EfficienzaLiquamixConcimazione 
go 
CREATE VIEW EfficienzaLiquamixConcimazione AS SELECT * FROM $(PUADB).dbo.EfficienzaLiquamixConcimazione	WITH(NOLOCK) 
go

if exists (select 1 from sys.views where name = 'Epoche_RER')
	DROP VIEW Epoche_RER
go 
CREATE VIEW Epoche_RER AS SELECT * FROM $(PUADB).dbo.Epoche_RER WITH(NOLOCK)
go 

if exists (select 1 from sys.views where name = 'EpochexFasiDelCiclo')
	DROP VIEW EpochexFasiDelCiclo 
go 
CREATE VIEW EpochexFasiDelCiclo AS SELECT * FROM $(PUADB).dbo.EpochexFasiDelCiclo WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EpochexSpecieVegetali')
	DROP VIEW EpochexSpecieVegetali 
go
CREATE VIEW EpochexSpecieVegetali AS SELECT * FROM $(PUADB).dbo.EpochexSpecieVegetali WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'FasiCicloColturale')
	DROP VIEW FasiCicloColturale
go 
CREATE VIEW FasiCicloColturale AS SELECT * FROM $(PUADB).dbo.FasiCicloColturale WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'FattoriCorrettivi')
	DROP VIEW FattoriCorrettivi
GO 
CREATE VIEW FattoriCorrettivi AS SELECT * FROM $(PUADB).dbo.FattoriCorrettivi WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'FattoriCorrettivixSpecie')
	DROP VIEW FattoriCorrettivixSpecie
GO
CREATE VIEW FattoriCorrettivixSpecie AS SELECT * FROM $(PUADB).dbo.FattoriCorrettivixSpecie WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'GrigliaBilancioSO')
	DROP VIEW GrigliaBilancioSO 
go 
CREATE VIEW GrigliaBilancioSO AS SELECT * FROM $(PUADB).dbo.GrigliaBilancioSO WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Grigliak2o')
	DROP VIEW Grigliak2o 
go 
CREATE VIEW Grigliak2o AS SELECT * FROM $(PUADB).dbo.Grigliak2o WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'GrigliaP2O5')
	DROP VIEW GrigliaP2O5 
go 
CREATE VIEW GrigliaP2O5 AS SELECT * FROM $(PUADB).dbo.GrigliaP2O5 WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'GrigliaSo')
	DROP VIEW GrigliaSo 
go
CREATE VIEW GrigliaSo AS SELECT * FROM $(PUADB).dbo.GrigliaSo WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'GruppoFinalita_Rer')
	DROP VIEW GruppoFinalita_Rer
go 
CREATE VIEW GruppoFinalita_Rer AS SELECT * FROM $(PUADB).dbo.GruppoFinalita_Rer WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'GruppoFinalitaxSpecieVegetalixConc')
	DROP VIEW GruppoFinalitaxSpecieVegetalixConc 
go 
CREATE VIEW GruppoFinalitaxSpecieVegetalixConc AS SELECT * FROM $(PUADB).dbo.GruppoFinalitaxSpecieVegetalixConc WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LimitiConcimazionexSpecieVegetali')
	DROP VIEW LimitiConcimazionexSpecieVegetali
go 
CREATE VIEW LimitiConcimazionexSpecieVegetali AS SELECT * FROM $(PUADB).dbo.LimitiConcimazionexSpecieVegetali WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Lisciviazione')
	DROP VIEW Lisciviazione 
go 
CREATE VIEW Lisciviazione AS SELECT * FROM $(PUADB).dbo.Lisciviazione WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'MaxAmmendanti')
	DROP VIEW MaxAmmendanti
go 
CREATE VIEW MaxAmmendanti AS SELECT * FROM $(PUADB).dbo.MaxAmmendanti WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'ModalitaxEpoca')
	DROP VIEW ModalitaxEpoca 
go
CREATE VIEW ModalitaxEpoca AS SELECT * FROM $(PUADB).dbo.ModalitaxEpoca	WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'ModalitaEpochexSpecieVegetali')
	DROP VIEW ModalitaEpochexSpecieVegetali 
go 
CREATE VIEW ModalitaEpochexSpecieVegetali AS SELECT * FROM $(PUADB).dbo.ModalitaEpochexSpecieVegetali WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Precessione')
	DROP VIEW Precessione
go 
CREATE VIEW Precessione AS SELECT * FROM $(PUADB).dbo.Precessione WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'RapportoCn')
	DROP VIEW RapportoCn 
go 
CREATE VIEW RapportoCn AS SELECT * FROM $(PUADB).dbo.RapportoCn WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'SpecieConcimazione')
	DROP VIEW SpecieConcimazione 
go 
CREATE VIEW SpecieConcimazione AS SELECT * FROM $(PUADB).dbo.SpecieConcimazione WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'TipoFertilizzante')
	DROP VIEW TipoFertilizzante
go
CREATE VIEW TipoFertilizzante AS SELECT * FROM $(PUADB).dbo.TipoFertilizzante WITH(NOLOCK)
go 

if exists (select 1 from sys.views where name = 'TipoTerreno')
	DROP VIEW TipoTerreno
go
CREATE VIEW TipoTerreno AS SELECT * FROM $(PUADB).dbo.TipoTerreno WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'PC_CicloColturale')
	DROP VIEW PC_CicloColturale
GO
CREATE VIEW PC_CicloColturale AS SELECT * FROM $(PUADB).dbo.PC_CicloColturale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_ClassiTessitura')
	DROP VIEW PC_ClassiTessitura
GO
CREATE VIEW PC_ClassiTessitura AS SELECT * FROM $(PUADB).dbo.PC_ClassiTessitura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_CoefficenteMinimoSoN')
	DROP VIEW PC_CoefficenteMinimoSoN
GO
CREATE VIEW PC_CoefficenteMinimoSoN AS SELECT * FROM $(PUADB).dbo.PC_CoefficenteMinimoSoN WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_DisponibilitaOssigeno')
	DROP VIEW PC_DisponibilitaOssigeno
GO
CREATE VIEW PC_DisponibilitaOssigeno AS SELECT * FROM $(PUADB).dbo.PC_DisponibilitaOssigeno WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Dotazione')
	DROP VIEW PC_Dotazione
GO
CREATE VIEW PC_Dotazione AS SELECT * FROM $(PUADB).dbo.PC_Dotazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Fabbisogni')
	DROP VIEW PC_Fabbisogni
GO
CREATE VIEW PC_Fabbisogni AS SELECT * FROM $(PUADB).dbo.PC_Fabbisogni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_FasiCicloColturale')
	DROP VIEW PC_FasiCicloColturale
GO
CREATE VIEW PC_FasiCicloColturale AS SELECT * FROM $(PUADB).dbo.PC_FasiCicloColturale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_FasiCicloColturalexGruppoVegetale')
	DROP VIEW PC_FasiCicloColturalexGruppoVegetale
GO
CREATE VIEW PC_FasiCicloColturalexGruppoVegetale AS SELECT * FROM $(PUADB).dbo.PC_FasiCicloColturalexGruppoVegetale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Frequenza')
	DROP VIEW PC_Frequenza
GO
CREATE VIEW PC_Frequenza AS SELECT * FROM $(PUADB).dbo.PC_Frequenza WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_GrigliaK2O')
	DROP VIEW PC_GrigliaK2O
GO
CREATE VIEW PC_GrigliaK2O AS SELECT * FROM $(PUADB).dbo.PC_GrigliaK2O WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_GrigliaP2O5')
	DROP VIEW PC_GrigliaP2O5
GO
CREATE VIEW PC_GrigliaP2O5 AS SELECT * FROM $(PUADB).dbo.PC_GrigliaP2O5 WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_GrigliaSO')
	DROP VIEW PC_GrigliaSO
GO
CREATE VIEW PC_GrigliaSO AS SELECT * FROM $(PUADB).dbo.PC_GrigliaSO WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_GruppiTessitura')
	DROP VIEW PC_GruppiTessitura
GO
CREATE VIEW PC_GruppiTessitura AS SELECT * FROM $(PUADB).dbo.PC_GruppiTessitura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_GruppoFinalitaxSpecieVegetali')
	DROP VIEW PC_GruppoFinalitaxSpecieVegetali
GO
CREATE VIEW PC_GruppoFinalitaxSpecieVegetali AS SELECT * FROM $(PUADB).dbo.PC_GruppoFinalitaxSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Liscivazione')
	DROP VIEW PC_Liscivazione
GO
CREATE VIEW PC_Liscivazione AS SELECT * FROM $(PUADB).dbo.PC_Liscivazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_MatriciOrganiche')
	DROP VIEW PC_MatriciOrganiche
GO
CREATE VIEW PC_MatriciOrganiche AS SELECT * FROM $(PUADB).dbo.PC_MatriciOrganiche WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_MatriciOrganicheXFrequenza')
	DROP VIEW PC_MatriciOrganicheXFrequenza
GO
CREATE VIEW PC_MatriciOrganicheXFrequenza AS SELECT * FROM $(PUADB).dbo.PC_MatriciOrganicheXFrequenza WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_PerditeImmobilizzazioniDispersioni')
	DROP VIEW PC_PerditeImmobilizzazioniDispersioni
GO
CREATE VIEW PC_PerditeImmobilizzazioniDispersioni AS SELECT * FROM $(PUADB).dbo.PC_PerditeImmobilizzazioniDispersioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_PrecessioneColturale')
	DROP VIEW PC_PrecessioneColturale
GO
CREATE VIEW PC_PrecessioneColturale AS SELECT * FROM $(PUADB).dbo.PC_PrecessioneColturale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_RapportoCN')
	DROP VIEW PC_RapportoCN
GO
CREATE VIEW PC_RapportoCN AS SELECT * FROM $(PUADB).dbo.PC_RapportoCN WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_SpecieConcimazione')
	DROP VIEW PC_SpecieConcimazione
GO
CREATE VIEW PC_SpecieConcimazione AS SELECT * FROM $(PUADB).dbo.PC_SpecieConcimazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Tessiture')
	DROP VIEW PC_Tessiture
GO
CREATE VIEW PC_Tessiture AS SELECT * FROM $(PUADB).dbo.PC_Tessiture WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_TriangoloTessitura')
	DROP VIEW PC_TriangoloTessitura
GO
CREATE VIEW PC_TriangoloTessitura AS SELECT * FROM $(PUADB).dbo.PC_TriangoloTessitura WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PC_Ubicazione')
	DROP VIEW PC_Ubicazione
GO
CREATE VIEW PC_Ubicazione AS SELECT * FROM $(PUADB).dbo.PC_Ubicazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'DatiColture_Raccolta')
	DROP VIEW DatiColture_Raccolta
GO
CREATE VIEW DatiColture_Raccolta AS SELECT * FROM $(PUADB).dbo.DatiColture_Raccolta WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Densita_Apparente')
	DROP VIEW Densita_Apparente
GO
CREATE VIEW Densita_Apparente AS SELECT * FROM $(PUADB).dbo.Densita_Apparente WITH(NOLOCK)
GO

-- ==============================================================================
-- =====
-- =====  Viste del Pua
-- =====
-- ==============================================================================
if exists (select 1 from sys.views where name = 'AzotoResiduoxSpecie')
	DROP VIEW AzotoResiduoxSpecie
GO
CREATE VIEW AzotoResiduoxSpecie AS SELECT * FROM $(PUADB).dbo.AzotoResiduoxSpecie WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'ClassiTessituraB')
	DROP VIEW ClassiTessituraB
GO
CREATE VIEW ClassiTessituraB AS SELECT * FROM $(PUADB).dbo.ClassiTessituraB WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Efficienza')
	DROP VIEW Efficienza
GO
CREATE VIEW Efficienza AS SELECT * FROM $(PUADB).dbo.Efficienza WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EfficienzaRiferimentoxTipiAllevamentixEffluenti')
	DROP VIEW EfficienzaRiferimentoxTipiAllevamentixEffluenti
GO
CREATE VIEW EfficienzaRiferimentoxTipiAllevamentixEffluenti AS SELECT * FROM $(PUADB).dbo.EfficienzaRiferimentoxTipiAllevamentixEffluenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'EfficienzaxCategoriaAllevamenti')
	DROP VIEW EfficienzaxCategoriaAllevamenti
GO
CREATE VIEW EfficienzaxCategoriaAllevamenti AS SELECT * FROM $(PUADB).dbo.EfficienzaxCategoriaAllevamenti WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EfficienzaxTipiAllevamentixEffluenti')
	DROP VIEW EfficienzaxTipiAllevamentixEffluenti
GO
CREATE VIEW EfficienzaxTipiAllevamentixEffluenti AS SELECT * FROM $(PUADB).dbo.EfficienzaxTipiAllevamentixEffluenti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'EfficienzaxTipoFertilizzante')
	DROP VIEW EfficienzaxTipoFertilizzante
GO
CREATE VIEW EfficienzaxTipoFertilizzante AS SELECT * FROM $(PUADB).dbo.EfficienzaxTipoFertilizzante WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Effluenti')
	DROP VIEW Effluenti
GO
CREATE VIEW Effluenti AS SELECT * FROM $(PUADB).dbo.Effluenti WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EffluentixFertilizzanti')
	DROP VIEW EffluentixFertilizzanti 
GO
CREATE VIEW EffluentixFertilizzanti AS SELECT * FROM $(PUADB).dbo.EffluentixFertilizzanti WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'EpocaColturale')
	DROP VIEW EpocaColturale
GO
CREATE VIEW EpocaColturale AS SELECT * FROM $(PUADB).dbo.EpocaColturale WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'EpocheModalita')
	DROP VIEW EpocheModalita
GO
CREATE VIEW EpocheModalita AS SELECT * FROM $(PUADB).dbo.EpocheModalita WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'FertilizzantixTipoOrganici')
	DROP VIEW FertilizzantixTipoOrganici 
go 
CREATE VIEW FertilizzantixTipoOrganici AS SELECT * FROM $(PUADB).dbo.FertilizzantixTipoOrganici WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Frequenza')
	DROP VIEW Frequenza
GO
CREATE VIEW Frequenza AS SELECT * FROM $(PUADB).dbo.Frequenza WITH(NOLOCK)
go


if exists (select 1 from sys.views where name = 'GruppoFinalitaxSpeciexEpoca')
	DROP VIEW GruppoFinalitaxSpeciexEpoca
GO
CREATE VIEW GruppoFinalitaxSpeciexEpoca AS SELECT * FROM $(PUADB).dbo.GruppoFinalitaxSpeciexEpoca WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LiquamiMungituraxConsistenza')
	DROP VIEW LiquamiMungituraxConsistenza
GO
CREATE VIEW LiquamiMungituraxConsistenza AS SELECT * FROM $(PUADB).dbo.LiquamiMungituraxConsistenza WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LiquamiTrattamenti')
	DROP VIEW LiquamiTrattamenti
GO
CREATE VIEW LiquamiTrattamenti AS SELECT * FROM $(PUADB).dbo.LiquamiTrattamenti WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LiquamiTrattamentiEfficienza')
	DROP VIEW LiquamiTrattamentiEfficienza
GO
CREATE VIEW LiquamiTrattamentiEfficienza AS SELECT * FROM $(PUADB).dbo.LiquamiTrattamentiEfficienza WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'LiquamiTrattamentixSpecie')
	DROP VIEW LiquamiTrattamentixSpecie
GO
CREATE VIEW LiquamiTrattamentixSpecie AS SELECT * FROM $(PUADB).dbo.LiquamiTrattamentixSpecie WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Lista_Categorie_Animali_Attributi')
	DROP VIEW Lista_Categorie_Animali_Attributi
GO
CREATE VIEW Lista_Categorie_Animali_Attributi AS SELECT * FROM $(PUADB).dbo.Lista_Categorie_Animali_Attributi WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Lista_Tipi_Allevamenti')
	DROP VIEW Lista_Tipi_Allevamenti
GO
CREATE VIEW Lista_Tipi_Allevamenti AS SELECT * FROM $(PUADB).dbo.Lista_Tipi_Allevamenti WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'Lista_TipiStallaxCategorie')
	DROP VIEW Lista_TipiStallaxCategorie
GO
CREATE VIEW Lista_TipiStallaxCategorie AS SELECT * FROM $(PUADB).dbo.Lista_TipiStallaxCategorie WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'MatriciOrganiche')
	DROP VIEW MatriciOrganiche
GO
CREATE VIEW MatriciOrganiche AS SELECT * FROM $(PUADB).dbo.MatriciOrganiche WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'MatriciOrganichexFrequenza')
	DROP VIEW MatriciOrganichexFrequenza
GO
CREATE VIEW MatriciOrganichexFrequenza AS SELECT * FROM $(PUADB).dbo.MatriciOrganichexFrequenza WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'ParticelleCatastali_Vulnerabili')
	DROP VIEW ParticelleCatastali_Vulnerabili
GO
CREATE VIEW ParticelleCatastali_Vulnerabili AS SELECT * FROM $(PUADB).dbo.ParticelleCatastali_Vulnerabili WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'PossibilitaDistribuzione')
	DROP VIEW PossibilitaDistribuzione
GO
CREATE VIEW PossibilitaDistribuzione AS SELECT * FROM $(PUADB).dbo.PossibilitaDistribuzione WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'PrecessioneColturale')
	DROP VIEW PrecessioneColturale
GO
CREATE VIEW PrecessioneColturale AS SELECT * FROM $(PUADB).dbo.PrecessioneColturale WITH(NOLOCK)
go

if exists (select 1 from sys.views where name = 'StabulazionexCategoria')
	DROP VIEW StabulazionexCategoria
GO
CREATE VIEW StabulazionexCategoria AS SELECT * FROM $(PUADB).dbo.StabulazionexCategoria WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TrattamentoEffluenti')
	DROP VIEW TrattamentoEffluenti
GO
CREATE VIEW TrattamentoEffluenti AS SELECT * FROM $(PUADB).dbo.TrattamentoEffluenti WITH(NOLOCK)
GO

--LAST PuaDB 

/* ########################################################################################## */
/* ########################################################################################## */
/* ########################################################################################## */
/* ########################################################################################## */



/* ########################################################################################## */
/* #########                       Database Utenti                  ######################### */
/* ########################################################################################## */


USE $(UtentiDB)
go

PRINT CHAR(10) + ' ++ DB = $(UtentiDB)'
GO

if exists (select 1 from sys.views where name = 'GruppoVegetale')
	DROP VIEW GruppoVegetale
GO
CREATE VIEW  GruppoVegetale AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVegetale WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'SpecieVegetali')
	DROP VIEW SpecieVegetali
GO
CREATE VIEW  SpecieVegetali AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Cultivar')
	DROP VIEW Cultivar
GO
CREATE VIEW  Cultivar AS SELECT * FROM $(MetaschemaDB).dbo.Cultivar WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementi')
	DROP VIEW TipologieSementi
GO
CREATE VIEW  TipologieSementi AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementixSpecieVegetali')
	DROP VIEW TipologieSementixSpecieVegetali
GO
CREATE VIEW  TipologieSementixSpecieVegetali AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementixSpecieVegetali WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Operazioni')
	DROP VIEW Operazioni
GO
CREATE VIEW  Operazioni AS SELECT * FROM $(MetaschemaDB).dbo.Operazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoOperazioni')
	DROP VIEW GruppoOperazioni
GO
CREATE VIEW GruppoOperazioni AS SELECT * FROM $(MetaschemaDB).dbo.GruppoOperazioni WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'Servizi_Pratiche')
	DROP VIEW Servizi_Pratiche
GO
CREATE VIEW Servizi_Pratiche AS SELECT * FROM $(MetaschemaDB).dbo.Servizi WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'WTransizioniDiStatoConfigurazione')
	DROP VIEW WTransizioniDiStatoConfigurazione
GO
CREATE VIEW WTransizioniDiStatoConfigurazione AS SELECT * FROM $(MetaschemaDB).dbo.WTransizioniDiStatoConfigurazione WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'WAnagraficaStati')
	DROP VIEW WAnagraficaStati
GO
CREATE VIEW WAnagraficaStati AS SELECT * FROM $(MetaschemaDB).dbo.WAnagraficaStati WITH(NOLOCK)
GO

-- ==============================================================================
-- =====  Viste Guida Impostazioni
-- ==============================================================================
IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni')
    DROP VIEW Guida_Impostazioni
GO
CREATE VIEW Guida_Impostazioni AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni_Sezioni')
    DROP VIEW Guida_Impostazioni_Sezioni
GO
CREATE VIEW Guida_Impostazioni_Sezioni AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni_Sezioni WITH(NOLOCK)
GO

IF EXISTS (select 1 from sys.views where name = 'Guida_Impostazioni_Valori')
    DROP VIEW Guida_Impostazioni_Valori
GO
CREATE VIEW Guida_Impostazioni_Valori AS select * from $(MetaschemaDB).dbo.Guida_Impostazioni_Valori WITH(NOLOCK)
GO


/* ############################################################### */
/*	            Viste legate alle lingue sul DB Utenti             */
/* ##############################################################  */

if exists (select 1 from sys.views where name = 'Cultivar_XLingue')
	DROP VIEW Cultivar_XLingue
GO
CREATE VIEW Cultivar_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.Cultivar_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'GruppoVegetale_XLingue')
	DROP VIEW GruppoVegetale_XLingue
GO
CREATE VIEW GruppoVegetale_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.GruppoVegetale_XLingue WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'Operazioni_XLingue')
	DROP VIEW Operazioni_XLingue
GO
CREATE VIEW Operazioni_XLingue AS SELECT     Lingua_Cod, LAV_COD, LAV_DES, DATA_AGG, inviato, datainvio, Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine FROM         $(MetaschemaDB).dbo.operazioni_XLingue WITH(NOLOCK)
GO


if exists (select 1 from sys.views where name = 'SpecieVegetali_XLingue')
	DROP VIEW SpecieVegetali_XLingue
GO
CREATE VIEW SpecieVegetali_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.SpecieVegetali_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'TipologieSementi_XLingue')
	DROP VIEW TipologieSementi_XLingue
GO
CREATE VIEW TipologieSementi_XLingue AS SELECT * FROM $(MetaschemaDB).dbo.TipologieSementi_XLingue WITH(NOLOCK)
GO

if exists (select 1 from sys.views where name = 'gruppoOperazioni_XLingua')
	DROP VIEW gruppoOperazioni_XLingua
GO
CREATE VIEW gruppoOperazioni_XLingua AS SELECT     Lingua_COD, Gru_COD, Gru_DES, inviato, datainvio, Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine FROM         $(MetaschemaDB).dbo.gruppoOperazioni_XLingua WITH(NOLOCK)
GO
 
/* Creazione di tutte le viste XLingua_xx */
BEGIN
	 
	DECLARE Lingue_Cursor CURSOR FOR SELECT Lingua_Cod, CodiceISO FROM Lingue 
									 WHERE Lingua_Cod != 1 -- 1 = Italiano

	DECLARE @Lingua  SMALLINT
	DECLARE @CodiceISO NVARCHAR(5)

	OPEN Lingue_Cursor
	FETCH NEXT FROM Lingue_Cursor INTO @Lingua, @CodiceISO

	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE Tabelle_I18N_Cursor CURSOR FOR 
				SELECT Nome_Tabella_Originale,Nome_Tabella_traduzioni,Campi_da_tradurre,clausola_join,nome_campo_lingua
				FROM $(MetaschemaDB).dbo.Tabelle_I18N
				WHERE clausola_join is not null and Crea_Su_DBUtenti = 1

		DECLARE @Nome_Tabella_Originale NVARCHAR(100)
		DECLARE @Nome_Tabella_traduzioni NVARCHAR(100)
		DECLARE @Campi_da_tradurre NVARCHAR(100)
		DECLARE @Clausola_Join NVARCHAR(MAX)
		DECLARE @Nome_Campo_Lingua NVARCHAR(100)

		OPEN Tabelle_I18N_Cursor
		FETCH NEXT FROM Tabelle_I18N_Cursor INTO @Nome_Tabella_Originale, @Nome_Tabella_traduzioni, @Campi_da_tradurre, @Clausola_Join, @Nome_Campo_Lingua

		WHILE @@FETCH_STATUS = 0
		BEGIN

			DECLARE @nomeColonna VARCHAR(100)
			DECLARE @listaColonne NVARCHAR(MAX)
			DECLARE @deleteVista NVARCHAR(100)
			DECLARE @creaVista NVARCHAR(MAX)
			DECLARE @primogiro VARCHAR(20)

			DECLARE nomeColonnaCursor CURSOR FOR
					SELECT cc.name FROM sys.tables tt INNER JOIN sys.columns cc ON tt.object_id=cc.object_id 
					WHERE tt.name =  @Nome_Tabella_Originale
					UNION
					SELECT cc.name FROM sys.views tt INNER JOIN sys.columns cc ON tt.object_id=cc.object_id 
					WHERE tt.name =  @Nome_Tabella_Originale 
        
			OPEN nomeColonnaCursor
			FETCH NEXT FROM nomeColonnaCursor INTO @nomeColonna

			SET @primogiro = 'true'
			SET @listaColonne = ''

			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF @primogiro = 'false'
				BEGIN
					SET @listaColonne = @listaColonne + N' ,'
				END
       
				IF CHARINDEX( @nomeColonna ,@Campi_da_tradurre) <> 0
					SET @listaColonne = @listaColonne + N' ISNULL(' + @Nome_Tabella_traduzioni + '.'+@nomeColonna+','+@Nome_Tabella_Originale+'.'+@nomeColonna+') AS '+@nomeColonna
				ELSE
					SET @listaColonne = @listaColonne+ N' '+@Nome_Tabella_Originale+'.'+@nomeColonna
				SET @primogiro = 'false'
       
				FETCH NEXT FROM nomeColonnaCursor INTO @nomeColonna 
			END

			DEALLOCATE nomeColonnaCursor
			
			IF @listaColonne = ''
			BEGIN
				PRINT CHAR(10) + 'ERRORE: listaColonne è vuota per la tabella ' + @Nome_Tabella_Originale;
			END
			
			-- Cancello la vista
			IF EXISTS (select 1 from sys.views where name = + @Nome_Tabella_traduzioni + '_' + @CodiceISO)
			BEGIN
				SET @deleteVista = N'DROP VIEW [' + @Nome_Tabella_traduzioni + '_' + @CodiceISO  + ']'

				BEGIN TRY
					EXEC sp_executesql @deleteVista;
				END TRY
				BEGIN CATCH
					PRINT CHAR(10) + 'ERRORE DROP View: ' + ERROR_MESSAGE();
					PRINT @deleteVista;
				END CATCH
			END
				 
			SET @creaVista = N'CREATE VIEW [' + @Nome_Tabella_traduzioni + '_' + @CodiceISO + ']' +
							' AS (SELECT '+ @listaColonne+
							' FROM ['+@Nome_Tabella_Originale  + ']' +
							'  WITH(NOLOCK) LEFT OUTER JOIN ['+@Nome_Tabella_traduzioni+ ']  WITH(NOLOCK) ON '+ @Clausola_Join +  
							'  AND  [' + @Nome_Tabella_traduzioni + '].' + @Nome_Campo_Lingua + ' = '+
							CONVERT(VARCHAR, @Lingua)   
							+ ' )'
 
			BEGIN TRY
				--print (@creaVista)
				EXEC sp_executesql @creaVista;
			END TRY
			BEGIN CATCH
				PRINT CHAR(10) + 'ERRORE CREA View: ' + ERROR_MESSAGE();
				PRINT @creaVista;
			END CATCH				 

			FETCH NEXT FROM  Tabelle_I18N_Cursor INTO @Nome_Tabella_Originale,@Nome_Tabella_traduzioni,@Campi_da_tradurre,@Clausola_Join, @Nome_Campo_Lingua

		END
		DEALLOCATE Tabelle_I18N_Cursor

		FETCH NEXT FROM Lingue_Cursor INTO @Lingua, @CodiceISO

	END
	DEALLOCATE Lingue_Cursor

END
GO

