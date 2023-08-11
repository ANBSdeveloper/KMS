//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { DistrictDto, DataServiceProxy, ZoneDto, WardNameListDto } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-ward-list",
  templateUrl: "./ward-list.component.html",
  styleUrls: ["./ward-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class WardListComponent extends DxListComponentBase<WardNameListDto> {
  //#region Variables
  entityName = "ward";
  sidebarName = "ward_sidebar";
  permissionName = "Wards";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getWardNames(
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
