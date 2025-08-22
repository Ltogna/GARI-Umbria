Imports System.Web
Imports System.Web.SessionState
Imports System.Diagnostics


Public Class Custom500
    Inherits System.Web.UI.Page


    Public objError
    Public messaggio As String
    Public stack As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not (TypeOf Context.Handler Is IRequiresSessionState OrElse _
            TypeOf Context.Handler Is IReadOnlySessionState)) OrElse _
            IsNothing(Session("ASG_objParametri_Utenti")) Then

            errore.Attributes.Add("onLoad", "setTimeout('goURL()', 5000);")
            errore.Text = "NESSUN PROBLEMA...<br><h3>E' scaduta la sessione, basta chiudere la pagina e rientrare</h3>"
            Exit Sub

        End If

        Dim ErrorDescription As String = Session("objTrace").GetBaseException().ToString
        objError = Session("objTrace")

        messaggio = objError.innerexception.message.ToString
        stack = objError.innerexception.stacktrace.ToString

        errore.Text = "Si è verificato un errore.<br><h3>Se il problema persiste contattare l'assistenza allegando il messaggio riportato sotto.<br><b>Ricordarsi di indicare i dati necessari per la rigenerazione dell'errore</b></h3>"
        pannelloerrore.Visible = True
        Session("objTrace") = Nothing


    End Sub

End Class