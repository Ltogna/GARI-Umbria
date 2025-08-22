Imports AgronicaCoreDataProvider.CostantiPersonalizzate

Public Class EFFabbricati
    Public Shared Function CreateFabbricato(ByRef dal As AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities,
                                            ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                            ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                            ByRef CentroAziendale As AgronicaCoreEntityFramework_POCO.Centri_Aziendali,
                                            ByRef username As String) As AgronicaCoreEntityFramework_POCO.Fabbricati

        Dim base_code As Long
        Dim top_code As Long

        Dim codiciGiasProDal As New AgronicaCoreUtentiDAL.Utenti_CodiciGiasPRO_R
        codiciGiasProDal.Calcola_BaseCode_TopCode(base_code, top_code, objParametri_Utenti)


        Dim fabbr As New AgronicaCoreEntityFramework_POCO.Fabbricati
        fabbr.PIVA = CentroAziendale.PIVA
        fabbr.SA_COD = CentroAziendale.sa_cod
        Dim idGen As New AgronicaCoreDataProvider.Agro_Sequenze
        fabbr.Fabbricato_Cod = idGen.NuovoId_SeqMagazzino(CentroAziendale.PIVA, CentroAziendale.sa_cod, base_code, top_code, objParametri_Server)
        fabbr.Fabbricato_Des = ""
        fabbr.Indirizzo_Cod = 0
        fabbr.Tipo_Fabbricato_Cod = 0
        fabbr.PROV = ""
        fabbr.COM = ""
        fabbr.SEZIONE = ""
        fabbr.FOGLIO = 0
        fabbr.NUMERO = 0
        fabbr.SUBALTERNO = ""
        fabbr.MC_Biologico = 0
        fabbr.MC_Convenzionale = 0
        fabbr.MC_Conversione = 0
        fabbr.Regolamento_Cod = 1
        fabbr.TitoloPossesso = 0
        fabbr.Conversione_Inizio = AGRODATAINIZIO
        fabbr.Conversione_Fine = AGRODATAINIZIO
        fabbr.Idoneo_AutorizSanitaria = 0
        fabbr.Idoneo_CDX_M004 = 0
        fabbr.Idoneo_CondIgieniche = 0
        fabbr.Idoneo_Costruzione = 0
        fabbr.Idoneo_DiagrammiFlusso = 0
        fabbr.Idoneo_HACCP = 0
        fabbr.Idoneo_Layout = 0
        fabbr.Idoneo_Planimetria = 0
        fabbr.Idoneo_SeparazAmbienti = 0
        fabbr.Idoneo_SeparazProdotti = 0
        fabbr.Idoneo_SupMinCoperte = 0
        fabbr.Idoneo_SupMinScoperte = 0
        fabbr.inviato = 0
        fabbr.Data_Creazione = DateTime.Now
        fabbr.Data_Modifica = DateTime.Now
        fabbr.Validita_Inizio = AGRODATAINIZIO
        fabbr.Validita_Fine = AGRODATAFINE
        fabbr.Username_Creazione = username
        fabbr.Username_Modifica = username
        fabbr.Validazione = 0
        fabbr.Data_Validazione = DateTime.Now
        fabbr.UserName_Validazione = ""
        fabbr.MQ_Biologico = 0
        fabbr.MQ_Biologico_Scoperto = 0
        fabbr.MQ_Convenzionale = 0
        fabbr.MQ_Convenzionale_Scoperto = 0
        fabbr.MQ_Conversione = 0
        fabbr.MQ_Conversione_Scoperto = 0
        fabbr.N_Piani = 0
        fabbr.Sup_Piano = 0
        fabbr.Num_Autorizzazione = ""
        fabbr.Data_Richiesta_Autorizzazione = AGRODATAINIZIO
        fabbr.Tipologia_Utilizzo = 0
        fabbr.ChkVirtuale = 0
        dal.Fabbricati.Add(fabbr)
        dal.SaveChanges()
        Return fabbr
    End Function
End Class
