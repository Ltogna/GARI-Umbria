Imports System.Text
Imports System.Text.RegularExpressions
Imports AgronicaCoreAnagrafeDAL
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDTOStd.InData.Agenda
Imports AgronicaCoreModelsSTD.anagrafiche
Imports AgronicaCoreModelsSTD.profilazione
Imports AgronicaCoreVarieDAL

Public Class Utenti_Visibilita

#Region "Lettura"

    Public Function LeggiAziendeVisibilita(utenti As List(Of UtenteDTO),
                                           imprese As List(Of ImpresaDto),
                                           objParametri_Server As AgronicaCoreParametri,
                                           objParametri_Utenti As AgronicaCoreParametri) As List(Of ImpresexUtentiVisibilita)

        Dim objUtenti As New AgronicaCoreUtentiBIZ.Utenti
        Dim listaVisibilita As New List(Of ImpresexUtentiVisibilita)

        Dim usernames As New List(Of String)
        If utenti.Any() Then
            usernames = utenti.Select(Function(u) u.UserName)
        End If
        Dim usersHash As New Dictionary(Of String, String)
        objUtenti.LeggiDettagliMinimi(usernames, objParametri_Utenti).
            ForEach(Sub(u) usersHash.Add(u.UserName.ToLowerInvariant().Trim(), u.Cognome & " " & u.Nome & " (" & u.UserName & ")"))

        If Not imprese.Any() Then
            'Limito la visibilità alle imprese visibili all'utente attuale
            imprese = LeggiPiveCapostipiti(objParametri_Utenti.UsernameOperazione, objParametri_Utenti).
                Select(Function(p) New ImpresaDto With {.piva = p}).ToList()
        End If

        Return LeggiAppoggioXImprese(utenti, imprese, objParametri_Server).
            Select.AsParallel.
            Select(Function(i) New ImpresexUtentiVisibilita With {
                .Username = i.Item("Username"),
                .DettagliUtente = usersHash(i.Item("Username").ToString().ToLowerInvariant().Trim()),
                .piva = i.Item("Piva"),
                .rag_soc = i.Item("rag_soc"),
                .Sa_Cod = i.Item("Sa_Cod"),
                .Sa_Nome = i.Item("Sa_Nome")
            }).ToList()
    End Function

    ''' <summary>
    ''' Carica la visibilità degli utenti specificati, tenendo in considerazione
    ''' la visibilità dell'utente che ha richiesto l'operazione.
    ''' </summary>
    ''' <param name="utenti">Utenti di cui si vuole conoscere la visibilità</param>
    ''' <param name="restringiVisibilita">Imprese da considerare nella lettura della visibilità.
    ''' Funge da filtro sulle aziende che si andranno a leggere, col risultato di mostrare la
    ''' visibilità degli utenti solo sul sottoinsieme di imprese indicato. Passare una lista
    ''' vuota per evitare di filtrare le aziende.</param>
    Public Function LeggiVisibilitaUtenti(
        utenti As List(Of UtenteDTO), restringiVisibilita As List(Of ImpresaDto),
        objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri,
        Optional leggiDettagli As Boolean = False
    ) As List(Of ImpresexUtentiVisibilita)

        Dim listaVisibilita As New List(Of ImpresexUtentiVisibilita)
        Dim usernames = utenti.AsParallel.Select(Function(u) u.UserName.ToLower).ToHashSet
        Dim currUserVisibility = LeggiVisibilitaUtente(
            objParametri_Utenti.UsernameOperazione,
            restringiVisibilita.Select(Function(i) i.piva).ToList(),
            objParametri_Utenti,
            objParametri_Server
        )
        'Carico i dati degli utenti -- usata per ricerca visibilità
        Dim d As String = String.Empty
        Dim dettagli = New Dictionary(Of String, String)
        If leggiDettagli Then
            ReplaceWithBasicData(utenti, objParametri_Server, objParametri_Utenti)
            dettagli = utenti.ToDictionary(Function(u) u.UserName, Function(u) u.Nome & " " & u.Cognome)
        End If

        For Each username In usernames
            If (dettagli.Count > 0) Then
                dettagli.TryGetValue(username, d)
            End If
            Dim visib = LeggiVisibilitaUtente(
                            username, New List(Of String),
                            objParametri_Utenti, objParametri_Server
                        ).Intersect(currUserVisibility).AsParallel.
                          Select(Function(i) New ImpresexUtentiVisibilita With {
                            .Username = username,
                            .DettagliUtente = d,
                            .piva = i.piva,
                            .rag_soc = i.rag_soc,
                            .Sa_Cod = i.Sa_Cod,
                            .Sa_Nome = i.Sa_Nome
                          })
            '.DettagliUtente = utente.Cognome & " " & utente.Nome & " (" & utente.UserName & ")",
            listaVisibilita.AddRange(visib)
        Next
        Return listaVisibilita
    End Function

    Public Function LeggiVisibilitaGruppi(gruppi As IEnumerable(Of Integer), restringiVisibilita As List(Of ImpresaDto), params As ObjParams)
        Dim gropupsReader As New Gruppi_UtenteBiz(params.ObjParametri_Server, params.ObjParametri_Utenti)
        Dim users = gropupsReader.GetUtentiFromGruppoUtenti(gruppi).Select.AsParallel.
            Select(Function(r) If(IsDBNull(r("UserName")), "", r("UserName"))).
            Where(Function(username) Not String.IsNullOrWhiteSpace(username)).
            Select(Function(username) New UtenteDTO With {.UserName = username}).
            ToList
        Return LeggiVisibilitaUtenti(
            users, restringiVisibilita.ToList,
            params.ObjParametri_Server, params.ObjParametri_Utenti, True
        )
    End Function

    Private Sub ReplaceWithBasicData(
        ByRef users As IEnumerable(Of UtenteDTO),
        objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri
    )
        Dim objUtenti As New AgronicaCoreUtentiBIZ.Utenti
        Dim usernames = users.AsParallel.Select(Function(u) u.UserName.ToLower).ToHashSet
        Dim listaUtenti = objUtenti.Carica_Utenti_Dati_Base(enum_Id_Servizio.GiasOnline, objParametri_Server, objParametri_Utenti)
        If IsNothing(users) OrElse users.Count = 0 Then
            users = listaUtenti.ListaDatiBaseUtente.AsParallel.
                Select(Function(u) New UtenteDTO With {
                    .UserName = u.UserName,
                    .Nome = u.Nome,
                    .Cognome = u.Cognome
                }).ToList()
        Else
            users = listaUtenti.ListaDatiBaseUtente.AsParallel.
                Where(Function(u) usernames.Contains(u.UserName.ToLower)).
                Select(Function(u) New UtenteDTO With {
                    .UserName = u.UserName,
                    .Nome = u.Nome,
                    .Cognome = u.Cognome
                }).ToList()
        End If
    End Sub

    ''' <summary>
    ''' Legge alcuni dati aggiuntivi in riferimento alle aziende specificate.
    ''' </summary>
    ''' <param name="toRead">Collezione di imprese da leggere. (Basta che sia valorizzato il campo piva)</param>
    ''' <returns>Una DataTable con i dati riferiti alle aziende o Nothing se non sono state specificate aziende da leggere</returns>
    Public Function GetImpreseFromVisibilita(toRead As IEnumerable(Of IImpresaDto), params As ObjParams) As DataTable
        If toRead.Any Then
            Dim anagrafe As New AgronicaCoreAnagrafeDAL.Imprese_Read
            Dim pive = toRead.AsParallel.
                Select(Function(impresa) "'" & impresa.piva & "'").
                Distinct
            'If pive.Count > 10_000 Then
            '    Return Nothing
            'End If

            Dim xIn = "i.piva in (" & pive.Aggregate(Function(acc, piva) acc & ", " & piva) & ")"
            Dim imprese As DataTable = anagrafe.Leggi_x_anagraficaVisibilita_Utente_NG(
                String.Empty, xIn, "i.piva ASC, i.rag_soc ASC",
                params.ObjParametri_Server, params.ObjParametri_Utenti,
                False, False
            )
            Return imprese
        End If
        Return Nothing
    End Function

    Public Function HaVisibilitaTotale(username As String, objParametri_utenti As AgronicaCoreParametri) As Boolean
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable = objProfilo.Leggi(
            username, CInt(enum_Id_Servizio.GiasOnline),
            enumSelezioneVariabile.Selezione_TabellaCompleta,
            "", "",
            objParametri_utenti
        )
        If DTProfilo.Rows.Count = 1 AndAlso DTProfilo.Rows(0).Item("Descrizione_2") = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function HannoVisibilitaTotale(username As IEnumerable(Of String), objParametri_utenti As AgronicaCoreParametri) As IEnumerable(Of UtenteVisibilitaTotale)
        Dim xFilter = " Utenti_Profili.Id_Servizio = " & enum_Id_Servizio.GiasOnline
        Dim prepareFilter = Function(acc, u)
                                If u.index Mod 10 = 0 Then
                                    acc.name &= ", " & vbNewLine & u.name
                                Else
                                    acc.name &= ", " & u.name
                                End If
                                Return acc
                            End Function
        Dim uu = username.AsParallel.
            DefaultIfEmpty(String.Empty).
            Select(Function(str, i) New With {.name = "'" & str & "'", .index = i}).
            GroupBy(Function(u) u.index \ 10_000).
            Select(Function(batch) batch.Aggregate(prepareFilter).name).
            ToList ' lista di stringhe aggregate
        Dim result As New List(Of UtenteVisibilitaTotale)

        For Each group In uu
            Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
            Dim pt = objProfilo.LeggiMassivo(group, xFilter, String.Empty, objParametri_utenti).
                Select.AsParallel.
                Select(Function(dr) New UtenteVisibilitaTotale(dr("Utente"), String.IsNullOrWhiteSpace(dr("Descrizione_1"))))
            result.AddRange(pt)
        Next
        Return result
    End Function

    Public Function HannoVisibilitaTotaleAggregato(username As IEnumerable(Of String), objParametri_utenti As AgronicaCoreParametri) As Boolean
        Dim prepareFilter = Function(acc, u)
                                If u.index Mod 100 = 0 Then
                                    acc.name &= ", --" & u.index & vbNewLine & u.name
                                ElseIf u.index Mod 10 = 0 Then
                                    acc.name &= ", " & vbNewLine & u.name
                                Else
                                    acc.name &= ", " & u.name
                                End If
                                Return acc
                            End Function
        Dim uu = username.AsParallel.
            DefaultIfEmpty(String.Empty).
            Select(Function(str, i) New With {.name = "'" & str & "'", .index = i}).
            GroupBy(Function(u) u.index \ 10_000).
            Select(Function(batch) batch.Aggregate(prepareFilter).name).
            ToList ' lista di stringhe aggregate

        For Each group In uu
            Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
            Dim allHaveFullVis = objProfilo.Leggi(
                String.Empty, enum_Id_Servizio.GiasOnline, enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
                "( Utente IN ( " & group & " ))", String.Empty, objParametri_utenti
            ).Select.Select(Function(dr) If(IsDBNull(dr("Descrizione_1")), String.Empty, CStr(dr("Descrizione_1")))).
            All(Function(vis) String.IsNullOrWhiteSpace(vis))
            If Not allHaveFullVis Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function ComparaVisibilitaUtenti(utenti As List(Of String), objParametri_utenti As AgronicaCoreParametri, objParametri_server As AgronicaCoreParametri) As Boolean
        If Not utenti.Any() Then
            Return 0
        ElseIf utenti.Count = 1 Then
            Return ComparaVisibilitaUtenti(
                    {utenti(0), objParametri_utenti.UsernameOperazione}.ToList,
                    objParametri_utenti, objParametri_server
                )
        End If

        Dim same = 0
        Dim A = LeggiVisibilitaUtente(utenti(0), New List(Of String), objParametri_utenti, objParametri_server).
                            Select(Function(i) i.piva).ToList()
        For i As Integer = 1 To utenti.Count - 1
            Dim B = LeggiVisibilitaUtente(utenti(i), New List(Of String), objParametri_utenti, objParametri_server).
                            Select(Function(j) j.piva).ToList()

            Dim AnB = A.Intersect(B)
            If AnB.Count = A.Count AndAlso AnB.Count = B.Count Then
                'Intersezione ha lo stesso numero di elementi dei due insiemi in verifica
                ' ==> gli insiemi sono uguali
                same = 0
            Else
                Dim AIB = A.Except(B)
                Dim BIA = B.Except(A)

                If AIB.Count > BIA.Count Then
                    'Togliendo l'intersezione da A ho più elementi che togliendola da B
                    ' ==> A è più grande
                    Return 1
                Else
                    'Togliendo l'intersezione da A ho meno o lo stesso numero di elementi che togliendola da B
                    ' ==> A è minore o uguale a B
                    Return -1
                End If
            End If
        Next

        Return same
    End Function

    ''' <param name="imprese">Collezione di imprese per cui voglio sapere se sono padri o meno</param>
    ''' <returns>Le pive delle imprese padre tra quelle specificate</returns>
    Public Function CheckIfAnyHasSons(imprese As IEnumerable(Of IImpresaDto), params As ObjParams)
        Dim hierarchy As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        Dim fathers = hierarchy.FilterBusinessWithSons(
            imprese.AsParallel.Select(Function(b) b.piva),
            params.ObjParametri_Server
        )
        Return fathers
    End Function

#End Region

#Region "Scrittura"

    Public Sub ModificaVisibilitaAziendaUtenti(
        utenti As IEnumerable(Of IUtente), imprese As List(Of ImpresaDto),
        sovrascrivi As Boolean, bloccaSeHaVisibilita As Boolean,
        objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri
    )
        Try
            Const noUsersWithVisibility = -1
            Dim params As New ObjParams With {
                .ObjParametri_Server = objParametri_Server,
                .ObjParametri_Utenti = objParametri_Utenti
            }

            AgronicaCoreDataProvider.ConnessioniTransazioni.ApriConnessione(True, objParametri_Utenti)

            Dim usernames = utenti.AsParallel.Select(Function(u) u.UserName)
            If Not VerificaEsistenzaUtenti(usernames, objParametri_Utenti) Then
                Throw New Exception("One or more users do not exist.")
            End If
            If bloccaSeHaVisibilita AndAlso CheckUsersVisibility(usernames, objParametri_Utenti) > noUsersWithVisibility Then
                Throw New Exception("Visibility has already been set for one or more users.")
            End If
            Dim usernamesBatch = utenti.AsParallel.
                  Select(Function(u, i) New With {.user = u, .index = i}).
                  GroupBy(Function(u) u.index \ 10_000).ToList

            If sovrascrivi Then
                usernamesBatch.ForEach(Sub(batch) OverwriteVisibility(batch.Select(Function(a) a.user), imprese, params))
            Else
                usernamesBatch.ForEach(Sub(batch) AddToVisibility(batch.Select(Function(a) a.user), imprese, params))
                'For Each utente In utenti
                '    Dim inVisibilita = LeggiVisibilitaUtente(utente.UserName, New List(Of String), objParametri_Utenti, objParametri_Server)
                '    Dim nuovaVisibilita = imprese.Union(inVisibilita).ToList()
                '    AssegnaVisibilita(utente.UserName, nuovaVisibilita, objParametri_Server, objParametri_Utenti)
                'Next
            End If

            UpdateAppoggioIfNotTooMany(utenti, params)

            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Utenti)
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Utenti)
        Catch ex As Exception
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(2, objParametri_Utenti)
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Utenti)
            Throw ex
        End Try
    End Sub

    Private Sub UpdateAppoggioIfNotTooMany(users As IEnumerable(Of IUtente), params As ObjParams)
        Dim xVisibAppoggio As New Utenti
        Dim leggiUtenti As New AgronicaCoreUtentiDAL.Utenti_Read
        Dim configSiti As New AgronicaCoreVarieDAL.Configurazione_Siti_R
        Dim chiaveStr = configSiti.Leggi_Valore(
            Sito_Cod:=0, Chiave:="AgroProfilazione_MaxUtentiCaricatiDefault",
            xFiltroAggiuntivo:=String.Empty, xOrderBy:=String.Empty, params.ObjParametri_Server
        )
        Dim smallMediumBusiness = If(String.IsNullOrWhiteSpace(chiaveStr), 200, CInt(chiaveStr))
        If users.Count <= smallMediumBusiness Then
            For Each user In users
                xVisibAppoggio.InizializzaTabellaUtentiVisibilitaAppoggio(
                    user.UserName, CInt(enum_Id_Servizio.GiasOnline),
                    params.ObjParametri_Server, params.ObjParametri_Utenti
                )
            Next
        End If
    End Sub

    Public Sub OverwriteVisibility(utenti As IEnumerable(Of IUtente), imprese As IEnumerable(Of IImpresaDto), params As ObjParams)
        Dim usernames = utenti.AsParallel.Select(Function(u) u.UserName)
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Write

        If imprese.Any Then
            Dim listaPive = CaricaGerarchia(imprese.ToList, params.ObjParametri_Server)
            objProfilo.CancellaMassivo(usernames, String.Empty, params.ObjParametri_Utenti)
            objProfilo.ScriviMassivo(
                usernames,
                CreaFiltroXMLPermessi(listaPive),
                CreaFiltroSQLPermessi(listaPive),
                params.ObjParametri_Utenti
            )
        Else 'Assign visibility over every business
            objProfilo.CancellaMassivo(usernames, String.Empty, params.ObjParametri_Utenti)
            objProfilo.ScriviMassivo(usernames, String.Empty, String.Empty, params.ObjParametri_Utenti)
        End If
    End Sub

    Public Sub AddToVisibility(utenti As IEnumerable(Of IUtente), imprese As IEnumerable(Of IImpresaDto), params As ObjParams)
        Dim profili As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim profiliW As New AgronicaCoreUtentiDAL.Utenti_Profili_Write
        Dim usernames = utenti.AsParallel.Select(Function(u) u.UserName).ToHashSet
        Dim pive = imprese.AsParallel.Select(Function(i) i.piva).ToHashSet
        Dim getStringOrDefault = Function(r As DataRow, field As String) If(IsDBNull(r(field)), String.Empty, r(field))

        Dim dt = profili.LeggiMassivo(usernames, String.Empty, String.Empty, params.ObjParametri_Utenti)
        Dim sameVisGr = dt.Select.AsParallel.GroupBy(Function(row) getStringOrDefault(row, "Descrizione_1"))
        For Each vis In sameVisGr
            Dim ancestors = LeggiPiveCapostipiti(vis.Key).Except({"###########"}).ToList
            Dim newVis = ancestors.Union(pive).Distinct.Select(Function(piva) New ImpresaDto With {.piva = piva})

            Dim listaPive = CaricaGerarchia(newVis, params.ObjParametri_Server)
            profiliW.CancellaMassivo(usernames, String.Empty, params.ObjParametri_Utenti)
            profiliW.ScriviMassivo(
                usernames,
                CreaFiltroXMLPermessi(listaPive),
                CreaFiltroSQLPermessi(listaPive),
                params.ObjParametri_Utenti
            )
        Next
    End Sub

    Public Sub ImpostaVisibilitaAzienda(
        utente As UtenteDTO,
        imprese As List(Of ImpresaDto),
        bloccaSeUtenteEsiste As Boolean,
        bloccaSeHaVisibilita As Boolean,
        objParametri_Server As AgronicaCoreParametri,
        objParametri_Utenti As AgronicaCoreParametri,
        Optional initTransition As Boolean = True
    )
        Dim xVisibAppoggio As New Utenti
        'Dim xVisibAppoggio_R As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
        Dim esisteUtente As Boolean = VerificaEsistenzaUtente(utente.UserName, objParametri_Utenti)
        Dim username = utente.UserName

        Try
            If initTransition Then
                AgronicaCoreDataProvider.ConnessioniTransazioni.ApriConnessione(True, objParametri_Utenti)
            End If

            If bloccaSeUtenteEsiste AndAlso esisteUtente Then
                Throw New Exception("User already exists.")
            End If
            If bloccaSeHaVisibilita AndAlso VerificaVisibilitaUtente(utente.UserName, objParametri_Utenti) Then
                Throw New Exception("Visibility has already been set for user " & utente.UserName)
            End If

            If Not esisteUtente Then
                username = creaUtenteBase(utente, objParametri_Server, objParametri_Utenti)
            End If

            AssegnaVisibilita(username, imprese, objParametri_Server, objParametri_Utenti)
            If initTransition Then
                AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Utenti)
                AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Utenti)
            End If
        Catch ex As Exception
            If initTransition Then
                AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(2, objParametri_Utenti)
                AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Utenti)
            End If
            Throw ex
        End Try

    End Sub

    Public Sub AssegnaVisibilitaNulla(
        utente As IUtente,
        objParametri_Server As AgronicaCoreParametri,
        objParametri_Utenti As AgronicaCoreParametri
    )
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Write
        'Dim xVisibAppoggio_R As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
        Dim esisteUtente As Boolean = VerificaEsistenzaUtente(utente.UserName, objParametri_Utenti)
        If Not esisteUtente Then
            Throw New Exception("User does not exist!")
        End If
        Dim fittizia As List(Of ImpresaDto) = {New ImpresaDto("###########")}.ToList

        If VerificaVisibilitaUtente(utente.UserName, objParametri_Utenti) Then
            objProfilo.Cancella(utente.UserName, CInt(enum_Id_Servizio.GiasOnline), "", objParametri_Utenti)
        End If
        AssegnaVisibilita(utente.UserName, fittizia, objParametri_Server, objParametri_Utenti)
        'xVisibAppoggio.InizializzaTabellaUtentiVisibilitaAppoggio(
        '    username, CInt(enum_Id_Servizio.GiasOnline),
        '    objParametri_Server, objParametri_Utenti
        ')
    End Sub

    Public Sub CopiaVisibilitaUtenti(base As IEnumerable(Of String), template As String, objParametri_utenti As AgronicaCoreParametri)
        Dim objProfilo_R As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim objProfilo_W As New AgronicaCoreUtentiDAL.Utenti_Profili_Write
        Dim DTProfilo As DataTable = objProfilo_R.Leggi(
            template, CInt(enum_Id_Servizio.GiasOnline),
            enumSelezioneVariabile.Selezione_TabellaCompleta,
            "", "",
            objParametri_utenti
        )

        If DTProfilo.Rows.Count <> 1 Then
            Throw New ArgumentOutOfRangeException()
        End If

        Dim enu = base.GetEnumerator
        While enu.MoveNext
            objProfilo_W.Cancella(enu.Current, enum_Id_Servizio.GiasOnline, "", objParametri_utenti)
            objProfilo_W.Scrivi(enu.Current, enum_Id_Servizio.GiasOnline,
                DTProfilo.Rows(0).Item("Descrizione_1"),
                DTProfilo.Rows(0).Item("Descrizione_2"),
                0, 0,
                AGRODATAINIZIO, AGRODATAFINE,
                objParametri_utenti
            )
        End While

    End Sub

    Public Sub RimuoviImpreseDaVisibilitaUtenti(
        utenti As IEnumerable(Of IUtente),
        imprese As IEnumerable(Of IImpresaDto),
        objParametri_server As AgronicaCoreParametri,
        objParametri_utenti As AgronicaCoreParametri
    )
        Dim enu = utenti.GetEnumerator
        Dim toRemove As IEnumerable(Of ImpresaDto)
        If imprese.Any() Then
            toRemove = imprese
        Else
            toRemove = LeggiVisibilitaUtente(objParametri_utenti.UtenteUsername, New List(Of String), objParametri_utenti, objParametri_server)
        End If

        While enu.MoveNext
            Dim visibilita As IEnumerable(Of ImpresaDto) = LeggiVisibilitaUtente(
                enu.Current.UserName, New List(Of String),
                objParametri_utenti, objParametri_server
            )
            Dim newVisibility = visibilita.ToList()
            If HaVisibilitaTotale(objParametri_utenti.UtenteUsername, objParametri_utenti) AndAlso Not imprese.Any() Then
                newVisibility = New List(Of ImpresaDto)()
            Else
                newVisibility.RemoveAll(Function(i) toRemove.Any(Function(tr) tr.piva = i.piva))
            End If

            If newVisibility.Any Then
                AssegnaVisibilita(
                    enu.Current.UserName, newVisibility,
                    objParametri_server, objParametri_utenti
                )
            Else
                AssegnaVisibilitaNulla(enu.Current, objParametri_server, objParametri_utenti)
            End If

        End While

    End Sub


#End Region

#Region "Lettura"
    ''' <param name="listaUtenti">Utenti di cui si vuole controllare la visibilità</param>
    ''' <returns>Ritorna true se gli utenti specificati hanno tutti stessa visibilità, false in caso contrario.</returns>
    Public Function ControllaStessaVisibilita(ByVal listaUtenti As List(Of String),
                                             ByVal objParametriServer As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                             ByVal objParametriUtenti As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean
        If (listaUtenti.Count > 1) Then
            Dim impreseVisibiliConfronto = LeggiVisibilitaUtente(listaUtenti(0), New List(Of String), objParametriUtenti, objParametriServer)

            listaUtenti.Remove(listaUtenti(0))

            For Each utente In listaUtenti
                Dim impreseVisibili = LeggiVisibilitaUtente(utente, New List(Of String), objParametriUtenti, objParametriServer)
                Dim common = impreseVisibili.Intersect(impreseVisibiliConfronto)
                If common.Count <> impreseVisibili.Count OrElse common.Count <> impreseVisibiliConfronto.Count Then
                    Return False
                End If
            Next
        End If

        Return True

    End Function


    Public Function ControllaStessaVisibilitaLite(ByVal listaUtenti As List(Of String), params As ObjParams) As Boolean
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable = objProfilo.GetDistinctVisibility(listaUtenti, params.ObjParametri_Utenti)
        If DTProfilo Is Nothing OrElse DTProfilo.Rows.Count = 0 Then
            Return False
        ElseIf DTProfilo.Rows.Count = 1 Then
            Return True
        Else
            Dim vis = DTProfilo.Select.Select(Function(dr) If(IsDBNull(dr("Descrizione_1")), String.Empty, CStr(dr("Descrizione_1")))).
                DefaultIfEmpty(String.Empty)
            Dim ref = GetPiveHierarchyFromXML(vis.FirstOrDefault, params)

            For Each xmlstr In vis
                Dim pive = GetPiveHierarchyFromXML(xmlstr, params)
                If Not ref.SetEquals(pive) Then
                    Return False
                End If
            Next

        End If
        Return True
    End Function

    Private Function GetPiveHierarchyFromXML(xmlFilter As String, params As ObjParams) As HashSet(Of String)
        If String.IsNullOrEmpty(xmlFilter) Then
            Return New HashSet(Of String)
        End If

        Dim filter = String.Empty
        Dim objGerarchia As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        Dim ancestors = RegularExpressions.Regex.Matches(xmlFilter, "\""(([a-z0-9A-Z#]){11,})\""+", RegexOptions.None, TimeSpan.FromSeconds(3)).
                    Cast(Of RegularExpressions.Match)().
                    Select(Function(m) m.Value.Replace("""", "")).ToList()
        If ancestors.Any() Then
            filter = ancestors.Select(Function(p) "'" & p & "'").
                                Aggregate(Function(p1, p2) p1 & ", " & p2)
        End If
        Return objGerarchia.LeggixGerarchiaAlberoImprese(filter, "", "", params.ObjParametri_Server).
            Select.AsParallel.Select(Function(i) CStr(i.Item("PIVA"))).
            ToHashSet()
    End Function

    ''' <param name="utente">L'utente selezionato.</param>
    ''' <param name="utenteConfronto">L'utente la quale visibilità si vuole confrontare con l'utente selezionato.</param>
    ''' <returns>Ritorna -1 se l'utente selezionato vede meno imprese dell'utente di confronto, 0 se entrambi hanno uguale visibilità, 1 se l'utente selezionato vede più imprese dell'utente di confronto.</returns>
    Public Function ConfrontaVisibilitaUtenti(ByVal utente As String,
                                              ByVal utenteConfronto As String,
                                             ByVal objParametriServer As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                             ByVal objParametriUtenti As AgronicaCoreDataProvider.AgronicaCoreParametri) As Integer

        Dim impreseVisibili = LeggiVisibilitaUtente(utente, New List(Of String), objParametriUtenti, objParametriServer)
        Dim impreseVisibiliConfronto = LeggiVisibilitaUtente(utenteConfronto, New List(Of String), objParametriUtenti, objParametriServer)

        Dim common = impreseVisibili.Intersect(impreseVisibiliConfronto)
        Dim diff = impreseVisibili.Except(impreseVisibiliConfronto)

        If common.Count = impreseVisibili.Count AndAlso common.Count = impreseVisibiliConfronto.Count Then
            Return 0
        End If

        'Return IIf(impreseVisibili.Count < impreseVisibiliConfronto.Count, -1, 1)
        Return IIf(diff.Any(), 1, -1)

    End Function

    Public Function LeggiPiveCapostipiti(username As String, objUtenti As AgronicaCoreParametri) As List(Of String)
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable = objProfilo.Leggi(
            username, CInt(enum_Id_Servizio.GiasOnline),
            enumSelezioneVariabile.Selezione_TabellaCompleta,
            "", "",
            objUtenti
        )
        If DTProfilo.Rows.Count = 1 AndAlso DTProfilo.Rows(0).Item("Descrizione_1") <> "" Then
            Dim descr1 = CType(DTProfilo.Rows(0).Item("Descrizione_1"), String)
            Return LeggiPiveCapostipiti(descr1)
        Else
            Return New List(Of String)
        End If
    End Function

    Public Function LeggiPiveCapostipiti(stringaXmlPermessi As String) As List(Of String)
        If Not String.IsNullOrEmpty(stringaXmlPermessi) Then
            Return RegularExpressions.Regex.Matches(stringaXmlPermessi, "\""(([a-z0-9A-Z#]){11,})\""+", RegexOptions.None, TimeSpan.FromSeconds(3)).
                    Cast(Of RegularExpressions.Match)().
                    Select(Function(m) m.Value.Replace("""", "")).ToList()
        Else
            Return New List(Of String)
        End If
    End Function

    'Private Function LeggiPiveVisibiliDaCapostipiti(username As String, objParametri_Utenti As AgronicaCoreParametri, objParametri_Server As AgronicaCoreParametri) As HashSet(Of String)
    '    Dim objGerarchia As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
    '    Dim listaPive As List(Of String) = LeggiPiveCapostipiti(username, objParametri_Utenti)
    '    Return objGerarchia.LeggiGerarchiaDaCapostipiti(listaPive, objParametri_Server).
    '        Select.AsParallel.
    '        Select(Function(i) CStr(i.Item("PIVA"))).ToHashSet
    'End Function
#End Region

#Region "Funzioni Private"

    Private Function creaUtenteBase(utente As UtenteDTO,
                                    objParametri_Server As AgronicaCoreParametri,
                                    objParametri_Utenti As AgronicaCoreParametri) As String

        Dim handleConfigSiti As New Configurazione_Siti_R
        Dim objUtente As New AgronicaCoreUtentiDAL.Utenti_Write
        Dim objDettagli As New AgronicaCoreUtentiDAL.Utenti_Dettagli_W
        Dim objTipologiexPermessi_W As New AgronicaCoreUtentiDAL.Utenti_TipologiexPermessi_W
        Dim objImpostazioniFiltroMono_W As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_FiltroMono_W
        Dim esitoOperazioneOK As Boolean
        Dim tipologiaCod As Integer = 0
        If utente.Tipologia IsNot Nothing AndAlso utente.Tipologia.codice Then
            tipologiaCod = utente.Tipologia.codice
        End If

        Dim hashPasswordAbilitato As Boolean = False
        Dim dtConfigSiti As DataTable = handleConfigSiti.Leggi(0, "AbilitaHashPassword", "", "", objParametri_Server)
        If dtConfigSiti.Rows.Count > 0 Then
            hashPasswordAbilitato = dtConfigSiti.Rows(0)("Valore")
        End If

        'Scrivo record utente
        Dim username = Pulisci_Username(utente.UserName)
        Dim password = Pulisci_Username(utente.Password)

        'se la password è blank viene generata una random
        If password = "" Then
            password = AgronicaCoreUtentiBIZ.Utenti.GeneraPasswordRequisiti()
        End If

        If Not Utenti.ValidaComplessitaPassword(password, gestioneHashAbilitata:=hashPasswordAbilitato) Then
            Dim strErr = Utenti.MessaggioRequisitiPassword(gestioneHashAbilitata:=hashPasswordAbilitato)
            Throw New Exception(strErr)
        End If

        esitoOperazioneOK = objUtente.Scrivi(username, password,
                         enum_AgroLingue.Italiano_it,
                         hashPasswordAbilitato, False,
                         objParametri_Utenti, tipologiaCod)
        If Not esitoOperazioneOK Then
            Throw New Exception("Error occurred in user creation.")
        End If

        'Scrivo record dettagli utente
        'TODO: imposta check tipo utente
        If utente.flag_azienda_persona = 2 Then
            esitoOperazioneOK = objDettagli.Scrivi(username, utente.Cognome, utente.Nome,
                           Tel:="", utente.Email, utente.piva, utente.codice_fiscale,
                           utente.Rag_Soc, utente.flag_azienda_persona,
                           utente.username_commerciale, objParametri_Utenti)
        Else
            'TODO: fix valorizzazione dati
            esitoOperazioneOK = objDettagli.Scrivi(username, Cognome:="", Nome:="",
                           Tel:="", utente.Email, utente.piva, utente.codice_fiscale,
                           utente.Rag_Soc, utente.flag_azienda_persona,
                           utente.username_commerciale, objParametri_Utenti)
        End If
        If Not esitoOperazioneOK Then
            Throw New Exception("Error occurred in user creation.")
        End If

        If tipologiaCod <> 0 Then
            'Assegno permessi da tipologia utente
            objTipologiexPermessi_W.GeneraPermessiUtenteDaTipologia(
                utente.Tipologia.codice, username,
                AGRODATAINIZIO, AGRODATAFINE,
                objParametri_Utenti
            )
            'Assegno impostazioni da tipologia utente
            objTipologiexPermessi_W.GeneraImpostazioniUtenteDaTipologia(
                utente.Tipologia.codice, username,
                AGRODATAINIZIO, AGRODATAFINE,
                xFiltroAggiuntivo:="", objParametri_Utenti
            )
            'Impostazioni filtromono da tipologia utente
            objImpostazioniFiltroMono_W.ApplicaProfilo(
                utente.Tipologia.codice,
                username,
                objParametri_Utenti
            )
        End If

        Return username

    End Function

    Private Function Pulisci_Username(ByVal Username As String) As String

        Username = Username.Replace("'", "")
        Username = Username.Replace(" ", "")
        Username = Username.Replace("à", "a")
        Username = Username.Replace("è", "e")
        Username = Username.Replace("é", "e")
        Username = Username.Replace("ù", "u")
        Username = Username.Replace("ò", "o")
        Username = Username.Replace("ì", "i")

        Return Username

    End Function

    Private Sub AssegnaVisibilita(
        username As String, imprese As IEnumerable(Of IImpresaDto),
        objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri
    )

        Dim xVisibAppoggio As New Utenti
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Write
        Dim listaPive = CaricaGerarchia(imprese, objParametri_Server)

        objProfilo.Cancella(username, CInt(enum_Id_Servizio.GiasOnline), "", objParametri_Utenti)

        objProfilo.Scrivi(username, CInt(enum_Id_Servizio.GiasOnline),
                          CreaFiltroXMLPermessi(listaPive),
                          CreaFiltroSQLPermessi(listaPive),
                          Codice_1:=0, Codice_2:=0,
                          CostantiPersonalizzate.AGRODATAINIZIO,
                          CostantiPersonalizzate.AGRODATAFINE,
                          objParametri_Utenti)

        xVisibAppoggio.InizializzaTabellaUtentiVisibilitaAppoggio(
            username, CInt(enum_Id_Servizio.GiasOnline),
            objParametri_Server, objParametri_Utenti
        )

    End Sub

    ''' <summary>
    ''' Carica la gerarchia di imprese visibili a partire da quelle specificate.
    ''' </summary>
    ''' <param name="imprese">Imprese da cui leggere la gerarchia</param>
    ''' <param name="objParametri_Server"></param>
    ''' <returns>Lista di stringhe su cui creare i filtri visibilità</returns>
    Private Function CaricaGerarchia(imprese As IEnumerable(Of IImpresaDto), objParametri_Server As AgronicaCoreParametri) As List(Of String)
        Dim gerarchiaImprese As New GerarchiaImprese_R
        Dim initialPive = imprese.AsParallel.Select(Function(i) i.piva).Distinct.ToList
        Dim hierarchy = gerarchiaImprese.LeggiGerarchiaDaCapostipiti(initialPive, objParametri_Server, True).Select.AsParallel
        Dim sons = hierarchy.
            Where(Function(row) CInt(row("TipoImpresaGerarchia")) > enum_TipoImpresaGerarchia.Impresa).
            Select(Function(row) CStr(row("figlio"))).ToList
        Dim includedInHierarchy = hierarchy.Where(Function(row) initialPive.Contains(CStr(row("figlio"))) AndAlso initialPive.Contains(CStr(row("padre")))).
            Select(Function(row) CStr(row("figlio"))).ToList
        initialPive = initialPive.Except(includedInHierarchy).ToList
        Return initialPive.Union(sons).ToList
    End Function

    Private Function CreaFiltroXMLPermessi(piveImprese As List(Of String)) As String

        Dim xmlPermessi As New StringBuilder
        xmlPermessi.Append("<DatiFiltri><Filtro><DatiGerarchiaImprese>")
        For Each piva In piveImprese
            If piva <> "" Then
                xmlPermessi.Append("<GerarchiaImprese padre=""" & piva & """/>")
            End If
        Next
        xmlPermessi.Append("</DatiGerarchiaImprese><DatiPive/><Impresa><Struttura><Appezzamento><Impianto><Agenda><Contatto/></Agenda></Impianto></Appezzamento></Struttura></Impresa></Filtro></DatiFiltri>")

        If piveImprese.Count = 0 Then
            xmlPermessi.Clear()
        End If

        Return xmlPermessi.ToString()

    End Function

    Private Function CreaFiltroSQLPermessi(piveImprese As List(Of String)) As String

        Dim sqlPermessi As New StringBuilder
        For Each piva In piveImprese
            If piva <> "" Then
                sqlPermessi.Append(" ((GerarchiaImprese.Padre = '" & piva & "' and GerarchiaImprese.Foglia=1 ) OR Imprese.Piva = '" & piva & "') OR")
            End If
        Next
        If sqlPermessi.ToString() <> "" Then
            Return "AND (" & Left(sqlPermessi.ToString, sqlPermessi.Length - 2) & ")"
        End If
        Return sqlPermessi.ToString()

    End Function

    Private Function VerificaEsistenzaUtente(username As String, objParametri_Utenti As AgronicaCoreParametri) As Boolean
        Dim objUtentiDettagliDAL As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Dim DtDettagliUtenti As DataTable
        DtDettagliUtenti = objUtentiDettagliDAL.Leggi(username, CInt(enum_Id_Servizio.Nessuno),
                                                      enumSelezioneVariabile.Selezione_TabellaCompleta,
                                                      "", "",
                                                      objParametri_Utenti)

        Return (DtDettagliUtenti IsNot Nothing AndAlso DtDettagliUtenti.Rows.Count > 0)
    End Function

    ''' <returns>True if all the users exist, False otherwise.</returns>
    Private Function VerificaEsistenzaUtenti(username As IEnumerable(Of String), objParametri_Utenti As AgronicaCoreParametri) As Boolean
        If username Is Nothing OrElse username.Count = 0 Then
            Return True
        End If
        Dim objUtentiDettagliDAL As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Dim xFiltro = username.AsParallel.Select(Function(name) "'" & name & "'").
            Aggregate(Function(acc, str) acc & "," & str)
        xFiltro = " Utenti_Dettagli.UserName IN ( " & xFiltro & " ) "
        Dim DtDettagliUtenti = objUtentiDettagliDAL.Leggi(
            String.Empty, CInt(enum_Id_Servizio.Nessuno),
            enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
            xFiltro, String.Empty, objParametri_Utenti
        )
        Return (DtDettagliUtenti IsNot Nothing AndAlso DtDettagliUtenti.Rows.Count = username.Count)
    End Function

    Private Function VerificaVisibilitaUtente(username As String, objParametri_Utenti As AgronicaCoreParametri) As Boolean
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable
        DTProfilo = objProfilo.Leggi(username, CInt(enum_Id_Servizio.GiasOnline),
                                     enumSelezioneVariabile.Selezione_TabellaCompleta,
                                     "", "",
                                     objParametri_Utenti)

        Return (DTProfilo IsNot Nothing AndAlso DTProfilo.Rows.Count > 0)
    End Function

    ''' <returns>1 if all users have visibility, -1 if none of the users have visibility, 0 otherwise.</returns>
    Private Function CheckUsersVisibility(username As IEnumerable(Of String), objParametri_Utenti As AgronicaCoreParametri) As Integer
        If username Is Nothing OrElse username.Count = 0 Then
            Return True
        End If
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim xFiltro = username.AsParallel.Select(Function(name) "'" & name & "'").
            Aggregate(Function(acc, str) acc & "," & str)
        xFiltro = " Utente IN ( " & xFiltro & " ) "
        Dim DTProfilo = objProfilo.Leggi(
            String.Empty, CInt(enum_Id_Servizio.GiasOnline),
            enumSelezioneVariabile.Selezione_TabellaCompleta,
            xFiltro, String.Empty, objParametri_Utenti
        )
        Dim noVis = DTProfilo.Select(" Descrizione_1 like '%########%' ").Count

        If noVis = DTProfilo.Rows.Count Then
            Return -1
        ElseIf noVis = 0 AndAlso DTProfilo.Rows.Count = username.Count Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Function LeggiAppoggioXImprese(utenti As List(Of UtenteDTO),
                                           imprese As List(Of ImpresaDto),
                                           objParametri_Server As AgronicaCoreParametri) As DataTable

        Dim objUtentiAppoggio As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
        Dim xFiltroAggiuntivo As New StringBuilder With {.Length = 0}

        If imprese.Any Then
            xFiltroAggiuntivo.Append(" i.PIVA in ( ")
            xFiltroAggiuntivo.Append(
                imprese.Select(Function(i) "'" & i.piva & "'").
                    Aggregate(Function(p1, p2) p1 & ", " & p2)
            )
            xFiltroAggiuntivo.Append(" ) ")
        End If
        If xFiltroAggiuntivo.Length > 0 AndAlso utenti.Any() Then
            xFiltroAggiuntivo.Append(" AND ")
        End If
        If utenti.Any Then
            xFiltroAggiuntivo.Append(" va.Username in ( ")
            xFiltroAggiuntivo.Append(
                utenti.Select(Function(u) u.UserName).
                    Aggregate(Function(u1, u2) u1 & ", " & u2)
            )
            xFiltroAggiuntivo.Append(" ) ")
        End If

        Return objUtentiAppoggio.LeggiJoinImprese(xFiltroAggiuntivo.ToString(), "", objParametri_Server)
    End Function

    ''' <see cref="LeggiVisibilitaUtenti" />
    ''' <param name="utenteUsername">Utenti di cui si vuole conoscere la visibilità</param>
    ''' <param name="restringiVisibilita">Pive delle imprese da considerare nella lettura della visibilità.
    ''' Funge da filtro sulle aziende che si andranno a leggere, col risultato di mostrare la
    ''' visibilità dell'utente solo sul sottoinsieme di imprese indicato. Passare una lista
    ''' vuota per evitare di filtrare le aziende.</param>
    Private Function LeggiVisibilitaUtente(
        utenteUsername As String, restringiVisibilita As List(Of String),
        objParametri_Utenti As AgronicaCoreParametri, objParametri_Server As AgronicaCoreParametri
    ) As List(Of ImpresaGerarchiaBaseDto)
        Dim objGerarchia As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        Dim listaPive As List(Of String) = LeggiPiveCapostipiti(utenteUsername, objParametri_Utenti)

        If restringiVisibilita.Any() AndAlso listaPive.Any() Then
            listaPive = listaPive.Where(Function(i) restringiVisibilita.Contains(i))
        ElseIf restringiVisibilita.Any() AndAlso Not listaPive.Any() Then
            listaPive = restringiVisibilita
        End If

        Return objGerarchia.LeggiGerarchiaDaCapostipiti(
                listaPive, objParametri_Server,
                enumSelezioneVariabile.Selezione_JoinDescrizioni
            ).Select.AsParallel.
            Select(Function(i) New ImpresaGerarchiaBaseDto With {
                .piva = If(IsDBNull(i.Item("figlio")), String.Empty, i.Item("figlio")),
                .Padre = If(IsDBNull(i.Item("Padre")), String.Empty, i.Item("Padre")),
                .rag_soc = If(IsDBNull(i.Item("rag_soc")), String.Empty, i.Item("rag_soc")),
                .IsFoglia = i.Item("Foglia"),
                .Sa_Nome = ""
            }).Distinct.ToList()
    End Function

#End Region

End Class
