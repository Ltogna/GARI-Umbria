import { AGRODATAFINE, AGRODATAINIZIO } from "../CostantiPersonalizzate";
import {IntervalloTemporale as IIntervalloTemporale} from "../../Service/api.service";
import {IntervalloTemporale as IIntervalloTemporaleNetCore} from "../../Service/net-core6-api.service";
import {FormControl} from "@angular/forms";

export class IntervalloTemporale implements IIntervalloTemporale, IIntervalloTemporaleNetCore {
  inizio: Date;
  fine: Date;

  constructor(inizio?: Date, fine?: Date) {
    this.inizio = inizio ?? AGRODATAINIZIO;
    this.fine = fine ?? AGRODATAFINE;
  }

  overlaps(validity: IntervalloTemporale): boolean {
    return this.inizio <= validity.fine && this.fine >= validity.inizio;
  }

  contains(date: Date): boolean {
    return this.inizio <= date && this.fine >= date;
  }
}

