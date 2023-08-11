//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { map } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-product-combo",
  templateUrl: "./product-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ProductComboComponent,
    },
  ],
})
export class ProductComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "product";
  @Input() classCode: string = undefined;
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() required = true;

  entityName = "product";
  permissionName = "Products";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>()
      .getProduct(this.value)
      .pipe(
        map((res) => ({
          result: {
            ...res.result,
            displayName: res.result.code + " - " + res.result.name,
          },
        }))
      );
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>()
      .getProducts(true, this.classCode, maxResult, skip, search, "", undefined)
      .pipe(
        map((res) => ({
          result: {
            items: res.result.items.map((p) => ({
              ...p,
              displayName: p.code + " - " + p.name,
            })),
            totalCount: res.result.totalCount,
          },
        }))
      );
  }
  //#endregion
}
