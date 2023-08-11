//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { RewardTypeDataSource } from "../data-source/reward-type.data-source";
//#endregion
@Component({
  selector: "app-reward-type-combo",
  templateUrl: "./reward-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: RewardTypeComboComponent,
    },
  ],
})
export class RewardTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "reward_type";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private rewardTypeDataSource: RewardTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of(this.rewardTypeDataSource.getItem(this.value));
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.rewardTypeDataSource.items,
        totalCount: this.rewardTypeDataSource.items.length,
      },
      success: true,
    });
  }
  //#endregion
}
