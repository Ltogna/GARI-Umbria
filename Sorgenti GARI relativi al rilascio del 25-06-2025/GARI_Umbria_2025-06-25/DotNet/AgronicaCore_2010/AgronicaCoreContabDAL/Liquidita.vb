Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi

Public Class Liquidita_R
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Leggi(ByVal Piva As String, _
                          ByVal xFiltroAggiuntivo As String, _
                                ByVal xOrderBy As String, _
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_R.Leggi"

        Dim MessaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim i As Integer

        Try

            StbSQL.Length = 0

            StbSQL.Append(" SELECT    * ")
            StbSQL.Append(" FROM Liquidita ")
            StbSQL.Append(" WHERE 1=1 ")

            If Piva <> "" Then
                StbSQL.Append(" AND Liquidita.Piva = '" & Agro_SQL_SaveText(Piva) & "'   ")
            End If

            
            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------

            '################################
            If xOrderBy <> "" Then
                StbSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StbSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function


    '###################################################################################
    'Attenzione: tra liquidita e Risorse_Umane c'è solo il join del cod_contatto
    'perchè la piva di liquidità non è detto che sia la piva che ha creato il contatto.
    'Se in archivio si trovano più contatti con lo stesso cod_contatto, vengono restituite più righe
    Public Function Leggi_Estesa(ByVal Piva As String,
                                ByVal Cod_Liquidita As Integer,
                                ByVal Nazione As String,
                                ByVal Cifre_Controllo As String,
                                ByVal Cin As String,
                                ByVal Abi As String,
                                ByVal Cab As String,
                                ByVal Bic As String,
                                ByVal Numero As String,
                                ByVal Cau_Risorsa As String,
                                ByVal Cod_Istituto As Integer,
                                ByVal Avviso As Integer,
                                ByVal Cod_Contatto As String,
                                ByVal Per_Risorsa As String,
                                ByVal Cod_RisUm As Integer,
                                ByVal Cod_Rapporto As Integer,
                                ByVal Piva_Contatto As String,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_R.Leggi_Estesa"

        Dim MessaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim i As Integer

        Try

            StbSQL.Length = 0

            StbSQL.Append(" SELECT     Liquidita.Piva, Liquidita.Cod_Liquidita, Liquidita.Nazione, Liquidita.Cifre_Controllo, Liquidita.Cin, Liquidita.Abi, Liquidita.Cab, Liquidita.Bic,  ")
            StbSQL.Append(" Liquidita.Numero, Liquidita.Saldo_Attuale, Liquidita.Saldo_Iniziale, Liquidita.Cau_Risorsa, Liquidita.Cod_Istituto, Liquidita.Avviso, ")
            StbSQL.Append(" Liquidita.ChkDefault, Liquidita.ChkAbilitazione, ")
            StbSQL.Append(" CASE ")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 0 THEN 'Nessuna (Solo Consultazione)'")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 1 THEN 'Partita Doppia/Pagamenti/Incassi'  ")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 2 THEN 'Partita Doppia'")
            StbSQL.Append(" ELSE '' ")
            StbSQL.Append(" END as Abilitazione_Des, ")

            StbSQL.Append(" Liquidita.Importo_Avviso, Liquidita.Note, Liquidita.Cod_Contatto, Ist_Credito.Istituto_Des, Ist_Credito.Filiale, Ist_Credito.Per_Risorsa, ")
            StbSQL.Append(" Risorse_Umane.Cod_RisUm, Risorse_Umane.Cod_Rapporto, Risorse_Umane.Validita_Inizio, Risorse_Umane.Validita_Fine, ")
            StbSQL.Append(" Risorse_Umane.Settore_Des, Risorse_Umane.Attivita_Des, Risorse_Umane.Patentino, Risorse_Umane.Data_Rilascio_Patentino, ")
            StbSQL.Append(" Risorse_Umane.Data_Scadenza_Patentino, Contatti.Sa_Cod, Contatti.Id_CF, Contatti.Rag_Soc, Contatti.Codice_Fiscale,  ")
            StbSQL.Append(" Imprese.rag_soc AS Impresa ")

            StbSQL.Append(" FROM Liquidita ")
            StbSQL.Append(" LEFT OUTER JOIN Ist_Credito ON Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            StbSQL.Append("                 AND Ist_Credito.Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")
            StbSQL.Append(" LEFT OUTER JOIN Imprese ON Imprese.PIVA = Liquidita.Cod_Contatto ")
            StbSQL.Append(" LEFT OUTER JOIN Risorse_Umane ON Liquidita.Cod_Contatto = Risorse_Umane.Cod_Contatto ") 'AND Liquidita.Piva = Risorse_Umane.Piva
            StbSQL.Append(" LEFT OUTER JOIN Contatti ON Risorse_Umane.Piva = Contatti.Piva AND Risorse_Umane.Cod_Contatto = Contatti.Cod_Contatto ")

            StbSQL.Append(" WHERE     Liquidita.Validita_Inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
            StbSQL.Append(" AND       Liquidita.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

            If Piva <> "" Then
                StbSQL.Append(" AND Liquidita.Piva = '" & Agro_SQL_SaveText(Piva) & "'   ")
            End If

            If Cod_Liquidita <> 0 Then
                StbSQL.Append(" AND Liquidita.Cod_Liquidita = " & Agro_SQL_SaveNum(Cod_Liquidita) & "   ")
            End If

            If Nazione <> "" Then
                StbSQL.Append(" AND Liquidita.Nazione = '" & Agro_SQL_SaveText(Nazione) & "'   ")
            End If

            If Cifre_Controllo <> "" Then
                StbSQL.Append(" AND Liquidita.Cifre_Controllo = '" & Agro_SQL_SaveText(Cifre_Controllo) & "'   ")
            End If

            If Cin <> "" Then
                StbSQL.Append(" AND Liquidita.Cin = '" & Agro_SQL_SaveText(Cin) & "'   ")
            End If

            If Abi <> "" Then
                StbSQL.Append(" AND Liquidita.Abi = '" & Agro_SQL_SaveText(Abi) & "'   ")
            End If

            If Cab <> "" Then
                StbSQL.Append(" AND Liquidita.Cab = '" & Agro_SQL_SaveText(Cab) & "'   ")
            End If

            If Bic <> "" Then
                StbSQL.Append(" AND Liquidita.Bic = '" & Agro_SQL_SaveText(Bic) & "'   ")
            End If

            If Numero <> "" Then
                StbSQL.Append(" AND Liquidita.Numero = '" & Agro_SQL_SaveText(Numero) & "'   ")
            End If

            If Cau_Risorsa <> "" Then
                StbSQL.Append(" AND Liquidita.Cau_Risorsa = '" & Agro_SQL_SaveText(Cau_Risorsa) & "'   ")
            End If

            If Cod_Istituto <> 0 Then
                StbSQL.Append(" AND Liquidita.Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & "   ")
            End If

            If Avviso <> 0 Then
                StbSQL.Append(" AND Liquidita.Avviso = " & Agro_SQL_SaveNum(Avviso) & "   ")
            End If

            If Cod_Contatto <> "" Then
                StbSQL.Append(" AND Liquidita.Cod_Contatto = '" & Agro_SQL_SaveText(Cod_Contatto) & "'   ")
            End If

            If Per_Risorsa <> "" Then
                StbSQL.Append(" AND Ist_Credito.Per_Risorsa = '" & Agro_SQL_SaveText(Per_Risorsa) & "'   ")
            End If

            If Cod_RisUm <> 0 Then
                StbSQL.Append(" AND Risorse_Umane.Cod_RisUm = " & Agro_SQL_SaveNum(Cod_RisUm) & "   ")
            End If

            If Cod_Rapporto <> 0 Then
                StbSQL.Append(" AND Risorse_Umane.Cod_Rapporto = " & Agro_SQL_SaveNum(Cod_Rapporto) & "   ")
            End If

            If Piva_Contatto <> "" Then
                StbSQL.Append(" AND Risorse_Umane.Piva_Contatto = '" & Agro_SQL_SaveText(Piva_Contatto) & "'   ")
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------

            '################################
            If xOrderBy <> "" Then
                StbSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StbSQL.Append(" ORDER BY Imprese.rag_soc, Contatti.Rag_Soc ")
            End If


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StbSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_Per_Contatto(ByVal Piva As String,
                                ByVal Cod_Liquidita As Integer,
                                ByVal Nazione As String,
                                ByVal Cifre_Controllo As String,
                                ByVal Cin As String,
                                ByVal Abi As String,
                                ByVal Cab As String,
                                ByVal Bic As String,
                                ByVal Numero As String,
                                ByVal Cau_Risorsa As String,
                                ByVal Cod_Istituto As Integer,
                                ByVal Avviso As Integer,
                                ByVal Cod_Contatto As String,
                                ByVal Per_Risorsa As String,
                                ByVal Cod_RisUm As Integer,
                                ByVal Cod_Rapporto As Integer,
                                ByVal Piva_Contatto As String,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_R.Leggi_Estesa"

        Dim MessaggioErrore As String = ""
        Dim StbSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim i As Integer

        Try

            StbSQL.Length = 0

            StbSQL.Append(" SELECT DISTINCT  Liquidita.Piva, Liquidita.Cod_Liquidita, Liquidita.Nazione, Liquidita.Cifre_Controllo, Liquidita.Cin, Liquidita.Abi, Liquidita.Cab, Liquidita.Bic,  ")
            StbSQL.Append(" Liquidita.Numero, Liquidita.Saldo_Attuale, Liquidita.Saldo_Iniziale, Liquidita.Cau_Risorsa, Liquidita.Cod_Istituto, Liquidita.Avviso, ")
            StbSQL.Append(" Liquidita.ChkDefault, Liquidita.ChkAbilitazione, ")
            StbSQL.Append(" Liquidita.Validita_Inizio, Liquidita.Validita_Fine, ")
            StbSQL.Append(" CASE ")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 0 THEN 'Nessuna (Solo Consultazione)'")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 1 THEN 'Partita Doppia/Pagamenti/Incassi'  ")
            StbSQL.Append(" WHEN Liquidita.ChkAbilitazione = 2 THEN 'Partita Doppia'")
            StbSQL.Append(" ELSE '' ")
            StbSQL.Append(" END as Abilitazione_Des, ")

            StbSQL.Append(" Liquidita.Importo_Avviso, Liquidita.Note, Liquidita.Cod_Contatto, Ist_Credito.Istituto_Des, Ist_Credito.Filiale, Ist_Credito.Per_Risorsa, ")
            StbSQL.Append(" Contatti.Sa_Cod, Contatti.Id_CF, Contatti.Rag_Soc, Contatti.Codice_Fiscale,  ")
            StbSQL.Append(" Imprese.rag_soc AS Impresa ")

            StbSQL.Append(" FROM Liquidita ")
            StbSQL.Append(" LEFT OUTER JOIN Ist_Credito ON Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            StbSQL.Append("                 AND Ist_Credito.Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")
            StbSQL.Append(" LEFT OUTER JOIN Imprese ON Imprese.PIVA = Liquidita.Cod_Contatto ")
            StbSQL.Append(" LEFT OUTER JOIN Risorse_Umane ON Liquidita.Cod_Contatto = Risorse_Umane.Cod_Contatto ")
            StbSQL.Append(" and Risorse_Umane.Piva = Liquidita.Piva")
            StbSQL.Append(" LEFT OUTER JOIN Contatti ON Risorse_Umane.Piva = Contatti.Piva AND Risorse_Umane.Cod_Contatto = Contatti.Cod_Contatto ")

            StbSQL.Append(" WHERE     Liquidita.Validita_Inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
            StbSQL.Append(" AND       Liquidita.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

            If Piva <> "" Then
                StbSQL.Append(" AND Liquidita.Piva = '" & Agro_SQL_SaveText(Piva) & "'   ")
            End If

            If Cod_Liquidita <> 0 Then
                StbSQL.Append(" AND Liquidita.Cod_Liquidita = " & Agro_SQL_SaveNum(Cod_Liquidita) & "   ")
            End If

            If Nazione <> "" Then
                StbSQL.Append(" AND Liquidita.Nazione = '" & Agro_SQL_SaveText(Nazione) & "'   ")
            End If

            If Cifre_Controllo <> "" Then
                StbSQL.Append(" AND Liquidita.Cifre_Controllo = '" & Agro_SQL_SaveText(Cifre_Controllo) & "'   ")
            End If

            If Cin <> "" Then
                StbSQL.Append(" AND Liquidita.Cin = '" & Agro_SQL_SaveText(Cin) & "'   ")
            End If

            If Abi <> "" Then
                StbSQL.Append(" AND Liquidita.Abi = '" & Agro_SQL_SaveText(Abi) & "'   ")
            End If

            If Cab <> "" Then
                StbSQL.Append(" AND Liquidita.Cab = '" & Agro_SQL_SaveText(Cab) & "'   ")
            End If

            If Bic <> "" Then
                StbSQL.Append(" AND Liquidita.Bic = '" & Agro_SQL_SaveText(Bic) & "'   ")
            End If

            If Numero <> "" Then
                StbSQL.Append(" AND Liquidita.Numero = '" & Agro_SQL_SaveText(Numero) & "'   ")
            End If

            If Cau_Risorsa <> "" Then
                StbSQL.Append(" AND Liquidita.Cau_Risorsa = '" & Agro_SQL_SaveText(Cau_Risorsa) & "'   ")
            End If

            If Cod_Istituto <> 0 Then
                StbSQL.Append(" AND Liquidita.Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & "   ")
            End If

            If Avviso <> 0 Then
                StbSQL.Append(" AND Liquidita.Avviso = " & Agro_SQL_SaveNum(Avviso) & "   ")
            End If

            If Cod_Contatto <> "" Then
                StbSQL.Append(" AND Liquidita.Cod_Contatto = '" & Agro_SQL_SaveText(Cod_Contatto) & "'   ")
            End If

            If Per_Risorsa <> "" Then
                StbSQL.Append(" AND Ist_Credito.Per_Risorsa = '" & Agro_SQL_SaveText(Per_Risorsa) & "'   ")
            End If

            If Cod_RisUm <> 0 Then
                StbSQL.Append(" AND Risorse_Umane.Cod_RisUm = " & Agro_SQL_SaveNum(Cod_RisUm) & "   ")
            End If

            If Cod_Rapporto <> 0 Then
                StbSQL.Append(" AND Risorse_Umane.Cod_Rapporto = " & Agro_SQL_SaveNum(Cod_Rapporto) & "   ")
            End If

            If Piva_Contatto <> "" Then
                StbSQL.Append(" AND Risorse_Umane.Piva_Contatto = '" & Agro_SQL_SaveText(Piva_Contatto) & "'   ")
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StbSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------

            '################################
            If xOrderBy <> "" Then
                StbSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StbSQL.Append(" ORDER BY Imprese.rag_soc, Contatti.Rag_Soc ")
            End If


            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StbSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    '###################################################################################
    Public Function IBAN_from_CodLiquidita(ByVal Piva As String,
                                         ByVal Cod_Liquidita As Integer,
                                         ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As String


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_R.IBAN_from_CodLiquidita()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim IBAN As String = ""

        Try

            If Cod_Liquidita > 0 Then

                'DT = NewCom_Liquidita_Leggi(objServer, objSession, objPage, _
                '            Piva, _
                '            Cod_Liquidita, _
                '            , , , , , , , , , , , , , , , , , , )

                DT = Leggi_Estesa(Piva,
                                    Cod_Liquidita,
                                    "", "", "", "", "", "", "",
                                    "", 0, 0, "", "", 0, 0, "",
                                    "",
                                    "",
                                    objParametri)

                If Not IsNothing(DT) AndAlso
                       DT.Rows.Count <> 0 Then

                    'Nome banca - c/c 105948 - ABI: 05018 - CAB: 01600 - CIN: Y 
                    'Return RsRisorse.Fields("Istituto_Des").Value & " - c/c " & RsRisorse("Numero").Value & " - ABI: " & RsRisorse("Abi").Value & " - CAB: " & RsRisorse("Cab").Value & " - CIN: " & RsRisorse("Interbancario").Value
                    'Nome banca - IBAN: IT 12 L 12345 12345 123456789012

                    IBAN = DT.Rows(0).Item("Istituto_Des")

                    'If DT.Rows(0).Item("Nazione") <> "" And DT.Rows(0).Item("Nazione") <> "0" Then
                    If DT.Rows(0).Item("Abi") <> "" And DT.Rows(0).Item("Cab") <> "" Then

                        IBAN += " " & DT.Rows(0).Item("Nazione") & " " _
                                        & DT.Rows(0).Item("Cifre_Controllo") & " " _
                                        & DT.Rows(0).Item("Cin") & " " _
                                        & DT.Rows(0).Item("Abi") & " " _
                                        & DT.Rows(0).Item("Cab") & " " _
                                        & DT.Rows(0).Item("Numero") & " "

                    End If

                    If DT.Rows(0).Item("Bic") <> "" And DT.Rows(0).Item("Bic") <> "0" Then
                        IBAN += " Bic/Swift: " & DT.Rows(0).Item("Bic")
                    End If
                    IBAN = Trim(IBAN)

                    'If DT.Rows(0).Item("Bic") = "" Then

                    '    IBAN = DT.Rows(0).Item("Istituto_Des") & _
                    '            " IBAN:  " & DT.Rows(0).Item("Nazione") & " " & _
                    '            DT.Rows(0).Item("Cifre_Controllo") & " " & _
                    '            DT.Rows(0).Item("Cin") & " " & DT.Rows(0).Item("Abi") & " " & _
                    '            DT.Rows(0).Item("Cab") & " " & DT.Rows(0).Item("Numero")

                    'Else

                    '    IBAN = DT.Rows(0).Item("Istituto_Des") & _
                    '            " IBAN:  " & DT.Rows(0).Item("Nazione") & " " & _
                    '            DT.Rows(0).Item("Cifre_Controllo") & " " & _
                    '            DT.Rows(0).Item("Cin") & " " & DT.Rows(0).Item("Abi") & " " & _
                    '            DT.Rows(0).Item("Cab") & " " & DT.Rows(0).Item("Numero") & _
                    '            " Bic/Swift: " & DT.Rows(0).Item("Bic")

                    'End If

                End If

            End If

            DT = Nothing

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return IBAN

    End Function

    '##############################################################################################
    Public Function LeggiRisFinanziarie_BYCodContatto(ByVal Piva As String,
                                                        ByVal Cod_Istituto As Integer,
                                                        ByVal Filiale As Integer,
                                                        ByVal Per_Risorsa As Integer,
                                                        ByVal Cod_Contatto As String,
                                                        ByVal Cau_Risorsa As enum_Liquidita_CauRisorsa,
                                                        ByVal xFiltroAggiuntivo As String,
                                                        ByVal xOrderBy As String,
                                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                                        ) As DataTable


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_R.LeggiRisFinanziarie_BYCodContatto()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0

            StrSQL.Append(" SELECT Ist_Credito.Cod_Istituto, Istituto_Des, Filiale, Per_Risorsa, ")

            StrSQL.Append(" Liquidita.Piva, Liquidita.Cod_Liquidita, Liquidita.Nazione, Liquidita.Cifre_Controllo, Liquidita.Cin, Liquidita.Abi, Liquidita.Cab, Liquidita.Bic,  ")
            StrSQL.Append(" Liquidita.Numero, Liquidita.Saldo_Attuale, Liquidita.Saldo_Iniziale, Liquidita.Cau_Risorsa, Liquidita.Cod_Istituto, Liquidita.Avviso, ")
            StrSQL.Append(" Liquidita.Importo_Avviso, Liquidita.Note, Liquidita.Cod_Contatto ")

            StrSQL.Append(" FROM  Ist_Credito ")
            'StrSQL.Append(" INNER JOIN Liquidita ON Ist_Credito.Piva = Liquidita.Piva AND Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            'CORREZIONE BUG DEL 07/01/2013: la piva di ist_credito è quella del superuser!
            StrSQL.Append(" INNER JOIN Liquidita ON Ist_Credito.Cod_Istituto = Liquidita.Cod_Istituto ")
            StrSQL.Append("                     AND Ist_Credito.Piva = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")

            StrSQL.Append(" WHERE Ist_Credito.Validita_Inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & "  ")
            StrSQL.Append(" AND   Ist_Credito.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & "  ")

            If Piva <> "" Then
                StrSQL.Append(" AND    Liquidita.Piva = '" & Agro_SQL_SaveText(Piva) & "'  " + vbCrLf)
            End If

            If Cod_Istituto <> 0 Then
                StrSQL.Append(" AND     Ist_Credito.Cod_Istituto = " & Agro_SQL_SaveNum(Cod_Istituto) & "  " + vbCrLf)
            End If

            If Cod_Contatto <> "" Then
                StrSQL.Append(" AND    Liquidita.Cod_Contatto = '" & Agro_SQL_SaveText(Cod_Contatto) & "'  " + vbCrLf)
            End If

            If Cau_Risorsa <> enum_Liquidita_CauRisorsa.Nessuna Then
                StrSQL.Append(" AND     Liquidita.Cau_Risorsa = " & Agro_SQL_SaveNum(Cau_Risorsa) & "  " + vbCrLf)
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

End Class

'############################################################
'############################################################
'############################################################
'############################################################



Public Class Liquidita_W
    Inherits AgronicaCoreDataProvider.DataProvider

    '############################################################
    Public Function ModificaSaldo(
                                ByVal Cod_Liquidita As Int32,
                                ByVal Importo As Decimal,
                                    ByVal xFiltroAggiuntivo As String,
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                    ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_W.ModificaSaldo()"

        '====================================================================================
        'Parametri opzionali :
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        '------------------------------

        Try

            If Cod_Liquidita = -1 Then
                Throw New Exception("Parametro non corretto nella query (Cod_Liquidita obbligatorio)")
            End If

            '---------------------------------------------
            StrSQL.Length = 0

            StrSQL.Append(" UPDATE Liquidita SET ")
            StrSQL.Append("        Saldo_Attuale = Saldo_Attuale + " & Agro_SQL_SaveNum(Importo) & " ")
            StrSQL.Append("        ,UserName_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "'")
            StrSQL.Append(" WHERE  Cod_Liquidita = " & Agro_SQL_SaveNum(Cod_Liquidita) & " ")

            '----------------------------------------------------------------------
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


    Public Function Scrivi(ByVal PIVA As String,
                                ByVal Sa_Cod As Integer,
                                ByVal Cod_Liquidita As Integer,
                                ByVal Cod_Contatto As String,
                                ByVal Riferimento As String,
                                ByVal Cau_Risorsa As String,
                                ByVal Cod_Istituto As Integer,
                                ByVal Numero As String,
                                ByVal Abi As String,
                                ByVal Cab As String,
                                ByVal Cin As String,
                                ByVal Cifre_Controllo As String,
                                ByVal Nazione As String,
                                ByVal Bic As String,
                                ByVal Interbancario As String,
                                ByVal Saldo_Attuale As Decimal,
                                ByVal Saldo_Iniziale As Decimal,
                                ByVal Avviso As Integer,
                                ByVal Importo_Avviso As Decimal,
                                ByVal Note As String,
                                ByVal Rilevamento As Double,
                                ByVal Data_Rilevamento As Date,
                                ByVal Offset As Double,
                                ByVal ChkDefault As Integer,
                                ByVal ChkAbilitazione As Integer,
                                ByVal Validita_Inizio As Date,
                                ByVal Validita_Fine As Date,
                                ByVal xFiltroAggiuntivo As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                , Optional ByVal username_creazione As String = "" _
                                , Optional ByVal username_modifica As String = ""
                                ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_W.Scrivi()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        '------------------------------

        Try

            If username_creazione = "" Then
                username_creazione = objParametri.UsernameOperazione
            End If

            If username_modifica = "" Then
                username_modifica = objParametri.UsernameOperazione
            End If


            '---------------------------------------------
            'Query per l'inserimento dei dati                 ' #### CLASSE ####
            StrSQL.Length = 0

            StrSQL.Append("INSERT INTO Liquidita ")
            StrSQL.Append("                    (Piva,           Sa_Cod,            Cod_Liquidita,   Cod_Contatto,   ")
            StrSQL.Append("                     Riferimento,    Cau_Risorsa,       Cod_Istituto,                    ")
            StrSQL.Append("                     Numero,         Abi,               Cab,                             ")
            StrSQL.Append("                     Cin,            Cifre_Controllo,   Nazione,         Bic,            ")
            StrSQL.Append("                     Interbancario,  Saldo_Attuale,     Saldo_Iniziale,                  ")
            StrSQL.Append("                     Avviso,         Importo_Avviso,    Note,                            ")

            StrSQL.Append("                     Rilevamento,    Data_Rilevamento,    Offset,  ")
            StrSQL.Append("                     ChkDefault,     ChkAbilitazione, ")

            StrSQL.Append("                    Inviato,            DataInvio, ")
            StrSQL.Append("                    UserName_Creazione, UserName_Modifica, ")
            StrSQL.Append("                    Validita_Inizio,    Validita_Fine ")
            StrSQL.Append("                    ) ")

            StrSQL.Append("VALUES (")
            StrSQL.Append("          '" & Agro_SQL_SaveText(PIVA) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Sa_Cod))
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Cod_Liquidita))
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Cod_Contatto) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Riferimento) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Cau_Risorsa) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Cod_Istituto))
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Numero) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Abi) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Cab) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Cin) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Cifre_Controllo) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Nazione) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Bic) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Interbancario) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Saldo_Attuale) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Saldo_Iniziale) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Avviso) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Importo_Avviso) & " ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Note) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Rilevamento) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Data_Rilevamento) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Offset) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(ChkDefault) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(ChkAbilitazione) & " ")

            StrSQL.Append("         , 0  ")
            StrSQL.Append("         , Null  ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Inizio) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Fine) & "  ")
            StrSQL.Append(")")

            '----------------------------------------------------------------------
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


    Public Function Modifica(ByVal PIVA As String,
                                    ByVal Sa_Cod As Integer,
                                    ByVal Cod_Liquidita As Integer,
                                    ByVal Cod_Contatto As String,
                                    ByVal Riferimento As String,
                                    ByVal Cau_Risorsa As String,
                                    ByVal Cod_Istituto As Integer,
                                    ByVal Numero As String,
                                    ByVal Abi As String,
                                    ByVal Cab As String,
                                    ByVal Cin As String,
                                    ByVal Cifre_Controllo As String,
                                    ByVal Nazione As String,
                                    ByVal Bic As String,
                                    ByVal Interbancario As String,
                                    ByVal Saldo_Attuale As Decimal,
                                    ByVal Saldo_Iniziale As Decimal,
                                    ByVal Avviso As Integer,
                                    ByVal Importo_Avviso As Decimal,
                                    ByVal Note As String,
                                    ByVal Validita_Inizio As Date,
                                    ByVal Validita_Fine As Date,
                                    ByVal ChkDefault As Integer,
                                        ByVal xFiltroAggiuntivo As String,
                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                        ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_W.Modifica()"

        '====================================================================================
        'Parametri opzionali :
        '   objConnessione = nothing    =>  viene creata (e distrutta) con StringaConnessione
        '   DirectoryLOG = ""           =>  viene usato il valore di default
        '   FileLOG = ""                =>  viene usato il valore di default
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        '------------------------------

        Try

            '---------------------------------------------
            'Query per l'inserimento dei dati                 ' #### CLASSE ####
            StrSQL.Length = 0

            StrSQL.AppendLine("UPDATE Liquidita SET ")
            StrSQL.AppendLine("    Cod_Contatto      = '" & Agro_SQL_SaveText(Cod_Contatto) & "'")
            StrSQL.AppendLine("   ,Riferimento       = '" & Agro_SQL_SaveText(Riferimento) & "'")
            StrSQL.AppendLine("   ,Cau_Risorsa       = '" & Agro_SQL_SaveText(Cau_Risorsa) & "'")
            StrSQL.AppendLine("   ,Cod_Istituto      = " & Agro_SQL_SaveNum(Cod_Istituto) & " ")
            StrSQL.AppendLine("   ,Numero            = '" & Agro_SQL_SaveText(Numero) & "'")
            StrSQL.AppendLine("   ,Abi               = '" & Agro_SQL_SaveText(Abi) & "'")
            StrSQL.AppendLine("   ,Cab               = '" & Agro_SQL_SaveText(Cab) & "'")
            StrSQL.AppendLine("   ,Cin               = '" & Agro_SQL_SaveText(Cin) & "'")
            StrSQL.AppendLine("   ,Cifre_Controllo   = '" & Agro_SQL_SaveText(Cifre_Controllo) & "'")
            StrSQL.AppendLine("   ,Nazione           = '" & Agro_SQL_SaveText(Nazione) & "'")
            StrSQL.AppendLine("   ,Bic               = '" & Agro_SQL_SaveText(Bic) & "'")
            StrSQL.AppendLine("   ,Interbancario     = '" & Agro_SQL_SaveText(Interbancario) & "'")
            StrSQL.AppendLine("   ,Saldo_Attuale     = " & Agro_SQL_SaveNum(Saldo_Attuale) & " ")
            StrSQL.AppendLine("   ,Saldo_Iniziale    = " & Agro_SQL_SaveNum(Saldo_Iniziale) & " ")
            StrSQL.AppendLine("   ,Avviso            = " & Agro_SQL_SaveNum(Avviso) & " ")
            StrSQL.AppendLine("   ,Importo_Avviso    = " & Agro_SQL_SaveNum(Importo_Avviso) & " ")
            StrSQL.AppendLine("   ,Note              = '" & Agro_SQL_SaveText(Note) & "'")

            StrSQL.AppendLine("   ,Inviato           = 0 ")
            StrSQL.AppendLine("   ,DataInvio         = Null ")
            StrSQL.AppendLine("   ,UserName_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "'")
            StrSQL.AppendLine("   ,Validita_Inizio   =  " & Agro_SQL_SaveDate(Validita_Inizio))
            StrSQL.AppendLine("   ,Validita_Fine     =  " & Agro_SQL_SaveDate(Validita_Fine))
            StrSQL.AppendLine("   ,ChkDefault        =  " & Agro_SQL_SaveNum(ChkDefault))
            StrSQL.AppendLine(" WHERE   Cod_Liquidita = " & Agro_SQL_SaveNum(Cod_Liquidita) & " ")
            '------------------------------

            If PIVA <> "" Then
                StrSQL.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(PIVA) & "'   ")
            End If

            If Sa_Cod <> 0 Then
                StrSQL.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & "   ")
            End If

            '----------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
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



    Public Function Cancella(
                                ByVal PIVA As String,
                                ByVal Sa_Cod As Integer,
                                ByVal Cod_Liquidita As Integer,
                                ByVal Cod_Contatto As String,
                                    ByVal xFiltroAggiuntivo As String,
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                    ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Liquidita_W.Cancella()"

        '====================================================================================
        'Parametri opzionali :
        '   objConnessione = nothing    =>  viene creata (e distrutta) con StringaConnessione
        '   DirectoryLOG = ""           =>  viene usato il valore di default
        '   FileLOG = ""                =>  viene usato il valore di default
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        '------------------------------

        Try

            '---------------------------------------------
            'Query per l'inserimento dei dati                 ' #### CLASSE ####
            StrSQL.Length = 0

            '---------------------------------------------
            If objParametri.FlagCancellazioneLogica = AgronicaCoreDataProvider.AgronicaCoreParametri.enumCancellazioneLogica.CancellazioneLogica Then

                StrSQL.Append(" UPDATE  Liquidita ")
                StrSQL.Append(" SET ")
                StrSQL.Append("          Username_Modifica = '" & objParametri.UsernameOperazione & "' ")
                StrSQL.Append("         ,Inviato = -1 ")
                StrSQL.Append(" WHERE    Inviato > 0 ")
            Else

                StrSQL.Append(" DELETE ")
                StrSQL.Append(" FROM  Liquidita ")
                StrSQL.Append(" WHERE   Inviato = 0 ")
            End If

            If PIVA <> "" Then
                StrSQL.Append(" AND Piva = '" & Agro_SQL_SaveText(PIVA) & "'   ")
            End If

            If Sa_Cod <> 0 Then
                StrSQL.Append(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & "   ")
            End If

            If Cod_Liquidita <> 0 Then
                StrSQL.Append(" AND   Cod_Liquidita = " & Cod_Liquidita & "  ")
            End If

            If Cod_Contatto <> "" Then
                StrSQL.Append(" AND Cod_Contatto = '" & Agro_SQL_SaveText(Trim(Cod_Contatto)) & "'   ")
            End If


            '----------------------------------------------------------------------
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
