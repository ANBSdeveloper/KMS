//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-customer-location-combo",
  templateUrl: "./customer-location-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: CustomerLocationComboComponent,
    },
  ],
})
export class CustomerLocationComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "customer_location";
  @Input() required: boolean = false;
  
  entityName = "customer-location";
  sidebarName = "customer-location_sidebar";
  permissionName = "CustomerLocations";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getCustomerLocation(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getCustomerLocations(
      true,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
