//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { AppSettingDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-app-setting-sidebar",
  templateUrl: "./app-setting-sidebar.component.html",
  styleUrls: ["./app-setting-sidebar.component.scss"],
})
export class AppSettingSidebarComponent
  extends SidebarEditFormComponentBase<AppSettingDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("app_setting_content_block") formBlockUI: NgBlockUI;

  entityName = "app_setting";
  sidebarName = "app_setting_sidebar";
  permissionName = "AppSettings";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      code: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
      ],
      value: ["", [Validators.required, Validators.maxLength(250)]],
      description: ["", [Validators.maxLength(250)]],
    });
  }

  get newModel() {
    return new AppSettingDto({
      code: undefined,
      description: undefined,
      value: undefined,
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getAppSetting(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateAppSetting(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createAppSetting(data);
  }
  //#endregion
}
