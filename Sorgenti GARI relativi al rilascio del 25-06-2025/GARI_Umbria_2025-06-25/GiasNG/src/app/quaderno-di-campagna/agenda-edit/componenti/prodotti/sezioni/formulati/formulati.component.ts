import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormArray, FormGroup, FormGroupDirective } from "@angular/forms";
import { Lavorazione } from "app/Model/attivita/Lavorazione";
import { QdCService } from "app/quaderno-di-campagna/agenda-edit/service/qdc.service";
import { GiasDropDownTemplateSComponent } from 'gias-ui-kit';
import { GiasDropDownTemplateService } from 'gias-ui-kit';
import { UtilityFunctions } from "app/Utility/UtilityFunctions";
import { Subscription } from "rxjs";
import { QdCFormulatiService } from "../../../../service/prodotti/formulati.service";

@Component({
    standalone: false,
    selector: "app-formulati",
    templateUrl: "./formulati.component.html",
    styleUrls: ["./formulati.component.scss"],
    providers: [GiasDropDownTemplateService, QdCFormulatiService]
})
export class FormulatiComponent implements OnInit, OnDestroy {

    FormulatiForm: FormGroup;
    Operazione: Lavorazione;
    Subs: Subscription = new Subscription();

    constructor(public parent: FormGroupDirective,
        public qdcservice: QdCService,
        public qdcformulatiservice: QdCFormulatiService
    ) { }

    async ngOnInit() {

        this.FormulatiForm = <FormGroup>this.parent.form;

        this.Operazione = <Lavorazione>(
            this.FormulatiForm.get("Operazione").value
        );

        //Triggero il validator del formgroup
        this.FormulatiForm.markAllAsTouched();

        //Aggiungo un elemento all'Array delle epocheDPI per poter visualizzare la dropdown
        if (this.FormulatiForm.get("EpocaDPI").value) {

            this.qdcformulatiservice.Array_EpocheDPI.push(this.FormulatiForm.get("EpocaDPI").value);

        } else {

            this.FormulatiForm.patchValue({
                EpocaDPI: null,
            }, { emitEvent: false });

            await this.qdcformulatiservice.getArray_EpocheDPI(this.FormulatiForm);
        }

        this.Subs.add(
            this.qdcservice.TestataForm.get("Disciplinare").valueChanges.subscribe(async d => {

                await this.qdcformulatiservice.getArray_EpocheDPI(this.FormulatiForm);

            })
        );

        //Aggiorno il valore di EpocaDPI in tutti i DosiProdotti
        this.Subs.add(this.FormulatiForm.get("EpocaDPI").valueChanges.subscribe(value => {

            let DosiProdotti = this.qdcservice.DosiProdottiFormArray(this.FormulatiForm, null);

            if (DosiProdotti && DosiProdotti.controls.length > 0) {
                for (let i = 0; i < DosiProdotti.controls.length; i++) {
                    DosiProdotti.controls[i].patchValue({
                        EpocaDPI: value
                    });
                }
            }
        }));

        this.qdcformulatiservice.AbilitaDisabilitaEpocaDPI(this.FormulatiForm);

        await this.qdcformulatiservice.Carica_Controlli_Ribaltamento_in_Agenda_Di_Ricetta_da_APP_Formulati(this.FormulatiForm);
    }


    ngOnDestroy(): void {
        this.Subs.unsubscribe();
    }

    //Carico la ddl solo quando scatta l'evento di open
    async openControlFormulati(
        ddlEl: GiasDropDownTemplateSComponent,
        formName: string
    ) {
        let fn: any;

        switch (formName) {
            case "EpocaDPI":
                fn = await this.qdcformulatiservice.getArray_EpocheDPI(this.FormulatiForm);
                UtilityFunctions.loadDropDownItems(ddlEl, fn);
                break;
        }
    }



}
