//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { DialogService } from "primeng/dynamicdialog";
import { map } from "rxjs/operators";
import { CustomerDialogComponent } from "../customer-dialog/customer-dialog.component";
//#endregion
@Component({
  selector: "app-shop-combo",
  templateUrl: "./shop-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ShopComboComponent,
    },
  ],
})
export class ShopComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "shop";
  @Input() required: boolean = true;
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() dataSource = [];
  @Input() rsmStaffId = undefined;
  @Input() asmStaffId = undefined;
  @Input() ssStaffId = undefined;
  @Input() detailButton = false;
  //#endregion

  constructor(injector: Injector,  public dialogService: DialogService) {
    super(injector);
  }
  //#endregion

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getCustomer(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    var staffId = this.ssStaffId
      ? this.ssStaffId
      : this.asmStaffId
      ? this.asmStaffId
      : this.rsmStaffId
      ? this.rsmStaffId
      : "";
    return this.getDataService<DataServiceProxy>()
      .getCustomers(
        true,
        true,
        undefined,
        undefined,
        undefined,
        staffId,
        undefined,
        undefined,
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

  showDetail(_) {
    this.dialogService.open(CustomerDialogComponent, {
      data: {
        id: this.value,
      },
      baseZIndex: 1001,
      header: this.l("customer_information"),
      width: "70%",
    });
  }
  //#endregion
}
