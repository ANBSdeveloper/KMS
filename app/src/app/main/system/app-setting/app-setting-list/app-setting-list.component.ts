//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { AppSettingDto, BrandDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
//#endregion
@Component({
  selector: "app-app-setting-list",
  templateUrl: "./app-setting-list.component.html",
  styleUrls: ["./app-setting-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AppSettingListComponent extends DxListComponentBase<AppSettingDto> {
  //#region Variables
  entityName = "app_setting";
  sidebarName = "app_setting_sidebar";
  permissionName = "AppSettings";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    return this.getDataService<DataServiceProxy>().getAppSettings(
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteAppSetting(id);
  }
  //#endregion
}
