import { Component, ElementRef, EventEmitter, Inject, Input, OnInit, Output, SkipSelf, ViewChild } from '@angular/core';
import { ControlContainer, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { IntlService } from '@progress/kendo-angular-intl';
import { DateTimePickerComponent, FormatSettings } from '@progress/kendo-angular-dateinputs';
import { Subject, takeUntil } from 'rxjs';
import { AGRODATAFINE, AGRODATAINIZIO, enum_InputType } from '../utils/models';
import { GIAS_MASTER_SERVICE_TOKEN, IGiasMasterService } from '../utils/gias-master.service';
import { GiasFormErrorVisualizerService } from '../gias-form-error-visualizer/gias-form-error-visualizer.service';
import { recursiveParentName } from '../utils/recursive-parent-name';

@Component({
  standalone: false,
  selector: 'gias-date-time-picker-template',
  templateUrl: './gias-date-time-picker-template.component.html',
  styleUrls: ['./gias-date-time-picker-template.component.scss'],
  viewProviders: [{
    provide: ControlContainer,
    useFactory: (container: ControlContainer) => container,
    deps: [[new SkipSelf(), ControlContainer]],
  }]
})
export class GiasDateTimePickerTemplateComponent implements OnInit {

  @ViewChild('datetimepicker') dateTimePicker: DateTimePickerComponent;

  @Input() public name: string;
  @Input() public value: Date;
  @Input() public nullValue: Date;
  @Input() public startValue: Date;
  @Input() public endValue: Date;
  @Input() public obligatory = false;
  @Input() public isDisabled: boolean;
  @Input() giasFormControlName: string;
  @Input() parentGroup: FormGroup;
  @Input() readOnlyInput: boolean;
  @Input() disabledDatesValidation: boolean = true;
  @Input() disabledDates: Date[] = [];
  @Output() valueChange = new EventEmitter<Date>();
  @Output() onBlur = new EventEmitter();

  public DateValue: Date;
  public signal$: Subject<void> = new Subject();
  public completeFormControlName: string;

  public format: FormatSettings = {
    displayFormat: "dd/MM/yyyy HH:mm",
    inputFormat: "dd/MM/yyyy HH:mm",
  };

  fg: FormControl;
  inputType: enum_InputType;

  private millennium = 2000;

  constructor(@Inject(GIAS_MASTER_SERVICE_TOKEN) private masterService: IGiasMasterService,
    private rootFormGroup: FormGroupDirective,
    public intl: IntlService,
    private elementRef: ElementRef,
    private formErrorVisualizerService: GiasFormErrorVisualizerService) {
    this.masterService.currentInputType.subscribe((it) => {
      this.inputType = it;
    });
  }

  get validator() {
    if (this.fg.hasValidator(Validators.required)) {
      return true;
    } else {
      return false;
    }
  }

  ngOnInit(): void {
    this.fg = this.rootFormGroup.form.controls[this.giasFormControlName] as FormControl;
    if (this.fg) {
      this.completeFormControlName = recursiveParentName(this.fg);
      this.millennium = (this.fg.value as Date).getFullYear() < 2000 ? 1900 : 2000;
    }
    if (this.startValue === undefined || this.startValue == null) {
      this.startValue = AGRODATAINIZIO;
    }

    if (this.endValue === undefined || this.endValue == null) {
      this.endValue = AGRODATAFINE;
    }

    this.DateValue = AGRODATAINIZIO;
    try {
      this.DateValue = this.value;
    } catch (e) {

    }

    this.fg?.statusChanges.pipe(takeUntil(this.signal$)).subscribe((val) => {
      this.updateFormErrorVisualizer();
    });

    this.fg?.valueChanges.pipe(takeUntil(this.signal$)).subscribe((val) => {
      this.updateFormErrorVisualizer();
    });

    this.updateFormErrorVisualizer();

  }

  onChange(event: Date) {
    if (event == null) {
      // this.DateValue = this.nullValue;
      // event = this.nullValue;
      // this.controlContainer.control.get(this.giasFormControlName).setValue(event);
    }

    let adjustedYear;
    if (event.getFullYear() <= 100) {
      // Modifica rapida, cambio per la prima volta il valore
      adjustedYear = event.getFullYear() + this.millennium;
    } else if (this.millennium === 2000 && !this.isXXcentury(event.getFullYear())) {
      // Sono nel XXI secolo, sto facendo più modifiche all'anno della data
      adjustedYear = event.getFullYear() % 100 + 2000;
    } else if (this.millennium === 1900 && !this.isXXIcentury(event.getFullYear())) {
      // Sono nel XX secolo, sto facendo più modifiche all'anno della data
      adjustedYear = event.getFullYear() % 100 + 1900;
    } else {
      // Sto scrivendo l'anno per intero (cambio millennio)
      adjustedYear = event.getFullYear();
      this.millennium = this.isXXcentury(event.getFullYear()) ? 1900 : 2000;
    }
    // console.log(event.getFullYear() + " --> ", adjustedYear, event.getMonth(), event.getDate());

    this.fg.patchValue(new Date(adjustedYear, event.getMonth(), event.getDate(), event.getHours(), event.getMinutes()));
    this.valueChange.emit(event);
  }

  ngAfterViewInit() {
    if (this.dateTimePicker.value && this.nullValue && this.dateTimePicker.value.getTime() === this.nullValue.getTime()) {
      this.elementRef.nativeElement.querySelector('input').value = '';
    }
  }

  handleFocus() {
    if (this.dateTimePicker.value && this.nullValue && this.dateTimePicker.value.getTime() === this.nullValue.getTime()) {
      this.elementRef.nativeElement.querySelector('input').value = '';
    }
  }

  handleBlur() {
    if (this.dateTimePicker.value && this.nullValue && this.dateTimePicker.value.getTime() === this.nullValue.getTime()) {
      this.elementRef.nativeElement.querySelector('input').value = '';
    }
    this.onBlur.emit();
  }

  onClose() {
    if (this.dateTimePicker.value && this.nullValue && this.dateTimePicker.value.getTime() === this.nullValue.getTime()) {
      this.elementRef.nativeElement.querySelector('input').value = '';
    }
  }

  onOpen() {
    if (this.dateTimePicker.value && this.nullValue && this.dateTimePicker.value.getTime() === this.nullValue.getTime()) {
      this.elementRef.nativeElement.querySelector('input').value = '';
    }
  }

  ngOnDestroy(): void {
    this.signal$.next();
    this.signal$.complete();
  }

  updateFormErrorVisualizer() {
    this.formErrorVisualizerService.handleFormControl(this.name, this.completeFormControlName, this.fg);
  }

  private isXXcentury(year: number): boolean {
    // Sto scrivendo un numero a 3 cifre che inizia con 19
    if (year / 1000 < 1 && (year - (year % 10)) / 10 === 19) return true;

    // Ho scrtitto un numero a 4 cifre che inizia con 19
    return (year - (year % 100)) / 100 === 19;
  }

  private isXXIcentury(year: number): boolean {
    if (year > 9000) year -= 9000;
    // Sto scrivendo un numero a 3 cifre che inizia con 20
    if (year / 1000 < 1 && (year - (year % 10)) / 10 === 20) return true;

    // Ho scrtitto un numero a 4 cifre che inizia con 20
    return (year - (year % 100)) / 100 === 20;
  }

}
