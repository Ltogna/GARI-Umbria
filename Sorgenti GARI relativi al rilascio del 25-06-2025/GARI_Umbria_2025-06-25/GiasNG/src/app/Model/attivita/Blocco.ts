export class Blocco{

  tipo: Tipo_Blocco;
  utente: string;
  data: Date;
}

export enum Tipo_Blocco{
  Nessuno = 0,
  QuadernoDiCampagna = 1
}
