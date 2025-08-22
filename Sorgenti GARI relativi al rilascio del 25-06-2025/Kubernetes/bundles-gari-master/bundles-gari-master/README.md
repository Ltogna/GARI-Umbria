# Umbria GARI bundles

## Initial installation

Dopo aver installato quesi bundle è necessario:
1. deploy `etl-import-lpis-base-data`
   1. Se il deploy va in timeout e il secondo job non si è avviato, rilanciare con `helm install etl-import-lpis-base-data nexus/etl-import-lpis-base-data --set runEtl=false`
2. deploy `etl-import-parties-acl-caa`
3. Eseguire `initial-setup/star.rest` (è importante farlo solo dopo aver fatto il deploy di `etl-import-parties-acl-caa`)
4. Eseguire `initial-setup/load-regional-office.rest` per caricare l'ufficio regionale a cui vengono collegate tutte le aziende
5. Caricare le ZVN regionali da sian, eseguendo `initial-setup/load-zvn.rest`

## Utenti test

* ada -> Utente CAA
* cristoforocolombo -> Consultazione Anagrafiche