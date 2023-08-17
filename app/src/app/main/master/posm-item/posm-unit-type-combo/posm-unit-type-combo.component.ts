//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { PosmUnitTypeDataSource } from "../data-source/posm-unit-type.data-source";
//#endregion
@Component({
  selector: "app-posm-unit-type-combo",
  templateUrl: "./posm-unit-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmUnitTypeComboComponent,
    },
  ],
})
export class PosmUnitTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_unit_type";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private posmUnitTypeDataSource: PosmUnitTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of(this.posmUnitTypeDataSource.findItem(this.value));
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.posmUnitTypeDataSource.items,
        totalCount: this.posmUnitTypeDataSource.items.length,
      },
      success: true,
    });
  }
  //#endregion
}
