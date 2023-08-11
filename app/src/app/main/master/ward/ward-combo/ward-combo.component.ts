//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-ward-combo",
  templateUrl: "./ward-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: WardComboComponent,
    },
  ],
})
export class WardComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "ward";
  @Input() set districtId(value: number) {
    const temp = !value ? undefined : value;
    if (this._districtId != temp) {
      this.value = undefined;
      this.loadData();
    }
    // for province combo's clear button
    if (temp === undefined) {
      this.value = undefined;
    }
  }
  get districtId(): number {
    return this._districtId;
  }
  _districtId: number;

  entityName = "ward";
  sidebarName = "ward_sidebar";
  permissionName = "Wards";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  clear() {
    this.value = undefined;
  }
  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getWard(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getWards(
      !this.districtId ? undefined : this.districtId,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
