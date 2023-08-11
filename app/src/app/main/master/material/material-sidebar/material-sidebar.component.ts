//#region Import
import { Component, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { MaterialDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-material-sidebar",
  templateUrl: "./material-sidebar.component.html",
  styleUrls: ["./material-sidebar.component.scss"],
})
export class MaterialSidebarComponent
  extends SidebarEditFormComponentBase<MaterialDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("material_content_block") formBlockUI: NgBlockUI;

  entityName = "material";
  sidebarName = "material_sidebar";
  permissionName = "Materials";
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
      description: [""], //["", [Validators.required, Validators.maxLength(2000)]],
      value: [0, [Validators.min(0)]],
      isActive: [{ value: true, disabled: this.readOnly }],
      isDesign: [{ value: true, disabled: this.readOnly }],
      materialTypeId: [undefined, [Validators.required]],
    });
  }

  get newModel() {
    return new MaterialDto({
      name: "",
      code: "",
      materialTypeId: undefined,
      description: "",
      value: 0,
      isActive: true,
      isDesign: true,
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getMaterial(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateMaterial(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createMaterial(data);
  }
  //#endregion
}
