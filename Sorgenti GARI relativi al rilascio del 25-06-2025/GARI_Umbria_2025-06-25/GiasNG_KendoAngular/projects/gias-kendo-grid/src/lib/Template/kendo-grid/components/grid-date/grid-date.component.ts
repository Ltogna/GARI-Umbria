import { Component } from '@angular/core';

@Component({
    standalone: false,
    selector: 'gias-grid-date',
    templateUrl: './grid-date.component.html',
    styleUrls: ['./grid-date.component.css'],
})
export class GridDateTimeComponent {

    public value: Date = new Date(2000, 2, 10);

}
