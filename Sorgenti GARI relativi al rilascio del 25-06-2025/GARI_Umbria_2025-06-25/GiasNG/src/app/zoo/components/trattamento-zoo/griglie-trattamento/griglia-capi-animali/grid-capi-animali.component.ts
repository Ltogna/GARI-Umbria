import { Component, Inject, OnDestroy, ViewChild } from "@angular/core";
import { GRID_HTTP_TOKEN, generateGridProviders } from "gias-kendo-grid";
import { GridCapiAnimaliService } from "./service/grid-capi-animali.service";
import { NumericTextBoxComponent } from "@progress/kendo-angular-inputs";
import { enum_UdM_Dose } from "app/zoo/models/tipi-enumerativi-zoo";
import { enum_UnitaMisura } from "app/Model/TipiEnumerativi";

@Component({
    selector: 'app-capi-animali',
    templateUrl: './grid-capi-animali.component.html',
    standalone: false,
    styleUrls: ['./grid-capi-animali.component.scss'],
    providers: [...generateGridProviders(GridCapiAnimaliService, 
                                         GridCapiAnimaliComponent)]
})
export class GridCapiAnimaliComponent {

    @ViewChild('totaleFarmacoDaSomministrare') totaleFarmacoDaSomministrare!: NumericTextBoxComponent;
    
    public numericFormat: string = '#.0000';
    protected qtaApplied = true;

    constructor(
                @Inject(GRID_HTTP_TOKEN) protected gridService: GridCapiAnimaliService
    ) {

        // let udm: string = '';

        // switch(this.gridService.trattamentoFormService.dettagliProtocollo.udmDose) {
        //         case enum_UdM_Dose.mg_su_Kg:
        //         case enum_UnitaMisura.Milligrammi:
        //             udm = 'mg';
        //         break;
        //         case enum_UdM_Dose.ml_su_100Kg:
        //         case enum_UdM_Dose.ml_su_Capo:
        //         case enum_UnitaMisura.Millilitri:
        //             udm = 'ml';
        //         break;
        //         default:
        //             udm = '';
        // }

        // this.numericFormat += ' ' + udm;
        this.gridService.calculateNumericFormat(this.gridService.trattamentoFormService.dettagliProtocollo.udmDose);

    }

    public get gridSelectedRows(): Array<any> {
        return this.gridService.gridpublicService.getValue()?.data?.rows?.filter((row: any) => row.Selected) ?? [];
    }

    public get isTotQtaDisabled(): boolean {
        return this.gridService.trattamentoFormService.isInfoMode
         || this.gridSelectedRows.length == 0;
        //  || !this.gridService.isTotEditable;
    }

    onKeyDown(event: KeyboardEvent): void {
        if (event.key === 'Enter') {
            event.preventDefault();
            // this.gridService.modificaTotEDistribuisci();
        }
    }

    onBlur(): void {
        if (!this.qtaApplied) {
            this.gridService.modificaTotEDistribuisci();
            this.qtaApplied = true;
        }
    }

    onClickModificaTotEDistribuisci() {
        this.gridService.isTotEditable = true;
        setTimeout(() => this.totaleFarmacoDaSomministrare.focus(), 0);
    }

    onCellClick(event: any) {
        if (!event.dataItem.Selected)
            event.sender.closeRow(event.rowIndex);
    }

    onCellClose(event: any) {
    }

}