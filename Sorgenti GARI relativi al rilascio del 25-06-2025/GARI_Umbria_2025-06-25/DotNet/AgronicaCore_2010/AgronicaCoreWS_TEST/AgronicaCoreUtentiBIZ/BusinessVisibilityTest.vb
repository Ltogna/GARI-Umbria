Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AgronicaCoreDataProvider
Imports AgronicaCoreModelsSTD

<TestClass()> Public Class BusinessVisibilityTest

    'Time limit for http calls on GIAS is 15 minutes.
    Private database = "AGRONICA_NAZIONALE_SERVER_2019"
    Private testUser = "1000176"

    Private objParams As ObjParams = ObjParamsSecret.GetObjParams(database)

    Private Function XmlContains(xml As String, pive As IEnumerable(Of String)) As Boolean
        Dim found = RegularExpressions.Regex.Matches(xml, "\""(([a-z0-9A-Z#]){11,})\""+").
                    Cast(Of RegularExpressions.Match)().
                    Select(Function(m) m.Value.Replace("""", "")).
                    OrderBy(Function(str) str)
        Return found.SequenceEqual(pive.OrderBy(Function(str) str).ToArray)
    End Function

    Private Function SqlContains(sql As String, pive As IEnumerable(Of String)) As Boolean
        Dim found = RegularExpressions.Regex.Matches(sql, "'(([a-z0-9A-Z#]){11,})'+").
                    Cast(Of RegularExpressions.Match)().
                    Select(Function(m) m.Value.Replace("'", "")).
                    ToHashSet.OrderBy(Function(str) str)
        Return found.SequenceEqual(pive.OrderBy(Function(str) str))
    End Function

    Private Function GetUserFilters(username As String) As Tuple(Of String, String)
        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim userVisProfile As DataRow = objProfilo.Leggi(
            username,
            TipiEnumerativi.enum_Id_Servizio.GiasOnline,
            AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
            String.Empty, String.Empty,
            objParams.ObjParametri_Utenti
        ).Select.First
        Dim xmlFilter = CStr(userVisProfile("Descrizione_1"))
        Dim sqlFilter = CStr(userVisProfile("Descrizione_2"))
        Return New Tuple(Of String, String)(xmlFilter, sqlFilter)
    End Function

    Private Sub AssertCorrectVisibilityFilters(username As String, expectedVis As IEnumerable(Of String))
        Dim userVis = GetUserFilters(username)
        Assert.IsTrue(XmlContains(userVis.Item1, expectedVis))
        Assert.IsTrue(SqlContains(userVis.Item2, expectedVis))
    End Sub

    <TestMethod()> Public Sub overwriteWithSingleBuisiness()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            {New anagrafiche.ImpresaDto("00121340335")},
            objParams
        )
        AssertCorrectVisibilityFilters(testUser, {"00121340335"})
    End Sub

    <TestMethod()> Public Sub overwriteWithMultipleBuisiness()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("00121340335"),
            New anagrafiche.ImpresaDto("00329360333"),
            New anagrafiche.ImpresaDto("00156490336")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {
            "00121340335", "00329360333", "00156490336"
        })
    End Sub

    <TestMethod()> Public Sub overwriteWithSingleFather()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            {New anagrafiche.ImpresaDto("UZ020033106")},
            objParams
        )
        AssertCorrectVisibilityFilters(testUser, {"UZ020033106"})
    End Sub

    <TestMethod()> Public Sub overwriteWithMultipleFathers()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("UZ020033106"),
            New anagrafiche.ImpresaDto("UZ020033107"),
            New anagrafiche.ImpresaDto("UZ020033108")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {
            "UZ020033106", "UZ020033107", "UZ020033108"
        })
    End Sub

    <TestMethod()> Public Sub overwriteWithFatherAndSon()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("UZ020033106"),
            New anagrafiche.ImpresaDto("00114060338")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {"UZ020033106"})
    End Sub

    <TestMethod()> Public Sub overwriteWithFatherAndSon2()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("01283730339"),
            New anagrafiche.ImpresaDto("UZ020033106"),
            New anagrafiche.ImpresaDto("00114060338")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {
            "01283730339", "UZ020033106", "UZ020033107", "UZ020033108", "UZ020033109", "UZ020033110", "UZ020033111", "UZ020033112", "UZ020033113"
        })
    End Sub

    <TestMethod()> Public Sub overwriteWithFatherAndOrphan()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("UZ020033106"),
            New anagrafiche.ImpresaDto("00121130330")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {"UZ020033106", "00121130330"})
    End Sub

    <TestMethod()> Public Sub overwriteWithDuplicated()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("UZ103005001"),
            New anagrafiche.ImpresaDto("UZ103005002"),
            New anagrafiche.ImpresaDto("UZ103005003"),
            New anagrafiche.ImpresaDto("UZ103005004"),
            New anagrafiche.ImpresaDto("UZ103005005"),
            New anagrafiche.ImpresaDto("UZ103005006"),
            New anagrafiche.ImpresaDto("UZ103005007"),
            New anagrafiche.ImpresaDto("UZ103005008"),
            New anagrafiche.ImpresaDto("UZ136005001"),
            New anagrafiche.ImpresaDto("UZ136005002"),
            New anagrafiche.ImpresaDto("UZ136005003"),
            New anagrafiche.ImpresaDto("UZ136005004"),
            New anagrafiche.ImpresaDto("UZ136005005"),
            New anagrafiche.ImpresaDto("UZ136005006"),
            New anagrafiche.ImpresaDto("UZ136005007"),
            New anagrafiche.ImpresaDto("UZ136005008")
        }
        visManager.OverwriteVisibility(
            {New profilazione.BaseUtente(testUser)},
            business, objParams
        )
        AssertCorrectVisibilityFilters(testUser, {
            "UZ103005001", "UZ103005002", "UZ103005003", "UZ103005004", "UZ103005005", "UZ103005006", "UZ103005007", "UZ103005008",
            "UZ136005001", "UZ136005002", "UZ136005003", "UZ136005004", "UZ136005005", "UZ136005006", "UZ136005007", "UZ136005008"
        })
    End Sub

    <TestMethod()> Public Sub EditVisibilityEntryPoint()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        Dim business = {
            New anagrafiche.ImpresaDto("UZ103005001"),
            New anagrafiche.ImpresaDto("UZ103005002"),
            New anagrafiche.ImpresaDto("UZ103005003"),
            New anagrafiche.ImpresaDto("UZ103005004"),
            New anagrafiche.ImpresaDto("UZ103005005"),
            New anagrafiche.ImpresaDto("UZ103005006"),
            New anagrafiche.ImpresaDto("UZ103005007"),
            New anagrafiche.ImpresaDto("UZ103005008"),
            New anagrafiche.ImpresaDto("UZ136005001"),
            New anagrafiche.ImpresaDto("UZ136005002"),
            New anagrafiche.ImpresaDto("UZ136005003"),
            New anagrafiche.ImpresaDto("UZ136005004"),
            New anagrafiche.ImpresaDto("UZ136005005"),
            New anagrafiche.ImpresaDto("UZ136005006"),
            New anagrafiche.ImpresaDto("UZ136005007"),
            New anagrafiche.ImpresaDto("UZ136005008")
        }
        visManager.ModificaVisibilitaAziendaUtenti(
            {New profilazione.BaseUtente(testUser)}.ToList,
            business.ToList,
            True, False,
            objParams.ObjParametri_Server, objParams.ObjParametri_Utenti
        )
        AssertCorrectVisibilityFilters(testUser, {
            "UZ103005001", "UZ103005002", "UZ103005003", "UZ103005004", "UZ103005005", "UZ103005006", "UZ103005007", "UZ103005008",
            "UZ136005001", "UZ136005002", "UZ136005003", "UZ136005004", "UZ136005005", "UZ136005006", "UZ136005007", "UZ136005008"
        })
    End Sub

    <TestMethod()> Public Sub RemoveVisibility()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        visManager.RimuoviImpreseDaVisibilitaUtenti(
            {New profilazione.BaseUtente(testUser)}.ToList, {},
            objParams.ObjParametri_Server, objParams.ObjParametri_Utenti
        )
        AssertCorrectVisibilityFilters(testUser, {"###########"})
    End Sub

    <TestMethod()> Public Sub AddVisibility()
        Dim visManager As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
        visManager.RimuoviImpreseDaVisibilitaUtenti(
            {New profilazione.BaseUtente(testUser)}.ToList, {},
            objParams.ObjParametri_Server, objParams.ObjParametri_Utenti
        )
        Dim business = {
            New anagrafiche.ImpresaDto("UZ103005001"),
            New anagrafiche.ImpresaDto("UZ103005002"),
            New anagrafiche.ImpresaDto("UZ103005003")
        }
        visManager.ModificaVisibilitaAziendaUtenti(
            {New profilazione.BaseUtente(testUser)}.ToList,
            business.ToList,
            False, False,
            objParams.ObjParametri_Server, objParams.ObjParametri_Utenti
        )
        business = {
            New anagrafiche.ImpresaDto("UZ136005006"),
            New anagrafiche.ImpresaDto("UZ136005007"),
            New anagrafiche.ImpresaDto("UZ136005008")
        }
        visManager.ModificaVisibilitaAziendaUtenti(
            {New profilazione.BaseUtente(testUser)}.ToList,
            business.ToList,
            False, False,
            objParams.ObjParametri_Server, objParams.ObjParametri_Utenti
        )
        AssertCorrectVisibilityFilters(testUser, {
            "UZ103005001", "UZ103005002", "UZ103005003", "UZ136005006", "UZ136005007", "UZ136005008"
        })
    End Sub

End Class