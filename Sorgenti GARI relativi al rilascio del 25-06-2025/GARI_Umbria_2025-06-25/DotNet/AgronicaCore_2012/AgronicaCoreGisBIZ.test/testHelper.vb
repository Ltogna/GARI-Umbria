Imports AgronicaCoreDataProvider
Imports System.Configuration.ConfigurationManager


Public Class testHelper

    Public Shared Function GetObjParametri() As AgronicaCoreParametri

        Dim objParametri As AgronicaCoreParametri
        Const AGRODATAINIZIO As Date = #1/1/1900#
        Const AGRODATAFINE As Date = #12/31/2100#

        Dim SuperUser_CodFiscale_ORIGINE As String = AppSettings("PivaSuperUser_ORIGINE")
        Dim SuperUser_Username_ORIGINE As String = AppSettings("UsernameSuperUser_Origine")

        Dim Import_CodFiscale_ORIGINE As String = AppSettings("Import_CodFiscale_ORIGINE")
        Dim Import_Username_ORIGINE As String = AppSettings("Import_Username_ORIGINE")


        Dim Stringa_Connessione_Server_GIAS_Origine As String = ConnectionStrings("cnGias").ConnectionString

        objParametri = New AgronicaCoreDataProvider.AgronicaCoreParametri( _
                        AGRODATAINIZIO, _
                        AGRODATAFINE, _
                        AgronicaCoreDataProvider.AgronicaCoreParametri.enumCancellazioneLogica.CancellazioneFisica, _
                        AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati, _
                        ".\", _
                        "Transfert_GiacenzeDDT_daGias_aGias.txt", _
                        SuperUser_Username_ORIGINE, _
                        SuperUser_CodFiscale_ORIGINE, _
                        Import_Username_ORIGINE, _
                        Import_CodFiscale_ORIGINE, _
                        Stringa_Connessione_Server_GIAS_Origine _
                    )
        Return objParametri
    End Function

End Class
