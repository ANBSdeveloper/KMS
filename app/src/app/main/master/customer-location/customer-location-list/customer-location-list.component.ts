//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { CustomerLocationDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";

//#endregion
@Component({
  selector: "app-customer-location-list",
  templateUrl: "./customer-location-list.component.html",
  styleUrls: ["./customer-location-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerLocationListComponent extends DxListComponentBase<CustomerLocationDto> {
  //#region Variables
  entityName = "customer-location";
  sidebarName = "customer-location_sidebar";
  permissionName = "CustomerLocations";
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
    return this.getDataService<DataServiceProxy>().getCustomerLocations(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteCustomerLocation(id);
  }
  //#endregion
}
