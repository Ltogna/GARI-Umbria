Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.AgronicaCoreParametri



Public Class Ist_Credito_R
    Inherits AgronicaCoreDataProvider.DataProvider


    '##############################################################################################
    'default di Cod_Istituto è -1
    Public Function LeggiDistinctIstCredito_BYCodContatto(ByVal Piva As String, _
                                                            ByVal Cod_Istituto As Integer, _
                                                            ByVal Filiale As Integer, _
                                                            ByVal Per_Risorsa As Integer, _
                                                            ByVal Cod_Contatto As String, _
                                                            ByVal xFiltroAggiuntivo As String, _
                                                            ByVal xOrderBy As String, _
                                                            ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                                            ) As DataTable


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_R.LeggiDistinctIstCredito_BYCodContatto()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0

            StrSQL.Append(" SELECT DISTINCT Ist_Credito.Cod_Istituto, Istituto_Des, Filiale, Per_Risorsa ")
            StrSQL.Append(" FROM  Ist_Credito ")
            'StrSQL.Append(" INNER JOIN Liquidita ON Ist_Credito.Piva = Liquidita.Piva AND Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            'CORREZIONE BUG DEL 07/01/2013: la piva di ist_credito è quella del superuser!
            StrSQL.Append(" INNER JOIN Liquidita ON Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            StrSQL.Append("                     AND Ist_Credito.Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")

            'StrSQL.Append(" WHERE Ist_Credito.Validita_Inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & "  ")
            'StrSQL.Append(" AND   Ist_Credito.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & "  ")

            'filtro la cassa
            StrSQL.Append(" WHERE Ist_Credito.Cod_Istituto > 0 ")

            If Piva <> "" Then
                StrSQL.Append(" AND    Liquidita.Piva = '" & Agro_SQL_SaveText(Piva) & "'  " + vbCrLf)
            End If

            '0 è la cassa
            If Cod_Istituto <> -1 Then
                StrSQL.Append(" AND     Ist_Credito.Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & "  " + vbCrLf)
            End If

            If Cod_Contatto <> "" Then
                StrSQL.Append(" AND    Liquidita.Cod_Contatto = '" & Agro_SQL_SaveText(Cod_Contatto) & "'  " + vbCrLf)
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.Append(" AND   Ist_Credito.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.Append(" AND   Ist_Credito.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
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

    '##############################################################################################
    Public Function Esiste_CASSA(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                    ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_R.LeggiDistinctIstCredito_BYCodContatto()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable
        Dim Flag_esiste As Boolean = False

        Try

            DT = Leggi(0, 0, 0, "", "", objParametri)

            If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
                Flag_esiste = True
            End If

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return Flag_esiste

    End Function

    '##############################################################################################
    'default di Cod_Istituto è -1 (CODISTITUTO_NOFILTRO)
    Public Function Leggi( _
                    ByVal Cod_Istituto As Integer, _
                    ByVal Filiale As Integer, _
                    ByVal Per_Risorsa As Integer, _
                    ByVal xFiltroAggiuntivo As String, _
                    ByVal xOrderBy As String, _
                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                    ) As DataTable


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_R.Leggi()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0

            StrSQL.Append(" SELECT Ist_Credito.* ")
            StrSQL.Append(" FROM  Ist_Credito ")

            StrSQL.Append(" WHERE Ist_Credito.Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")

            StrSQL.Append(" AND Ist_Credito.Validita_Inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & "  ")
            StrSQL.Append(" AND   Ist_Credito.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & "  ")

            '0 è la cassa
            If Cod_Istituto <> CODISTITUTO_NOFILTRO Then
                StrSQL.Append(" AND     Ist_Credito.Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & "  " + vbCrLf)
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.Append(" AND   Ist_Credito.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.Append(" AND   Ist_Credito.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.Append(" ORDER BY Istituto_Des ")
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


    '##########################################################################################################################################
    Public Function Esiste_Istituto_byDesc(ByVal IstitutoDes As String, _
                                           ByVal objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_R.Esiste_Istituto_byDesc()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim Esiste_Istituto As Boolean = False
        'Dim filtro As String = " Ist_Credito.Istituto_Des LIKE '" & Agro_SQL_SaveText(IstitutoDes) & "' "
        Dim filtro As String = " Ist_Credito.Istituto_Des = '" & Agro_SQL_SaveText(IstitutoDes) & "' "

        Try

            Dim Dt As DataTable

            Dt = Leggi(0, 0, 0, _
                       filtro, "", _
                       objParametri)

            If Not Dt Is Nothing Then
                Select Case Dt.Rows.Count
                    Case 0
                    Case 1
                        Esiste_Istituto = True
                    Case Is > 1
                        Esiste_Istituto = True
                End Select
            End If

            Dt = Nothing

        Catch ex As Exception
            Esiste_Istituto = False
            MessaggioErrore = ex.Message
            MyBase.Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return Esiste_Istituto

    End Function

    '##########################################################################################################################################
    Public Function Recupera_CodIstituto_byDesc(ByVal IstitutoDes As String, _
                                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Integer


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_R.Recupera_CodIstituto_byDesc()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim Cod_Istituto As Integer = -1
        'Dim filtro As String = " Ist_Credito.Istituto_Des LIKE '" & Agro_SQL_SaveText(IstitutoDes) & "' "
        Dim filtro As String = " Ist_Credito.Istituto_Des = '" & Agro_SQL_SaveText(IstitutoDes) & "' "

        Try

            Dim Dt As DataTable

            Dt = Leggi(CODISTITUTO_NOFILTRO, 0, 0, _
                       filtro, "", _
                       objParametri)

            If Not Dt Is Nothing Then
                Select Case Dt.Rows.Count
                    Case 0
                    Case 1
                        Cod_Istituto = Dt.Rows(0).Item("Cod_Istituto")
                    Case Is > 1
                        'prendo il primo
                        Cod_Istituto = Dt.Rows(0).Item("Cod_Istituto")
                End Select
            End If

            Dt = Nothing

        Catch ex As Exception
            Cod_Istituto = -1
            MessaggioErrore = ex.Message
            MyBase.Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return Cod_Istituto

    End Function



End Class


'#################################################################
'#################################################################
'#################################################################

Public Class Ist_Credito_W
    Inherits AgronicaCoreDataProvider.DataProvider



    '##############################################################################################
    Public Function Scrivi( _
                          ByVal Sa_Cod As Integer _
                        , ByVal Cod_Istituto As Integer _
                        , ByVal Istituto_Des As String _
                        , ByVal Filiale As Integer _
                        , ByVal Per_Risorsa As String _
                        , ByVal validita_inizio As Date _
                        , ByVal validita_fine As Date _
                        , ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                , Optional ByVal Data_creazione As DateTime = #2/1/1900# _
                , Optional ByVal Data_modifica As DateTime = #2/1/1900# _
                , Optional ByVal username_creazione As String = "" _
                , Optional ByVal username_modifica As String = "" _
                ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_W.Scrivi()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            If Data_creazione = #2/1/1900# Then
                Data_creazione = Date.Now
            End If

            If Data_modifica = #2/1/1900# Then
                Data_modifica = Date.Now
            End If

            If username_creazione = "" Then
                username_creazione = objParametri.UsernameOperazione
            End If

            If username_modifica = "" Then
                username_modifica = objParametri.UsernameOperazione
            End If



            '---------------------------------------------
            StrSQL.Length = 0
            StrSQL.Append(" INSERT Ist_Credito " + vbCrLf)

            StrSQL.Append("              (")

            StrSQL.Append("   [Piva] " & vbCrLf)
            StrSQL.Append("  ,[Sa_Cod] " & vbCrLf)
            StrSQL.Append("  ,[Cod_Istituto] " & vbCrLf)
            StrSQL.Append("  ,[Istituto_Des] " & vbCrLf)
            StrSQL.Append("  ,[Filiale] " & vbCrLf)
            StrSQL.Append("  ,[Per_Risorsa], " & vbCrLf)

            StrSQL.Append("              Inviato,            datainvio, ")
            'StrSQL.Append("              Data_Creazione,     Data_Modifica, ")
            StrSQL.Append("              UserName_Creazione, UserName_Modifica, ")
            StrSQL.Append("              Validita_Inizio,    Validita_Fine ")

            StrSQL.Append("              ) ")

            StrSQL.Append(" VALUES ( ")

            StrSQL.Append(" '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'" & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Sa_Cod) & " " & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Cod_Istituto) & " " & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Istituto_Des) & "'" & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Filiale) & " " & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Per_Risorsa) & "'" & vbCrLf)


            StrSQL.Append("         , 0  " + vbCrLf)
            StrSQL.Append("         , Null  " + vbCrLf)

            'StrSQL.Append("			, " & Agro_SQL_SaveDateTime(Data_creazione) & "  ")
            'StrSQL.Append("			, " & Agro_SQL_SaveDateTime(Data_modifica) & "  ")
            StrSQL.Append("			,'" & Agro_SQL_SaveText(username_creazione) & "' ")
            StrSQL.Append("			,'" & Agro_SQL_SaveText(username_modifica) & "' ")
            StrSQL.Append("			, " & Agro_SQL_SaveDate(validita_inizio) & "  ")
            StrSQL.Append("			, " & Agro_SQL_SaveDate(validita_fine) & "  ")

            StrSQL.Append(") ")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function

    '##############################################################################################
    Public Function Modifica(ByVal Cod_Istituto As Integer, _
                              ByVal Sa_Cod As Integer _
                            , ByVal Istituto_Des As String _
                            , ByVal Filiale As Integer _
                            , ByVal Per_Risorsa As String _
                            , ByVal validita_inizio As Date _
                            , ByVal validita_fine As Date _
                            , ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                            , Optional ByVal Data_modifica As DateTime = #2/1/1900# _
                            , Optional ByVal username_modifica As String = "" _
                            ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_W.Modifica()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            If Data_modifica = #2/1/1900# Then
                Data_modifica = Date.Now
            End If

            If username_modifica = "" Then
                username_modifica = objParametri.UsernameOperazione
            End If

            '---------------------------------------------
            StrSQL.Length = 0
            StrSQL.Append(" UPDATE Ist_Credito SET " + vbCrLf)

            StrSQL.Append("    Sa_Cod       = " & Agro_SQL_SaveNum(Sa_Cod) & "  ")
            StrSQL.Append("   ,Istituto_Des = '" & Agro_SQL_SaveText(Istituto_Des) & "'   ")
            StrSQL.Append("   ,Filiale      = " & Agro_SQL_SaveNum(Filiale) & "  ")
            StrSQL.Append("   ,Per_Risorsa  = '" & Agro_SQL_SaveText(Per_Risorsa) & "'  ")

            StrSQL.Append("   ,Inviato           = 0 ")
            StrSQL.Append("   ,DataInvio         = Null ")
            ' 'StrSQL.Append("              Data_Creazione,     Data_Modifica, ")
            StrSQL.Append("  ,UserName_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "'  ")
            StrSQL.Append("  ,Validita_Inizio   =  " & Agro_SQL_SaveDate(validita_inizio))
            StrSQL.Append("  ,Validita_Fine     =  " & Agro_SQL_SaveDate(validita_fine))

            StrSQL.Append("  WHERE Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'" & vbCrLf)
            StrSQL.Append(" AND Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & " " & vbCrLf)


            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function


    '#################################################################
    Public Function Inserisci_CASSA(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                        ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_W.Inserisci_CASSA()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim xRisp As Boolean = False

        Try

            'modifica del 09/02/2016: non scriviamo più cassa perchè il nome della cassa
            'è scritto nel campo Numero di Liquidita (da quando è stata introdotta la gestione delle più casse)
            'xRisp = Scrivi(0, 0, "CASSA", 0, 1, AGRODATAINIZIO, AGRODATAFINE, objParametri, _
            '       Date.Now, Date.Now, objParametri.PivaSuperUser, objParametri.PivaSuperUser)
            xRisp = Scrivi(0, 0, "", 0, enum_IstCredito_PerRisorsa.ContoCorrente, AGRODATAINIZIO, AGRODATAFINE, objParametri, _
              Date.Now, Date.Now, objParametri.PivaSuperUser, objParametri.PivaSuperUser)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function

    '#################################################################
    Public Function Modifica_CASSA(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                        ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_W.Modifica_CASSA()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim xRisp As Boolean = False

        Try

            '09/02/2016: non scriviamo più cassa perchè il nome della cassa
            'è scritto nel campo Numero di Liquidita (da quando è stata introdotta la gestione delle più casse)
            xRisp = Modifica(0, 0, "", 0, enum_IstCredito_PerRisorsa.ContoCorrente, AGRODATAINIZIO, AGRODATAFINE, objParametri, _
              Date.Now, objParametri.PivaSuperUser)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function



    '#################################################################
    Public Function Cancella(ByVal xFiltroAggiuntivo As String, _
                              ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                              ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Ist_Credito_W.Cancella()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------
            StrSQL.Length = 0

            '---------------------------------------------
            If objParametri.FlagCancellazioneLogica = enumCancellazioneLogica.CancellazioneLogica Then
                StrSQL.Append(" UPDATE ... ")
                StrSQL.Append(" SET ")
                StrSQL.Append("         Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                StrSQL.Append("         ,Data_Modifica= " & Agro_SQL_SaveDate(Date.Now) & " ")
                StrSQL.Append("         ,Inviato = -1 ")
                StrSQL.Append(" WHERE   1=1 ")
                StrSQL.Append(" AND     Inviato >= 0 ")
            Else
                StrSQL.Append(" DELETE FROM ... ")
                StrSQL.Append(" WHERE 1=1 ")
            End If
            '---------------------------------------------

            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function




End Class


