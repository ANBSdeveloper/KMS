//#region Import
import {
  Component,
  EventEmitter,
  Injector,
  Input,
  Output,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { FormComponentBase } from "@cbms/ng-core-vuexy";

import { AreaComboComponent } from "../../area/area-combo/area-combo.component";
import { ZoneComboComponent } from "../../zone/zone-combo/zone-combo.component";
//#endregion
@Component({
  selector: "app-filter-branch-by-zone-area",
  templateUrl: "./filter-branch-by-zone-area.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: FilterBranchByZoneAreaComponent,
    },
  ],
  encapsulation: ViewEncapsulation.None
})
export class FilterBranchByZoneAreaComponent extends FormComponentBase {
  @ViewChild("zoneCombo") zoneCombo: ZoneComboComponent;
  @ViewChild("areaCombo") areaCombo: AreaComboComponent;
  @Output() zoneId = new EventEmitter();
  @Output() areaId = new EventEmitter();
  @Output() searchValue = new EventEmitter();
  @Output() isDisplayAllBranch = new EventEmitter();

  @Input() set isSelectAll(value)  {
    this.formGroup.controls["isSelectAll"].setValue(value);
  }
  get isSelectAll() {
    return this.formGroup.controls["isSelectAll"].value;
  }

  constructor(injector: Injector) {
    super(injector);

    this.formGroup = this.fb.group({
      zoneId: [undefined],
      areaId: [undefined],
      isSelectAll: [undefined],
    });
  }

  //#endregion

  zoneComboChange(id) {
    this.zoneId.emit(id.value);
    this.areaCombo.value = undefined;
    setTimeout(() => {
      this.areaCombo.loadData();
    }, 50);
  }

  areaComboChange(record) {
    this.areaId.emit(record.value);
  }

  searchChange(e) {
    this.searchValue.emit(e.target.value);
  }

  displayAllBranchChange(e) {
    this.isDisplayAllBranch.emit(e.target.checked);
  }
}
