//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { of } from "rxjs";
//#endregion
@Component({
  selector: "app-area-combo",
  templateUrl: "./area-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: AreaComboComponent,
    },
  ],
})
export class AreaComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "area";
  @Input() placeholder: string = "";
  @Input() zoneId = undefined;
  
  entityName = "area";
  sidebarName = "area_sidebar";
  permissionName = "Areas";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Requeset
  getRequest() {
    return this.getDataService<DataServiceProxy>().getArea(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    if (this.zoneId) {
      return this.getDataService<DataServiceProxy>().getAreaByZones(
        this.zoneId == null ? 0 : this.zoneId,
        maxResult,
        skip,
        search,
        "",
        undefined
      );
    } else {
      return of({
        result: {
          items: [],
          totalCount: 0,
        },
      });
    }
  }
  //#endregion
}
