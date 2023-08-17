//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { VendorDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { filter, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-vendor-sidebar",
  templateUrl: "./vendor-sidebar.component.html",
  styleUrls: ["./vendor-sidebar.component.scss"],
})
export class VendorSidebarComponent
  extends SidebarEditFormComponentBase<VendorDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("vendor_content_block") formBlockUI: NgBlockUI;

  entityName = "vendor";
  sidebarName = "vendor_sidebar";
  permissionName = "Vendors";
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
      phone: ["", [Validators.required, Validators.maxLength(20)]],
      address: ["", [Validators.required, Validators.maxLength(200)]],
      zoneId: ["", []],
      taxReg: ["", [Validators.maxLength(20)]],
      representative: ["", [Validators.maxLength(500)]],
    });
  }
  
  get newModel() {
    return new VendorDto({
      isActive: true,
      name: "",
      code: "",
      taxReg: "",
      representative: ""
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getVendor(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateVendor(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createVendor(data);
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
