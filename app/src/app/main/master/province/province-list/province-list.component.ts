//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { ProvinceDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-province-list",
  templateUrl: "./province-list.component.html",
  styleUrls: ["./province-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ProvinceListComponent extends DxListComponentBase<ProvinceDto> {
  //#region  Variables
  entityName = "province";
  sidebarName = "province_sidebar";
  permissionName = "Provinces";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getProvinces(
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    //return this.getDataService<DataServiceProxy>().deleteProvince(id);
  }
  //#endregion
}
