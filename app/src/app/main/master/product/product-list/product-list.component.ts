//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  ProductBaseDto,
  DataServiceProxy,
  InvestmentSettingDtoApiResultObject,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { AbstractControl, FormGroup, Validators } from "@angular/forms";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { Observable } from "rxjs";
//#endregion
@Component({
  selector: "app-product-list",
  templateUrl: "./product-list.component.html",
  styleUrls: ["./product-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ProductListComponent extends DxListComponentBase<ProductBaseDto> {
  //#region Variables
  entityName = "product";
  // sidebarName = "product_sidebar";
  permissionName = "Products";
  formGroup: FormGroup;
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }
  init() {
    this.filterFormGroup = this.fb.group({
      productClassId: [undefined],
      statusActive: [undefined],
    });
    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }
  showDetail(row: ProductBaseDto) {
    this.router.navigate([`master/product/${row.id}`]);
  }
  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var productClass = this.c("productClassId").value
      ? this.c("productClassId").value
      : undefined;
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getProductItemClasses(
      isActive,
      productClass,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
}
