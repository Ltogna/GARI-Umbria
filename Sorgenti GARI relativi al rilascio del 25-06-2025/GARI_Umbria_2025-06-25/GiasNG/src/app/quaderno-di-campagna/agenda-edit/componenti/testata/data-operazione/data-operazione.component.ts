import { Component } from '@angular/core';
import {QdCService} from "../../../service/qdc.service";
import {QdCTestataService} from "../../../service/testata/testata.service";
import { Tipo_Attivita } from 'gias-ui-kit';

@Component({
  standalone: false,
  selector: 'app-data-operazione',
  templateUrl: './data-operazione.component.html',
  styleUrls: ['./data-operazione.component.css']
})
export class DataOperazioneComponent {

  constructor(public qdcservice: QdCService,
              public testataservice: QdCTestataService) { }

  /*
  * Mostra l'Ora solo se sono in una operazione QdC (esclude le ricette/brogliaccio e le Visite)
  * */
  public mostraOraOperazione(): boolean{

    let mostra: boolean = false;

    if(this.qdcservice.TestataForm &&
      this.qdcservice.TestataForm.get("Tipo").value === Tipo_Attivita.QuadernoDiCampagna &&
      !this.qdcservice.mostraTestataVisita()
    ) {
      mostra = true;
    }

    return mostra;
  }

}
