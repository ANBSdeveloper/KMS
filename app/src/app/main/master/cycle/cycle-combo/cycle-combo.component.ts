//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-cycle-combo",
  templateUrl: "./cycle-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: CycleComboComponent,
    },
  ],
})
export class CycleComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "number";
  @Input() valueName: string = "id";
  @Input() label: string = "cycle";
  @Input() required = true;

  entityName = "cycle";
  sidebarName = "cycle_sidebar";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getCycle(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getCycles(
      true,
      undefined,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
