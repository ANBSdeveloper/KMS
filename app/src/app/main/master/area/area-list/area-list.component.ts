//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DataServiceProxy,
  AreaByZoneNameListDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-area-list",
  templateUrl: "./area-list.component.html",
  styleUrls: ["./area-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AreaListComponent extends DxListComponentBase<AreaByZoneNameListDto> {
  //#region Variables
  entityName = "area";
  sidebarName = "area_sidebar";
  permissionName = "Areas";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getAreaByZoneNames(
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
