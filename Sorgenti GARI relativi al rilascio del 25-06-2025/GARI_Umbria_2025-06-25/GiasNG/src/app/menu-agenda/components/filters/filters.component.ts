/* eslint-disable */
import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {AGRODATAFINE, AGRODATAINIZIO, NessunaSpecieQdC} from 'app/Model/CostantiPersonalizzate';
import { GiasDropDownTemplateService, ObjParametriAgenda } from 'gias-ui-kit';
import { GiasMultiSelectTemplateService } from 'gias-ui-kit';
import { Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { FiltersConfig } from '../models';
import { DrawerComponent, DrawerPosition } from '@progress/kendo-angular-layout';
import { faMagnifyingGlass, faSliders, faXmark } from '@fortawesome/free-solid-svg-icons';
import { CookieService } from 'app/Service/cookie.service';
import {TranslocoService} from "@jsverse/transloco";
import { SpecieItem, CentroItem, OperazioneItem, ImpiantoItem } from 'gias-kendo-grid';
import { FiltersService } from './filters.service';
import { CreateFormGroup, InitializeFilters } from './utilities';

@Component({
    standalone: false,
    selector: 'agenda-filters',
    templateUrl: './filters.component.html',
    styleUrls: ['./filters.component.css'],
    providers: [GiasDropDownTemplateService, GiasMultiSelectTemplateService],
})
export class AgendaFiltersComponent implements OnInit, OnDestroy {
    Data_Inizio = AGRODATAINIZIO;
    Data_Fine = AGRODATAFINE;
    defaultSpecie: SpecieItem = { veg_des: this.transloco.translate('TutteLeSpecie'), veg_cod: '-1' };
    defaultCentri: CentroItem = { sa_nome: this.transloco.translate('TuttiICentriAziendali'), sa_cod: '0' };

    @ViewChild('drawer') drawer: DrawerComponent;
    drawerWidth: number;
    drawerPosition: DrawerPosition  = "end";

    objParametriAgenda: ObjParametriAgenda;
    FiltersFG: FormGroup = CreateFormGroup(this.fb, InitializeFilters());
    signal: Subject<void> = new Subject<void>();

    faFilters = faSliders;
    faClose = faXmark;
    faSearch = faMagnifyingGlass;

    public shouldApply: boolean = true;

    displayButton: boolean = true;
    @Input() fromVisita: boolean = false;

    constructor(
        private filtersService: FiltersService,
        private fb: FormBuilder,
        private cookieService: CookieService,
        private transloco: TranslocoService
    ) {
        this.filtersService.fg = this.FiltersFG;
        this.filtersService.fg.get('MostraDDT').patchValue(true);
    }

    // Liste ddl, usate in file .html
    get Centri(): CentroItem[] { return this.filtersService.Centri; }
    get Speci(): SpecieItem[] { return this.filtersService.Specie; }
    get Operazioni(): OperazioneItem[] { return this.filtersService.Operazioni; }
    get Impianti(): ImpiantoItem[] { return this.filtersService.Impianti; }

    ngOnInit(): void {
        this.filtersService.init();
        this.handleSpecieChange();
    }

    onOpenFilters() {
        this.displayButton = false;
        this.drawer.toggle();
    }

    applyFilters() {
        if (!this.shouldApply) return;
        if (!this.FiltersFG.get('Da').value)
            this.FiltersFG.get('Da').patchValue(AGRODATAINIZIO);
        if (!this.FiltersFG.get('A').value)
            this.FiltersFG.get('A').patchValue(AGRODATAFINE);
        const values = this.FiltersFG.value;

        this.saveCookies();
        this.filtersService.applyFilters(values);

        this.centerVisual();
        if (!!this.drawer.expanded) {
            this.drawer.toggle();
        }
    }

    onCancel() {
        this.displayButton = true;
        this.shouldApply = false;

        if (!!this.drawer.expanded) {
            this.drawer.toggle();
        }
    }

    ngOnDestroy(): void {
        this.signal.next();
        this.signal.complete();
    }

    public centerVisual(): void {
        document.documentElement.scrollTop = 0;
    }

    /**
     * Handles changes to the "Specie" form control within the Filters FormGroup.
     *
     * This method listens for value changes in the "Specie" control and performs the following actions:
     * - If the selected "Specie" has a `veg_cod` value of '0', it clears and disables the "Impianti" control.
     * - Fetches updated "Impianti" data from the `filtersService` and updates the service's "Impianti" property.
     * - Ensures that the selected "Impianti" values in the form are valid by checking if they exist in the updated "Impianti" list.
     *   If any selected "Impianti" are invalid, the control's value is reset to an empty array.
     *
     * @private
     */
    private handleSpecieChange() {
        this.FiltersFG.get('Specie').valueChanges.pipe(
            takeUntil(this.signal),
            tap((Specie: SpecieItem) => {
                if (+Specie.veg_cod === NessunaSpecieQdC) {
                    this.FiltersFG.get('Impianti').patchValue([]);
                    this.filtersService.emptySelectedImpianti();
                    this.FiltersFG.get('Impianti').disable();
                } else {
                    this.FiltersFG.get('Impianti').enable();
                }
            }),
            switchMap(() => this.filtersService.CaricaImpianti()),
            tap((data: ImpiantoItem[]) => this.filtersService.Impianti = data),
        ).subscribe(() => {
            let filterPresel: FiltersConfig = this.FiltersFG.getRawValue();
            let containsAllImpiantoItem = true;
            filterPresel.Impianti.forEach((imp) => {
                let index = this.Impianti.findIndex((val) => { return val.chiave == imp.chiave });
                if (index < 0) {
                    containsAllImpiantoItem = false;
                }
            });
            if (!containsAllImpiantoItem) {
                this.FiltersFG.controls["Impianti"].setValue([]);
            }
        });
    }

    private saveCookies() {

        if (this.fromVisita)    //se arriviamo dalle visite, evitiamo di salvare i cookie per non sovrascrivere
            return;             //quelli del menu agenda

        const values = this.FiltersFG.value;
        const getDateString = (date: Date) => {
            let dd = date.getDate();
            let mm = date.getMonth() + 1;
            let yyyy = date.getFullYear();
            return mm + '/' + dd + '/' + yyyy;
        }

        this.cookieService.setCookie({
            name: this.filtersService._validitaInizio,
            value: getDateString(values.Da),
            expireDays: 10 * 365
        });
        this.cookieService.setCookie({
            name: this.filtersService._validitaFine,
            value: getDateString(values.A),
            expireDays: 10 * 365
        });

        if (values.CentroAziendale.sa_cod === '0')
            this.cookieService.deleteCookie(this.filtersService._centro);
        else  this.cookieService.setCookie({
            name: this.filtersService._centro,
            value: JSON.stringify(values.CentroAziendale),
            expireDays: 10 * 365
        });

        if (values.Specie.veg_cod === '-1')
            this.cookieService.deleteCookie(this.filtersService._specie);
        else this.cookieService.setCookie({
            name: this.filtersService._specie,
            value: JSON.stringify(values.Specie),
            expireDays: 10 * 365
        });

        if (!values.Impianti || !values.Impianti.length) {
            this.cookieService.deleteCookie(this.filtersService._impianti);
            this.filtersService.emptySelectedImpianti()
        }
        else this.cookieService.setCookie({
            name: this.filtersService._impianti,
            value: JSON.stringify(values.Impianti),
            expireDays: 10 * 365
        });

        if (!values.TipoOperazione.length) {
            this.cookieService.deleteCookie(this.filtersService._operazioni);
            this.filtersService.emptySelectedOperazioni();
        }
        else this.cookieService.setCookie({
            name: this.filtersService._operazioni,
            value: JSON.stringify(values.TipoOperazione),
            expireDays: 10 * 365
        });
    }

}
