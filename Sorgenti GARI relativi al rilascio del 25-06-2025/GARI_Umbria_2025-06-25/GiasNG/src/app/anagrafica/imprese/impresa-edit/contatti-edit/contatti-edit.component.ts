import {Component, OnDestroy, OnInit} from '@angular/core';
import {generateGridProviders} from 'gias-kendo-grid';
import {ContattiGridService} from './contatti-edit.service';
import {ContattiRootService} from './contattiRoot.service';
import {ObjParametriAgendaService} from 'app/Service/obj-parametri-agenda.service';
import {Enum_DBTypeOperation} from 'gias-ui-kit';
import {map, Subject, takeUntil} from 'rxjs';
import {PermessiUtenteService} from "../../../../Service/permessi-utente.service";
import {enum_Security_Attivita} from "../../../../Model/TipiEnumerativi";


@Component({
    standalone: false,
    selector: 'app-contatti-edit',
    templateUrl: './contatti-edit.component.html',
    styleUrls: ['./contatti-edit.component.css'],
    providers: [
        ...generateGridProviders(ContattiGridService, ContattiEditComponent)
    ]
})
export class ContattiEditComponent implements OnInit, OnDestroy {
    private signal:  Subject<void> = new Subject();

    isChecked: boolean = false;
    disabled: boolean = true;
    visible: boolean = false;

    contattoPubblicoVisible: boolean = false;
    contattoPubblicoIsChecked: boolean = false;
    contattoPubblicoDisabled: boolean = true;

    constructor(
        private contattiRootService: ContattiRootService,
        private objParametriAgendaService: ObjParametriAgendaService,
        private permessiUtenteService: PermessiUtenteService
    ){this.isChecked = false;}

    ngOnDestroy(): void {
        this.signal.next();
        this.signal.complete();
    }

    clicked(event) {
        this.isChecked = event;
        this.contattiRootService.setSalvaInPadre(this.isChecked);
    }

    contattoPubblicoClicked(event) {
        this.contattoPubblicoIsChecked = event;
        this.contattiRootService.setCreaContattiPubblici(this.contattoPubblicoIsChecked);
    }

    ngOnInit(): void {
        if (this.objParametriAgendaService.getObjParamValue().TipoOperazioneDB == Enum_DBTypeOperation.Write) {
            this.disabled = false;
            this.visible = true;
            this.contattoPubblicoVisible = true;
            if (this.permessiUtenteService.getPermesso(enum_Security_Attivita.Modifica_Contatti_Pubblici, 2) == true){
                this.contattoPubblicoDisabled = false;
            }
        }
        this.contattiRootService.singoloPadreSource.asObservable().pipe(takeUntil(this.signal), map(val => {
            this.disabled = !val;
            this.isChecked = false;
        })).subscribe();
    }
}
