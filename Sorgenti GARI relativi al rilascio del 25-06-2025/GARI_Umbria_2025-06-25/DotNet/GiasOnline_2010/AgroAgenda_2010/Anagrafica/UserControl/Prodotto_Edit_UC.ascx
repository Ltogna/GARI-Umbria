<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Prodotto_Edit_UC.ascx.vb" Inherits="AgroAgenda_2010.Prodotto_Edit_UC" %>
<%@ Register Assembly="AgronicaControlli_2010" Namespace="AgronicaControlli_2010" TagPrefix="cc1" %>

    <style type="text/css">
        .errorClass {
            border-color:#D41E1A;
            border-width: 1px;
            border-style: dotted;
            background-color: Yellow;               
        }

        .buttonClass {
            margin: 0 0 10px 1px;
        }

        .jumbotron {
            margin-bottom: 0 !important;
        }

        .erroreCampiObbigatori {
            border:2px solid #D41E1A;             
        }

        /*Larghezza calendario come gli altri campi di input dei Parametri Qualitativi*/
        .kendoCalendar {
            width: 100%;
        }

        .table_prodotto_Edit_UC {
            border-collapse:collapse;
            border-spacing:0;
            border:0 none;
            border:0; 
            height:auto;
            width:auto;
            POSITION:absolute;
        }

        .table_prodotto_Edit_UC td {
            border:0 none;
            padding:1px;
            text-align:left;
        }

        .prodotto_Edit_UC_infoArea {
            background-color: #FFF;
            margin: 16px;
            padding: 20px 10px;
            border-radius: 5px;
        }
     
        .prodotto_Edit_UC_searchArea {
            margin: 5px;
            padding: 10px;
        }
        .prodotto_Edit_UC_searchArea .jumbotron {
            padding: 0 15px 0 15px;
        }
        .tab-pane .jumbotron {
            display: inline;
        }
        .tab-pane.DatiTecnici .jumbotron div, .prodotto_Edit_UC_searchArea .jumbotron {
            border-width: 0 !important;
            box-shadow: none;
        }
        .tab-pane .k-grid { box-shadow: none; }
        .border_no { 
            box-shadow: none;
            border-width: 0;
            padding-left: 0;
        }
        
    </style>

    <div class="panel-group prodotto_Edit_UC_searchArea" style="display: none;">
            <div class="jumbotron" style="padding-top: 30px;">
                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-12">
                        <div class="form-horizontal">
						    <div class="form-group">
								<div class="input-group">
                                     <%--i18N Categorie da tradurre--%>
									<label class="input-group-addon alert-info" for="id_ddl_prodotto_Edit_UC_Categorie">Categoria Prodotti:</label>
									<input type="text" id='ddl_prodotto_Edit_UC_Categorie' class="form-control" />
								</div>
							</div>
						</div>
                    </div>
                    <%--i18N Radio button Regolamento: da tradurre--%>
                   <div class="col-lg-5 col-md-5 col-sm-12 gias-pt-15px">
                       <div class="row-radio-group" id="FiltroRegGroup">
                           <label class="lbl_required" id="lbl_filtro_reg" for="FiltroRegGroup">Regolamento:</label>
                           <div class="radio-single">
                               <input id="FiltroReg_TUTTI" name="FiltroReg" value="0" class="k-radio" type="radio" />
                               <label class="k-radio-label" for="FiltroReg_TUTTI">Tutti</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroReg_CONV" name="FiltroReg" value="1" class="k-radio" type="radio" />
                               <label class="k-radio-label" for="FiltroReg_CONV">CONV</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroReg_BIO" name="FiltroReg" value="4" class="k-radio" type="radio" />
                               <label class="k-radio-label" for="FiltroReg_BIO">BIO</label>
                           </div>
                       </div>
                    
                       <%--i18N Radio button Visibilita: da tradurre--%>
                       <div class="row-radio-group" id="FiltroVisiGroup">
                           <label class="lbl_required" id="lbl_filtro_visibilita" for="FiltroVisiGroup">Visibilità:</label>
                           <div class="radio-single">
                               <input id="FiltroVisi_TUTTI" name="FiltroVisi" value="-99" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroVisi_TUTTI">Tutti</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroVisi_Privato" name="FiltroVisi" value="0" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroVisi_Privato">Privato</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroVisi_Pubblico" name="FiltroVisi" value="-1" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroVisi_Pubblico">Pubblico</label>
                           </div>
                       </div>

                       <%--i18N Radio button Valorizzati: da tradurre--%>
                       <div class="row-radio-group" id="FiltroValorizGroup">
                           <label class="lbl_required" id="lbl_filtro_valorizzati" for="FiltroValorizGroup">Valorizzati:</label>
                           <div class="radio-single">
                               <input id="FiltroValoriz_TUTTI" name="FiltroValoriz" value="-1" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroValoriz_TUTTI">Tutti</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroValoriz_Si" name="FiltroValoriz" value="1" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroValoriz_Si">Sì</label>
                           </div>
                           <div class="radio-single">
                               <input id="FiltroValoriz_No" name="FiltroValoriz" value="0" class="k-radio" type="radio" />
                               <label class="k-checkbox-label" for="FiltroValoriz_No">No</label>
                           </div>
                       </div>
                    </div>
				</div>

                <div class="row">
                    <%--Ricerca per descrizione--%>
                    <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 30px;">
                       <div class="form-horizontal">
                            <div class="form-group">
                                <div class="input-group">
                                    <%--i18N Descrizione da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_descrizione_prodotto">Descrizione/Cod.Articolo:</span>
                                    <input type="text" id="txt_descrizione_prodotto" class="form-control " />      
                                    <label id="lbl_prodotto_Edit_UC_descrizione_3_caratteri" for="txt_descrizione_prodotto" class="input-group-addon "> (Inserire 3 caratteri)</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5 col-md-5 col-sm-12" style="padding-top: 30px;">
						<div class="form-horizontal">
							<div class="form-group">
								<div id="ddl_Ricerca_XCategCommle" class="input-group">
                                    <%--i18N Categ. Comm.le: da tradurre--%>
                                    <label class="input-group-addon alert-info " for="multiselCategCommle">Categoria Commerciale:</label>
									<select name="multiselCategCommle" multiple="multiple" ID="multiselCategCommle" class="form-control"></select>
								</div>
							</div>
						</div>
                    </div>
				</div>

                <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px;">
							<div class="form-horizontal">
								<div class="form-group">
									<div id="ddl_Ricerca_XSpecie" class="input-group">
                                        <%--i18N Specie: da tradurre--%>
                                        <label class="input-group-addon alert-info " for="multiselSpecie">Specie:</label>
                                        <select name="multiselSpecie" multiple="multiple" ID="multiselSpecie" class="form-control"></select>
									</div>
								</div>
							</div>
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-12" style="padding-top: 10px;">
							<div class="form-horizontal">
								<div class="form-group">
									<div id="ddl_Ricerca_XVarieta" class="input-group">
                                        <%--i18N Varietà da tradurre--%>
                                        <label class="input-group-addon alert-info " for="multiselVarieta">Varietà:</label>
										<select name="multiselVarieta" multiple="multiple" ID="multiselVarieta" class="form-control"></select>
									</div>
								</div>
							</div>
                        </div>
                </div>

                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 text-right">
                        <div class="btn btn-success xonne-btn-primary" id="prodotto_Edit_UC_Ricerca" style="margin-bottom: 20px;">
                            <%--i18N Ricerca da tradurre--%>
                            <i class="fa fa-search"></i>Ricerca
                        </div>
                    </div>
                </div>
            </div>
    </div>

    <div class="panel-group prodotto_Edit_UC_infoArea" style="display: none;">

            <!--Errore campi non completati in Nuovo-->
            <div class="row" id="div_riepilogo_error" style="margin-bottom: 25px;">
                <div class="col-lg-12 col-md-12">
                    <%--i18N I seguenti campi devono essere compilati: da tradurre--%>
                    <b>I seguenti campi devono essere compilati:</b>
                    <br />
                    <br />
                </div>
                <div class="col-lg-12 col-md-12 col-sm-12">
                    <ul id="div_riepilogo_error_elenco">
                         <%--i18N Il campo da tradurre--%>
                        <li class="voce_1" style="display: none;">Il campo <b>Codice Prodotto</b> è da compilare</li>
                        <li class="voce_2" style="display: none;">Il campo <b>Descrizione</b> è da compilare</li>
                        <li class="voce_3" style="display: none;">Il campo <b>Codice Esterno</b> è da compilare</li>
                    </ul>
                </div>
            </div>

             <div class="panel-area prodotto_Edit_UC_informazioni">               
                <div class="row">
                    <!--Drop Down Categoria Prodotto-->
                    <div class="col-lg-8 col-md-8 col-sm-12" id="prodotto_Edit_UC_modifyArea">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="input-group"> 
                                    <%--i18N Categoria Prodotto da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_prodotto_UC_categ_prod">Categoria Prodotto:</span>
                                    <input type="text" id='ddl_prodotto_UC_categ_prod' class="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Switch Alias(parte GIAS LAN)-->
                    <div class="alias col-lg-4 col-md-4 col-sm-12">
                       <div class="input-group">
                            <%--i18N Alias da tradurre--%>
                            <label class="lbl_required" id="lbl_prodotto_UC_alias" for="cb_prodotto_UC_alias">Prodotto per Descrizioni Alternative:</label>
                            <input type="checkbox" id="cb_prodotto_UC_alias" name="cb_prodotto_UC_alias" class="kendoSwitch"/>
                       </div>
                    </div>
                </div>

                <!--TextBox Codice Prodotto-->
                <div class="row">
                    <div class="col-lg-7 col-md-7 col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="input-group">
                                    <%--i18N Codice Prodotto da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_prodotto_UC_cod_prod">Codice Prodotto:</span>
                                    <input type="text" id="txt_prodotto_UC_cod_prod" class="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--TextBox Codice Esterno-->
                    <div class="col-lg-4 col-md-4 col-sm-12">
                        <div id="codice_esterno" class="input-group">
                            <%--i18N Codice Esterno da tradurre--%>
                            <span class="input-group-addon alert-info" id="lbl_prodotto_UC_cod_est">Codice Esterno:</span>
                            <input type="text" id="txt_prodotto_UC_cod_est" class="form-control" onChange="txt_prodotto_UC_cod_est_change()" disabled />
                        </div>
                    </div>
                </div>

                <!--TextBox Descrizione-->
                <div class="row">
                    <div class="col-lg-8 col-md-8 col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="input-group">
                                    <%--i18N Descrizione da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_prodotto_UC_Descrizione">Descrizione *:</span>
                                    <input type="text" id="txt_prodotto_UC_Descrizione" class="form-control" />
                                </div>
                            </div>
                       </div>
                    </div>
                    <!--Kendo Switch Componi Descrizione-->
                    <div id="switch_prodotto_UC_componi_descrizione" class="col-lg-4 col-md-4 col-sm-12" >
                        <%--i18N Componi Descrizione da tradurre--%>
                        <label class="lbl_required" id="lbl_prodotto_UC_componi_descrizione" for="cb_prodotto_UC_componi_descrizione" >Componi Descrizione in automatico:</label>
                        <input type="checkbox" id="cb_prodotto_UC_componi_descrizione" name="cb_prodotto_UC_componi_descrizione" class="kendoSwitch"/>
                    </div>

                <!--Drop Down Categoria Commerciale(parte GIAS LAN)-->
                <div class="row">
                    <div class="col-lg-8 col-md-8 col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="categoria_risorsa input-group"> 
                                    <%--i18N Categoria Commerciale da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_prodotto_UC_categ_ris">Categoria Commerciale:</span>
                                    <input type="text" id='ddl_prodotto_UC_categ_ris' class="prodotto_UC_categ_ris form-control txtUI required" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Drop Down Linea Produzione(parte GIAS LAN)-->
                <div class="row">
                    <div class="col-lg-8 col-md-8 col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="linea_produzione input-group"> 
                                    <%--i18N Linea Produzione da tradurre--%>
                                    <span class="input-group-addon alert-info" id="lbl_prodotto_UC_linea_prod">Linea Produzione:</span>
                                    <input type="text" id='ddl_prodotto_UC_linea_prod' class="prodotto_UC_linea_prod form-control txtUI required" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
           </div>
                <!-- Fine area con info generali-->
    </div>
    <div class="panel-group prodotto_Edit_UC_modifyArea" style="display: none;">               
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="form-horizontal" style="margin-top: 5px; margin-bottom: 5px;">

                    <ul class="nav nav-tabs" role="tablist" id="Prodotto_Edit_UC_tabs">
                        <%--i18N Nomi Tab da tradurre--%>
                        <li class="tabDatiTecnici active"><a href="#tab_prodotto_UC_dati_tecnici" data-toggle="tab" id="a_tab_prodotto_UC_dati_tecnici">Dati Generali</a></li>
                        <li class="tabConfigurazione"><a href="#tab_prodotto_UC_configurazione" data-toggle="tab" id="a_tab_prodotto_UC_configurazione">Configurazione</a></li>
                        <li class="tabStoricoPrezzi"><a href="#tab_prodotto_UC_storico_prezzi" data-toggle="tab" id="a_tab_prodotto_UC_storico_prezzi">Storico Prezzi</a></li>
                        <li class="tabParametriQualitativi"><a href="#tab_prodotto_UC_parametri_qualitativi" data-toggle="tab" id="a_tab_prodotto_UC_parametri_qualitativi">Parametri Qualitativi</a></li>
                        <li class="tabDatiContabili"><a href="#tab_prodotto_UC_dati_contabilita" data-toggle="tab" id="a_tab_prodotto_UC_dati_contabilita">Dati Contabilità</a></li>
                        <li class="tabTraduzioni"><a href="#tab_prodotto_UC_traduzioni" data-toggle="tab" id="a_tab_prodotto_UC_traduzioni">Traduzioni</a></li>
                        <li class="tabAltriDati"><a href="#tab_prodotto_UC_altri_dati" data-toggle="tab" id="a_tab_prodotto_UC_altri_dati">Dati Vendita Dettaglio</a></li>
                        <li class="tabAlias"><a href="#tab_prodotto_UC_alias" data-toggle="tab" id="a_tab_prodotto_UC_alias">Descrizioni Alternative</a></li>
                    </ul>
				
                    <div class="tab-content">
                        <div class="tab-pane DatiTecnici fade in active" id="tab_prodotto_UC_dati_tecnici" style="overflow: auto; margin-bottom: 70px;">

                      <div class="jumbotron">  

                       <!--Inizio Dati Colturali-->
                       <div class="dati_colturali col-lg-12 border_si border_no">
                        <div class="row">
                            <div id="titolo_dati_cul" class="col-md-12">
                                <%--i18N Dati Colturali da tradurre--%>
                                <h4 style="color: #052747; text-transform: uppercase;">Dati Colturali</h4>
                            </div>
                            <div id="titolo_dati_zoo" class="col-md-12" style="display:none">
                                <%--i18N Dati Dati Zootecnici da tradurre--%>
                                <h4 style="color: #052747; text-transform: uppercase;">Dati Zootecnici</h4>
                            </div>
                        </div>

                        <div class="row">

                            <!--Drop Down Sementi e materiale vivaisti-->
                            <div id="sementi_materiale" class="col-lg-4 col-md-4 col-sm-12">
                                <div class="input-group ">
                                    <%--i18N Sementi e materiale vivaisti da tradurre--%>
                                    <span class="input-group-addon alert-info " id="lbl_prodotto_UC_sementi_materiale"> Sementi e materiale vivaisti:</span>
                                    <input type="text" id="ddl_prodotto_UC_sementi_materiale" class="form-control " />
                                </div>
                            </div>

                           <!--Drop Down Specie Vegetale e Drop Down Specie Animale-->
                            <div class="col-lg-5 col-md-5 col-sm-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="specie_vegetale input-group ">
                                            <%--i18N Specie Vegetale da tradurre--%>
                                            <span class="input-group-addon alert-info" id="lbl_prodotto_UC_specie_veg"> Specie Vegetale:</span>
                                            <input type="text" id="ddl_prodotto_UC_specie_veg" class="form-control " />
                                        </div>
                                        <div class="specie_animale input-group " style="display:none">
                                            <%--i18N Specie Animale da tradurre--%>
                                            <span class="input-group-addon alert-info" id="lbl_prodotto_UC_specie_anim"> Specie Animale:</span>
                                            <input type="text" id="ddl_prodotto_UC_specie_anim" class="form-control " />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                            <!--Drop Down Varietà Colturale e Drop Down Indirizzo Produttivo Animale-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="varieta_colturale input-group">
                                                <%--i18N Varietà Colturale da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_varieta"> Varietà Colturale:</span>
                                                <input type="text" id="ddl_prodotto_UC_varieta" class="form-control " />
                                            </div>
                                            <div class="indirizzo_produttivo input-group " style="display:none">
                                                <%--i18N Indirizzo Produttivo da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_indi_produt"> Indirizzo Produttivo:</span>
                                                <input type="text" id="ddl_prodotto_UC_indi_produt" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Drop Down Tipologia Varietale e Drop Down Razza Animale-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="tipologia_varietale input-group">
                                                <%--i18N Tipologia Varietale da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_tipo_varietale"> Tipologia Varietale:</span>
                                                <input type="text" id="ddl_prodotto_UC_tipo_varietale" class="form-control " />
                                            </div>
                                            <div class="razza_animale input-group " style="display:none">
                                                <%--i18N Razza da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_razza_anim"> Razza:</span>
                                                <input type="text" id="ddl_prodotto_UC_razza_anim" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Drop Down Regolamento-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="regolamento input-group">
                                                <%--i18N Regolamento da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_Regolamento"> Regolamento:</span>
                                                <input type="text" id="ddl_prodotto_UC_Regolamento" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                              <!--Drop Down Prodotto Base di Riferimento-->
                            <div id="prodotto_base" class="row" style="display:none">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class=" input-group ">
                                                <%--i18N  Prodotto Base di Riferimento da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_Prodottobase"> Prodotto Base di Riferimento:</span>
                                                <input type="text" id="ddl_prodotto_UC_Prodottobase" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Drop Down Finalità Produttiva (parte GIAS LAN)-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="ddl_prodotto_UC_final_prod input-group"> 
                                                <%--i18N Finalità Produttiva da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_final_prod">Finalità Produttiva:</span>
                                                <input type="text" id='ddl_prodotto_UC_final_prod' class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div hidden class="tecnologieSementi input-group"> 
                                                <%--i18N Tipologia Sementi --%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_TecnologieSementi">Tecnologie Sementi:</span>
                                                <input type="text" id='ddl_prodotto_UC_TecnologieSementi' class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div hidden class="germinabilita input-group"> 
                                                <%--i18N germinabilita --%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_germinabilita">Germinabilità (%):</span>
                                                <input type="text" name="txt_prodotto_UC_germinabilita" id='txt_prodotto_UC_germinabilita' class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           </div>
                          <!--Fine Dati Colturali-->

                          <!--Referenze parametri Qualitativi-->
                          <div class="Referenze_Param_Qual col-lg-12 border_si" style="display:none">
                            <div class="row">
                                <div class="col-md-12">
                                    <%--i18N Referenze da tradurre--%>
                                    <h4 style="color: #052747; text-transform: uppercase;">Parametri Qualitativi</h4>
                                </div>
                            </div>
                            <div class="row" id="prodotto_UC_parametri_qualitativi_list" style="margin-top: 5px;">
                             <!-- N.B. Riempita dinamicamente  -->
                            </div>
                          </div>
                          <!--Fine Referenze-->


                          <!--Inzio Dettagli-->
                          <div class="dettagli col-lg-12 border_si" style="display:none">
                            <div class="row">
                                <div class="col-md-12">
                                    <%--i18N Dettagli da tradurre--%>
                                    <h4 style="color: #052747; text-transform: uppercase;">Dettagli</h4>
                                </div>
                            </div>
                             <div class="row">
                                 <div class="col-lg-6 col-md-6 col-sm-12" style="padding-left: 80px">
                                     <%--i18N Agricoltura Biologica da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_agricoltura_biologica" class="dettagli k-checkbox" />
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_agricoltura_biologica">Agricoltura Biologica</label>
                                 </div>
                                 <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px">
                                     <%--i18N Origine Non Agricola da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_origine_non_agri" class="dettagli k-checkbox"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_origine_non_agri">Origine Non Agricola</label>
                                </div>
                           </div>
                             <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12" style="padding-left: 80px">
                                    <%--i18N Agricoltura Convenzionale da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_agricoltura_convezionale" class="dettagli k-checkbox"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_agricoltura_convezionale" >Agricoltura Convenzionale</label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px">
                                     <%--i18N Ausiliare di Fabbricazione da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_ausi_fabbr" class="dettagli k-checkbox"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_ausi_fabbr" >Ausiliare di Fabbricazione</label>
                                </div>  
                          </div>
                         </div>
                          <!--Fine Dettagli-->

                          <div class="informazioni_aggiuntive col-lg-12 border_si">
                            <br/>
                            <!--Drop Down Unità Misura (parte GIAS LAN)-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group"> 
                                                <%--i18N Unità Misura Default da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_unita_mis_def">Unità Misura Default:</span>
                                                <input type="text" id='ddl_prodotto_UC_unita_mis_def' class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--TextBox Descrizione Addizionale (parte GIAS LAN)-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Descrizione Addizionale da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_descr_add">Descrizione Addizionale:</span>
                                                <input type="text" id="txt_prodotto_UC_descr_add" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                            </div>

                              <!--Drop Down Ditta di Provenienza-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="ditta_di_provenienza input-group ">
                                                <%--i18N Ditta di Provenienza da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_ditta_di_provenienza"> Ditta di Provenienza:</span>
                                                <input type="text" id="ddl_prodotto_UC_ditta_di_provenienza" class="form-control " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--TextArea Note-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <%--i18N Note da tradurre--%>
                                                    <label class="input-group-addon alert-info" id="lbl_prodotto_UC_Note" for="txt_prodotto_UC_Note">Note:</label>
                                                    <textarea id="txt_prodotto_UC_Note" class="form-control k-content" rows="3"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                </div>
                            </div>

                              <div class="row">
                                  <div class="col-md-8 col-sm-12">
                                      <div class="form-group">
                                          <div class="input-group">
                                              <label class="input-group-addon alert-info" id="lbl_prodotto_UC_mat_prima_priorita_cdg" for="ddl_prodotto_UC_mat_prima_priorita_cdg">Priorità prodotto per costi di gestione:</label>
                                              <select class="form-group" name="ddl_prodotto_UC_mat_prima_priorita_cdg" id="ddl_prodotto_UC_mat_prima_priorita_cdg"></select>
                                          </div>
                                      </div>
                                  </div>
                              </div>

                          </div>

                          <!--Inizio Visibilità-->
                         <div class="col-lg-12 border_si">
                             <div class="row">
                                <div class="col-md-12">
                                    <%--i18N Visibiltà da tradurre--%>
                                    <h4 style="color: #052747; text-transform: uppercase;">Visibilità</h4>
                                </div>
                            </div>
                            <!--Kendo Switch per Materia Prima/Lavorato-->
                            <div class="col-lg-3 col-md-3 col-sm-12" >
                                <%--i18N Materia prima/Lavorato movimentabile da tutte le imprese da tradurre--%>
                                <label class="lbl_required" id="lbl_prodotto_UC_materia_prima" for="lbl_prodotto_UC_materia_prima" >Materia prima/Lavorato movimentabile da tutte le imprese:</label>
                                <input type="checkbox" id="cb_prodotto_UC_materia_prima" name="cb_prodotto_UC_materia_prima" class="kendoSwitch"/>
                            </div>

                            <!--TextBox Impresa Referente-->
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Impresa Referente da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_impresa_ref">Impresa Referente:</span>
                                                <input type="text" id="txt_prodotto_UC_impresa_ref" class="form-control " disabled/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                          </div>
                           <!--Fine Visibilità-->
                       </div>                            
                     </div>

                        <div class="tab-pane Configurazione fade in" id="tab_prodotto_UC_configurazione" style="overflow: auto; margin-bottom: 70px;">
                            <div class="jumbotron">
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12" style="padding-bottom: 0px">
                                        <!--CheckBox Prodotto Confezionato / Impostazione Pesi (fa parte del GIAS LAN)-->
                                        <div class="input-group ">
                                            <%--i18N Prodotto Confezionato da tradurre--%>
                                            <input type="checkbox" id="chk_prodotto_UC_Udm_Cod_Extra" class="k-checkbox" onclick="prodotto_Edit_UC_Udm_Cod_Extra()"/>
                                            <label class="k-checkbox-label" for="chk_prodotto_UC_Udm_Cod_Extra">Prodotto Confezionato</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="configurazione_base col-lg-12 border_si">
                                    <br/>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <!--Dropdown tipo Default (fa parte del GIAS LAN)-->
                                            <div class="input-group ">
                                                <%--i18N Tipo Default da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_tipo_default"> Tipo Default:</span>
                                                <input type="text" id="ddl_prodotto_UC_tipo_default" class="form-control " />
                                            </div>
                                         </div>
                                      </div>
                                     <div class="row">
                                        <!--Dropdown Unita di Misura Aspetto Bene (fa parte del GIAS LAN)-->
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="input-group ">
                                                <%--i18N Udm Aspetto Bene da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_cod_extra"> Udm Aspetto Bene:</span>
                                                <input type="text" id="ddl_prodotto_UC_cod_extra" class="form-control " />
                                            </div>
                                         </div>
                                        <div class="col-lg-2 col-md-2 col-sm-12">
                                            &nbsp;
                                         </div>
                                         <!--Dropdown e TextBox Peso (fa parte del GIAS LAN)-->
                                         <div class="col-lg-4 col-md-4 col-sm-12">
                                            <div class="input-group ">
                                                <input type="text" id="ddl_prodotto_UC_peso" class="form-control "/>
                                                <span class="input-group-btn" style="width:0px;"></span>
                                                <input type="text" id="txt_prodotto_UC_Qta_Extra" class="form-control" />
                                            </div>                             
                                         </div>
                                      </div>
                                     <div class="row">
                                        <!--Dropdown Set (fa parte del GIAS LAN)-->
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="input-group ">
                                                 <%--i18N Set da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_set"> Set:</span>
                                                <input type="text" id="ddl_prodotto_UC_set" class="form-control " />
                                            </div>
                                         </div>
                                         <div class="col-lg-2 col-md-2 col-sm-12">
                                            &nbsp;
                                         </div>
                                         <!--TextBox tara Nominale (fa parte del GIAS LAN)-->
                                         <div class="col-lg-4 col-md-4 col-sm-12">
                                            <div class="input-group ">
                                                <%--i18N Tara Nominale Kg da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_tara"> Tara Nominale Kg:</span>
                                                <input type="text" id="txt_prodotto_UC_tara_nomi" class="form-control" />
                                            </div>
                                         </div>
                                      </div>
                                     <div class="row">
                                      <!--Dropdown Unita di Misura Base Gias FF (fa parte del GIAS LAN)-->
                                        <div class="Confezione_Base col-lg-6 col-md-6 col-sm-12">
                                            <div class="input-group ">
                                                <%--i18N Udm Base Gias FF da tradurre--%>
                                                <span class="input-group-addon alert-info" id="lbl_prodotto_UC_Confezione_Base"> Udm Base Gias FF:</span>
                                                <input type="text" id="ddl_prodotto_UC_Confezione_Base" class="form-control " />
                                            </div>
                                         </div>
                                      </div>

                                     <div class="row">
                                      <!--CheckBox e TextBox Beni Contenuti (fa parte del GIAS LAN)-->
                                        <div class="Qta_Contenitore_Conf col-lg-12 col-md-12 col-sm-12">
                                            <div class="Qta_Contenitore_Conf input-group ">
                                                <%--i18N Numero Beni Contenuti da tradurre--%>
                                                <input type="checkbox" id="chk_prodotto_UC_Qta_Contenitore_Conf" class="k-checkbox" onclick="prodotto_UC_Qta_Contenitore_Conf()"/>
                                                <label class="k-checkbox-label" for="chk_prodotto_UC_Qta_Contenitore_Conf">Numero Beni Contenuti:</label>
                                                <input type="text" id="txt_prodotto_UC_Qta_Contenitore_Conf" class="form-control" />
                                           </div>
                                         </div>
                                     </div>

                                     <br/>
                                     <br/>

                                    <!--CheckBox Imposta Aspetto come unita di misura principale (fa parte del GIAS LAN)-->
                                     <div class="row">
                                         <div class="col-lg-4 col-md-4 col-sm-12">
                                            <div class="input-group ">
                                                <%--i18N Imposta l'Unità di Misura 'Aspetto' come Principale da tradurre--%>
                                                <input type="checkbox" id="chk_prodotto_UC_imposta_flag_extra" class="k-checkbox" />
                                                <label class="k-checkbox-label" for="chk_prodotto_UC_imposta_flag_extra">Imposta 'UDM Aspetto Bene' come Principale</label>
                                            </div>
                                         </div>
                                      </div>
                               </div>
                            <br/><br/>
                            <!--CheckBox Imposta il Peso Variato come Principale (fa parte del GIAS LAN)-->
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px">
                                <div class="input-group ">
                                    <%--i18N Imposta il Peso Variato (Ricondotto) come Principale da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_imposta_flag_Variazione" class="k-checkbox" />
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_imposta_flag_Variazione">Imposta il Peso Variato (Ricondotto) come Principale</label>
                                </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px">
                                <div class="input-group " id ="checkbox_composizioneLotto">
                                    <%--i18N Utilizza per composizione lotto entrata da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_composizioneLotto" class="k-checkbox" onclick="Nascondi_Mostra_Filtro_Specie_Varieta()"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_composizioneLotto">Utilizza per composizione lotto entrata</label>
                                </div>
                                </div>
                            </div>
                                
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12" style="padding-top: 10px">
                                <div class="input-group " id ="checkbox_utilizzoCA">
                                    <%--i18N Utilizza per composizione lotto entrata da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_utilizzoCA" class="k-checkbox"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_utilizzoCA">Utilizzabile in contratti affitto</label>
                                </div>
                                </div>
                            </div>

                            <br/>
                          <!--Inzio Dettagli Bene Confezionamento-->
                          <div id="dettagli_beni_di_confezionamento" class="bene_conf col-lg-12 border_si">
                            <div class="row">
                                <div class="col-md-12">
                                    <%--i18N Dettagli Bene Confezionamento da tradurre--%>
                                    <h4 style="color: #052747; text-transform: uppercase;">Dettagli Bene Confezionamento</h4>
                                </div>
                            </div>
                            <div class="row">
                                 <!--CheckBox Imballagio , contenitore e confezione-->
                                 <div id="checkbox_imballaggio" class="col-lg-2 col-md-2 col-sm-12">
                                     <%--i18N Imballaggio da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_imballaggio" class=" k-checkbox" onclick="Nascondi_Mostra_Filtro_Specie_Varieta()"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_imballaggio">Imballaggio</label>
                                 </div>
                                 <div id="checkbox_contenitore" class="col-lg-2 col-md-2 col-sm-12">
                                     <%--i18N Contenitore da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_contenitore" class="k-checkbox" onclick="prodotto_Edit_UC_contenitore()" />
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_contenitore">Contenitore</label>
                                 </div>
                                 <div id="checkbox_confezione"class="col-lg-2 col-md-2 col-sm-12" style="display:none">
                                    <%--i18N Confezione da tradurre--%>
                                    <input type="checkbox" id="chk_prodotto_UC_confezione" class=" k-checkbox" onclick="Nascondi_Mostra_Filtro_Specie_Varieta()"/>
                                    <label class="k-checkbox-label" for="chk_prodotto_UC_confezione">Confezione</label>
                                 </div>
                           </div>
                            <br/>
                             <div class="row">
                                <!--TextBox Numero Beni Contenuti-->
                                <div class="Qta_Contenitore col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Numero Beni Contenuti da tradurre--%>
                                                <span class="input-group-addon alert-info" id="prodotto_UC_lblQta_Contenitore" >Numero Beni Contenuti</span>
                                                <input type="number" id="txt_prodotto_UC_Qta_Contenitore" class="form-control " min="0"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="row">
                                <!--Dropdown categoria-->
                                <div id="categoria_dettagli_beni_conf" class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <%--i18N Categoria da tradurre--%>
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblconfezioni" for="ddl_prodotto_UC_confezioni">Categoria:</label>
                                                    <input name="ddl_prodotto_UC_confezioni" id="ddl_prodotto_UC_confezioni" class="form-control" />
                                                </div>
                                            </div>                                      
                                    </div>
                                </div>
                            </div> 
                              <div id="filtri_dett_beni_conf_veg">
                                  <div class="row">
                                      <div class="col-md-12">
                                          <%--i18N Utilizzabile con  da tradurre--%>
                                          <h6 style="color: #052747; text-transform: uppercase;">Utilizzabile con:</h6>
                                      </div>
                                  </div>
                                  <div class="row">                                     
                                      <!--Multiselect specie per dettagli beni di confezionamento-->
                                      <div class="col-lg-6 col-md-6 col-sm-12">
                                          <div class="form-horizontal">
                                              <div class="form-group">
                                                  <div class="input-group">
                                                      <%--i18N Specie da tradurre--%>
                                                      <label class="input-group-addon alert-info" id="prodotto_UC_lblspecie_dett_beni">Specie Vegetale:</label>
                                                      <select name="multisel_prodotto_UC_specie_dett_beni" multiple="multiple" id="multisel_prodotto_UC_specie_dett_beni" class="form-control"></select>
                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                                  <div id="multisel_varieta_beni_conf_veg" class="row">
                                      <!--Multiselect varieta per dettagli beni di confezionamento-->
                                      <div class="col-lg-6 col-md-6 col-sm-12">
                                          <div class="form-horizontal">
                                              <div class="form-group">
                                                  <div class="input-group">
                                                      <%--i18N Varieta da tradurre--%>
                                                      <label class="input-group-addon alert-info" id="prodotto_UC_lblvarieta_dett_beni" for="multisel_prodotto_UC_varieta_dett_beni">Varietà:</label>
                                                      <select name="multisel_prodotto_UC_varieta_dett_beni" multiple="multiple" id="multisel_prodotto_UC_varieta_dett_beni" class="form-control"></select>
                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                         </div>
                          <!--Fine Dettagli Bene Confezionamento-->

                          <br/>
                          <br/>

                         <!--CheckBox escludi da gestione Preparazioni (fa parte del GIAS LAN)-->
                         <div class="col-lg-12 border_si border_no">
                          <div class="row">
                              <div class="col-lg-6 col-md-6 col-sm-12" style="display:none">
                                <div class="input-group ">
                                  <%--i18N Escludi da Gestione Preparazioni da tradurre--%>
                                  <input type="checkbox" id="chk_prodotto_UC_escludi_da_preparazioni" class="k-checkbox" />
                                  <label class="k-checkbox-label" for="chk_prodotto_UC_escludi_da_preparazioni">Escludi da Gestione Preparazioni</label>
                               </div>
                             </div>
                          <!--CheckBox escludi da movimentazioni magazzino (fa parte del GIAS LAN)-->
                             <div class="col-lg-6 col-md-6 col-sm-12 gias-p-x-30px gias-p-x-15-2023" >
                                <div class="input-group ">
                                  <%--i18N Escludi da Movimentazioni di Magazzino da tradurre--%>
                                  <input type="checkbox" id="chk_prodotto_UC_escludi_da_magazzino" class="k-checkbox" />
                                  <label class="k-checkbox-label" for="chk_prodotto_UC_escludi_da_magazzino">Escludi da Movimentazioni di Magazzino</label>
                               </div>
                             </div>
                         </div>
                        </div>
                        </div>
                       </div>

                        <div class="tab-pane StoricoPrezzi fade in" id="tab_prodotto_UC_storico_prezzi" style="overflow: auto; margin-bottom: 70px;">

                            <div class="jumbotron">
                                 <div class="row">
                                    <!-- Griglia Storico Prezzi -->
                                    <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                        <div id="prodotto_UC_griglia_storico_prezzi"></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="tab-pane ParametriQual fade in" id="tab_prodotto_UC_parametri_qualitativi" style="overflow: auto; margin-bottom: 70px;">

                           <div class="jumbotron">
                                <div class="row">
                                    <div class= "col-lg-6 col-md-6 col-sm-12">
                                    <!-- Griglia Calibri -->
                                        <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                        <div id="prodotto_UC_griglia_calibri"></div>
                                    </div>
                                </div>
                                <div class= "col-lg-6 col-md-6 col-sm-12">
                                    <!-- Griglia Indici -->
                                        <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                        <div id="prodotto_UC_griglia_indici"></div>
                                    </div>
                                </div>
                            </div>
                       </div>
                        </div>

                        <div class="tab-pane DatiContabili fade in" id="tab_prodotto_UC_dati_contabilita" style="overflow: auto; margin-bottom: 70px;">
                            <div class="jumbotron">
                                <div class="row">
                                    <!-- Dropdown IVA-->
                                    <div class="col-lg-6 col-md-6 col-sm-12 ">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <%--i18N Aliquota Iva da tradurre--%>
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblCodIva" for="prodotto_UC_ddlCodIva">Aliquota Iva:</label>
                                                    <input name="prodotto_UC_ddlCodIva" id="prodotto_UC_ddlCodIva" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                   <!-- Dropdown IVA in compensazione-->
                                   <div class="col-lg-6 col-md-6 col-sm-12 ">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Aliquota Iva in Compensazione da tradurre--%>
                                                <label class="input-group-addon alert-info" id="prodotto_UC_lblCodIvaCompensazione" for="prodotto_UC_ddlCodIva">Aliquota Iva in Compensazione:</label>
                                                <input name="prodotto_UC_ddlCodIvaCompensazione" id="prodotto_UC_ddlCodIvaCompensazione" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                  </div>
                                </div>
                                <div class="row">
                                    <!-- Dropdown Conto Economico Acquisto-->
                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <%--i18N Conto Economico Acquisto da tradurre--%>
                                                    <label class="input-group-addon alert-info" id="lblContoEconomico" for="ddlContoEconomico">Conto Economico Acquisto:</label>
                                                    <input name="prodotto_UC_ddlContoEconomicoAcquisto" id="prodotto_UC_ddlContoEconomicoAcquisto" class="form-control" />
                                                </div>
                                           </div>
                                       </div>
                                    </div>
                                  <!-- Dropdown Conto Economico Vendita-->
                                   <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Conto Economico Vendita da tradurre--%>
                                                <label class="input-group-addon alert-info" id="prodotto_UC_lblContoEconomicoVendita" for="prodotto_UC_ddlContoEconomicoVendita">Conto Economico Vendita:</label>
                                                <input name="prodotto_UC_ddlContoEconomicoVendita" id="prodotto_UC_ddlContoEconomicoVendita" class="form-control" />
                                           </div>
                                        </div>
                                    </div>
                                   </div>
                            </div>
                            <div class="row">
                                <!-- Dropdown Conto Patrimoniale Acquisto-->
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Conto Patrimoniale Acquisto da tradurre--%>
                                                <label class="input-group-addon alert-info" id="prodotto_UC_lblContoPatrimonialeAcquisto" for="prodotto_UC_ddlContoPatrimonialeAcquisto">Conto Patrimoniale Acquisto:</label>
                                                <input name="prodotto_UC_ddlContoPatrimonialeAcquisto" id="prodotto_UC_ddlContoPatrimonialeAcquisto" class="form-control" />
                                           </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Dropdown Conto Patrimoniale Vendita-->
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Conto Patrimoniale Vendita da tradurre--%>
                                                <label class="input-group-addon alert-info" id="prodotto_UC_lblContoPatrimonialeVendita" for="prodotto_UC_ddlPatrimonialeVendita">Conto Patrimoniale Vendita:</label>
                                                <input name="prodotto_UC_ddlPatrimonialeVendita" id="prodotto_UC_ddlContoPatrimonialeVendita" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <!-- Dropdown Gruppo Merce-->
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <%--i18N Gruppo Merce da tradurre--%>
                                                <label class="input-group-addon alert-info" id="prodotto_UC_lblGruppoMerce" for="prodotto_UC_ddlGruppoMerce">Gruppo Merce:</label>
                                                <input name="prodotto_UC_ddlGruppoMerce" id="prodotto_UC_ddlGruppoMerce" class="form-control" />
                                           </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                     </div>

                        <div class="tab-pane Traduzioni fade in" id="tab_prodotto_UC_traduzioni" style="overflow: auto; margin-bottom: 70px;">
                            <div class="jumbotron">
                                 <div class="row">
                                      <!-- Griglia Traduzioni -->
                                    <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                        <div id="prodotto_UC_griglia_traduzioni"></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                       <div class="tab-pane Altri_Dati fade in" id="tab_prodotto_UC_altri_dati" style="overflow: auto; margin-bottom: 70px;">
                            <div class="jumbotron">
                                 <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblEAN" for="txt_prodotto_UC_EAN">EAN/GTIN:</label>
                                                    <input type="text" id="txt_prodotto_UC_EAN" name="EAN/GTIN" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblBarcode" for="txt_prodotto_UC_Barcode">Barcode Interno:</label>
                                                    <input type="text" id="txt_prodotto_UC_Barcode" name="Barcode Interno" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                           <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblPesoNetto" for="txt_prodotto_UC_PesoNetto">Peso Netto:</label>
                                                    <input type="text" id="txt_prodotto_UC_PesoNetto" name="Peso Netto" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-md-4 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblPesoSgocciolato" for="txt_prodotto_UC_PesoSgocciolato">Peso Sgocciolato:</label>
                                                    <input type="text" id="txt_prodotto_UC_PesoSgocciolato" name="Peso Sgocciolato" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-md-4 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">                                                    
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblTara" for="txt_prodotto_UC_Tara">Tara:</label>
                                                    <input name="txt_prodotto_UC_Tara" id="txt_prodotto_UC_Tara" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                           </div>


                           <div class="row">
                                    <div id="produzione_propria" class="col-lg-6 col-md-6 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div  class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblProduzionePropria" for="prodotto_UC_ddlProduzionePropria">Produzione Propria:</label>
                                                    <input name="prodotto_UC_ddlProduzionePropria" id="prodotto_UC_ddlProduzionePropria" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="input-group">                                                    
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblPesoEgalizzato" for="prodotto_UC_ddlPesoEgalizzato">Peso Egalizzato:</label>
                                                    <input name="prodotto_UC_ddlPesoEgalizzato" id="prodotto_UC_ddlPesoEgalizzato" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                           </div>

                           <div class="row">
                                  <div id="ingredienti" class="col-lg-12 col-md-12 col-sm-12">
                                        <div class="form-horizontal">
				                            <div class="form-group">
					                            <div class="input-group">
                                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblIngredienti" for="txt_prodotto_UC_Ingredienti">Ingredienti:</label>
                                                    <textarea id="txt_prodotto_UC_Ingredienti" class="form-control k-content" style="resize: none;" rows="10"></textarea>
                                               </div>
                                            </div>
                                       </div>
                                </div>
                            </div>
                           <div class="row">
                               &nbsp;
                            <div class="col-lg-12 col-md-12 col-sm-12" id="id_prodotto_UC_rimuovi_immagine">
                                <div class="btn btn-success col-lg-12 col-md-12 col-sm-12" id="btn_prodotto_UC_rimuovi_immagine">
                                    <span class="fa fa-ban"></span><span class="lampeggiante">Rimuovi Immagine</span>
                                </div>
                            </div>
                           </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <label class="input-group-addon alert-info" id="prodotto_UC_lblImage" for="prodotto_UC_imageEditor">Immagine:</label>
                                    <div id="prodotto_UC_imageEditor"></div>
                                 </div>
                            </div>
                                
                        </div>
                    </div>


                       <div class="tab-pane Altri_Dati fade in" id="tab_prodotto_UC_alias" style="overflow: auto; margin-bottom: 70px;">
                           <div class="jumbotron">
                            <div class="row">
                                <!-- Griglia Storico Prezzi -->
                                <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                    <div id="prodotto_UC_griglia_alias"></div>
                                </div>
                            </div>
                           </div>
                       </div>

                 </div>
               </div>
            </div>
        </div>
    </div>  

    <div class="panel-group prodotto_Edit_UC_gridArea" style="display: none;">
        <!--Griglia-->
        <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
            <input type="hidden" id="chiave_prodotti" />
            <input type="hidden" id="hdKendoProdotto_Valorizzazione" />
            <div id="divKendoProdotto"></div>
        </div>
    </div>
    <!-- fine container -->

<input type="hidden" id="hf_Piva" runat="server" />

<script src="../Scripts/footable.min.js?<% =Application("GiasVersioneCorrente")%>" type="text/javascript"></script>

<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/Anagrafica/UserControl/Prodotto_Edit_UC.js")) %>"></script>
<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/Anagrafica/UserControl/Prodotto_Edit_UC_globali.js")) %>"></script>
<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/Anagrafica/UserControl/Prodotto_Edit_UC_ws_client.js")) %>"></script>
<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/Anagrafica/UserControl/Prodotto_Edit_UC_jQueryDocReady.js")) %>"></script>
<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/funzioni_comuni.js")) %>"></script>
<script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/leggitabelle_ws_client.js")) %>"></script>

<script>
    var cIdPiva = "#<%=hf_Piva.ClientID %>";
</script>
