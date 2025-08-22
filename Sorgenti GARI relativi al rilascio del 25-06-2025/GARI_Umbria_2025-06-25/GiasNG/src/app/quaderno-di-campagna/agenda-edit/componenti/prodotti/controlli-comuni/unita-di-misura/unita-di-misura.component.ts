/* eslint-disable */
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from "@angular/forms";
import { UnitaDiMisura } from "app/Model/metaschema/UnitaDiMisura";
import { QdCProdottiService } from "app/quaderno-di-campagna/agenda-edit/service/prodotti.service";
import { QdCUnitadiMisuraService } from "app/quaderno-di-campagna/agenda-edit/service/unita-di-misura.service";
import { GiasDropDownTemplateSComponent } from 'gias-ui-kit';
import { GiasDropDownTemplateService } from 'gias-ui-kit';
import { UtilityFunctions } from "app/Utility/UtilityFunctions";
import { Subscription, skip, startWith, pairwise, distinctUntilChanged } from 'rxjs';
import { debounceTime } from "rxjs/operators";
import { enum_LAVCOD } from '../../../../../../Model/TipiEnumerativi';


@Component({
    standalone: false,
    selector: "app-unita-di-misura",
    templateUrl: "./unita-di-misura.component.html",
    styleUrls: ["./unita-di-misura.component.css"],
    providers: [GiasDropDownTemplateService]
})

export class UnitaDiMisuraComponent implements OnInit {

    @Input() mostraDose: boolean = false;

    Subs: Subscription = new Subscription();
    ProdottiForm: FormGroup;
    Categoria_Magazzino: number;

    constructor(
        public parent: FormGroupDirective,
        public prodottiservice: QdCProdottiService,
        private ddlService: GiasDropDownTemplateService,
        private unitadimisuraservice: QdCUnitadiMisuraService
    ) { }

    ngOnInit(): void {
        this.ProdottiForm = <FormGroup>this.parent.form;

        this.Categoria_Magazzino = this.ProdottiForm.get("Categoria_Magazzino").value;

        this.Subs.add(
            this.ddlService.currentDropDownValueObject
                .pipe(skip(1))
                .subscribe(async (ddlElem) => {
                    switch (ddlElem.FormControlName) {
                        case "UdM":
                            this.unitadimisuraservice.changeUdM(ddlElem.Value, this.ProdottiForm, false);
                            break;
                    }
                })
        );

        this.Subs.add(this.ProdottiForm.get("UdM").valueChanges.pipe(
            startWith(this.ProdottiForm.get("UdM").value),
            pairwise()).subscribe(([prev, next]: [any, any]) => {
                this.unitadimisuraservice._unita = prev;
            })
        );
    }

    //Carico la ddl solo quando scatta l'evento di open
    async openControlUdM(ddlEl: GiasDropDownTemplateSComponent, formName: string): Promise<void> {
        let fn: any;

        switch (formName) {
            case "UdM":
                fn = await this.prodottiservice.getArray_UdM(this.ProdottiForm.getRawValue(), true);
                UtilityFunctions.loadDropDownItems(ddlEl, fn);
                if ((fn instanceof Array) && (<Array<UnitaDiMisura>>fn).length == 1) {
                    this.ProdottiForm.get("UdM").setValue((<Array<UnitaDiMisura>>fn)[0]);
                }
                break;
        }
    }
}
