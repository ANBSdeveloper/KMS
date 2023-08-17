//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { DialogService } from "primeng/dynamicdialog";
import { map } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-posm-item-combo",
  templateUrl: "./posm-item-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmItemComboComponent,
    },
    DialogService,
  ],
})
export class PosmItemComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_item";
  @Input() posmClassId: number;
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() required = true;

  //#endregion

  constructor(injector: Injector, public dialogService: DialogService) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>()
      .getPosmItem(this.value)
      .pipe(
        map((res) => ({
          result: {
            ...res.result,
            displayName: res.result.code + " - " + res.result.name,
          },
        }))
      );
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>()
      .getPosmItems(
        true,
        this.posmClassId,
        maxResult,
        skip,
        search,
        "",
        undefined
      )
      .pipe(
        map((res) => ({
          result: {
            items: res.result.items.map((p) => ({
              ...p,
              displayName: p.code + " - " + p.name,
            })),
            totalCount: res.result.totalCount,
          },
        }))
      );
  }
  //#endregion

  get showPosmItemVisible() {
    return this.value;
  }
}
