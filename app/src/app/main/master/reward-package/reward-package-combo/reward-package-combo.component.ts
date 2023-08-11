//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import moment from "moment";
import { DialogService } from "primeng/dynamicdialog";
import { map } from "rxjs/operators";
import { RewardType } from "../data-source/reward-type.data-source";
import { RewardItemDialogComponent } from "../reward-item-dialog/reward-item-dialog.component";
//#endregion
@Component({
  selector: "app-reward-package-combo",
  templateUrl: "./reward-package-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: RewardPackageComboComponent,
    },
    DialogService,
  ],
})
export class RewardPackageComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "reward_package";
  @Input() customerId: number;
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
      .getRewardPackage(this.value)
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
      .getRewardPackageListByTypeCustomerId(
        RewardType.BTTT,
        this.customerId,
        true,
        moment().startOf("day").toDate(),
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

  get showRewardPackageVisible() {
    return this.value;
  }

  showRewardPackage() {
    const ref = this.dialogService.open(RewardItemDialogComponent, {
      data: {
        id: this.value,
      },
      baseZIndex: 2000,
      header: this.l("reward_package_title", this.selectedItem.name),
      width: "70%",
    });
  }
}
