import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ButtonsModule } from "@progress/kendo-angular-buttons";
import { InputsModule } from "@progress/kendo-angular-inputs";
import { LabelModule } from "@progress/kendo-angular-label";
import { LayoutModule } from "@progress/kendo-angular-layout";
import { UikitModule } from "app/Utility/uikit.module";
import { TranslocoRootModule } from "app/transloco/transloco-root.module";
import { IconsModule } from "@progress/kendo-angular-icons";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { BreadcrumbsService } from "app/Utility/Template/breadcrumbs/breadcrumbs.service";
import { TrattamentoZooComponent } from "./trattamento-zoo.component";
import { TrattamentoZooRoutingModule } from "./trattamento-zoo.routing.module";
import { GridProdottiSomministrazioneComponent } from "./griglie-trattamento/griglia-prodotti-somministrazione/grid-prodotti-somministrazione.component";
import { GridCapiAnimaliComponent } from "./griglie-trattamento/griglia-capi-animali/grid-capi-animali.component";
import { CapiAnimaliConfigService } from "./griglie-trattamento/griglia-capi-animali/service/capi-animali-config.service";
import { ProdottoSomministrazioneConfigService } from "./griglie-trattamento/griglia-prodotti-somministrazione/service/prodotto-somministrazione-config.service";
import { TRANSLOCO_SCOPE } from "@jsverse/transloco";
import { LOCALIZATION_LANGUAGES } from "app/Model/CostantiPersonalizzate";
import { GiasUikitModule } from 'gias-ui-kit';
import { GiasKendoGridModule } from "gias-kendo-grid";

export const loader = LOCALIZATION_LANGUAGES.reduce((acc, lang) => {
  acc[lang] = () => import(`../../i18n/${lang}.json`);
  return acc;
}, {});

@NgModule({
    imports: [
        TranslocoRootModule,
        // UikitModule,
        ReactiveFormsModule,
        CommonModule,
        HttpClientModule,
        LayoutModule,
        LabelModule,
        InputsModule,
        ButtonsModule,
        IconsModule,
        FontAwesomeModule,
        GiasUikitModule,
        TrattamentoZooRoutingModule,
        GiasKendoGridModule,
        FormsModule
    ],
    declarations: [
        GridProdottiSomministrazioneComponent,
        GridCapiAnimaliComponent,
        TrattamentoZooComponent
    ],
    exports: [],
    providers: [
        BreadcrumbsService,
        CapiAnimaliConfigService,
        ProdottoSomministrazioneConfigService,
        {
              provide: TRANSLOCO_SCOPE,
              useValue: {
                scope: 'zoo',
                loader,
                multi: true
              }
        },
    ]
})
export class TrattamentoZooModule {}