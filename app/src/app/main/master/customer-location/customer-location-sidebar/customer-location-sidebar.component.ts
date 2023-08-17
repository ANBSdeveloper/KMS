//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { CustomerLocationDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-customer-location-sidebar",
  templateUrl: "./customer-location-sidebar.component.html",
  styleUrls: ["./customer-location-sidebar.component.scss"],
})
export class CustomerLocationSidebarComponent
  extends SidebarEditFormComponentBase<CustomerLocationDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("customer-location_content_block") formBlockUI: NgBlockUI;

  entityName = "customer-location";
  sidebarName = "customer-location_sidebar";
  permissionName = "CustomerLocations";
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
      isActive: [{ value: true, disabled: this.readOnly }],
    });
  }
  
  get newModel() {
    return new CustomerLocationDto({
      isActive: true,
      name: "",
      code: "",
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getCustomerLocation(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateCustomerLocation(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createCustomerLocation(data);
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
