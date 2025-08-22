import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import { MenuClient, Utente, WidgetsClient, Widget_Modelli_Previsionali_Indicatori_IN } from 'app/Service/api.service';
import { isSuperUser } from 'app/Service/utils';
import { Indicatore, IndicatoriData } from './indicatori-widget.models';
import {takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import { IndicatoriWidgetService } from './indicatori-widget.service';

const DSS_STORAGE_KEY = "DSS_Indicatori_Periodo";

@Component({
  standalone: false,
  selector: 'app-indicatori-widget',
  templateUrl: './indicatori-widget.component.html',
  styleUrls: ['./indicatori-widget.component.css']
})
export class IndicatoriWidgetComponent implements OnInit, OnDestroy {
  @Input() params: string | null = null;
  @Input() piva: string | null = null;

  data: Indicatore[] = [];
  loading = true;
  error = false;
  fullWidgetOpened = false;
  series: { value: string, color: string }[] = [];
  isDatePickerDialogOpen = false;
  user: Utente | null = null;

  startDate: Date;
  endDate: Date;
  dialogStartDate: Date;
  dialogEndDate: Date;
  private signal$: Subject<void> = new Subject();
  private destroyed = false;
  constructor(
    private widgetsClient: WidgetsClient,
    private menuClient: MenuClient,
    private indicatoriWidgetService: IndicatoriWidgetService
  ) {
    this.loadDates();
    this.initDates(false);

    this.menuClient
      .menuInformazioniUtente()
      .subscribe((data) => this.user = data.RispostaStringa);
  }

  ngOnDestroy(): void {
    this.signal$.next();
    this.signal$.complete();
    this.destroyed = true;
  }

  ngOnInit(): void {
    this.getData();
  }

  resetDates(): void {
    this.initDates(true);
    this.getData();
  }

  confirmDialog(): void {
    this.startDate = this.dialogStartDate;
    this.endDate = this.dialogEndDate;

    this.saveDates();
    this.getData();

    this.isDatePickerDialogOpen = false;
  }

  rejectDialog(): void {
    this.dialogStartDate = this.startDate;
    this.dialogEndDate = this.endDate;
    this.isDatePickerDialogOpen = false;
  }

  areDateControlsVisible(): boolean {
    if (this.user == null) {
      return false;
    }

    return false; //isSuperUser(this.user);
  }

  onClickChart() {
    this.indicatoriWidgetService.ApriPlugInIndicatoriDSS();
  }

  private initDates(reset = false): void {
    if (reset || this.startDate == null) {
      this.startDate = new Date(new Date().getFullYear(), 0, 1); // First day of the year
    }

    if (reset || this.endDate == null) {
      this.endDate = new Date();
    }

    this.dialogStartDate = this.startDate;
    this.dialogEndDate = this.endDate;

    this.saveDates();
  }

  private saveDates(): void {
    const payload = JSON.stringify({ startDate: this.startDate, endDate: this.endDate });
    localStorage.setItem(DSS_STORAGE_KEY, payload)
  }

  private loadDates(): void {
    const entry = localStorage.getItem(DSS_STORAGE_KEY);
    if (entry == null) {
      return;
    }

    const payload = JSON.parse(entry);
    if (payload.startDate != null) {
      this.startDate = new Date(payload.startDate);
    }


    if (payload.endDate != null) {
      this.endDate = new Date(payload.endDate);
    }
  }

  private getData(): void {
    this.loading = true;
    const payload = {
      Piva: this.piva,
      ParamExtra: {
        DataInizio: this.startDate,
        DataFine: this.endDate
      }
    } as Widget_Modelli_Previsionali_Indicatori_IN;
    this.widgetsClient
      .widgetsModelliPrevisionaliElaboraIndicatori(payload)
      .pipe(takeUntil(this.signal$))
      .subscribe((data) => {
        if (!this.destroyed){
          this.processData(data.RispostaStringa)
        }
      });
  }

  private processData(data: IndicatoriData): void {
    if (data == null || data.Stato == 0) {
      setTimeout(() => this.getData(), 500);
      return;
    }

    if (data.Stato == -1) {
      this.error = true;
      this.loading = false;
      return;
    }

    this.data = data.Indicatori;
    this.loading = false;

    let colors = {};
    let col = "";
    for (const indic of this.data) {

      switch (indic.Risultato.Status) {
        case "2":
          col = "#ccc";
          break;
        case "-1":
          col = "#ccc";
          break;
        case "3":
          col = "#ccc";
          break;
        default:
          for (const band of indic.Risultato.Bands) {
            if (indic.Risultato.Value <= band.Value) {
              col = band.Color;
              break;
            }
          };
      }

      if (col !== "") {
        col = "C_" + col;
        if (!colors.hasOwnProperty(col)) {
          colors[col] = 0;
        }
        colors[col] += 1;
        col = "";
      }
    }

    const series = []
    for (const k of Object.keys(colors)) {
      series.push({
        value: colors[k],
        color: k.substring(2)
      });
    };

    this.series = series;

    this.indicatoriWidgetService.paramIndicatori = data.Indicatori;

    this.indicatoriWidgetService.paramIndicatori.sort(function (elem0, elem1) {
        let v0 = [elem0.Stazione.toUpperCase(), elem0.Specie.toUpperCase()];
        let v1 = [elem1.Stazione.toUpperCase(), elem1.Specie.toUpperCase()];
        let cmp = 0;
        for (let idx = 0; (cmp === 0 && idx < v0.length); idx++) {
            if (v0[idx] < v1[idx]) {
                cmp = -1;
            } else if (v0[idx] > v1[idx]) {
                cmp = 1;
            }
        }
        return cmp;
    });

    this.indicatoriWidgetService.paramIndicatori.forEach((indic) => {
        switch (indic.Risultato.Status.toString()) {
            case "-1": // Status_OutOfRange
                indic.Risultato.Status = "outofrange";
                break;
            case "1": // Status_Warning
                indic.Risultato.Status = "warning";
                break;
            case "2": // Status_Error
                indic.Risultato.Status = "error";
                break;
            case "3": // Status_ProgressBefore
                indic.Risultato.Status = "progress";
                break;
            default: //case 0: // Status_Valid
                indic.Risultato.Status = "valid";
        }

        if (indic.Risultato.MeteoStatus == undefined) {
            indic.Risultato.MeteoStatus = "0";
            indic.Risultato.MeteoMsg = "";
        }

        switch (indic.Risultato.MeteoStatus.toString()) {
            case "1":
                indic.Risultato.MeteoStatus = "info";
                break;
            case "2":
                indic.Risultato.MeteoStatus = "warning";
                break;
            default:
                indic.Risultato.MeteoStatus = "none";
                break;
        }
    });

  }
}
