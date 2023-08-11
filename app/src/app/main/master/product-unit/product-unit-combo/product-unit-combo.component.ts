//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { of } from "rxjs";
//#endregion
@Component({
  selector: "app-product-unit-combo",
  templateUrl: "./product-unit-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ProductUnitComboComponent,
    },
  ],
})
export class ProductUnitComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "product_unit";
  @Input() productId = undefined;
  
  entityName = "product_unit";
  sidebarName = "product_unit_sidebar";
  permissionName = "ProductUnits";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getProductUnit(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    if (this.productId) {
      return this.getDataService<DataServiceProxy>().getProductUnits(
        this.productId,
        maxResult,
        skip,
        search,
        "",
        undefined
      );
    } else {
      return of({
        result: {
          items: [],
          totalCount: 0,
        },
      });
    }
  }
  //#endregion
}
