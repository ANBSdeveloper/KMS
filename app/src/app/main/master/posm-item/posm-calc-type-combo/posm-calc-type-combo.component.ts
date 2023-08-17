//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { PosmCalcTypeDataSource } from "../data-source/posm-calc-type.data-source";
//#endregion
@Component({
  selector: "app-posm-calc-type-combo",
  templateUrl: "./posm-calc-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmCalcTypeComboComponent,
    },
  ],
})
export class PosmCalcTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_calc_type";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private posmCalcTypeDataSource: PosmCalcTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of(this.posmCalcTypeDataSource.findItem(this.value));
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.posmCalcTypeDataSource.items,
        totalCount: this.posmCalcTypeDataSource.items.length,
      },
      success: true,
    });
  }
  //#endregion
}
