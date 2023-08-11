//#region Import
import { Component, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { ProvinceDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-province-sidebar",
  templateUrl: "./province-sidebar.component.html",
  styleUrls: ["./province-sidebar.component.scss"],
})
export class ProvinceSidebarComponent
  extends SidebarEditFormComponentBase<ProvinceDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("province_content_block") formBlockUI: NgBlockUI;

  entityName = "province";
  sidebarName = "province_sidebar";
  permissionName = "Provinces";
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
      name: ["", [Validators.required, Validators.maxLength(250)]],
      // isActive: [{ value: true, disabled: this.readOnly }],
    });
  }

  get newModel() {
    return new ProvinceDto({
      name: "",
      code: "",
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getProvince(id);
  }

  updateRequest(id, data) {
    //return this.getDataService<DataServiceProxy>().updateProvince(id, data);
  }

  createRequest(data) {
    //return this.getDataService<DataServiceProxy>().createProvince(data);
  }
  //#endregion
}
