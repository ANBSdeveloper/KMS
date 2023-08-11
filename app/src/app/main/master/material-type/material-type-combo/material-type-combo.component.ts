//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-material-type-combo",
  templateUrl: "./material-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: MaterialTypeComboComponent,
    },
  ],
})
export class MaterialTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "material_type";
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() required = false;

  entityName = "material-type";
  sidebarName = "material_type_sidebar";
  permissionName = "MaterialTypes";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Api Requeset
  getRequest() {
    return this.getDataService<DataServiceProxy>().getMaterialType(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getMaterialTypes(
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
