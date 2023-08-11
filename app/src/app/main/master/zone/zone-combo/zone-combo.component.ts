//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-zone-combo",
  templateUrl: "./zone-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ZoneComboComponent,
    },
  ],
})
export class ZoneComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "zone";
  @Input() placeholder: string = "";
  
  entityName = "zone";
  sidebarName = "zone_sidebar";
  permissionName = "Zones";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Requeset
  getRequest() {
    return this.getDataService<DataServiceProxy>().getZone(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getZones(
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
