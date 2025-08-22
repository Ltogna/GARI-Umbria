import { Inject, Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { ImpreseFactoryService, IMPRESE_SERVICE_TOKEN } from 'app/Service/ServiceFactory/imprese.factory.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { ObjParametriAgenda } from 'gias-ui-kit';
import { first, Observable, of, switchMap } from 'rxjs';


@Injectable()
export class PivaValidatorService implements AsyncValidator{
    constructor(@Inject(IMPRESE_SERVICE_TOKEN) private impreseService: ImpreseFactoryService,
                private objParametriAgendaService: ObjParametriAgendaService) { }

    validate(control: AbstractControl): Observable<ValidationErrors> {

        // if (!control.touched) {
        //     return of(null);
        // }
        let objParametriAgenda = new ObjParametriAgenda();
        if (this.objParametriAgendaService) {
            objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
        }
        if (control.value.length < 11 || control.value == undefined) {
            return of(null);
        }
        else {
            if (objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Update) {
                return of(null);
            }
            return (this.impreseService.controlloPresenzaPiva(control.value).pipe(
                switchMap((res) => {
                    let risp = res.toString(); //IMPORTANTE
                    if (risp != '' && risp != undefined) {
                        return of({ 'piva': false });
                    }
                    return of(null);
                })
            )).pipe(first());
        }
    }

    registerOnValidatorChange?(fn: () => void): void {
        throw new Error('Method not implemented.');
    }

}
