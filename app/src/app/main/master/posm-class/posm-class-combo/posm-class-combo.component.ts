//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-posm-class-combo",
  templateUrl: "./posm-class-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmClassComboComponent,
    },
  ],
})
export class PosmClassComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_class";
  @Input() required: boolean = false;
  entityName = "posm_class";
  sidebarName = "posm_class_sidebar";
  permissionName = "PosmClasses";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getPosmClass(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getPosmClasses(
      true,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
