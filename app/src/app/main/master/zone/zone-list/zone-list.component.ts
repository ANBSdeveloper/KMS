//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DistrictDto,
  DataServiceProxy,
  ZoneDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-zone-list",
  templateUrl: "./zone-list.component.html",
  styleUrls: ["./zone-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ZoneListComponent extends DxListComponentBase<ZoneDto> {
  //#region Variables
  entityName = "zone";
  sidebarName = "zone_sidebar";
  permissionName = "Zones";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getZones(
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
