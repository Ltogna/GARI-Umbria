import { Injectable, Injector } from "@angular/core";
import { EditingMode, KendoGridColumn, KendoGridModel, KendoGridRow, KendoServerResult, LoaderType } from 'gias-kendo-grid';
import { ConfigTemplate } from 'gias-kendo-grid';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { map, Observable } from "rxjs";
import { QuadernoDiCampagnaClient, ReportImpiegoProdottiFitosanitari_IN } from 'app/Service/net-core6-api.service';
import { RispostaStandard } from "app/Service/master.service";
import { TranslocoService } from "@jsverse/transloco";
import { CommandsColumnSettings } from 'gias-kendo-grid';

export class GridReportFitoServerResult extends KendoServerResult {
    constructor(rows: KendoGridRow[], cols: KendoGridColumn[], model: KendoGridModel) {
      super(model, cols, rows);
    }
  }

@Injectable()
export class GridReportFitoHttpService extends AbstractGridConfigService<GridReportFitoServerResult>{
    editingMode: EditingMode = EditingMode.IN_LINE; //FIX, it should not be possible to edit the grid
    loader: LoaderType = LoaderType.SERVICE;
    gridId: string = "ReportFitoGridId";
    rowId: string = "ReportFitoRowId";

    private reportImpiegoProdottiFitosantariIn: ReportImpiegoProdottiFitosanitari_IN;

    setReportImpiegoProdottiFitosanitariIn(value: ReportImpiegoProdottiFitosanitari_IN){
        this.reportImpiegoProdottiFitosantariIn = value;
    }

    constructor(injector: Injector, 
        private quadernoDiCampagnaClient: QuadernoDiCampagnaClient,
        private translocoService: TranslocoService) {

        super(injector, ConfigTemplate.DefaultTemplate);
    
        this.views.enabled = false;
        this.groups.groupable.enabled = false;
        this.cmdColumn = new CommandsColumnSettings({
          editBtn: false,
          infoBtn: false,
          removeBtn: false
        });
    }

    read(options?: any): Observable<GridReportFitoServerResult> {

        this.loadingService.set_isLoading({ isLoading: true, component: this.gridPublicService.gridElRef });


        return this.quadernoDiCampagnaClient.quadernoDiCampagnaGetReportImpiegoProdottiFitosanitari(this.reportImpiegoProdottiFitosantariIn)
            .pipe(map((r: any) => 
            {
                let objResult = JSON.parse(r.RispostaStringa);

                let gridReportFitoServerResult = new GridReportFitoServerResult(
                    this.setRowGrid(objResult.result),
                    this.setColumnsGrid(objResult.kendoColumns),
                    this.setKendoModelGrid(objResult.kendoColumns)
                );
                
                this.loadingService.set_isLoading({ isLoading: false, component: this.gridPublicService.gridElRef });

                return gridReportFitoServerResult;
            }));
    }

    setRowGrid(responseRows){
        let rows: Array<KendoGridRow> = [];
        for (let itemOfRows of responseRows){
            rows.push(itemOfRows);
        }

        return rows;
    }

    setColumnsGrid(columnsName): Array<KendoGridColumn> {
        let columns: Array<KendoGridColumn> = [];
        for (let itemOfColumns of columnsName) {
            if (itemOfColumns.Display){
                let kendoGridColumn: KendoGridColumn;

                if (itemOfColumns.Field === 'Qta'){

                    let numberFormat = 'n6';
                    if (columnsName.find(x => x.Field === 'Prodotto')) {
                        numberFormat = 'n3';
                    }

                    kendoGridColumn = new KendoGridColumn({ field: itemOfColumns.Field, title: (itemOfColumns.TranslateTitle) ? this.translocoService.translate(itemOfColumns.Title) : itemOfColumns.Title }, 
                        { resizable: true, editable: false, hidden: itemOfColumns.Hidden, numeric: {defaultValue: 0, format: numberFormat} });
                    
                } else if (itemOfColumns.Field === 'TitoloInPercentuale'){
                    kendoGridColumn = new KendoGridColumn({ field: itemOfColumns.Field, title: (itemOfColumns.TranslateTitle) ? this.translocoService.translate(itemOfColumns.Title) : itemOfColumns.Title }, 
                        { resizable: true, editable: false, hidden: itemOfColumns.Hidden, numeric: {defaultValue: 0, format: 'n2'} });
                } else{
                    kendoGridColumn = new KendoGridColumn({ field: itemOfColumns.Field, title: (itemOfColumns.TranslateTitle) ? this.translocoService.translate(itemOfColumns.Title) : itemOfColumns.Title }, { resizable: true, editable: false, hidden: itemOfColumns.Hidden });
                }
                columns.push(kendoGridColumn);
            }
        }
        return columns;
    }

    setKendoModelGrid(arrayModel): KendoGridModel {

        let gridModel = new KendoGridModel();

        arrayModel.forEach((item) => {
            gridModel[item.Field] = { editable: false, type: item.DataType };
        });

        return gridModel;
    }

    perform(actionType: HttpAction, items: any, oldRow?: any): Observable<any> {
        throw new Error("Method not implemented.");
    }

}