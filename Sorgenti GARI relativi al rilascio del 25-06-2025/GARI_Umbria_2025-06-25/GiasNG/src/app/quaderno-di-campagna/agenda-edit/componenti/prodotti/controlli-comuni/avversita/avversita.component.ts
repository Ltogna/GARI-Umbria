/* eslint-disable */
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';
import { TranslocoService } from '@jsverse/transloco';
import { Lavorazione } from 'app/Model/attivita/Lavorazione';
import { enum_LAVCOD } from 'app/Model/TipiEnumerativi';
import { QdCProdottiService } from 'app/quaderno-di-campagna/agenda-edit/service/prodotti.service';
import { QdCService } from 'app/quaderno-di-campagna/agenda-edit/service/qdc.service';
import { GiasDropDownTemplateSComponent } from 'gias-ui-kit';
import { GiasDropDownTemplateService } from 'gias-ui-kit';
import { UtilityFunctions } from 'app/Utility/UtilityFunctions';
import { skip, Subscription } from 'rxjs';
import { QdCDettagliFormulatiService } from '../../../../service/prodotti/dettagli-formulati.service';
import { DropdownListAvversita, enum_Problema_DettaglioProdotto } from '../../../../quaderno-di-campagna-form/quaderno-di-campagna-form.model';
import { GiasMultiColumnComboboxTemplateComponent } from 'gias-ui-kit';
import { LAVCOD_DISTRIBUZIONE_INSETTI } from '../../../../../../Model/CostantiPersonalizzate';

@Component({
    standalone: false,
    selector: 'app-avversita',
    templateUrl: './avversita.component.html',
    styleUrls: ['./avversita.component.css'],
    providers: [GiasDropDownTemplateService]
})
export class AvversitaComponent implements OnInit, OnDestroy, AfterViewInit {

    @Input() hideRileva = false;
    @Input() showComandi = false;

    @ViewChild('AvversitaMultiColumnCombobox') AvversitaMultiColumnCombobox: GiasMultiColumnComboboxTemplateComponent;

    FormulatiForm: FormGroup;
    Operazione: Lavorazione;
    Subs: Subscription = new Subscription();

    constructor(public prodottiservice: QdCProdottiService,
        private ddlService: GiasDropDownTemplateService,
        public qdcservice: QdCService,
        private translocoService: TranslocoService,
        public parent: FormGroupDirective,
        public qdcdettagliformulatiservice: QdCDettagliFormulatiService
    ) { }

    async ngOnInit() {

        this.FormulatiForm = <FormGroup>this.parent.form;
        this.Operazione = <Lavorazione>(this.FormulatiForm.get('Operazione').value);

        //Carico subito le Avversità se sono nel caso avversita->prodotto
        if (this.qdcservice.Mostra_Avversita_Prima_Dei_Prodotti) {
            let avversita: DropdownListAvversita = this.FormulatiForm.get('Avversita').value;

            //TODO Da verificare in modifica di un coadiuvante con filtro avversita - > prodotto
            if ((!avversita || avversita.codice <= 0) && avversita?.codice != -1) {
                this.FormulatiForm.patchValue({
                    Avversita: null,
                }, { emitEvent: false });

                await this.qdcdettagliformulatiservice.getArray_Avversita();
            }
        }

        this.Subs.add(
            this.ddlService.currentDropDownValueObject
                .pipe(skip(1))
                .subscribe(async (ddlElem) => {
                    switch (ddlElem.FormControlName) {
                        case 'Avversita':
                            switch (+this.Operazione.primaryKey.codice) {
                                case LAVCOD_DISTRIBUZIONE_INSETTI:
                                    await this.qdcdettagliformulatiservice.changeAvversitaInsetti(ddlElem.Value);
                                    break;
                                default:
                                    await this.qdcdettagliformulatiservice.changeAvversita(ddlElem.Value);
                                    break;
                            }
                            break;
                    }
                })
        );

    }

    ngAfterViewInit() {
        if (this.FormulatiForm.get('Problema_DettaglioProdotto_Da_Risolvere').getRawValue() === enum_Problema_DettaglioProdotto.Avversita_Non_Corretta ||
            this.FormulatiForm.get('Problema_DettaglioProdotto_Da_Risolvere').getRawValue() === enum_Problema_DettaglioProdotto.Avversita_Ambigua) {

            this.qdcdettagliformulatiservice.getArray_Avversita(false).then();
        }
    }

    ngOnDestroy(): void {
        this.Subs.unsubscribe();
    }

    getDescrizioneAvversita() {
        let descrizione = this.translocoService.translate('qdc.lbl_avversitaResource1.Text');

        if (+this.Operazione.primaryKey.codice === enum_LAVCOD.DISERBO) {
            descrizione = this.translocoService.translate('Infestanti') + '/' + this.translocoService.translate('GruppiInfestanti');
        }

        return descrizione;
    }

    //Carico la ddl solo quando scatta l'evento di open
    async openControlAvversita(ddlEl: GiasDropDownTemplateSComponent) {
        let fn: any = await this.qdcdettagliformulatiservice.getArray_Avversita();
        UtilityFunctions.loadDropDownItems(ddlEl, fn);
    }

    mostraAvversita() {
        let mostra: boolean = false;
        let lav_cod: number = +this.Operazione.primaryKey.codice;

        if (this.qdcdettagliformulatiservice.MostraNascondiDDLSezioneFormulati('Avversita') &&
            this.qdcdettagliformulatiservice.Array_Avversita &&
            this.qdcdettagliformulatiservice.Array_Avversita.length > 0) {
            mostra = true;
        }

        return mostra;
    }

}
