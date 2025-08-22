Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AgronicaCoreModelsSTD
Imports AgronicaCoreDataProvider

<TestClass()> Public Class PermissionsAttributionTest

    'Time limit for http calls on GIAS is 15 minutes.
    Private database = "AGRONICA_NAZIONALE_SERVER_2019"
    Private testUser = "user.test00"
    Private testedProfile As profilazione.TipologiaUtente = New profilazione.TipologiaUtente(55, "Tester")
    Private originalProfile As profilazione.TipologiaUtente = New profilazione.TipologiaUtente(14, "piemonte")

    Private objParams As ObjParams = ObjParamsSecret.GetObjParams(database)

    Private Function ReadUsers(filter As String) As ParallelQuery(Of DataRow)
        Dim ObjUtentiDettagli As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Return ObjUtentiDettagli.Leggi_anchePermessi(
                TipiEnumerativi.enum_Id_Servizio.GiasOnline, Id_Attivita:=-1,
                AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni,
                filter, "", objParams.ObjParametri_Utenti
            ).Select.AsParallel
    End Function

    <TestMethod()> <Priority(1)> Public Sub AssignProfile_Massive()
        Dim userManager As New AgronicaCoreUtentiBIZ.Utenti
        Dim filterOutSuperUser = " Utenti.UserName != '" & objParams.ObjParametri_Server.SuperUserUsername & "' "

        Dim allUsers = ReadUsers(filterOutSuperUser).
            Select(Function(row) New profilazione.BaseUtente With {
                .UserName = CStr(row("UserName"))
            }).ToList
        userManager.AssociaProfilo(allUsers, testedProfile, False, objParams, False)
        Dim isAllTestedProfile = ReadUsers(filterOutSuperUser).
            All(Function(row) CInt(row("Tipologia_Cod")) = testedProfile.codice)

        Assert.IsTrue(isAllTestedProfile)
    End Sub

    <TestMethod()> <Priority(1)> Public Sub AssignProfile_Multiple()
        Dim userManager As New AgronicaCoreUtentiBIZ.Utenti
        Dim ObjUtentiDettagli As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Dim reduceUsersCount = " Utenti.UserName like '1000%' "

        Dim allUsers = ReadUsers(reduceUsersCount).
            Take(10).
            Select(Function(row) New profilazione.BaseUtente With {
                .UserName = CStr(row("UserName"))
            }).ToList
        userManager.AssociaProfilo(allUsers, testedProfile, False, objParams, False)
        Dim isAllTestedProfile = ReadUsers(reduceUsersCount).
            Where(Function(row) allUsers.Exists(Function(u) u.UserName = CStr(row("UserName")))).
            All(Function(row) CInt(row("Tipologia_Cod")) = testedProfile.codice)

        Assert.IsTrue(isAllTestedProfile)
    End Sub

    <TestMethod()> <Priority(1)> Public Sub AssignProfile_Single()
        Dim userManager As New AgronicaCoreUtentiBIZ.Utenti
        Dim ObjUtentiDettagli As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Dim selectUser = " Utenti.UserName = '" & testUser & "' "

        Dim allUsers = ReadUsers(selectUser).
            Select(Function(row) New profilazione.BaseUtente With {
                .UserName = CStr(row("UserName"))
            }).ToList
        userManager.AssociaProfilo(allUsers, testedProfile, False, objParams, False)
        Dim isAllTestedProfile = ReadUsers(selectUser).
            All(Function(row) CInt(row("Tipologia_Cod")) = testedProfile.codice)

        Assert.IsTrue(isAllTestedProfile)
    End Sub

    <TestMethod()> Public Sub AddRemovePermission_emptyList()
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, {},
            TipiEnumerativi.enum_TipoPermesso.DISABILITATO, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, {},
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, {},
            TipiEnumerativi.enum_TipoPermesso.LETTURA_SCRITTURA, objParams
        )
    End Sub

    <TestMethod()> Public Sub RemovePermission_notAssigned()
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        Dim permissions = {
            New baseClass.BaseCodeDescr(13, ""),
            New baseClass.BaseCodeDescr(514, ""),
            New baseClass.BaseCodeDescr(243, ""),
            New baseClass.BaseCodeDescr(247, ""),
            New baseClass.BaseCodeDescr(265, ""),
            New baseClass.BaseCodeDescr(562, "")
        }
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.DISABILITATO, objParams
        )
    End Sub

    <TestMethod()> Public Sub AddRemovePermission()
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        Dim permissions = {
            New baseClass.BaseCodeDescr(13, ""),
            New baseClass.BaseCodeDescr(514, ""),
            New baseClass.BaseCodeDescr(243, ""),
            New baseClass.BaseCodeDescr(247, ""),
            New baseClass.BaseCodeDescr(265, ""),
            New baseClass.BaseCodeDescr(562, "")
        }
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA_SCRITTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.DISABILITATO, objParams
        )
    End Sub

    <TestMethod()> Public Sub AddRemoveCartographyPermission()
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        Dim permissions = {
            New baseClass.BaseCodeDescr(12, "")
        }
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA_SCRITTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.DISABILITATO, objParams
        )
    End Sub

    <TestMethod()> Public Sub ReAddPermission()
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        Dim permissions = {
            New baseClass.BaseCodeDescr(13, ""),
            New baseClass.BaseCodeDescr(514, ""),
            New baseClass.BaseCodeDescr(243, ""),
            New baseClass.BaseCodeDescr(247, ""),
            New baseClass.BaseCodeDescr(265, ""),
            New baseClass.BaseCodeDescr(562, "")
        }
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
    End Sub

    <TestMethod()> Public Sub AddPermission_noConnected()
        Dim userManager As New AgronicaCoreUtentiBIZ.Utenti
        Dim tipologie As New AgronicaCoreUtentiBIZ.Tipologie
        Dim ObjUtentiDettagli As New AgronicaCoreUtentiDAL.Utenti_Dettagli_R
        Dim filterOutSuperUser = " Utenti.UserName != '" & objParams.ObjParametri_Server.SuperUserUsername & "' "
        Dim permissions = {
            New baseClass.BaseCodeDescr(13, ""),
            New baseClass.BaseCodeDescr(514, ""),
            New baseClass.BaseCodeDescr(243, ""),
            New baseClass.BaseCodeDescr(247, ""),
            New baseClass.BaseCodeDescr(265, ""),
            New baseClass.BaseCodeDescr(562, "")
        }

        Dim allUsers = ReadUsers(filterOutSuperUser).
            Select(Function(row) New profilazione.BaseUtente With {
                .UserName = CStr(row("UserName"))
            }).ToList
        userManager.AssociaProfilo(allUsers, originalProfile, False, objParams, False)

        tipologie.Aggiorna_Permessi_Tipologia(
            testedProfile.codice, permissions,
            TipiEnumerativi.enum_TipoPermesso.LETTURA, objParams
        )
    End Sub

End Class