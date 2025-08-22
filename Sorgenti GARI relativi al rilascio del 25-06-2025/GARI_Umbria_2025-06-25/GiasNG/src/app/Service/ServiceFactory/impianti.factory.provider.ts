import { IntlService } from "@progress/kendo-angular-intl";
import { AjaxAgronicaAPIService } from "../ajax-agronica.api.service";
import { AjaxAgronicaService } from "../ajax-agronica.service";
import { AppezzamentiService } from "../Anagrafica/appezzamenti.service";
import { AppezzamentiBudgetService } from "../Budget/appezzamenti.budget.service";
import { BudgetService } from "../Budget/budget.service";
import { ConversionService } from "../conversion.service";
import { MasterService } from "../master.service";
import { IMPIANTI_SERVICE_TOKEN } from "./impianti.factory.service";

export let ImpiantiServiceFactory = (
    masterService: MasterService,
    ajaxAgronicaService: AjaxAgronicaService,
    ajaxAgronicaApiService: AjaxAgronicaAPIService,
    conversionService: ConversionService,
    intlService: IntlService,
    budgetService: BudgetService) => {
    const currentBudget = budgetService.getBudget();
    //budgetService.currentBudget.subscribe((currentBudget) => {
        if (!currentBudget.activeBudget) {
            return new AppezzamentiService(masterService, ajaxAgronicaService, ajaxAgronicaApiService, conversionService, intlService);
        } else {
            return new AppezzamentiBudgetService(masterService, ajaxAgronicaService, ajaxAgronicaApiService, conversionService, intlService, budgetService);
        }
    //})

};

export let ImpiantiServiceProvider = {
  provide: IMPIANTI_SERVICE_TOKEN,
  useFactory: ImpiantiServiceFactory,
  deps: [
      MasterService,
      AjaxAgronicaService,
      AjaxAgronicaAPIService,
      ConversionService,
      IntlService,
      BudgetService
  ]
};
