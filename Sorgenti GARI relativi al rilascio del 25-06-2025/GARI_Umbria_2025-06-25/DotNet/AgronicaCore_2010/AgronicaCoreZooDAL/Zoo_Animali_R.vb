
Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.AgronicaCoreParametri


Public Class Zoo_Animali_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Da usare con Selezione_TabellaCompleta
    ''' </summary>
    ''' <param name="PIVA"></param>
    ''' <param name="Sa_Cod"></param>
    ''' <param name="Cod_Progetto"></param>
    ''' <param name="Matricola"></param>
    ''' <param name="Gen_Cod"></param>
    ''' <param name="Spe_Cod"></param>
    ''' <param name="Raz_Cod"></param>
    ''' <param name="Ipro_Cod"></param>
    ''' <param name="Nome"></param>
    ''' <param name="Collare"></param>
    ''' <param name="xSelezioneVariabile"></param>
    ''' <param name="xFiltroAggiuntivo"></param>
    ''' <param name="xOrderBy"></param>
    ''' <param name="objParametri"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[pierantoni]	20/01/2011	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function Leggi( _
                        ByVal PIVA As String, _
                        ByVal Sa_Cod As Integer, _
                        ByVal Cod_Progetto As Integer, _
                        ByVal Matricola As String, _
                        ByVal Gen_Cod As Integer, _
                        ByVal Spe_Cod As Integer, _
                        ByVal Raz_Cod As Integer, _
                        ByVal Ipro_Cod As Integer, _
                        ByVal Nome As String, _
                        ByVal Collare As String, _
                                    ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile, _
                                    ByVal xFiltroAggiuntivo As String, _
                                    ByVal xOrderBy As String, _
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                    ) As DataTable


        Dim NomeRoutine As String = "AgronicaCoreZooDAL.Zoo_Animali_R.Leggi()"



        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            Select Case xSelezioneVariabile

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta
                    StrSQL.Length = 0

                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM  Zoo_Animali ")
                    StrSQL.Append(" WHERE Validita_inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")


                    If PIVA <> "" Then
                        StrSQL.Append(" AND PIVA = '" & Agro_SQL_SaveText(Trim(PIVA)) & "'")
                    End If

                    If Sa_Cod <> 0 Then
                        StrSQL.Append(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & "  ")
                    End If

                    If Cod_Progetto <> 0 Then
                        StrSQL.Append(" AND Cod_Progetto = " & Agro_SQL_SaveNum(Cod_Progetto) & "  ")
                    End If

                    If Matricola <> "" Then
                        StrSQL.Append(" AND Matricola =  '" & Agro_SQL_SaveText(Trim(Matricola)) & "' ")
                    End If

                    If Gen_Cod <> 0 Then
                        StrSQL.Append(" AND GEN_COD = " & Agro_SQL_SaveNum(Gen_Cod) & "  ")
                    End If

                    If Spe_Cod <> 0 Then
                        StrSQL.Append(" AND SPE_COD = " & Agro_SQL_SaveNum(Spe_Cod) & "  ")
                    End If

                    If Raz_Cod <> 0 Then
                        StrSQL.Append(" AND RAZ_COD = " & Agro_SQL_SaveNum(Raz_Cod) & "  ")
                    End If

                    If Ipro_Cod <> 0 Then
                        StrSQL.Append(" AND IPRO_COD = " & Agro_SQL_SaveNum(Ipro_Cod) & "  ")
                    End If

                    If Nome <> "" Then
                        StrSQL.Append(" AND NOME = '" & Agro_SQL_SaveText(Nome) & "'  ")
                    End If

                    If Collare <> "" Then
                        StrSQL.Append(" AND COLLARE = '" & Agro_SQL_SaveText(Collare) & "'  ")
                    End If


                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND     dbo.Zoo_Animali.Inviato >= 0 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND     dbo.Zoo_Animali.Inviato = -1 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------

                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY Spe_Cod, Gen_Cod, IPro_Cod, Raz_Cod, Matricola ASC ")
                    End If

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni



                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta
                    '
                    '
                    '
                    '


            End Select


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function


    Public Function LeggixAnagrafica(
                    ByVal PIVA As String,
                    ByVal Sa_Cod As Integer,
                    ByVal Cod_Progetto As Integer,
                    ByVal Matricola As String,
                    ByVal Gen_Cod As Integer,
                    ByVal Spe_Cod As Integer,
                    ByVal Raz_Cod As Integer,
                    ByVal Ipro_Cod As Integer,
                    ByVal Nome As String,
                    ByVal Collare As String,
                                ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As DataTable


        Dim NomeRoutine As String = "AgronicaCoreZooDAL.Zoo_Animali_R.Leggi()"



        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            Select Case xSelezioneVariabile

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta
                    StrSQL.Length = 0

                    StrSQL.AppendLine("SELECT ")
                    StrSQL.AppendLine("     CONCAT(ZOO_Animali.Piva, '_', ZOO_Animali.sa_cod, '_', ZOO_Animali.Cod_Progetto)  as chiave, ")
                    StrSQL.AppendLine("     Zoo_Animali.*, ")
                    StrSQL.AppendLine("     Lista_Specie_Animali.SPE_DES, ")
                    StrSQL.AppendLine("     Lista_Razze_Animali.RAZ_DES, ")
                    StrSQL.AppendLine("     ZOO_Animali.Validita_Inizio, ")
                    StrSQL.AppendLine("     ZOO_Animali.Validita_Fine, ")
                    StrSQL.AppendLine("     Zoo_Animali_Lista_Tipi.Tipo_Des, ")

                    StrSQL.Append("      ZOO_Animali.Data_Creazione,   ")
                    StrSQL.Append("      ZOO_Animali.Data_Modifica,   ")
                    StrSQL.Append(" ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Creazione), ZOO_Animali.Username_Creazione) AS Utente_Creazione, ")
                    StrSQL.Append(" ISNULL ((SELECT TOP 1 [User] FROM Utenti WHERE CODICE_FISCALE = ZOO_Animali.Username_Modifica), ZOO_Animali.Username_Modifica) AS Utente_Modifica ")

                    StrSQL.AppendLine(" FROM ZOO_Animali ")
                    StrSQL.AppendLine(" Left Join Lista_Specie_Animali ON Zoo_Animali.GEN_COD = Lista_Specie_Animali.GEN_COD  ")
                    StrSQL.AppendLine("                               AND Zoo_Animali.SPE_COD = Lista_Specie_Animali.SPE_COD ")

                    StrSQL.AppendLine(" Left Join Lista_Razze_Animali ON Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD  ")
                    StrSQL.AppendLine("                              AND Zoo_Animali.SPE_COD = Lista_Razze_Animali.SPE_COD  ")
                    StrSQL.AppendLine("                              AND Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD")

                    StrSQL.AppendLine(" Left Join Zoo_Animali_Lista_Tipi ON Zoo_Animali.GEN_COD = Zoo_Animali_Lista_Tipi.GEN_COD  ")
                    StrSQL.AppendLine("                              AND Zoo_Animali.SPE_COD = Zoo_Animali_Lista_Tipi.SPE_COD  ")
                    StrSQL.AppendLine("                              AND Zoo_Animali.TIPO_COD = Zoo_Animali_Lista_Tipi.TIPO_COD ")

                    StrSQL.Append(" WHERE 1=1 ")

                    If PIVA <> "" Then
                        StrSQL.Append(" AND ZOO_Animali.PIVA = '" & Agro_SQL_SaveText(Trim(PIVA)) & "'")
                    End If

                    If Sa_Cod <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & "  ")
                    End If

                    If Cod_Progetto <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.Cod_Progetto = " & Agro_SQL_SaveNum(Cod_Progetto) & "  ")
                    End If

                    If Matricola <> "" Then
                        StrSQL.Append(" AND ZOO_Animali.Matricola =  '" & Agro_SQL_SaveText(Trim(Matricola)) & "' ")
                    End If

                    If Gen_Cod <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.GEN_COD = " & Agro_SQL_SaveNum(Gen_Cod) & "  ")
                    End If

                    If Spe_Cod <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.SPE_COD = " & Agro_SQL_SaveNum(Spe_Cod) & "  ")
                    End If

                    If Raz_Cod <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.RAZ_COD = " & Agro_SQL_SaveNum(Raz_Cod) & "  ")
                    End If

                    If Ipro_Cod <> 0 Then
                        StrSQL.Append(" AND ZOO_Animali.IPRO_COD = " & Agro_SQL_SaveNum(Ipro_Cod) & "  ")
                    End If

                    If Nome <> "" Then
                        StrSQL.Append(" AND ZOO_Animali.NOME = '" & Agro_SQL_SaveText(Nome) & "'  ")
                    End If

                    If Collare <> "" Then
                        StrSQL.Append(" AND ZOO_Animali.COLLARE = '" & Agro_SQL_SaveText(Collare) & "'  ")
                    End If


                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND     dbo.Zoo_Animali.Inviato >= 0 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND     dbo.Zoo_Animali.Inviato = -1 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------

                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY Zoo_Animali.Validita_Inizio,  Zoo_Animali.Matricola ASC ")
                    End If

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni



                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta
                    '
                    '
                    '
                    '


            End Select


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function


    Public Function Leggi_Griglia(ByVal Piva As String,
                                  ByVal Id_Agenda As Integer,
                                  ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                  ) As DataTable
        Dim NomeRoutine As String = "AgronicaCoreZooDAL.Zoo_Animali_R.Leggi_Griglia()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim PivaSuperUser = objParametri.PivaSuperUser

        Try

            StrSQL.Length = 0
            StrSQL.AppendLine(" Select Distinct Movimenti_Dettagli.Id_Mov_Det, Movimenti_Dettagli.Id_Mov_Det as Riga, Movimenti_Dettagli.Id_Mov_Esterno, Zoo_Animali.Cod_Progetto, Zoo_Animali.Raz_Cod, IsNull(Lista_Razze_Animali.RAZ_DES, '') As Raz_Des,  ")
            StrSQL.AppendLine(" Zoo_Animali.Matricola, Zoo_Animali.MAT_MADRE As Matricola_Madre, Zoo_Animali.DAT_NASCITA As Data, Zoo_Animali.Sesso As Sesso_Des, Zoo_Animali.Certificato, Zoo_Animali.Data_Documento_Ingresso as DataCertificato, ")
            StrSQL.AppendLine(" Zoo_Animali.Username_Creazione, Zoo_Animali.Data_Creazione, ")

            StrSQL.AppendLine(" COALESCE(Contatti_FornFatt.Cod_Contatto, '') AS CF_FornFatt, ")
            StrSQL.AppendLine(" COALESCE(Contatti_FornFatt.Rag_Soc + Contatti_FornFatt.Cognome + ' ' + Contatti_FornFatt.Nome, '') AS RagSoc_FornFatt, ")
            StrSQL.AppendLine(" COALESCE(Contatti_FornProv.Cod_Contatto, '') AS CF_FornProv, ")
            StrSQL.AppendLine(" COALESCE(Contatti_FornProv.Rag_Soc + Contatti_FornProv.Cognome + ' ' + Contatti_FornProv.Nome, '') AS RagSoc_FornProv, ")

            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Lotto_Fornitore, '') AS Lotto_Fornitore, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Modello4_Ingresso, '') AS Codice_Modello4_Ingresso, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Modello4_Ingresso_Numero, '') AS N_Modello4_Ingresso, ")

            StrSQL.AppendLine(" COALESCE(Zoo_Animali.N_Bolla_Fornitore, '') AS N_DDT_Ingresso, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.N_Bolla_Uscita, '') AS N_DDT_Uscita, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Data_DDT_Ingresso, CONVERT(datetime, '1900-01-01 00:00:00.000', 120)) AS Data_DDT_Ingresso, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Data_DDT_Uscita, CONVERT(datetime, '2100-12-31 00:00:00.000', 120)) AS Data_DDT_Uscita, ")

            StrSQL.AppendLine(" COALESCE(Movimenti_Dettagli.Prezzo_Unitario, 0) AS Prezzo_Unitario, ")

            StrSQL.AppendLine(" COALESCE(MovDettPeso.Kg_Pagati, 0) AS Qta, ")
            StrSQL.AppendLine(" COALESCE(MovDettPeso.Kg_Arrivo, 0) AS Qta_Arrivo, ")
            StrSQL.AppendLine(" COALESCE(Zoo_Animali.Incremento_Teorico, 0) AS Incremento_Teorico, ")

            StrSQL.AppendLine(" (Select top 1 Codice_Distinta From Zoo_Animali_Distinte Where ")
            StrSQL.AppendLine("                 Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ")
            StrSQL.AppendLine("             And Zoo_Animali.Piva = Movimenti_Dettagli.Piva ")
            StrSQL.AppendLine("             And Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto ")
            StrSQL.AppendLine("             And Zoo_Animali_Distinte.Validita_inizio <= Movimenti.Data_Movimento ")
            StrSQL.AppendLine("             And Zoo_Animali_Distinte.Validita_Fine >= Movimenti.Data_Movimento) as Lotto,  ")

            StrSQL.AppendLine(" (Select top 1 Cod_Progetto From Zoo_Animali_Distinte Where ")
            StrSQL.AppendLine("                 Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ")
            StrSQL.AppendLine("             And Zoo_Animali.Piva = Movimenti_Dettagli.Piva ")
            StrSQL.AppendLine("             And Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto ")
            StrSQL.AppendLine("             And Zoo_Animali_Distinte.Validita_inizio <= Movimenti.Data_Movimento ")
            StrSQL.AppendLine("             And Zoo_Animali_Distinte.Validita_Fine >= Movimenti.Data_Movimento) as Distinta_Cod  ")

            'StrSQL.Append(" (Select top 1 Distinta_Chiusa From Zoo_Animali_Distinte Where ")
            'StrSQL.Append("                 Zoo_Animali.Cod_Progetto = Zoo_Animali_Distinte.Cod_Animale  ")
            'StrSQL.Append("             And Zoo_Animali.Piva = Movimenti_Dettagli.Piva ")
            'StrSQL.Append("             And Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto) ")
            'StrSQL.Append("             And Zoo_Animali_Distinte.Validita_inizio <= Movimenti.Data_Movimento ")
            'StrSQL.Append("             And Zoo_Animali_Distinte.Validita_Fine >= Movimenti.Data_Movimento) as Distinta_Chiusa,  ")

            StrSQL.AppendLine(" From Movimenti  ")
            StrSQL.AppendLine(" Inner Join Movimenti_Dettagli On (Movimenti.Piva = movimenti_Dettagli.Piva  ")
            StrSQL.AppendLine("                              And Movimenti.Id_Agenda = movimenti_Dettagli.Id_Agenda  ")
            StrSQL.AppendLine("                              And Movimenti.Id_Mov = movimenti_Dettagli.Id_Mov)  ")
            StrSQL.AppendLine(" Inner Join Zoo_Animali On (Movimenti_Dettagli.Piva = Zoo_Animali.Piva ")
            StrSQL.AppendLine("                       And Movimenti_Dettagli.Cod_Progetto = Zoo_Animali.Cod_Progetto) ")
            StrSQL.AppendLine(" Left outer Join Lista_Razze_Animali On ( Zoo_Animali.GEN_COD = Lista_Razze_Animali.GEN_COD ")
            StrSQL.AppendLine("                                     And Zoo_Animali.SPE_COD = Lista_Razze_Animali.Spe_COD ")
            StrSQL.AppendLine("                                     And Zoo_Animali.RAZ_COD = Lista_Razze_Animali.RAZ_COD) ")

            'x Fornitore_Fatturazione
            StrSQL.AppendLine("OUTER APPLY (SELECT TOP 1 * FROM Contatti  (NOLOCK) ")
            StrSQL.AppendLine("             WHERE Contatti.Cod_Contatto = Zoo_Animali.CF_Fornitore AND ")
            StrSQL.AppendLine("                (Contatti.Piva = Zoo_Animali.Piva OR Contatti.Sa_Cod = -1)) Contatti_FornFatt")

            'x Fornitore_Provenienza
            StrSQL.AppendLine("OUTER APPLY (SELECT TOP 1 * FROM Contatti  (NOLOCK) ")
            StrSQL.AppendLine("             WHERE Contatti.Cod_Contatto = Zoo_Animali.Fornitore_Provenienza AND ")
            StrSQL.AppendLine("                (Contatti.Piva = Zoo_Animali.Piva OR Contatti.Sa_Cod = -1)) Contatti_FornProv")

            'x Peso singolo capo
            StrSQL.AppendLine("OUTER APPLY (SELECT TOP 1 MovDettPeso.Qta AS Kg_Pagati, MovDettPeso.Qta_Dettaglio1 AS Kg_Arrivo ")
            StrSQL.AppendLine("    FROM Movimenti AS MovPeso (NOLOCK) ")
            StrSQL.AppendLine("    INNER JOIN Movimenti_dettagli AS MovDettPeso ON MovPeso.Piva = MovDettPeso.Piva ")
            StrSQL.AppendLine("        AND MovPeso.Id_Agenda = MovDettPeso.Id_Agenda ")
            StrSQL.AppendLine("        AND MovPeso.Id_Mov = MovDettPeso.Id_Mov ")
            StrSQL.AppendLine("    WHERE MovPeso.Cau_Mov = '" & Agro_SQL_SaveText(CAU_PESATURA_ANIMALI) & "' ")
            StrSQL.AppendLine("        AND MovPeso.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            StrSQL.AppendLine("        AND MovPeso.Id_Agenda = " & Agro_SQL_SaveNum(Id_Agenda) & " ")
            StrSQL.AppendLine("    ) MovDettPeso ")

            StrSQL.AppendLine(" Where Movimenti.Cau_Mov = '3700'")
            StrSQL.AppendLine(" AND Movimenti.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            StrSQL.AppendLine(" AND Movimenti.Id_Agenda = " & Agro_SQL_SaveNum(Id_Agenda) & " ")

            StrSQL.AppendLine(" Order by Id_Mov_Det Asc ")

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT

    End Function


    Public Function Leggi_Barcode(ByVal Codice As String,
                                  ByVal Tipo As Integer,
                                  ByVal Bar_QR As Integer,
                                  ByVal Codifica As Integer,
                                  ByVal Data As DateTime,
                                  ByVal xFiltroAggiuntivo As String,
                                  ByVal xOrderBy As String,
                                  ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                  ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreZooDAL.Zoo_Animali_R.Leggi_Barcode()"


        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0

            StrSQL.Append(" SELECT * ")
            StrSQL.Append(" FROM  Zoo_Barcode ")
            StrSQL.Append(" WHERE Validita_inizio <= " & Agro_SQL_SaveDate(Data) & " ")
            StrSQL.Append(" AND   Validita_Fine >= " & Agro_SQL_SaveDate(Data) & " ")


            If Codice <> "" Then
                StrSQL.Append(" AND Codice = '" & Agro_SQL_SaveText(Trim(Codice)) & "'")
            End If

            If Tipo <> 0 Then
                StrSQL.Append(" AND Tipo = " & Agro_SQL_SaveNum(Tipo) & "  ")
            End If

            If Bar_QR <> 0 Then
                StrSQL.Append(" AND Bar_QR = " & Agro_SQL_SaveNum(Bar_QR) & "  ")
            End If

            If Codifica <> 0 Then
                StrSQL.Append(" AND Codifica = " & Agro_SQL_SaveNum(Codifica) & "  ")
            End If


            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.Append(" AND     dbo.Zoo_Barcode.Inviato >= 0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.Append(" AND     dbo.Zoo_Barcode.Inviato = -1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------

            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.Append(" ORDER BY Codice, Tipo, Bar_QR, Inizio, Fine, Codifica ASC ")
            End If


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function



End Class
