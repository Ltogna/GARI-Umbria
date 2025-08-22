Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.AgronicaCoreParametri


Imports AgronicaCoreDataProvider
Imports System.Configuration.ConfigurationManager

Public Class testHelper

    Public Shared Function GetObjParametri(ByVal PivaSuperUser As String) As AgronicaCoreParametri

        Dim objParametriHLP As New AgronicaCoreParametri_Helper
        Dim objAgronicaCore As New DataProvider

        Dim StringaConnessione_Server As String = objAgronicaCore.FindIniConnessioni(System.Configuration.ConfigurationManager.AppSettings("StarGate_PathFileINI"), _
                                                                                     System.Configuration.ConfigurationManager.AppSettings("Connessione_ONLINE_Server"))


        Dim username As String = ""
        Dim objParametri_Server As AgronicaCoreParametri = objParametriHLP.Crea_ObjParametri(AGRODATAINIZIO, AGRODATAFINE, enumCancellazioneLogica.CancellazioneFisica, enumVisibilita.Visibilita_Tutti, "", "", username, "", username, "", StringaConnessione_Server)
        objParametri_Server.PivaSuperUser = PivaSuperUser



        
        Return objParametri_Server
    End Function


End Class
