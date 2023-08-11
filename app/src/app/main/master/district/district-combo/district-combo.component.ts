//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-district-combo",
  templateUrl: "./district-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: DistrictComboComponent,
    },
  ],
})
export class DistrictComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "district";
  @Input() set provinceId(value: number) {
    const temp = !value ? undefined : value;
    if (this._provinceId != temp) {
      this.value = undefined;
      this.loadData();
    }
    // for province combo's clear button
    if (temp === undefined) {
      this.value = undefined;
    }
  }
  get provinceId(): number {
    return this._provinceId;
  }
  _provinceId: number;

  entityName = "district";
  sidebarName = "district_sidebar";
  permissionName = "Districts";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  clear() {
    this.value = undefined;
  }
  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getDistrict(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    console.log(this.provinceId);
    return this.getDataService<DataServiceProxy>().getDistricts(
      !this.provinceId ? undefined : this.provinceId,
      maxResult,
      skip,
      search,
      "",
      undefined
    );
  }
  //#endregion
}
