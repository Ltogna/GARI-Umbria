Imports System.Text
Imports AgronicaCoreVarieDAL

Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreUtility

Public Class AgronicaBase
    Inherits AgroControlliCommons

    Dim _LinkHomePageGlobale As String
    Public ReadOnly Property LinkHomePageGlobale As String
        Get
            Return _LinkHomePageGlobale
        End Get
    End Property

#Region "Methods & Event Handlers"



    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim ObjParametriServer As AgronicaCoreDataProvider.AgronicaCoreParametri =
            HttpContext.Current.Session("ASG_objParametri_Server")



        Dim PaginaRedirect As String = Http.CookieLeggi("LinkHomePageGlobale")
        Dim PaginaRedirectQueryStringSessione As String = ""
        Dim PaginaRedirectQueryStringSuperUser As String = ""

        If Not ObjParametriServer Is Nothing Then

            If String.IsNullOrWhiteSpace(PaginaRedirect) Then
                PaginaRedirect = LeggiDaSessioneOppureDaConfigSiti("LinkHomePageGlobale", "",
                       agronicacoreparametri_tipoDB.SuperServer)
            End If


            _LinkHomePageGlobale = PaginaRedirect

            PaginaRedirectQueryStringSessione = "Session=Expire"
            PaginaRedirectQueryStringSuperUser = "pivasuperuser=" & ObjParametriServer.PivaSuperUser

        End If

        Dim stb As New StringBuilder
        stb.AppendLine("<script type='text/javascript' >")
        stb.AppendLine(" var AgronicaBasePaginaLoginRedirectDaSessione = '" & PaginaRedirect & "';")
        stb.AppendLine(" var AgronicaBasePaginaLoginRedirectDaSessioneQueryStringSessione = '" & PaginaRedirectQueryStringSessione & "';")
        stb.AppendLine(" var AgronicaBasePaginaLoginRedirectDaSessioneQueryStringPivaSuperUser = '" & PaginaRedirectQueryStringSuperUser & "';")

        stb.AppendLine("</script>")


        Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "AgronicaBasePaginaLoginRedirectDaSessione", stb.ToString)
        Page.ClientScript.RegisterClientScriptInclude("AgronicaBase", Page.ClientScript.GetWebResourceUrl(Me.[GetType](), "AgronicaControlli_2010.AgronicaBase.js"))


    End Sub

    Private Sub AgronicaBase_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim ObjParametriServer As AgronicaCoreDataProvider.AgronicaCoreParametri =
            HttpContext.Current.Session("ASG_objParametri_Super_Server")

        If Not ObjParametriServer Is Nothing Then


            _LinkHomePageGlobale = LeggiDaSessioneOppureDaConfigSiti("LinkHomePageGlobale", "",
                       agronicacoreparametri_tipoDB.SuperServer)
        End If

    End Sub

#End Region

End Class
