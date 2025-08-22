//#region Zootecnia NEW

export enum enum_Tipo_RisorsaUmana {
    Proprietario = "proprietario",
    Veterinario = "veterinario"
}

export enum enum_UdM_Dose {
  undefined = 0,
  ml_su_100Kg = 2152,
  ml_su_Capo = 2153,
  n_su_Capo = 2154,
}

export const enumUdMDoseArray = Object.entries(enum_UdM_Dose)
  .filter(([key, value]) => typeof value === 'number') // Filtra solo i valori numerici
  .map(([key, value]) => ({
    id: value as number, // Il valore numerico dell'enumerativo
    descrizione: getDescrizione(key) // Descrizione personalizzata
  }));

/**
 * Funzione per generare descrizioni personalizzate.
 * Crea la chiave da usare con transloco per ottenere la traduzione corretta.
 */ 
function getDescrizione(value: string | enum_UdM_Dose): string {
  console.debug('getDescrizione', value);
  return "Zoo_" + value;
}

export enum enum_TypeTab_Zootecnia {
  ZooOperations = 0,
  Prescriptions = 1,
  FuturePrescriptions = 2
}
  
//#endregion