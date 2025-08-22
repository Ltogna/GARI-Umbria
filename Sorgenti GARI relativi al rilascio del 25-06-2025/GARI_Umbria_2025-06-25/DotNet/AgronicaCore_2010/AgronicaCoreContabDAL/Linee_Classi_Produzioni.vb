Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.UtilityProvider


Public Class Linee_Classi_Produzioni_R
    Inherits AgronicaCoreDataProvider.DataProvider


    '####################################################################
    Public Function Leggi(ByVal Piva As String,
                          ByVal Linea_Classe_Cod As Integer,
                          ByVal xFiltroAggiuntivo As String,
                          ByVal xOrderBy As String,
                          ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                          ) As DataTable

        Const nomeRoutine As String = "AgronicaCoreContabDAL.Linee_Classi_Produzioni_R.Leggi"

        Dim messaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            StbSQL.Length = 0

            StbSQL.AppendLine(" SELECT Linee_Classi_Produzioni.* ")
            StbSQL.AppendLine(" FROM Linee_Classi_Produzioni ")
            StbSQL.AppendLine(" WHERE 1 = 1 ")

            If Piva <> "" Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Linea_Classe_Cod <> 0 Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Linea_Classe_Cod = " & Agro_SQL_SaveNum(Linea_Classe_Cod))
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StbSQL.AppendLine(" AND   Linee_Classi_Produzioni.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StbSQL.AppendLine(" AND   Linee_Classi_Produzioni.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StbSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StbSQL.AppendLine(" ORDER BY Linea_Classe_Des ")
            End If


            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StbSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt

    End Function

    '####################################################################
    'la tabella Linee_Classi_Produzioni viene usata anche per classificare i prodotti:
    'nella tabella materie_prime se il campo cat_cod è valorizzato
    'il suo valore è un Linea_Classe_Cod, quindi la materia prima è raggruppata sotto questa classe
    Public Function LeggiClassiProdotto(ByVal Piva As String,
                                        ByVal Linea_Classe_Cod As Integer,
                                        ByVal Mat_Cod As Integer,
                                        ByVal xFiltroAggiuntivo As String,
                                        ByVal xOrderBy As String,
                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                        ) As DataTable

        Const nomeRoutine = "AgronicaCoreContabDAL.Linee_Classi_Produzioni_R.LeggiClassiProdotto"

        Dim messaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            StbSQL.Length = 0

            'questa query serve per avere l'elenco delle classi prodotto,
            'quindi serve il distinct perché una classe può essere usata per più prodotti
            StbSQL.AppendLine(" SELECT DISTINCT Linee_Classi_Produzioni.Piva, Linea_Classe_Cod, Linea_Classe_Des, Tipo_Produzione, Linea_Classe_Padre_Cod, Tipo_Classe ")

            StbSQL.AppendLine(" FROM Linee_Classi_Produzioni ")
            StbSQL.AppendLine(" INNER JOIN Materie_Prime ON Materie_Prime.Cat_Cod = Linee_Classi_Produzioni.Linea_Classe_Cod ")

            StbSQL.AppendLine(" WHERE 1 = 1 ")

            If Piva <> "" Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Linea_Classe_Cod <> 0 Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Linea_Classe_Cod = " & Agro_SQL_SaveNum(Linea_Classe_Cod))
            End If

            If Mat_Cod <> 0 Then
                StbSQL.AppendLine(" AND Materie_Prime.Mat_Cod = " & Agro_SQL_SaveNum(Mat_Cod))
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StbSQL.AppendLine(" AND   Linee_Classi_Produzioni.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StbSQL.AppendLine(" AND   Linee_Classi_Produzioni.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StbSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StbSQL.AppendLine(" ORDER BY Linea_Classe_Des ")
            End If


            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StbSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt

    End Function

    Public Function Leggi_con_Filtro_Tipo_Classe(ByVal Piva As String,
                                                 ByVal TipoClasse As Integer,
                                                 ByVal xFiltroAggiuntivo As String,
                                                 ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                                 ) As DataTable

        Const nomeRoutine = "AgronicaCoreContabDAL.Linee_Classi_Produzioni_R.Leggi_con_Filtro_Tipo_Classe"

        Dim messaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            StbSQL.Length = 0

            StbSQL.AppendLine(" SELECT DISTINCT Linee_Classi_Produzioni.* ")
            StbSQL.AppendLine(" FROM Linee_Classi_Produzioni ")
            StbSQL.AppendLine(" WHERE 1 = 1 ")

            If Piva <> "" Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Validita_Inizio <= " & Agro_SQL_SaveDate(AGRODATAFINE))

            StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Validita_Fine >= " & Agro_SQL_SaveDate(AGRODATAINIZIO))

            If TipoClasse <> -1 Then
                StbSQL.AppendLine(" AND Linee_Classi_Produzioni.Tipo_Classe = " & Agro_SQL_SaveNum(TipoClasse))
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            StbSQL.AppendLine(" ORDER BY Linee_Classi_Produzioni.Piva, Linee_Classi_Produzioni.Tipo_Classe, Linee_Classi_Produzioni.Linea_Classe_Des ASC ")


            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StbSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt

    End Function

End Class










''''''''''''''''''''''''''''''''''''''''''''''''''

Public Class Linee_Classi_Produzioni_W
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Scrivi(ByVal Piva As String,
                           ByVal Linea_Classe_Cod As Integer,
                           ByVal Linea_Classe_Des As String,
                           ByVal Tipo_Produzione As Integer,
                           ByVal Linea_Classe_Padre_Cod As Integer,
                           ByVal Tipo_Classe As Integer,
                           ByVal Modulo_Generazione As Integer,
                           ByVal ChkUtility As Integer,
                           ByVal DirPicture As String,
                           ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                           Optional ByVal Data_creazione As DateTime = #2/1/1900#,
                           Optional ByVal Data_modifica As DateTime = #2/1/1900#,
                           Optional ByVal username_creazione As String = "",
                           Optional ByVal username_modifica As String = ""
                           ) As Boolean

        Const nomeRoutine = "AgronicaCoreContabDAL.Linee_Classi_Produzioni_W.Scrivi()"

        Dim messaggioErrore As String = ""
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

            StrSQL.AppendLine("INSERT INTO [Linee_Classi_Produzioni] ")
            StrSQL.AppendLine("                    ( Piva , Linea_Classe_Cod , Linea_Classe_Des , Tipo_Produzione ,  Linea_Classe_Padre_Cod , inviato           ,")
            StrSQL.AppendLine("                    datainvio,  Username_Creazione , Username_Modifica ,  Validita_Inizio , Validita_Fine ,Tipo_Classe  , Modulo_Generazione ,ChkUtility  ,DirPicture, data_creazione, data_modifica   ")
            StrSQL.AppendLine("                    ) ")

            StrSQL.AppendLine("VALUES (")
            StrSQL.AppendLine("          '" & Agro_SQL_SaveText(Piva) & "' ")
            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(Linea_Classe_Cod))
            StrSQL.AppendLine("         , '" & Agro_SQL_SaveText(Linea_Classe_Des) & "' ")
            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(Tipo_Produzione))
            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(Linea_Classe_Padre_Cod))

            StrSQL.AppendLine("         , 0  ")
            StrSQL.AppendLine("         , Null  ")
            StrSQL.AppendLine("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("         , " & Agro_SQL_SaveDate(AGRODATAINIZIO) & "  ")
            StrSQL.AppendLine("         , " & Agro_SQL_SaveDate(AGRODATAFINE) & "  ")

            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(Tipo_Classe))
            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(Modulo_Generazione))
            StrSQL.AppendLine("         , " & Agro_SQL_SaveNum(ChkUtility))
            StrSQL.AppendLine("         ,'" & Agro_SQL_SaveText(DirPicture) & "' ")


            StrSQL.AppendLine("         , " & Agro_SQL_SaveDateTime(Data_creazione) & "  ")
            StrSQL.AppendLine("         , " & Agro_SQL_SaveDateTime(Data_modifica) & "  ")

            StrSQL.AppendLine(")")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return xRisp

    End Function

End Class
