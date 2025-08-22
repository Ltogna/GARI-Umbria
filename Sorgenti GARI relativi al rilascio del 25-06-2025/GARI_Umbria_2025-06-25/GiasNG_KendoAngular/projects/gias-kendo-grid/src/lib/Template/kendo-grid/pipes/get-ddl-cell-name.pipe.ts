import { Pipe, PipeTransform } from '@angular/core';
import { GridErrorService } from '../services/grid-log.service';
import { DropdownListItem, KendoGridColumn, KendoGridRow } from '../models/grid.model';

@Pipe({
    standalone: false,
  name: 'cellDropdownName',
  pure: true
})
export class CellDropdownNamePipe implements PipeTransform {

  constructor(public logService: GridErrorService) { }


  transform(row: KendoGridRow, column: KendoGridColumn): any {
    let ddlItem = null;

    if (column.ddl?.valuePrimitive) {
      ddlItem = column.ddl.data.find(x => row[column.ddl.formControlName] == x['id'] && row[column.ddl.descriptionField] == x['name']);
    } else {
      let value
      try {
        value = row[column.ddl.formControlName];
      } catch (e) {
        console.log(column.field);
      }
      value = row[column.ddl.formControlName];
      if (value?.id != null) {
        value = value.id;
      }
      ddlItem = column.ddl.data.find(x => value == x['id']);
    }

    if (ddlItem == null && column.ddl.loadOnEdit) {
      ddlItem = {
        name: row[column.ddl.descriptionField],
      };
      // if (column.ddl.data == null || column.ddl.data.length == 0) {
      //     const obj = new DropdownListItem(row[column.ddl.formControlName], row[column.ddl.descriptionField]);
      //     column.ddl.data = [obj];
      // } else {
      //     const obj = new DropdownListItem(row[column.ddl.formControlName], row[column.ddl.descriptionField]);
      //     column.ddl.data = [obj];
      // }
      const obj = new DropdownListItem(row[column.ddl.formControlName], row[column.ddl.descriptionField]);
      column.ddl.data = [obj];
    }
    return ddlItem?.name;
  }
}

