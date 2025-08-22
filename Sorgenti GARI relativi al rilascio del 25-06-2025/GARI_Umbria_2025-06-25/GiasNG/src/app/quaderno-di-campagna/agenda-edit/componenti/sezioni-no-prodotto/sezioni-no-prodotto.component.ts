import {Component, ViewChild} from '@angular/core';
import {QdCService} from '../../service/qdc.service';
import {FormArray, FormGroup} from '@angular/forms';
import {GiasPanelBar} from 'gias-ui-kit';
import {Sezione} from '../../quaderno-di-campagna-form/quaderno-di-campagna-form.model';
import {enum_LAVCOD} from '../../../../Model/TipiEnumerativi';
import {Lavorazione} from "../../../../Model/attivita/Lavorazione";
import {SpecieVegetale} from "../../../../Model/MetaschemaModel";
import {Specie} from "../../../../Model/metaschema/utilizzi/Specie";

@Component({
  standalone: false,
  selector: 'app-sezioni-no-prodotto',
  templateUrl: './sezioni-no-prodotto.component.html',
  styleUrls: ['./sezioni-no-prodotto.component.css']
})
export class SezioniNoProdottoComponent {

    @ViewChild("Sezioni_PanelBar")Sezioni_PanelBar: GiasPanelBar;

  constructor(public qdcservice: QdCService) { }

  public get trattamentoFormGroup(): FormGroup {
      return this.qdcservice.QdCForm.get('Trattamento') as FormGroup;
  }

  private get LAVCOD_RILIEVI(): Array<enum_LAVCOD | number> {
      return [
          enum_LAVCOD.DANNI_RACCOLTA,
          enum_LAVCOD.FASI_FENOLOGICHE,
          enum_LAVCOD.RILIEVO_AVVERSITA_CAMPO,
          enum_LAVCOD.RILIEVO_INDICI_RESE_RACCOLTA,
          enum_LAVCOD.RILIEVO_INDICI_MATURITA,
          enum_LAVCOD.RILIEVO_ERBE_INFESTANTI
      ];
  }

  mostraSezioni(): boolean {
      let sezioni = this.trattamentoFormGroup
                        .get('Sezioni_Senza_Prodotto') as FormArray;
      return sezioni.length > 0;
  }

  Disabilita_Sezione(): boolean {
      let hasImpiantiSel = this.qdcservice.ImpiantiSelezionatiFormArray?.length > 0 || this.qdcservice.RilievoSenzaImpianti();

      return !hasImpiantiSel && !this.Sezioni_PanelBar?.IsExpanded.getValue();
  }

    public Espandi_Sezione(): boolean{
        let espandi = false;

        if (!this.Disabilita_Sezione()){

            espandi = true;
        }


        return espandi;
    }

  Componi_Descrizione_Sezione(sezione: Sezione) {
      return sezione.Operazione.descrizione;
  }

  isRilievo(sezione: FormGroup): boolean {
      let lavCod = +sezione.value.Operazione.primaryKey.codice;
      return this.LAVCOD_RILIEVI.includes(lavCod);
  }

  isAbbattimento(sezione: FormGroup): boolean {
      let lavCod = +sezione.value.Operazione.primaryKey.codice;
      return lavCod === enum_LAVCOD.ABBATTIMENTOIMPIANTI;
  }

}
