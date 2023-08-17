//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { PosmClassDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-posm-class-sidebar",
  templateUrl: "./posm-class-sidebar.component.html",
  styleUrls: ["./posm-class-sidebar.component.scss"],
})
export class PosmClassSidebarComponent
  extends SidebarEditFormComponentBase<PosmClassDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("posm-class-content-block") formBlockUI: NgBlockUI;

  entityName = "posm_class";
  sidebarName = "posm_class_sidebar";
  permissionName = "PosmClasses";
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
    return new PosmClassDto({
      isActive: true,
      includeInfo: false,
      name: "",
      code: "",
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getPosmClass(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updatePosmClass(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createPosmClass(data);
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
