Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.Sicurezza

Public Class Esportazione_OP_Catasto
    Inherits System.Web.UI.Page

    Dim objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri
    Private Qs_Data As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Tolgo la pagina dalla cache
        Response.Expires = 0

        '----- Verifico che l'utente sia autenticato
        If Session("ASG_Utente_Username") = "" Then
            Response.Redirect("../../Messaggi/AccessoNegato.htm")
        End If

        objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))

        Qs_Data = Stringa_Decodifica(Request.QueryString("data").ToString, _
                               AgroKey_EncoderDecoder, _
                               Server)

        Dim Messaggio As String = ""
        Dim Dt As New DataTable
        Dim strFiltroImpianti As String
        Dim i As Integer
        Dim XmlDoc As New System.Xml.XmlDocument
        Dim XML_FiltroStampa As System.Xml.XmlElement
        Dim XMLs_VariabiliStampe As System.Xml.XmlNodeList
        Dim XML_VariabiliStampe As System.Xml.XmlElement

        Dim strXmlVariabilistampe As String
        strXmlVariabilistampe = Session("strXmlVariabilistampe")

        'Carico la stringa xml in un nuovo documento
        XmlDoc = New System.Xml.XmlDocument
        XmlDoc.LoadXml(strXmlVariabilistampe)

        Dim objAgroWeb As New AgronicaCoreGestioneRichieste.AgroWebConfig

        Dim Query1_TempTableCreazione As String = ""
        Dim Query2_TempTableIndice As String = ""
        Dim Query3_TempTableFill As String = ""
        Dim Query4_TempTableJoin As String = ""
        Dim stbQ As New System.Text.StringBuilder

        If XmlDoc.HasChildNodes Then

            XML_FiltroStampa = XmlDoc.SelectSingleNode("ParametriAgronicaStampe_2010")

            XMLs_VariabiliStampe = XML_FiltroStampa.GetElementsByTagName("VariabiliStampe")

            For i = 0 To XMLs_VariabiliStampe.Count - 1

                XML_VariabiliStampe = XMLs_VariabiliStampe.Item(i)

                Query3_TempTableFill += " INSERT INTO #tempimpianti (Piva, Sa_Cod, Appezza, Id_Reg)  " & vbCrLf
                Query3_TempTableFill += " VALUES     ('" + Agro_SQL_SaveText(XML_VariabiliStampe.GetAttribute("piva")) + "'," + Agro_SQL_SaveNum(XML_VariabiliStampe.GetAttribute("sa_cod")) + "," + Agro_SQL_SaveNum(XML_VariabiliStampe.GetAttribute("appezza")) + "," + Agro_SQL_SaveNum(XML_VariabiliStampe.GetAttribute("id_reg")) + ")  " & vbCrLf

            Next


            If Query3_TempTableFill <> "" Then

                Query1_TempTableCreazione += " SELECT Piva, Sa_Cod, Appezza, Id_Reg "
                Query1_TempTableCreazione += "   INTO #tempimpianti   "
                Query1_TempTableCreazione += "       FROM Reg_Impianti "
                Query1_TempTableCreazione += "           WHERE 1 = 0   "
                Query1_TempTableCreazione += vbCrLf

                Query2_TempTableIndice = " CREATE UNIQUE INDEX [#AgroIndextempimpianti] ON [dbo].[#tempimpianti]([Piva], [Sa_Cod], [Appezza], [Id_Reg]) "

                '----------------------------------------------------------
                stbQ.Append(" SELECT DISTINCT Reg_Impianti.Piva,  ISNULL(Imprese.rag_soc, ' ') AS rag_soc ")
                stbQ.Append(" ,  '' AS Tipo_Socio ")
                stbQ.Append(" ,  ISNULL(Imprese_1.Piva, ' ') AS PIVA_padre ")
                stbQ.Append(" ," & CDate(Qs_Data).Year.ToString & " AS Anno ")
                stbQ.Append(", ISNULL(Codifica_SpecieVegetali_Dogane.NC_Cod, ' ') AS Codice_Prodotto ")
                stbQ.Append(", ISNULL(Codifica_SpecieVegetali_Dogane.NC_Des, ' ') AS Descrizione_Prodotto ")
                '(05/10/2015 fede) spostata lettura codici x evitare doppioni righe (es. pesco mappato con 3 codici specie)
                'stbQ.Append(", ISNULL(xDecoder_Specie_OP2007.Specie_Cod, ' ') AS Codice_Specie ")
                'stbQ.Append(", ISNULL(xDecoder_Specie_OP2007.Specie_Des, ' ') AS Descrizione_Specie ")
                'stbQ.Append(", ISNULL(xDecoder_Varieta_OP2007.Varieta_Cod, ' ') AS Codice_Varieta ")
                'stbQ.Append(", ISNULL(xDecoder_Varieta_OP2007.Varieta_Des, ' ') AS Descrizione_Varieta ")
                stbQ.Append(", ' ' AS Codice_Specie ")
                stbQ.Append(", ' ' AS Descrizione_Specie ")
                stbQ.Append(", ' ' AS Codice_Varieta ")
                stbQ.Append(", ' ' AS Descrizione_Varieta ")

                stbQ.Append(", cultivar.CUL_COD,cultivar.veg_cod ")

                stbQ.Append(", ISNULL(GruppoVegetale.Gru_Des,' ') AS Gru_Des ")

                stbQ.Append(", ISNULL(year(Reg_Impianti.Validita_Inizio), ' ') AS anno_impianto ")

                stbQ.Append(", ISNULL(( SELECT TOP 1  Reg_Impianti_Codici.val_cod  ")
                stbQ.Append("                FROM Reg_Impianti_Codici  ")
                stbQ.Append("                WHERE(Reg_Impianti_Codici.Piva = Reg_Impianti.Piva) ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Progetto_Cod = 0)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.id_cod = " + CStr(enum_CodiciAnagrafe.Impianto_TraFila_Maschio) + ") " & vbCrLf)
                stbQ.Append(" ) , 0)   ")
                stbQ.Append(" + ' x ' + ")
                stbQ.Append(" ISNULL(( SELECT TOP 1  Reg_Impianti_Codici.val_cod  ")
                stbQ.Append("                FROM Reg_Impianti_Codici  ")
                stbQ.Append("                WHERE(Reg_Impianti_Codici.Piva = Reg_Impianti.Piva) ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.Progetto_Cod = 0)  ")
                stbQ.Append(" AND     (Reg_Impianti_Codici.id_cod = " + CStr(enum_CodiciAnagrafe.Impianto_SuFila_Maschio) + ") " & vbCrLf)
                stbQ.Append(" ) , 0)  AS Sesto, ")

                stbQ.Append(" ROUND(ISNULL(( SELECT TOP 1  Imprese_Progetti.P_HA  ")
                stbQ.Append("                FROM Imprese_Progetti  ")
                stbQ.Append("                WHERE(Imprese_Progetti.Piva = Reg_Impianti.Piva) ")
                stbQ.Append(" AND     (Imprese_Progetti.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append(" AND     (Imprese_Progetti.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append(" AND     (Imprese_Progetti.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append(" AND     Imprese_Progetti.validita_inizio <=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append(" AND     Imprese_Progetti.validita_fine >=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append(" ) , 0) * ISNULL(AppezzamentiXParticelle.AREA, 0),0) AS N_piante  ")

                stbQ.Append(", ISNULL(( SELECT    TOP 1 Regolamenti.Reg_Des  ")
                stbQ.Append("                FROM Imprese_Progetti  ")
                stbQ.Append("             INNER JOIN  Regolamenti ON Imprese_Progetti.Regolamento_Cod = Regolamenti.Reg_Cod  ")
                stbQ.Append("                WHERE(Imprese_Progetti.Piva = Reg_Impianti.Piva) ")
                stbQ.Append("             AND     (Imprese_Progetti.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append("           AND     Imprese_Progetti.validita_inizio <=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append("           AND     Imprese_Progetti.validita_fine >=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append(" ) , ' ') AS Regolamento  ")

                stbQ.Append(",  ISNULL(GruppoFinalita.Grfi_Des, ' ') AS grfi_des ")
                stbQ.Append(",  ISNULL(Copertura.Cop_Des, 'Nessuna') AS cop_des ")

                stbQ.Append(", CASE ImpreseXParticelle.TitoloPossesso WHEN 1 THEN 'Proprietà' WHEN 2 THEN 'Comodato' ")
                stbQ.Append("                                        WHEN 3 THEN 'Affitto con contratto' WHEN 4 THEN 'Affitto senza contratto' ")
                stbQ.Append("                                        WHEN 5 THEN 'In conto terzi' ELSE 'Altro' END AS titolo_possesso ")

                stbQ.Append(", ISNULL(( SELECT    TOP 1 CONVERT(varchar(10), Imprese_Progetti.Validita_Inizio, 103) ")
                stbQ.Append("                FROM Imprese_Progetti  ")
                stbQ.Append("                WHERE(Imprese_Progetti.Piva = Reg_Impianti.Piva) ")
                stbQ.Append("             AND     (Imprese_Progetti.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append("               AND     Imprese_Progetti.validita_inizio <=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append("               AND     Imprese_Progetti.validita_fine >=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append(" ) , ' ') AS inizio_esercizio   ")
                stbQ.Append(", ISNULL(( SELECT    TOP 1 CONVERT(varchar(10), Imprese_Progetti.Validita_fine, 103) ")
                stbQ.Append("                FROM Imprese_Progetti  ")
                stbQ.Append("                WHERE(Imprese_Progetti.Piva = Reg_Impianti.Piva) ")
                stbQ.Append("             AND     (Imprese_Progetti.Sa_Cod = Reg_Impianti.sa_cod)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Appezza  = Reg_Impianti.Appezza)  ")
                stbQ.Append("             AND     (Imprese_Progetti.Id_Reg = Reg_Impianti.Id_Reg)  ")
                stbQ.Append("           AND     Imprese_Progetti.validita_inizio <=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append("           AND     Imprese_Progetti.validita_fine >=" & Agro_SQL_SaveDate(Qs_Data) & "   ")
                stbQ.Append(" ) , ' ') AS fine_esercizio   ")

                stbQ.Append(", ISNULL(( SELECT     TOP 1 CAC_Codifica_InfoAggiuntive.InfoAgg_Des " & vbCrLf)
                stbQ.Append(" FROM     Reg_Impianti_Codici " & vbCrLf)
                stbQ.Append(" INNER JOIN CAC_Codifica_InfoAggiuntive ON Reg_Impianti_Codici.val_cod = CAC_Codifica_InfoAggiuntive.InfoAgg_Cod" & vbCrLf)
                stbQ.Append(" WHERE  (Reg_Impianti_Codici.Piva= Reg_Impianti.Piva) " & vbCrLf)
                stbQ.Append(" AND (Reg_Impianti_Codici.Sa_Cod = Reg_Impianti.sa_cod) " & vbCrLf)
                stbQ.Append(" AND (Reg_Impianti_Codici.Appezza  = Reg_Impianti.Appezza) " & vbCrLf)
                stbQ.Append(" AND (Reg_Impianti_Codici.Id_Reg = Reg_Impianti.Id_Reg) " & vbCrLf)
                stbQ.Append(" AND (Reg_Impianti_Codici.Progetto_Cod = 0) " & vbCrLf)
                stbQ.Append(" AND (Reg_Impianti_Codici.id_cod = " + CStr(enum_CodiciAnagrafe.Dettaglio_Specie_Personalizzato) + ") " & vbCrLf)
                stbQ.Append(" AND (CAC_Codifica_InfoAggiuntive.Piva_SuperUser = '" & Agro_SQL_SaveText(CStr(Session("ASG_SuperUser_CodFiscale"))) & "') " & vbCrLf)
                stbQ.Append(" AND (CAC_Codifica_InfoAggiuntive.Argomento_Cod = 2) " & vbCrLf)
                stbQ.Append(" ) , '') AS dett_specie_pers, " & vbCrLf)

                stbQ.Append(" ISNULL(AppezzamentiXParticelle.SEZIONE, ' ') AS Sezione,  ")
                stbQ.Append(" ISNULL(AppezzamentiXParticelle.FOGLIO, ' ') AS Foglio,  ")
                stbQ.Append(" ISNULL(AppezzamentiXParticelle.NUMERO, ' ') AS Particella,  ")
                stbQ.Append(" ISNULL(AppezzamentiXParticelle.SUBALTERNO, ' ') AS Sub,  ")

                stbQ.Append("  CAST(convert(float, (cast (isnull(ParticelleCatastali.ETTARI,0) as varchar(100))+ '.' + " & vbCrLf)
                stbQ.Append("     right('000' + cast (isnull(ParticelleCatastali.Are,0) as varchar(100)), 2)  +  " & vbCrLf)
                stbQ.Append("     right('000' + cast (isnull(ParticelleCatastali.CentiAre,0) as varchar(100)), 2))    " & vbCrLf)
                stbQ.Append("     ) As decimal(18,4))   as Sup_Cat,  " & vbCrLf)

                'stbQ.Append(" ISNULL(ParticelleCatastali.ETTARI, 0) AS ETTARI,  ")
                'stbQ.Append(" ISNULL(ParticelleCatastali.are, 0) AS ARE,  ")
                'stbQ.Append(" ISNULL(ParticelleCatastali.centiare, 0) AS CENTIARE,  ")
                stbQ.Append(" CAST(ISNULL(AppezzamentiXParticelle.AREA, 0) As decimal(18,4))  AS AREA, ")
                'stbQ.Append(" cast(CAST(ISNULL(AppezzamentiXParticelle.AREA, 0) As decimal(18,4)) as varchar(100) )  AS AREA, ")

                stbQ.Append(" ISNULL(istat.localita, ' ') AS COMUNE,  ")
                stbQ.Append(" ISNULL(istat.comuni_prov, ' ') AS PROVINCIA, ")

                stbQ.Append(" ISNULL(IndCentro.ind_des, ' ') AS indirizzo ")
                'resa ha
                stbQ.Append(" , ISNULL((SELECT TOP 1 Imprese_Progetti.Produzione_Prevista FROM Imprese_Progetti ")
                stbQ.Append("        WHERE Reg_Impianti.Piva = Imprese_Progetti.PIVA ")
                stbQ.Append("	     AND Reg_Impianti.sa_cod = Imprese_Progetti.sa_cod ")
                stbQ.Append("	     AND Reg_Impianti.appezza = Imprese_Progetti.APPEZZA ")
                stbQ.Append("	     AND Reg_Impianti.id_reg = Imprese_Progetti.ID_REG ")
                stbQ.Append("       AND Imprese_Progetti.Validita_Inizio <= " & Agro_SQL_SaveDate(Qs_Data) & " " & vbCrLf)
                stbQ.Append("       AND Imprese_Progetti.Validita_Fine >= " & Agro_SQL_SaveDate(Qs_Data) & " " & vbCrLf)
                stbQ.Append("), 0)/1000 AS ResaHa ")

                'resa x util
                stbQ.Append(" , ISNULL((SELECT TOP 1 Imprese_Progetti.Produzione_Prevista FROM Imprese_Progetti ")
                stbQ.Append("        WHERE Reg_Impianti.Piva = Imprese_Progetti.PIVA ")
                stbQ.Append("	     AND Reg_Impianti.sa_cod = Imprese_Progetti.sa_cod ")
                stbQ.Append("	     AND Reg_Impianti.appezza = Imprese_Progetti.APPEZZA ")
                stbQ.Append("	     AND Reg_Impianti.id_reg = Imprese_Progetti.ID_REG ")
                stbQ.Append("       AND Imprese_Progetti.Validita_Inizio <= " & Agro_SQL_SaveDate(Qs_Data) & " " & vbCrLf)
                stbQ.Append("       AND Imprese_Progetti.Validita_Fine >= " & Agro_SQL_SaveDate(Qs_Data) & " " & vbCrLf)
                stbQ.Append("), 0)/1000 * ISNULL(AppezzamentiXParticelle.AREA, 0) AS ResaxUtil ")

                stbQ.Append("   FROM Reg_Impianti  LEFT OUTER JOIN AppezzamentiXParticelle  ")
                stbQ.Append("  ON Reg_Impianti.Piva = AppezzamentiXParticelle.Piva  ")
                stbQ.Append("  AND Reg_Impianti.Sa_Cod = AppezzamentiXParticelle.Sa_Cod  ")
                stbQ.Append("  AND Reg_Impianti.Appezza = AppezzamentiXParticelle.Appezza  ")
                stbQ.Append("  LEFT OUTER JOIN ParticelleCatastali  ")
                stbQ.Append("  ON ParticelleCatastali.PROV = AppezzamentiXParticelle.PROV  ")
                stbQ.Append("  AND ParticelleCatastali.COM = AppezzamentiXParticelle.COM  ")
                stbQ.Append("  AND ParticelleCatastali.SEZIONE = AppezzamentiXParticelle.SEZIONE  ")
                stbQ.Append("  AND ParticelleCatastali.FOGLIO = AppezzamentiXParticelle.FOGLIO  ")
                stbQ.Append("  AND ParticelleCatastali.NUMERO = AppezzamentiXParticelle.NUMERO  ")
                stbQ.Append("  AND ParticelleCatastali.SUBALTERNO = AppezzamentiXParticelle.SUBALTERNO  ")

                stbQ.Append("  LEFT OUTER JOIN Istat  ")
                stbQ.Append("  on ParticelleCatastali.PROV = istat.prov AND ParticelleCatastali.com = istat.com  ")

                stbQ.Append("  LEFT OUTER JOIN Copertura ON Reg_Impianti.COP_COD = Copertura.Cop_Cod  ")
                stbQ.Append("  LEFT OUTER JOIN GruppoVarietale ON Reg_Impianti.GRVA_Cod_VEG = GruppoVarietale.Grva_Cod  ")
                stbQ.Append("  LEFT OUTER JOIN GruppoFinalita ON Reg_Impianti.GRFI_COD = GruppoFinalita.Grfi_Cod  ")
                stbQ.Append("  LEFT OUTER JOIN Cultivar ON Reg_Impianti.CUL_COD = Cultivar.Cul_Cod  ")
                stbQ.Append("  LEFT OUTER JOIN SpecieVegetali  ")
                stbQ.Append("  ON SpecieVegetali.Veg_Cod = Cultivar.Veg_Cod  ")
                stbQ.Append("  LEFT OUTER JOIN GruppoVegetale ON SpecieVegetali.Gru_Cod = GruppoVegetale.Gru_Cod  ")
                stbQ.Append("  INNER JOIN Imprese ON Reg_Impianti.Piva = Imprese.Piva  ")

                stbQ.Append(" INNER JOIN CentrixIndirizzi " & vbCrLf)
                stbQ.Append(" ON Reg_Impianti.Piva = CentrixIndirizzi.Piva " & vbCrLf)
                stbQ.Append(" AND Reg_Impianti.sa_cod = CentrixIndirizzi.sa_cod " & vbCrLf)
                stbQ.Append(" INNER JOIN Indirizzi IndCentro ON CentrixIndirizzi.cod_indirizzo = IndCentro.cod_indirizzo " & vbCrLf)

                stbQ.Append("  LEFT OUTER JOIN Imprese Imprese_1  ")
                stbQ.Append("  INNER JOIN GerarchiaImprese ON Imprese_1.Piva = GerarchiaImprese.Padre ON Imprese.Piva = GerarchiaImprese.Figlio ")

                stbQ.Append("   INNER JOIN  #tempimpianti  ")
                stbQ.Append("   ON #tempimpianti.Piva = Reg_Impianti.Piva AND Reg_Impianti.Sa_Cod = #tempimpianti.Sa_Cod AND  Reg_Impianti.Appezza = #tempimpianti.Appezza AND  Reg_Impianti.Id_Reg = #tempimpianti.Id_Reg ")

                stbQ.Append("   LEFT OUTER JOIN  ImpreseXParticelle ON AppezzamentiXParticelle.PIVA = ImpreseXParticelle.PIVA AND AppezzamentiXParticelle.SA_COD = ImpreseXParticelle.sa_cod AND   ")
                stbQ.Append("   AppezzamentiXParticelle.PROV = ImpreseXParticelle.PROV And AppezzamentiXParticelle.COM = ImpreseXParticelle.COM And  ")
                stbQ.Append("   AppezzamentiXParticelle.SEZIONE = ImpreseXParticelle.SEZIONE And AppezzamentiXParticelle.FOGLIO = ImpreseXParticelle.FOGLIO And  ")
                stbQ.Append("   AppezzamentiXParticelle.NUMERO = ImpreseXParticelle.NUMERO And  ")
                stbQ.Append("   AppezzamentiXParticelle.SUBALTERNO = ImpreseXParticelle.SUBALTERNO  ")

                'stbQ.Append("   LEFT OUTER JOIN xDecoder_Varieta_OP2007 ON Cultivar.Cul_Cod = xDecoder_Varieta_OP2007.Cul_Cod LEFT OUTER JOIN ")
                'stbQ.Append("   xDecoder_Specie_OP2007 ON SpecieVegetali.Veg_Cod = xDecoder_Specie_OP2007.Veg_Cod LEFT OUTER JOIN ")
                stbQ.Append("   LEFT OUTER JOIN  Codifica_SpecieVegetali_Dogane ON SpecieVegetali.Veg_Cod = Codifica_SpecieVegetali_Dogane.Veg_cod ")

                ' stbQ.Append("   ORDER BY coop_padre Asc ")

                Query4_TempTableJoin = stbQ.ToString

                '/**************************************************
                '         NUOVO METODO CON TABELLA TEMPORANEA  
                Dim obj_MultiQuery As New AgronicaCoreDataProvider.AccessoMultiQuery
                Dt = obj_MultiQuery.MLT_SelectFiltrataConTabellaTemporanea_2013(Query1_TempTableCreazione,
                                                                        Query2_TempTableIndice,
                                                                        Query3_TempTableFill,
                                                                        Query4_TempTableJoin,
                                                                         objParametri_Server.StringaConnessione,
                                                                         Messaggio)

                If Not Dt Is Nothing AndAlso Dt.Rows.Count > 0 Then

                    '--------------------------------------------------------------
                    'assegna le codifiche specie e varieta OP
                    Dim objCodificheOP As New AgronicaCoreMetaSchemaDAL.Tabelle_OP_R
                    Dim DtSpecieOP As DataTable
                    Dim DtVarietaOP As DataTable
                    Dim strVarietaNONMappate As String
                    Dim Flag_VarietaValorizzata As Boolean = False
                    Dim Flag_SpecieValorizzata As Boolean = False

                    DtSpecieOP = objCodificheOP.xDecoder_Specie_OP2007_Leggi(0, 0, "", 0, "", 0, "", 0, "", "", "", objParametri_Server)
                    DtVarietaOP = objCodificheOP.xDecoder_Varieta_OP2007_Leggi_2(0, 0, "", 0, "", 0, "", CDate(Qs_Data), "", "", objParametri_Server)

                    For i = 0 To Dt.Rows.Count - 1

                        If Not IsNothing(Dt.Rows(i).Item("cul_cod")) AndAlso Not IsDBNull(Dt.Rows(i).Item("cul_cod")) Then
                            Flag_VarietaValorizzata = True
                        Else
                            Flag_VarietaValorizzata = False
                        End If
                        If Not IsNothing(Dt.Rows(i).Item("veg_cod")) AndAlso Not IsDBNull(Dt.Rows(i).Item("veg_cod")) Then
                            Flag_SpecieValorizzata = True
                        Else
                            Flag_SpecieValorizzata = False
                        End If

                        If Flag_VarietaValorizzata = True Or Flag_SpecieValorizzata = True Then

                            If Flag_VarietaValorizzata = True Then

                                Dim DrVarieta() As DataRow = DtVarietaOP.Select("cul_cod=" & Dt.Rows(i).Item("cul_cod"))
                                If Not DrVarieta Is Nothing AndAlso DrVarieta.Length > 0 Then
                                    Dt.Rows(i).Item("Codice_Specie") = DrVarieta(0).Item("Specie_Cod")
                                    Dt.Rows(i).Item("Descrizione_Specie") = DrVarieta(0).Item("Specie_Des")
                                    Dt.Rows(i).Item("Codice_Varieta") = DrVarieta(0).Item("Varieta_Cod")
                                    Dt.Rows(i).Item("Descrizione_Varieta") = DrVarieta(0).Item("Varieta_Des")
                                Else
                                    If Flag_SpecieValorizzata = True Then
                                        Dim DrSpecie() As DataRow = DtSpecieOP.Select("veg_cod=" & Dt.Rows(i).Item("veg_cod"))
                                        If Not DrSpecie Is Nothing AndAlso DrSpecie.Length > 0 Then
                                            Dt.Rows(i).Item("Codice_Specie") = DrSpecie(0).Item("Specie_Cod")
                                            Dt.Rows(i).Item("Descrizione_Specie") = DrSpecie(0).Item("Specie_Des")
                                            Dt.Rows(i).Item("Codice_Varieta") = 999
                                            Dt.Rows(i).Item("Descrizione_Varieta") = "ALTRE VARIETA"
                                        End If
                                        strVarietaNONMappate &= "veg_cod=" & Dt.Rows(i).Item("veg_cod") & " - cul_cod=" & Dt.Rows(i).Item("cul_cod") & vbCrLf
                                    End If
                                End If
                            Else
                                'varietà non presente
                                If Flag_SpecieValorizzata = True Then
                                    Dim DrSpecie() As DataRow = DtSpecieOP.Select("veg_cod=" & Dt.Rows(i).Item("veg_cod"))
                                    If Not DrSpecie Is Nothing AndAlso DrSpecie.Length > 0 Then
                                        Dt.Rows(i).Item("Codice_Specie") = DrSpecie(0).Item("Specie_Cod")
                                        Dt.Rows(i).Item("Descrizione_Specie") = DrSpecie(0).Item("Specie_Des")
                                        Dt.Rows(i).Item("Codice_Varieta") = 999
                                        Dt.Rows(i).Item("Descrizione_Varieta") = "ALTRE VARIETA"
                                    End If
                                    strVarietaNONMappate &= "veg_cod=" & Dt.Rows(i).Item("veg_cod") & " - cul_cod=" & Dt.Rows(i).Item("cul_cod") & vbCrLf
                                End If
                            End If
                        Else
                            Dt.Rows(i).Item("Codice_Specie") = ""
                            Dt.Rows(i).Item("Descrizione_Specie") = ""
                            Dt.Rows(i).Item("Codice_Varieta") = ""
                            Dt.Rows(i).Item("Descrizione_Varieta") = ""
                        End If

                    Next

                    'rimuovo le colonne cul_cod, veg_cod gias
                    Dt.Columns.RemoveAt(11)
                    Dt.Columns.RemoveAt(12)


                    For i = 0 To Dt.Columns.Count - 1
                        Select Case Dt.Columns(i).ColumnName.ToLower
                            Case "rag_soc".ToLower
                                Dt.Columns(i).ColumnName = "Ragione Sociale Impresa"
                            Case "piva".ToLower
                                Dt.Columns(i).ColumnName = "Partita Iva Impresa"
                            Case "tipo_socio".ToLower
                                Dt.Columns(i).ColumnName = "Tipo Impresa"
                            Case "piva_padre".ToLower
                                Dt.Columns(i).ColumnName = "Partita Iva Padre"
                            Case "codice_prodotto".ToLower
                                Dt.Columns(i).ColumnName = "Prodotto"
                            Case "codice_specie".ToLower
                                Dt.Columns(i).ColumnName = "Specie"
                            Case "codice_varieta".ToLower
                                Dt.Columns(i).ColumnName = "Varieta'"
                            Case "gru_des".ToLower
                                Dt.Columns(i).ColumnName = "Tipo Prodotto"
                            Case "n_piante".ToLower
                                Dt.Columns(i).ColumnName = "Numero Piante"
                            Case "regolamento".ToLower
                                Dt.Columns(i).ColumnName = "Tipologia Produzione"
                            Case "grfi_des".ToLower
                                Dt.Columns(i).ColumnName = "Tipologia Prodotto"
                            Case "cop_des".ToLower
                                Dt.Columns(i).ColumnName = "Tipologia Coltivazione"
                            Case "titolo_possesso".ToLower
                                Dt.Columns(i).ColumnName = "Titolo Possesso"
                            Case "inizio_esercizio".ToLower
                                Dt.Columns(i).ColumnName = "Impegnativa dal"
                            Case "fine_esercizio".ToLower
                                Dt.Columns(i).ColumnName = "Impegnativa al"
                            Case "dett_specie_pers".ToLower
                                Dt.Columns(i).ColumnName = "N° Ciclo"
                            Case "area".ToLower
                                Dt.Columns(i).ColumnName = "Superficie Coltivata [ha]"
                            Case "sup_cat".ToLower
                                Dt.Columns(i).ColumnName = "Superficie Catastale [ha]"
                            Case "Descrizione_Prodotto".ToLower
                                Dt.Columns(i).ColumnName = "Descrizione Prodotto"
                            Case "Descrizione_Specie".ToLower
                                Dt.Columns(i).ColumnName = "Descrizione Specie"
                            Case "Descrizione_varieta".ToLower
                                Dt.Columns(i).ColumnName = "Descrizione Varieta"
                            Case "anno_impianto".ToLower
                                Dt.Columns(i).ColumnName = "Anno Impianto"
                            Case "ResaHA".ToLower
                                Dt.Columns(i).ColumnName = "Resa/Ha [t]"
                            Case "ResaxUtil".ToLower
                                Dt.Columns(i).ColumnName = "Resa x Utilizzo [t]"
                        End Select
                    Next



                    AgronicaCoreGestioneRichieste.Esporta.EsportaExcel(Dt, "EsportazioneOPCatasto", Page)

                End If

            End If

        End If

    End Sub

End Class