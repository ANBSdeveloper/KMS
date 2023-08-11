//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { StatusActiveDataSource } from "../reward-package/data-source/status-active.data-source";

//#endregion
@Component({
  selector: "app-status-active-combo",
  templateUrl: "./status-active-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: StatusActiveComboComponent,
    },
  ],
})
export class StatusActiveComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "status";
  @Input() placeholder: string = "";
  //#endregion

  constructor(injector: Injector, private statusActiveDataSource: StatusActiveDataSource) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of(this.statusActiveDataSource.getItem(this.value));
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.statusActiveDataSource.items,
        totalCount: this.statusActiveDataSource.items.length
      },
      success: true
    })
  }
  //#endregion
}
