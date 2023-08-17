//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { PosmTypeDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-posm-type-sidebar",
  templateUrl: "./posm-type-sidebar.component.html",
  styleUrls: ["./posm-type-sidebar.component.scss"],
})
export class PosmTypeSidebarComponent
  extends SidebarEditFormComponentBase<PosmTypeDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("posm-type-content-block") formBlockUI: NgBlockUI;

  entityName = "posm_type";
  sidebarName = "posm_type_sidebar";
  permissionName = "PosmTypes";
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
      includeInfo: [{ value: false, disabled: this.readOnly }],
      isActive: [{ value: true, disabled: this.readOnly }],
    });
  }
  
  get newModel() {
    return new PosmTypeDto({
      isActive: true,
      name: "",
      code: "",
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getPosmType(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updatePosmType(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createPosmType(data);
  }
  //#endregion

  @HostListener('window:keydown', ['$event'])
  keydown(e: KeyboardEvent) {
    if (
     (<any>this.sidebar).isOpened && e.key == 's' &&
      (navigator.platform.match('Mac') ? e.metaKey : e.ctrlKey)
    ) {
      console.log(e);
      e.preventDefault();
      this.submit();
    }
  }
}
