//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-province-combo",
  templateUrl: "./province-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ProvinceComboComponent,
    },
  ],
})
export class ProvinceComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "province";
  
  entityName = "province";
  sidebarName = "province_sidebar";
  permissionName = "Provinces";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Requeset
  getRequest() {
    return this.getDataService<DataServiceProxy>().getProvince(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getProvinces(
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
