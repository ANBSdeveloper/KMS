//#region Import
import { Component, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { DistrictDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-district-sidebar",
  templateUrl: "./district-sidebar.component.html",
  styleUrls: ["./district-sidebar.component.scss"],
})
export class DistrictSidebarComponent
  extends SidebarEditFormComponentBase<DistrictDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("district_content_block") formBlockUI: NgBlockUI;

  entityName = "district";
  sidebarName = "district_sidebar";
  permissionName = "Districts";
  //#endregion
  
  constructor(injector: Injector) {
    super(injector);
    this.formGroup = this.fb.group({});
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
      provinceId: [undefined, [Validators.required]],
      // isActive: [{ value: true, disabled: this.readOnly }],
    });
  }

  get newModel() {
    return new DistrictDto({
      name: "",
      code: "",
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getDistrict(id);
  }

  updateRequest(id, data) {
    //return this.getDataService<DataServiceProxy>().updateDistrict(id, data);
  }

  createRequest(data) {
    //return this.getDataService<DataServiceProxy>().createDistrict(data);
  }
  //#endregion
}
