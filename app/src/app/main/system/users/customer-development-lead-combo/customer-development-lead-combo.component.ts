//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { DialogService } from "primeng/dynamicdialog";
import { RoleType } from "../../role/role-type.enum";
//#endregion
@Component({
  selector: "app-customer-development-lead-combo",
  templateUrl: "./customer-development-lead-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: CustomerDevelopmentLeadComboComponent,
    },
    DialogService,
  ],
})
export class CustomerDevelopmentLeadComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "user";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  //#endregion

  constructor(injector: Injector, public dialogService: DialogService) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getUser(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getUsers(
      true,
      undefined,
      RoleType.CustomerDevelopmentLead,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion

  get showUserVisible() {
    return this.value;
  }
}
