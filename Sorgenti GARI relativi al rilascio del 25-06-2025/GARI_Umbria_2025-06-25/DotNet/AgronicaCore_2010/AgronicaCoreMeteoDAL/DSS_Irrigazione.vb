Imports System.Text
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.DataProviderExtensions

Public Class DSS_Irrigazione_R
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Leggi(ByVal Piva As String,
                          ByVal Sa_Cod As Integer,
                          ByVal Appezza As Integer,
                          ByVal Id_Reg As Integer,
                          ByVal Progetto_Cod As Integer,
                          ByVal Data_Esecuzione As Date,
                            ByVal xFiltroAggiuntivo As String,
                            ByVal xOrderBy As String,
                            ByVal objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                            ) As DataTable

        '----- Descrizione
        Dim nomeRoutine As String = "AgronicaCoreMeteoDAL.DSS_Irrigazione_W.Leggi()"

        '----- Variabili
        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            Stb.Length = 0

            Stb.AppendLine(" SELECT * ")
            Stb.AppendLine(" FROM DSS_Irrigazione  ")
            Stb.AppendLine(" WHERE 1 = 1  ")
            If Piva <> "" Then
                Stb.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "'")
            End If

            If Sa_Cod <> 0 Then
                Stb.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            End If

            If Appezza <> 0 Then
                Stb.AppendLine(" AND Appezza = " & Agro_SQL_SaveNum(Appezza) & " ")
            End If

            If Id_Reg <> 0 Then
                Stb.AppendLine(" AND Id_Reg = " & Agro_SQL_SaveNum(Id_Reg) & " ")
            End If

            If Progetto_Cod <> 0 Then
                Stb.AppendLine(" AND Progetto_Cod = " & Agro_SQL_SaveNum(Progetto_Cod) & " ")
            End If

            If Data_Esecuzione <> AGRODATAINIZIO AndAlso Data_Esecuzione <> AGRODATAFINE Then
                Stb.AppendLine(" AND Data_Esecuzione = " & Agro_SQL_SaveDateTime(Data_Esecuzione))
            End If

            If xFiltroAggiuntivo <> "" Then
                Stb.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                Stb.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, Stb.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT





    End Function


    Public Function Leggi_Precedenti(ByVal Piva As String,
                                     ByVal Sa_Cod As Integer,
                                     ByVal Appezza As Integer,
                                     ByVal Id_Reg As Integer,
                                     ByVal Progetto_Cod As Integer,
                                     ByVal Data_Esecuzione_Inizio As Date,
                                     ByVal Data_Esecuzione_Fine As Date,
                                     ByVal xFiltroAggiuntivo As String,
                                     ByVal xOrderBy As String,
                                     ByVal objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                     ) As DataTable

        '----- Descrizione
        Dim nomeRoutine As String = "AgronicaCoreMeteoDAL.DSS_Irrigazione_W.Leggi_Precedenti()"

        '----- Variabili
        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            Stb.Length = 0

            Stb.AppendLine(" SELECT ID AS ID_DSS_Irrigazione, ")
            Stb.AppendLine(" CONCAT(CONVERT(varchar, Data_Consiglio, 103), ' - ', FORMAT(Qta_Acqua, 'G', 'de-de'),' ',ISNULL(UnitaMisura.UDM_SIM,''), ' (', CONVERT(varchar, Data_Esecuzione, 103),')') AS Descrizione_DSS_Irrigazione,")
            Stb.AppendLine(" Qta_Acqua AS Qta_Acqua_DSS_Irrigazione, ISNULL(DSS_Irrigazione.Udm_Cod,0) AS Udm_Cod_DSS_Irrigazione, ISNULL(UnitaMisura.Udm_Sim,0) AS Udm_Sim_DSS_Irrigazione, ")
            Stb.AppendLine(" ISNULL(UnitaMisura.Udm_Des,0) AS Udm_Des_DSS_Irrigazione, DSS_Irrigazione.Provider_Consiglio, DSS_Irrigazione.Modello_Consiglio,")
            Stb.AppendLine(" DSS_Irrigazione.Data_Esecuzione, DSS_Irrigazione.Data_Consiglio")
            Stb.AppendLine(" FROM DSS_Irrigazione  ")
            Stb.AppendLine(" LEFT JOIN UnitaMisura  ")
            Stb.AppendLine(" ON UnitaMisura.UDM_COD = DSS_Irrigazione.UdM_Cod")
            Stb.AppendLine(" WHERE DSS_Irrigazione.Provider_Consiglio = 1 AND DSS_Irrigazione.Modello_Consiglio = 1")

            If Piva <> "" Then
                Stb.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "'")
            End If

            If Sa_Cod <> 0 Then
                Stb.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            End If

            If Appezza <> 0 Then
                Stb.AppendLine(" AND Appezza = " & Agro_SQL_SaveNum(Appezza) & " ")
            End If

            If Id_Reg <> 0 Then
                Stb.AppendLine(" AND Id_Reg = " & Agro_SQL_SaveNum(Id_Reg) & " ")
            End If

            If Progetto_Cod <> 0 Then
                Stb.AppendLine(" AND Progetto_Cod = " & Agro_SQL_SaveNum(Progetto_Cod) & " ")
            End If

            If Data_Esecuzione_Inizio <> AGRODATAINIZIO AndAlso Data_Esecuzione_Inizio <> AGRODATAFINE AndAlso
                Data_Esecuzione_Fine <> AGRODATAINIZIO AndAlso Data_Esecuzione_Fine <> AGRODATAFINE Then

                Stb.AppendLine(" AND ( Data_Esecuzione <= " & Agro_SQL_SaveDateTime(Data_Esecuzione_Fine) &
                               " AND Data_Esecuzione >= " & Agro_SQL_SaveDateTime(Data_Esecuzione_Inizio) & ")")
            End If

            If xFiltroAggiuntivo <> "" Then
                Stb.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                Stb.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                Stb.AppendLine(" ORDER BY Data_Esecuzione DESC")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, Stb.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT





    End Function

End Class

Public Class DSS_Irrigazione_W
    Inherits AgronicaCoreDataProvider.DataProvider

    '============================================================================
    Public Function Scrivi(ByVal Piva As String,
                           ByVal Sa_Cod As Integer,
                           ByVal Appezza As Integer,
                           ByVal Id_Reg As Integer,
                           ByVal Progetto_Cod As Integer,
                           ByVal Data_Esecuzione As Date,
                           ByVal Data_Consiglio As Date,
                           ByVal Qta_Acqua As Decimal,
                           ByVal UdM_Cod As Integer,
                           ByVal Provider_Consiglio As Integer,
                           ByVal Modello_Consiglio As Integer,
                           ByVal objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                           ) As Boolean

        Dim nomeRoutine As String = "AgronicaCoreMeteoDAL.DSS_Irrigazione_W.Scrivi()"

        Dim messaggioErrore As String = ""
        Dim strSql As New StringBuilder
        Dim xRisp As Boolean = False

        Try

            '---------------------------------------------
            strSql.Length = 0

            strSql.AppendLine(" INSERT INTO DSS_Irrigazione ")
            strSql.AppendLine("         ( ")
            strSql.AppendLine("          Piva, Sa_Cod, Appezza,")
            strSql.AppendLine("          Id_Reg, Progetto_Cod, Data_Esecuzione,")
            strSql.AppendLine("          Data_Consiglio, Qta_Acqua, UdM_Cod,")
            strSql.AppendLine("          Provider_Consiglio, Modello_Consiglio,")
            strSql.AppendLine("          Username_Creazione, Username_Modifica")
            strSql.AppendLine("         ) ")

            strSql.AppendLine(" VALUES ( ")
            strSql.AppendLine("          '" & Agro_SQL_SaveText(Piva) & "'   ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Sa_Cod))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Appezza))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Id_Reg))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Progetto_Cod))
            strSql.AppendLine("         , " & Agro_SQL_SaveDateTime(Data_Esecuzione))
            strSql.AppendLine("         , " & Agro_SQL_SaveDateTime(Data_Consiglio))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Qta_Acqua))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(UdM_Cod))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Provider_Consiglio))
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Modello_Consiglio))
            strSql.AppendLine("         , '" & objParametri.UtenteUsername & "'")
            strSql.AppendLine("         , '" & objParametri.UtenteUsername & "'")
            strSql.AppendLine("        ) ")


            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, strSql.ToString, nomeRoutine)
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
