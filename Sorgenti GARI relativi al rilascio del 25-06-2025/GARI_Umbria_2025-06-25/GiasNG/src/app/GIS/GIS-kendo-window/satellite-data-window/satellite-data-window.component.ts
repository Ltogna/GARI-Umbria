/* eslint-disable */
import { Component, EventEmitter, Output } from "@angular/core";
import { enum_GISDrawingOperations } from "../../GIS-enum/GIS-drawing-operations";

@Component({
    standalone: false,
    selector: 'satellite-data-window',
    templateUrl: './satellite-data-window.component.html',
    styleUrls: ['./satellite-data-window.component.css']
})
export class LayerWindowComponent {
    @Output() operationEvent = new EventEmitter<enum_GISDrawingOperations>();

    opened: boolean = true;

    GISDrawingOperations = enum_GISDrawingOperations

    setOperation(op: enum_GISDrawingOperations) {
        this.operationEvent.emit(op);
    }

    public toggle(isOpened: boolean): void {
        //this.opened = isOpened;
    }
}
