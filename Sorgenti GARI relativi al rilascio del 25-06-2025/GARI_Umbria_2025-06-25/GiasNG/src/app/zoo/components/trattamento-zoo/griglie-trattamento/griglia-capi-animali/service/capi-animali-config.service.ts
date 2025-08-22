import { Injectable } from "@angular/core";
import { Validators } from "@angular/forms";
import { TranslocoService } from "@jsverse/transloco";
import { KendoGridColumn, ModelEntry, NumericSettings } from "gias-kendo-grid";
import { CELL_TYPES } from "gias-ui-kit";

export class CapoAnimale {
    chiave: number;
    Matricola: string;
    Raggruppamento_Des: string;
    STA_NUM: number;
    STA_DES: string;
    RAZ_DES: string;
    Stato_Des: string;
    Validita_Inizio: Date;
    Validita_Fine: Date;
    Incremento_Teorico_Calcolato: number;
    Quantita: number;

    constructor(chiave, matricola, raggruppamento_Des, sta_num, sta_des, raz_des, stato_des, validita_inizio, validita_fine, incremento_teorico_calcolato, quantita) {
        this.chiave = chiave;
        this.Matricola = matricola;
        this.Raggruppamento_Des = raggruppamento_Des;
        this.STA_NUM = sta_num;
        this.STA_DES = sta_des;
        this.RAZ_DES = raz_des;
        this.Stato_Des = stato_des;
        this.Validita_Inizio = validita_inizio;
        this.Validita_Fine = validita_fine;
        this.Incremento_Teorico_Calcolato = incremento_teorico_calcolato;
        this.Quantita = quantita;
    }
}


@Injectable()
export class CapiAnimaliConfigService { 

    constructor(
        public translocoService: TranslocoService
    ) {}

    kendoColumnsCapiAnimali: KendoGridColumn[] = [
        new KendoGridColumn(
          {field: 'chiave', title: this.translocoService.translate('Chiave')},{ resizable:true, filterable:false, editable: false, width: 135, hidden: true }
        ),
        new KendoGridColumn(
          {field: 'Matricola', title: this.translocoService.translate('Matricola')},{ resizable:true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'Raggruppamento_Des', title: this.translocoService.translate('Raggruppamento') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'STA_NUM', title: this.translocoService.translate('StallaNumero') }, { resizable: true, editable: false, width: 135, hidden: true }
        ),
        new KendoGridColumn(
          { field: 'STA_DES', title: this.translocoService.translate('StallaDes') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'RAZ_DES', title: this.translocoService.translate('Razza') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'Stato_Des', title: this.translocoService.translate('Stato') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'Validita_Inizio', title: this.translocoService.translate('Validita_Inizio') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'Validita_Fine', title: this.translocoService.translate('Validita_Fine') }, { resizable: true, editable: false, width: 135, hidden: true }
        ),
        new KendoGridColumn(
          { field: 'Incremento_Teorico_Calcolato', title: this.translocoService.translate('zoo.Peso_Stimato') }, { resizable: true, editable: false, width: 135 }
        ),
        new KendoGridColumn(
          { field: 'Quantita', title: this.translocoService.translate('Quantita') }, { resizable: true, editable: true, width: 135, validators: [Validators.required, Validators.min(1)], numeric: new NumericSettings({
                  defaultValue: 0,
                  format: 'n4',
                  min: 0
              })}
        ),
    ];
    
    kendoModelCapiAnimali= {
        chiave: new ModelEntry(CELL_TYPES.STRING),
        Matricola: new ModelEntry(CELL_TYPES.STRING, false),
        Raggruppamento_Des: new ModelEntry(CELL_TYPES.STRING, false),
        STA_NUM: new ModelEntry(CELL_TYPES.NUMBER, false),
        STA_DES: new ModelEntry(CELL_TYPES.STRING, false),
        RAZ_DES: new ModelEntry(CELL_TYPES.STRING, false),
        Stato_Des: new ModelEntry(CELL_TYPES.STRING, false),
        Validita_Inizio: new ModelEntry(CELL_TYPES.DATE, false),
        Validita_Fine: new ModelEntry(CELL_TYPES.DATE, false),
        Incremento_Teorico_Calcolato: new ModelEntry(CELL_TYPES.NUMBER, false),
        Quantita: new ModelEntry(CELL_TYPES.NUMBER, true)
    };

}