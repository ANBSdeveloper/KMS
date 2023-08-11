//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-product-class-combo",
  templateUrl: "./product-class-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ProductClassComboComponent,
    },
  ],
})
export class ProductClassComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "product_class";
  @Input() placeholder: string = "";
  
  entityName = "product_class";
  sidebarName = "product_class_sidebar";
  permissionName = "ProductClasses";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getProductClass(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getProductClasses(
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
