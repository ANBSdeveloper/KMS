//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  BrandDto,
  DataServiceProxy,
  SubProductClassDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-sub-product-class-list",
  templateUrl: "./sub-product-class-list.component.html",
  styleUrls: ["./sub-product-class-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class SubProductClassListComponent extends DxListComponentBase<SubProductClassDto> {
  //#region Variables
  entityName = "sub_product_class";
  sidebarName = "sub_product_class_sidebar";
  permissionName = "SubProductClasses";
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
    return this.getDataService<DataServiceProxy>().getSubProductClasses(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteSubProductClass(id);
  }
  //#endregion
}
