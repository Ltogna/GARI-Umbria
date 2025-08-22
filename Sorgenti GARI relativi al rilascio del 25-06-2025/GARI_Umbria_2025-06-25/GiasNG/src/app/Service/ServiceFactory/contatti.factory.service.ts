import { Injectable, InjectionToken } from "@angular/core";
import { IntlService } from "@progress/kendo-angular-intl";
import { InvestimentoCatastaleFiltriService } from "app/anagrafica/catasto/investimento-catastale/investimento-catastale-filtri.service";
import { Appezzamento } from "app/Model/anagrafiche/Appezzamento";
import { CentroAziendale } from "app/Model/anagrafiche/CentroAziendale";
import { Contatto } from "app/Model/anagrafiche/Contatto";
import { Fabbricato } from "app/Model/anagrafiche/Fabbricato";
import { Impresa } from "app/Model/anagrafiche/Impresa";
import { ParticelleCatastali } from "app/Model/anagrafiche/ParticelleCatastali";
import { CoreWS_Generic } from "app/Model/CoreWS/CoreWS_Generic";
import { AGRODATAINIZIO } from "app/Model/CostantiPersonalizzate";
import { FiltroImpresa } from "app/Model/filtri/FiltroImpresa";
import { AjaxAgronicaService } from "app/Service/ajax-agronica.service";
import { MasterService } from "app/Service/master.service";
import { BehaviorSubject, map, Observable, of } from "rxjs";
import { ObjParametriAgenda } from 'gias-ui-kit';
import { AjaxAgronicaAPIService } from "../ajax-agronica.api.service";

export class OrganismoImpresa{
    impresa: Impresa;
    organismoReferente: Contatto[];
}

export class RiferimentoTrasferimentoDatiImpresa{
    impresa: Impresa;
    riferimentoTrasferimentoDati: Contatto[];
}

export class MagazzinoConferimentoImpresa{
    impresa: Impresa;
    magazzinoConferimento: Fabbricato[];
}

export const CONTATTI_SERVICE_TOKEN = new InjectionToken<ContattiFactoryService>('app.contatti.service');

@Injectable()
export abstract class ContattiFactoryService {

    protected organismoReferentexImpresa: OrganismoImpresa[] = new Array<OrganismoImpresa>();
    protected riferimentoTrasferimentoDatiImpresa: RiferimentoTrasferimentoDatiImpresa[] = new Array<RiferimentoTrasferimentoDatiImpresa>();
    protected magazzinoConferimentoImpresa: MagazzinoConferimentoImpresa[] = new Array<MagazzinoConferimentoImpresa>();

    constructor(protected ajaxAgronicaService: AjaxAgronicaService,
                protected ajaxAgronicaAPIService: AjaxAgronicaAPIService,
                protected masterService: MasterService) {
    }

    abstract leggi(oibjAgenda: ObjParametriAgenda): Observable<any>;

    abstract LeggiOrganismiReferenti(impresa: Impresa): Promise<Contatto[]>;

    abstract LeggiRiferimento_Trasferimento_Dati(impresa: Impresa): Promise<Contatto[]>;

    abstract LeggiMagazzinoConferimento(impresa: Impresa): Promise<Fabbricato[]>;

}
