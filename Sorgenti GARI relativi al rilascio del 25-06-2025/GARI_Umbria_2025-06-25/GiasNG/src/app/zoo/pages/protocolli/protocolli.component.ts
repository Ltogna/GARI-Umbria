import { AfterViewInit, Component, DestroyRef, inject, Input, ViewChild } from '@angular/core';
import { filter, forkJoin, from, map, Subject, switchMap, tap } from "rxjs";
import { ZooOperationsFilters, ZooPrescriptionsFilters } from "../../models/zoo-operations-filters.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { generateGridProviders, GiasKendoGridComponent, GiasKendoGridModule } from "gias-kendo-grid";
import { enum_TipoPrescrizione } from "../../models/tipo-prescrizione.enum";
import { ZooPrescriptionsGridConfigService } from 'app/zoo/pages/protocolli/prescription-grid.service';
import { ZooPrescriptionGridFlatItem } from 'app/zoo/models/zoo-prescription-grid-item.model';
import { enum_PagineGiasNG } from 'app/Model/TipiEnumerativi';
import { GestioneRichiesteService } from 'app/Service/gestione-richieste.service';
import { PrescrizioniClient } from 'app/Service/net-core6-api.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { IconsModule } from '@progress/kendo-angular-icons';
import { LabelModule } from '@progress/kendo-angular-label';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { KENDO_TOOLTIPS } from '@progress/kendo-angular-tooltip';
import { TranslocoRootModule } from 'app/transloco/transloco-root.module';
import { enum_TypeTab_Zootecnia } from 'app/zoo/models/tipi-enumerativi-zoo';
import { Enum_DBTypeOperation, GiasUikitModule, ObjParametriAgenda } from 'gias-ui-kit';

@Component({
  standalone: true,
  selector: 'zoo-protocolli',
  templateUrl: './protocolli.component.html',
  styleUrl: './protocolli.component.css',
  imports: [
    GiasUikitModule,
    GiasKendoGridModule,
    LayoutModule,
    IconsModule,
    LabelModule,
    InputsModule,
    ButtonsModule,
    KENDO_TOOLTIPS,
    TranslocoRootModule
  ],
  providers: [
    // eslint-disable-next-line no-use-before-define
    ...generateGridProviders(ZooPrescriptionsGridConfigService, ProtocolliComponent)
  ]
})
export class ProtocolliComponent implements AfterViewInit {
  @ViewChild('zooProtocolsGrid') grid: GiasKendoGridComponent;
  @Input() filters$: Subject<ZooOperationsFilters>;
  private destroyRef = inject(DestroyRef);
  private first = true;
  constructor(private gestioneRichieste: GestioneRichiesteService, 
            protected prescriptions: PrescrizioniClient,
            protected agenda: ObjParametriAgendaService,){

  }
  ngAfterViewInit() {
    this.filters$.pipe(
      takeUntilDestroyed(this.destroyRef),
      filter(x => x !== null),
      filter(() => !!this.grid),
      map(filters => {
        const ff = filters as ZooPrescriptionsFilters;
        ff.prescriptionType = enum_TipoPrescrizione.Protocollo_Terapeutico;
        return ff;
      }),
      filter(() => this.first),
      //tap(() => this.first = false),
      switchMap((filters) => { 
        return this.grid.config.read(filters) 
      }),
    ).subscribe(() => this.grid.publicService.refresh(true));
  }

  public startProtocollo(dataItem: any) {
    this.createOperationFromProtocol(dataItem)
  }

  private createOperationFromProtocol(protocol: ZooPrescriptionGridFlatItem) {
      forkJoin([
        from(this.gestioneRichieste.gestionePassaggioStessoSito(enum_PagineGiasNG.Pagina_Trattamento_Zoo)),
        this.prescriptions.prescrizioniGetAttivitaFromPrescrizione(protocol.IdRicetta)
          .pipe(map(r => r.RispostaOK ? r.RispostaStringa as any : null))
      ]).subscribe(([redirectUrl, attivita]) => {
        const objP = this.agenda.getObjParamValue() as ObjParametriAgenda;
        objP.TipoOperazioneDB = Enum_DBTypeOperation.Write;
        objP.GenericObj_string = JSON.stringify(attivita);
        let queryParams: { [key:string] : string | string[] } = {"t_Tab":enum_TypeTab_Zootecnia.Prescriptions.toString()};
        this.agenda.navigateTo(redirectUrl, queryParams, objP, false);
      })
    }

}
