import {LeggiPrescrizioni as ILeggiPrescrizioni} from "../../Service/net-core6-api.service";
import {enum_TipoPrescrizione} from "./tipo-prescrizione.enum";
import {TUTTI_CENTRI_AZIENDALI} from "../../Model/CostantiPersonalizzate";

export class LeggiPrescrizioni implements ILeggiPrescrizioni {
  constructor(
    public Piva: string,
    public Sa_Cod: number = TUTTI_CENTRI_AZIENDALI,
    public Sta_Num?: number | null,
    public Tipo_Cod?: number | null, // enum_TipoPrescrizione
    public Ricetta_Cod?: number | null,
    public Data_Emissione?: Date | null
  ) {
  }
}

export class LeggiPrescrizioniIndicazioni extends LeggiPrescrizioni {
  constructor(
    public Piva: string,
    public Sa_Cod: number = TUTTI_CENTRI_AZIENDALI,
    public Sta_Num?: number | null
  ) {
    super(Piva, Sa_Cod, Sta_Num, enum_TipoPrescrizione.Indicazione_Terapeutica);
  }
}

export class LeggiPrescrizioniProtocolli extends LeggiPrescrizioni {
  constructor(
    public Piva: string,
    public Sa_Cod: number = TUTTI_CENTRI_AZIENDALI,
    public Sta_Num?: number | null
  ) {
    super(Piva, Sa_Cod, Sta_Num, enum_TipoPrescrizione.Protocollo_Terapeutico);
  }
}
