Imports AgronicaCoreDataProvider

Public Class Statistiche_R
    Inherits AgronicaCoreDataProvider.LogProvider

    Public Function getStatistiche_PrevisioniAI(piva As String,
                                                dataStats As Date,
                                                objParametri_Server As AgronicaCoreParametri) As DataTable

        Dim objStatPrevisioniDAL As New AgronicaCoreContabDAL.Statistiche_R
        Dim dt As DataTable

        Dim Inizio_Anno = "01/01/" & dataStats.Year
        Dim Fine_Anno = "31/12/" & dataStats.Year

        Dim xFiltroAggiuntivo As String = ""
        xFiltroAggiuntivo = xFiltroAggiuntivo + " Reg_Impianti.Cul_Cod != 0 "
        xFiltroAggiuntivo = xFiltroAggiuntivo + " AND ( ((Imprese_Progetti.Validita_inizio <= " + UtilityProvider.Agro_SQL_SaveDate(dataStats)
        xFiltroAggiuntivo = xFiltroAggiuntivo + " AND " + UtilityProvider.Agro_SQL_SaveDate(dataStats) + " <= Imprese_Progetti.Validita_Fine)) OR  "
        xFiltroAggiuntivo = xFiltroAggiuntivo + "   ((Imprese_Progetti.Validita_Fine >= " + UtilityProvider.Agro_SQL_SaveDate(Inizio_Anno)
        xFiltroAggiuntivo = xFiltroAggiuntivo + " AND " + UtilityProvider.Agro_SQL_SaveDate(Fine_Anno) + " >= Imprese_Progetti.Validita_Fine) ) ) "

        dt = objStatPrevisioniDAL.Leggi_Statistiche_PrevisioniAI(piva, xFiltroAggiuntivo, objParametri_Server)

        Return dt

    End Function

End Class
