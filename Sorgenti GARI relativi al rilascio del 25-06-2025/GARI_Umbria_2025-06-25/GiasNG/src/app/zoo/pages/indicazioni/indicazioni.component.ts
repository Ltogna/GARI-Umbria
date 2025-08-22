import { AfterViewInit, Component, DestroyRef, inject, Input, ViewChild } from '@angular/core';
import { filter, map, Subject, switchMap, tap } from "rxjs";
import { ZooOperationsFilters, ZooPrescriptionsFilters } from "../../models/zoo-operations-filters.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { generateGridProviders, GiasKendoGridComponent, GiasKendoGridModule } from "gias-kendo-grid";
import { enum_TipoPrescrizione } from "../../models/tipo-prescrizione.enum";
import { ZooIndicationsGridConfigService } from './indications-grid.service';
import { GiasUikitModule } from 'gias-ui-kit';

@Component({
  standalone: true,
  selector: 'zoo-indicazioni',
  templateUrl: './indicazioni.component.html',
  styleUrl: './indicazioni.component.css',
  imports: [
    GiasUikitModule,
    GiasKendoGridModule
  ],
  providers: [
    // eslint-disable-next-line no-use-before-define
    ...generateGridProviders(ZooIndicationsGridConfigService, IndicazioniComponent)
  ]
})
export class IndicazioniComponent implements AfterViewInit {
  @ViewChild('zooIndicationsGrid') grid: GiasKendoGridComponent;
  @Input() filters$: Subject<ZooOperationsFilters>;
  private destroyRef = inject(DestroyRef);
  private first = true;

  ngAfterViewInit() {
    this.filters$.pipe(
      takeUntilDestroyed(this.destroyRef),
      filter(x => x !== null),
      filter(() => !!this.grid),
      map(filters => {
        const ff = filters as ZooPrescriptionsFilters;
        ff.prescriptionType = enum_TipoPrescrizione.Indicazione_Terapeutica;
        return ff;
      }),
      filter(() => this.first),
      //tap(() => this.first = false),
      switchMap((filters) => { 
        return this.grid.config.read(filters) 
      }),
    ).subscribe(() => this.grid.publicService.refresh(true));
  }

}
