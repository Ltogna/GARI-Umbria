import {Component, Input, OnInit} from '@angular/core';
import { GISModality } from "app/GIS/GIS-enum/GIS-feature";
import { enum_PagineGiasNG } from "app/Model/TipiEnumerativi";
import { Enum_SiteRedirector } from "app/Model/siti.enum";
import { GestioneRichiesteService } from "app/Service/gestione-richieste.service";
import { ObjParametriAgendaService } from "app/Service/obj-parametri-agenda.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import {FunzioniComuniService} from '../../../Service/FunzioniComuni.service';
import {environment} from '../../../../environments/environment';
import {ConfigurazioneSitiService, EnumChiaviConfigurazioneSiti} from '../../../Service/configurazione-siti.service';

@Component({
  standalone: false,
  selector: "app-mappa-multiazienda-widget",
  templateUrl: "./mappa-multiazienda-widget.component.html",
  styleUrls: ["./mappa-multiazienda-widget.component.scss"],
})
export class MappaMultiAziendaComponent implements OnInit {
  @Input() codice: string | null = null;
  loading: boolean;
  urlGIS: string;

  constructor(
    private gestioneRichiesteService: GestioneRichiesteService,
    private objParametriAgendaService: ObjParametriAgendaService,
    private gestioneMultiAziendaService: GestioneMultiAziendaService
  ) {  }

  ngOnInit(): void {
    this.initMap();
  }

  initMap(): void {
    this.loading = true;

    this.gestioneRichiesteService.getPathPrefixGestionePassaggioAltroSitoIFrame()
      .subscribe(prefix => {
        this.gestioneRichiesteService.gestionePassaggioAltroSito(
          Enum_SiteRedirector.GiasNG,
          enum_PagineGiasNG.Pagina_GIS,
          [],
          this.objParametriAgendaService.getObjParamValue()
        ).then((resp) => {
          let modality: GISModality;

          if (this.codice == "MappaPaesePoligoni")
            modality = GISModality.PaesePoligoniMultiAzienda;
          else modality = GISModality.PaeseDistribuzioneMultiAzienda;

          let year =
            this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda[
              "text"
              ] ?? "";
          let country =
            this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda[
              "code"
              ] ?? "";

          this.urlGIS =
            this.gestioneRichiesteService.getAbsolutePath(
              ":" + window.location.port + prefix + "/" + resp
            ) +
            "?seFrame=1&modalita=" + modality
            + "&nazione=" + country
            + "&anno=" + year;
        }).finally(() => {
          this.loading = false;
        });
      });
  }
}
