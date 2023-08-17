//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { PosmRequestTypeDataSource } from "../data-source/posm-request-type.data-source";
//#endregion
@Component({
  selector: "app-posm-request-type-combo",
  templateUrl: "./posm-request-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmRequestTypeComboComponent,
    },
  ],
})
export class PosmRequestTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_request_type";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private posmRequestTypeDataSource: PosmRequestTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of(this.posmRequestTypeDataSource.findItem(this.value));
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.posmRequestTypeDataSource.items,
        totalCount: this.posmRequestTypeDataSource.items.length,
      },
      success: true,
    });
  }
  //#endregion
}
