Imports System.Data.Entity.Spatial

Public Class ElementiGrafici
    Public Property PivaSuperUser As String
    Public Property ElementoGraficoCod As Integer
    Public Property ElementoGraficoDes As String
    Public Property EntitaCod As Integer
    Public Property Layer As Integer
    Public Property Cartography As String
    Public Property Flag_GPS As Integer

    Public Sub New()
        PivaSuperUser = ""
        ElementoGraficoCod = 0
        ElementoGraficoDes = ""
        EntitaCod = 0
        Layer = 0
        Cartography = 0
        Flag_GPS = 0
    End Sub
End Class
