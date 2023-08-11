//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { DistrictDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-district-list",
  templateUrl: "./district-list.component.html",
  styleUrls: ["./district-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class DistrictListComponent extends DxListComponentBase<DistrictDto> {
  //#region Variables
  entityName = "district";
  sidebarName = "district_sidebar";
  permissionName = "Districts";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getDistricts(
      undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    //return this.getDataService<DataServiceProxy>().deleteDistrict(id);
  }
  //#endregion
}
