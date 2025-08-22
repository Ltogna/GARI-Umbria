/* eslint-disable no-use-before-define */
import { AfterViewInit, Component, ContentChild, Inject, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { MasterService, RispostaStandard } from 'app/Service/master.service';
import { GRID_HTTP_TOKEN } from 'gias-kendo-grid';
import { GridPublicService } from 'gias-kendo-grid';
import { generateGridProviders } from 'gias-kendo-grid';
import { BussinessMenuAgendaService } from '../../shared_services/bussiness-logic.service';
import { ZooGridService } from './zoo-grid.service';


@Component({
    standalone: false,
    selector: 'grid-zoo',
    templateUrl: './zoo.component.html',
    styleUrls: ['./zoo.component.scss'],
    providers: [
        ...generateGridProviders(ZooGridService, GridZooComponent),
        BussinessMenuAgendaService
    ],
    encapsulation: ViewEncapsulation.None,
})
export class GridZooComponent implements AfterViewInit {

    @ContentChild('stampeRef') stampeRef: TemplateRef<any>;
    @ViewChild('warningDialogRef', { static: true }) public warningDialogRef: TemplateRef<any>;
    public warningDialogMsg: string;

    constructor(
        @Inject(GRID_HTTP_TOKEN) public gridConfig: ZooGridService,
        private master: MasterService,
        private bussiness: BussinessMenuAgendaService,
        private gpubService: GridPublicService) {
    }


    copiaOperazione(dataItem: any): void {
        this.master.set_isLoading({ message: '', isLoading: true });

        this.bussiness.copiaOperazione(dataItem, this.gpubService.value.data.rows).subscribe((r) => {
            if (r.RispostaOK) {
                let copiaOpRisp = r.RispostaStringa;
                this.bussiness.copiaCambiaSito(copiaOpRisp);
            } else
                this.master.changeErrorMsgType({ show:true, msg: r.Errore, errorNumber: 0 });
        });
    }

    ngAfterViewInit(): void {
        this.gridConfig.SetComponentRef(this);
    }
}
