import {Inject, Injectable} from '@angular/core';
import {ReplaySubject} from 'rxjs';
import {AgriculturalPlot} from './multi-edit-plots.model';
import {KendoGridRow} from 'gias-kendo-grid';
import {IMPIANTI_SERVICE_TOKEN, ImpiantiFactoryService} from '../../../../Service/ServiceFactory/impianti.factory.service';
import {GiasMessageService} from '../../../../Service/gias-message.service';
import {FunzioniComuniService} from '../../../../Service/FunzioniComuni.service';
import {IntervalloTemporale} from '../../../../Model/anagrafiche/IntervalloTemporale';
import {AnagraficaService} from '../../../anagrafica.service';
import {AGRODATAINIZIO} from '../../../../Model/CostantiPersonalizzate';

@Injectable()
export class AgriculturalPlotService {
  listViewRows$: ReplaySubject<AgriculturalPlot[]> = new ReplaySubject<AgriculturalPlot[]>();
  loading$: ReplaySubject<boolean> = new ReplaySubject<boolean>();

  set checkedRows(rows: KendoGridRow[]) {
    this._checkedRows = rows;
  }

  private _checkedRows: KendoGridRow[] = [];

  constructor(
    @Inject(IMPIANTI_SERVICE_TOKEN) private appezzamentiService: ImpiantiFactoryService,
    private giasMessageService: GiasMessageService,
    private fcService: FunzioniComuniService,
    private anagraficaService: AnagraficaService
  ) {  }

  setListViewRows(): void {
    this.loading$.next(true);
    this.listViewRows$.next(this.getUniquePlotsFromRows().map(this.mapToListViewItems.bind(this)));
    this.loading$.next(false);
  }

  private getUniquePlotsFromRows() {
    let a: Map<string, KendoGridRow> = new Map<string, KendoGridRow>();
    const date: Date = new Date(this.anagraficaService.filterData.getValue().data);

    this._checkedRows.map(r => {
      const key: string = JSON.stringify({
        codice: r['APPEZZA'],
        centroAziendalePK: {
          codice: r['SA_COD'],
          partitaIva: r['PIVA']
        }
      });

      const actualPlantValidity: IntervalloTemporale = new IntervalloTemporale(r['Validita_Inizio_Impianto'], r['Validita_Fine_Impianto']);
      if (date.toISOString() == AGRODATAINIZIO.toISOString() || actualPlantValidity.contains(date)) {
        a.set(key, r);
      }
    });

    return Array.from(a.values());
  }

  private mapToListViewItems(r: KendoGridRow): AgriculturalPlot {
    return new AgriculturalPlot(
      `${r['PIVA']}_${r['SA_COD']}_${r['APPEZZA']}`,
      this.extractDescription(r),
      r['PIVA'],
      r['SA_COD'],
      r['APPEZZA'],
      0,
      0,
      new IntervalloTemporale(),
      false,
      false,
      false
    );
  }

  private extractDescription(r: KendoGridRow): string {
    return `${r['app_nome']}: ${r['Utilizzo']}`;
  }
}
