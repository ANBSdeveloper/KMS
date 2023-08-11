//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-brand-combo",
  templateUrl: "./brand-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: BrandComboComponent,
    },
  ],
})
export class BrandComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "brand";
  
  entityName = "brand";
  sidebarName = "brand_sidebar";
  permissionName = "Brands";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getBrand(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getBrands(
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
