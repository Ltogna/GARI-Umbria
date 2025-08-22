Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreDataProvider

Public Class DatiMeteoXCalcoloPrelievi

    Private _objMeteoNT As AgronicaCoreWebService.MeteoNT
    Private _objParametri_Server As AgronicaCoreParametri
    Private _linguaSession As Lingua

    Public Sub New(ByVal lang As Lingua, ByRef objParametri_Server As AgronicaCoreParametri)
        _objMeteoNT = New AgronicaCoreWebService.MeteoNT
        _objParametri_Server = objParametri_Server
        _linguaSession = lang
    End Sub

    Public Function RecuperaDatiMeteo(ByVal id_stazione As String, ByVal DataDa As DateTime, ByVal DataA As DateTime) As List(Of DatiMeteoXCalcoloConfronto)
        Dim ret As New List(Of DatiMeteoXCalcoloConfronto)
        Try

            Dim meteostruct = LeggiStrutturaSensoriStazioneMeteo(id_stazione)

            If meteostruct Is Nothing Then
                Throw New Exception("La stazione meteo " + id_stazione + " non ha la coppia di sensori Pluviometro e/o Temperatura. Non è possibile recuperare i dati di pioggia e temperatura necessari al calcolo. operazione interrotta")
            End If

            '1- leggi dati da stazione meteo

            Dim objParamDatiMeteo As New JObject

            objParamDatiMeteo("TipoSorgente") = 1
            objParamDatiMeteo("Sorgente") = id_stazione
            objParamDatiMeteo("DataInizio") = DataDa.ToString("yyyy-MM-dd")
            objParamDatiMeteo("DataFine") = DataA.ToString("yyyy-MM-dd")
            objParamDatiMeteo("FrequenzaDati") = "G"
            objParamDatiMeteo("SogliaTemp") = 0
            objParamDatiMeteo("SogliaFreddo") = 0
            objParamDatiMeteo("CfrStorico") = ""
            objParamDatiMeteo("LinguaCodiceISO") = _linguaSession.CodiceISO


            Dim deserilizedResponse = JsonConvert.DeserializeObject(Of rispostaStandard(Of Risposta_DatiReteAcqua))(_objMeteoNT.DatiMeteoElabora(objParamDatiMeteo, _objParametri_Server))
            If deserilizedResponse.RispostaOK = True Then
                Dim tmp_table = JObject.Parse(deserilizedResponse.RispostaStringa.Meteo_Table)
                Dim temp_meteo_data As New List(Of TempMeteoData)
                Dim jr = JArray.Parse(tmp_table("kendo_rows").ToString)

                For i = 0 To jr.Count - 1

                    Dim obj = JObject.Parse(jr(i).ToString)

                        temp_meteo_data.Add(New TempMeteoData() With {
                                        .data = DateTime.Parse(obj.Property("DataOra").Value().ToString()),
                                        .week = AgronicaCoreUtility.DataOra.GetWeekNumberFromDate(.data, Globalization.CalendarWeekRule.FirstFullWeek),
                                        .pioggia = Decimal.Parse(obj.Property("S_" + meteostruct.Find(Function(x) x.id_tipoSensore = 2).id_sensore.ToString()).Value().ToString()),
                                        .temp_min = Decimal.Parse(obj.Property("SD_MIN_" + meteostruct.Find(Function(x) x.id_tipoSensore = 1).id_sensore.ToString()).Value().ToString()),
                                        .temp_max = Decimal.Parse(obj.Property("SD_MAX_" + meteostruct.Find(Function(x) x.id_tipoSensore = 1).id_sensore.ToString()).Value().ToString()),
                                        .temp_med = Decimal.Parse(obj.Property("S_" + meteostruct.Find(Function(x) x.id_tipoSensore = 1).id_sensore.ToString()).Value().ToString())
                                        })
                    Next

                Dim o As DatiMeteoXCalcoloConfronto
                Dim week As Integer = -1
                For Each meteo In temp_meteo_data.OrderBy(Of Integer)(Function(x) x.week)
                    If week <> meteo.week Then
                        If o IsNot Nothing Then
                            ret.Add(o)
                        End If
                        o = New DatiMeteoXCalcoloConfronto()
                        o.anno = meteo.data.Year
                        o.settimana = meteo.week
                        week = meteo.week
                    End If
                    o.pioggia.Add(meteo.pioggia)
                    o.temp_min.Add(meteo.temp_min)
                    o.temp_media.Add(meteo.temp_med)
                    o.temp_max.Add(meteo.temp_max)
                Next

            Else
                Throw New Exception(deserilizedResponse.Errore)
            End If

        Catch ex As Exception
            ret = Nothing
        End Try
        Return ret
    End Function

    Private Function LeggiStrutturaSensoriStazioneMeteo(ByVal id_stazione As String) As List(Of TipiSensoreXStazione)
        Dim sensorList As List(Of TipiSensoreXStazione)

        'recupero la struttura dei sensori per la stazione in modo da recuperare dai dati solo la parte di pluviometro e temperatura (con i due sensori derivati max e min)
        Dim objParamStructStation As New JObject
        objParamStructStation("id_stazione") = id_stazione
        Try
            sensorList = JsonConvert.DeserializeObject(Of List(Of TipiSensoreXStazione))(JsonConvert.DeserializeObject(Of RispostaStandard)(_objMeteoNT.LeggiTipoSensoriPerStazione(objParamStructStation, _objParametri_Server)).RispostaStringa)

            'tiposensore=1 -> pluviomentro
            'tiposensore=2 -> temperatura

            If sensorList.Any(Function(x) x.id_tipoSensore = 1) = False Or sensorList.Any(Function(x) x.id_tipoSensore = 2) = False Then
                Throw New Exception("La stazione meteo " + id_stazione + " non ha la coppia di sensori Pluviometro e/o Temperatura. Non è possibile recuperare i dati di pioggia e temperatura necessari al calcolo. operazione interrotta")
            End If
        Catch ex As Exception
            sensorList = Nothing
        End Try

        Return sensorList
    End Function


    Private Class TempMeteoData
        Public Property data As DateTime
        Public Property week As Integer
        Public Property pioggia As Decimal
        Public Property temp_min As Decimal
        Public Property temp_med As Decimal
        Public Property temp_max As Decimal
    End Class

End Class


