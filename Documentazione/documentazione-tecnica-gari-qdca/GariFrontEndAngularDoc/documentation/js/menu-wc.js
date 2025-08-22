'use strict';

customElements.define('compodoc-menu', class extends HTMLElement {
    constructor() {
        super();
        this.isNormalMode = this.getAttribute('mode') === 'normal';
    }

    connectedCallback() {
        this.render(this.isNormalMode);
    }

    render(isNormalMode) {
        let tp = lithtml.html(`
        <nav>
            <ul class="list">
                <li class="title">
                    <a href="index.html" data-type="index-link">GiasNG documentation</a>
                </li>

                <li class="divider"></li>
                ${ isNormalMode ? `<div id="book-search-input" role="search"><input type="text" placeholder="Type to search"></div>` : '' }
                <li class="chapter">
                    <a data-type="chapter-link" href="index.html"><span class="icon ion-ios-home"></span>Getting started</a>
                    <ul class="links">
                        <li class="link">
                            <a href="overview.html" data-type="chapter-link">
                                <span class="icon ion-ios-keypad"></span>Overview
                            </a>
                        </li>
                        <li class="link">
                            <a href="index.html" data-type="chapter-link">
                                <span class="icon ion-ios-paper"></span>README
                            </a>
                        </li>
                                <li class="link">
                                    <a href="dependencies.html" data-type="chapter-link">
                                        <span class="icon ion-ios-list"></span>Dependencies
                                    </a>
                                </li>
                                <li class="link">
                                    <a href="properties.html" data-type="chapter-link">
                                        <span class="icon ion-ios-apps"></span>Properties
                                    </a>
                                </li>
                    </ul>
                </li>
                    <li class="chapter modules">
                        <a data-type="chapter-link" href="modules.html">
                            <div class="menu-toggler linked" data-bs-toggle="collapse" ${ isNormalMode ?
                                'data-bs-target="#modules-links"' : 'data-bs-target="#xs-modules-links"' }>
                                <span class="icon ion-ios-archive"></span>
                                <span class="link-name">Modules</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                        </a>
                        <ul class="links collapse " ${ isNormalMode ? 'id="modules-links"' : 'id="xs-modules-links"' }>
                            <li class="link">
                                <a href="modules/AmministrazioneSistemaModule.html" data-type="entity-link" >AmministrazioneSistemaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' : 'data-bs-target="#xs-components-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' :
                                            'id="xs-components-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' }>
                                            <li class="link">
                                                <a href="components/ConsultaSincroDatiAppComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConsultaSincroDatiAppComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' : 'data-bs-target="#xs-injectables-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' :
                                        'id="xs-injectables-links-module-AmministrazioneSistemaModule-6e640bac13ea8cef2ea399f133d85b8356aa21198b32016930f5ecb8cf0e597341a1bb50b584dc9a90440ea0190e800fe1198d60520d5e4fa399c0d54fbc7926"' }>
                                        <li class="link">
                                            <a href="injectables/ConsultaSincroLogGridService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConsultaSincroLogGridService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/AmministrazionSistemaRoutingModule.html" data-type="entity-link" >AmministrazionSistemaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/AnagraficaModule.html" data-type="entity-link" >AnagraficaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' : 'data-bs-target="#xs-components-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' :
                                            'id="xs-components-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' }>
                                            <li class="link">
                                                <a href="components/AgriculturalExerciceContributeLinkComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AgriculturalExerciceContributeLinkComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AgriculturalPlotMachineLinkComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AgriculturalPlotMachineLinkComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AnagraficaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnagraficaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppezzamentiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppezzamentiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AxpMessageContentComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AxpMessageContentComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CampiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CampiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CampiShortDescriptionComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CampiShortDescriptionComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ContattiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ContattiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/EserciziComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EserciziComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/EserciziCopiaSpostaAppezzamentiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EserciziCopiaSpostaAppezzamentiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FabbricatiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FabbricatiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImgBase64Component.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImgBase64Component</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpiantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpiantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpiantoShortDescriptionComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpiantoShortDescriptionComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpreseComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpreseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/InvestimentoCatastaleAppezzamentoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InvestimentoCatastaleAppezzamentoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/InvestimentoCatastaleCampoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InvestimentoCatastaleCampoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/InvestimentoCatastaleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InvestimentoCatastaleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchinaShortDescriptionComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchinaShortDescriptionComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchineComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchineComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/NuovaOperazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >NuovaOperazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OnCloseEsercizioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OnCloseEsercizioComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SelezionaCentroComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SelezionaCentroComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' : 'data-bs-target="#xs-injectables-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' :
                                        'id="xs-injectables-links-module-AnagraficaModule-ac275a378582cb6e9435cd723b3fd3ca9dcbcfb85c22e14a8f1f38d796061a290cf7d56690e7a7c6f260ac3c5187bc68476f189c30135f126c3289da853e818f"' }>
                                        <li class="link">
                                            <a href="injectables/AgriculturalPlotsMachinesLinkService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AgriculturalPlotsMachinesLinkService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisToolbarService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisToolbarService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MeasureDistanceService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MeasureDistanceService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PositionService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PositionService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/AnagraficaRoutingModule.html" data-type="entity-link" >AnagraficaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/AnalisiTerrenoModule.html" data-type="entity-link" >AnalisiTerrenoModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' : 'data-bs-target="#xs-components-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' :
                                            'id="xs-components-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' }>
                                            <li class="link">
                                                <a href="components/AnalisiTerrenoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnalisiTerrenoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AnalisiTerrenoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnalisiTerrenoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GrigliaAnalisiTerrenoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GrigliaAnalisiTerrenoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GrigliaEntitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GrigliaEntitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ParametriAnalisiTerrenoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ParametriAnalisiTerrenoComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' : 'data-bs-target="#xs-injectables-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' :
                                        'id="xs-injectables-links-module-AnalisiTerrenoModule-a3a127e39f92955b106edc7b3cca9839d66e13df26cfbd3759adec42fb57103dac18f7798e95620f5cba9b0dd44b7b25065ee8398c5325fe4f42f55a048a98ae"' }>
                                        <li class="link">
                                            <a href="injectables/AnalisiDocumentsService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnalisiDocumentsService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/AnalisiTerrenoFormService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnalisiTerrenoFormService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/BreadcrumbsService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BreadcrumbsService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EntitaConfigService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EntitaConfigService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisToolbarService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisToolbarService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MeasureDistanceService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MeasureDistanceService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PositionService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PositionService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/AnalisiTerrenoRoutingModule.html" data-type="entity-link" >AnalisiTerrenoRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/AppezzamentoModule.html" data-type="entity-link" >AppezzamentoModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' : 'data-bs-target="#xs-components-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' :
                                            'id="xs-components-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' }>
                                            <li class="link">
                                                <a href="components/AppezzamentoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppezzamentoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppezzamentoGlobalEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppezzamentoGlobalEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DatiCatastaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiCatastaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/EsercizioEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EsercizioEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpiantoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpiantoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IndirizziComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndirizziComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SalvataggioAppezzamentoGISComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SalvataggioAppezzamentoGISComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UtilizzoTerrenoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UtilizzoTerrenoComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' : 'data-bs-target="#xs-injectables-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' :
                                        'id="xs-injectables-links-module-AppezzamentoModule-4ca0fcf371586151e3e2dcd0ae0f914f457246ad7dc4f14d74c26b984cab6dc57bdd51b3021ad0d0e4792cfb958ed628637f1a00422c2f8dedfa4f0b607750a9"' }>
                                        <li class="link">
                                            <a href="injectables/DatiCatastaliService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiCatastaliService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/DestinazioneUsoService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DestinazioneUsoService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/FunzioniComuniService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FunzioniComuniService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GruppoFinalitaService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppoFinalitaService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GruppoVarietaleService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppoVarietaleService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/SpecieVegetaliService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SpecieVegetaliService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/VarietaService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VarietaService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/AppModule.html" data-type="entity-link" >AppModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' : 'data-bs-target="#xs-components-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' :
                                            'id="xs-components-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                            <li class="link">
                                                <a href="components/AccessoNegatoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AccessoNegatoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BreadcrumbComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BreadcrumbComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CounterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CounterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DialogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DialogComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FetchDataComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FetchDataComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FooterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FooterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HomeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HomeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpreseFilterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpreseFilterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LoginComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LoginComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuContestualeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuContestualeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PercentGridViewerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PercentGridViewerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PreferitiConfigComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PreferitiConfigComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PreferitoCardComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PreferitoCardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SideMenuComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SideMenuComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SideMenuFooterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SideMenuFooterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SideMenuServicesComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SideMenuServicesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SidemenuChildComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SidemenuChildComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaCampiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaCampiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaCatastoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaCatastoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaCentriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaCentriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaImpiantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaImpiantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaImpreseComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaImpreseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestAnagraficaMacchineComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestAnagraficaMacchineComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WindowMessaggioErroreComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WindowMessaggioErroreComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#directives-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' : 'data-bs-target="#xs-directives-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' :
                                        'id="xs-directives-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                        <li class="link">
                                            <a href="directives/BackButtonDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BackButtonDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/ClearInputButtonDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ClearInputButtonDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/HomeButtonDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HomeButtonDirective</a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' : 'data-bs-target="#xs-injectables-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' :
                                        'id="xs-injectables-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                        <li class="link">
                                            <a href="injectables/MasterService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MasterService</a>
                                        </li>
                                    </ul>
                                </li>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#pipes-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' : 'data-bs-target="#xs-pipes-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                            <span class="icon ion-md-add"></span>
                                            <span>Pipes</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="pipes-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' :
                                            'id="xs-pipes-links-module-AppModule-7731b6d8113d1f046a2d96c437ba8016db2dac072e345ba8c75e5ce0c982031c69d569d364e3125c041c27b5ed9d3d2dc76b6bd9f39648f6e1adfd3812df35d5"' }>
                                            <li class="link">
                                                <a href="pipes/FilterLinkMenuChildrenPipe.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FilterLinkMenuChildrenPipe</a>
                                            </li>
                                            <li class="link">
                                                <a href="pipes/FilterPipe.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FilterPipe</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/AppRoutingModule.html" data-type="entity-link" >AppRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/AppServerModule.html" data-type="entity-link" >AppServerModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AppServerModule-f44bbb75130ea39ba6e66f0a03ac09016142ae81c8a6583e5c4476c20f18f5540a2f001e8123a181aa355e5ab7d2678fda6457d45048cade4f112fb8ecd26565"' : 'data-bs-target="#xs-components-links-module-AppServerModule-f44bbb75130ea39ba6e66f0a03ac09016142ae81c8a6583e5c4476c20f18f5540a2f001e8123a181aa355e5ab7d2678fda6457d45048cade4f112fb8ecd26565"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppServerModule-f44bbb75130ea39ba6e66f0a03ac09016142ae81c8a6583e5c4476c20f18f5540a2f001e8123a181aa355e5ab7d2678fda6457d45048cade4f112fb8ecd26565"' :
                                            'id="xs-components-links-module-AppServerModule-f44bbb75130ea39ba6e66f0a03ac09016142ae81c8a6583e5c4476c20f18f5540a2f001e8123a181aa355e5ab7d2678fda6457d45048cade4f112fb8ecd26565"' }>
                                            <li class="link">
                                                <a href="components/AppComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/BudgetModule.html" data-type="entity-link" >BudgetModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' : 'data-bs-target="#xs-components-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' :
                                            'id="xs-components-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' }>
                                            <li class="link">
                                                <a href="components/BudgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BudgetComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' : 'data-bs-target="#xs-injectables-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' :
                                        'id="xs-injectables-links-module-BudgetModule-b932bd0e46ae0bd503a56d9bc2c2663ab70d7b529430387ca60e7f93789e6ede77caee4a490df9fadfd9c9f2ae950e5f52d5a09757f1f779c799c7e3e5e7b786"' }>
                                        <li class="link">
                                            <a href="injectables/BudgetLoadedGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BudgetLoadedGuard</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/BudgetRoutingModule.html" data-type="entity-link" >BudgetRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/CampoModule.html" data-type="entity-link" >CampoModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-CampoModule-efc37d067cff8b7e582f529ae01ef63b96e36d9a779afde031b2173615f075b897b7b323ee16a0f3269ee971a45534fb4112e6f86af538fde66442ee4376a614"' : 'data-bs-target="#xs-components-links-module-CampoModule-efc37d067cff8b7e582f529ae01ef63b96e36d9a779afde031b2173615f075b897b7b323ee16a0f3269ee971a45534fb4112e6f86af538fde66442ee4376a614"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-CampoModule-efc37d067cff8b7e582f529ae01ef63b96e36d9a779afde031b2173615f075b897b7b323ee16a0f3269ee971a45534fb4112e6f86af538fde66442ee4376a614"' :
                                            'id="xs-components-links-module-CampoModule-efc37d067cff8b7e582f529ae01ef63b96e36d9a779afde031b2173615f075b897b7b323ee16a0f3269ee971a45534fb4112e6f86af538fde66442ee4376a614"' }>
                                            <li class="link">
                                                <a href="components/AppezzamentiCampoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppezzamentiCampoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CampiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CampiEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoCampoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoCampoEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/CatastoModule.html" data-type="entity-link" >CatastoModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' : 'data-bs-target="#xs-components-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' :
                                            'id="xs-components-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' }>
                                            <li class="link">
                                                <a href="components/CatastoClassamentoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoClassamentoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoMacrousiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoMacrousiEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoMetodoProduzioneEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoMetodoProduzioneEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoPossessiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoPossessiEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CatastoZoneEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CatastoZoneEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' : 'data-bs-target="#xs-injectables-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' :
                                        'id="xs-injectables-links-module-CatastoModule-3022bc3f588d22b233e81314294ef128ec91b1e7ec5620a5a9eb3e06df323bbbf6f66ba83d80f98d40775df72f689aadbe6fa3c3604573abef999b165c027aad"' }>
                                        <li class="link">
                                            <a href="injectables/ClassamentoCatastoService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ClassamentoCatastoService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MacrousiCatastoService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacrousiCatastoService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MetodoProduzioneService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MetodoProduzioneService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PossessiService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PossessiService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/CentriModule.html" data-type="entity-link" >CentriModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-CentriModule-1f0d52a9dce2c4cdcea8e51304b1daf6e4cabba94a68f907832732a340d0a6b6f59d6201c2b6af51ad552a939c6ebee8cee709fd683cdc68b10889572d25b3bf"' : 'data-bs-target="#xs-components-links-module-CentriModule-1f0d52a9dce2c4cdcea8e51304b1daf6e4cabba94a68f907832732a340d0a6b6f59d6201c2b6af51ad552a939c6ebee8cee709fd683cdc68b10889572d25b3bf"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-CentriModule-1f0d52a9dce2c4cdcea8e51304b1daf6e4cabba94a68f907832732a340d0a6b6f59d6201c2b6af51ad552a939c6ebee8cee709fd683cdc68b10889572d25b3bf"' :
                                            'id="xs-components-links-module-CentriModule-1f0d52a9dce2c4cdcea8e51304b1daf6e4cabba94a68f907832732a340d0a6b6f59d6201c2b6af51ad552a939c6ebee8cee709fd683cdc68b10889572d25b3bf"' }>
                                            <li class="link">
                                                <a href="components/BiologicoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BiologicoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CentriCodiciGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CentriCodiciGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CentriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CentriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CentroEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CentroEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DatiAccessoriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiAccessoriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DatiCentroComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiCentroComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ConfrontoPianoColturaleModule.html" data-type="entity-link" >ConfrontoPianoColturaleModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ConfrontoPianoColturaleModule-85814c4f09727dc7985fe36d38b125398dee70aabaa35cc34006de2c41760966e82fda276f80f0d45593a39cf5b12a80988bd199a0cbdefeb6283226aab48b87"' : 'data-bs-target="#xs-components-links-module-ConfrontoPianoColturaleModule-85814c4f09727dc7985fe36d38b125398dee70aabaa35cc34006de2c41760966e82fda276f80f0d45593a39cf5b12a80988bd199a0cbdefeb6283226aab48b87"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ConfrontoPianoColturaleModule-85814c4f09727dc7985fe36d38b125398dee70aabaa35cc34006de2c41760966e82fda276f80f0d45593a39cf5b12a80988bd199a0cbdefeb6283226aab48b87"' :
                                            'id="xs-components-links-module-ConfrontoPianoColturaleModule-85814c4f09727dc7985fe36d38b125398dee70aabaa35cc34006de2c41760966e82fda276f80f0d45593a39cf5b12a80988bd199a0cbdefeb6283226aab48b87"' }>
                                            <li class="link">
                                                <a href="components/ConfrontoPcCatastoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConfrontoPcCatastoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ConfrontoPcCatastoGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConfrontoPcCatastoGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ConfrontoPianoColturaleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConfrontoPianoColturaleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PianoColturaleGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PianoColturaleGridComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ConfrontoPianoColturaleRoutingModule.html" data-type="entity-link" >ConfrontoPianoColturaleRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ContattiModule.html" data-type="entity-link" >ContattiModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ContattiModule-31fd6a695f5601227866b1fd9815a7d79d71672718cde8dfd2a0afd87c75d0fa15e04e8db1f47dc276b2d0f94b1983f8d4b0523027908b0397f74905ed822d51"' : 'data-bs-target="#xs-components-links-module-ContattiModule-31fd6a695f5601227866b1fd9815a7d79d71672718cde8dfd2a0afd87c75d0fa15e04e8db1f47dc276b2d0f94b1983f8d4b0523027908b0397f74905ed822d51"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ContattiModule-31fd6a695f5601227866b1fd9815a7d79d71672718cde8dfd2a0afd87c75d0fa15e04e8db1f47dc276b2d0f94b1983f8d4b0523027908b0397f74905ed822d51"' :
                                            'id="xs-components-links-module-ContattiModule-31fd6a695f5601227866b1fd9815a7d79d71672718cde8dfd2a0afd87c75d0fa15e04e8db1f47dc276b2d0f94b1983f8d4b0523027908b0397f74905ed822d51"' }>
                                            <li class="link">
                                                <a href="components/ContattiAssociaUtenteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ContattiAssociaUtenteComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/DashboardModule.html" data-type="entity-link" >DashboardModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' : 'data-bs-target="#xs-components-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' :
                                            'id="xs-components-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' }>
                                            <li class="link">
                                                <a href="components/AlertDocumentiWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AlertDocumentiWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AziendeMultiPieWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AziendeMultiPieWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ChartWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ChartWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/Co2ComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >Co2ComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ColtureWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ColtureWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ColumnsChartWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ColumnsChartWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ConformitaWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConformitaWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CostiRicaviAIWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CostiRicaviAIWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DashboardComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DashboardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DatiGeneraliFarmersComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiGeneraliFarmersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ErosioneComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ErosioneComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GhgColtureWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GhgColtureWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IndicatoriWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndicatoriWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IndicatoriWidgetDashboardComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndicatoriWidgetDashboardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IndicatoriWidgetFullComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndicatoriWidgetFullComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IndicatoriWidgetGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndicatoriWidgetGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MappaMultiAziendaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MappaMultiAziendaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MeteoWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MeteoWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MeteoWidgetFullComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MeteoWidgetFullComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MonitoraggioWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MonitoraggioWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OperazioniTemplateWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OperazioniTemplateWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PaeseMultiAziendaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PaeseMultiAziendaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PlvComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PlvComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProduttivitaComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProduttivitaComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProduzioneColtureWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProduzioneColtureWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RiepilogoMeteoWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RiepilogoMeteoWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoAggregatoComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoAggregatoComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoAllagamentoComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoAllagamentoComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoGelataComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoGelataComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoGrandineComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoGrandineComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoSiccitaComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoSiccitaComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RischioMeteoVentoForteComparisonWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RischioMeteoVentoForteComparisonWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SeminatiMultiAziendaWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SeminatiMultiAziendaWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/StimeProduzioneColtureWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >StimeProduzioneColtureWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TargetSuperficieWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TargetSuperficieWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UltimeAttivitaWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UltimeAttivitaWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UltimeVisiteWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UltimeVisiteWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UltimiAcquistiWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UltimiAcquistiWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UltimiProdottiWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UltimiProdottiWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UltimiRilieviWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UltimiRilieviWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UniqueRatingWidgetComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UniqueRatingWidgetComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WidgetConfigComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WidgetConfigComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WidgetGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WidgetGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WidgetResizerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WidgetResizerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WidgetTemplateComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WidgetTemplateComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WidgetsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WidgetsComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' : 'data-bs-target="#xs-injectables-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' :
                                        'id="xs-injectables-links-module-DashboardModule-08fe0e096c4c4817fef8a2cafb9772709c22ed75d79d21e4f2252799ba7986f1f220a4b44a1a101e8d64696a323f4cc200361414bdb86cadb41348acc97bf6ea"' }>
                                        <li class="link">
                                            <a href="injectables/ComparisonWidgetService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ComparisonWidgetService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/IndicatoriWidgetService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >IndicatoriWidgetService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MonitoraggioWidgetService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MonitoraggioWidgetService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/RiepilogoMeteoWidgetService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RiepilogoMeteoWidgetService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/DashboardRoutingModule.html" data-type="entity-link" >DashboardRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/DatiPrevisionaliColtureModule.html" data-type="entity-link" >DatiPrevisionaliColtureModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-DatiPrevisionaliColtureModule-1f161774bf9f2f2c17ac679e26610f8fc8c76525112c2b8be4950101f1fb10ef4369d487e51e52e31e46521b0a38748cbc30eeec240291ce3cfcce3790c20936"' : 'data-bs-target="#xs-components-links-module-DatiPrevisionaliColtureModule-1f161774bf9f2f2c17ac679e26610f8fc8c76525112c2b8be4950101f1fb10ef4369d487e51e52e31e46521b0a38748cbc30eeec240291ce3cfcce3790c20936"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-DatiPrevisionaliColtureModule-1f161774bf9f2f2c17ac679e26610f8fc8c76525112c2b8be4950101f1fb10ef4369d487e51e52e31e46521b0a38748cbc30eeec240291ce3cfcce3790c20936"' :
                                            'id="xs-components-links-module-DatiPrevisionaliColtureModule-1f161774bf9f2f2c17ac679e26610f8fc8c76525112c2b8be4950101f1fb10ef4369d487e51e52e31e46521b0a38748cbc30eeec240291ce3cfcce3790c20936"' }>
                                            <li class="link">
                                                <a href="components/DatiPrevisionailiColtureGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiPrevisionailiColtureGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DatiPrevisionaliColtureComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DatiPrevisionaliColtureComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/DatiPrevisionaliColtureRoutingModule.html" data-type="entity-link" >DatiPrevisionaliColtureRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/DomandaIrriguaModule.html" data-type="entity-link" >DomandaIrriguaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-DomandaIrriguaModule-ed8bad5037be06f3bce3c74767a6e4a6ae4ecb810feccc22340ed0c52f00d0b4e063bc7d8f6df031ce6c1c7fb50e46a59b8aa703700b3ca90a6355df7cee1466"' : 'data-bs-target="#xs-components-links-module-DomandaIrriguaModule-ed8bad5037be06f3bce3c74767a6e4a6ae4ecb810feccc22340ed0c52f00d0b4e063bc7d8f6df031ce6c1c7fb50e46a59b8aa703700b3ca90a6355df7cee1466"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-DomandaIrriguaModule-ed8bad5037be06f3bce3c74767a6e4a6ae4ecb810feccc22340ed0c52f00d0b4e063bc7d8f6df031ce6c1c7fb50e46a59b8aa703700b3ca90a6355df7cee1466"' :
                                            'id="xs-components-links-module-DomandaIrriguaModule-ed8bad5037be06f3bce3c74767a6e4a6ae4ecb810feccc22340ed0c52f00d0b4e063bc7d8f6df031ce6c1c7fb50e46a59b8aa703700b3ca90a6355df7cee1466"' }>
                                            <li class="link">
                                                <a href="components/DomandaIrriguaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DomandaIrriguaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LettureContatoriAziendaliGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LettureContatoriAziendaliGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LettureContatoriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LettureContatoriComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/DomandaIrriguaRoutingModule.html" data-type="entity-link" >DomandaIrriguaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ExportQdCToAgeaModule.html" data-type="entity-link" >ExportQdCToAgeaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ExportQdCToAgeaModule-2511b5af78b1a17e8f9468d78f49118740e980a75fbd82427785c96f38cca6dcb824c5da5df0e716e2e7b2b91c3471bb664e444aba7d9ce77ff582e3663fa132"' : 'data-bs-target="#xs-components-links-module-ExportQdCToAgeaModule-2511b5af78b1a17e8f9468d78f49118740e980a75fbd82427785c96f38cca6dcb824c5da5df0e716e2e7b2b91c3471bb664e444aba7d9ce77ff582e3663fa132"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ExportQdCToAgeaModule-2511b5af78b1a17e8f9468d78f49118740e980a75fbd82427785c96f38cca6dcb824c5da5df0e716e2e7b2b91c3471bb664e444aba7d9ce77ff582e3663fa132"' :
                                            'id="xs-components-links-module-ExportQdCToAgeaModule-2511b5af78b1a17e8f9468d78f49118740e980a75fbd82427785c96f38cca6dcb824c5da5df0e716e2e7b2b91c3471bb664e444aba7d9ce77ff582e3663fa132"' }>
                                            <li class="link">
                                                <a href="components/ExportQdCToAgeaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ExportQdCToAgeaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ExportQdcToAgeaFarmFilterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ExportQdcToAgeaFarmFilterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ExportQdcToAgeaJsonDialogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ExportQdcToAgeaJsonDialogComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/FabbricatiModule.html" data-type="entity-link" >FabbricatiModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-FabbricatiModule-ba6e24db31edc20543cbd5cd9abc46302dff705063bac98745de8beae52279ed7f58de82e40aadff28c70d9109c7092356916be441c95db10b567f1ea1038b53"' : 'data-bs-target="#xs-components-links-module-FabbricatiModule-ba6e24db31edc20543cbd5cd9abc46302dff705063bac98745de8beae52279ed7f58de82e40aadff28c70d9109c7092356916be441c95db10b567f1ea1038b53"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-FabbricatiModule-ba6e24db31edc20543cbd5cd9abc46302dff705063bac98745de8beae52279ed7f58de82e40aadff28c70d9109c7092356916be441c95db10b567f1ea1038b53"' :
                                            'id="xs-components-links-module-FabbricatiModule-ba6e24db31edc20543cbd5cd9abc46302dff705063bac98745de8beae52279ed7f58de82e40aadff28c70d9109c7092356916be441c95db10b567f1ea1038b53"' }>
                                            <li class="link">
                                                <a href="components/FabbricatiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FabbricatiEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/FiltroRicercaModule.html" data-type="entity-link" >FiltroRicercaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' : 'data-bs-target="#xs-components-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' :
                                            'id="xs-components-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' }>
                                            <li class="link">
                                                <a href="components/ClearButtonComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ClearButtonComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CodiciComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CodiciComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FiltriTemporaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltriTemporaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FiltroRicercaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltroRicercaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GeoFiltersComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GeoFiltersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GrigliaFiltroRicercaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GrigliaFiltroRicercaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PrintExportComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PrintExportComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RedSpotComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RedSpotComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TreeAziende.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeAziende</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TreeAziendeFilters.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeAziendeFilters</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' : 'data-bs-target="#xs-injectables-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' :
                                        'id="xs-injectables-links-module-FiltroRicercaModule-bb1e71a391388e1566223127ec4f780a5b8c2324a4cf89c9a79548e5c61040c16ef5e4cf353b6f606b7013a9c144eb6a5ae5b99babfd15ac53653df90acf82e5"' }>
                                        <li class="link">
                                            <a href="injectables/DialogWindowService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DialogWindowService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PendingChangesGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PendingChangesGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/StampeFiltroRicercaService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >StampeFiltroRicercaService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/TreeAziendeService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeAziendeService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/FiltroRicercaRoutingModule.html" data-type="entity-link" >FiltroRicercaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/GISModule.html" data-type="entity-link" >GISModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' : 'data-bs-target="#xs-components-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' :
                                            'id="xs-components-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                            <li class="link">
                                                <a href="components/DrawWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DrawWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAlgorithmResultComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAlgorithmResultComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariAnimationComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariAnimationComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariCalendarioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariCalendarioComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariGraficiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariGraficiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariNotificheComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariNotificheComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariOpzioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariOpzioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAnalisiMappeSatellitariWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAnalisiMappeSatellitariWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiAlgorithmConfigurationComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiAlgorithmConfigurationComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiExportComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiExportComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiFileUploadCatastoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiFileUploadCatastoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiFileUploadComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiFileUploadComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiFileUploadRasterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiFileUploadRasterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiFileUploadStandardComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiFileUploadStandardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiMuzGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiMuzGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiRasterMasksComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiRasterMasksComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiRasterMasksGroupsPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiRasterMasksGroupsPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiRasterMasksPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiRasterMasksPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiRasterMasksUsersPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiRasterMasksUsersPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISAttributiSettingsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISAttributiSettingsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISBookmarksWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISBookmarksWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCalendarModalComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCalendarModalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProiezioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProiezioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProiezioniConfigComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProiezioniConfigComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProiezioniDialogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProiezioniDialogComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProiezioniGroupsPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProiezioniGroupsPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProiezioniUsersPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProiezioniUsersPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProziezioniLayerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProziezioniLayerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISCfgProziezioniPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISCfgProziezioniPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISConfigurationModalComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISConfigurationModalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISDeleteFeatureWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISDeleteFeatureWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISEditMuzComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISEditMuzComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISEditMuzParticelleCatastaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISEditMuzParticelleCatastaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISFixedLayerPropertyWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISFixedLayerPropertyWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISGestionePianoRateoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISGestionePianoRateoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISGestionePianoRateoLoaderComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISGestionePianoRateoLoaderComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISGestionePianoRateoPickerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISGestionePianoRateoPickerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISInitMuzGroupComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISInitMuzGroupComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerAdvancedSettingsWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerAdvancedSettingsWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerColorPickerWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerColorPickerWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerGroupsPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerGroupsPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerListItemComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerListItemComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerListItemMappeSatellitariComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerListItemMappeSatellitariComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerListItemRasterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerListItemRasterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerPermissionsWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerPermissionsWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerUsersPermissionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerUsersPermissionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerVisibilityConfigurationWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerVisibilityConfigurationWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISLayerWindowToolbarComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISLayerWindowToolbarComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISParticelleCatastaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISParticelleCatastaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISRasterConfigurationWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISRasterConfigurationWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GISWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisAlgorithmConfigurationComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisAlgorithmConfigurationComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisAlgorithmConfigurationLogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisAlgorithmConfigurationLogComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisAlgorithmConfigurationWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisAlgorithmConfigurationWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisBottomWindow.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisBottomWindow</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisCheckboxComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisCheckboxComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisToolbarComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisToolbarComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisToolbarMobileComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisToolbarMobileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GoogleMapComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LineeGuidaABWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LineeGuidaABWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MarkerWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MarkerWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PolygonMergeWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonMergeWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PolygonWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ThemeWindowBandSliderComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ThemeWindowBandSliderComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ThemeWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ThemeWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ThemeWindowSettingsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ThemeWindowSettingsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToolDisegnaPuntiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ToolDisegnaPuntiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToolDisegnoAvanzatoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ToolDisegnoAvanzatoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToolPolygonMergeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ToolPolygonMergeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ToolScomponiPuntiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ToolScomponiPuntiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TreeWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeWindowComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WMSWindowComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WMSWindowComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#directives-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' : 'data-bs-target="#xs-directives-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' :
                                        'id="xs-directives-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                        <li class="link">
                                            <a href="directives/AfterValueChangedDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AfterValueChangedDirective</a>
                                        </li>
                                        <li class="link">
                                            <a href="directives/GISTooltipDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GISTooltipDirective</a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' : 'data-bs-target="#xs-injectables-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' :
                                        'id="xs-injectables-links-module-GISModule-56f9884a715c44b65c6fad69298be66e359c5e949d3ce88907e4b76f72afa961ff219bdf379b21477029db089e73282999ddd3e184c86e89b6f07f6cc9444a7b"' }>
                                        <li class="link">
                                            <a href="injectables/GisLoadedGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisLoadedGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/LayerService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LayerService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/GISRoutingModule.html" data-type="entity-link" >GISRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/GruppiMerceModule.html" data-type="entity-link" >GruppiMerceModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-GruppiMerceModule-584c0d2a3b30e4bb49eac615f6658447fca4e4f0d6e473d0cf446091f78d73ed04e3075e42961598897b52d1d7766daaecab7a83132d3aac971173fa0e5827f8"' : 'data-bs-target="#xs-components-links-module-GruppiMerceModule-584c0d2a3b30e4bb49eac615f6658447fca4e4f0d6e473d0cf446091f78d73ed04e3075e42961598897b52d1d7766daaecab7a83132d3aac971173fa0e5827f8"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-GruppiMerceModule-584c0d2a3b30e4bb49eac615f6658447fca4e4f0d6e473d0cf446091f78d73ed04e3075e42961598897b52d1d7766daaecab7a83132d3aac971173fa0e5827f8"' :
                                            'id="xs-components-links-module-GruppiMerceModule-584c0d2a3b30e4bb49eac615f6658447fca4e4f0d6e473d0cf446091f78d73ed04e3075e42961598897b52d1d7766daaecab7a83132d3aac971173fa0e5827f8"' }>
                                            <li class="link">
                                                <a href="components/EditGruppiMerceComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditGruppiMerceComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/EditGruppiMerceGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditGruppiMerceGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiMerceGrid2Component.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiMerceGrid2Component</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiMerceMasterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiMerceMasterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiUtentiGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiUtentiGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiUtentiPerGruppiMerce.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiUtentiPerGruppiMerce</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiUtentixGruppiMerceGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiUtentixGruppiMerceGridComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/GruppiMerceRoutingModule.html" data-type="entity-link" >GruppiMerceRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/GruppiRaccoltaModule.html" data-type="entity-link" >GruppiRaccoltaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-GruppiRaccoltaModule-64b30d17d443078522234ca2c8297cae062e40f39282049300f537432f5fa7804236e605a0060ee3939f75e13cfcd6a5c993097aae998eafad95a2e314d98da7"' : 'data-bs-target="#xs-components-links-module-GruppiRaccoltaModule-64b30d17d443078522234ca2c8297cae062e40f39282049300f537432f5fa7804236e605a0060ee3939f75e13cfcd6a5c993097aae998eafad95a2e314d98da7"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-GruppiRaccoltaModule-64b30d17d443078522234ca2c8297cae062e40f39282049300f537432f5fa7804236e605a0060ee3939f75e13cfcd6a5c993097aae998eafad95a2e314d98da7"' :
                                            'id="xs-components-links-module-GruppiRaccoltaModule-64b30d17d443078522234ca2c8297cae062e40f39282049300f537432f5fa7804236e605a0060ee3939f75e13cfcd6a5c993097aae998eafad95a2e314d98da7"' }>
                                            <li class="link">
                                                <a href="components/GruppiRaccoltaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiRaccoltaComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/GruppiRaccoltaRoutingModule.html" data-type="entity-link" >GruppiRaccoltaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ImpresaModule.html" data-type="entity-link" >ImpresaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ImpresaModule-8becb6093d25b4b3aff23ca00a700a1767aa941730c83e5eaab5b34094ab464f8eb97928274159a6ca6a35f65c677982207f62b6a857e9548d14e93f05fe7c2d"' : 'data-bs-target="#xs-components-links-module-ImpresaModule-8becb6093d25b4b3aff23ca00a700a1767aa941730c83e5eaab5b34094ab464f8eb97928274159a6ca6a35f65c677982207f62b6a857e9548d14e93f05fe7c2d"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ImpresaModule-8becb6093d25b4b3aff23ca00a700a1767aa941730c83e5eaab5b34094ab464f8eb97928274159a6ca6a35f65c677982207f62b6a857e9548d14e93f05fe7c2d"' :
                                            'id="xs-components-links-module-ImpresaModule-8becb6093d25b4b3aff23ca00a700a1767aa941730c83e5eaab5b34094ab464f8eb97928274159a6ca6a35f65c677982207f62b6a857e9548d14e93f05fe7c2d"' }>
                                            <li class="link">
                                                <a href="components/ContattiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ContattiEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CooperativeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CooperativeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpresaEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpresaEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/MacchinaModule.html" data-type="entity-link" >MacchinaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-MacchinaModule-e6a5882ea91aaf97fa8f26bd1c8cb091c4232caa7e88e3073516c7efe341fef14adc8bd3d30cfc90cae30ce612a81d81ce88f18be8315641eb185927cf0b8952"' : 'data-bs-target="#xs-components-links-module-MacchinaModule-e6a5882ea91aaf97fa8f26bd1c8cb091c4232caa7e88e3073516c7efe341fef14adc8bd3d30cfc90cae30ce612a81d81ce88f18be8315641eb185927cf0b8952"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-MacchinaModule-e6a5882ea91aaf97fa8f26bd1c8cb091c4232caa7e88e3073516c7efe341fef14adc8bd3d30cfc90cae30ce612a81d81ce88f18be8315641eb185927cf0b8952"' :
                                            'id="xs-components-links-module-MacchinaModule-e6a5882ea91aaf97fa8f26bd1c8cb091c4232caa7e88e3073516c7efe341fef14adc8bd3d30cfc90cae30ce612a81d81ce88f18be8315641eb185927cf0b8952"' }>
                                            <li class="link">
                                                <a href="components/CostiMacchinaEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CostiMacchinaEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchinaEditCaratteristicheComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchinaEditCaratteristicheComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchineEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchineEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchineEditGerarchiaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchineEditGerarchiaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MacchineEditImageComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MacchineEditImageComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/MenuAgendaModule.html" data-type="entity-link" >MenuAgendaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' : 'data-bs-target="#xs-components-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' :
                                            'id="xs-components-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' }>
                                            <li class="link">
                                                <a href="components/AgendaFiltersComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AgendaFiltersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AllOperationsTableComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AllOperationsTableComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BrogliaccioGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BrogliaccioGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridAddMacchineComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridAddMacchineComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridAddOperaiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridAddOperaiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridZooComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridZooComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuAgendaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuAgendaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OperationsListComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OperationsListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OperationsMassEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OperationsMassEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RicetteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RicetteComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' : 'data-bs-target="#xs-injectables-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' :
                                        'id="xs-injectables-links-module-MenuAgendaModule-40b4ea3684b385b5390663c38065b7ccac5543eb4f7bf0f0526ca1f4ec36a92f23acd1f0aa66f9a1fe6b73ff955ce0b81d68b9e7b6890050a9e98e37e4b9ccc3"' }>
                                        <li class="link">
                                            <a href="injectables/DataLayerStyleService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DataLayerStyleService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EditFeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditFeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EditFeatureWindowService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditFeatureWindowService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/FeatureInformationService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FeatureInformationService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/FeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GeoJsonService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GeoJsonService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapDataService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapDataService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapFeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapFeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapGeoJsonLazyService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapGeoJsonLazyService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapGeoJsonService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapGeoJsonService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/InfowindowClustererService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InfowindowClustererService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/LayerStyleService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LayerStyleService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PolygonLabelInfowindowService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonLabelInfowindowService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PolygonLabelService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonLabelService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/QdCLoadedGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >QdCLoadedGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/RetinaturaService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RetinaturaService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/RicetteService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RicetteService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/TreeGisService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeGisService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/WKTService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WKTService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/WmsService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WmsService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/PianoContiModule.html" data-type="entity-link" >PianoContiModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-PianoContiModule-f0b339dea77e0c9e2dd7e6af7581ce3d805341227c5a26877838ec66f034c278c45c0ff256020df2bc8a238768414c4d96522e7075611c0670d15ee28d3f4e9a"' : 'data-bs-target="#xs-components-links-module-PianoContiModule-f0b339dea77e0c9e2dd7e6af7581ce3d805341227c5a26877838ec66f034c278c45c0ff256020df2bc8a238768414c4d96522e7075611c0670d15ee28d3f4e9a"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-PianoContiModule-f0b339dea77e0c9e2dd7e6af7581ce3d805341227c5a26877838ec66f034c278c45c0ff256020df2bc8a238768414c4d96522e7075611c0670d15ee28d3f4e9a"' :
                                            'id="xs-components-links-module-PianoContiModule-f0b339dea77e0c9e2dd7e6af7581ce3d805341227c5a26877838ec66f034c278c45c0ff256020df2bc8a238768414c4d96522e7075611c0670d15ee28d3f4e9a"' }>
                                            <li class="link">
                                                <a href="components/PianoContiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PianoContiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PianoContiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PianoContiEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/PianoContiRoutingModule.html" data-type="entity-link" >PianoContiRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ProfilazioneImpreseModule.html" data-type="entity-link" >ProfilazioneImpreseModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' : 'data-bs-target="#xs-components-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' :
                                            'id="xs-components-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' }>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseCampoApplicativoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseCampoApplicativoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseContattiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseContattiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseDefaultDistintaProduzioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseDefaultDistintaProduzioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseDefaultPianiColturaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseDefaultPianiColturaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseDefaultSpecieComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseDefaultSpecieComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseGlobalComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseGlobalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseGruppoNoteDialogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseGruppoNoteDialogComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseGruppoOperazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseGruppoOperazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseLavorazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseLavorazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseMacchineComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseMacchineComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseMacchineDialogComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseMacchineDialogComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseMacchineOperatoriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseMacchineOperatoriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseNoteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseNoteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseNoteListComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseNoteListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseParametriGeneraliColturaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseParametriGeneraliColturaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseSpecieComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseSpecieComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfilazioneImpreseVarietaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseVarietaComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' : 'data-bs-target="#xs-injectables-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' :
                                        'id="xs-injectables-links-module-ProfilazioneImpreseModule-3e562ebc5ad756b9afee86cd7527e7b6bf606d405f2fe21a55cca3a90f0c9c665c03058f03ee9e86a43b20aae3dbaa578c0a8367fe94c39a5ad4e253de93fdf3"' }>
                                        <li class="link">
                                            <a href="injectables/ProfilazioneImpreseService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfilazioneImpreseService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/ProfilazioneImpreseRoutingModule.html" data-type="entity-link" >ProfilazioneImpreseRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ProfilazioneModule.html" data-type="entity-link" >ProfilazioneModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ProfilazioneModule-2fe6368653917d8445d393d91baabf5cbd94ed816d896fc83656e7ce2be124a868ad805ae2cd676c0e04dfb2f4346edd46392d68e13eb55b72bb52ae03a30d58"' : 'data-bs-target="#xs-components-links-module-ProfilazioneModule-2fe6368653917d8445d393d91baabf5cbd94ed816d896fc83656e7ce2be124a868ad805ae2cd676c0e04dfb2f4346edd46392d68e13eb55b72bb52ae03a30d58"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ProfilazioneModule-2fe6368653917d8445d393d91baabf5cbd94ed816d896fc83656e7ce2be124a868ad805ae2cd676c0e04dfb2f4346edd46392d68e13eb55b72bb52ae03a30d58"' :
                                            'id="xs-components-links-module-ProfilazioneModule-2fe6368653917d8445d393d91baabf5cbd94ed816d896fc83656e7ce2be124a868ad805ae2cd676c0e04dfb2f4346edd46392d68e13eb55b72bb52ae03a30d58"' }>
                                            <li class="link">
                                                <a href="components/CardImpostazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CardImpostazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ClientePermessiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ClientePermessiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ClientePermessiGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ClientePermessiGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FiltriUtenteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltriUtenteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FiltroSqlMateriePrimeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltroSqlMateriePrimeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormImpostazioniAziendeCentriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FormImpostazioniAziendeCentriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormImpostazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FormImpostazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormatiStampaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FormatiStampaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GeneralFieldComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GeneralFieldComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridAziendeCentriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridAziendeCentriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridAziendeCentriImpostazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridAziendeCentriImpostazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridGroupTransitionsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridGroupTransitionsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridGruppiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridGruppiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridPasswordComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridPasswordComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridPermessiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridPermessiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridPermessiEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridPermessiEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridProfiliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridProfiliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridProfiliPermessiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridProfiliPermessiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridUtentiPermessiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridUtentiPermessiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridVisibilitaEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridVisibilitaEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GruppiUtentiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GruppiUtentiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImportUtentiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImportUtentiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpostazioniUtenteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpostazioniUtenteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuImpostazioniAziendeCentriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuImpostazioniAziendeCentriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuProfilazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuProfilazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuProfilazioneGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuProfilazioneGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuUtentiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuUtentiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuVisibilitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuVisibilitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PaginaAziendeCentriImpostazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PaginaAziendeCentriImpostazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PaginaImpostazioniMassiveComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PaginaImpostazioniMassiveComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfiliPermessiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfiliPermessiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsAnnataAgrariaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsAnnataAgrariaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsCategorieMagazzinoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsCategorieMagazzinoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsGiasAppComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsGiasAppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsGisComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsGisComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsLavorazioniGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsLavorazioniGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsManagerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsManagerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsProxyComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsProxyComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SettingsSpecieVarietaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SettingsSpecieVarietaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SezioneImpostazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SezioneImpostazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VisibilitaUtenteEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VisibilitaUtenteEditComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ProfilazioneRoutingModule.html" data-type="entity-link" >ProfilazioneRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/QuadernoDiCampagnaModule.html" data-type="entity-link" >QuadernoDiCampagnaModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' : 'data-bs-target="#xs-components-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' :
                                            'id="xs-components-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                            <li class="link">
                                                <a href="components/AbbattimentoImpiantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AbbattimentoImpiantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AcquaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AcquaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AnagraficheIndiciMaturitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnagraficheIndiciMaturitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AnagraficheRilieviAvversitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnagraficheRilieviAvversitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AvversitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AvversitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AziendaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AziendaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoneAggiuntaProdottoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoneAggiuntaProdottoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoneCreaProdottoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoneCreaProdottoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoneRicercaProdotti.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoneRicercaProdotti</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoniGestioneProdottoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoniGestioneProdottoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoniSalvataggioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoniSalvataggioComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoniSezioneProdottiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoniSezioneProdottiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BottoniTestataComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BottoniTestataComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ConfigurazioneOperazioniCulturaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConfigurazioneOperazioniCulturaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CostiAccessoriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CostiAccessoriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DataOperazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DataOperazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DettagliFertilizzantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DettagliFertilizzantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DettagliFormulatiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DettagliFormulatiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DettagliSementiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DettagliSementiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DisciplinareComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DisciplinareComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DosiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DosiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FertilizzantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FertilizzantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FlagMagazziniAgenzieComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FlagMagazziniAgenzieComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FlagMagazziniEsterniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FlagMagazziniEsterniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FlagVisualizzaGiacenzeZeroComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FlagVisualizzaGiacenzeZeroComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FormulatiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FormulatiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GestioneNoteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GestioneNoteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GestioneOperazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GestioneOperazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridDosiProdottiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridDosiProdottiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridImpiantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridImpiantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridMacchineComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridMacchineComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridNoteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridNoteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridOperatoriComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridOperatoriComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridProdottiDaTrattareComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridProdottiDaTrattareComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridRaccoltaAutoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridRaccoltaAutoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridRilieviComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridRilieviComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridRipartizioneManualeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridRipartizioneManualeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ImpreseParametriGHGComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ImpreseParametriGHGComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MagazzinoLottoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MagazzinoLottoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ModalitaApplicazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ModalitaApplicazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MultiOperazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MultiOperazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/NotaTestualeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >NotaTestualeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OperatoreComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OperatoreComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OperazioneCausaleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OperazioneCausaleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OpzioniRaccoltaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OpzioniRaccoltaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/OpzioniSeminaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >OpzioniSeminaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProdottiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProdottiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/QuadernoDiCampagnaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >QuadernoDiCampagnaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/QuantitaProdottiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >QuantitaProdottiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RaccoltaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RaccoltaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RicercaFertilizzantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RicercaFertilizzantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RicercaFormulatiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RicercaFormulatiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RicercaSementiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RicercaSementiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RilevaFaseEpocaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RilevaFaseEpocaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RilieviAvversitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RilieviAvversitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RilieviComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RilieviComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RipartizioneManualeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RipartizioneManualeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SementiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SementiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SezioniNoProdottoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SezioniNoProdottoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SpecieAnimaleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SpecieAnimaleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SpecieComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SpecieComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SuperficiImpiantiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SuperficiImpiantiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestataComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestataComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestataRicettaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestataRicettaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TestataVisitaComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestataVisitaComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TrattamentoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TrattamentoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UnitaDiMisuraComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UnitaDiMisuraComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#directives-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' : 'data-bs-target="#xs-directives-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' :
                                        'id="xs-directives-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                        <li class="link">
                                            <a href="directives/ProjectDocumentaleDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProjectDocumentaleDirective</a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' : 'data-bs-target="#xs-injectables-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' :
                                        'id="xs-injectables-links-module-QuadernoDiCampagnaModule-2d5c66ac56efe0e3848ee90d20e0ce53bf9d422cd74153bb99def9b54e4632248f167a5f8ab322c9e8c4c97d23a9597115935c973fdabce41fcaa1d5dcdc9031"' }>
                                        <li class="link">
                                            <a href="injectables/DataLayerStyleService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DataLayerStyleService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EditFeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditFeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EditFeatureWindowService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >EditFeatureWindowService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/FeatureInformationService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FeatureInformationService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/FeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GeoJsonService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GeoJsonService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GisToolbarService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisToolbarService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapDataService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapDataService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapFeatureService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapFeatureService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapGeoJsonLazyService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapGeoJsonLazyService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapGeoJsonService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapGeoJsonService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/GoogleMapService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleMapService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/InfowindowClustererService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InfowindowClustererService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/LayerStyleService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LayerStyleService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MeasureDistanceService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MeasureDistanceService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PolygonLabelInfowindowService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonLabelInfowindowService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PolygonLabelService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PolygonLabelService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/PositionService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PositionService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/QdCLoadedGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >QdCLoadedGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/RetinaturaService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RetinaturaService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/TreeGisService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeGisService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/WKTService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WKTService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/WmsService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >WmsService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/QuadernoDiCampagnaRoutingModule.html" data-type="entity-link" >QuadernoDiCampagnaRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ReportAbilitazionePdCModule.html" data-type="entity-link" >ReportAbilitazionePdCModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ReportAbilitazionePdCModule-c4ccf1a811f703e5dfc206da84251d3b4f0bcdf0fc35f85a28d71bc215a83513957afaa1d639d66e87a8ac8f2395bc6f24d790a273bed3d3a557e4c0a7141d96"' : 'data-bs-target="#xs-components-links-module-ReportAbilitazionePdCModule-c4ccf1a811f703e5dfc206da84251d3b4f0bcdf0fc35f85a28d71bc215a83513957afaa1d639d66e87a8ac8f2395bc6f24d790a273bed3d3a557e4c0a7141d96"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ReportAbilitazionePdCModule-c4ccf1a811f703e5dfc206da84251d3b4f0bcdf0fc35f85a28d71bc215a83513957afaa1d639d66e87a8ac8f2395bc6f24d790a273bed3d3a557e4c0a7141d96"' :
                                            'id="xs-components-links-module-ReportAbilitazionePdCModule-c4ccf1a811f703e5dfc206da84251d3b4f0bcdf0fc35f85a28d71bc215a83513957afaa1d639d66e87a8ac8f2395bc6f24d790a273bed3d3a557e4c0a7141d96"' }>
                                            <li class="link">
                                                <a href="components/ReportAbilitazionePdCComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ReportAbilitazionePdCComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ReportAbilitazionePdCRoutingModule.html" data-type="entity-link" >ReportAbilitazionePdCRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ReportImpiegoProdottiFitosanitariModule.html" data-type="entity-link" >ReportImpiegoProdottiFitosanitariModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ReportImpiegoProdottiFitosanitariModule-6c3ae27a2186031291851ef6496a3d11ac03ca63dee02d1abf03e8e317d8cf9f2d77dbdc495f0913c7617ff53d799e9d2e9378a944f69a9b431ad22498300129"' : 'data-bs-target="#xs-components-links-module-ReportImpiegoProdottiFitosanitariModule-6c3ae27a2186031291851ef6496a3d11ac03ca63dee02d1abf03e8e317d8cf9f2d77dbdc495f0913c7617ff53d799e9d2e9378a944f69a9b431ad22498300129"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ReportImpiegoProdottiFitosanitariModule-6c3ae27a2186031291851ef6496a3d11ac03ca63dee02d1abf03e8e317d8cf9f2d77dbdc495f0913c7617ff53d799e9d2e9378a944f69a9b431ad22498300129"' :
                                            'id="xs-components-links-module-ReportImpiegoProdottiFitosanitariModule-6c3ae27a2186031291851ef6496a3d11ac03ca63dee02d1abf03e8e317d8cf9f2d77dbdc495f0913c7617ff53d799e9d2e9378a944f69a9b431ad22498300129"' }>
                                            <li class="link">
                                                <a href="components/ProvinciaComuneMultiselectComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProvinciaComuneMultiselectComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ReportImpiegoProdottiFitosanitariComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ReportImpiegoProdottiFitosanitariComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ReportImpiegoProdottiFitosanitariRoutingModule.html" data-type="entity-link" >ReportImpiegoProdottiFitosanitariRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/RequisistiStabilimentoModule.html" data-type="entity-link" >RequisistiStabilimentoModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-RequisistiStabilimentoModule-5db1b1793ba39d14494db3ff64149d2018eabbdf21014876971c187703b4e922514e4558994323bd961258d7a39a0a9e9b0f5a55892dea37dbd68c1caa48a9d4"' : 'data-bs-target="#xs-components-links-module-RequisistiStabilimentoModule-5db1b1793ba39d14494db3ff64149d2018eabbdf21014876971c187703b4e922514e4558994323bd961258d7a39a0a9e9b0f5a55892dea37dbd68c1caa48a9d4"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-RequisistiStabilimentoModule-5db1b1793ba39d14494db3ff64149d2018eabbdf21014876971c187703b4e922514e4558994323bd961258d7a39a0a9e9b0f5a55892dea37dbd68c1caa48a9d4"' :
                                            'id="xs-components-links-module-RequisistiStabilimentoModule-5db1b1793ba39d14494db3ff64149d2018eabbdf21014876971c187703b4e922514e4558994323bd961258d7a39a0a9e9b0f5a55892dea37dbd68c1caa48a9d4"' }>
                                            <li class="link">
                                                <a href="components/FiltriRequisitiStabilimentoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltriRequisitiStabilimentoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RequisitiStabilimentoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RequisitiStabilimentoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RequisitiStabilimentoContrattiGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RequisitiStabilimentoContrattiGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RequisitiStabilimentoGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RequisitiStabilimentoGridComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VDContrattiComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VDContrattiComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VDPianoColturaleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VDPianoColturaleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VisualizzaDettagliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VisualizzaDettagliComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/RequisistiStabilimentoRoutingModule.html" data-type="entity-link" >RequisistiStabilimentoRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/RilieviModule.html" data-type="entity-link" >RilieviModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' : 'data-bs-target="#xs-components-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' :
                                            'id="xs-components-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' }>
                                            <li class="link">
                                                <a href="components/FiltersRilieviComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltersRilieviComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuRilieviComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuRilieviComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' : 'data-bs-target="#xs-injectables-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' :
                                        'id="xs-injectables-links-module-RilieviModule-3d19145a05a8f36600a84e74744b69e1dbc69856a44a63b4c081d0f7529f3f703d794b5bff11d42a087ce3b7cea8acb26c0eb5359d0e0d7334ceea13619d9f68"' }>
                                        <li class="link">
                                            <a href="injectables/FiltersRilieviService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltersRilieviService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/RilieviRoutingModule.html" data-type="entity-link" >RilieviRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/SharedServicesModule.html" data-type="entity-link" >SharedServicesModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/TestGiasKendoGridModule.html" data-type="entity-link" >TestGiasKendoGridModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-TestGiasKendoGridModule-4dea26779d425c483865e72c7370c3fc59a56433b70bbdf8f86d27b692a296f548698c099651bd04b6bd96b764e32621480c5ec5e62663156521272531785999"' : 'data-bs-target="#xs-components-links-module-TestGiasKendoGridModule-4dea26779d425c483865e72c7370c3fc59a56433b70bbdf8f86d27b692a296f548698c099651bd04b6bd96b764e32621480c5ec5e62663156521272531785999"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-TestGiasKendoGridModule-4dea26779d425c483865e72c7370c3fc59a56433b70bbdf8f86d27b692a296f548698c099651bd04b6bd96b764e32621480c5ec5e62663156521272531785999"' :
                                            'id="xs-components-links-module-TestGiasKendoGridModule-4dea26779d425c483865e72c7370c3fc59a56433b70bbdf8f86d27b692a296f548698c099651bd04b6bd96b764e32621480c5ec5e62663156521272531785999"' }>
                                            <li class="link">
                                                <a href="components/TestGiasGiasKendoGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TestGiasGiasKendoGridComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/TestGiasKendoGridRoutingModule.html" data-type="entity-link" >TestGiasKendoGridRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/TranslocoRootModule.html" data-type="entity-link" >TranslocoRootModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/TrattamentoZooModule.html" data-type="entity-link" >TrattamentoZooModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' : 'data-bs-target="#xs-components-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' :
                                            'id="xs-components-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' }>
                                            <li class="link">
                                                <a href="components/GridCapiAnimaliComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridCapiAnimaliComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GridProdottiSomministrazioneComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GridProdottiSomministrazioneComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TrattamentoZooComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TrattamentoZooComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' : 'data-bs-target="#xs-injectables-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' :
                                        'id="xs-injectables-links-module-TrattamentoZooModule-3e658fbfe35d072cffdb8ced9ba7bf4349edc3bb80624e1cb9806cf88227b31e2d53f532ff1ffcccf186220e54d140159882b85797cc1dd7e8fe767ae91ecfc7"' }>
                                        <li class="link">
                                            <a href="injectables/BreadcrumbsService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BreadcrumbsService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/CapiAnimaliConfigService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CapiAnimaliConfigService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/ProdottoSomministrazioneConfigService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProdottoSomministrazioneConfigService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/TrattamentoZooRoutingModule.html" data-type="entity-link" >TrattamentoZooRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/UikitModule.html" data-type="entity-link" >UikitModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' : 'data-bs-target="#xs-components-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' :
                                            'id="xs-components-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' }>
                                            <li class="link">
                                                <a href="components/AnagraficaTreeFiltersComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnagraficaTreeFiltersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CodiciTemplateComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CodiciTemplateComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DraggableComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DraggableComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GiasAddressTemplateComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GiasAddressTemplateComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GiasBreadcrumbsComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GiasBreadcrumbsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GisTreeFiltersComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GisTreeFiltersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/KendoTreeContainerComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >KendoTreeContainerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SvgIconSpeciesComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SvgIconSpeciesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TreeViewComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >TreeViewComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#directives-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' : 'data-bs-target="#xs-directives-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' }>
                                        <span class="icon ion-md-code-working"></span>
                                        <span>Directives</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="directives-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' :
                                        'id="xs-directives-links-module-UikitModule-4b34cfd57905aecc72a2ca4dcf0bcfc4bd4826da7ea10f8343854bb9c317b56e04411c39806b86509d074a7c3dc5959f697d5186723c3efef5ea4fa503c0f1b5"' }>
                                        <li class="link">
                                            <a href="directives/DraggableDirective.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >DraggableDirective</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/ValutazioniMainComponentModule.html" data-type="entity-link" >ValutazioniMainComponentModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ValutazioniMainComponentModule-f79ae2b2bab4f366834d3ea87e4a7d4b3aebfabee3fef968c2bc69d5b25d1d1dd1e8eec5c14584c06f0723114ee8ebc2a479e16b8f8429dbd51f738f047fd183"' : 'data-bs-target="#xs-components-links-module-ValutazioniMainComponentModule-f79ae2b2bab4f366834d3ea87e4a7d4b3aebfabee3fef968c2bc69d5b25d1d1dd1e8eec5c14584c06f0723114ee8ebc2a479e16b8f8429dbd51f738f047fd183"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ValutazioniMainComponentModule-f79ae2b2bab4f366834d3ea87e4a7d4b3aebfabee3fef968c2bc69d5b25d1d1dd1e8eec5c14584c06f0723114ee8ebc2a479e16b8f8429dbd51f738f047fd183"' :
                                            'id="xs-components-links-module-ValutazioniMainComponentModule-f79ae2b2bab4f366834d3ea87e4a7d4b3aebfabee3fef968c2bc69d5b25d1d1dd1e8eec5c14584c06f0723114ee8ebc2a479e16b8f8429dbd51f738f047fd183"' }>
                                            <li class="link">
                                                <a href="components/ValutazioniMainComponentComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniMainComponentComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ValutazioniMainRoutingModule.html" data-type="entity-link" >ValutazioniMainRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ValutazioniModule.html" data-type="entity-link" >ValutazioniModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ValutazioniModule-429d62a780f41ab79d8d9566972a662c6ad0a25db0b8cab74ee6ed66b9e8168eeba2463e0b0b52adf696799210441d8a9fb594564baeabb62e1545d690838d09"' : 'data-bs-target="#xs-components-links-module-ValutazioniModule-429d62a780f41ab79d8d9566972a662c6ad0a25db0b8cab74ee6ed66b9e8168eeba2463e0b0b52adf696799210441d8a9fb594564baeabb62e1545d690838d09"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ValutazioniModule-429d62a780f41ab79d8d9566972a662c6ad0a25db0b8cab74ee6ed66b9e8168eeba2463e0b0b52adf696799210441d8a9fb594564baeabb62e1545d690838d09"' :
                                            'id="xs-components-links-module-ValutazioniModule-429d62a780f41ab79d8d9566972a662c6ad0a25db0b8cab74ee6ed66b9e8168eeba2463e0b0b52adf696799210441d8a9fb594564baeabb62e1545d690838d09"' }>
                                            <li class="link">
                                                <a href="components/AnnoValutazioniEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AnnoValutazioniEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ContoEconomicoDettaglioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ContoEconomicoDettaglioComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MainValutazioniEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MainValutazioniEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioneEditDettaglioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioneEditDettaglioComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioniComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioniContoEconomicoComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniContoEconomicoComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioniEditComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniEditComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioniStatoPatrimonialeComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniStatoPatrimonialeComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ValutazioniStatoPatrimonialeDettaglioComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ValutazioniStatoPatrimonialeDettaglioComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ValutazioniRoutingModule.html" data-type="entity-link" >ValutazioniRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/VisiteModule.html" data-type="entity-link" >VisiteModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-VisiteModule-396c653bd4d4fab7c1a7df38f0b6145a66c359e51ece4c7a05d908bf95194183312544828fc7b1a08b04b2649ea93a153231a1b86cc340ee79e4256550de625f"' : 'data-bs-target="#xs-components-links-module-VisiteModule-396c653bd4d4fab7c1a7df38f0b6145a66c359e51ece4c7a05d908bf95194183312544828fc7b1a08b04b2649ea93a153231a1b86cc340ee79e4256550de625f"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-VisiteModule-396c653bd4d4fab7c1a7df38f0b6145a66c359e51ece4c7a05d908bf95194183312544828fc7b1a08b04b2649ea93a153231a1b86cc340ee79e4256550de625f"' :
                                            'id="xs-components-links-module-VisiteModule-396c653bd4d4fab7c1a7df38f0b6145a66c359e51ece4c7a05d908bf95194183312544828fc7b1a08b04b2649ea93a153231a1b86cc340ee79e4256550de625f"' }>
                                            <li class="link">
                                                <a href="components/FiltersVisiteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FiltersVisiteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuVisiteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuVisiteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VisiteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >VisiteComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/VisiteRoutingModule.html" data-type="entity-link" >VisiteRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/ZooModule.html" data-type="entity-link" >ZooModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-ZooModule-5cac47f64ca0a40bd163aebb7af6e475d8b4ea580c864de64bd7d07f97eab57dfab864801739f0e15e367c648c156823b55ac57556fda62819ba5e0783765fe9"' : 'data-bs-target="#xs-components-links-module-ZooModule-5cac47f64ca0a40bd163aebb7af6e475d8b4ea580c864de64bd7d07f97eab57dfab864801739f0e15e367c648c156823b55ac57556fda62819ba5e0783765fe9"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-ZooModule-5cac47f64ca0a40bd163aebb7af6e475d8b4ea580c864de64bd7d07f97eab57dfab864801739f0e15e367c648c156823b55ac57556fda62819ba5e0783765fe9"' :
                                            'id="xs-components-links-module-ZooModule-5cac47f64ca0a40bd163aebb7af6e475d8b4ea580c864de64bd7d07f97eab57dfab864801739f0e15e367c648c156823b55ac57556fda62819ba5e0783765fe9"' }>
                                            <li class="link">
                                                <a href="components/MenuZooComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuZooComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ZooComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ZooComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ZooFavoritesEditingComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ZooFavoritesEditingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ZooOperationsGridComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ZooOperationsGridComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/ZooRoutingModule.html" data-type="entity-link" >ZooRoutingModule</a>
                            </li>
                </ul>
                </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#components-links"' :
                            'data-bs-target="#xs-components-links"' }>
                            <span class="icon ion-md-cog"></span>
                            <span>Components</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="components-links"' : 'id="xs-components-links"' }>
                            <li class="link">
                                <a href="components/AgriculturalPlotSlopeComponent.html" data-type="entity-link" >AgriculturalPlotSlopeComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/AgriculturalPlotWeavingComponent.html" data-type="entity-link" >AgriculturalPlotWeavingComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/AlertDocumentiWidgetComponent.html" data-type="entity-link" >AlertDocumentiWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/AziendeMultiPieWidgetComponent.html" data-type="entity-link" >AziendeMultiPieWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CacCodificheComponent.html" data-type="entity-link" >CacCodificheComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ChartWidgetComponent.html" data-type="entity-link" >ChartWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/Co2ComparisonWidgetComponent.html" data-type="entity-link" >Co2ComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ColtureMultiAziendaWidget.html" data-type="entity-link" >ColtureMultiAziendaWidget</a>
                            </li>
                            <li class="link">
                                <a href="components/ColtureWidgetComponent.html" data-type="entity-link" >ColtureWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ColumnsChartWidgetComponent.html" data-type="entity-link" >ColumnsChartWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ComparisonWidgetComponent.html" data-type="entity-link" >ComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ConformitaWidgetComponent.html" data-type="entity-link" >ConformitaWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CostiRicaviAIWidgetComponent.html" data-type="entity-link" >CostiRicaviAIWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CustomMessageComponent.html" data-type="entity-link" >CustomMessageComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/DatiGeneraliFarmersComponent.html" data-type="entity-link" >DatiGeneraliFarmersComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ErosioneComparisonWidgetComponent.html" data-type="entity-link" >ErosioneComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/GestioneRichiesteComponent.html" data-type="entity-link" >GestioneRichiesteComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/GhgColtureWidgetComponent.html" data-type="entity-link" >GhgColtureWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/HeaderComponent.html" data-type="entity-link" >HeaderComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/HeaderComponent-1.html" data-type="entity-link" >HeaderComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IndicatoriWidgetComponent.html" data-type="entity-link" >IndicatoriWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IndicatoriWidgetDashboardComponent.html" data-type="entity-link" >IndicatoriWidgetDashboardComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IndicatoriWidgetFullComponent.html" data-type="entity-link" >IndicatoriWidgetFullComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IndicatoriWidgetGridComponent.html" data-type="entity-link" >IndicatoriWidgetGridComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IndicazioniComponent.html" data-type="entity-link" >IndicazioniComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LayerWindowComponent.html" data-type="entity-link" >LayerWindowComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/MappaMultiAziendaComponent.html" data-type="entity-link" >MappaMultiAziendaComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/MeteoWidgetComponent.html" data-type="entity-link" >MeteoWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/MeteoWidgetFullComponent.html" data-type="entity-link" >MeteoWidgetFullComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/MonitoraggioWidgetComponent.html" data-type="entity-link" >MonitoraggioWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/NavMenuComponent.html" data-type="entity-link" >NavMenuComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/OperazioniTemplateWidgetComponent.html" data-type="entity-link" >OperazioniTemplateWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PaeseMultiAziendaComponent.html" data-type="entity-link" >PaeseMultiAziendaComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PageNotFoundComponent.html" data-type="entity-link" >PageNotFoundComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PlvComparisonWidgetComponent.html" data-type="entity-link" >PlvComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ProduttivitaComparisonWidgetComponent.html" data-type="entity-link" >ProduttivitaComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ProduzioneColtureWidgetComponent.html" data-type="entity-link" >ProduzioneColtureWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ProtocolliComponent.html" data-type="entity-link" >ProtocolliComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ProtocolliInCorsoComponent.html" data-type="entity-link" >ProtocolliInCorsoComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RiepilogoMeteoWidgetComponent.html" data-type="entity-link" >RiepilogoMeteoWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoAggregatoComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoAggregatoComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoAllagamentoComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoAllagamentoComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoGelataComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoGelataComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoGrandineComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoGrandineComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoSiccitaComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoSiccitaComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/RischioMeteoVentoForteComparisonWidgetComponent.html" data-type="entity-link" >RischioMeteoVentoForteComparisonWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/SeminatiMultiAziendaWidgetComponent.html" data-type="entity-link" >SeminatiMultiAziendaWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/SpecialEditComponent.html" data-type="entity-link" >SpecialEditComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/StimeProduzioneColtureWidgetComponent.html" data-type="entity-link" >StimeProduzioneColtureWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/TargetSuperficieWidgetComponent.html" data-type="entity-link" >TargetSuperficieWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/TestGridComponent.html" data-type="entity-link" >TestGridComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UltimeAttivitaWidgetComponent.html" data-type="entity-link" >UltimeAttivitaWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UltimeVisiteWidgetComponent.html" data-type="entity-link" >UltimeVisiteWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UltimiAcquistiWidgetComponent.html" data-type="entity-link" >UltimiAcquistiWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UltimiProdottiWidgetComponent.html" data-type="entity-link" >UltimiProdottiWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UltimiRilieviWidgetComponent.html" data-type="entity-link" >UltimiRilieviWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/UniqueRatingWidgetComponent.html" data-type="entity-link" >UniqueRatingWidgetComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetConfigComponent.html" data-type="entity-link" >WidgetConfigComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetConfigListComponent.html" data-type="entity-link" >WidgetConfigListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetGridComponent.html" data-type="entity-link" >WidgetGridComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetResizerComponent.html" data-type="entity-link" >WidgetResizerComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetsComponent.html" data-type="entity-link" >WidgetsComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/WidgetTemplateComponent.html" data-type="entity-link" >WidgetTemplateComponent</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#directives-links"' :
                                'data-bs-target="#xs-directives-links"' }>
                                <span class="icon ion-md-code-working"></span>
                                <span>Directives</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="directives-links"' : 'id="xs-directives-links"' }>
                                <li class="link">
                                    <a href="directives/DraggableDirective.html" data-type="entity-link" >DraggableDirective</a>
                                </li>
                                <li class="link">
                                    <a href="directives/ProjectDocumentaleDirective.html" data-type="entity-link" >ProjectDocumentaleDirective</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#classes-links"' :
                            'data-bs-target="#xs-classes-links"' }>
                            <span class="icon ion-ios-paper"></span>
                            <span>Classes</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="classes-links"' : 'id="xs-classes-links"' }>
                            <li class="link">
                                <a href="classes/Acqua.html" data-type="entity-link" >Acqua</a>
                            </li>
                            <li class="link">
                                <a href="classes/Actions.html" data-type="entity-link" >Actions</a>
                            </li>
                            <li class="link">
                                <a href="classes/AddressFG.html" data-type="entity-link" >AddressFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/AddressModel.html" data-type="entity-link" >AddressModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/Aggiorna_Campo_Edit_Request.html" data-type="entity-link" >Aggiorna_Campo_Edit_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Aggiorna_Campo_inLine_Edit_Request.html" data-type="entity-link" >Aggiorna_Campo_inLine_Edit_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Aggiorna_Macchina_Edit_Request.html" data-type="entity-link" >Aggiorna_Macchina_Edit_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/AggiungiRicettaAlPUA.html" data-type="entity-link" >AggiungiRicettaAlPUA</a>
                            </li>
                            <li class="link">
                                <a href="classes/AgriculturalExcercice.html" data-type="entity-link" >AgriculturalExcercice</a>
                            </li>
                            <li class="link">
                                <a href="classes/AgriculturalPlot.html" data-type="entity-link" >AgriculturalPlot</a>
                            </li>
                            <li class="link">
                                <a href="classes/AgriculturalPlotService.html" data-type="entity-link" >AgriculturalPlotService</a>
                            </li>
                            <li class="link">
                                <a href="classes/AgronicaCoreParametri_NG.html" data-type="entity-link" >AgronicaCoreParametri_NG</a>
                            </li>
                            <li class="link">
                                <a href="classes/AgronicaLink_NG.html" data-type="entity-link" >AgronicaLink_NG</a>
                            </li>
                            <li class="link">
                                <a href="classes/Anagrafica_PageSelector.html" data-type="entity-link" >Anagrafica_PageSelector</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficaRoutes.html" data-type="entity-link" >AnagraficaRoutes</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficaTree_PageSelector.html" data-type="entity-link" >AnagraficaTree_PageSelector</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficheIndiciMaturitaGridModel.html" data-type="entity-link" >AnagraficheIndiciMaturitaGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficheIndiciMaturitaResult.html" data-type="entity-link" >AnagraficheIndiciMaturitaResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficheRilieviAvversitaGridModel.html" data-type="entity-link" >AnagraficheRilieviAvversitaGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnagraficheRilieviAvversitaResult.html" data-type="entity-link" >AnagraficheRilieviAvversitaResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnniKendoServerResult.html" data-type="entity-link" >AnniKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnniModel.html" data-type="entity-link" >AnniModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/AnnniObject.html" data-type="entity-link" >AnnniObject</a>
                            </li>
                            <li class="link">
                                <a href="classes/APICallsBasic.html" data-type="entity-link" >APICallsBasic</a>
                            </li>
                            <li class="link">
                                <a href="classes/APP_Ricetta_Operazione.html" data-type="entity-link" >APP_Ricetta_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentiCampoKendo.html" data-type="entity-link" >AppezzamentiCampoKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentiCampoKendoServerResult.html" data-type="entity-link" >AppezzamentiCampoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentiRequest.html" data-type="entity-link" >AppezzamentiRequest</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentiServerResult.html" data-type="entity-link" >AppezzamentiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Appezzamento.html" data-type="entity-link" >Appezzamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentoCampo.html" data-type="entity-link" >AppezzamentoCampo</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentoCampoId.html" data-type="entity-link" >AppezzamentoCampoId</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentoJoinDescrizioni.html" data-type="entity-link" >AppezzamentoJoinDescrizioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentoModel.html" data-type="entity-link" >AppezzamentoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppezzamentoXParcoMacchine.html" data-type="entity-link" >AppezzamentoXParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="classes/ApportoMacroelementi.html" data-type="entity-link" >ApportoMacroelementi</a>
                            </li>
                            <li class="link">
                                <a href="classes/AppPage.html" data-type="entity-link" >AppPage</a>
                            </li>
                            <li class="link">
                                <a href="classes/AssociaProfiloObj.html" data-type="entity-link" >AssociaProfiloObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/AssociazionePK.html" data-type="entity-link" >AssociazionePK</a>
                            </li>
                            <li class="link">
                                <a href="classes/Attivita.html" data-type="entity-link" >Attivita</a>
                            </li>
                            <li class="link">
                                <a href="classes/Attivita_Con_Parametri_Aggiuntivi.html" data-type="entity-link" >Attivita_Con_Parametri_Aggiuntivi</a>
                            </li>
                            <li class="link">
                                <a href="classes/AttivitaCDG.html" data-type="entity-link" >AttivitaCDG</a>
                            </li>
                            <li class="link">
                                <a href="classes/AttivitaPersonalizzata.html" data-type="entity-link" >AttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="classes/AttivitaxModificaMutipla.html" data-type="entity-link" >AttivitaxModificaMutipla</a>
                            </li>
                            <li class="link">
                                <a href="classes/Avversita.html" data-type="entity-link" >Avversita</a>
                            </li>
                            <li class="link">
                                <a href="classes/AvversitaGruppo.html" data-type="entity-link" >AvversitaGruppo</a>
                            </li>
                            <li class="link">
                                <a href="classes/AvversitaRilievo.html" data-type="entity-link" >AvversitaRilievo</a>
                            </li>
                            <li class="link">
                                <a href="classes/AziendaCentro.html" data-type="entity-link" >AziendaCentro</a>
                            </li>
                            <li class="link">
                                <a href="classes/AziendeCentriGridModel.html" data-type="entity-link" >AziendeCentriGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/BackEndColor.html" data-type="entity-link" >BackEndColor</a>
                            </li>
                            <li class="link">
                                <a href="classes/Balance.html" data-type="entity-link" >Balance</a>
                            </li>
                            <li class="link">
                                <a href="classes/BaseCodeDescr.html" data-type="entity-link" >BaseCodeDescr</a>
                            </li>
                            <li class="link">
                                <a href="classes/BaseCodeDescrStr.html" data-type="entity-link" >BaseCodeDescrStr</a>
                            </li>
                            <li class="link">
                                <a href="classes/BaseCodeDescrVal.html" data-type="entity-link" >BaseCodeDescrVal</a>
                            </li>
                            <li class="link">
                                <a href="classes/BaseLayerListItemComponent.html" data-type="entity-link" >BaseLayerListItemComponent</a>
                            </li>
                            <li class="link">
                                <a href="classes/BioOrganismoDiControllo.html" data-type="entity-link" >BioOrganismoDiControllo</a>
                            </li>
                            <li class="link">
                                <a href="classes/BioTipoAttivita.html" data-type="entity-link" >BioTipoAttivita</a>
                            </li>
                            <li class="link">
                                <a href="classes/BloccaAttivitaAgendaImpl.html" data-type="entity-link" >BloccaAttivitaAgendaImpl</a>
                            </li>
                            <li class="link">
                                <a href="classes/BloccaSbloccaAppezzamenti.html" data-type="entity-link" >BloccaSbloccaAppezzamenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/BloccaSbloccaAppezzamenti-1.html" data-type="entity-link" >BloccaSbloccaAppezzamenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/Blocco.html" data-type="entity-link" >Blocco</a>
                            </li>
                            <li class="link">
                                <a href="classes/BloccoxReport.html" data-type="entity-link" >BloccoxReport</a>
                            </li>
                            <li class="link">
                                <a href="classes/BreadCrumb.html" data-type="entity-link" >BreadCrumb</a>
                            </li>
                            <li class="link">
                                <a href="classes/BreadCrumbsSettings.html" data-type="entity-link" >BreadCrumbsSettings</a>
                            </li>
                            <li class="link">
                                <a href="classes/BrogliaccioServerResult.html" data-type="entity-link" >BrogliaccioServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/BudgetAnagrafica.html" data-type="entity-link" >BudgetAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="classes/BudgetTestata.html" data-type="entity-link" >BudgetTestata</a>
                            </li>
                            <li class="link">
                                <a href="classes/BufferZone.html" data-type="entity-link" >BufferZone</a>
                            </li>
                            <li class="link">
                                <a href="classes/CacCodiciSistema.html" data-type="entity-link" >CacCodiciSistema</a>
                            </li>
                            <li class="link">
                                <a href="classes/CacConvKendoGridModel.html" data-type="entity-link" >CacConvKendoGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/CambioLinguaObj.html" data-type="entity-link" >CambioLinguaObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/CampiCodiciTableData.html" data-type="entity-link" >CampiCodiciTableData</a>
                            </li>
                            <li class="link">
                                <a href="classes/CampiKendoServerResult.html" data-type="entity-link" >CampiKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Campo.html" data-type="entity-link" >Campo</a>
                            </li>
                            <li class="link">
                                <a href="classes/CancellaPermessiHttpDto.html" data-type="entity-link" >CancellaPermessiHttpDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CapoAnimale.html" data-type="entity-link" >CapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="classes/CapoAnimaleCDC.html" data-type="entity-link" >CapoAnimaleCDC</a>
                            </li>
                            <li class="link">
                                <a href="classes/Caratteristica.html" data-type="entity-link" >Caratteristica</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaratteristicaDettagli.html" data-type="entity-link" >CaratteristicaDettagli</a>
                            </li>
                            <li class="link">
                                <a href="classes/Carburante.html" data-type="entity-link" >Carburante</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaCatastoSettings.html" data-type="entity-link" >CaricaCatastoSettings</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaComboDestinazioniUso.html" data-type="entity-link" >CaricaComboDestinazioniUso</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaDatiCatastali.html" data-type="entity-link" >CaricaDatiCatastali</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaDatiDto.html" data-type="entity-link" >CaricaDatiDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaDatiDto-1.html" data-type="entity-link" >CaricaDatiDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CaricaZoo.html" data-type="entity-link" >CaricaZoo</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoAppezzamento.html" data-type="entity-link" >CatastoAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoAppezzamento_Extended.html" data-type="entity-link" >CatastoAppezzamento_Extended</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoCampo.html" data-type="entity-link" >CatastoCampo</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoCampoId.html" data-type="entity-link" >CatastoCampoId</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoCampoKendoServerResult.html" data-type="entity-link" >CatastoCampoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoCentroAziendale.html" data-type="entity-link" >CatastoCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoKendoModel.html" data-type="entity-link" >CatastoKendoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoKendoServerResult.html" data-type="entity-link" >CatastoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoKendoServerResult-1.html" data-type="entity-link" >CatastoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CatastoKendoServerResult-2.html" data-type="entity-link" >CatastoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CategoriaOperazione.html" data-type="entity-link" >CategoriaOperazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Causale.html" data-type="entity-link" >Causale</a>
                            </li>
                            <li class="link">
                                <a href="classes/Causale-1.html" data-type="entity-link" >Causale</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentraMappa.html" data-type="entity-link" >CentraMappa</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentriResolverResult.html" data-type="entity-link" >CentriResolverResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentriWrapper.html" data-type="entity-link" >CentriWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroAziendale.html" data-type="entity-link" >CentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroAziendaleDropdowns.html" data-type="entity-link" >CentroAziendaleDropdowns</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroAziendaleEsternoCollegato.html" data-type="entity-link" >CentroAziendaleEsternoCollegato</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroAziendaleNG.html" data-type="entity-link" >CentroAziendaleNG</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroDiCosto.html" data-type="entity-link" >CentroDiCosto</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroDropdownLists.html" data-type="entity-link" >CentroDropdownLists</a>
                            </li>
                            <li class="link">
                                <a href="classes/CentroKendoServerResult.html" data-type="entity-link" >CentroKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CheckboxMostraTutteLeImpreseCtrlObj.html" data-type="entity-link" >CheckboxMostraTutteLeImpreseCtrlObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/CheckTrattamento.html" data-type="entity-link" >CheckTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/children.html" data-type="entity-link" >children</a>
                            </li>
                            <li class="link">
                                <a href="classes/ClassamentiKendo.html" data-type="entity-link" >ClassamentiKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ClasseTessitura.html" data-type="entity-link" >ClasseTessitura</a>
                            </li>
                            <li class="link">
                                <a href="classes/ClientePermessiGridServerResult.html" data-type="entity-link" >ClientePermessiGridServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ClientePermessiScrivi.html" data-type="entity-link" >ClientePermessiScrivi</a>
                            </li>
                            <li class="link">
                                <a href="classes/ClientePermesso.html" data-type="entity-link" >ClientePermesso</a>
                            </li>
                            <li class="link">
                                <a href="classes/Cluster.html" data-type="entity-link" >Cluster</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodeType.html" data-type="entity-link" >CodeType</a>
                            </li>
                            <li class="link">
                                <a href="classes/Codice.html" data-type="entity-link" >Codice</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiceAnagrafe.html" data-type="entity-link" >CodiceAnagrafe</a>
                            </li>
                            <li class="link">
                                <a href="classes/Codici_Attivita_x_CentriAziendali.html" data-type="entity-link" >Codici_Attivita_x_CentriAziendali</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciAnagrafe_Occorrenze.html" data-type="entity-link" >CodiciAnagrafe_Occorrenze</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciAnagrafeValori.html" data-type="entity-link" >CodiciAnagrafeValori</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciAnagrafeValoriChiave.html" data-type="entity-link" >CodiciAnagrafeValoriChiave</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciAnagraficiServerResult.html" data-type="entity-link" >CodiciAnagraficiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciCampiKendoModel.html" data-type="entity-link" >CodiciCampiKendoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciNazioniISO3166.html" data-type="entity-link" >CodiciNazioniISO3166</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciServerResult.html" data-type="entity-link" >CodiciServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciServerResult-1.html" data-type="entity-link" >CodiciServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodiciXOperazione.html" data-type="entity-link" >CodiciXOperazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/CodificheKendoServerResult.html" data-type="entity-link" >CodificheKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ColturePrecedenti.html" data-type="entity-link" >ColturePrecedenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/CompanyModel.html" data-type="entity-link" >CompanyModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/Comune.html" data-type="entity-link" >Comune</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConduzioneSuxSpecie.html" data-type="entity-link" >ConduzioneSuxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConduzioneTraxSpecie.html" data-type="entity-link" >ConduzioneTraxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/Configurazione_Siti.html" data-type="entity-link" >Configurazione_Siti</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConfrontaVisibilitaUtenti.html" data-type="entity-link" >ConfrontaVisibilitaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/Constants.html" data-type="entity-link" >Constants</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConsultaSincroForm.html" data-type="entity-link" >ConsultaSincroForm</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConsultaSincroKendo.html" data-type="entity-link" >ConsultaSincroKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConsultaSincroLogKendoServerResult.html" data-type="entity-link" >ConsultaSincroLogKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ConsultaSincroServerResult.html" data-type="entity-link" >ConsultaSincroServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContattiEditGridModel.html" data-type="entity-link" >ContattiEditGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContattiEditGridRow.html" data-type="entity-link" >ContattiEditGridRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContattiEditServerResult.html" data-type="entity-link" >ContattiEditServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContattiKendo.html" data-type="entity-link" >ContattiKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContattiServerResult.html" data-type="entity-link" >ContattiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Contatto.html" data-type="entity-link" >Contatto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContoEconomicoKendoServerResult.html" data-type="entity-link" >ContoEconomicoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContoEconomicoModel.html" data-type="entity-link" >ContoEconomicoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ContoEconomicoWrapper.html" data-type="entity-link" >ContoEconomicoWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/Contribute.html" data-type="entity-link" >Contribute</a>
                            </li>
                            <li class="link">
                                <a href="classes/ControllaMassimali_QdC.html" data-type="entity-link" >ControllaMassimali_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/Controllo.html" data-type="entity-link" >Controllo</a>
                            </li>
                            <li class="link">
                                <a href="classes/Controllo_Inserimento_Dose_Prodotto.html" data-type="entity-link" >Controllo_Inserimento_Dose_Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="classes/Controllo_Sportello.html" data-type="entity-link" >Controllo_Sportello</a>
                            </li>
                            <li class="link">
                                <a href="classes/CooperativeKendoServerResult.html" data-type="entity-link" >CooperativeKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Copertura.html" data-type="entity-link" >Copertura</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoperturaxSpecie.html" data-type="entity-link" >CoperturaxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/CopiaImpostazioniObj.html" data-type="entity-link" >CopiaImpostazioniObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/CopiaOperazioneResult.html" data-type="entity-link" >CopiaOperazioneResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/CopiaSpostaAppezzamenti.html" data-type="entity-link" >CopiaSpostaAppezzamenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/CopyProfileObj.html" data-type="entity-link" >CopyProfileObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoreWS_Generic.html" data-type="entity-link" >CoreWS_Generic</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoreWS_GenericObjP.html" data-type="entity-link" >CoreWS_GenericObjP</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoreWS_GenericPayload.html" data-type="entity-link" >CoreWS_GenericPayload</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoreWS_Gis.html" data-type="entity-link" >CoreWS_Gis</a>
                            </li>
                            <li class="link">
                                <a href="classes/CoreWSRequest.html" data-type="entity-link" >CoreWSRequest</a>
                            </li>
                            <li class="link">
                                <a href="classes/CostiAccessori.html" data-type="entity-link" >CostiAccessori</a>
                            </li>
                            <li class="link">
                                <a href="classes/CostoUnitario.html" data-type="entity-link" >CostoUnitario</a>
                            </li>
                            <li class="link">
                                <a href="classes/CostoUnitarioChiave.html" data-type="entity-link" >CostoUnitarioChiave</a>
                            </li>
                            <li class="link">
                                <a href="classes/CostsCrop.html" data-type="entity-link" >CostsCrop</a>
                            </li>
                            <li class="link">
                                <a href="classes/CostsRevenuesTot.html" data-type="entity-link" >CostsRevenuesTot</a>
                            </li>
                            <li class="link">
                                <a href="classes/Countries_IN.html" data-type="entity-link" >Countries_IN</a>
                            </li>
                            <li class="link">
                                <a href="classes/Crea_Prodotti.html" data-type="entity-link" >Crea_Prodotti</a>
                            </li>
                            <li class="link">
                                <a href="classes/CreateRecipeData.html" data-type="entity-link" >CreateRecipeData</a>
                            </li>
                            <li class="link">
                                <a href="classes/Cultivar.html" data-type="entity-link" >Cultivar</a>
                            </li>
                            <li class="link">
                                <a href="classes/CultivarxSpecie.html" data-type="entity-link" >CultivarxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/Data.html" data-type="entity-link" >Data</a>
                            </li>
                            <li class="link">
                                <a href="classes/Data-1.html" data-type="entity-link" >Data</a>
                            </li>
                            <li class="link">
                                <a href="classes/Data-2.html" data-type="entity-link" >Data</a>
                            </li>
                            <li class="link">
                                <a href="classes/DataCarenzaRaccolta_x_Impianto.html" data-type="entity-link" >DataCarenzaRaccolta_x_Impianto</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiCatastaliServerResult.html" data-type="entity-link" >DatiCatastaliServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiFeatureConAttributi.html" data-type="entity-link" >DatiFeatureConAttributi</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiModificaStandard.html" data-type="entity-link" >DatiModificaStandard</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiPrevisionaliColture.html" data-type="entity-link" >DatiPrevisionaliColture</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiPrevisionaliColtureComplete.html" data-type="entity-link" >DatiPrevisionaliColtureComplete</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiPrevisionaliColtureKendoServerResult.html" data-type="entity-link" >DatiPrevisionaliColtureKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/DatiPrevisionaliColtureRequest.html" data-type="entity-link" >DatiPrevisionaliColtureRequest</a>
                            </li>
                            <li class="link">
                                <a href="classes/DdlFeatureTypeElement.html" data-type="entity-link" >DdlFeatureTypeElement</a>
                            </li>
                            <li class="link">
                                <a href="classes/DdlMachineFG.html" data-type="entity-link" >DdlMachineFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/DdlTipoOggettoGrafico.html" data-type="entity-link" >DdlTipoOggettoGrafico</a>
                            </li>
                            <li class="link">
                                <a href="classes/Decision.html" data-type="entity-link" >Decision</a>
                            </li>
                            <li class="link">
                                <a href="classes/DefaultDistintaProduzione.html" data-type="entity-link" >DefaultDistintaProduzione</a>
                            </li>
                            <li class="link">
                                <a href="classes/DefaultGenerale.html" data-type="entity-link" >DefaultGenerale</a>
                            </li>
                            <li class="link">
                                <a href="classes/DefaultSpecie.html" data-type="entity-link" >DefaultSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/DeselectCommand.html" data-type="entity-link" >DeselectCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/DestinazioneUso.html" data-type="entity-link" >DestinazioneUso</a>
                            </li>
                            <li class="link">
                                <a href="classes/DestinazioneUso-1.html" data-type="entity-link" >DestinazioneUso</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioFertilizzazione.html" data-type="entity-link" >DettaglioFertilizzazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioRaccolta.html" data-type="entity-link" >DettaglioRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioRegistrazione.html" data-type="entity-link" >DettaglioRegistrazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioRegistroSomministrazioni.html" data-type="entity-link" >DettaglioRegistroSomministrazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioRilievo.html" data-type="entity-link" >DettaglioRilievo</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioSemina.html" data-type="entity-link" >DettaglioSemina</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioSpecie.html" data-type="entity-link" >DettaglioSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioSpecificoModel.html" data-type="entity-link" >DettaglioSpecificoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioSpecificoServerResult.html" data-type="entity-link" >DettaglioSpecificoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioSpecificoWrapper.html" data-type="entity-link" >DettaglioSpecificoWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioTrattamento.html" data-type="entity-link" >DettaglioTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioVarietaPersonalizzato.html" data-type="entity-link" >DettaglioVarietaPersonalizzato</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettaglioVisita.html" data-type="entity-link" >DettaglioVisita</a>
                            </li>
                            <li class="link">
                                <a href="classes/DettagliProtocollo.html" data-type="entity-link" >DettagliProtocollo</a>
                            </li>
                            <li class="link">
                                <a href="classes/DialogBooleanResult.html" data-type="entity-link" >DialogBooleanResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Disciplinare.html" data-type="entity-link" >Disciplinare</a>
                            </li>
                            <li class="link">
                                <a href="classes/Ditta.html" data-type="entity-link" >Ditta</a>
                            </li>
                            <li class="link">
                                <a href="classes/Ditta-1.html" data-type="entity-link" >Ditta</a>
                            </li>
                            <li class="link">
                                <a href="classes/DittaMacchina.html" data-type="entity-link" >DittaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="classes/Documento.html" data-type="entity-link" >Documento</a>
                            </li>
                            <li class="link">
                                <a href="classes/DocumentoAllegato.html" data-type="entity-link" >DocumentoAllegato</a>
                            </li>
                            <li class="link">
                                <a href="classes/DoseConsentitaDiserbo.html" data-type="entity-link" >DoseConsentitaDiserbo</a>
                            </li>
                            <li class="link">
                                <a href="classes/DoseEtichetta.html" data-type="entity-link" >DoseEtichetta</a>
                            </li>
                            <li class="link">
                                <a href="classes/Effluente.html" data-type="entity-link" >Effluente</a>
                            </li>
                            <li class="link">
                                <a href="classes/elimina_operazione_multipla.html" data-type="entity-link" >elimina_operazione_multipla</a>
                            </li>
                            <li class="link">
                                <a href="classes/Elimina_Ricetta_Brogliaccio.html" data-type="entity-link" >Elimina_Ricetta_Brogliaccio</a>
                            </li>
                            <li class="link">
                                <a href="classes/Elimina_Rilievi_Visita.html" data-type="entity-link" >Elimina_Rilievi_Visita</a>
                            </li>
                            <li class="link">
                                <a href="classes/EnteRilascio.html" data-type="entity-link" >EnteRilascio</a>
                            </li>
                            <li class="link">
                                <a href="classes/Epoca.html" data-type="entity-link" >Epoca</a>
                            </li>
                            <li class="link">
                                <a href="classes/ErroreGias.html" data-type="entity-link" >ErroreGias</a>
                            </li>
                            <li class="link">
                                <a href="classes/ErrorMsg.html" data-type="entity-link" >ErrorMsg</a>
                            </li>
                            <li class="link">
                                <a href="classes/Esercizio.html" data-type="entity-link" >Esercizio</a>
                            </li>
                            <li class="link">
                                <a href="classes/EsercizioCDC.html" data-type="entity-link" >EsercizioCDC</a>
                            </li>
                            <li class="link">
                                <a href="classes/EsercizioRilievoCDC.html" data-type="entity-link" >EsercizioRilievoCDC</a>
                            </li>
                            <li class="link">
                                <a href="classes/EserciziRequest.html" data-type="entity-link" >EserciziRequest</a>
                            </li>
                            <li class="link">
                                <a href="classes/EserciziRequestManager.html" data-type="entity-link" >EserciziRequestManager</a>
                            </li>
                            <li class="link">
                                <a href="classes/EserciziServerResult.html" data-type="entity-link" >EserciziServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ExportQdCtoAgea.html" data-type="entity-link" >ExportQdCtoAgea</a>
                            </li>
                            <li class="link">
                                <a href="classes/ExportQdCtoAgeaPaginated.html" data-type="entity-link" >ExportQdCtoAgeaPaginated</a>
                            </li>
                            <li class="link">
                                <a href="classes/FabbricatiModel.html" data-type="entity-link" >FabbricatiModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/Fabbricato.html" data-type="entity-link" >Fabbricato</a>
                            </li>
                            <li class="link">
                                <a href="classes/FabbricatoKendoServerResult.html" data-type="entity-link" >FabbricatoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Farmaco.html" data-type="entity-link" >Farmaco</a>
                            </li>
                            <li class="link">
                                <a href="classes/FaseCicloColturale.html" data-type="entity-link" >FaseCicloColturale</a>
                            </li>
                            <li class="link">
                                <a href="classes/FaseFenologica.html" data-type="entity-link" >FaseFenologica</a>
                            </li>
                            <li class="link">
                                <a href="classes/FeatureTypeUtil.html" data-type="entity-link" >FeatureTypeUtil</a>
                            </li>
                            <li class="link">
                                <a href="classes/FilterBreadCrumb.html" data-type="entity-link" >FilterBreadCrumb</a>
                            </li>
                            <li class="link">
                                <a href="classes/FilterData.html" data-type="entity-link" >FilterData</a>
                            </li>
                            <li class="link">
                                <a href="classes/Filters.html" data-type="entity-link" >Filters</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltersConfig.html" data-type="entity-link" >FiltersConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltersConfig-1.html" data-type="entity-link" >FiltersConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltersRilieviConfig.html" data-type="entity-link" >FiltersRilieviConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroCalcoloNPK.html" data-type="entity-link" >FiltroCalcoloNPK</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroCatasto.html" data-type="entity-link" >FiltroCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroCatasto-1.html" data-type="entity-link" >FiltroCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroFinalita2.html" data-type="entity-link" >FiltroFinalita2</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroImpresa.html" data-type="entity-link" >FiltroImpresa</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroPC_Finalita_Rer.html" data-type="entity-link" >FiltroPC_Finalita_Rer</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroSpecieFinalita.html" data-type="entity-link" >FiltroSpecieFinalita</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroTemporale.html" data-type="entity-link" >FiltroTemporale</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroTemporaleAvanzato.html" data-type="entity-link" >FiltroTemporaleAvanzato</a>
                            </li>
                            <li class="link">
                                <a href="classes/FiltroValoriParametriQualitativi.html" data-type="entity-link" >FiltroValoriParametriQualitativi</a>
                            </li>
                            <li class="link">
                                <a href="classes/Finalita.html" data-type="entity-link" >Finalita</a>
                            </li>
                            <li class="link">
                                <a href="classes/FinalitaMacchina.html" data-type="entity-link" >FinalitaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="classes/FinalitaPianoConcimazione.html" data-type="entity-link" >FinalitaPianoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/FinalitaxSpecie.html" data-type="entity-link" >FinalitaxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/FinalitaxSpecie-1.html" data-type="entity-link" >FinalitaxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/FixedLayerProperty.html" data-type="entity-link" >FixedLayerProperty</a>
                            </li>
                            <li class="link">
                                <a href="classes/FlagFioritura.html" data-type="entity-link" >FlagFioritura</a>
                            </li>
                            <li class="link">
                                <a href="classes/FlagProtetto.html" data-type="entity-link" >FlagProtetto</a>
                            </li>
                            <li class="link">
                                <a href="classes/FooterModel.html" data-type="entity-link" >FooterModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormaAllevamento.html" data-type="entity-link" >FormaAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormaAllevamentoxSpecie.html" data-type="entity-link" >FormaAllevamentoxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormaGiuridica.html" data-type="entity-link" >FormaGiuridica</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormCaricaDati.html" data-type="entity-link" >FormCaricaDati</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormCopiaVisitaConfig.html" data-type="entity-link" >FormCopiaVisitaConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormDatiIscrizioneLibroSoci.html" data-type="entity-link" >FormDatiIscrizioneLibroSoci</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormeGiuridiche.html" data-type="entity-link" >FormeGiuridiche</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriAziende.html" data-type="entity-link" >FormFiltriAziende</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriCatasto.html" data-type="entity-link" >FormFiltriCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriCentriAziendali.html" data-type="entity-link" >FormFiltriCentriAziendali</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriGIS.html" data-type="entity-link" >FormFiltriGIS</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriMovimenti.html" data-type="entity-link" >FormFiltriMovimenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriPianoColturale.html" data-type="entity-link" >FormFiltriPianoColturale</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriServizi.html" data-type="entity-link" >FormFiltriServizi</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltriTemporali.html" data-type="entity-link" >FormFiltriTemporali</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltroRicercaConfig.html" data-type="entity-link" >FormFiltroRicercaConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltroTemporale.html" data-type="entity-link" >FormFiltroTemporale</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormFiltroTemporaleBreve.html" data-type="entity-link" >FormFiltroTemporaleBreve</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormGroupValidator.html" data-type="entity-link" >FormGroupValidator</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormReportImpiegoProdottiFitosanitariConfig.html" data-type="entity-link" >FormReportImpiegoProdottiFitosanitariConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/FormReportImpiegoProdottiFitosanitariConfig-1.html" data-type="entity-link" >FormReportImpiegoProdottiFitosanitariConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/Genere.html" data-type="entity-link" >Genere</a>
                            </li>
                            <li class="link">
                                <a href="classes/GenericCodeDescr.html" data-type="entity-link" >GenericCodeDescr</a>
                            </li>
                            <li class="link">
                                <a href="classes/GenericCodeValue.html" data-type="entity-link" >GenericCodeValue</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJson_Feature_New.html" data-type="entity-link" >GeoJson_Feature_New</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJson_Geometry_New.html" data-type="entity-link" >GeoJson_Geometry_New</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJson_New.html" data-type="entity-link" >GeoJson_New</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJson_Shape_New.html" data-type="entity-link" >GeoJson_Shape_New</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJSONAgroGisProp.html" data-type="entity-link" >GeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJSONAgroGisPropTreeNode.html" data-type="entity-link" >GeoJSONAgroGisPropTreeNode</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJsonFilterServiceParam.html" data-type="entity-link" >GeoJsonFilterServiceParam</a>
                            </li>
                            <li class="link">
                                <a href="classes/GeoJsonUtils.html" data-type="entity-link" >GeoJsonUtils</a>
                            </li>
                            <li class="link">
                                <a href="classes/GestioneMagazziniQS.html" data-type="entity-link" >GestioneMagazziniQS</a>
                            </li>
                            <li class="link">
                                <a href="classes/GetProvince.html" data-type="entity-link" >GetProvince</a>
                            </li>
                            <li class="link">
                                <a href="classes/GHGFiltroRicerca.html" data-type="entity-link" >GHGFiltroRicerca</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiacenzeXProdotto.html" data-type="entity-link" >GiacenzeXProdotto</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasBaseDraw.html" data-type="entity-link" >GiasBaseDraw</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasCentriDDLItem.html" data-type="entity-link" >GiasCentriDDLItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasCircle.html" data-type="entity-link" >GiasCircle</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasCluster.html" data-type="entity-link" >GiasCluster</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasClusterStats.html" data-type="entity-link" >GiasClusterStats</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasDefaultRenderer.html" data-type="entity-link" >GiasDefaultRenderer</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasInfoWindow.html" data-type="entity-link" >GiasInfoWindow</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasMarker.html" data-type="entity-link" >GiasMarker</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasPolygon.html" data-type="entity-link" >GiasPolygon</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasPolyline.html" data-type="entity-link" >GiasPolyline</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasRagioneSocialeDDLItem.html" data-type="entity-link" >GiasRagioneSocialeDDLItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasRagioneSocialeDDLItem-1.html" data-type="entity-link" >GiasRagioneSocialeDDLItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasRectangle.html" data-type="entity-link" >GiasRectangle</a>
                            </li>
                            <li class="link">
                                <a href="classes/GiasValutazioniDDLItem.html" data-type="entity-link" >GiasValutazioniDDLItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiExportGridModel.html" data-type="entity-link" >GISAttributiExportGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiExportResult.html" data-type="entity-link" >GISAttributiExportResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiKendoServerResult.html" data-type="entity-link" >GISAttributiKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiMuzGridResult.html" data-type="entity-link" >GISAttributiMuzGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiRasterMasksGridModel.html" data-type="entity-link" >GISAttributiRasterMasksGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiRasterMasksPermissionsGrid.html" data-type="entity-link" >GISAttributiRasterMasksPermissionsGrid</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISAttributiRasterMasksResult.html" data-type="entity-link" >GISAttributiRasterMasksResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISBookmarksWindowGridModel.html" data-type="entity-link" >GISBookmarksWindowGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISBookmarksWindowResult.html" data-type="entity-link" >GISBookmarksWindowResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisCfgProiezioniConfigModel.html" data-type="entity-link" >GisCfgProiezioniConfigModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisCfgProiezioniConfigServerResult.html" data-type="entity-link" >GisCfgProiezioniConfigServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISCfgProiezioniGridModel.html" data-type="entity-link" >GISCfgProiezioniGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISCfgProiezioniPermissionsGrid.html" data-type="entity-link" >GISCfgProiezioniPermissionsGrid</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISCfgProiezioniResult.html" data-type="entity-link" >GISCfgProiezioniResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisDataReadParam.html" data-type="entity-link" >GisDataReadParam</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisDataReadRval_New.html" data-type="entity-link" >GisDataReadRval_New</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISEditMuzParticelleCatastaliGridResult.html" data-type="entity-link" >GISEditMuzParticelleCatastaliGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisFeaturesUtils.html" data-type="entity-link" >GisFeaturesUtils</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisFixedLayerPropertyWindowArgs.html" data-type="entity-link" >GisFixedLayerPropertyWindowArgs</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisLayerColorPickerWindowArgs.html" data-type="entity-link" >GisLayerColorPickerWindowArgs</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISPanelQdC.html" data-type="entity-link" >GISPanelQdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISParticelleCatastaliGridModel.html" data-type="entity-link" >GISParticelleCatastaliGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISParticelleCatastaliGridResult.html" data-type="entity-link" >GISParticelleCatastaliGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISParticelleCatastaliMuzGridModel.html" data-type="entity-link" >GISParticelleCatastaliMuzGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GisTempFilterParams.html" data-type="entity-link" >GisTempFilterParams</a>
                            </li>
                            <li class="link">
                                <a href="classes/GISUtility.html" data-type="entity-link" >GISUtility</a>
                            </li>
                            <li class="link">
                                <a href="classes/GoogleMapUtils.html" data-type="entity-link" >GoogleMapUtils</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridAddMacchineServerResult.html" data-type="entity-link" >GridAddMacchineServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridAddOperaiServerResult.html" data-type="entity-link" >GridAddOperaiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridAnalisiTerrenoServerResult.html" data-type="entity-link" >GridAnalisiTerrenoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridAziendeImpostazioniServerResult.html" data-type="entity-link" >GridAziendeImpostazioniServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridCapiAnimaliServerResult.html" data-type="entity-link" >GridCapiAnimaliServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridCategorieMagazzinoServerResult.html" data-type="entity-link" >GridCategorieMagazzinoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridCommandItem.html" data-type="entity-link" >GridCommandItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridDataDetail.html" data-type="entity-link" >GridDataDetail</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridDetailInfo.html" data-type="entity-link" >GridDetailInfo</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridDosiProdotti_ddl_UdM.html" data-type="entity-link" >GridDosiProdotti_ddl_UdM</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridDosiProdottiServerResult.html" data-type="entity-link" >GridDosiProdottiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridDosiProdottiValidator.html" data-type="entity-link" >GridDosiProdottiValidator</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridEntitaServerResult.html" data-type="entity-link" >GridEntitaServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridFiltroRicercaServerResult.html" data-type="entity-link" >GridFiltroRicercaServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridGruppiServerResult.html" data-type="entity-link" >GridGruppiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridImpiantiServerResult.html" data-type="entity-link" >GridImpiantiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridImpiantiValidator.html" data-type="entity-link" >GridImpiantiValidator</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridImpiantoSelezionatoModel.html" data-type="entity-link" >GridImpiantoSelezionatoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridMacchinaModel.html" data-type="entity-link" >GridMacchinaModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridMacchineServerResult.html" data-type="entity-link" >GridMacchineServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridMassiveEvent.html" data-type="entity-link" >GridMassiveEvent</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridMasterServerResult.html" data-type="entity-link" >GridMasterServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridNoteServerResult.html" data-type="entity-link" >GridNoteServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridOpEditServerRsult.html" data-type="entity-link" >GridOpEditServerRsult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridOperatoreModel.html" data-type="entity-link" >GridOperatoreModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridOperatoriServerResult.html" data-type="entity-link" >GridOperatoriServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridOperazioneCausaleServerResult.html" data-type="entity-link" >GridOperazioneCausaleServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridParametriAnalisiTerrenoServerResult.html" data-type="entity-link" >GridParametriAnalisiTerrenoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridPermessiEditServerResult.html" data-type="entity-link" >GridPermessiEditServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridPermessiServerResult.html" data-type="entity-link" >GridPermessiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridProdottiDaTrattareServerResult.html" data-type="entity-link" >GridProdottiDaTrattareServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridProdottiSomministrazioneServerResult.html" data-type="entity-link" >GridProdottiSomministrazioneServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridProfilazioneServerResult.html" data-type="entity-link" >GridProfilazioneServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridProfiliPermessiServerResult.html" data-type="entity-link" >GridProfiliPermessiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridProfiliServerResult.html" data-type="entity-link" >GridProfiliServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRaccoltaAutoServerResult.html" data-type="entity-link" >GridRaccoltaAutoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRaccoltaModel.html" data-type="entity-link" >GridRaccoltaModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRaccoltaObject.html" data-type="entity-link" >GridRaccoltaObject</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridReportFitoServerResult.html" data-type="entity-link" >GridReportFitoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRicetteResult.html" data-type="entity-link" >GridRicetteResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRilieviModel.html" data-type="entity-link" >GridRilieviModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRilieviObject.html" data-type="entity-link" >GridRilieviObject</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRilieviServerResult.html" data-type="entity-link" >GridRilieviServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRilieviServerResult-1.html" data-type="entity-link" >GridRilieviServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridRipManualeServerResult.html" data-type="entity-link" >GridRipManualeServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridSpecieVarietaServerResult.html" data-type="entity-link" >GridSpecieVarietaServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridUtentiPermessiServerResult.html" data-type="entity-link" >GridUtentiPermessiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridVarietaServerResult.html" data-type="entity-link" >GridVarietaServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridVisiteServerResult.html" data-type="entity-link" >GridVisiteServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GridZooResult.html" data-type="entity-link" >GridZooResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GroundSlopeFG.html" data-type="entity-link" >GroundSlopeFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceGridModel.html" data-type="entity-link" >GruppiMerceGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceGridModel-1.html" data-type="entity-link" >GruppiMerceGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceGridRow.html" data-type="entity-link" >GruppiMerceGridRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceGridRow-1.html" data-type="entity-link" >GruppiMerceGridRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceServerResult.html" data-type="entity-link" >GruppiMerceServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiMerceServerResult-1.html" data-type="entity-link" >GruppiMerceServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiPanelCtrl.html" data-type="entity-link" >GruppiPanelCtrl</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiRaccoltaKendoServerResult.html" data-type="entity-link" >GruppiRaccoltaKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentiGridModel.html" data-type="entity-link" >GruppiUtentiGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentiGridRow.html" data-type="entity-link" >GruppiUtentiGridRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentiServerResult.html" data-type="entity-link" >GruppiUtentiServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentixGruppiMerceGridModel.html" data-type="entity-link" >GruppiUtentixGruppiMerceGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentixGruppiMerceGridRow.html" data-type="entity-link" >GruppiUtentixGruppiMerceGridRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppiUtentixGruppiMerceServerResult.html" data-type="entity-link" >GruppiUtentixGruppiMerceServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoAvversita.html" data-type="entity-link" >GruppoAvversita</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoFinalita.html" data-type="entity-link" >GruppoFinalita</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoNota.html" data-type="entity-link" >GruppoNota</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoNoteModel.html" data-type="entity-link" >GruppoNoteModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoRaccolta.html" data-type="entity-link" >GruppoRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoUtente.html" data-type="entity-link" >GruppoUtente</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoVarietale.html" data-type="entity-link" >GruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="classes/GruppoVarietalexSpecie.html" data-type="entity-link" >GruppoVarietalexSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/HeaderModel.html" data-type="entity-link" >HeaderModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/HeaderModel-1.html" data-type="entity-link" >HeaderModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/HubIoTPlatformDestination.html" data-type="entity-link" >HubIoTPlatformDestination</a>
                            </li>
                            <li class="link">
                                <a href="classes/Immagine.html" data-type="entity-link" >Immagine</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpegniAggiuntiviFacoltativi.html" data-type="entity-link" >ImpegniAggiuntiviFacoltativi</a>
                            </li>
                            <li class="link">
                                <a href="classes/Impianti.html" data-type="entity-link" >Impianti</a>
                            </li>
                            <li class="link">
                                <a href="classes/Impianto.html" data-type="entity-link" >Impianto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpostazioniApp.html" data-type="entity-link" >ImpostazioniApp</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpostazioniApp-1.html" data-type="entity-link" >ImpostazioniApp</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpostazioniGIS.html" data-type="entity-link" >ImpostazioniGIS</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpostazioniTema.html" data-type="entity-link" >ImpostazioniTema</a>
                            </li>
                            <li class="link">
                                <a href="classes/Impresa.html" data-type="entity-link" >Impresa</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpresaCodiciTemplateResult.html" data-type="entity-link" >ImpresaCodiciTemplateResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpresaDTO.html" data-type="entity-link" >ImpresaDTO</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpresaKendoServerResult.html" data-type="entity-link" >ImpresaKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpresaModel.html" data-type="entity-link" >ImpresaModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpresaPadre.html" data-type="entity-link" >ImpresaPadre</a>
                            </li>
                            <li class="link">
                                <a href="classes/Imprese_Impostazioni.html" data-type="entity-link" >Imprese_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseDDLCtrlObj.html" data-type="entity-link" >ImpreseDDLCtrlObj</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseFactoryService.html" data-type="entity-link" >ImpreseFactoryService</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseParametriGHGForm.html" data-type="entity-link" >ImpreseParametriGHGForm</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseParametriGHGKendo.html" data-type="entity-link" >ImpreseParametriGHGKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseParametriGHGKendoServerResult.html" data-type="entity-link" >ImpreseParametriGHGKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ImpreseParametriGHGServerResult.html" data-type="entity-link" >ImpreseParametriGHGServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndiceRilievo.html" data-type="entity-link" >IndiceRilievo</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizziAppezzamentoModel.html" data-type="entity-link" >IndirizziAppezzamentoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizziServerResult.html" data-type="entity-link" >IndirizziServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Indirizzo.html" data-type="entity-link" >Indirizzo</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizzoAssociato.html" data-type="entity-link" >IndirizzoAssociato</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizzoAssociatoChiave.html" data-type="entity-link" >IndirizzoAssociatoChiave</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizzoDefault.html" data-type="entity-link" >IndirizzoDefault</a>
                            </li>
                            <li class="link">
                                <a href="classes/IndirizzoProduttivo.html" data-type="entity-link" >IndirizzoProduttivo</a>
                            </li>
                            <li class="link">
                                <a href="classes/InfomodificaOperazioneSingola.html" data-type="entity-link" >InfomodificaOperazioneSingola</a>
                            </li>
                            <li class="link">
                                <a href="classes/Inizializza_QdC.html" data-type="entity-link" >Inizializza_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/IntervalloTemporale.html" data-type="entity-link" >IntervalloTemporale</a>
                            </li>
                            <li class="link">
                                <a href="classes/InvestimentoCatastaleAppezzamentoKendoServerResult.html" data-type="entity-link" >InvestimentoCatastaleAppezzamentoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/InvestimentoCatastoCampoKendoServerResult.html" data-type="entity-link" >InvestimentoCatastoCampoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/InvestimentoCatastoKendoServerResult.html" data-type="entity-link" >InvestimentoCatastoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/InvestomentoCatastaleAppezzamentoKendoModel.html" data-type="entity-link" >InvestomentoCatastaleAppezzamentoKendoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/InvisibleRenderer.html" data-type="entity-link" >InvisibleRenderer</a>
                            </li>
                            <li class="link">
                                <a href="classes/Irrigazione.html" data-type="entity-link" >Irrigazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/IrrigazionexSpecie.html" data-type="entity-link" >IrrigazionexSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/Istat.html" data-type="entity-link" >Istat</a>
                            </li>
                            <li class="link">
                                <a href="classes/ISTAT_GetCAP_Request.html" data-type="entity-link" >ISTAT_GetCAP_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/ItemSelector.html" data-type="entity-link" >ItemSelector</a>
                            </li>
                            <li class="link">
                                <a href="classes/Job.html" data-type="entity-link" >Job</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoAnniRow.html" data-type="entity-link" >KendoAnniRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoAppezzamentiCampoModel.html" data-type="entity-link" >KendoAppezzamentiCampoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCampiModel.html" data-type="entity-link" >KendoCampiModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCatastoCampoModel.html" data-type="entity-link" >KendoCatastoCampoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCatastoModel.html" data-type="entity-link" >KendoCatastoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCatastoRow.html" data-type="entity-link" >KendoCatastoRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCentroModel.html" data-type="entity-link" >KendoCentroModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCentroRow.html" data-type="entity-link" >KendoCentroRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoContattiModel.html" data-type="entity-link" >KendoContattiModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoCostiMacchinaModel.html" data-type="entity-link" >KendoCostiMacchinaModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoDatiPrevisionaliColtureModel.html" data-type="entity-link" >KendoDatiPrevisionaliColtureModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoEserciziModel.html" data-type="entity-link" >KendoEserciziModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoFabbricatoRow.html" data-type="entity-link" >KendoFabbricatoRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoGISAppuntiModel.html" data-type="entity-link" >KendoGISAppuntiModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoGruppiRaccoltaModel.html" data-type="entity-link" >KendoGruppiRaccoltaModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoImpresaRow.html" data-type="entity-link" >KendoImpresaRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoInvestimentoCatastoCampoModel.html" data-type="entity-link" >KendoInvestimentoCatastoCampoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoInvestimentoCatastoModel.html" data-type="entity-link" >KendoInvestimentoCatastoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoMacchineModel.html" data-type="entity-link" >KendoMacchineModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/KendoServerResultImpl.html" data-type="entity-link" >KendoServerResultImpl</a>
                            </li>
                            <li class="link">
                                <a href="classes/KeyValuePair.html" data-type="entity-link" >KeyValuePair</a>
                            </li>
                            <li class="link">
                                <a href="classes/LatLng.html" data-type="entity-link" >LatLng</a>
                            </li>
                            <li class="link">
                                <a href="classes/Lavorazione.html" data-type="entity-link" >Lavorazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_AppezzamentiCampo_Request.html" data-type="entity-link" >Leggi_AppezzamentiCampo_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Campi_SpecieVegetali_Request.html" data-type="entity-link" >Leggi_Campi_SpecieVegetali_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_FasiCicloColturalexSpecie.html" data-type="entity-link" >Leggi_FasiCicloColturalexSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Forme_Giuridiche_Request.html" data-type="entity-link" >Leggi_Forme_Giuridiche_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Impostazioni.html" data-type="entity-link" >Leggi_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Imprese_Codici_Request.html" data-type="entity-link" >Leggi_Imprese_Codici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Imprese_Codici_Request-1.html" data-type="entity-link" >Leggi_Imprese_Codici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Imprese_Codici_Request-2.html" data-type="entity-link" >Leggi_Imprese_Codici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Macchine_CmbTipo_Request.html" data-type="entity-link" >Leggi_Macchine_CmbTipo_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Macchine_Ditte_Request.html" data-type="entity-link" >Leggi_Macchine_Ditte_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Modalita_Applicazione.html" data-type="entity-link" >Leggi_Modalita_Applicazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Odc_Request.html" data-type="entity-link" >Leggi_Odc_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Odc_Request-1.html" data-type="entity-link" >Leggi_Odc_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Odc_Request-2.html" data-type="entity-link" >Leggi_Odc_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Padri.html" data-type="entity-link" >Leggi_Padri</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Padri-1.html" data-type="entity-link" >Leggi_Padri</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Padri-2.html" data-type="entity-link" >Leggi_Padri</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_ParticelleCampo_Request.html" data-type="entity-link" >Leggi_ParticelleCampo_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Tecnici_Request.html" data-type="entity-link" >Leggi_Tecnici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Tecnici_Request-1.html" data-type="entity-link" >Leggi_Tecnici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/Leggi_Tecnici_Request-2.html" data-type="entity-link" >Leggi_Tecnici_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiAppezzamento.html" data-type="entity-link" >LeggiAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiAppezzamento-1.html" data-type="entity-link" >LeggiAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiAttivitaPersonalizzata.html" data-type="entity-link" >LeggiAttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiAvversita.html" data-type="entity-link" >LeggiAvversita</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiAziende.html" data-type="entity-link" >LeggiAziende</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCAC_Codifica_InfoAggiuntive.html" data-type="entity-link" >LeggiCAC_Codifica_InfoAggiuntive</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCampi.html" data-type="entity-link" >LeggiCampi</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCampi-1.html" data-type="entity-link" >LeggiCampi</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCampi-2.html" data-type="entity-link" >LeggiCampi</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCentriAziendali.html" data-type="entity-link" >LeggiCentriAziendali</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiClasseTessitura.html" data-type="entity-link" >LeggiClasseTessitura</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiConduzione.html" data-type="entity-link" >LeggiConduzione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiContattiMacchine.html" data-type="entity-link" >LeggiContattiMacchine</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCopertura.html" data-type="entity-link" >LeggiCopertura</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiCultivar.html" data-type="entity-link" >LeggiCultivar</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDatiRilievi.html" data-type="entity-link" >LeggiDatiRilievi</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDefault_DPI_QdC.html" data-type="entity-link" >LeggiDefault_DPI_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDestinazioniUso.html" data-type="entity-link" >LeggiDestinazioniUso</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDettaglio.html" data-type="entity-link" >LeggiDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDettaglio-1.html" data-type="entity-link" >LeggiDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDisciplinare.html" data-type="entity-link" >LeggiDisciplinare</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDisciplinari.html" data-type="entity-link" >LeggiDisciplinari</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDisponibilitaAttualeFertilizzante.html" data-type="entity-link" >LeggiDisponibilitaAttualeFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDitteTrappole.html" data-type="entity-link" >LeggiDitteTrappole</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDoseConsentitaDiserbo.html" data-type="entity-link" >LeggiDoseConsentitaDiserbo</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiDosiEtichetta.html" data-type="entity-link" >LeggiDosiEtichetta</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiEfficienza.html" data-type="entity-link" >LeggiEfficienza</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiEpoche.html" data-type="entity-link" >LeggiEpoche</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiFiltro.html" data-type="entity-link" >LeggiFiltro</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiFiltro-1.html" data-type="entity-link" >LeggiFiltro</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiFinalita.html" data-type="entity-link" >LeggiFinalita</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiFormaAllevamento.html" data-type="entity-link" >LeggiFormaAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiGriglia.html" data-type="entity-link" >LeggiGriglia</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiGruppoVarietale.html" data-type="entity-link" >LeggiGruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiIAF.html" data-type="entity-link" >LeggiIAF</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiImpianto.html" data-type="entity-link" >LeggiImpianto</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiImpostazioni_AziendeCentri.html" data-type="entity-link" >LeggiImpostazioni_AziendeCentri</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiIndirizziCentro.html" data-type="entity-link" >LeggiIndirizziCentro</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiInizializza_QdC.html" data-type="entity-link" >LeggiInizializza_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiInvestimentoCatastale.html" data-type="entity-link" >LeggiInvestimentoCatastale</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiInvestimentoCatastaleCampo.html" data-type="entity-link" >LeggiInvestimentoCatastaleCampo</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiIrrigazione.html" data-type="entity-link" >LeggiIrrigazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiLink_Operazione.html" data-type="entity-link" >LeggiLink_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiListaAnalisiTerreno.html" data-type="entity-link" >LeggiListaAnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiLocalizzazioni.html" data-type="entity-link" >LeggiLocalizzazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiMagazzini_QdC.html" data-type="entity-link" >LeggiMagazzini_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiMenuRicette.html" data-type="entity-link" >LeggiMenuRicette</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiMisuraPerAvversitaAnagrafica_In.html" data-type="entity-link" >LeggiMisuraPerAvversitaAnagrafica_In</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiMisureAvversita.html" data-type="entity-link" >LeggiMisureAvversita</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiNote.html" data-type="entity-link" >LeggiNote</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiOperazioni.html" data-type="entity-link" >LeggiOperazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPianoConti.html" data-type="entity-link" >LeggiPianoConti</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPianoConti-1.html" data-type="entity-link" >LeggiPianoConti</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPianoContixConti.html" data-type="entity-link" >LeggiPianoContixConti</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPortinnesto.html" data-type="entity-link" >LeggiPortinnesto</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPrescrizioni.html" data-type="entity-link" >LeggiPrescrizioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPrescrizioniIndicazioni.html" data-type="entity-link" >LeggiPrescrizioniIndicazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPrescrizioniProtocolli.html" data-type="entity-link" >LeggiPrescrizioniProtocolli</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPrescrizioniVeterinarie.html" data-type="entity-link" >LeggiPrescrizioniVeterinarie</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiProdotti.html" data-type="entity-link" >LeggiProdotti</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiProfilazione.html" data-type="entity-link" >LeggiProfilazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiPUA.html" data-type="entity-link" >LeggiPUA</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiRilieviProduzione.html" data-type="entity-link" >LeggiRilieviProduzione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiScriviVisibilitaUtenti.html" data-type="entity-link" >LeggiScriviVisibilitaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiSezioni.html" data-type="entity-link" >LeggiSezioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiSpecie.html" data-type="entity-link" >LeggiSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiTestata.html" data-type="entity-link" >LeggiTestata</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiUltimo_Magazzino_Prodotto_Movimentato.html" data-type="entity-link" >LeggiUltimo_Magazzino_Prodotto_Movimentato</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiUnitaDiMisura.html" data-type="entity-link" >LeggiUnitaDiMisura</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiVincoli.html" data-type="entity-link" >LeggiVincoli</a>
                            </li>
                            <li class="link">
                                <a href="classes/LeggiVisite.html" data-type="entity-link" >LeggiVisite</a>
                            </li>
                            <li class="link">
                                <a href="classes/LetturaContatoriGridItem.html" data-type="entity-link" >LetturaContatoriGridItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/LetturaContatoriGridItemForm.html" data-type="entity-link" >LetturaContatoriGridItemForm</a>
                            </li>
                            <li class="link">
                                <a href="classes/LetturaContatoriGridItemModel.html" data-type="entity-link" >LetturaContatoriGridItemModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/LetturaContatoriModel.html" data-type="entity-link" >LetturaContatoriModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/LettureContatoriFilters.html" data-type="entity-link" >LettureContatoriFilters</a>
                            </li>
                            <li class="link">
                                <a href="classes/LicenzaColtivazione.html" data-type="entity-link" >LicenzaColtivazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Link_Operazione.html" data-type="entity-link" >Link_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/LinkedContribute.html" data-type="entity-link" >LinkedContribute</a>
                            </li>
                            <li class="link">
                                <a href="classes/LinkedMachine.html" data-type="entity-link" >LinkedMachine</a>
                            </li>
                            <li class="link">
                                <a href="classes/LinkMenu.html" data-type="entity-link" >LinkMenu</a>
                            </li>
                            <li class="link">
                                <a href="classes/LoadingObject.html" data-type="entity-link" >LoadingObject</a>
                            </li>
                            <li class="link">
                                <a href="classes/Localizzazione.html" data-type="entity-link" >Localizzazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Macchina.html" data-type="entity-link" >Macchina</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacchinaGerarchia.html" data-type="entity-link" >MacchinaGerarchia</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacchinaKendoServerResult.html" data-type="entity-link" >MacchinaKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Macchine.html" data-type="entity-link" >Macchine</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacchineCodificaAgea.html" data-type="entity-link" >MacchineCodificaAgea</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacchineDettaglio1.html" data-type="entity-link" >MacchineDettaglio1</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacchineDettaglio2.html" data-type="entity-link" >MacchineDettaglio2</a>
                            </li>
                            <li class="link">
                                <a href="classes/MachinesXTypeReadParams.html" data-type="entity-link" >MachinesXTypeReadParams</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacrousiCatastoAppezzamento.html" data-type="entity-link" >MacrousiCatastoAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/MacrousiKendo.html" data-type="entity-link" >MacrousiKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/Macrouso.html" data-type="entity-link" >Macrouso</a>
                            </li>
                            <li class="link">
                                <a href="classes/MagazzinoConferimentoImpresa.html" data-type="entity-link" >MagazzinoConferimentoImpresa</a>
                            </li>
                            <li class="link">
                                <a href="classes/MapGridOverlay.html" data-type="entity-link" >MapGridOverlay</a>
                            </li>
                            <li class="link">
                                <a href="classes/Marca.html" data-type="entity-link" >Marca</a>
                            </li>
                            <li class="link">
                                <a href="classes/Marker.html" data-type="entity-link" >Marker</a>
                            </li>
                            <li class="link">
                                <a href="classes/MassEditMacchinaRowModel.html" data-type="entity-link" >MassEditMacchinaRowModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/MassEditOperaioRowModel.html" data-type="entity-link" >MassEditOperaioRowModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/MatomoConfig.html" data-type="entity-link" >MatomoConfig</a>
                            </li>
                            <li class="link">
                                <a href="classes/Mdi.html" data-type="entity-link" >Mdi</a>
                            </li>
                            <li class="link">
                                <a href="classes/MenuContestualeSettings.html" data-type="entity-link" >MenuContestualeSettings</a>
                            </li>
                            <li class="link">
                                <a href="classes/MenuEntry.html" data-type="entity-link" >MenuEntry</a>
                            </li>
                            <li class="link">
                                <a href="classes/MenuRicette.html" data-type="entity-link" >MenuRicette</a>
                            </li>
                            <li class="link">
                                <a href="classes/Mercator.html" data-type="entity-link" >Mercator</a>
                            </li>
                            <li class="link">
                                <a href="classes/Messaggio_Utente_Permessi.html" data-type="entity-link" >Messaggio_Utente_Permessi</a>
                            </li>
                            <li class="link">
                                <a href="classes/MessaggioFinestraInformativa.html" data-type="entity-link" >MessaggioFinestraInformativa</a>
                            </li>
                            <li class="link">
                                <a href="classes/MetodoProduzione.html" data-type="entity-link" >MetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="classes/MetodoProduzioneId.html" data-type="entity-link" >MetodoProduzioneId</a>
                            </li>
                            <li class="link">
                                <a href="classes/MetodoProduzioneKendo.html" data-type="entity-link" >MetodoProduzioneKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/MFSet.html" data-type="entity-link" >MFSet</a>
                            </li>
                            <li class="link">
                                <a href="classes/MisuraAvversita.html" data-type="entity-link" >MisuraAvversita</a>
                            </li>
                            <li class="link">
                                <a href="classes/ModificaListaRicetteQdC.html" data-type="entity-link" >ModificaListaRicetteQdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/ModificaMultipla_Operazione.html" data-type="entity-link" >ModificaMultipla_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/ModuliGias.html" data-type="entity-link" >ModuliGias</a>
                            </li>
                            <li class="link">
                                <a href="classes/MultipleImageMapType.html" data-type="entity-link" >MultipleImageMapType</a>
                            </li>
                            <li class="link">
                                <a href="classes/MultiselectContributeFG.html" data-type="entity-link" >MultiselectContributeFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/NextPositionCommand.html" data-type="entity-link" >NextPositionCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/NodeType.html" data-type="entity-link" >NodeType</a>
                            </li>
                            <li class="link">
                                <a href="classes/Nota.html" data-type="entity-link" >Nota</a>
                            </li>
                            <li class="link">
                                <a href="classes/NotaInterventoDdlItem.html" data-type="entity-link" >NotaInterventoDdlItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/NotaXGruppo.html" data-type="entity-link" >NotaXGruppo</a>
                            </li>
                            <li class="link">
                                <a href="classes/NoteIntervento.html" data-type="entity-link" >NoteIntervento</a>
                            </li>
                            <li class="link">
                                <a href="classes/NoteInterventoGruppi.html" data-type="entity-link" >NoteInterventoGruppi</a>
                            </li>
                            <li class="link">
                                <a href="classes/NoteInterventoUtilizzo.html" data-type="entity-link" >NoteInterventoUtilizzo</a>
                            </li>
                            <li class="link">
                                <a href="classes/Obj_Dose_Ha_Hl.html" data-type="entity-link" >Obj_Dose_Ha_Hl</a>
                            </li>
                            <li class="link">
                                <a href="classes/Obj_Dose_Ha_Tot.html" data-type="entity-link" >Obj_Dose_Ha_Tot</a>
                            </li>
                            <li class="link">
                                <a href="classes/Obj_Dose_Hl_Tot.html" data-type="entity-link" >Obj_Dose_Hl_Tot</a>
                            </li>
                            <li class="link">
                                <a href="classes/Obj_Errore_Gias_QdC.html" data-type="entity-link" >Obj_Errore_Gias_QdC</a>
                            </li>
                            <li class="link">
                                <a href="classes/ObjParametriCAC.html" data-type="entity-link" >ObjParametriCAC</a>
                            </li>
                            <li class="link">
                                <a href="classes/objTreeNode.html" data-type="entity-link" >objTreeNode</a>
                            </li>
                            <li class="link">
                                <a href="classes/OpenBookmarksCommand.html" data-type="entity-link" >OpenBookmarksCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/OperationSelector.html" data-type="entity-link" >OperationSelector</a>
                            </li>
                            <li class="link">
                                <a href="classes/Operatore.html" data-type="entity-link" >Operatore</a>
                            </li>
                            <li class="link">
                                <a href="classes/OpzioniAgenda.html" data-type="entity-link" >OpzioniAgenda</a>
                            </li>
                            <li class="link">
                                <a href="classes/OpzioniRaccolta.html" data-type="entity-link" >OpzioniRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="classes/OrganismoImpresa.html" data-type="entity-link" >OrganismoImpresa</a>
                            </li>
                            <li class="link">
                                <a href="classes/OrientamentoTecnicoEconomico.html" data-type="entity-link" >OrientamentoTecnicoEconomico</a>
                            </li>
                            <li class="link">
                                <a href="classes/PaginaProfilazioneItem.html" data-type="entity-link" >PaginaProfilazioneItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/Parametri.html" data-type="entity-link" >Parametri</a>
                            </li>
                            <li class="link">
                                <a href="classes/Parametri_Aggiuntivi_Attivita.html" data-type="entity-link" >Parametri_Aggiuntivi_Attivita</a>
                            </li>
                            <li class="link">
                                <a href="classes/Parametri_Aggiuntivi_ControllaDosi.html" data-type="entity-link" >Parametri_Aggiuntivi_ControllaDosi</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParametriAggiuntivi_QueryString.html" data-type="entity-link" >ParametriAggiuntivi_QueryString</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParametriFiltroRicercaNG.html" data-type="entity-link" >ParametriFiltroRicercaNG</a>
                            </li>
                            <li class="link">
                                <a href="classes/paramLoadGeoJson.html" data-type="entity-link" >paramLoadGeoJson</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParcoMacchine.html" data-type="entity-link" >ParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastali.html" data-type="entity-link" >ParticelleCatastali</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliClassamento.html" data-type="entity-link" >ParticelleCatastaliClassamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliClassamentoId.html" data-type="entity-link" >ParticelleCatastaliClassamentoId</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliMacrouso.html" data-type="entity-link" >ParticelleCatastaliMacrouso</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliMacrousoId.html" data-type="entity-link" >ParticelleCatastaliMacrousoId</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliMetodoProduzione.html" data-type="entity-link" >ParticelleCatastaliMetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliZona.html" data-type="entity-link" >ParticelleCatastaliZona</a>
                            </li>
                            <li class="link">
                                <a href="classes/ParticelleCatastaliZonaId.html" data-type="entity-link" >ParticelleCatastaliZonaId</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessiFeature.html" data-type="entity-link" >PermessiFeature</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessiLayer.html" data-type="entity-link" >PermessiLayer</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessiXGruppiUtenteResult.html" data-type="entity-link" >PermessiXGruppiUtenteResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessiXUtenteResult.html" data-type="entity-link" >PermessiXUtenteResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessoConfigurazioneResult.html" data-type="entity-link" >PermessoConfigurazioneResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PermessoConfigurazioneResult-1.html" data-type="entity-link" >PermessoConfigurazioneResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoColturale.html" data-type="entity-link" >PianoColturale</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoColturaleKendoModel.html" data-type="entity-link" >PianoColturaleKendoModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoColturaleKendoServerResult.html" data-type="entity-link" >PianoColturaleKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoConti.html" data-type="entity-link" >PianoConti</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoContiKendoServerResult.html" data-type="entity-link" >PianoContiKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoContiModel.html" data-type="entity-link" >PianoContiModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoContiWrapper.html" data-type="entity-link" >PianoContiWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/PianoContixTree.html" data-type="entity-link" >PianoContixTree</a>
                            </li>
                            <li class="link">
                                <a href="classes/PK.html" data-type="entity-link" >PK</a>
                            </li>
                            <li class="link">
                                <a href="classes/PolygonInterceptionMapType.html" data-type="entity-link" >PolygonInterceptionMapType</a>
                            </li>
                            <li class="link">
                                <a href="classes/PolygonInterceptionUtils.html" data-type="entity-link" >PolygonInterceptionUtils</a>
                            </li>
                            <li class="link">
                                <a href="classes/Portinnesto.html" data-type="entity-link" >Portinnesto</a>
                            </li>
                            <li class="link">
                                <a href="classes/PortinnestoxSpecie.html" data-type="entity-link" >PortinnestoxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/PossessiParticelleKendo.html" data-type="entity-link" >PossessiParticelleKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/PossessoParticella.html" data-type="entity-link" >PossessoParticella</a>
                            </li>
                            <li class="link">
                                <a href="classes/PossessoParticellaId.html" data-type="entity-link" >PossessoParticellaId</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrejectionPermissionGroup.html" data-type="entity-link" >PrejectionPermissionGroup</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrejectionPermissionGroup-1.html" data-type="entity-link" >PrejectionPermissionGroup</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrejectionPermissionUser.html" data-type="entity-link" >PrejectionPermissionUser</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrejectionPermissionUser-1.html" data-type="entity-link" >PrejectionPermissionUser</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrescriptionActivity.html" data-type="entity-link" >PrescriptionActivity</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrevPositionCommand.html" data-type="entity-link" >PrevPositionCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/PrincipioAttivo.html" data-type="entity-link" >PrincipioAttivo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProdottiDaTrattare.html" data-type="entity-link" >ProdottiDaTrattare</a>
                            </li>
                            <li class="link">
                                <a href="classes/Prodotto.html" data-type="entity-link" >Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProdottoDaTrattareCDC.html" data-type="entity-link" >ProdottoDaTrattareCDC</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseDefaultDistintaProduzioneGridModel.html" data-type="entity-link" >ProfilazioneImpreseDefaultDistintaProduzioneGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseDefaultDistintaProduzioneGridResult.html" data-type="entity-link" >ProfilazioneImpreseDefaultDistintaProduzioneGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseDefaultSpecieGridModel.html" data-type="entity-link" >ProfilazioneImpreseDefaultSpecieGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseDefaultSpecieGridResult.html" data-type="entity-link" >ProfilazioneImpreseDefaultSpecieGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseParametriGeneraliColturaGridModel.html" data-type="entity-link" >ProfilazioneImpreseParametriGeneraliColturaGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProfilazioneImpreseParametriGeneraliColturaGridResult.html" data-type="entity-link" >ProfilazioneImpreseParametriGeneraliColturaGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Progetto.html" data-type="entity-link" >Progetto</a>
                            </li>
                            <li class="link">
                                <a href="classes/PropertyValidator.html" data-type="entity-link" >PropertyValidator</a>
                            </li>
                            <li class="link">
                                <a href="classes/PropertyValidatorFields.html" data-type="entity-link" >PropertyValidatorFields</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProseguiSelezionati.html" data-type="entity-link" >ProseguiSelezionati</a>
                            </li>
                            <li class="link">
                                <a href="classes/ProvenienzaSeme.html" data-type="entity-link" >ProvenienzaSeme</a>
                            </li>
                            <li class="link">
                                <a href="classes/Provincia.html" data-type="entity-link" >Provincia</a>
                            </li>
                            <li class="link">
                                <a href="classes/Pua.html" data-type="entity-link" >Pua</a>
                            </li>
                            <li class="link">
                                <a href="classes/PulsanteGestioneCostiService.html" data-type="entity-link" >PulsanteGestioneCostiService</a>
                            </li>
                            <li class="link">
                                <a href="classes/QdCFormModel.html" data-type="entity-link" >QdCFormModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/QdCRow.html" data-type="entity-link" >QdCRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/QuantitaSuImpianto.html" data-type="entity-link" >QuantitaSuImpianto</a>
                            </li>
                            <li class="link">
                                <a href="classes/RaggruppamentiColturaliDPI.html" data-type="entity-link" >RaggruppamentiColturaliDPI</a>
                            </li>
                            <li class="link">
                                <a href="classes/RapportoContabile.html" data-type="entity-link" >RapportoContabile</a>
                            </li>
                            <li class="link">
                                <a href="classes/RapportoContabile-1.html" data-type="entity-link" >RapportoContabile</a>
                            </li>
                            <li class="link">
                                <a href="classes/ReadBudgetTestataParams.html" data-type="entity-link" >ReadBudgetTestataParams</a>
                            </li>
                            <li class="link">
                                <a href="classes/ReadDataFilters.html" data-type="entity-link" >ReadDataFilters</a>
                            </li>
                            <li class="link">
                                <a href="classes/ReadDettaglioAziendale.html" data-type="entity-link" >ReadDettaglioAziendale</a>
                            </li>
                            <li class="link">
                                <a href="classes/ReadRequisitiStabilimento.html" data-type="entity-link" >ReadRequisitiStabilimento</a>
                            </li>
                            <li class="link">
                                <a href="classes/ReadTecniciParams.html" data-type="entity-link" >ReadTecniciParams</a>
                            </li>
                            <li class="link">
                                <a href="classes/Redirect_To_GiasNG_Page.html" data-type="entity-link" >Redirect_To_GiasNG_Page</a>
                            </li>
                            <li class="link">
                                <a href="classes/RegistrazioneContabile.html" data-type="entity-link" >RegistrazioneContabile</a>
                            </li>
                            <li class="link">
                                <a href="classes/Regolamenti.html" data-type="entity-link" >Regolamenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/RegolamentoConcimazione.html" data-type="entity-link" >RegolamentoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/RemovedYears.html" data-type="entity-link" >RemovedYears</a>
                            </li>
                            <li class="link">
                                <a href="classes/RequisitiStabilimentoGridModel.html" data-type="entity-link" >RequisitiStabilimentoGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/RequisitiStabilimentoKendoServerResult.html" data-type="entity-link" >RequisitiStabilimentoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/RequisitiStabilimentoKendoServerResult-1.html" data-type="entity-link" >RequisitiStabilimentoKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/RevenuesCrop.html" data-type="entity-link" >RevenuesCrop</a>
                            </li>
                            <li class="link">
                                <a href="classes/Ricetta_Operazione.html" data-type="entity-link" >Ricetta_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/RicettaForm.html" data-type="entity-link" >RicettaForm</a>
                            </li>
                            <li class="link">
                                <a href="classes/RicettaModalParams.html" data-type="entity-link" >RicettaModalParams</a>
                            </li>
                            <li class="link">
                                <a href="classes/RicettaRow.html" data-type="entity-link" >RicettaRow</a>
                            </li>
                            <li class="link">
                                <a href="classes/RicetteServerResult.html" data-type="entity-link" >RicetteServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/RiferimentoTrasferimentoDatiImpresa.html" data-type="entity-link" >RiferimentoTrasferimentoDatiImpresa</a>
                            </li>
                            <li class="link">
                                <a href="classes/RigheSelezionate.html" data-type="entity-link" >RigheSelezionate</a>
                            </li>
                            <li class="link">
                                <a href="classes/RilevamentoDiMagazzino.html" data-type="entity-link" >RilevamentoDiMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="classes/RilieviAvversitaGridModel.html" data-type="entity-link" >RilieviAvversitaGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/RilieviAvversitaResult.html" data-type="entity-link" >RilieviAvversitaResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Risorsa.html" data-type="entity-link" >Risorsa</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaAcqua.html" data-type="entity-link" >RisorsaAcqua</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaCausale.html" data-type="entity-link" >RisorsaCausale</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaDestinazioneUso.html" data-type="entity-link" >RisorsaDestinazioneUso</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaMacchina.html" data-type="entity-link" >RisorsaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaPersona.html" data-type="entity-link" >RisorsaPersona</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaProdotto.html" data-type="entity-link" >RisorsaProdotto</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaRegistrazione.html" data-type="entity-link" >RisorsaRegistrazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaSpecie.html" data-type="entity-link" >RisorsaSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaTimeSheet.html" data-type="entity-link" >RisorsaTimeSheet</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaUmanaVisita.html" data-type="entity-link" >RisorsaUmanaVisita</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorsaZootecnica.html" data-type="entity-link" >RisorsaZootecnica</a>
                            </li>
                            <li class="link">
                                <a href="classes/RisorseUmane.html" data-type="entity-link" >RisorseUmane</a>
                            </li>
                            <li class="link">
                                <a href="classes/RispostaCore.html" data-type="entity-link" >RispostaCore</a>
                            </li>
                            <li class="link">
                                <a href="classes/RispostaStandard.html" data-type="entity-link" >RispostaStandard</a>
                            </li>
                            <li class="link">
                                <a href="classes/rispostaStandard.html" data-type="entity-link" >rispostaStandard</a>
                            </li>
                            <li class="link">
                                <a href="classes/RispostaTentativoCancellazione.html" data-type="entity-link" >RispostaTentativoCancellazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Rubrica.html" data-type="entity-link" >Rubrica</a>
                            </li>
                            <li class="link">
                                <a href="classes/RubricaKendoServerResult.html" data-type="entity-link" >RubricaKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/RubricaVoci.html" data-type="entity-link" >RubricaVoci</a>
                            </li>
                            <li class="link">
                                <a href="classes/RubricaVociConChiave.html" data-type="entity-link" >RubricaVociConChiave</a>
                            </li>
                            <li class="link">
                                <a href="classes/SalvaOperazioniPreferite.html" data-type="entity-link" >SalvaOperazioniPreferite</a>
                            </li>
                            <li class="link">
                                <a href="classes/SaveRequisitiStabilimento.html" data-type="entity-link" >SaveRequisitiStabilimento</a>
                            </li>
                            <li class="link">
                                <a href="classes/Scrivi_Utenti.html" data-type="entity-link" >Scrivi_Utenti</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviCampiAnagrafica.html" data-type="entity-link" >ScriviCampiAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviCatasto.html" data-type="entity-link" >ScriviCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviCatasto-1.html" data-type="entity-link" >ScriviCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviCatasto-2.html" data-type="entity-link" >ScriviCatasto</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviGruppoRaccolta.html" data-type="entity-link" >ScriviGruppoRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviGruppoUtente.html" data-type="entity-link" >ScriviGruppoUtente</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviGruppoUtentexTransizioniStato.html" data-type="entity-link" >ScriviGruppoUtentexTransizioniStato</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviListaAttivita.html" data-type="entity-link" >ScriviListaAttivita</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScriviPermessiResult.html" data-type="entity-link" >ScriviPermessiResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ScrivPermessiHttpDto.html" data-type="entity-link" >ScrivPermessiHttpDto</a>
                            </li>
                            <li class="link">
                                <a href="classes/SelectAreaCommand.html" data-type="entity-link" >SelectAreaCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/SelectedBudget.html" data-type="entity-link" >SelectedBudget</a>
                            </li>
                            <li class="link">
                                <a href="classes/SelectionTarget.html" data-type="entity-link" >SelectionTarget</a>
                            </li>
                            <li class="link">
                                <a href="classes/SementieriParametrizzazione.html" data-type="entity-link" >SementieriParametrizzazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/SeminaTrapianto.html" data-type="entity-link" >SeminaTrapianto</a>
                            </li>
                            <li class="link">
                                <a href="classes/SettigsAziendeCentriServerResult.html" data-type="entity-link" >SettigsAziendeCentriServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/SettigsLavorazioniServerResult.html" data-type="entity-link" >SettigsLavorazioniServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/settingsFieldData.html" data-type="entity-link" >settingsFieldData</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione.html" data-type="entity-link" >Sezione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Prodotto.html" data-type="entity-link" >Sezione_Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Prodotto_Fertilizzanti.html" data-type="entity-link" >Sezione_Prodotto_Fertilizzanti</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Prodotto_Formulati.html" data-type="entity-link" >Sezione_Prodotto_Formulati</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Prodotto_Raccolta.html" data-type="entity-link" >Sezione_Prodotto_Raccolta</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Prodotto_Sementi.html" data-type="entity-link" >Sezione_Prodotto_Sementi</a>
                            </li>
                            <li class="link">
                                <a href="classes/Sezione_Rilievi.html" data-type="entity-link" >Sezione_Rilievi</a>
                            </li>
                            <li class="link">
                                <a href="classes/SezioneImpostazioni.html" data-type="entity-link" >SezioneImpostazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/SignalOverflowRows.html" data-type="entity-link" >SignalOverflowRows</a>
                            </li>
                            <li class="link">
                                <a href="classes/Simbolo.html" data-type="entity-link" >Simbolo</a>
                            </li>
                            <li class="link">
                                <a href="classes/SistemiRiferimentoCartografia.html" data-type="entity-link" >SistemiRiferimentoCartografia</a>
                            </li>
                            <li class="link">
                                <a href="classes/Soglia.html" data-type="entity-link" >Soglia</a>
                            </li>
                            <li class="link">
                                <a href="classes/SottogruppoStalla.html" data-type="entity-link" >SottogruppoStalla</a>
                            </li>
                            <li class="link">
                                <a href="classes/SottogruppoStallaLight.html" data-type="entity-link" >SottogruppoStallaLight</a>
                            </li>
                            <li class="link">
                                <a href="classes/Specie.html" data-type="entity-link" >Specie</a>
                            </li>
                            <li class="link">
                                <a href="classes/SpecieAnimale.html" data-type="entity-link" >SpecieAnimale</a>
                            </li>
                            <li class="link">
                                <a href="classes/SpecieVegetale.html" data-type="entity-link" >SpecieVegetale</a>
                            </li>
                            <li class="link">
                                <a href="classes/Stalla.html" data-type="entity-link" >Stalla</a>
                            </li>
                            <li class="link">
                                <a href="classes/Stato.html" data-type="entity-link" >Stato</a>
                            </li>
                            <li class="link">
                                <a href="classes/StatoPatrimonialeKendoServerResult.html" data-type="entity-link" >StatoPatrimonialeKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/StatoPatrimonialeModel.html" data-type="entity-link" >StatoPatrimonialeModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/StatoPatrimonialeWrapper.html" data-type="entity-link" >StatoPatrimonialeWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/Superfici.html" data-type="entity-link" >Superfici</a>
                            </li>
                            <li class="link">
                                <a href="classes/SupTrattata_SupRiduzioneBuffer.html" data-type="entity-link" >SupTrattata_SupRiduzioneBuffer</a>
                            </li>
                            <li class="link">
                                <a href="classes/SupTrattata_x_DettaglioSemina.html" data-type="entity-link" >SupTrattata_x_DettaglioSemina</a>
                            </li>
                            <li class="link">
                                <a href="classes/SwaggerException.html" data-type="entity-link" >SwaggerException</a>
                            </li>
                            <li class="link">
                                <a href="classes/SwaggerException-1.html" data-type="entity-link" >SwaggerException</a>
                            </li>
                            <li class="link">
                                <a href="classes/SwitchClusterModeCommand.html" data-type="entity-link" >SwitchClusterModeCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/TabstripItem.html" data-type="entity-link" >TabstripItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/TagliatoIntero.html" data-type="entity-link" >TagliatoIntero</a>
                            </li>
                            <li class="link">
                                <a href="classes/TecnicaConduzioneSuFila.html" data-type="entity-link" >TecnicaConduzioneSuFila</a>
                            </li>
                            <li class="link">
                                <a href="classes/TecnicaConduzioneTraFila.html" data-type="entity-link" >TecnicaConduzioneTraFila</a>
                            </li>
                            <li class="link">
                                <a href="classes/TempiSospensione.html" data-type="entity-link" >TempiSospensione</a>
                            </li>
                            <li class="link">
                                <a href="classes/Testata.html" data-type="entity-link" >Testata</a>
                            </li>
                            <li class="link">
                                <a href="classes/Testata_Visita.html" data-type="entity-link" >Testata_Visita</a>
                            </li>
                            <li class="link">
                                <a href="classes/TestataRicetta.html" data-type="entity-link" >TestataRicetta</a>
                            </li>
                            <li class="link">
                                <a href="classes/TestGiasKendoGridModel.html" data-type="entity-link" >TestGiasKendoGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/TestGiasKendoGridResult.html" data-type="entity-link" >TestGiasKendoGridResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/TimeIntervalFG.html" data-type="entity-link" >TimeIntervalFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/Tipo.html" data-type="entity-link" >Tipo</a>
                            </li>
                            <li class="link">
                                <a href="classes/Tipo1.html" data-type="entity-link" >Tipo1</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoAllevamento.html" data-type="entity-link" >TipoAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoEffluente.html" data-type="entity-link" >TipoEffluente</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoFertilizzante.html" data-type="entity-link" >TipoFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipologiaFertilizzante.html" data-type="entity-link" >TipologiaFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipologiaSede.html" data-type="entity-link" >TipologiaSede</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipologiaUtente.html" data-type="entity-link" >TipologiaUtente</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoMaturazione.html" data-type="entity-link" >TipoMaturazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoObject.html" data-type="entity-link" >TipoObject</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoRisorsa.html" data-type="entity-link" >TipoRisorsa</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoTarga.html" data-type="entity-link" >TipoTarga</a>
                            </li>
                            <li class="link">
                                <a href="classes/TipoVisita.html" data-type="entity-link" >TipoVisita</a>
                            </li>
                            <li class="link">
                                <a href="classes/TitoloDiPossesso.html" data-type="entity-link" >TitoloDiPossesso</a>
                            </li>
                            <li class="link">
                                <a href="classes/TransizioneDiStato.html" data-type="entity-link" >TransizioneDiStato</a>
                            </li>
                            <li class="link">
                                <a href="classes/TranslocoException.html" data-type="entity-link" >TranslocoException</a>
                            </li>
                            <li class="link">
                                <a href="classes/Trappola.html" data-type="entity-link" >Trappola</a>
                            </li>
                            <li class="link">
                                <a href="classes/Trattamento.html" data-type="entity-link" >Trattamento</a>
                            </li>
                            <li class="link">
                                <a href="classes/TrattamentoZoo.html" data-type="entity-link" >TrattamentoZoo</a>
                            </li>
                            <li class="link">
                                <a href="classes/TreeNode.html" data-type="entity-link" >TreeNode</a>
                            </li>
                            <li class="link">
                                <a href="classes/TreeResolverInput.html" data-type="entity-link" >TreeResolverInput</a>
                            </li>
                            <li class="link">
                                <a href="classes/TreeResolverResult.html" data-type="entity-link" >TreeResolverResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/TreeValutazione.html" data-type="entity-link" >TreeValutazione</a>
                            </li>
                            <li class="link">
                                <a href="classes/UdmConvertitaToKG_L.html" data-type="entity-link" >UdmConvertitaToKG_L</a>
                            </li>
                            <li class="link">
                                <a href="classes/UdmScomposta.html" data-type="entity-link" >UdmScomposta</a>
                            </li>
                            <li class="link">
                                <a href="classes/UnitaDiMisura.html" data-type="entity-link" >UnitaDiMisura</a>
                            </li>
                            <li class="link">
                                <a href="classes/UpdateTree.html" data-type="entity-link" >UpdateTree</a>
                            </li>
                            <li class="link">
                                <a href="classes/UserModel.html" data-type="entity-link" >UserModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/UserToken.html" data-type="entity-link" >UserToken</a>
                            </li>
                            <li class="link">
                                <a href="classes/Utente.html" data-type="entity-link" >Utente</a>
                            </li>
                            <li class="link">
                                <a href="classes/Utente-1.html" data-type="entity-link" >Utente</a>
                            </li>
                            <li class="link">
                                <a href="classes/Utente_DettagliAWS.html" data-type="entity-link" >Utente_DettagliAWS</a>
                            </li>
                            <li class="link">
                                <a href="classes/Utente_Impostazioni.html" data-type="entity-link" >Utente_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="classes/Utente_Permesso.html" data-type="entity-link" >Utente_Permesso</a>
                            </li>
                            <li class="link">
                                <a href="classes/UtenteDTO.html" data-type="entity-link" >UtenteDTO</a>
                            </li>
                            <li class="link">
                                <a href="classes/UtenteFinestraTemp.html" data-type="entity-link" >UtenteFinestraTemp</a>
                            </li>
                            <li class="link">
                                <a href="classes/UtenteFlatModel.html" data-type="entity-link" >UtenteFlatModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/UtilizzoTerreno.html" data-type="entity-link" >UtilizzoTerreno</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioneExcel.html" data-type="entity-link" >ValutazioneExcel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioneTestata.html" data-type="entity-link" >ValutazioneTestata</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioneTestataxAnno.html" data-type="entity-link" >ValutazioneTestataxAnno</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioneTipoAnno.html" data-type="entity-link" >ValutazioneTipoAnno</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioniiWrapper.html" data-type="entity-link" >ValutazioniiWrapper</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioniKendoServerResult.html" data-type="entity-link" >ValutazioniKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/ValutazioniModel.html" data-type="entity-link" >ValutazioniModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/VariabiliInSessione_NG.html" data-type="entity-link" >VariabiliInSessione_NG</a>
                            </li>
                            <li class="link">
                                <a href="classes/Varieta.html" data-type="entity-link" >Varieta</a>
                            </li>
                            <li class="link">
                                <a href="classes/Varieta-1.html" data-type="entity-link" >Varieta</a>
                            </li>
                            <li class="link">
                                <a href="classes/VarietaxSpecie.html" data-type="entity-link" >VarietaxSpecie</a>
                            </li>
                            <li class="link">
                                <a href="classes/VDContrattiGridModel.html" data-type="entity-link" >VDContrattiGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/VDContrattiKendoServerResult.html" data-type="entity-link" >VDContrattiKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/VincolixData.html" data-type="entity-link" >VincolixData</a>
                            </li>
                            <li class="link">
                                <a href="classes/Vincolo.html" data-type="entity-link" >Vincolo</a>
                            </li>
                            <li class="link">
                                <a href="classes/VisualizzaBottone.html" data-type="entity-link" >VisualizzaBottone</a>
                            </li>
                            <li class="link">
                                <a href="classes/VisualizzaDettagliGridModel.html" data-type="entity-link" >VisualizzaDettagliGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/VisualizzaDettagliKendoServerResult.html" data-type="entity-link" >VisualizzaDettagliKendoServerResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/Voci_Menu.html" data-type="entity-link" >Voci_Menu</a>
                            </li>
                            <li class="link">
                                <a href="classes/warmUpEFGiasNG_Request.html" data-type="entity-link" >warmUpEFGiasNG_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/warmUpGiasNG_Request.html" data-type="entity-link" >warmUpGiasNG_Request</a>
                            </li>
                            <li class="link">
                                <a href="classes/warmUpGiasNG_Response.html" data-type="entity-link" >warmUpGiasNG_Response</a>
                            </li>
                            <li class="link">
                                <a href="classes/WaterCounter.html" data-type="entity-link" >WaterCounter</a>
                            </li>
                            <li class="link">
                                <a href="classes/WeavingClassFG.html" data-type="entity-link" >WeavingClassFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/WeavingFG.html" data-type="entity-link" >WeavingFG</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetAcquistoGridModel.html" data-type="entity-link" >WidgetAcquistoGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetAcquistoResult.html" data-type="entity-link" >WidgetAcquistoResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetDocumentiGridModel.html" data-type="entity-link" >WidgetDocumentiGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetDocumentiResult.html" data-type="entity-link" >WidgetDocumentiResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetProdottoGridModel.html" data-type="entity-link" >WidgetProdottoGridModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/WidgetProdottoResult.html" data-type="entity-link" >WidgetProdottoResult</a>
                            </li>
                            <li class="link">
                                <a href="classes/WindowArgs.html" data-type="entity-link" >WindowArgs</a>
                            </li>
                            <li class="link">
                                <a href="classes/WindowErrorMsg.html" data-type="entity-link" >WindowErrorMsg</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZoneKendo.html" data-type="entity-link" >ZoneKendo</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZoomAreaCommand.html" data-type="entity-link" >ZoomAreaCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZoomAt15Command.html" data-type="entity-link" >ZoomAt15Command</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZoomInCommand.html" data-type="entity-link" >ZoomInCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooModel.html" data-type="entity-link" >ZooModel</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZoomOutCommand.html" data-type="entity-link" >ZoomOutCommand</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooOperationGridFlatItem.html" data-type="entity-link" >ZooOperationGridFlatItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooOperationsFilters.html" data-type="entity-link" >ZooOperationsFilters</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooPrescriptionGridFlatItem.html" data-type="entity-link" >ZooPrescriptionGridFlatItem</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooPrescriptionsFilters.html" data-type="entity-link" >ZooPrescriptionsFilters</a>
                            </li>
                            <li class="link">
                                <a href="classes/ZooRow.html" data-type="entity-link" >ZooRow</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#injectables-links"' :
                                'data-bs-target="#xs-injectables-links"' }>
                                <span class="icon ion-md-arrow-round-down"></span>
                                <span>Injectables</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"' }>
                                <li class="link">
                                    <a href="injectables/AdvancedTimeFilterService.html" data-type="entity-link" >AdvancedTimeFilterService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgeaService.html" data-type="entity-link" >AgeaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgendaClient.html" data-type="entity-link" >AgendaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgendaService.html" data-type="entity-link" >AgendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgriculturalExerciceContributeLinkService.html" data-type="entity-link" >AgriculturalExerciceContributeLinkService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgriculturalPlotSlopeService.html" data-type="entity-link" >AgriculturalPlotSlopeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgriculturalPlotsMachinesLinkService.html" data-type="entity-link" >AgriculturalPlotsMachinesLinkService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgriculturalPlotWeavingService.html" data-type="entity-link" >AgriculturalPlotWeavingService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgronicaChatGPTClient.html" data-type="entity-link" >AgronicaChatGPTClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgronicaControlli_2010Client.html" data-type="entity-link" >AgronicaControlli_2010Client</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgronicaCoreDPINGClient.html" data-type="entity-link" >AgronicaCoreDPINGClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgronicaCoreUtentiBIZClient.html" data-type="entity-link" >AgronicaCoreUtentiBIZClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AgronicaCoreUtilityClient.html" data-type="entity-link" >AgronicaCoreUtilityClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AjaxAgronicaAPIService.html" data-type="entity-link" >AjaxAgronicaAPIService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AjaxAgronicaNetCore6ApiService.html" data-type="entity-link" >AjaxAgronicaNetCore6ApiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AjaxAgronicaService.html" data-type="entity-link" >AjaxAgronicaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AlertDocumentiWidgetGridConfig.html" data-type="entity-link" >AlertDocumentiWidgetGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AlgorithmConfigurationWindowService.html" data-type="entity-link" >AlgorithmConfigurationWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagrafeClient.html" data-type="entity-link" >AnagrafeClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficaBusinessLogicService.html" data-type="entity-link" >AnagraficaBusinessLogicService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficaClient.html" data-type="entity-link" >AnagraficaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficaNGClient.html" data-type="entity-link" >AnagraficaNGClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficaService.html" data-type="entity-link" >AnagraficaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficheIndiciMaturitaGridConfig.html" data-type="entity-link" >AnagraficheIndiciMaturitaGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnagraficheRilieviAvversitaGridConfig.html" data-type="entity-link" >AnagraficheRilieviAvversitaGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnalisiDocumentsService.html" data-type="entity-link" >AnalisiDocumentsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnalisiTerrenoClient.html" data-type="entity-link" >AnalisiTerrenoClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnalisiTerrenoFormService.html" data-type="entity-link" >AnalisiTerrenoFormService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnalisiTerrenoService.html" data-type="entity-link" >AnalisiTerrenoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnniHttpService.html" data-type="entity-link" >AnniHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentiBudgetService.html" data-type="entity-link" >AppezzamentiBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentiEventsService.html" data-type="entity-link" >AppezzamentiEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentiHttpService.html" data-type="entity-link" >AppezzamentiHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentiService.html" data-type="entity-link" >AppezzamentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentoCampoEditService.html" data-type="entity-link" >AppezzamentoCampoEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentoCampoService.html" data-type="entity-link" >AppezzamentoCampoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentoClient.html" data-type="entity-link" >AppezzamentoClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentoEditCodici.html" data-type="entity-link" >AppezzamentoEditCodici</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AppezzamentoEditService.html" data-type="entity-link" >AppezzamentoEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AttivitaClient.html" data-type="entity-link" >AttivitaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AttivitaPersonalizzataService.html" data-type="entity-link" >AttivitaPersonalizzataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthDispatcherClient.html" data-type="entity-link" >AuthDispatcherClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthService.html" data-type="entity-link" >AuthService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AvversitaService.html" data-type="entity-link" >AvversitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AziendaDDLService.html" data-type="entity-link" >AziendaDDLService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BIO_Dati_OrientamentoProduttivoService.html" data-type="entity-link" >BIO_Dati_OrientamentoProduttivoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BreadcrumbsService.html" data-type="entity-link" >BreadcrumbsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BreadcrumbsTxtService.html" data-type="entity-link" >BreadcrumbsTxtService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BrogliaccioGridConfig.html" data-type="entity-link" >BrogliaccioGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BudgetClient.html" data-type="entity-link" >BudgetClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BudgetClient-1.html" data-type="entity-link" >BudgetClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BudgetLoadedGuard.html" data-type="entity-link" >BudgetLoadedGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BudgetService.html" data-type="entity-link" >BudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BussinessMenuAgendaService.html" data-type="entity-link" >BussinessMenuAgendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CacCodificheGridConfigervice.html" data-type="entity-link" >CacCodificheGridConfigervice</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CacCodificheService.html" data-type="entity-link" >CacCodificheService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CalcoloSuperficiService.html" data-type="entity-link" >CalcoloSuperficiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiBudgetService.html" data-type="entity-link" >CampiBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiEditDataService.html" data-type="entity-link" >CampiEditDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiEditService.html" data-type="entity-link" >CampiEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiFactoryService.html" data-type="entity-link" >CampiFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiGridEventsService.html" data-type="entity-link" >CampiGridEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiGridHttpService.html" data-type="entity-link" >CampiGridHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CampiService.html" data-type="entity-link" >CampiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CapiAnimaliConfigService.html" data-type="entity-link" >CapiAnimaliConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CaratteristicheService.html" data-type="entity-link" >CaratteristicheService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoBudgetService.html" data-type="entity-link" >CatastoBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoCampoEditService.html" data-type="entity-link" >CatastoCampoEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoCampoService.html" data-type="entity-link" >CatastoCampoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoEditService.html" data-type="entity-link" >CatastoEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoEditUtilityService.html" data-type="entity-link" >CatastoEditUtilityService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoFactoryService.html" data-type="entity-link" >CatastoFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoGridEventsService.html" data-type="entity-link" >CatastoGridEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoHttpService.html" data-type="entity-link" >CatastoHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CatastoService.html" data-type="entity-link" >CatastoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CategorieMagazzinoService.html" data-type="entity-link" >CategorieMagazzinoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentriAziendaliService.html" data-type="entity-link" >CentriAziendaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentriCodici.html" data-type="entity-link" >CentriCodici</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentriHttpService.html" data-type="entity-link" >CentriHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentriResolver.html" data-type="entity-link" >CentriResolver</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentriService.html" data-type="entity-link" >CentriService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CentroEditService.html" data-type="entity-link" >CentroEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ChatGPTService.html" data-type="entity-link" >ChatGPTService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ClassamentiParticellaService.html" data-type="entity-link" >ClassamentiParticellaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ClassamentoCatastoService.html" data-type="entity-link" >ClassamentoCatastoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ClientePermessiGridService.html" data-type="entity-link" >ClientePermessiGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ClipboardService.html" data-type="entity-link" >ClipboardService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodiciAnagraficiConfigService.html" data-type="entity-link" >CodiciAnagraficiConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodiciTemplate.html" data-type="entity-link" >CodiciTemplate</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodiciTemplateConfigService.html" data-type="entity-link" >CodiciTemplateConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodificaCACClient.html" data-type="entity-link" >CodificaCACClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodificaInfoAggiuntiveService.html" data-type="entity-link" >CodificaInfoAggiuntiveService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodificheAgeaService.html" data-type="entity-link" >CodificheAgeaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CodificheClient.html" data-type="entity-link" >CodificheClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ComparisonWidgetService.html" data-type="entity-link" >ComparisonWidgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConduzioneService.html" data-type="entity-link" >ConduzioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfigurazioneSitiService.html" data-type="entity-link" >ConfigurazioneSitiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfrontoCatastoClient.html" data-type="entity-link" >ConfrontoCatastoClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfrontoPcCatastoGridConfigService.html" data-type="entity-link" >ConfrontoPcCatastoGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfrontoPianoColturaleDataService.html" data-type="entity-link" >ConfrontoPianoColturaleDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfrontoPianoColturaleFormsService.html" data-type="entity-link" >ConfrontoPianoColturaleFormsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConfrontoPianoColturaleService.html" data-type="entity-link" >ConfrontoPianoColturaleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConsultaSincroLogGridService.html" data-type="entity-link" >ConsultaSincroLogGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConsultaSincroService.html" data-type="entity-link" >ConsultaSincroService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContabClient.html" data-type="entity-link" >ContabClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContabilitaClient.html" data-type="entity-link" >ContabilitaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContatoriAcquaClient.html" data-type="entity-link" >ContatoriAcquaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiBudgetService.html" data-type="entity-link" >ContattiBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiEventsService.html" data-type="entity-link" >ContattiEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiFactoryService.html" data-type="entity-link" >ContattiFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiGridService.html" data-type="entity-link" >ContattiGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiHttpService.html" data-type="entity-link" >ContattiHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiRootService.html" data-type="entity-link" >ContattiRootService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContattiService.html" data-type="entity-link" >ContattiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContoEconomicoDettaglioConfigHttpService.html" data-type="entity-link" >ContoEconomicoDettaglioConfigHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContoEconomicoHttpService.html" data-type="entity-link" >ContoEconomicoHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContoEconomicoService.html" data-type="entity-link" >ContoEconomicoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ContributeService.html" data-type="entity-link" >ContributeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConversioniService.html" data-type="entity-link" >ConversioniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ConversionService.html" data-type="entity-link" >ConversionService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CookieService.html" data-type="entity-link" >CookieService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CooperativeConfigService.html" data-type="entity-link" >CooperativeConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CoperturaService.html" data-type="entity-link" >CoperturaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CostiMacchinaDataService.html" data-type="entity-link" >CostiMacchinaDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CostiMacchinaEditConfigService.html" data-type="entity-link" >CostiMacchinaEditConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CostiMacchinaEditService.html" data-type="entity-link" >CostiMacchinaEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CostiMacchinaParentFormService.html" data-type="entity-link" >CostiMacchinaParentFormService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CreaDaPoligonoService.html" data-type="entity-link" >CreaDaPoligonoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CssService.html" data-type="entity-link" >CssService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CurrentPageGuard.html" data-type="entity-link" >CurrentPageGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CustomMessageService.html" data-type="entity-link" >CustomMessageService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CustomMessagesService.html" data-type="entity-link" >CustomMessagesService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DataLayerStyleService.html" data-type="entity-link" >DataLayerStyleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DateUtilsServiceService.html" data-type="entity-link" >DateUtilsServiceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiCatastaliHttpService.html" data-type="entity-link" >DatiCatastaliHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiCatastaliService.html" data-type="entity-link" >DatiCatastaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiGeneraliGuard.html" data-type="entity-link" >DatiGeneraliGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiPrevisionaliColtureGridConfigurationService.html" data-type="entity-link" >DatiPrevisionaliColtureGridConfigurationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiPrevisionaliService.html" data-type="entity-link" >DatiPrevisionaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DatiReteAcquaClient.html" data-type="entity-link" >DatiReteAcquaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DefaultPianiColturaliClient.html" data-type="entity-link" >DefaultPianiColturaliClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DeleteMessageService.html" data-type="entity-link" >DeleteMessageService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DemetraClient.html" data-type="entity-link" >DemetraClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DestinazioneUsoService.html" data-type="entity-link" >DestinazioneUsoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DialogWindowService.html" data-type="entity-link" >DialogWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DisciplinariService.html" data-type="entity-link" >DisciplinariService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DitteService.html" data-type="entity-link" >DitteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DocumentiClient.html" data-type="entity-link" >DocumentiClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DosiEtichettaService.html" data-type="entity-link" >DosiEtichettaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DpiService.html" data-type="entity-link" >DpiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DrawingManagerService.html" data-type="entity-link" >DrawingManagerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DrawingOptionsService.html" data-type="entity-link" >DrawingOptionsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DrawingService.html" data-type="entity-link" >DrawingService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DrawWindowOperationService.html" data-type="entity-link" >DrawWindowOperationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/DSSClient.html" data-type="entity-link" >DSSClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EditCentroStore.html" data-type="entity-link" >EditCentroStore</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EditFeatureService.html" data-type="entity-link" >EditFeatureService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EditFeatureWindowService.html" data-type="entity-link" >EditFeatureWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EditGruppiMerceGridService.html" data-type="entity-link" >EditGruppiMerceGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EditGruppiMerceService.html" data-type="entity-link" >EditGruppiMerceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EntitaConfigService.html" data-type="entity-link" >EntitaConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EpocheService.html" data-type="entity-link" >EpocheService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EserciziCopiaSpostaAppezzamentiService.html" data-type="entity-link" >EserciziCopiaSpostaAppezzamentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EserciziEditService.html" data-type="entity-link" >EserciziEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EserciziEventsService.html" data-type="entity-link" >EserciziEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EserciziHttpService.html" data-type="entity-link" >EserciziHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ExportDocumentiClient.html" data-type="entity-link" >ExportDocumentiClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ExternalNavigationService.html" data-type="entity-link" >ExternalNavigationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FabbricatiGridService.html" data-type="entity-link" >FabbricatiGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FabbricatiService.html" data-type="entity-link" >FabbricatiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FeatureInformationService.html" data-type="entity-link" >FeatureInformationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FeatureService.html" data-type="entity-link" >FeatureService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FertilizzazioneService.html" data-type="entity-link" >FertilizzazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltersRilieviService.html" data-type="entity-link" >FiltersRilieviService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltersService.html" data-type="entity-link" >FiltersService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltersServiceVisite.html" data-type="entity-link" >FiltersServiceVisite</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltroJSONService.html" data-type="entity-link" >FiltroJSONService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltroRicercaClient.html" data-type="entity-link" >FiltroRicercaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltroRicercaClient-1.html" data-type="entity-link" >FiltroRicercaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FiltroRicercaService.html" data-type="entity-link" >FiltroRicercaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/Form_CampiEdit_Service.html" data-type="entity-link" >Form_CampiEdit_Service</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FormaAllevamentoService.html" data-type="entity-link" >FormaAllevamentoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FormeGiurificheService.html" data-type="entity-link" >FormeGiurificheService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FreshAndFoodClient.html" data-type="entity-link" >FreshAndFoodClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FunzioniComuniService.html" data-type="entity-link" >FunzioniComuniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GeoJsonFilterService.html" data-type="entity-link" >GeoJsonFilterService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GeoJsonService.html" data-type="entity-link" >GeoJsonService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GerarchiaService.html" data-type="entity-link" >GerarchiaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GestioneMultiAziendaService.html" data-type="entity-link" >GestioneMultiAziendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GestioneRichiesteService.html" data-type="entity-link" >GestioneRichiesteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/Gias2010Redirector.html" data-type="entity-link" >Gias2010Redirector</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GiasAppClient.html" data-type="entity-link" >GiasAppClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GIASAppClient.html" data-type="entity-link" >GIASAppClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GiasDialogService.html" data-type="entity-link" >GiasDialogService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GiasIstatService.html" data-type="entity-link" >GiasIstatService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GiasMessageService.html" data-type="entity-link" >GiasMessageService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAnalisiMappeSatellitariWindowService.html" data-type="entity-link" >GISAnalisiMappeSatellitariWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiConfigService.html" data-type="entity-link" >GISAttributiConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiEventsService.html" data-type="entity-link" >GISAttributiEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiExportGridConfig.html" data-type="entity-link" >GISAttributiExportGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiFileUploadService.html" data-type="entity-link" >GISAttributiFileUploadService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiMuzGridConfigService.html" data-type="entity-link" >GISAttributiMuzGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiMuzService.html" data-type="entity-link" >GISAttributiMuzService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISAttributiRasterMasksGridConfig.html" data-type="entity-link" >GISAttributiRasterMasksGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisAttributiRasterMasksPermissionsService.html" data-type="entity-link" >GisAttributiRasterMasksPermissionsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisAttributiService.html" data-type="entity-link" >GisAttributiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISBookmarksWindowService.html" data-type="entity-link" >GISBookmarksWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISBookmarkWindowGridConfigService.html" data-type="entity-link" >GISBookmarkWindowGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISCfgProiezioniConfigDataService.html" data-type="entity-link" >GISCfgProiezioniConfigDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISCfgProiezioniConfigService.html" data-type="entity-link" >GISCfgProiezioniConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISCfgProiezioniDataService.html" data-type="entity-link" >GISCfgProiezioniDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISCfgProiezioniGridConfig.html" data-type="entity-link" >GISCfgProiezioniGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisCfgProiezioniPermissionsService.html" data-type="entity-link" >GisCfgProiezioniPermissionsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISCfgProiezioniService.html" data-type="entity-link" >GISCfgProiezioniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisClient.html" data-type="entity-link" >GisClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisClient-1.html" data-type="entity-link" >GisClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISEditMuzParticelleCatastaliGridConfigService.html" data-type="entity-link" >GISEditMuzParticelleCatastaliGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisFixedLayerPropertyService.html" data-type="entity-link" >GisFixedLayerPropertyService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISGeometrySelectionService.html" data-type="entity-link" >GISGeometrySelectionService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISGestionePianoRateoService.html" data-type="entity-link" >GISGestionePianoRateoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISLayerAdvancedSettingsWindowService.html" data-type="entity-link" >GISLayerAdvancedSettingsWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisLayerColorPickerService.html" data-type="entity-link" >GisLayerColorPickerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISLayerPermissionsWindowService.html" data-type="entity-link" >GISLayerPermissionsWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisLoadedGuard.html" data-type="entity-link" >GisLoadedGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISParticelleCatastaliGridConfigService.html" data-type="entity-link" >GISParticelleCatastaliGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISParticelleCatastaliService.html" data-type="entity-link" >GISParticelleCatastaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GISRasterConfigurationWindowService.html" data-type="entity-link" >GISRasterConfigurationWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisService.html" data-type="entity-link" >GisService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GisToolbarService.html" data-type="entity-link" >GisToolbarService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapDataService.html" data-type="entity-link" >GoogleMapDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapFeatureService.html" data-type="entity-link" >GoogleMapFeatureService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapGeoJsonLazyService.html" data-type="entity-link" >GoogleMapGeoJsonLazyService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapGeoJsonService.html" data-type="entity-link" >GoogleMapGeoJsonService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapHeatmapService.html" data-type="entity-link" >GoogleMapHeatmapService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapMovementControlService.html" data-type="entity-link" >GoogleMapMovementControlService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GoogleMapService.html" data-type="entity-link" >GoogleMapService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridAddMacchineService.html" data-type="entity-link" >GridAddMacchineService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridAddOperaiService.html" data-type="entity-link" >GridAddOperaiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridAnalisiTerrenoHttpService.html" data-type="entity-link" >GridAnalisiTerrenoHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridAziendeCentriImpostazioniGridConfigService.html" data-type="entity-link" >GridAziendeCentriImpostazioniGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridBatchHandlerService.html" data-type="entity-link" >GridBatchHandlerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridCapiAnimaliService.html" data-type="entity-link" >GridCapiAnimaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridCategorieMagazzinoService.html" data-type="entity-link" >GridCategorieMagazzinoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridDosiProdottiControlliService.html" data-type="entity-link" >GridDosiProdottiControlliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridDosiProdottiHttpService.html" data-type="entity-link" >GridDosiProdottiHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridDosiProdottiService.html" data-type="entity-link" >GridDosiProdottiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridEntitaHttpService.html" data-type="entity-link" >GridEntitaHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridErrorService.html" data-type="entity-link" >GridErrorService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridFiltroRicercaHttpService.html" data-type="entity-link" >GridFiltroRicercaHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridFiltroSqlMateriePrimeService.html" data-type="entity-link" >GridFiltroSqlMateriePrimeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridGroupTransitionsService.html" data-type="entity-link" >GridGroupTransitionsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridGruppiService.html" data-type="entity-link" >GridGruppiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridImpiantiHttpService.html" data-type="entity-link" >GridImpiantiHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridImpiantiService.html" data-type="entity-link" >GridImpiantiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridImpreseVisibilitaService.html" data-type="entity-link" >GridImpreseVisibilitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridMacchineHttpService.html" data-type="entity-link" >GridMacchineHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridNoteHttpService.html" data-type="entity-link" >GridNoteHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridOpEditSrvice.html" data-type="entity-link" >GridOpEditSrvice</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridOperatoriHttpService.html" data-type="entity-link" >GridOperatoriHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridOperazioneCausaleHttpService.html" data-type="entity-link" >GridOperazioneCausaleHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridParametriAnalisiTerrenoHttpService.html" data-type="entity-link" >GridParametriAnalisiTerrenoHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridPermessiEditService.html" data-type="entity-link" >GridPermessiEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridPermessiService.html" data-type="entity-link" >GridPermessiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridProdottiDaTrattareHttpService.html" data-type="entity-link" >GridProdottiDaTrattareHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridProdottiSomministrazioneService.html" data-type="entity-link" >GridProdottiSomministrazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridProfiliPermessiService.html" data-type="entity-link" >GridProfiliPermessiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridProfiliService.html" data-type="entity-link" >GridProfiliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridRaccoltaAutoConfigService.html" data-type="entity-link" >GridRaccoltaAutoConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridReportFitoHttpService.html" data-type="entity-link" >GridReportFitoHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridRilieviConfigService.html" data-type="entity-link" >GridRilieviConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridRilieviHttpService.html" data-type="entity-link" >GridRilieviHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridRipManualeConfigService.html" data-type="entity-link" >GridRipManualeConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridRootHelper.html" data-type="entity-link" >GridRootHelper</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridSpecieVarietaConfigService.html" data-type="entity-link" >GridSpecieVarietaConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridUtentiModificaService.html" data-type="entity-link" >GridUtentiModificaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridUtentiPermessiService.html" data-type="entity-link" >GridUtentiPermessiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridVarietaService.html" data-type="entity-link" >GridVarietaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridVisibilitaEditService.html" data-type="entity-link" >GridVisibilitaEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridVisiteHttpService.html" data-type="entity-link" >GridVisiteHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GridWorkerService.html" data-type="entity-link" >GridWorkerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiMerceGrid2Service.html" data-type="entity-link" >GruppiMerceGrid2Service</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiRaccoltaGridConfigurationService.html" data-type="entity-link" >GruppiRaccoltaGridConfigurationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiRaccoltaGridEventsService.html" data-type="entity-link" >GruppiRaccoltaGridEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiRaccoltaService.html" data-type="entity-link" >GruppiRaccoltaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiUtentiGridService.html" data-type="entity-link" >GruppiUtentiGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiUtentiPerGruppiMerceSerivce.html" data-type="entity-link" >GruppiUtentiPerGruppiMerceSerivce</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiUtentiService.html" data-type="entity-link" >GruppiUtentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppiUtentixGruppiMerceGridService.html" data-type="entity-link" >GruppiUtentixGruppiMerceGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppoFinalitaService.html" data-type="entity-link" >GruppoFinalitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GruppoVarietaleService.html" data-type="entity-link" >GruppoVarietaleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/HttpService.html" data-type="entity-link" >HttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpiantiFactoryService.html" data-type="entity-link" >ImpiantiFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpiantiService.html" data-type="entity-link" >ImpiantiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImportAttivitaClient.html" data-type="entity-link" >ImportAttivitaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpostazioniAziendeCentriGridConfigService.html" data-type="entity-link" >ImpostazioniAziendeCentriGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpostazioniAziendeCentriService.html" data-type="entity-link" >ImpostazioniAziendeCentriService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpostazioniFormService.html" data-type="entity-link" >ImpostazioniFormService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpostazioniLavorazioniGridConfigService.html" data-type="entity-link" >ImpostazioniLavorazioniGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpostazioniUtentiService.html" data-type="entity-link" >ImpostazioniUtentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpresaEditService.html" data-type="entity-link" >ImpresaEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpresaRichiestaGuard.html" data-type="entity-link" >ImpresaRichiestaGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseBudgetService.html" data-type="entity-link" >ImpreseBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseFilterService.html" data-type="entity-link" >ImpreseFilterService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseGridHttpService.html" data-type="entity-link" >ImpreseGridHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseParametriGHGGridService.html" data-type="entity-link" >ImpreseParametriGHGGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseParametriGHGService.html" data-type="entity-link" >ImpreseParametriGHGService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ImpreseService.html" data-type="entity-link" >ImpreseService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IndicatoriWidgetService.html" data-type="entity-link" >IndicatoriWidgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IndirizziAppezzamentoDataService.html" data-type="entity-link" >IndirizziAppezzamentoDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IndirizziAppezzamentoService.html" data-type="entity-link" >IndirizziAppezzamentoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IndirizziConfigService.html" data-type="entity-link" >IndirizziConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IndirizziParentFormDataService.html" data-type="entity-link" >IndirizziParentFormDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InfowindowClustererService.html" data-type="entity-link" >InfowindowClustererService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleAppezzamentoDataService.html" data-type="entity-link" >InvestimentoCatastaleAppezzamentoDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleAppezzamentoGridService.html" data-type="entity-link" >InvestimentoCatastaleAppezzamentoGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleBudgetService.html" data-type="entity-link" >InvestimentoCatastaleBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleCampoBudgetService.html" data-type="entity-link" >InvestimentoCatastaleCampoBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleCampoGridService.html" data-type="entity-link" >InvestimentoCatastaleCampoGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleCampoService.html" data-type="entity-link" >InvestimentoCatastaleCampoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleFactoryService.html" data-type="entity-link" >InvestimentoCatastaleFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleFiltriService.html" data-type="entity-link" >InvestimentoCatastaleFiltriService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleGridService.html" data-type="entity-link" >InvestimentoCatastaleGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/InvestimentoCatastaleService.html" data-type="entity-link" >InvestimentoCatastaleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IrrigazioneService.html" data-type="entity-link" >IrrigazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IsAliveClient.html" data-type="entity-link" >IsAliveClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/IsAliveClient-1.html" data-type="entity-link" >IsAliveClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/JstsService.html" data-type="entity-link" >JstsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/KendoGridMasterDetailService.html" data-type="entity-link" >KendoGridMasterDetailService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/KendoWindowsService.html" data-type="entity-link" >KendoWindowsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LayerService.html" data-type="entity-link" >LayerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LayerStyleService.html" data-type="entity-link" >LayerStyleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LettureContatoriGridConfigService.html" data-type="entity-link" >LettureContatoriGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LingueService.html" data-type="entity-link" >LingueService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LoadExternalStylesService.html" data-type="entity-link" >LoadExternalStylesService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LocalizzazioniService.html" data-type="entity-link" >LocalizzazioniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LoginClient.html" data-type="entity-link" >LoginClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LogoutClient.html" data-type="entity-link" >LogoutClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchinaEditCaratteristicheConfigService.html" data-type="entity-link" >MacchinaEditCaratteristicheConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchinaEditFormsService.html" data-type="entity-link" >MacchinaEditFormsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineBudgetService.html" data-type="entity-link" >MacchineBudgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineConfigService.html" data-type="entity-link" >MacchineConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineEditGerarchiaGridService.html" data-type="entity-link" >MacchineEditGerarchiaGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineEditImageStorageService.html" data-type="entity-link" >MacchineEditImageStorageService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineFactoryService.html" data-type="entity-link" >MacchineFactoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineGridEventsService.html" data-type="entity-link" >MacchineGridEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacchineService.html" data-type="entity-link" >MacchineService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacrousiCatastoService.html" data-type="entity-link" >MacrousiCatastoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacrousiParticellaService.html" data-type="entity-link" >MacrousiParticellaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MacrousiService.html" data-type="entity-link" >MacrousiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MagazziniClient.html" data-type="entity-link" >MagazziniClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MapGridOverlayService.html" data-type="entity-link" >MapGridOverlayService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MappaMultiAziendaService.html" data-type="entity-link" >MappaMultiAziendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MappePrescrizioneService.html" data-type="entity-link" >MappePrescrizioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MasterService.html" data-type="entity-link" >MasterService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MatomoService.html" data-type="entity-link" >MatomoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MeasureDistanceService.html" data-type="entity-link" >MeasureDistanceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MenuAgendaDataStore.html" data-type="entity-link" >MenuAgendaDataStore</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MenuClient.html" data-type="entity-link" >MenuClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MenuContestualeService.html" data-type="entity-link" >MenuContestualeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MenuProfilazioneGridConfigService.html" data-type="entity-link" >MenuProfilazioneGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MessaggisticaClient.html" data-type="entity-link" >MessaggisticaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MetaschemaClient.html" data-type="entity-link" >MetaschemaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MetaschemaClient-1.html" data-type="entity-link" >MetaschemaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MetaschemaNGClient.html" data-type="entity-link" >MetaschemaNGClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MetodoProduzioneParticellaService.html" data-type="entity-link" >MetodoProduzioneParticellaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MetodoProduzioneService.html" data-type="entity-link" >MetodoProduzioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MisceleService.html" data-type="entity-link" >MisceleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MisureAvversitaAnagraficaService.html" data-type="entity-link" >MisureAvversitaAnagraficaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MisureIndiciMaturitaAnagraficaService.html" data-type="entity-link" >MisureIndiciMaturitaAnagraficaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ModelloClient.html" data-type="entity-link" >ModelloClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ModuliAttivi.html" data-type="entity-link" >ModuliAttivi</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ModuliAttiviGuard.html" data-type="entity-link" >ModuliAttiviGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MonitoraggioWidgetService.html" data-type="entity-link" >MonitoraggioWidgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MultiAziendaService.html" data-type="entity-link" >MultiAziendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NavigationService.html" data-type="entity-link" >NavigationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NewAgriClient.html" data-type="entity-link" >NewAgriClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NoteClient.html" data-type="entity-link" >NoteClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NoteService.html" data-type="entity-link" >NoteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NotificationClient.html" data-type="entity-link" >NotificationClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NuovaOperazioneService.html" data-type="entity-link" >NuovaOperazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ObjParametriAgendaService.html" data-type="entity-link" >ObjParametriAgendaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperationEditService.html" data-type="entity-link" >OperationEditService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperationsService.html" data-type="entity-link" >OperationsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperatoreDDLService.html" data-type="entity-link" >OperatoreDDLService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperazioneClient.html" data-type="entity-link" >OperazioneClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperazioniService.html" data-type="entity-link" >OperazioniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/OperazioniZooClient.html" data-type="entity-link" >OperazioniZooClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PendingChangesGuard.html" data-type="entity-link" >PendingChangesGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PermessiUtenteGuard.html" data-type="entity-link" >PermessiUtenteGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PermessiUtenteService.html" data-type="entity-link" >PermessiUtenteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PianoColturaleClient.html" data-type="entity-link" >PianoColturaleClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PianoColturaleGridConfigService.html" data-type="entity-link" >PianoColturaleGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PianoConcimazioneService.html" data-type="entity-link" >PianoConcimazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PianoContiHttpService.html" data-type="entity-link" >PianoContiHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PianoContiService.html" data-type="entity-link" >PianoContiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PivaValidatorService.html" data-type="entity-link" >PivaValidatorService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PolygonLabelInfowindowService.html" data-type="entity-link" >PolygonLabelInfowindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PolygonLabelService.html" data-type="entity-link" >PolygonLabelService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PolygonWindowEventsService.html" data-type="entity-link" >PolygonWindowEventsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PortinnestoService.html" data-type="entity-link" >PortinnestoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PositionService.html" data-type="entity-link" >PositionService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PossessiParticelleService.html" data-type="entity-link" >PossessiParticelleService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PossessiService.html" data-type="entity-link" >PossessiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PrescrizioniClient.html" data-type="entity-link" >PrescrizioniClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProdottiService.html" data-type="entity-link" >ProdottiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProdottoSomministrazioneConfigService.html" data-type="entity-link" >ProdottoSomministrazioneConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProductsService.html" data-type="entity-link" >ProductsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneClient.html" data-type="entity-link" >ProfilazioneClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneDataShareService.html" data-type="entity-link" >ProfilazioneDataShareService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneImpreseClient.html" data-type="entity-link" >ProfilazioneImpreseClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneImpreseDefaultDistintaProduzioneGridConfigService.html" data-type="entity-link" >ProfilazioneImpreseDefaultDistintaProduzioneGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneImpreseDefaultSpecieGridConfigService.html" data-type="entity-link" >ProfilazioneImpreseDefaultSpecieGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneImpreseParametriGeneraliColturaGridConfigService.html" data-type="entity-link" >ProfilazioneImpreseParametriGeneraliColturaGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneImpreseService.html" data-type="entity-link" >ProfilazioneImpreseService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneMacchineClient.html" data-type="entity-link" >ProfilazioneMacchineClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneService.html" data-type="entity-link" >ProfilazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfilazioneUtentiService.html" data-type="entity-link" >ProfilazioneUtentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProjectXContributeService.html" data-type="entity-link" >ProjectXContributeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProvenienzaSemeService.html" data-type="entity-link" >ProvenienzaSemeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProvisioningClient.html" data-type="entity-link" >ProvisioningClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PuaService.html" data-type="entity-link" >PuaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCControlliSalvataggioService.html" data-type="entity-link" >QdCControlliSalvataggioService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCDettagliFertilizzantiService.html" data-type="entity-link" >QdCDettagliFertilizzantiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCDettagliFormulatiService.html" data-type="entity-link" >QdCDettagliFormulatiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCDettagliSementiService.html" data-type="entity-link" >QdCDettagliSementiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCFertilizzantiService.html" data-type="entity-link" >QdCFertilizzantiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCFormService.html" data-type="entity-link" >QdCFormService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCFormToAttivitaService.html" data-type="entity-link" >QdCFormToAttivitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCFormulatiService.html" data-type="entity-link" >QdCFormulatiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCLoadedGuard.html" data-type="entity-link" >QdCLoadedGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCMultiOperazioneService.html" data-type="entity-link" >QdCMultiOperazioneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCProdottiService.html" data-type="entity-link" >QdCProdottiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCRaccoltaService.html" data-type="entity-link" >QdCRaccoltaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCRilieviService.html" data-type="entity-link" >QdCRilieviService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCSementiService.html" data-type="entity-link" >QdCSementiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCService.html" data-type="entity-link" >QdCService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCTestataService.html" data-type="entity-link" >QdCTestataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCUnitadiMisuraService.html" data-type="entity-link" >QdCUnitadiMisuraService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QdCVisibilitaControlliTestataService.html" data-type="entity-link" >QdCVisibilitaControlliTestataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QuadernoDiCampagnaClient.html" data-type="entity-link" >QuadernoDiCampagnaClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QualitaCatastoService.html" data-type="entity-link" >QualitaCatastoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RaccoltaDataShareService.html" data-type="entity-link" >RaccoltaDataShareService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RasterOverlayService.html" data-type="entity-link" >RasterOverlayService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RegolamentiService.html" data-type="entity-link" >RegolamentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoAPIService.html" data-type="entity-link" >RequisitiStabilimentoAPIService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoClient.html" data-type="entity-link" >RequisitiStabilimentoClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoContractsGridModel.html" data-type="entity-link" >RequisitiStabilimentoContractsGridModel</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoContrattiGridConfigurationService.html" data-type="entity-link" >RequisitiStabilimentoContrattiGridConfigurationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoGridConfigurationService.html" data-type="entity-link" >RequisitiStabilimentoGridConfigurationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RequisitiStabilimentoService.html" data-type="entity-link" >RequisitiStabilimentoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RetinaturaService.html" data-type="entity-link" >RetinaturaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RicetteGridConfig.html" data-type="entity-link" >RicetteGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RicetteService.html" data-type="entity-link" >RicetteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RiepilogoMeteoWidgetService.html" data-type="entity-link" >RiepilogoMeteoWidgetService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RilieviAvversitaGridConfig.html" data-type="entity-link" >RilieviAvversitaGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RilieviAvversitaService.html" data-type="entity-link" >RilieviAvversitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/RubricaConfigService.html" data-type="entity-link" >RubricaConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SalvataggioAppezzamentoGISService.html" data-type="entity-link" >SalvataggioAppezzamentoGISService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SatelliteGlobalDataLoader.html" data-type="entity-link" >SatelliteGlobalDataLoader</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SatelliteLocalDataLoader.html" data-type="entity-link" >SatelliteLocalDataLoader</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SatelliteService.html" data-type="entity-link" >SatelliteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ScriptService.html" data-type="entity-link" >ScriptService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SelezionaCentroService.html" data-type="entity-link" >SelezionaCentroService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SeminaTrapiantoService.html" data-type="entity-link" >SeminaTrapiantoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SharedClient.html" data-type="entity-link" >SharedClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SharedDataService.html" data-type="entity-link" >SharedDataService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SincroClient.html" data-type="entity-link" >SincroClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SmartTractorsClient.html" data-type="entity-link" >SmartTractorsClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SpecieAnimaliService.html" data-type="entity-link" >SpecieAnimaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SpecieVegetaliService.html" data-type="entity-link" >SpecieVegetaliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StampeFiltroRicercaService.html" data-type="entity-link" >StampeFiltroRicercaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StatisticheClient.html" data-type="entity-link" >StatisticheClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StatoPatrimonialeDettaglioHttpService.html" data-type="entity-link" >StatoPatrimonialeDettaglioHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StatoPatrimonialeHttpService.html" data-type="entity-link" >StatoPatrimonialeHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StatoPatrimonialeService.html" data-type="entity-link" >StatoPatrimonialeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/SuperficieValidatorService.html" data-type="entity-link" >SuperficieValidatorService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TabelleWsClientService.html" data-type="entity-link" >TabelleWsClientService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TestAnagraficaService.html" data-type="entity-link" >TestAnagraficaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TestClient.html" data-type="entity-link" >TestClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TestGiasKendoGridConfigService.html" data-type="entity-link" >TestGiasKendoGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TestGridMasterService.html" data-type="entity-link" >TestGridMasterService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ThemeWindowService.html" data-type="entity-link" >ThemeWindowService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TipologieUtentiService.html" data-type="entity-link" >TipologieUtentiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TranslocoHttpLoader.html" data-type="entity-link" >TranslocoHttpLoader</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TranslocoLoadedGuard.html" data-type="entity-link" >TranslocoLoadedGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TrattamentoZooFormService.html" data-type="entity-link" >TrattamentoZooFormService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeAziendeService.html" data-type="entity-link" >TreeAziendeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeContainerService.html" data-type="entity-link" >TreeContainerService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeFiltersService.html" data-type="entity-link" >TreeFiltersService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeGisFiltersService.html" data-type="entity-link" >TreeGisFiltersService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeGisService.html" data-type="entity-link" >TreeGisService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeGridService.html" data-type="entity-link" >TreeGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TreeService.html" data-type="entity-link" >TreeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/TuttiTipiConfig.html" data-type="entity-link" >TuttiTipiConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UltimiAcquistiWidgetGridConfig.html" data-type="entity-link" >UltimiAcquistiWidgetGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UltimiProdottiWidgetGridConfig.html" data-type="entity-link" >UltimiProdottiWidgetGridConfig</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UnitaDiMisuraService.html" data-type="entity-link" >UnitaDiMisuraService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UsersLoaderService.html" data-type="entity-link" >UsersLoaderService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UtentiClient.html" data-type="entity-link" >UtentiClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UtilityNGClient.html" data-type="entity-link" >UtilityNGClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ValutazioniClient.html" data-type="entity-link" >ValutazioniClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ValutazioniHttpService.html" data-type="entity-link" >ValutazioniHttpService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ValutazioniService.html" data-type="entity-link" >ValutazioniService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VarietaService.html" data-type="entity-link" >VarietaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VDContrattiGridService.html" data-type="entity-link" >VDContrattiGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VincoliService.html" data-type="entity-link" >VincoliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VisibilitaService.html" data-type="entity-link" >VisibilitaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VisiteClient.html" data-type="entity-link" >VisiteClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VisiteService.html" data-type="entity-link" >VisiteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VisualizzaDettagliGridConfigurationService.html" data-type="entity-link" >VisualizzaDettagliGridConfigurationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/VisualizzaDettagliService.html" data-type="entity-link" >VisualizzaDettagliService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WidgetConfigService.html" data-type="entity-link" >WidgetConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WidgetDocumentaleClient.html" data-type="entity-link" >WidgetDocumentaleClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WidgetIndiciClient.html" data-type="entity-link" >WidgetIndiciClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WidgetsClient.html" data-type="entity-link" >WidgetsClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WidgetStatisticheClient.html" data-type="entity-link" >WidgetStatisticheClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WKTService.html" data-type="entity-link" >WKTService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WmsService.html" data-type="entity-link" >WmsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZoneCatastoService.html" data-type="entity-link" >ZoneCatastoService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZoneParticellaService.html" data-type="entity-link" >ZoneParticellaService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZoneService.html" data-type="entity-link" >ZoneService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooClient.html" data-type="entity-link" >ZooClient</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooFiltersHelperService.html" data-type="entity-link" >ZooFiltersHelperService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooGridService.html" data-type="entity-link" >ZooGridService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooIndicationsGridConfigService.html" data-type="entity-link" >ZooIndicationsGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooOperationsGridConfigService.html" data-type="entity-link" >ZooOperationsGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooPrescriptionsGridConfigService.html" data-type="entity-link" >ZooPrescriptionsGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooPrescriptionsInProgressGridConfigService.html" data-type="entity-link" >ZooPrescriptionsInProgressGridConfigService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ZooRedirectorService.html" data-type="entity-link" >ZooRedirectorService</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interceptors-links"' :
                            'data-bs-target="#xs-interceptors-links"' }>
                            <span class="icon ion-ios-swap"></span>
                            <span>Interceptors</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"' }>
                            <li class="link">
                                <a href="interceptors/AuthInterceptor.html" data-type="entity-link" >AuthInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/CompressioneInterceptor.html" data-type="entity-link" >CompressioneInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/CoreWSInterceptor.html" data-type="entity-link" >CoreWSInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/DateInterceptor.html" data-type="entity-link" >DateInterceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/NetCore6Interceptor.html" data-type="entity-link" >NetCore6Interceptor</a>
                            </li>
                            <li class="link">
                                <a href="interceptors/UploadInterceptor.html" data-type="entity-link" >UploadInterceptor</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interfaces-links"' :
                            'data-bs-target="#xs-interfaces-links"' }>
                            <span class="icon ion-md-information-circle-outline"></span>
                            <span>Interfaces</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"' }>
                            <li class="link">
                                <a href="interfaces/Acquisto.html" data-type="entity-link" >Acquisto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ActivityImportData.html" data-type="entity-link" >ActivityImportData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ActivityImportJsonObject.html" data-type="entity-link" >ActivityImportJsonObject</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Aggiorna_Widgets_In.html" data-type="entity-link" >Aggiorna_Widgets_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiornaElementoGraficoPerTipoOggetto_In.html" data-type="entity-link" >AggiornaElementoGraficoPerTipoOggetto_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiornaElencoTipologie_In.html" data-type="entity-link" >AggiornaElencoTipologie_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiornaFiltroImpianti_In.html" data-type="entity-link" >AggiornaFiltroImpianti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiornaFiltroImpianti_Out.html" data-type="entity-link" >AggiornaFiltroImpianti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiornaOreMinuti_In.html" data-type="entity-link" >AggiornaOreMinuti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AggiungiRicettaAlPUA.html" data-type="entity-link" >AggiungiRicettaAlPUA</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AgroMeteoConfig.html" data-type="entity-link" >AgroMeteoConfig</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AlberoMenu.html" data-type="entity-link" >AlberoMenu</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AlertSerie.html" data-type="entity-link" >AlertSerie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AlgoritmoProiezione.html" data-type="entity-link" >AlgoritmoProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AllegatiDaRicettaDestinazione.html" data-type="entity-link" >AllegatiDaRicettaDestinazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AllegatoFile.html" data-type="entity-link" >AllegatoFile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AllegatoLayer.html" data-type="entity-link" >AllegatoLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AllegatoLayerModifica.html" data-type="entity-link" >AllegatoLayerModifica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiCorrezione.html" data-type="entity-link" >AnalisiCorrezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiDettaglio.html" data-type="entity-link" >AnalisiDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfAppezzamento.html" data-type="entity-link" >AnalisiEntita_1OfAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfCampo.html" data-type="entity-link" >AnalisiEntita_1OfCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfCatastoCentroAziendale.html" data-type="entity-link" >AnalisiEntita_1OfCatastoCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfCentroAziendale.html" data-type="entity-link" >AnalisiEntita_1OfCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfFabbricato.html" data-type="entity-link" >AnalisiEntita_1OfFabbricato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfImpianto.html" data-type="entity-link" >AnalisiEntita_1OfImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiEntita_1OfImpresa.html" data-type="entity-link" >AnalisiEntita_1OfImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiMeteo_In.html" data-type="entity-link" >AnalisiMeteo_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiMeteo_Out.html" data-type="entity-link" >AnalisiMeteo_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiParametro.html" data-type="entity-link" >AnalisiParametro</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiTerreno.html" data-type="entity-link" >AnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiTestate_MUZ.html" data-type="entity-link" >AnalisiTestate_MUZ</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiTipo.html" data-type="entity-link" >AnalisiTipo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnalisiTipologia.html" data-type="entity-link" >AnalisiTipologia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnimationData.html" data-type="entity-link" >AnimationData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnimationStepData.html" data-type="entity-link" >AnimationStepData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnomalieCapoAnimale.html" data-type="entity-link" >AnomalieCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AnomalieCapoAnimale-1.html" data-type="entity-link" >AnomalieCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Api_Response.html" data-type="entity-link" >Api_Response</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApiValidationRequest.html" data-type="entity-link" >ApiValidationRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/APP_Ricette_Operazioni.html" data-type="entity-link" >APP_Ricette_Operazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento.html" data-type="entity-link" >Appezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento-1.html" data-type="entity-link" >Appezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento2.html" data-type="entity-link" >Appezzamento2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_BlockAppezzamento.html" data-type="entity-link" >Appezzamento_BlockAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_BlockAppezzamento-1.html" data-type="entity-link" >Appezzamento_BlockAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_DatiSementieri.html" data-type="entity-link" >Appezzamento_DatiSementieri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_DatiSementieri-1.html" data-type="entity-link" >Appezzamento_DatiSementieri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_MUZ.html" data-type="entity-link" >Appezzamento_MUZ</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_PK.html" data-type="entity-link" >Appezzamento_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Appezzamento_PK-1.html" data-type="entity-link" >Appezzamento_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AppezzamentoCampo.html" data-type="entity-link" >AppezzamentoCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AppezzamentoJoinDescrizioni.html" data-type="entity-link" >AppezzamentoJoinDescrizioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AppezzamentoXParcoMacchine.html" data-type="entity-link" >AppezzamentoXParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApplicaStili.html" data-type="entity-link" >ApplicaStili</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApportoMacroelementi.html" data-type="entity-link" >ApportoMacroelementi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApportoMacroelementi-1.html" data-type="entity-link" >ApportoMacroelementi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApriDocumentaleParams.html" data-type="entity-link" >ApriDocumentaleParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApriDocumentaleParams-1.html" data-type="entity-link" >ApriDocumentaleParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApriSitoAnalisi_Out.html" data-type="entity-link" >ApriSitoAnalisi_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AssociaProfiloObj.html" data-type="entity-link" >AssociaProfiloObj</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AssociaUtente.html" data-type="entity-link" >AssociaUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AssociazionePK.html" data-type="entity-link" >AssociazionePK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AssociazionePK-1.html" data-type="entity-link" >AssociazionePK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttachmentCheckParams.html" data-type="entity-link" >AttachmentCheckParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivaAttributoLayer_In.html" data-type="entity-link" >AttivaAttributoLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivazioneConfigurazioneAlgoritmiCartografici.html" data-type="entity-link" >AttivazioneConfigurazioneAlgoritmiCartografici</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivazioneMascheraLayerRaster.html" data-type="entity-link" >AttivazioneMascheraLayerRaster</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Attivita.html" data-type="entity-link" >Attivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Attivita-1.html" data-type="entity-link" >Attivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Attivita_xModificaMultipla.html" data-type="entity-link" >Attivita_xModificaMultipla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivitaNavigazioneAziende_in.html" data-type="entity-link" >AttivitaNavigazioneAziende_in</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivitaPersonalizzata.html" data-type="entity-link" >AttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivitaPersonalizzata-1.html" data-type="entity-link" >AttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttivitaStatistiche.html" data-type="entity-link" >AttivitaStatistiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttributiExportData.html" data-type="entity-link" >AttributiExportData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttributoLayer.html" data-type="entity-link" >AttributoLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AttributoLayer_In.html" data-type="entity-link" >AttributoLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Avversita.html" data-type="entity-link" >Avversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Avversita-1.html" data-type="entity-link" >Avversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AvversitaGruppo.html" data-type="entity-link" >AvversitaGruppo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Axis.html" data-type="entity-link" >Axis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/AxisTitle.html" data-type="entity-link" >AxisTitle</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Azienda_Centro_Specie_Varieta.html" data-type="entity-link" >Azienda_Centro_Specie_Varieta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Band.html" data-type="entity-link" >Band</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodeDescr.html" data-type="entity-link" >BaseCodeDescr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodeDescr-1.html" data-type="entity-link" >BaseCodeDescr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodeDescrStr.html" data-type="entity-link" >BaseCodeDescrStr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodeDescrStr-1.html" data-type="entity-link" >BaseCodeDescrStr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodeValue_2OfStringAndInt32.html" data-type="entity-link" >BaseCodeValue_2OfStringAndInt32</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BaseCodiceDescr.html" data-type="entity-link" >BaseCodiceDescr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BioOrganismoDiControllo.html" data-type="entity-link" >BioOrganismoDiControllo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BioOrganismoDiControllo-1.html" data-type="entity-link" >BioOrganismoDiControllo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BioTipoAttivita.html" data-type="entity-link" >BioTipoAttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BioTipoAttivita-1.html" data-type="entity-link" >BioTipoAttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BloccaAttivitaAgenda.html" data-type="entity-link" >BloccaAttivitaAgenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BloccaSbloccaAppezzamenti.html" data-type="entity-link" >BloccaSbloccaAppezzamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Blocco.html" data-type="entity-link" >Blocco</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Blocco-1.html" data-type="entity-link" >Blocco</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Bookmark.html" data-type="entity-link" >Bookmark</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BreadcrumbParamsDto.html" data-type="entity-link" >BreadcrumbParamsDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BreadcrumbsInfo.html" data-type="entity-link" >BreadcrumbsInfo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BrogliaccioRow.html" data-type="entity-link" >BrogliaccioRow</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfAppezzamento.html" data-type="entity-link" >BudgetAnagrafica_1OfAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfCampo.html" data-type="entity-link" >BudgetAnagrafica_1OfCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfCaricaDatiCatastali.html" data-type="entity-link" >BudgetAnagrafica_1OfCaricaDatiCatastali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfLeggiAppezzamento.html" data-type="entity-link" >BudgetAnagrafica_1OfLeggiAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfLeggiCampi.html" data-type="entity-link" >BudgetAnagrafica_1OfLeggiCampi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfLeggiInvestimentoCatastale.html" data-type="entity-link" >BudgetAnagrafica_1OfLeggiInvestimentoCatastale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfLeggiInvestimentoCatastaleCampo.html" data-type="entity-link" >BudgetAnagrafica_1OfLeggiInvestimentoCatastaleCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfParametri_ObjParametriAgenda_NG.html" data-type="entity-link" >BudgetAnagrafica_1OfParametri_ObjParametriAgenda_NG</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfParametri_ObjParametriAgenda_NG_GestioneRichieste.html" data-type="entity-link" >BudgetAnagrafica_1OfParametri_ObjParametriAgenda_NG_GestioneRichieste</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BudgetAnagrafica_1OfString.html" data-type="entity-link" >BudgetAnagrafica_1OfString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BufferZone.html" data-type="entity-link" >BufferZone</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BufferZone_In.html" data-type="entity-link" >BufferZone_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CacCodificaModel.html" data-type="entity-link" >CacCodificaModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CambioLinguaObj.html" data-type="entity-link" >CambioLinguaObj</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Campione.html" data-type="entity-link" >Campione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Campo.html" data-type="entity-link" >Campo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Campo_PK.html" data-type="entity-link" >Campo_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Campo_PK-1.html" data-type="entity-link" >Campo_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cancella_EntitaGraf_Out.html" data-type="entity-link" >Cancella_EntitaGraf_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cancella_In.html" data-type="entity-link" >Cancella_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cancella_Out.html" data-type="entity-link" >Cancella_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CancellaImpostazione_AziendeCentri.html" data-type="entity-link" >CancellaImpostazione_AziendeCentri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CancellaMacchina.html" data-type="entity-link" >CancellaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CapoAnimale.html" data-type="entity-link" >CapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CapoAnimale-1.html" data-type="entity-link" >CapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CapoAnimaleLight.html" data-type="entity-link" >CapoAnimaleLight</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaratteristicaMacchina.html" data-type="entity-link" >CaratteristicaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaratteristicaMacchina-1.html" data-type="entity-link" >CaratteristicaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaratteristicheMacchina_In.html" data-type="entity-link" >CaratteristicheMacchina_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaratteristicheMacchinaValue.html" data-type="entity-link" >CaratteristicheMacchinaValue</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Carburante.html" data-type="entity-link" >Carburante</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Carburante-1.html" data-type="entity-link" >Carburante</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Carica_Stalle.html" data-type="entity-link" >Carica_Stalle</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaCentriImpresa.html" data-type="entity-link" >CaricaCentriImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaCombo_SpecieColtivate.html" data-type="entity-link" >CaricaCombo_SpecieColtivate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaCombo_SpecieVegetale_Semente_New.html" data-type="entity-link" >CaricaCombo_SpecieVegetale_Semente_New</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboCodici_Terreno.html" data-type="entity-link" >CaricaComboCodici_Terreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboCopertura.html" data-type="entity-link" >CaricaComboCopertura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboCultivar_conFiltroUtente.html" data-type="entity-link" >CaricaComboCultivar_conFiltroUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboFinalita.html" data-type="entity-link" >CaricaComboFinalita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboFinalita2.html" data-type="entity-link" >CaricaComboFinalita2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboGruppoVarietale.html" data-type="entity-link" >CaricaComboGruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboLavorazioni.html" data-type="entity-link" >CaricaComboLavorazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboLavorazioni_ConFiltro.html" data-type="entity-link" >CaricaComboLavorazioni_ConFiltro</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboPortinnesti.html" data-type="entity-link" >CaricaComboPortinnesti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboRegolamenti.html" data-type="entity-link" >CaricaComboRegolamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboSpecieVegetali_conFiltroUtente.html" data-type="entity-link" >CaricaComboSpecieVegetali_conFiltroUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboStatoImpianto.html" data-type="entity-link" >CaricaComboStatoImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboTipologieSementi.html" data-type="entity-link" >CaricaComboTipologieSementi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaComboTipologieSementiByVeg_Cod.html" data-type="entity-link" >CaricaComboTipologieSementiByVeg_Cod</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDate_Default_In.html" data-type="entity-link" >CaricaDate_Default_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDatiAggiuntivi.html" data-type="entity-link" >CaricaDatiAggiuntivi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDatiApp.html" data-type="entity-link" >CaricaDatiApp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDatiCatastali.html" data-type="entity-link" >CaricaDatiCatastali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDatiDDL.html" data-type="entity-link" >CaricaDatiDDL</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaDatiPrecision_In.html" data-type="entity-link" >CaricaDatiPrecision_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaFormeAllevamento.html" data-type="entity-link" >CaricaFormeAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaImpiantiEsistenti_GIS.html" data-type="entity-link" >CaricaImpiantiEsistenti_GIS</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaImpiantiIrrigazioni.html" data-type="entity-link" >CaricaImpiantiIrrigazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaImpreseCod_In.html" data-type="entity-link" >CaricaImpreseCod_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaOperazioni.html" data-type="entity-link" >CaricaOperazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaRicette.html" data-type="entity-link" >CaricaRicette</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CaricaZoo.html" data-type="entity-link" >CaricaZoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoAppezzamento.html" data-type="entity-link" >CatastoAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoAppezzamento-1.html" data-type="entity-link" >CatastoAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoCampo.html" data-type="entity-link" >CatastoCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoCentroAziendale.html" data-type="entity-link" >CatastoCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoCentroAziendale-1.html" data-type="entity-link" >CatastoCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoEsercizio.html" data-type="entity-link" >CatastoEsercizio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoEsercizio-1.html" data-type="entity-link" >CatastoEsercizio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CatastoEsercizio-2.html" data-type="entity-link" >CatastoEsercizio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Categoria.html" data-type="entity-link" >Categoria</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Categoria-1.html" data-type="entity-link" >Categoria</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CategoriaOperazione.html" data-type="entity-link" >CategoriaOperazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CategoriaOperazione-1.html" data-type="entity-link" >CategoriaOperazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CategorieMagazzino.html" data-type="entity-link" >CategorieMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentriParams.html" data-type="entity-link" >CentriParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentriParams_NG.html" data-type="entity-link" >CentriParams_NG</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendale.html" data-type="entity-link" >CentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendale-1.html" data-type="entity-link" >CentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendale_PK.html" data-type="entity-link" >CentroAziendale_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendale_PK-1.html" data-type="entity-link" >CentroAziendale_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendaleEsternoCollegato.html" data-type="entity-link" >CentroAziendaleEsternoCollegato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendaleEsternoCollegato-1.html" data-type="entity-link" >CentroAziendaleEsternoCollegato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroAziendaleLight.html" data-type="entity-link" >CentroAziendaleLight</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroDiCosto.html" data-type="entity-link" >CentroDiCosto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroDiCosto-1.html" data-type="entity-link" >CentroDiCosto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroDiCosto_CodeType.html" data-type="entity-link" >CentroDiCosto_CodeType</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroDiCosto_CodeType-1.html" data-type="entity-link" >CentroDiCosto_CodeType</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CentroItem.html" data-type="entity-link" >CentroItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CertificatoAnalisi.html" data-type="entity-link" >CertificatoAnalisi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cfg_GestioneSistemaRif_Out.html" data-type="entity-link" >Cfg_GestioneSistemaRif_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CfgAlbero_CfgGisUtente.html" data-type="entity-link" >CfgAlbero_CfgGisUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Chart.html" data-type="entity-link" >Chart</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CheckBoxFlags.html" data-type="entity-link" >CheckBoxFlags</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CheckIsAliveIN.html" data-type="entity-link" >CheckIsAliveIN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CheckTrattamento.html" data-type="entity-link" >CheckTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Chiave_SalvaGrafica_Out.html" data-type="entity-link" >Chiave_SalvaGrafica_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ChiaveAlbero.html" data-type="entity-link" >ChiaveAlbero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ChiaveImpianto.html" data-type="entity-link" >ChiaveImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ChiaveImpianto_In.html" data-type="entity-link" >ChiaveImpianto_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/City.html" data-type="entity-link" >City</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ClasseTessitura.html" data-type="entity-link" >ClasseTessitura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ClasseTessitura-1.html" data-type="entity-link" >ClasseTessitura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cliente_PermessiScrivi.html" data-type="entity-link" >Cliente_PermessiScrivi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cliente_Permesso.html" data-type="entity-link" >Cliente_Permesso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ClientValidationRequest.html" data-type="entity-link" >ClientValidationRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Clouds.html" data-type="entity-link" >Clouds</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiceAnagrafe.html" data-type="entity-link" >CodiceAnagrafe</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiceAnagrafe-1.html" data-type="entity-link" >CodiceAnagrafe</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiceAnagrafeBase.html" data-type="entity-link" >CodiceAnagrafeBase</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiciAnagrafeValori.html" data-type="entity-link" >CodiciAnagrafeValori</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiciAnagrafeValori-1.html" data-type="entity-link" >CodiciAnagrafeValori</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiciNazioniISO3166.html" data-type="entity-link" >CodiciNazioniISO3166</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiciNazioniISO3166-1.html" data-type="entity-link" >CodiciNazioniISO3166</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodiciXOperazione.html" data-type="entity-link" >CodiciXOperazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodificaCACModel.html" data-type="entity-link" >CodificaCACModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodificaMacchineAgeaRequest.html" data-type="entity-link" >CodificaMacchineAgeaRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CodificaMacchineAgeaRequest-1.html" data-type="entity-link" >CodificaMacchineAgeaRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Command.html" data-type="entity-link" >Command</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ComparisonWidgetData.html" data-type="entity-link" >ComparisonWidgetData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ComplexAxis.html" data-type="entity-link" >ComplexAxis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ComplexBand.html" data-type="entity-link" >ComplexBand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ComponentCanDeactivate.html" data-type="entity-link" >ComponentCanDeactivate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ComuneDatiIn.html" data-type="entity-link" >ComuneDatiIn</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneAlbero.html" data-type="entity-link" >ConfigurazioneAlbero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneGisUtente.html" data-type="entity-link" >ConfigurazioneGisUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneProiezione.html" data-type="entity-link" >ConfigurazioneProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneProiezioneGridModel.html" data-type="entity-link" >ConfigurazioneProiezioneGridModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneSuEntitaAttiva.html" data-type="entity-link" >ConfigurazioneSuEntitaAttiva</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioneSuLayer.html" data-type="entity-link" >ConfigurazioneSuLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfigurazioniGisGenerali.html" data-type="entity-link" >ConfigurazioniGisGenerali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConfrontaVisibilitaUtenti.html" data-type="entity-link" >ConfrontaVisibilitaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConnectionConfig.html" data-type="entity-link" >ConnectionConfig</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConnectionParameters.html" data-type="entity-link" >ConnectionParameters</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ConsultaSincroDatiApp.html" data-type="entity-link" >ConsultaSincroDatiApp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contatto.html" data-type="entity-link" >Contatto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contatto-1.html" data-type="entity-link" >Contatto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contatto-2.html" data-type="entity-link" >Contatto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contatto_PK.html" data-type="entity-link" >Contatto_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contatto_PK-1.html" data-type="entity-link" >Contatto_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ContattoAzienda.html" data-type="entity-link" >ContattoAzienda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Contribute.html" data-type="entity-link" >Contribute</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Controllo_Inserimento_Dose_Prodotto.html" data-type="entity-link" >Controllo_Inserimento_Dose_Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Controllo_Sportello.html" data-type="entity-link" >Controllo_Sportello</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cookie.html" data-type="entity-link" >Cookie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Coord.html" data-type="entity-link" >Coord</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Coordinate.html" data-type="entity-link" >Coordinate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoordinateFromImpresa_In.html" data-type="entity-link" >CoordinateFromImpresa_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoordsFromViaCentro_Out.html" data-type="entity-link" >CoordsFromViaCentro_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Copertura.html" data-type="entity-link" >Copertura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Copertura-1.html" data-type="entity-link" >Copertura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CopiaImpostazioniObj.html" data-type="entity-link" >CopiaImpostazioniObj</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CopiaOperazioniDto.html" data-type="entity-link" >CopiaOperazioniDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CopiaSpostaAppezzamenti.html" data-type="entity-link" >CopiaSpostaAppezzamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CopyProfileObj.html" data-type="entity-link" >CopyProfileObj</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWS_Gis_1OfCoreWSGisEndPoints_ModificaImpianto2019InData.html" data-type="entity-link" >CoreWS_Gis_1OfCoreWSGisEndPoints_ModificaImpianto2019InData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWS_Gis_1OfCoreWSGisEndPoints_SalvaNuovoElementoGraficoDaChiaveAlberoInData.html" data-type="entity-link" >CoreWS_Gis_1OfCoreWSGisEndPoints_SalvaNuovoElementoGraficoDaChiaveAlberoInData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWS_Gis_1OfGisDataReadParam.html" data-type="entity-link" >CoreWS_Gis_1OfGisDataReadParam</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWS_Gis_1OfSTBufferGeoJsonPolygonInData.html" data-type="entity-link" >CoreWS_Gis_1OfSTBufferGeoJsonPolygonInData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWSGisEndPoints_ModificaImpianto2019InData.html" data-type="entity-link" >CoreWSGisEndPoints_ModificaImpianto2019InData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CoreWSGisEndPoints_SalvaNuovoElementoGraficoDaChiaveAlberoInData.html" data-type="entity-link" >CoreWSGisEndPoints_SalvaNuovoElementoGraficoDaChiaveAlberoInData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CostoUnitario.html" data-type="entity-link" >CostoUnitario</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CostoUnitario-1.html" data-type="entity-link" >CostoUnitario</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Countries_IN.html" data-type="entity-link" >Countries_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Crea_ricetta.html" data-type="entity-link" >Crea_ricetta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CreaProdotti.html" data-type="entity-link" >CreaProdotti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CreaToken_In.html" data-type="entity-link" >CreaToken_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CriteriRicerca_IN.html" data-type="entity-link" >CriteriRicerca_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CriteriRicerca_OUT.html" data-type="entity-link" >CriteriRicerca_OUT</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Css.html" data-type="entity-link" >Css</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CUAAObj.html" data-type="entity-link" >CUAAObj</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CUAAObj_Detail.html" data-type="entity-link" >CUAAObj_Detail</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CUAAObj_Detail_Notification.html" data-type="entity-link" >CUAAObj_Detail_Notification</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Cultivar_GestioneFiltroUtente_Leggi_IN.html" data-type="entity-link" >Cultivar_GestioneFiltroUtente_Leggi_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Culture.html" data-type="entity-link" >Culture</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/CustomLinkMenu.html" data-type="entity-link" >CustomLinkMenu</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DateDefault_Out.html" data-type="entity-link" >DateDefault_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Dati_Transazione_Commerciale.html" data-type="entity-link" >Dati_Transazione_Commerciale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Dati_Utente_Retail.html" data-type="entity-link" >Dati_Utente_Retail</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiBaseUtente.html" data-type="entity-link" >DatiBaseUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiCatasto.html" data-type="entity-link" >DatiCatasto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiCatastoInput.html" data-type="entity-link" >DatiCatastoInput</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiColoreTema.html" data-type="entity-link" >DatiColoreTema</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiEntitaImpianto.html" data-type="entity-link" >DatiEntitaImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiGruppoUtente.html" data-type="entity-link" >DatiGruppoUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiIscrizioneLibroSoci.html" data-type="entity-link" >DatiIscrizioneLibroSoci</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiLayer.html" data-type="entity-link" >DatiLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiModifica.html" data-type="entity-link" >DatiModifica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiMUZVisibili_Out.html" data-type="entity-link" >DatiMUZVisibili_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiPrevisionaliColtureComplete.html" data-type="entity-link" >DatiPrevisionaliColtureComplete</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiPrevisionaliColtureRequest.html" data-type="entity-link" >DatiPrevisionaliColtureRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiRelativiPercorsoBreadcrumbs.html" data-type="entity-link" >DatiRelativiPercorsoBreadcrumbs</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiServer.html" data-type="entity-link" >DatiServer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiServerRequest.html" data-type="entity-link" >DatiServerRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiStrutturaAttributiLayer.html" data-type="entity-link" >DatiStrutturaAttributiLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DatiTema.html" data-type="entity-link" >DatiTema</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DefaultGenerali_In.html" data-type="entity-link" >DefaultGenerali_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DefaultGeneraliColtura_In.html" data-type="entity-link" >DefaultGeneraliColtura_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DefaultGeneraliData_In.html" data-type="entity-link" >DefaultGeneraliData_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DefaultRapportoContabile.html" data-type="entity-link" >DefaultRapportoContabile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DeleteSomministrazione.html" data-type="entity-link" >DeleteSomministrazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Dependencies.html" data-type="entity-link" >Dependencies</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioFertilizzazione.html" data-type="entity-link" >DettaglioFertilizzazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioSemina.html" data-type="entity-link" >DettaglioSemina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioTipologia.html" data-type="entity-link" >DettaglioTipologia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioTrattamento.html" data-type="entity-link" >DettaglioTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioVarietaPersonalizzato.html" data-type="entity-link" >DettaglioVarietaPersonalizzato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DettaglioVarietaPersonalizzato-1.html" data-type="entity-link" >DettaglioVarietaPersonalizzato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Disciplinare.html" data-type="entity-link" >Disciplinare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Disciplinare-1.html" data-type="entity-link" >Disciplinare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DispositiviXSorgente.html" data-type="entity-link" >DispositiviXSorgente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Dispositivo.html" data-type="entity-link" >Dispositivo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DistanzaDa.html" data-type="entity-link" >DistanzaDa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DistintaProduzione_In.html" data-type="entity-link" >DistintaProduzione_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DistintaProduzioneItem.html" data-type="entity-link" >DistintaProduzioneItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DistintaProduzioneValue.html" data-type="entity-link" >DistintaProduzioneValue</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DittaMacchina.html" data-type="entity-link" >DittaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DittaMacchina-1.html" data-type="entity-link" >DittaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Documento.html" data-type="entity-link" >Documento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Documento-1.html" data-type="entity-link" >Documento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DocumentoAllegato.html" data-type="entity-link" >DocumentoAllegato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DocumentoAllegato-1.html" data-type="entity-link" >DocumentoAllegato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DocumentoAllegato2.html" data-type="entity-link" >DocumentoAllegato2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DocumentoPerImport.html" data-type="entity-link" >DocumentoPerImport</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DocumentoPerScarico.html" data-type="entity-link" >DocumentoPerScarico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DoseEtichetta.html" data-type="entity-link" >DoseEtichetta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DraggableInterface.html" data-type="entity-link" >DraggableInterface</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DrawData.html" data-type="entity-link" >DrawData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownItem.html" data-type="entity-link" >DropdownItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListAttivitaPersonalizzata.html" data-type="entity-link" >DropdownListAttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListAvversita.html" data-type="entity-link" >DropdownListAvversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListCampo.html" data-type="entity-link" >DropdownListCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListDisciplinare.html" data-type="entity-link" >DropdownListDisciplinare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListMagazzino.html" data-type="entity-link" >DropdownListMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/DropdownListSpecieAnimali.html" data-type="entity-link" >DropdownListSpecieAnimali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Effluente.html" data-type="entity-link" >Effluente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElaborazioneGEE.html" data-type="entity-link" >ElaborazioneGEE</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Elemento_CfgSistemaRif_Out.html" data-type="entity-link" >Elemento_CfgSistemaRif_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElementoGraficoPerTipoOggetto_In.html" data-type="entity-link" >ElementoGraficoPerTipoOggetto_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoAlgoritmi.html" data-type="entity-link" >ElencoAlgoritmi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoAllegatiLayer_Out.html" data-type="entity-link" >ElencoAllegatiLayer_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoConfigurazioniProiezione.html" data-type="entity-link" >ElencoConfigurazioniProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoConfigurazioniSuLayer.html" data-type="entity-link" >ElencoConfigurazioniSuLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoDispositivi.html" data-type="entity-link" >ElencoDispositivi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoElaborazioniMassiveDettaglio.html" data-type="entity-link" >ElencoElaborazioniMassiveDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoElaborazioniMassiveDettaglioXAlgoritmo.html" data-type="entity-link" >ElencoElaborazioniMassiveDettaglioXAlgoritmo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoElaborazioniMassiveDettaglioXAlgoritmoXRichiesta.html" data-type="entity-link" >ElencoElaborazioniMassiveDettaglioXAlgoritmoXRichiesta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoElaborazioniMassivePerAzienda.html" data-type="entity-link" >ElencoElaborazioniMassivePerAzienda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoElaborazioniMassivePerTipoCheckList.html" data-type="entity-link" >ElencoElaborazioniMassivePerTipoCheckList</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoEntitaCodXRecuperoStaticMaps.html" data-type="entity-link" >ElencoEntitaCodXRecuperoStaticMaps</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoLogEsecuzioniConfigurazioniProiezione.html" data-type="entity-link" >ElencoLogEsecuzioniConfigurazioniProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoMaschereLayerRaster.html" data-type="entity-link" >ElencoMaschereLayerRaster</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoPermessiConfigurazione.html" data-type="entity-link" >ElencoPermessiConfigurazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoPermessiMaschera.html" data-type="entity-link" >ElencoPermessiMaschera</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoPermessiUtente.html" data-type="entity-link" >ElencoPermessiUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoTipologieLayer.html" data-type="entity-link" >ElencoTipologieLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ElencoUrlFirmati.html" data-type="entity-link" >ElencoUrlFirmati</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Elimina_operazione_multipla.html" data-type="entity-link" >Elimina_operazione_multipla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Elimina_Ricetta_Brogliaccio.html" data-type="entity-link" >Elimina_Ricetta_Brogliaccio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Elimina_Rilievi_Visita.html" data-type="entity-link" >Elimina_Rilievi_Visita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EndpointGEE.html" data-type="entity-link" >EndpointGEE</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EndpointMappeSatellitari.html" data-type="entity-link" >EndpointMappeSatellitari</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EnteRilascio.html" data-type="entity-link" >EnteRilascio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EnteRilascio-1.html" data-type="entity-link" >EnteRilascio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Entita_Impianto.html" data-type="entity-link" >Entita_Impianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Entita_Info_Out.html" data-type="entity-link" >Entita_Info_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EntitaAlgoritmoCartografico.html" data-type="entity-link" >EntitaAlgoritmoCartografico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EntitaInterferenza.html" data-type="entity-link" >EntitaInterferenza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Epoca.html" data-type="entity-link" >Epoca</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Epoca-1.html" data-type="entity-link" >Epoca</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ErroreGias.html" data-type="entity-link" >ErroreGias</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ErroreGias-1.html" data-type="entity-link" >ErroreGias</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Esercizio.html" data-type="entity-link" >Esercizio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Esercizio-1.html" data-type="entity-link" >Esercizio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EsercizioCapoAnimale.html" data-type="entity-link" >EsercizioCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EsercizioCapoAnimale-1.html" data-type="entity-link" >EsercizioCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EsportaShapeEntita_In.html" data-type="entity-link" >EsportaShapeEntita_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ExportDataType.html" data-type="entity-link" >ExportDataType</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ExportDocumenti_In.html" data-type="entity-link" >ExportDocumenti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ExportQdCtoAgea.html" data-type="entity-link" >ExportQdCtoAgea</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ExportQdCToAgeaModel.html" data-type="entity-link" >ExportQdCToAgeaModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ExportQdCtoAgeaPaginated.html" data-type="entity-link" >ExportQdCtoAgeaPaginated</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Fabbricato.html" data-type="entity-link" >Fabbricato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Fabbricato-1.html" data-type="entity-link" >Fabbricato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FabbricatoLight.html" data-type="entity-link" >FabbricatoLight</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FabbricatoLight_PK.html" data-type="entity-link" >FabbricatoLight_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FabbricatoLight_PK-1.html" data-type="entity-link" >FabbricatoLight_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Farmaco.html" data-type="entity-link" >Farmaco</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FarmFilters.html" data-type="entity-link" >FarmFilters</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FarmFilters-1.html" data-type="entity-link" >FarmFilters</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FaseCicloColturale.html" data-type="entity-link" >FaseCicloColturale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FaseCicloColturale-1.html" data-type="entity-link" >FaseCicloColturale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Feature.html" data-type="entity-link" >Feature</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FileParameter.html" data-type="entity-link" >FileParameter</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriAziende.html" data-type="entity-link" >FiltriAziende</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriCampi.html" data-type="entity-link" >FiltriCampi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriCatasto.html" data-type="entity-link" >FiltriCatasto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriCentriAziendali.html" data-type="entity-link" >FiltriCentriAziendali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriGIS.html" data-type="entity-link" >FiltriGIS</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriMovimenti.html" data-type="entity-link" >FiltriMovimenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriPianoColturale.html" data-type="entity-link" >FiltriPianoColturale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriServizi.html" data-type="entity-link" >FiltriServizi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltriTemporali.html" data-type="entity-link" >FiltriTemporali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroAziendeMappaAPP.html" data-type="entity-link" >FiltroAziendeMappaAPP</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroCalcoloNPK.html" data-type="entity-link" >FiltroCalcoloNPK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroFinalita2.html" data-type="entity-link" >FiltroFinalita2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroImpresa.html" data-type="entity-link" >FiltroImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroPC_Finalita_Rer.html" data-type="entity-link" >FiltroPC_Finalita_Rer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroTemporale.html" data-type="entity-link" >FiltroTemporale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroTemporale-1.html" data-type="entity-link" >FiltroTemporale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FiltroValoriParametriQualitativi.html" data-type="entity-link" >FiltroValoriParametriQualitativi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FinalitaMacchina.html" data-type="entity-link" >FinalitaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FinalitaMacchina-1.html" data-type="entity-link" >FinalitaMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FinalitaPianoConcimazione.html" data-type="entity-link" >FinalitaPianoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FinalitaPianoConcimazione-1.html" data-type="entity-link" >FinalitaPianoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FlagFioritura.html" data-type="entity-link" >FlagFioritura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FlagProtetto.html" data-type="entity-link" >FlagProtetto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FlagVisibilitaLayer.html" data-type="entity-link" >FlagVisibilitaLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FormaAllevamento.html" data-type="entity-link" >FormaAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FormaAllevamento-1.html" data-type="entity-link" >FormaAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FormatoDatiModel.html" data-type="entity-link" >FormatoDatiModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FormeGiuridiche.html" data-type="entity-link" >FormeGiuridiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Genere.html" data-type="entity-link" >Genere</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Genere-1.html" data-type="entity-link" >Genere</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Feature_New_1OfGeoJSONAgroGisProp.html" data-type="entity-link" >GeoJson_Feature_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Feature_New_1OfGeoJSONAgroGisProp-1.html" data-type="entity-link" >GeoJson_Feature_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Geometry.html" data-type="entity-link" >GeoJson_Geometry</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Geometry_New.html" data-type="entity-link" >GeoJson_Geometry_New</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Geometry_New-1.html" data-type="entity-link" >GeoJson_Geometry_New</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_New_1OfGeoJSONAgroGisProp.html" data-type="entity-link" >GeoJson_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_New_1OfGeoJSONAgroGisProp-1.html" data-type="entity-link" >GeoJson_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Shape_New_1OfGeoJSONAgroGisProp.html" data-type="entity-link" >GeoJson_Shape_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJson_Shape_New_1OfGeoJSONAgroGisProp-1.html" data-type="entity-link" >GeoJson_Shape_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJSONAgroGisProp.html" data-type="entity-link" >GeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJSONAgroGisProp-1.html" data-type="entity-link" >GeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJSONAgroGisPropTreeNode.html" data-type="entity-link" >GeoJSONAgroGisPropTreeNode</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GeoJSONAgroGisPropTreeNode-1.html" data-type="entity-link" >GeoJSONAgroGisPropTreeNode</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GestioneStampe.html" data-type="entity-link" >GestioneStampe</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Get_Imprese_Impostazioni.html" data-type="entity-link" >Get_Imprese_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Get_Lista_Categorie_Animali.html" data-type="entity-link" >Get_Lista_Categorie_Animali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Get_Lista_IndirizziProd_Animali.html" data-type="entity-link" >Get_Lista_IndirizziProd_Animali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Get_Lista_Razze_Animali.html" data-type="entity-link" >Get_Lista_Razze_Animali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetCampi.html" data-type="entity-link" >GetCampi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetCAP.html" data-type="entity-link" >GetCAP</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetComuni.html" data-type="entity-link" >GetComuni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetElencoDettaglio2.html" data-type="entity-link" >GetElencoDettaglio2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetImpresexAppezza_Movimentati.html" data-type="entity-link" >GetImpresexAppezza_Movimentati</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetImpresexParticelle.html" data-type="entity-link" >GetImpresexParticelle</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetImpresexParticelle_Movimentate.html" data-type="entity-link" >GetImpresexParticelle_Movimentate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetMacchina.html" data-type="entity-link" >GetMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetNewAgenda.html" data-type="entity-link" >GetNewAgenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetProprieta_In.html" data-type="entity-link" >GetProprieta_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetProprieta_Out.html" data-type="entity-link" >GetProprieta_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetProvincie.html" data-type="entity-link" >GetProvincie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetSenzaTrattamentiZooDto.html" data-type="entity-link" >GetSenzaTrattamentiZooDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetSomministrazioneProdottiDto.html" data-type="entity-link" >GetSomministrazioneProdottiDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetStatiModello.html" data-type="entity-link" >GetStatiModello</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetStazionamentoZooDto.html" data-type="entity-link" >GetStazionamentoZooDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GetTrattamentiZooDto.html" data-type="entity-link" >GetTrattamentiZooDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiacenzaZoo.html" data-type="entity-link" >GiacenzaZoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiacenzaZoo_PK.html" data-type="entity-link" >GiacenzaZoo_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiasClusterOptions.html" data-type="entity-link" >GiasClusterOptions</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiasDrawingManagerOptions.html" data-type="entity-link" >GiasDrawingManagerOptions</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiasPalm.html" data-type="entity-link" >GiasPalm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GiasRenderer.html" data-type="entity-link" >GiasRenderer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gis_Sat_Sentinel_Overlay.html" data-type="entity-link" >Gis_Sat_Sentinel_Overlay</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gis_Sat_Sentinel_Overlay_Passaggio.html" data-type="entity-link" >Gis_Sat_Sentinel_Overlay_Passaggio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gis_Sat_Sentinel_Overlay_Sensore.html" data-type="entity-link" >Gis_Sat_Sentinel_Overlay_Sensore</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gis_Sat_Sentinel_Overlay_Sensore_DatoRilevato.html" data-type="entity-link" >Gis_Sat_Sentinel_Overlay_Sensore_DatoRilevato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gis_Sat_Sentinel_Overlay_Tile.html" data-type="entity-link" >Gis_Sat_Sentinel_Overlay_Tile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GisDataReadParam.html" data-type="entity-link" >GisDataReadParam</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GisDataReadRval_New_1OfGeoJSONAgroGisProp.html" data-type="entity-link" >GisDataReadRval_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GisDataReadRval_New_1OfGeoJSONAgroGisProp-1.html" data-type="entity-link" >GisDataReadRval_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GISParticelleCatastaliModel.html" data-type="entity-link" >GISParticelleCatastaliModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GridDto_ChiaveVista.html" data-type="entity-link" >GridDto_ChiaveVista</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GridDto_SalvaVisteWrapper.html" data-type="entity-link" >GridDto_SalvaVisteWrapper</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GridDto_Vista.html" data-type="entity-link" >GridDto_Vista</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GridRowElement.html" data-type="entity-link" >GridRowElement</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Gruppo.html" data-type="entity-link" >Gruppo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoAreaOmogenea.html" data-type="entity-link" >GruppoAreaOmogenea</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoAvversita.html" data-type="entity-link" >GruppoAvversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoFinalita.html" data-type="entity-link" >GruppoFinalita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoFinalita-1.html" data-type="entity-link" >GruppoFinalita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoFinalita-2.html" data-type="entity-link" >GruppoFinalita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoMerceDto.html" data-type="entity-link" >GruppoMerceDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoOperazione.html" data-type="entity-link" >GruppoOperazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoOperazioneBackend.html" data-type="entity-link" >GruppoOperazioneBackend</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoRaccolta.html" data-type="entity-link" >GruppoRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoUtente.html" data-type="entity-link" >GruppoUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoVarietale.html" data-type="entity-link" >GruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/GruppoVarietale-1.html" data-type="entity-link" >GruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HistoryEntry.html" data-type="entity-link" >HistoryEntry</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HorizAxis.html" data-type="entity-link" >HorizAxis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HorizAxis-1.html" data-type="entity-link" >HorizAxis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HubAgeaResult.html" data-type="entity-link" >HubAgeaResult</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HubMeteoData.html" data-type="entity-link" >HubMeteoData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HubMeteoObservation.html" data-type="entity-link" >HubMeteoObservation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/HubMeteoParsedData.html" data-type="entity-link" >HubMeteoParsedData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IActivityImportPlant.html" data-type="entity-link" >IActivityImportPlant</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IAppConfig.html" data-type="entity-link" >IAppConfig</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICategory.html" data-type="entity-link" >ICategory</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IChiaveCompositaPiva.html" data-type="entity-link" >IChiaveCompositaPiva</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICodiceTemplateResult.html" data-type="entity-link" >ICodiceTemplateResult</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ICodiciTemplateService.html" data-type="entity-link" >ICodiciTemplateService</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IConfrontoPianoColturale.html" data-type="entity-link" >IConfrontoPianoColturale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IItemParams.html" data-type="entity-link" >IItemParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ILetturaContatori.html" data-type="entity-link" >ILetturaContatori</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IMenuAgendaGridCommand.html" data-type="entity-link" >IMenuAgendaGridCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Immagine.html" data-type="entity-link" >Immagine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Immagine-1.html" data-type="entity-link" >Immagine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpegniAggiuntiviFacoltativi.html" data-type="entity-link" >ImpegniAggiuntiviFacoltativi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpegniAggiuntiviFacoltativi-1.html" data-type="entity-link" >ImpegniAggiuntiviFacoltativi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpiantiAgendaNG.html" data-type="entity-link" >ImpiantiAgendaNG</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpiantiMappe.html" data-type="entity-link" >ImpiantiMappe</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpiantiParams.html" data-type="entity-link" >ImpiantiParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impianto.html" data-type="entity-link" >Impianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impianto-1.html" data-type="entity-link" >Impianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impianto2010.html" data-type="entity-link" >Impianto2010</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impianto_PK.html" data-type="entity-link" >Impianto_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impianto_PK-1.html" data-type="entity-link" >Impianto_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpiantoGis.html" data-type="entity-link" >ImpiantoGis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpiantoItem.html" data-type="entity-link" >ImpiantoItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImportDemetra.html" data-type="entity-link" >ImportDemetra</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpostaCampoChiaveLayer_In.html" data-type="entity-link" >ImpostaCampoChiaveLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpostaConfigurazioneAlbero.html" data-type="entity-link" >ImpostaConfigurazioneAlbero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpostaVisualizzazioneEtichetta_In.html" data-type="entity-link" >ImpostaVisualizzazioneEtichetta_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impostazione.html" data-type="entity-link" >Impostazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpostazioneCampo.html" data-type="entity-link" >ImpostazioneCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpostazioneSemplice.html" data-type="entity-link" >ImpostazioneSemplice</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Impresa.html" data-type="entity-link" >Impresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpresaDto.html" data-type="entity-link" >ImpresaDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpresaDto2.html" data-type="entity-link" >ImpresaDto2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpresaModel.html" data-type="entity-link" >ImpresaModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpresaModel-1.html" data-type="entity-link" >ImpresaModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ImpresaPadre.html" data-type="entity-link" >ImpresaPadre</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Imprese_Impostazioni.html" data-type="entity-link" >Imprese_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IncludiEsludiImpresaISCC_In.html" data-type="entity-link" >IncludiEsludiImpresaISCC_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IncludiEsludiImpresaISCCDetail_In.html" data-type="entity-link" >IncludiEsludiImpresaISCCDetail_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/InDataUtility.html" data-type="entity-link" >InDataUtility</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Indicatore.html" data-type="entity-link" >Indicatore</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndicatoriData.html" data-type="entity-link" >IndicatoriData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndicatoriWidgetGridColumn.html" data-type="entity-link" >IndicatoriWidgetGridColumn</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndicatoriWidgetGridRow.html" data-type="entity-link" >IndicatoriWidgetGridRow</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Indirizzo.html" data-type="entity-link" >Indirizzo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Indirizzo-1.html" data-type="entity-link" >Indirizzo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Indirizzo-2.html" data-type="entity-link" >Indirizzo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndirizzoAssociato.html" data-type="entity-link" >IndirizzoAssociato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndirizzoAssociato-1.html" data-type="entity-link" >IndirizzoAssociato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndirizzoProduttivo.html" data-type="entity-link" >IndirizzoProduttivo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IndirizzoProduttivo-1.html" data-type="entity-link" >IndirizzoProduttivo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/InfoInterferenze_In.html" data-type="entity-link" >InfoInterferenze_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/InfomodificaOperazioneSingola.html" data-type="entity-link" >InfomodificaOperazioneSingola</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/InformazioniAssistenza.html" data-type="entity-link" >InformazioniAssistenza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/InfoWindowClustererOptions.html" data-type="entity-link" >InfoWindowClustererOptions</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IntervalloTemporale.html" data-type="entity-link" >IntervalloTemporale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IntervalloTemporale-1.html" data-type="entity-link" >IntervalloTemporale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IOutsideResettableComponent.html" data-type="entity-link" >IOutsideResettableComponent</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IPermissionSubmit.html" data-type="entity-link" >IPermissionSubmit</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IPermissionSubmit-1.html" data-type="entity-link" >IPermissionSubmit</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Irrigazione.html" data-type="entity-link" >Irrigazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Irrigazione-1.html" data-type="entity-link" >Irrigazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Istat.html" data-type="entity-link" >Istat</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Istat-1.html" data-type="entity-link" >Istat</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ISubmitComponent.html" data-type="entity-link" >ISubmitComponent</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ITreeService.html" data-type="entity-link" >ITreeService</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUtente.html" data-type="entity-link" >IUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUtenteDTO.html" data-type="entity-link" >IUtenteDTO</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUtenteFinestraTemp.html" data-type="entity-link" >IUtenteFinestraTemp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUtenteImpresa.html" data-type="entity-link" >IUtenteImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/IUtentePassword.html" data-type="entity-link" >IUtentePassword</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Job.html" data-type="entity-link" >Job</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Job-1.html" data-type="entity-link" >Job</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Job_PK.html" data-type="entity-link" >Job_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Job_PK-1.html" data-type="entity-link" >Job_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/JSON_TipologiaxUtente_Result.html" data-type="entity-link" >JSON_TipologiaxUtente_Result</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/KendoColumn.html" data-type="entity-link" >KendoColumn</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/KeyAuth.html" data-type="entity-link" >KeyAuth</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/KeyValuePair_2OfInt32AndString.html" data-type="entity-link" >KeyValuePair_2OfInt32AndString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/KeyValuePair_2OfInt32AndString-1.html" data-type="entity-link" >KeyValuePair_2OfInt32AndString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LatLng.html" data-type="entity-link" >LatLng</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LatLng-1.html" data-type="entity-link" >LatLng</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Lavorazione.html" data-type="entity-link" >Lavorazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Lavorazione-1.html" data-type="entity-link" >Lavorazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Lavorazione-2.html" data-type="entity-link" >Lavorazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LayerTilesDescrizione_In.html" data-type="entity-link" >LayerTilesDescrizione_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_Categorie_Magazzino.html" data-type="entity-link" >Leggi_Categorie_Magazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_FasiCicloColturalexSpecie.html" data-type="entity-link" >Leggi_FasiCicloColturalexSpecie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_GruppoOperazioni.html" data-type="entity-link" >Leggi_GruppoOperazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_Impostazioni.html" data-type="entity-link" >Leggi_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_Macchine_Per_Contatto.html" data-type="entity-link" >Leggi_Macchine_Per_Contatto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_Modalita_Applicazione.html" data-type="entity-link" >Leggi_Modalita_Applicazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_Padri.html" data-type="entity-link" >Leggi_Padri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Leggi_PUA_Regolamenti.html" data-type="entity-link" >Leggi_PUA_Regolamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaAttivita_toKendoGrid.html" data-type="entity-link" >LeggiAgendaAttivita_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaDDT.html" data-type="entity-link" >LeggiAgendaDDT</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaDDT_toKendoGrid.html" data-type="entity-link" >LeggiAgendaDDT_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaGenerica_toKendoGrid.html" data-type="entity-link" >LeggiAgendaGenerica_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaStatistiche.html" data-type="entity-link" >LeggiAgendaStatistiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAgendaVisite_toKendoGrid.html" data-type="entity-link" >LeggiAgendaVisite_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAllegatiDaRicettaDestinazione.html" data-type="entity-link" >LeggiAllegatiDaRicettaDestinazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAllegatiDaRicettaDestinazione_In.html" data-type="entity-link" >LeggiAllegatiDaRicettaDestinazione_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAnagraficaDispositivi.html" data-type="entity-link" >LeggiAnagraficaDispositivi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAnalisiTerreno.html" data-type="entity-link" >LeggiAnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAppezzamento.html" data-type="entity-link" >LeggiAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAree.html" data-type="entity-link" >LeggiAree</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAttivitaPersonalizzata.html" data-type="entity-link" >LeggiAttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAvversita.html" data-type="entity-link" >LeggiAvversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAziende.html" data-type="entity-link" >LeggiAziende</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiAziendexUtenti.html" data-type="entity-link" >LeggiAziendexUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiBudgetTestate.html" data-type="entity-link" >LeggiBudgetTestate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiBufferZone_Out.html" data-type="entity-link" >LeggiBufferZone_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCAC_Codifica_InfoAggiuntive.html" data-type="entity-link" >LeggiCAC_Codifica_InfoAggiuntive</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCampi.html" data-type="entity-link" >LeggiCampi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCategorieMagazzinoImpostazioni.html" data-type="entity-link" >LeggiCategorieMagazzinoImpostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCentriAziendali.html" data-type="entity-link" >LeggiCentriAziendali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCentriConFiltroUtente.html" data-type="entity-link" >LeggiCentriConFiltroUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiClasseTessitura.html" data-type="entity-link" >LeggiClasseTessitura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCodiciUsatixEntitaAnagrafe_IN.html" data-type="entity-link" >LeggiCodiciUsatixEntitaAnagrafe_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiComuni_IN.html" data-type="entity-link" >LeggiComuni_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiConduzione.html" data-type="entity-link" >LeggiConduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiConfigurazioniGeneraliGis.html" data-type="entity-link" >LeggiConfigurazioniGeneraliGis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiContatori_IN.html" data-type="entity-link" >LeggiContatori_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiContattiMacchina.html" data-type="entity-link" >LeggiContattiMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiConto.html" data-type="entity-link" >LeggiConto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiCultivar.html" data-type="entity-link" >LeggiCultivar</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDateImpiantiPerFiltroTemporale_In.html" data-type="entity-link" >LeggiDateImpiantiPerFiltroTemporale_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDatiLayer_In.html" data-type="entity-link" >LeggiDatiLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDatiMUZVisibili_In.html" data-type="entity-link" >LeggiDatiMUZVisibili_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDefault_DPI_QdC.html" data-type="entity-link" >LeggiDefault_DPI_QdC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDettaglio.html" data-type="entity-link" >LeggiDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDettaglioSpecificoArete.html" data-type="entity-link" >LeggiDettaglioSpecificoArete</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDisciplinari.html" data-type="entity-link" >LeggiDisciplinari</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDisciplinariCombo.html" data-type="entity-link" >LeggiDisciplinariCombo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDisponibilitaAttualeFertilizzante.html" data-type="entity-link" >LeggiDisponibilitaAttualeFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiDoseConsentitaDiserbo.html" data-type="entity-link" >LeggiDoseConsentitaDiserbo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiEfficienza.html" data-type="entity-link" >LeggiEfficienza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoCompletoProdotti.html" data-type="entity-link" >LeggiElencoCompletoProdotti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoCompletoProdottiMultiCategoria.html" data-type="entity-link" >LeggiElencoCompletoProdottiMultiCategoria</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoConfigurazioni_In.html" data-type="entity-link" >LeggiElencoConfigurazioni_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoElaborazioniMassive_In.html" data-type="entity-link" >LeggiElencoElaborazioniMassive_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoElaborazioniMassive_Out.html" data-type="entity-link" >LeggiElencoElaborazioniMassive_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiElencoProdotti_APP.html" data-type="entity-link" >LeggiElencoProdotti_APP</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiEntita_In.html" data-type="entity-link" >LeggiEntita_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiEpoche.html" data-type="entity-link" >LeggiEpoche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiFabbricati.html" data-type="entity-link" >LeggiFabbricati</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiFabbricatiOmni.html" data-type="entity-link" >LeggiFabbricatiOmni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiFiltro.html" data-type="entity-link" >LeggiFiltro</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiFinalita.html" data-type="entity-link" >LeggiFinalita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiFormaAllevamento.html" data-type="entity-link" >LeggiFormaAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGenericoCategorieMagazzino.html" data-type="entity-link" >LeggiGenericoCategorieMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGiacenzaFarmaci.html" data-type="entity-link" >LeggiGiacenzaFarmaci</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGiacenzaFarmaci-1.html" data-type="entity-link" >LeggiGiacenzaFarmaci</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGiacenzeZoo.html" data-type="entity-link" >LeggiGiacenzeZoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGiacenzeZooDto.html" data-type="entity-link" >LeggiGiacenzeZooDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGiacenzeZooFirstSommDto.html" data-type="entity-link" >LeggiGiacenzeZooFirstSommDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGisPurpose_In.html" data-type="entity-link" >LeggiGisPurpose_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGriglia.html" data-type="entity-link" >LeggiGriglia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGruppiVarietali_IN.html" data-type="entity-link" >LeggiGruppiVarietali_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiGruppoVarietale.html" data-type="entity-link" >LeggiGruppoVarietale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiIAF.html" data-type="entity-link" >LeggiIAF</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiImpianti.html" data-type="entity-link" >LeggiImpianti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiImpianto.html" data-type="entity-link" >LeggiImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiImpostazioniAvanzateLayer.html" data-type="entity-link" >LeggiImpostazioniAvanzateLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiImpreseParametri.html" data-type="entity-link" >LeggiImpreseParametri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiIndirizziCentro.html" data-type="entity-link" >LeggiIndirizziCentro</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiIndirizziContatto.html" data-type="entity-link" >LeggiIndirizziContatto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiInizializza_QdC.html" data-type="entity-link" >LeggiInizializza_QdC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiInvestimentoCatastale.html" data-type="entity-link" >LeggiInvestimentoCatastale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiInvestimentoCatastaleCampo.html" data-type="entity-link" >LeggiInvestimentoCatastaleCampo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiIVA_Aliquote.html" data-type="entity-link" >LeggiIVA_Aliquote</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiLayerTilesDescrizione_In.html" data-type="entity-link" >LeggiLayerTilesDescrizione_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiLink_Operazione.html" data-type="entity-link" >LeggiLink_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiListaAnalisiTerreno.html" data-type="entity-link" >LeggiListaAnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMagazzini_QdC.html" data-type="entity-link" >LeggiMagazzini_QdC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMenuRicette.html" data-type="entity-link" >LeggiMenuRicette</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMisuraPerAvversitaAnagrafica_In.html" data-type="entity-link" >LeggiMisuraPerAvversitaAnagrafica_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMisuraPerIndiciMaturitaAnagrafiche_IN.html" data-type="entity-link" >LeggiMisuraPerIndiciMaturitaAnagrafiche_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMisureAvversita.html" data-type="entity-link" >LeggiMisureAvversita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiModello.html" data-type="entity-link" >LeggiModello</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiMUZ_In.html" data-type="entity-link" >LeggiMUZ_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiNote.html" data-type="entity-link" >LeggiNote</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiOperazione.html" data-type="entity-link" >LeggiOperazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiOperazioni.html" data-type="entity-link" >LeggiOperazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiOperazioni_IN.html" data-type="entity-link" >LeggiOperazioni_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiParametriAnalisiDettagli.html" data-type="entity-link" >LeggiParametriAnalisiDettagli</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiParticelleCatastali_toKendoGrid.html" data-type="entity-link" >LeggiParticelleCatastali_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPermessiLayerGruppiUtente_In.html" data-type="entity-link" >LeggiPermessiLayerGruppiUtente_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPermessiLayerGruppiUtente_Out.html" data-type="entity-link" >LeggiPermessiLayerGruppiUtente_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPermessiLayerUtenti_In.html" data-type="entity-link" >LeggiPermessiLayerUtenti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPermessiLayerUtenti_Out.html" data-type="entity-link" >LeggiPermessiLayerUtenti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPianiCampionamento.html" data-type="entity-link" >LeggiPianiCampionamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPianoConti.html" data-type="entity-link" >LeggiPianoConti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPianoContixConti.html" data-type="entity-link" >LeggiPianoContixConti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPortinnesto.html" data-type="entity-link" >LeggiPortinnesto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPrescrizioni.html" data-type="entity-link" >LeggiPrescrizioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiProdotti.html" data-type="entity-link" >LeggiProdotti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiProfilazione.html" data-type="entity-link" >LeggiProfilazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiProvince_IN.html" data-type="entity-link" >LeggiProvince_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiPUA.html" data-type="entity-link" >LeggiPUA</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRapportoDocumenti.html" data-type="entity-link" >LeggiRapportoDocumenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRapportoSpecifico.html" data-type="entity-link" >LeggiRapportoSpecifico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRegioni_IN.html" data-type="entity-link" >LeggiRegioni_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRicetteBrogliaccio_toKendoGrid.html" data-type="entity-link" >LeggiRicetteBrogliaccio_toKendoGrid</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRilievi.html" data-type="entity-link" >LeggiRilievi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiRilieviProduzione.html" data-type="entity-link" >LeggiRilieviProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSchemiAnalisi.html" data-type="entity-link" >LeggiSchemiAnalisi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiScriviVisibilitaUtente.html" data-type="entity-link" >LeggiScriviVisibilitaUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiScriviVisibilitaUtenti.html" data-type="entity-link" >LeggiScriviVisibilitaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiServizi_IN.html" data-type="entity-link" >LeggiServizi_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiServiziStati_IN.html" data-type="entity-link" >LeggiServiziStati_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSistemiRiferimento_In.html" data-type="entity-link" >LeggiSistemiRiferimento_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSottogruppiStalla.html" data-type="entity-link" >LeggiSottogruppiStalla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSpecie.html" data-type="entity-link" >LeggiSpecie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSpecieColtivate.html" data-type="entity-link" >LeggiSpecieColtivate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSpecieQdC.html" data-type="entity-link" >LeggiSpecieQdC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiSpecieVegetali_IN.html" data-type="entity-link" >LeggiSpecieVegetali_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiStalle.html" data-type="entity-link" >LeggiStalle</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiStimeProduzione.html" data-type="entity-link" >LeggiStimeProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiTestata.html" data-type="entity-link" >LeggiTestata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiTipiOggettoPerElementoGrafico.html" data-type="entity-link" >LeggiTipiOggettoPerElementoGrafico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiTipo.html" data-type="entity-link" >LeggiTipo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiUltimo_Magazzino_Prodotto_Movimentato.html" data-type="entity-link" >LeggiUltimo_Magazzino_Prodotto_Movimentato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiUMA_Lavorazioni.html" data-type="entity-link" >LeggiUMA_Lavorazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiUMA_Macrousi.html" data-type="entity-link" >LeggiUMA_Macrousi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiUnitaMisuraCategoria.html" data-type="entity-link" >LeggiUnitaMisuraCategoria</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiValoriImpostazioni_AziendeCentri.html" data-type="entity-link" >LeggiValoriImpostazioni_AziendeCentri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiVincoli.html" data-type="entity-link" >LeggiVincoli</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LeggiVisite.html" data-type="entity-link" >LeggiVisite</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LetturaDatiElaboratiSuSensoreListaValori_In.html" data-type="entity-link" >LetturaDatiElaboratiSuSensoreListaValori_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LettureContatori_IN.html" data-type="entity-link" >LettureContatori_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LettureContatoriFiltersForm.html" data-type="entity-link" >LettureContatoriFiltersForm</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LettureTecniciInCampo_In.html" data-type="entity-link" >LettureTecniciInCampo_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LicenzaColtivazione.html" data-type="entity-link" >LicenzaColtivazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LicenzaColtivazione-1.html" data-type="entity-link" >LicenzaColtivazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LimitiValoriTema.html" data-type="entity-link" >LimitiValoriTema</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LinkedContribute_1OfKeyValuePair_2OfInt32AndString.html" data-type="entity-link" >LinkedContribute_1OfKeyValuePair_2OfInt32AndString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LinkedContribute_1OfKeyValuePair_2OfInt32AndString-1.html" data-type="entity-link" >LinkedContribute_1OfKeyValuePair_2OfInt32AndString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LinkedMachine_1OfAppezzamento_PK.html" data-type="entity-link" >LinkedMachine_1OfAppezzamento_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LinkedMachine_1OfAppezzamento_PK-1.html" data-type="entity-link" >LinkedMachine_1OfAppezzamento_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LinkMenu.html" data-type="entity-link" >LinkMenu</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/List.html" data-type="entity-link" >List</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Lista_DatiPrecision_XmlAllegati_Out.html" data-type="entity-link" >Lista_DatiPrecision_XmlAllegati_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Lista_GisSat_SentinelOverlay_Out.html" data-type="entity-link" >Lista_GisSat_SentinelOverlay_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ListaGruppiUtente.html" data-type="entity-link" >ListaGruppiUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ListaMisuraPerIndiciMaturitaAnagrafica.html" data-type="entity-link" >ListaMisuraPerIndiciMaturitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ListaMisurePerAvversitaAnagrafica.html" data-type="entity-link" >ListaMisurePerAvversitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ListaMisurePerAvversitaAnagraficaExtended.html" data-type="entity-link" >ListaMisurePerAvversitaAnagraficaExtended</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ListaUtenti.html" data-type="entity-link" >ListaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LoadingStageState.html" data-type="entity-link" >LoadingStageState</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Log.html" data-type="entity-link" >Log</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LogEsecuzioniConfigurazioniProiezione.html" data-type="entity-link" >LogEsecuzioniConfigurazioniProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LogEventiEntity.html" data-type="entity-link" >LogEventiEntity</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/LoginModel.html" data-type="entity-link" >LoginModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Macchina.html" data-type="entity-link" >Macchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchinaDettaglio.html" data-type="entity-link" >MacchinaDettaglio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchinaGerarchia.html" data-type="entity-link" >MacchinaGerarchia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchinaGerarchia-1.html" data-type="entity-link" >MacchinaGerarchia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Macchine.html" data-type="entity-link" >Macchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Macchine-1.html" data-type="entity-link" >Macchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineCodificaAgea.html" data-type="entity-link" >MacchineCodificaAgea</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineCodificaAgea-1.html" data-type="entity-link" >MacchineCodificaAgea</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineDettaglio1.html" data-type="entity-link" >MacchineDettaglio1</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineDettaglio1-1.html" data-type="entity-link" >MacchineDettaglio1</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineDettaglio2.html" data-type="entity-link" >MacchineDettaglio2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineDettaglio2-1.html" data-type="entity-link" >MacchineDettaglio2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MacchineXContatti.html" data-type="entity-link" >MacchineXContatti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MachineDetail.html" data-type="entity-link" >MachineDetail</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MachinesXTypeReadParams.html" data-type="entity-link" >MachinesXTypeReadParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Macrouso.html" data-type="entity-link" >Macrouso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Macrouso-1.html" data-type="entity-link" >Macrouso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Main.html" data-type="entity-link" >Main</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Manutenzione.html" data-type="entity-link" >Manutenzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MappePrescrizioneGeoJson.html" data-type="entity-link" >MappePrescrizioneGeoJson</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MappeSatellitariData.html" data-type="entity-link" >MappeSatellitariData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Marker.html" data-type="entity-link" >Marker</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MascheraLayerRaster.html" data-type="entity-link" >MascheraLayerRaster</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MaschereLayerFiltro_In.html" data-type="entity-link" >MaschereLayerFiltro_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Mask.html" data-type="entity-link" >Mask</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Mdi.html" data-type="entity-link" >Mdi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Message.html" data-type="entity-link" >Message</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Messaggio_Utente_Permessi.html" data-type="entity-link" >Messaggio_Utente_Permessi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Messaggio_Utente_Permessi-1.html" data-type="entity-link" >Messaggio_Utente_Permessi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MessaggioEsecuzione.html" data-type="entity-link" >MessaggioEsecuzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Meteo.html" data-type="entity-link" >Meteo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoChart.html" data-type="entity-link" >MeteoChart</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoChart-1.html" data-type="entity-link" >MeteoChart</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoData.html" data-type="entity-link" >MeteoData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoData-1.html" data-type="entity-link" >MeteoData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoObservation.html" data-type="entity-link" >MeteoObservation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoWidgetFullData.html" data-type="entity-link" >MeteoWidgetFullData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MeteoWidgetFullForecast.html" data-type="entity-link" >MeteoWidgetFullForecast</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MetodoProduzione.html" data-type="entity-link" >MetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MetodoProduzione-1.html" data-type="entity-link" >MetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MisuraPerAvversitaAnagrafica.html" data-type="entity-link" >MisuraPerAvversitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MisuraPerAvversitaAnagrafica_In.html" data-type="entity-link" >MisuraPerAvversitaAnagrafica_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MisuraPerAvversitaAnagraficaExtended.html" data-type="entity-link" >MisuraPerAvversitaAnagraficaExtended</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MisuraPerIndiciMaturita_In.html" data-type="entity-link" >MisuraPerIndiciMaturita_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MisuraPerIndiciMaturitaAnagrafica.html" data-type="entity-link" >MisuraPerIndiciMaturitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ModificaListaRicetteQdC.html" data-type="entity-link" >ModificaListaRicetteQdC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ModificaMultipla_Attivita.html" data-type="entity-link" >ModificaMultipla_Attivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ModifichePermessiConfigurazione.html" data-type="entity-link" >ModifichePermessiConfigurazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ModifichePermessiMaschera.html" data-type="entity-link" >ModifichePermessiMaschera</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MonitoraggioStation.html" data-type="entity-link" >MonitoraggioStation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MovimentoDiMagazzino.html" data-type="entity-link" >MovimentoDiMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MultiColumnComboboxDose_Etichetta.html" data-type="entity-link" >MultiColumnComboboxDose_Etichetta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MultiColumnComboboxFertilizzazione.html" data-type="entity-link" >MultiColumnComboboxFertilizzazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MultiColumnComboboxProdotto.html" data-type="entity-link" >MultiColumnComboboxProdotto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MultiColumnComboboxSemina.html" data-type="entity-link" >MultiColumnComboboxSemina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MultiColumnComboboxTrattamento.html" data-type="entity-link" >MultiColumnComboboxTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MUZ.html" data-type="entity-link" >MUZ</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MUZ_Appezzamento.html" data-type="entity-link" >MUZ_Appezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MuzAnalysis.html" data-type="entity-link" >MuzAnalysis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NotaProfilazione.html" data-type="entity-link" >NotaProfilazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteIntervento.html" data-type="entity-link" >NoteIntervento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteIntervento-1.html" data-type="entity-link" >NoteIntervento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteIntervento-2.html" data-type="entity-link" >NoteIntervento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteInterventoGruppi.html" data-type="entity-link" >NoteInterventoGruppi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteInterventoGruppi-1.html" data-type="entity-link" >NoteInterventoGruppi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NoteVisibili_In.html" data-type="entity-link" >NoteVisibili_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Notifica_Utente_In.html" data-type="entity-link" >Notifica_Utente_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NotificaCUAA.html" data-type="entity-link" >NotificaCUAA</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NuovaElaborazioneGEE.html" data-type="entity-link" >NuovaElaborazioneGEE</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NuovaEmail_In.html" data-type="entity-link" >NuovaEmail_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Nuovo_Utente_Retail_In.html" data-type="entity-link" >Nuovo_Utente_Retail_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Obj_SalvaGrafica.html" data-type="entity-link" >Obj_SalvaGrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjDisciplinare.html" data-type="entity-link" >ObjDisciplinare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjInputHTML_Out.html" data-type="entity-link" >ObjInputHTML_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjOptionHTML_Out.html" data-type="entity-link" >ObjOptionHTML_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjParams_Agenda.html" data-type="entity-link" >ObjParams_Agenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjParams_Analisi2010.html" data-type="entity-link" >ObjParams_Analisi2010</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjParams_AuditPUA.html" data-type="entity-link" >ObjParams_AuditPUA</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ObjParams_Concimazione2017.html" data-type="entity-link" >ObjParams_Concimazione2017</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Operation.html" data-type="entity-link" >Operation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Operation2.html" data-type="entity-link" >Operation2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Operazione.html" data-type="entity-link" >Operazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Operazione_Agenda.html" data-type="entity-link" >Operazione_Agenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioneAgenda.html" data-type="entity-link" >OperazioneAgenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioneCausale_In.html" data-type="entity-link" >OperazioneCausale_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioneItem.html" data-type="entity-link" >OperazioneItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioneMascheraLayerRaster_In.html" data-type="entity-link" >OperazioneMascheraLayerRaster_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioniAgendaInput.html" data-type="entity-link" >OperazioniAgendaInput</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioniBookmark_In.html" data-type="entity-link" >OperazioniBookmark_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioniGruppiMUZ_In.html" data-type="entity-link" >OperazioniGruppiMUZ_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OperazioniParams.html" data-type="entity-link" >OperazioniParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OpzioniWatable.html" data-type="entity-link" >OpzioniWatable</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OpzioniWatable-1.html" data-type="entity-link" >OpzioniWatable</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OrganizationIdentity.html" data-type="entity-link" >OrganizationIdentity</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OrientamentoTecnicoEconomico.html" data-type="entity-link" >OrientamentoTecnicoEconomico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OrientamentoTecnicoEconomico-1.html" data-type="entity-link" >OrientamentoTecnicoEconomico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/OrigineDatiModel.html" data-type="entity-link" >OrigineDatiModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PadriGerarchia2.html" data-type="entity-link" >PadriGerarchia2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PaginaLinkGestioneMagazziniQueryStringDto.html" data-type="entity-link" >PaginaLinkGestioneMagazziniQueryStringDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parameters.html" data-type="entity-link" >Parameters</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parametri.html" data-type="entity-link" >Parametri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parametri_Aggiuntivi_Attivita.html" data-type="entity-link" >Parametri_Aggiuntivi_Attivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parametri_Aggiuntivi_ControllaDosi.html" data-type="entity-link" >Parametri_Aggiuntivi_ControllaDosi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parametri_ObjParametriAgenda_NG.html" data-type="entity-link" >Parametri_ObjParametriAgenda_NG</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parametri_ObjParametriAgenda_NG_GestioneRichieste.html" data-type="entity-link" >Parametri_ObjParametriAgenda_NG_GestioneRichieste</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriAggiuntivi_QueryString.html" data-type="entity-link" >ParametriAggiuntivi_QueryString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriAlgoritmoProiezione.html" data-type="entity-link" >ParametriAlgoritmoProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriAlgoritmoProiezioneLayer.html" data-type="entity-link" >ParametriAlgoritmoProiezioneLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriConnessioni_In.html" data-type="entity-link" >ParametriConnessioni_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriProiezioneLayer.html" data-type="entity-link" >ParametriProiezioneLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriProiezioneLayerComplete.html" data-type="entity-link" >ParametriProiezioneLayerComplete</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParametriTipoConfronto.html" data-type="entity-link" >ParametriTipoConfronto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParamsAlberoAnagrafe.html" data-type="entity-link" >ParamsAlberoAnagrafe</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parco_Macchine_Costo.html" data-type="entity-link" >Parco_Macchine_Costo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Parco_Macchine_Manutenzione.html" data-type="entity-link" >Parco_Macchine_Manutenzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParcoMacchine.html" data-type="entity-link" >ParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParcoMacchine-1.html" data-type="entity-link" >ParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParcoMacchineCaratteristiche.html" data-type="entity-link" >ParcoMacchineCaratteristiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParcoMacchineCaratteristiche-1.html" data-type="entity-link" >ParcoMacchineCaratteristiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParcoMacchineDto.html" data-type="entity-link" >ParcoMacchineDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastali.html" data-type="entity-link" >ParticelleCatastali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastali-1.html" data-type="entity-link" >ParticelleCatastali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastali_MUZ.html" data-type="entity-link" >ParticelleCatastali_MUZ</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastali_PK.html" data-type="entity-link" >ParticelleCatastali_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastali_PK-1.html" data-type="entity-link" >ParticelleCatastali_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliClassamento.html" data-type="entity-link" >ParticelleCatastaliClassamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliClassamento-1.html" data-type="entity-link" >ParticelleCatastaliClassamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliMacrouso.html" data-type="entity-link" >ParticelleCatastaliMacrouso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliMacrouso-1.html" data-type="entity-link" >ParticelleCatastaliMacrouso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliMetodoProduzione.html" data-type="entity-link" >ParticelleCatastaliMetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliMetodoProduzione-1.html" data-type="entity-link" >ParticelleCatastaliMetodoProduzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliZona.html" data-type="entity-link" >ParticelleCatastaliZona</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ParticelleCatastaliZona-1.html" data-type="entity-link" >ParticelleCatastaliZona</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PassaggioSitoAgenda.html" data-type="entity-link" >PassaggioSitoAgenda</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PassaggioSitoPianoConcimazione.html" data-type="entity-link" >PassaggioSitoPianoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PdcAnalisi.html" data-type="entity-link" >PdcAnalisi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PdcCampione.html" data-type="entity-link" >PdcCampione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PdcDettaglio_1OfGiacenzaZoo.html" data-type="entity-link" >PdcDettaglio_1OfGiacenzaZoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PdcDettaglio_1OfImpianto.html" data-type="entity-link" >PdcDettaglio_1OfImpianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PdcLFO.html" data-type="entity-link" >PdcLFO</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PermessiUtenteSuSingoloLayer_In.html" data-type="entity-link" >PermessiUtenteSuSingoloLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PermessiXGruppiUtente.html" data-type="entity-link" >PermessiXGruppiUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PermessiXUtente.html" data-type="entity-link" >PermessiXUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PermessoConfigurazione.html" data-type="entity-link" >PermessoConfigurazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PermessoMaschera.html" data-type="entity-link" >PermessoMaschera</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PfIndiciRischio_In.html" data-type="entity-link" >PfIndiciRischio_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PfPrescriptionUpdate_In.html" data-type="entity-link" >PfPrescriptionUpdate_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PfRateoSrv_In.html" data-type="entity-link" >PfRateoSrv_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PianoConcimazioneGEE.html" data-type="entity-link" >PianoConcimazioneGEE</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PianoDiCampionamento.html" data-type="entity-link" >PianoDiCampionamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Piva_Sa_Cod.html" data-type="entity-link" >Piva_Sa_Cod</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Plot.html" data-type="entity-link" >Plot</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PopolaRilievo.html" data-type="entity-link" >PopolaRilievo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Portinnesto.html" data-type="entity-link" >Portinnesto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Portinnesto-1.html" data-type="entity-link" >Portinnesto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PossessoParticella.html" data-type="entity-link" >PossessoParticella</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PossessoParticella-1.html" data-type="entity-link" >PossessoParticella</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Preferiti_in.html" data-type="entity-link" >Preferiti_in</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PrevisioniChatGPT_IN.html" data-type="entity-link" >PrevisioniChatGPT_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PrincipioAttivo.html" data-type="entity-link" >PrincipioAttivo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Prodotti.html" data-type="entity-link" >Prodotti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Prodotti_x_CAC.html" data-type="entity-link" >Prodotti_x_CAC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Prodotto.html" data-type="entity-link" >Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Prodotto-1.html" data-type="entity-link" >Prodotto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ProiezioneLayer.html" data-type="entity-link" >ProiezioneLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ProseguiSelezionati.html" data-type="entity-link" >ProseguiSelezionati</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ProtocolloInCorso.html" data-type="entity-link" >ProtocolloInCorso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ProvenienzaSeme.html" data-type="entity-link" >ProvenienzaSeme</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ProvenienzaSeme-1.html" data-type="entity-link" >ProvenienzaSeme</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Pua.html" data-type="entity-link" >Pua</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Pua-1.html" data-type="entity-link" >Pua</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RaggruppamentiColturaliDPI.html" data-type="entity-link" >RaggruppamentiColturaliDPI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RaggruppamentiColturaliDPI-1.html" data-type="entity-link" >RaggruppamentiColturaliDPI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RapportoContabile.html" data-type="entity-link" >RapportoContabile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RapportoContabile-1.html" data-type="entity-link" >RapportoContabile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RasterInfoClick_In.html" data-type="entity-link" >RasterInfoClick_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RasterInfoClick_Out.html" data-type="entity-link" >RasterInfoClick_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RasterMask.html" data-type="entity-link" >RasterMask</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RasterParameterVisualizationLayer.html" data-type="entity-link" >RasterParameterVisualizationLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Razza.html" data-type="entity-link" >Razza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Razza-1.html" data-type="entity-link" >Razza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ReadDettaglioAziendale.html" data-type="entity-link" >ReadDettaglioAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ReadRequisitiStabilimento.html" data-type="entity-link" >ReadRequisitiStabilimento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RegistrazioneContabile.html" data-type="entity-link" >RegistrazioneContabile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RegistrazioneContabile-1.html" data-type="entity-link" >RegistrazioneContabile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Regolamenti.html" data-type="entity-link" >Regolamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Regolamenti-1.html" data-type="entity-link" >Regolamenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RegolamentoConcimazione.html" data-type="entity-link" >RegolamentoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RegolamentoConcimazione-1.html" data-type="entity-link" >RegolamentoConcimazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ReportImpiegoProdottiFitosanitari_IN.html" data-type="entity-link" >ReportImpiegoProdottiFitosanitari_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RequestImportVisibilita.html" data-type="entity-link" >RequestImportVisibilita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RequestNDistribuito.html" data-type="entity-link" >RequestNDistribuito</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RequestUtente.html" data-type="entity-link" >RequestUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RequisitiStabilimentoContrattoModel.html" data-type="entity-link" >RequisitiStabilimentoContrattoModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RequisitiStabilimentoModel.html" data-type="entity-link" >RequisitiStabilimentoModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Result.html" data-type="entity-link" >Result</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Ricetta_numero_default.html" data-type="entity-link" >Ricetta_numero_default</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Ricetta_Operazione.html" data-type="entity-link" >Ricetta_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RicettaOperazione2WorkOrderKey.html" data-type="entity-link" >RicettaOperazione2WorkOrderKey</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Ricette_Operazioni.html" data-type="entity-link" >Ricette_Operazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RichiediDatiIOT.html" data-type="entity-link" >RichiediDatiIOT</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RichiestaAbacoUrl_In.html" data-type="entity-link" >RichiestaAbacoUrl_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RichiestaSignedUrl.html" data-type="entity-link" >RichiestaSignedUrl</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RichiestaSignedUrl_In.html" data-type="entity-link" >RichiestaSignedUrl_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RiepilogoMeteoModel.html" data-type="entity-link" >RiepilogoMeteoModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RifCentroAziendale.html" data-type="entity-link" >RifCentroAziendale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RifFabbricato.html" data-type="entity-link" >RifFabbricato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RifImpresa.html" data-type="entity-link" >RifImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RigheSelezionateDto.html" data-type="entity-link" >RigheSelezionateDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RilevamentoDiMagazzino.html" data-type="entity-link" >RilevamentoDiMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RimuoviVisibilitaUtente.html" data-type="entity-link" >RimuoviVisibilitaUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Rinnova_Utente_Retail_In.html" data-type="entity-link" >Rinnova_Utente_Retail_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Riporta_Utente_Retail_In.html" data-type="entity-link" >Riporta_Utente_Retail_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Risorsa.html" data-type="entity-link" >Risorsa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Risorsa-1.html" data-type="entity-link" >Risorsa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RisorsaAcqua.html" data-type="entity-link" >RisorsaAcqua</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RisorsaProdotto.html" data-type="entity-link" >RisorsaProdotto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RisorsaZootecnica.html" data-type="entity-link" >RisorsaZootecnica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RisorseUmane.html" data-type="entity-link" >RisorseUmane</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RisorseUmane-1.html" data-type="entity-link" >RisorseUmane</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard.html" data-type="entity-link" >RispostaStandard</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard-1.html" data-type="entity-link" >RispostaStandard</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAggiornaFiltroImpianti_Out.html" data-type="entity-link" >RispostaStandard_1OfAggiornaFiltroImpianti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAlberoMenu.html" data-type="entity-link" >RispostaStandard_1OfAlberoMenu</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAllegatoFile.html" data-type="entity-link" >RispostaStandard_1OfAllegatoFile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAnalisiMeteo_Out.html" data-type="entity-link" >RispostaStandard_1OfAnalisiMeteo_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAppezzamento.html" data-type="entity-link" >RispostaStandard_1OfAppezzamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAppezzamentoJoinDescrizioni.html" data-type="entity-link" >RispostaStandard_1OfAppezzamentoJoinDescrizioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfApriSitoAnalisi_Out.html" data-type="entity-link" >RispostaStandard_1OfApriSitoAnalisi_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfAttivita.html" data-type="entity-link" >RispostaStandard_1OfAttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfBaseCodeDescrOf.html" data-type="entity-link" >RispostaStandard_1OfBaseCodeDescrOf</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfBoolean.html" data-type="entity-link" >RispostaStandard_1OfBoolean</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfBreadcrumbsInfo.html" data-type="entity-link" >RispostaStandard_1OfBreadcrumbsInfo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCancella_EntitaGraf_Out.html" data-type="entity-link" >RispostaStandard_1OfCancella_EntitaGraf_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCancella_Out.html" data-type="entity-link" >RispostaStandard_1OfCancella_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCfg_GestioneSistemaRif_Out.html" data-type="entity-link" >RispostaStandard_1OfCfg_GestioneSistemaRif_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCfgAlbero_CfgGisUtente.html" data-type="entity-link" >RispostaStandard_1OfCfgAlbero_CfgGisUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfChiaveAlbero.html" data-type="entity-link" >RispostaStandard_1OfChiaveAlbero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfConfigurazioneGisUtente.html" data-type="entity-link" >RispostaStandard_1OfConfigurazioneGisUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfConfigurazioniGisGenerali.html" data-type="entity-link" >RispostaStandard_1OfConfigurazioniGisGenerali</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfContributeOf.html" data-type="entity-link" >RispostaStandard_1OfContributeOf</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCoordsFromViaCentro_Out.html" data-type="entity-link" >RispostaStandard_1OfCoordsFromViaCentro_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfCriteriRicerca_OUT.html" data-type="entity-link" >RispostaStandard_1OfCriteriRicerca_OUT</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfDateDefault_Out.html" data-type="entity-link" >RispostaStandard_1OfDateDefault_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfDateTime.html" data-type="entity-link" >RispostaStandard_1OfDateTime</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfDatiServer.html" data-type="entity-link" >RispostaStandard_1OfDatiServer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfDatiStrutturaAttributiLayer.html" data-type="entity-link" >RispostaStandard_1OfDatiStrutturaAttributiLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfDecimal.html" data-type="entity-link" >RispostaStandard_1OfDecimal</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoAlgoritmi.html" data-type="entity-link" >RispostaStandard_1OfElencoAlgoritmi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoAllegatiLayer_Out.html" data-type="entity-link" >RispostaStandard_1OfElencoAllegatiLayer_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoConfigurazioniProiezione.html" data-type="entity-link" >RispostaStandard_1OfElencoConfigurazioniProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoConfigurazioniSuLayer.html" data-type="entity-link" >RispostaStandard_1OfElencoConfigurazioniSuLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoLogEsecuzioniConfigurazioniProiezione.html" data-type="entity-link" >RispostaStandard_1OfElencoLogEsecuzioniConfigurazioniProiezione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoMaschereLayerRaster.html" data-type="entity-link" >RispostaStandard_1OfElencoMaschereLayerRaster</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoPermessiConfigurazione.html" data-type="entity-link" >RispostaStandard_1OfElencoPermessiConfigurazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoPermessiMaschera.html" data-type="entity-link" >RispostaStandard_1OfElencoPermessiMaschera</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoPermessiUtente.html" data-type="entity-link" >RispostaStandard_1OfElencoPermessiUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoTipologieLayer.html" data-type="entity-link" >RispostaStandard_1OfElencoTipologieLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfElencoUrlFirmati.html" data-type="entity-link" >RispostaStandard_1OfElencoUrlFirmati</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfEndpointMappeSatellitari.html" data-type="entity-link" >RispostaStandard_1OfEndpointMappeSatellitari</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfEntita_Info_Out.html" data-type="entity-link" >RispostaStandard_1OfEntita_Info_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfGetProprieta_Out.html" data-type="entity-link" >RispostaStandard_1OfGetProprieta_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfGisDataReadRval_New_1OfGeoJSONAgroGisProp.html" data-type="entity-link" >RispostaStandard_1OfGisDataReadRval_New_1OfGeoJSONAgroGisProp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfImpresa.html" data-type="entity-link" >RispostaStandard_1OfImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfInformazioniAssistenza.html" data-type="entity-link" >RispostaStandard_1OfInformazioniAssistenza</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfJObject.html" data-type="entity-link" >RispostaStandard_1OfJObject</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiAllegatiDaRicettaDestinazione.html" data-type="entity-link" >RispostaStandard_1OfLeggiAllegatiDaRicettaDestinazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiBufferZone_Out.html" data-type="entity-link" >RispostaStandard_1OfLeggiBufferZone_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiElencoElaborazioniMassive_Out.html" data-type="entity-link" >RispostaStandard_1OfLeggiElencoElaborazioniMassive_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiImpostazioniAvanzateLayer.html" data-type="entity-link" >RispostaStandard_1OfLeggiImpostazioniAvanzateLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiPermessiLayerGruppiUtente_Out.html" data-type="entity-link" >RispostaStandard_1OfLeggiPermessiLayerGruppiUtente_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiPermessiLayerUtenti_Out.html" data-type="entity-link" >RispostaStandard_1OfLeggiPermessiLayerUtenti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLeggiTipiOggettoPerElementoGrafico.html" data-type="entity-link" >RispostaStandard_1OfLeggiTipiOggettoPerElementoGrafico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLinkedMachine_1OfAppezzamento_PKOf.html" data-type="entity-link" >RispostaStandard_1OfLinkedMachine_1OfAppezzamento_PKOf</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfAttivitaPersonalizzata.html" data-type="entity-link" >RispostaStandard_1OfList_1OfAttivitaPersonalizzata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfAttivitaStatistiche.html" data-type="entity-link" >RispostaStandard_1OfList_1OfAttivitaStatistiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfAvversitaGruppo.html" data-type="entity-link" >RispostaStandard_1OfList_1OfAvversitaGruppo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfBaseCodeDescr.html" data-type="entity-link" >RispostaStandard_1OfList_1OfBaseCodeDescr</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfBookmark.html" data-type="entity-link" >RispostaStandard_1OfList_1OfBookmark</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfDatiMUZVisibili_Out.html" data-type="entity-link" >RispostaStandard_1OfList_1OfDatiMUZVisibili_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfDettaglioTrattamento.html" data-type="entity-link" >RispostaStandard_1OfList_1OfDettaglioTrattamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfDisciplinare.html" data-type="entity-link" >RispostaStandard_1OfList_1OfDisciplinare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfGiacenzaZoo.html" data-type="entity-link" >RispostaStandard_1OfList_1OfGiacenzaZoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfGruppoAreaOmogenea.html" data-type="entity-link" >RispostaStandard_1OfList_1OfGruppoAreaOmogenea</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfImpresa.html" data-type="entity-link" >RispostaStandard_1OfList_1OfImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfInt32.html" data-type="entity-link" >RispostaStandard_1OfList_1OfInt32</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfLavorazione.html" data-type="entity-link" >RispostaStandard_1OfList_1OfLavorazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfMessaggioEsecuzione.html" data-type="entity-link" >RispostaStandard_1OfList_1OfMessaggioEsecuzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfMovimentoDiMagazzino.html" data-type="entity-link" >RispostaStandard_1OfList_1OfMovimentoDiMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfMUZ.html" data-type="entity-link" >RispostaStandard_1OfList_1OfMUZ</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfObject.html" data-type="entity-link" >RispostaStandard_1OfList_1OfObject</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfObjInputHTML_Out.html" data-type="entity-link" >RispostaStandard_1OfList_1OfObjInputHTML_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfObjOptionHTML_Out.html" data-type="entity-link" >RispostaStandard_1OfList_1OfObjOptionHTML_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfPianoDiCampionamento.html" data-type="entity-link" >RispostaStandard_1OfList_1OfPianoDiCampionamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfProtocolloInCorso.html" data-type="entity-link" >RispostaStandard_1OfList_1OfProtocolloInCorso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfRasterInfoClick_Out.html" data-type="entity-link" >RispostaStandard_1OfList_1OfRasterInfoClick_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfSottogruppoStalla.html" data-type="entity-link" >RispostaStandard_1OfList_1OfSottogruppoStalla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfSpecie.html" data-type="entity-link" >RispostaStandard_1OfList_1OfSpecie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfStalla.html" data-type="entity-link" >RispostaStandard_1OfList_1OfStalla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfString.html" data-type="entity-link" >RispostaStandard_1OfList_1OfString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfTipoEntita_Out.html" data-type="entity-link" >RispostaStandard_1OfList_1OfTipoEntita_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfTipologiaLabel.html" data-type="entity-link" >RispostaStandard_1OfList_1OfTipologiaLabel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfTreeValutazionePianoContixConti.html" data-type="entity-link" >RispostaStandard_1OfList_1OfTreeValutazionePianoContixConti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfUnitaDiMisura_Alternativa.html" data-type="entity-link" >RispostaStandard_1OfList_1OfUnitaDiMisura_Alternativa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfUtilizzoTerreno.html" data-type="entity-link" >RispostaStandard_1OfList_1OfUtilizzoTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfValutazione_Conto.html" data-type="entity-link" >RispostaStandard_1OfList_1OfValutazione_Conto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfValutazione_Piano_Conti.html" data-type="entity-link" >RispostaStandard_1OfList_1OfValutazione_Piano_Conti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfValutazione_Testata.html" data-type="entity-link" >RispostaStandard_1OfList_1OfValutazione_Testata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfValutazioneTipoAnno.html" data-type="entity-link" >RispostaStandard_1OfList_1OfValutazioneTipoAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_Coltura.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_Coltura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_GHGColture.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_GHGColture</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_MovimentoMagazzino.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_MovimentoMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_Operazione.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_PrevisioniAI.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_PrevisioniAI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_ProduzioneColtura.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_ProduzioneColtura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidget_StimeProduzioneColture.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidget_StimeProduzioneColture</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidgetAcquisto.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidgetAcquisto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfList_1OfWidgetKPI.html" data-type="entity-link" >RispostaStandard_1OfList_1OfWidgetKPI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfLista_GisSat_SentinelOverlay_Out.html" data-type="entity-link" >RispostaStandard_1OfLista_GisSat_SentinelOverlay_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfListaGruppiUtente.html" data-type="entity-link" >RispostaStandard_1OfListaGruppiUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfListaMisuraPerIndiciMaturitaAnagrafica.html" data-type="entity-link" >RispostaStandard_1OfListaMisuraPerIndiciMaturitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfListaMisurePerAvversitaAnagrafica.html" data-type="entity-link" >RispostaStandard_1OfListaMisurePerAvversitaAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfListaMisurePerAvversitaAnagraficaExtended.html" data-type="entity-link" >RispostaStandard_1OfListaMisurePerAvversitaAnagraficaExtended</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfListaUtenti.html" data-type="entity-link" >RispostaStandard_1OfListaUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfMessaggioEsecuzione.html" data-type="entity-link" >RispostaStandard_1OfMessaggioEsecuzione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfObj_SalvaGrafica.html" data-type="entity-link" >RispostaStandard_1OfObj_SalvaGrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfObject.html" data-type="entity-link" >RispostaStandard_1OfObject</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfParcoMacchine.html" data-type="entity-link" >RispostaStandard_1OfParcoMacchine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfPua.html" data-type="entity-link" >RispostaStandard_1OfPua</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfSalvaEntitaConAttributi_Out.html" data-type="entity-link" >RispostaStandard_1OfSalvaEntitaConAttributi_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfSalvaNuovoAB_Out.html" data-type="entity-link" >RispostaStandard_1OfSalvaNuovoAB_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfSalvaNuovoMultiPoint_Out.html" data-type="entity-link" >RispostaStandard_1OfSalvaNuovoMultiPoint_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfScriviNuovoLayerPersonalizzato_Out.html" data-type="entity-link" >RispostaStandard_1OfScriviNuovoLayerPersonalizzato_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfServiziSottoscrivibili_Out.html" data-type="entity-link" >RispostaStandard_1OfServiziSottoscrivibili_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfSingoloObjImpianto_Out.html" data-type="entity-link" >RispostaStandard_1OfSingoloObjImpianto_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfString.html" data-type="entity-link" >RispostaStandard_1OfString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfUtente.html" data-type="entity-link" >RispostaStandard_1OfUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfUtilizzoTerreno.html" data-type="entity-link" >RispostaStandard_1OfUtilizzoTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfVerificaEsistenzaEntitaAnagrafiche.html" data-type="entity-link" >RispostaStandard_1OfVerificaEsistenzaEntitaAnagrafiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfVerificaEsistenzaEntitaPerImpianti_Out.html" data-type="entity-link" >RispostaStandard_1OfVerificaEsistenzaEntitaPerImpianti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfVerificaInterferenze_Out.html" data-type="entity-link" >RispostaStandard_1OfVerificaInterferenze_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfVerificaVicini_Out.html" data-type="entity-link" >RispostaStandard_1OfVerificaVicini_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfWidget_AgroMeteo.html" data-type="entity-link" >RispostaStandard_1OfWidget_AgroMeteo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfWidget_Complete_Configuration.html" data-type="entity-link" >RispostaStandard_1OfWidget_Complete_Configuration</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfWidget_MeteoImpresaLatLng.html" data-type="entity-link" >RispostaStandard_1OfWidget_MeteoImpresaLatLng</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RispostaStandard_1OfWidgetIndiciProduttivitaGlobal.html" data-type="entity-link" >RispostaStandard_1OfWidgetIndiciProduttivitaGlobal</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Risultato.html" data-type="entity-link" >Risultato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RowGridDosi.html" data-type="entity-link" >RowGridDosi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RowGridImpianti.html" data-type="entity-link" >RowGridImpianti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RowGridProdottiDaTrattare.html" data-type="entity-link" >RowGridProdottiDaTrattare</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Rubrica.html" data-type="entity-link" >Rubrica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Rubrica-1.html" data-type="entity-link" >Rubrica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RubricaVoci.html" data-type="entity-link" >RubricaVoci</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/RubricaVoci-1.html" data-type="entity-link" >RubricaVoci</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaColoriLayer2_In.html" data-type="entity-link" >SalvaColoriLayer2_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaEntitaConAttributi_In.html" data-type="entity-link" >SalvaEntitaConAttributi_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaEntitaConAttributi_Out.html" data-type="entity-link" >SalvaEntitaConAttributi_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaFlagVisibilitaLayer_In.html" data-type="entity-link" >SalvaFlagVisibilitaLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaGruppoNote_In.html" data-type="entity-link" >SalvaGruppoNote_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaGruppoNoteCompleto_In.html" data-type="entity-link" >SalvaGruppoNoteCompleto_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaImpostazioni_AziendeCentri.html" data-type="entity-link" >SalvaImpostazioni_AziendeCentri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNote_In.html" data-type="entity-link" >SalvaNote_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNuovoAB_In.html" data-type="entity-link" >SalvaNuovoAB_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNuovoAB_Out.html" data-type="entity-link" >SalvaNuovoAB_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza_In.html" data-type="entity-link" >SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNuovoMultiPoint_In.html" data-type="entity-link" >SalvaNuovoMultiPoint_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaNuovoMultiPoint_Out.html" data-type="entity-link" >SalvaNuovoMultiPoint_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaOperazioniPreferite.html" data-type="entity-link" >SalvaOperazioniPreferite</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaParametriConnessioni.html" data-type="entity-link" >SalvaParametriConnessioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaParametriConnessioni_In.html" data-type="entity-link" >SalvaParametriConnessioni_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaPermessiLayerGruppiUtente_In.html" data-type="entity-link" >SalvaPermessiLayerGruppiUtente_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaPermessiLayerUtenti_In.html" data-type="entity-link" >SalvaPermessiLayerUtenti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaProfilazione_In.html" data-type="entity-link" >SalvaProfilazione_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SalvaVisibilitaLayer_In.html" data-type="entity-link" >SalvaVisibilitaLayer_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SaveRequisitiStabilimento.html" data-type="entity-link" >SaveRequisitiStabilimento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Scripts.html" data-type="entity-link" >Scripts</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Scrivi_Macchina_Anagrafica.html" data-type="entity-link" >Scrivi_Macchina_Anagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviAnalisiTerreno.html" data-type="entity-link" >ScriviAnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviCampiAnagrafica.html" data-type="entity-link" >ScriviCampiAnagrafica</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviCampiAnagrafica2.html" data-type="entity-link" >ScriviCampiAnagrafica2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviCampiAnagraficaInLine.html" data-type="entity-link" >ScriviCampiAnagraficaInLine</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviCatasto.html" data-type="entity-link" >ScriviCatasto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviConfigurazioneGisUtente.html" data-type="entity-link" >ScriviConfigurazioneGisUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviGruppiUtentiPerGruppiMerce.html" data-type="entity-link" >ScriviGruppiUtentiPerGruppiMerce</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviGruppoRaccolta.html" data-type="entity-link" >ScriviGruppoRaccolta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviGruppoUtente.html" data-type="entity-link" >ScriviGruppoUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviGruppoUtentexTransizioniStato.html" data-type="entity-link" >ScriviGruppoUtentexTransizioniStato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviLetturaContatore.html" data-type="entity-link" >ScriviLetturaContatore</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviListaAnalisiTerreno.html" data-type="entity-link" >ScriviListaAnalisiTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviListaAttivita.html" data-type="entity-link" >ScriviListaAttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviMacchina.html" data-type="entity-link" >ScriviMacchina</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviModificaImpreseParametri.html" data-type="entity-link" >ScriviModificaImpreseParametri</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviNuovoLayerPersonalizzato_In.html" data-type="entity-link" >ScriviNuovoLayerPersonalizzato_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviNuovoLayerPersonalizzato_Out.html" data-type="entity-link" >ScriviNuovoLayerPersonalizzato_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ScriviUtenti.html" data-type="entity-link" >ScriviUtenti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SementieriParametrizzazione.html" data-type="entity-link" >SementieriParametrizzazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SementiMappaturaLibera_SpecieVegPermessa_In.html" data-type="entity-link" >SementiMappaturaLibera_SpecieVegPermessa_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SeminaTrapianto.html" data-type="entity-link" >SeminaTrapianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SeminaTrapianto-1.html" data-type="entity-link" >SeminaTrapianto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Series.html" data-type="entity-link" >Series</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Series-1.html" data-type="entity-link" >Series</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ServerData.html" data-type="entity-link" >ServerData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ServiziSottoscrivibili.html" data-type="entity-link" >ServiziSottoscrivibili</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ServiziSottoscrivibili_Out.html" data-type="entity-link" >ServiziSottoscrivibili_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SimpleAxis.html" data-type="entity-link" >SimpleAxis</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SimpleBand.html" data-type="entity-link" >SimpleBand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SingoloObjImpianto_Out.html" data-type="entity-link" >SingoloObjImpianto_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SistemaRiferimentoItem.html" data-type="entity-link" >SistemaRiferimentoItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SistemaRiferimentoModel.html" data-type="entity-link" >SistemaRiferimentoModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SistemiRiferimentoCartografia.html" data-type="entity-link" >SistemiRiferimentoCartografia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SistemiRiferimentoCartografia-1.html" data-type="entity-link" >SistemiRiferimentoCartografia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Soglia.html" data-type="entity-link" >Soglia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SostanzaAttiva.html" data-type="entity-link" >SostanzaAttiva</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SostanzaAttiva-1.html" data-type="entity-link" >SostanzaAttiva</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SottogruppoStalla.html" data-type="entity-link" >SottogruppoStalla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SottogruppoStallaLight.html" data-type="entity-link" >SottogruppoStallaLight</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SottomettiElaborazioneMassivaGISCheckList_In.html" data-type="entity-link" >SottomettiElaborazioneMassivaGISCheckList_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SottotipoRicovero.html" data-type="entity-link" >SottotipoRicovero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Specie.html" data-type="entity-link" >Specie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Specie-1.html" data-type="entity-link" >Specie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Specie-2.html" data-type="entity-link" >Specie</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SpecieItem.html" data-type="entity-link" >SpecieItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SpecieVegetali_In.html" data-type="entity-link" >SpecieVegetali_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SpecieVegetaliData_In.html" data-type="entity-link" >SpecieVegetaliData_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SpeciParams.html" data-type="entity-link" >SpeciParams</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SpostamentoGruppi.html" data-type="entity-link" >SpostamentoGruppi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SrvGisParametriCartograficiInizializzazione.html" data-type="entity-link" >SrvGisParametriCartograficiInizializzazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Stalla.html" data-type="entity-link" >Stalla</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/State.html" data-type="entity-link" >State</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/StatoAccrescimento.html" data-type="entity-link" >StatoAccrescimento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/StatoAccrescimento-1.html" data-type="entity-link" >StatoAccrescimento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Stazioni.html" data-type="entity-link" >Stazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/STBufferGeoJsonPolygonInData.html" data-type="entity-link" >STBufferGeoJsonPolygonInData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/StrutturaAttributiLayer.html" data-type="entity-link" >StrutturaAttributiLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/SubmissionState.html" data-type="entity-link" >SubmissionState</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Sys.html" data-type="entity-link" >Sys</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Tabs.html" data-type="entity-link" >Tabs</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TagliatoIntero.html" data-type="entity-link" >TagliatoIntero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TagliatoIntero-1.html" data-type="entity-link" >TagliatoIntero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TecnicaConduzioneSuFila.html" data-type="entity-link" >TecnicaConduzioneSuFila</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TecnicaConduzioneSuFila-1.html" data-type="entity-link" >TecnicaConduzioneSuFila</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TecnicaConduzioneTraFila.html" data-type="entity-link" >TecnicaConduzioneTraFila</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TecnicaConduzioneTraFila-1.html" data-type="entity-link" >TecnicaConduzioneTraFila</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TemaSelezionato.html" data-type="entity-link" >TemaSelezionato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TentativoCancellazioneGruppoMerce.html" data-type="entity-link" >TentativoCancellazioneGruppoMerce</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Test.html" data-type="entity-link" >Test</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TestataRicetta.html" data-type="entity-link" >TestataRicetta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TestataRicetta-1.html" data-type="entity-link" >TestataRicetta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Tile.html" data-type="entity-link" >Tile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoAllevamento.html" data-type="entity-link" >TipoAllevamento</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoEffluente.html" data-type="entity-link" >TipoEffluente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoEntita_Out.html" data-type="entity-link" >TipoEntita_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoFertilizzante.html" data-type="entity-link" >TipoFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoGruppo_Zoo.html" data-type="entity-link" >TipoGruppo_Zoo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaCapoAnimale.html" data-type="entity-link" >TipologiaCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaCapoAnimale-1.html" data-type="entity-link" >TipologiaCapoAnimale</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaDispositivi.html" data-type="entity-link" >TipologiaDispositivi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaFertilizzante.html" data-type="entity-link" >TipologiaFertilizzante</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaLabel.html" data-type="entity-link" >TipologiaLabel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaLabelDto.html" data-type="entity-link" >TipologiaLabelDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaLayer.html" data-type="entity-link" >TipologiaLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaSede.html" data-type="entity-link" >TipologiaSede</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaSede-1.html" data-type="entity-link" >TipologiaSede</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaTile.html" data-type="entity-link" >TipologiaTile</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipologiaUtente.html" data-type="entity-link" >TipologiaUtente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoRicovero.html" data-type="entity-link" >TipoRicovero</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoRisorsa.html" data-type="entity-link" >TipoRisorsa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoRisorsa-1.html" data-type="entity-link" >TipoRisorsa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoTarga.html" data-type="entity-link" >TipoTarga</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoTarga-1.html" data-type="entity-link" >TipoTarga</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TipoVisitaItem.html" data-type="entity-link" >TipoVisitaItem</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Title.html" data-type="entity-link" >Title</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TitoloDiPossesso.html" data-type="entity-link" >TitoloDiPossesso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TitoloDiPossesso-1.html" data-type="entity-link" >TitoloDiPossesso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TrackingGPSEntity.html" data-type="entity-link" >TrackingGPSEntity</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TransizioneDiStato.html" data-type="entity-link" >TransizioneDiStato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TreeValutazione_Item.html" data-type="entity-link" >TreeValutazione_Item</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/TreeValutazionePianoContixConti.html" data-type="entity-link" >TreeValutazionePianoContixConti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Tuple_2OfStringAndString.html" data-type="entity-link" >Tuple_2OfStringAndString</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UndoableCommand.html" data-type="entity-link" >UndoableCommand</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UnitaDiMisura.html" data-type="entity-link" >UnitaDiMisura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UnitaDiMisura-1.html" data-type="entity-link" >UnitaDiMisura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UnitaDiMisura_Alternativa.html" data-type="entity-link" >UnitaDiMisura_Alternativa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UnitaDiMisura_Alternativa-1.html" data-type="entity-link" >UnitaDiMisura_Alternativa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UpdatePrescrizione.html" data-type="entity-link" >UpdatePrescrizione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UrlFirmato.html" data-type="entity-link" >UrlFirmato</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/User.html" data-type="entity-link" >User</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UserModel.html" data-type="entity-link" >UserModel</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente.html" data-type="entity-link" >Utente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente-1.html" data-type="entity-link" >Utente</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente2.html" data-type="entity-link" >Utente2</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente_Impostazioni.html" data-type="entity-link" >Utente_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente_Impostazioni-1.html" data-type="entity-link" >Utente_Impostazioni</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente_Permesso.html" data-type="entity-link" >Utente_Permesso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Utente_Permesso-1.html" data-type="entity-link" >Utente_Permesso</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtenteDTO.html" data-type="entity-link" >UtenteDTO</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtenteFinestraTemp.html" data-type="entity-link" >UtenteFinestraTemp</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtentePermessi.html" data-type="entity-link" >UtentePermessi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtentePermessi-1.html" data-type="entity-link" >UtentePermessi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtentePermessoGerarchia.html" data-type="entity-link" >UtentePermessoGerarchia</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtenteTipologiaAccessoQDC.html" data-type="entity-link" >UtenteTipologiaAccessoQDC</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtilizzoTerreno.html" data-type="entity-link" >UtilizzoTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UtilizzoTerreno-1.html" data-type="entity-link" >UtilizzoTerreno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Value_Text.html" data-type="entity-link" >Value_Text</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Conto.html" data-type="entity-link" >Valutazione_Conto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Conto_PK.html" data-type="entity-link" >Valutazione_Conto_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Dettaglio.html" data-type="entity-link" >Valutazione_Dettaglio</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Dettaglio_Specifico.html" data-type="entity-link" >Valutazione_Dettaglio_Specifico</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Piano_Conti.html" data-type="entity-link" >Valutazione_Piano_Conti</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Piano_Conti_PK.html" data-type="entity-link" >Valutazione_Piano_Conti_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Testata.html" data-type="entity-link" >Valutazione_Testata</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_Testata_PK.html" data-type="entity-link" >Valutazione_Testata_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_TestataxAnno.html" data-type="entity-link" >Valutazione_TestataxAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Valutazione_TestataxAnno_PK.html" data-type="entity-link" >Valutazione_TestataxAnno_PK</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ValutazioneTipoAnno.html" data-type="entity-link" >ValutazioneTipoAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VariabiliInSessione_NG.html" data-type="entity-link" >VariabiliInSessione_NG</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Varieta.html" data-type="entity-link" >Varieta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Varieta-1.html" data-type="entity-link" >Varieta</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaAziendaAbilitataISCC_In.html" data-type="entity-link" >VerificaAziendaAbilitataISCC_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaEsistenzaEntitaAnagrafiche.html" data-type="entity-link" >VerificaEsistenzaEntitaAnagrafiche</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaEsistenzaEntitaAnagrafiche_In.html" data-type="entity-link" >VerificaEsistenzaEntitaAnagrafiche_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaEsistenzaEntitaPerImpianti_In.html" data-type="entity-link" >VerificaEsistenzaEntitaPerImpianti_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaEsistenzaEntitaPerImpianti_Out.html" data-type="entity-link" >VerificaEsistenzaEntitaPerImpianti_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaEsistenzaPalette_In.html" data-type="entity-link" >VerificaEsistenzaPalette_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaInterferenze_In.html" data-type="entity-link" >VerificaInterferenze_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaInterferenze_Out.html" data-type="entity-link" >VerificaInterferenze_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaVicini_In.html" data-type="entity-link" >VerificaVicini_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VerificaVicini_Out.html" data-type="entity-link" >VerificaVicini_Out</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Vincolo.html" data-type="entity-link" >Vincolo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Vincolo-1.html" data-type="entity-link" >Vincolo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/VisibilitaLayer.html" data-type="entity-link" >VisibilitaLayer</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Weather.html" data-type="entity-link" >Weather</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WeatherForecast.html" data-type="entity-link" >WeatherForecast</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget.html" data-type="entity-link" >Widget</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Acquisti_IN.html" data-type="entity-link" >Widget_Acquisti_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_AgroMeteo.html" data-type="entity-link" >Widget_AgroMeteo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Coltura.html" data-type="entity-link" >Widget_Coltura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Complete_Configuration.html" data-type="entity-link" >Widget_Complete_Configuration</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Configuration.html" data-type="entity-link" >Widget_Configuration</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Culture_IN.html" data-type="entity-link" >Widget_Culture_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_GHGColture.html" data-type="entity-link" >Widget_GHGColture</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_GHGColture_IN.html" data-type="entity-link" >Widget_GHGColture_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_MeteoImpresaLatLng.html" data-type="entity-link" >Widget_MeteoImpresaLatLng</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Modelli_Previsionali_Indicatori_IN.html" data-type="entity-link" >Widget_Modelli_Previsionali_Indicatori_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Modelli_Previsionali_Indicatori_ParamExtra.html" data-type="entity-link" >Widget_Modelli_Previsionali_Indicatori_ParamExtra</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_MovimentiMagazziono_In.html" data-type="entity-link" >Widget_MovimentiMagazziono_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_MovimentoMagazzino.html" data-type="entity-link" >Widget_MovimentoMagazzino</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Operazione.html" data-type="entity-link" >Widget_Operazione</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Operazioni_IN.html" data-type="entity-link" >Widget_Operazioni_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_PrevisioniAI.html" data-type="entity-link" >Widget_PrevisioniAI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_PrevisioniAI_IN.html" data-type="entity-link" >Widget_PrevisioniAI_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_ProduzioneColtura.html" data-type="entity-link" >Widget_ProduzioneColtura</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_Statistics_IN.html" data-type="entity-link" >Widget_Statistics_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_StimeProduzioneColture.html" data-type="entity-link" >Widget_StimeProduzioneColture</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widget_StimeProduzioneColture_IN.html" data-type="entity-link" >Widget_StimeProduzioneColture_IN</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetAcquisto.html" data-type="entity-link" >WidgetAcquisto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetData.html" data-type="entity-link" >WidgetData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetData-1.html" data-type="entity-link" >WidgetData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetData-2.html" data-type="entity-link" >WidgetData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetIndiciProduttivita.html" data-type="entity-link" >WidgetIndiciProduttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetIndiciProduttivitaGlobal.html" data-type="entity-link" >WidgetIndiciProduttivitaGlobal</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetIndiciProduttivitaXAnno.html" data-type="entity-link" >WidgetIndiciProduttivitaXAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetIndiciProduttivitaXImpresa.html" data-type="entity-link" >WidgetIndiciProduttivitaXImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetKPI.html" data-type="entity-link" >WidgetKPI</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetManagerRequest.html" data-type="entity-link" >WidgetManagerRequest</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetPositionData.html" data-type="entity-link" >WidgetPositionData</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivita.html" data-type="entity-link" >WidgetRequestIndiciProduttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivita-1.html" data-type="entity-link" >WidgetRequestIndiciProduttivita</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivitaXAnno.html" data-type="entity-link" >WidgetRequestIndiciProduttivitaXAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivitaXAnno-1.html" data-type="entity-link" >WidgetRequestIndiciProduttivitaXAnno</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivitaXImpresa.html" data-type="entity-link" >WidgetRequestIndiciProduttivitaXImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestIndiciProduttivitaXImpresa-1.html" data-type="entity-link" >WidgetRequestIndiciProduttivitaXImpresa</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestKpi.html" data-type="entity-link" >WidgetRequestKpi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WidgetRequestKpi-1.html" data-type="entity-link" >WidgetRequestKpi</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Widgets_In.html" data-type="entity-link" >Widgets_In</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Wind.html" data-type="entity-link" >Wind</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Window.html" data-type="entity-link" >Window</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/WindowSize.html" data-type="entity-link" >WindowSize</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ZooActivityForRedirect.html" data-type="entity-link" >ZooActivityForRedirect</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ZooBDNform.html" data-type="entity-link" >ZooBDNform</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ZooOperationsFiltersForm.html" data-type="entity-link" >ZooOperationsFiltersForm</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#miscellaneous-links"'
                            : 'data-bs-target="#xs-miscellaneous-links"' }>
                            <span class="icon ion-ios-cube"></span>
                            <span>Miscellaneous</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"' }>
                            <li class="link">
                                <a href="miscellaneous/enumerations.html" data-type="entity-link">Enums</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/functions.html" data-type="entity-link">Functions</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/typealiases.html" data-type="entity-link">Type aliases</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/variables.html" data-type="entity-link">Variables</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <a data-type="chapter-link" href="coverage.html"><span class="icon ion-ios-stats"></span>Documentation coverage</a>
                    </li>
                    <li class="divider"></li>
                    <li class="copyright">
                        Documentation generated using <a href="https://compodoc.app/" target="_blank" rel="noopener noreferrer">
                            <img data-src="images/compodoc-vectorise.png" class="img-responsive" data-type="compodoc-logo">
                        </a>
                    </li>
            </ul>
        </nav>
        `);
        this.innerHTML = tp.strings;
    }
});