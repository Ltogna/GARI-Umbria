Imports System.Text

<TestClass()>
Public Class prova

    Private testContextInstance As TestContext

    Public objP As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Server;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"
    Private objPUtenti As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Utenti;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"




    <TestMethod()>
    Public Sub TestMethod1()

        Dim cryptServ As String = AgronicaCoreDataProvider.Sicurezza.Stringa_Codifica_LANCompatibile( _
                          objP, _
                         AgronicaCoreDataProvider.CostantiPersonalizzate.AgroKey_EncoderDecoder)

        Dim cryptUte As String = AgronicaCoreDataProvider.Sicurezza.Stringa_Codifica_LANCompatibile( _
                          objPUtenti, _
                         AgronicaCoreDataProvider.CostantiPersonalizzate.AgroKey_EncoderDecoder)

        Dim Address_Crypt As String
        ' TODO: aggiungere qui la logica del test
    End Sub

End Class
