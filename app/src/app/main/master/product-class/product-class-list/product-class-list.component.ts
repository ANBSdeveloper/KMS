//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  ProductClassDto,
  DataServiceProxy,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-product-class-list",
  templateUrl: "./product-class-list.component.html",
  styleUrls: ["./product-class-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ProductClassListComponent extends DxListComponentBase<ProductClassDto> {
  //#region Variables
  entityName = "product_class";
  sidebarName = "product_class_sidebar";
  permissionName = "ProductClasses";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusActive: [undefined],
    });
    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getProductClasses(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteProductClass(id);
  }
  //#endregion
}
