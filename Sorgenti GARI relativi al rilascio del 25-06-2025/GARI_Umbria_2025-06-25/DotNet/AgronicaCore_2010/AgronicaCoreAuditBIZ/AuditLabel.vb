Imports System.Text
Imports System.Web
Imports Newtonsoft.Json


' GESTIONE ETICHETTE AUDIT
Public Class AuditLabel

    Public Shared Function AuditEtichette() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"AggiornaDati", "Aggiorna Dati|Data Update|Mettre à jour les données"},
            {"AggiornamentoEffettuato", "Aggiornamento effettuato correttamente|Update successful"},
            {"AggiornaWorkflow", "Aggiorna workflow|Workflow update"},
            {"Annulla", "Annulla|Cancel|Annuler"},
            {"Azienda", "Azienda|Company Name|Producteur"},
            {"Azioni", "Azioni|Actions|Actes"},
            {"Cancella", "Cancella|Delete|Annuler"},
            {"Cerca", "Cerca|Search|Rechercher"},
            {"Chiudi", "Chiudi|Close|Fermer"},
            {"Codice", "Codice|Code|Code"},
            {"Conferma", "Conferma|Confirm|Confirmer"},
            {"ConfermaCreazione", "Vuoi crearne una?|Do you want to create one?|Confirmer la Création"},
            {"ConfermaAggiornaWorkflow", "Vuoi aggiornare lo stato del workflow?|Do you want to update the workflow status?"},
            {"ConfermaCancellazione", "Vuoi eliminare l'elemento selezionato?|Do you want to delete the selected item?|Vous souhaitez supprimer l'élément sélectionné?"},
            {"ConfermaCopiaProfilo", "Vuoi copiare il profilo selezionato?|Do you want to copy the selected profile?|Voulez-vous copier le profil sélectionné ?"},
            {"ConfermaCopiaChecklist", "Vuoi copiare la scheda selezionata?|Do you want to copy the selected checklist?|Voulez-vous copier l'onglet sélectionné ?"},
            {"Copia", "Copia|Copy|Copie"},
            {"DataCompilazione", "Data Compilazione|Date|Date de Compilation"},
            {"DataFine", "Data Fine|End Date|Date de Fin"},
            {"DataInizio", "Data Inizio|Start Date|Date de Début"},
            {"ErroreAggiornamentoWorkflow", "Aggiornamento workflow non consentito|Workflow update not allowed"},
            {"ErroreCancellazioneProfilo", "Cancellazione non riuscita, sono presenti registrazioni che fanno riferimento alla profilazione|Deletion failed, there are records that refer to profiling"},
            {"ErroreProfilazione", "Non è presente una profilazione azienda valida|There is no valid company profiling"},
            {"ErroreSalvataggioProfilo", "Salvataggio non riuscito, verificare che non sia già presente un profilazione nell'intervallo di date specificate|Save failed, check that there is not already a profiling in the specified date range"},
            {"ErroreSalvataggio", "Salvataggio non riuscito|Save failed"},
            {"InserireData", "Inserire la data compilazione|Insert date"},
            {"InserireDateValidita", "Inserire data inizio e fine validità|Insert start and end date"},
            {"Modifica", "Modifica|Edit|Modifier"},
            {"Modifica Checklist", "Modifica Checklist|Edit Checklist|Modifier Checklist"},
            {"NomeOP", "Nome OP|OP Name"},
            {"Note", "Note|Note"},
            {"NuovaProfilazione", "Nuova Profilazione|New Profiling|Nouveau Profilage"},
            {"InserisciChecklistMassiva", "Inserisci Checklist Massiva|Enter Massive Checklist"},
            {"NuovaScheda", "Nuova Scheda|New Checklist|Nouvel Checklist"},
            {"PartitaIVA", "Partita IVA|VAT"},
            {"PIVAOP", "P.IVA OP|OP VAT|ID Producteur"},
            {"Precisare", "Precisare|Specify|Préciser"},
            {"ProfilazioneAzienda", "Profilazione Azienda|Grower Profiling|Profilage d'Entreprise"},
            {"ProfilazioniAziende", "Profilazioni Aziende|Grower Profiling|Profilage d'Entreprise"},
            {"ProfilazioneCreata", "Profilazione creata correttamente|Profiling created successfully|Profilage créé avec succès"},
            {"ProfilazioneNonDisponibile", "Profilazione non disponibile per l'azienda|Profiling not available for the company|Profilage Non Disponible"},
            {"ProgettoImpianto", "Progetto Impianto|Budwood Project"},
            {"Salva", "Salva|Save|Sauvegarder"},
            {"SalvaEsci", "Salva ed Esci|Save and Exit|Sauvegarder et Quitter"},
            {"SalvaModifiche", "Salva le modifiche|Save Changes|Enregistrer les Modifications"},
            {"SalvataggioEffettuato", "Salvataggio effettuato correttamente|Successful saving|Enregistré avec succès"},
            {"SchedaRegistrazioni", "Scheda Registrazioni|Checklist|Checklist"},
            {"SchedeRegistrazioni", "Schede Registrazioni|Checklist|Checklist"},
            {"SelezionareAzienda", "Selezionare un'azienda|Select company"},
            {"SelezionareProgetto", "Selezionare il progetto impianto di riferimento|Select budwood project"},
            {"Stampa", "Stampa|Print|Imprimer"},
            {"Stato", "Stato|Status|état"},
            {"StatoNonValido", "Stato non valido|Invalid state"},
            {"StatoWorkflow", "Stato Workflow|Workflow Status"},
            {"Superficie", "Superficie [ha]|Area (hectares)"},
            {"Utente", "Utente|Auditor|Utilisateur"},
            {"Versione", "Versione|Version|Version"},
            {"WorkflowAggiornato", "Workflow aggiornato correttamente|Wokflow updated"}
        }
    End Function

    Public Shared Function LeggiEtichette(ByVal Audit_Tipo As Integer, ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Dictionary(Of String, String)

        Dim Etichette = AuditEtichette()

        Dim objAudit As New AgronicaCoreAuditDAL.Audit_R
        Dim dtLabel = objAudit.LeggiAuditLabel(Audit_Tipo, objParametri)

        If dtLabel IsNot Nothing AndAlso dtLabel.Rows.Count > 0 Then
            For Each row In dtLabel.Rows
                Etichette(row.Item("Label")) = row.Item("Testo")
            Next
        End If

        Return Etichette

    End Function

    Public Shared Function LeggiEtichetta(ByVal Label As String, Optional ByVal Testo As String = Nothing) As String
        Dim Lingua = HttpContext.Current.Session("Lingua_Audit")
        Dim Etichette = HttpContext.Current.Session("Etichette_Audit")
        If Etichette.ContainsKey(Label) Then
            Dim Labels = Etichette(Label).Split("|")
            Return If(Labels.Length > Lingua AndAlso Labels(Lingua) <> "", Labels(Lingua), Labels(0))
        Else
            Return If(Testo, Label)
        End If
    End Function

    Public Shared Function GestioneEtichette(ByVal Audit_Tipo As Integer, ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As String

        Dim Etichette_Audit As Dictionary(Of String, String) = HttpContext.Current.Session("Etichette_Audit")
        If IsNothing(Etichette_Audit) Then
            Etichette_Audit = LeggiEtichette(Audit_Tipo, objParametri)
            HttpContext.Current.Session("Etichette_Audit") = Etichette_Audit
        End If

        ' se tipo 14 forzo lingua inglese (default italiano)
        Dim Lingua_Audit As Integer = If(Audit_Tipo = 14, 1, 0)

        'Forzature
        Select Case Audit_Tipo
            Case 18
                Lingua_Audit = 2 'Francese
            Case Else
                '...
        End Select

        If Etichette_Audit.ContainsKey("Lingua_Audit") Then
            Lingua_Audit = CInt(Etichette_Audit("Lingua_Audit"))
            If Lingua_Audit = -1 Then
                Lingua_Audit = objParametri.Lingua_Cod - 1
            End If
        End If
        HttpContext.Current.Session("Lingua_Audit") = Lingua_Audit

        Dim Etichette = JsonConvert.SerializeObject(Etichette_Audit, Formatting.None)
        Dim Gestione_Etichette As New StringBuilder
        Gestione_Etichette.AppendLine("var etichette = " & Etichette & ";")
        Gestione_Etichette.AppendLine("function Label(chiave, testo) {")
        Gestione_Etichette.AppendLine("let lingua = " & Lingua_Audit & ";")
        Gestione_Etichette.AppendLine("if (chiave in etichette) {")
        Gestione_Etichette.AppendLine("  let labels = etichette[chiave].split('|');")
        Gestione_Etichette.AppendLine("  return labels.length > lingua && labels[lingua] != '' ? labels[lingua]: labels[0];")
        Gestione_Etichette.AppendLine("} else return testo == undefined ? chiave : testo; }")

        Return Gestione_Etichette.ToString

    End Function

End Class
