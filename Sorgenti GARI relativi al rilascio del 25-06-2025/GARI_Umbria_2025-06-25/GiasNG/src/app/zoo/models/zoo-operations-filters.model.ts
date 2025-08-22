import {FormControl} from "@angular/forms";
import {enum_TipoPrescrizione} from "./tipo-prescrizione.enum";

export class ZooOperationsFilters {
  public forceReload = false;

  constructor(
    public center: number,
    public stable: number,
    public from: Date,
    public to: Date,
    public operations: string[]
  ) {
  }
}

export class ZooPrescriptionsFilters extends ZooOperationsFilters {
  public prescriptionType: enum_TipoPrescrizione | null;
}

export interface ZooOperationsFiltersForm {
  center: FormControl<number>;
  stable: FormControl<number>;
  from: FormControl<Date>;
  to: FormControl<Date>;
  operations: FormControl<string[]>;
}
