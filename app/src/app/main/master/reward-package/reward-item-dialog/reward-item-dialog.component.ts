//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DatatableDataSource,
  PermissionComponentBase,
} from "@cbms/ng-core-vuexy";
import { DynamicDialogRef } from "primeng/dynamicdialog";
import { DynamicDialogConfig } from "primeng/dynamicdialog";
import { DataServiceProxy, RewardItemDto } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { finalize } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-reward-item-dialog",
  templateUrl: "./reward-item-dialog.component.html",
  styleUrls: ["./reward-item-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class RewardItemDialogComponent extends PermissionComponentBase {
  @BlockUI("reward_item_dialog_block") formBlockUI: NgBlockUI;
  rewardItemDataSource = new DatatableDataSource<RewardItemDto>();
  constructor(
    injector: Injector,
    private dataSerivce: DataServiceProxy,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.formBlockUI.start();
    this.dataSerivce
      .getRewardPackage(this.config.data.id)
      .pipe(finalize(() => this.formBlockUI.stop()))
      .subscribe(
        (response) => {
          this.rewardItemDataSource.setData(response.result.rewardItems);
        },
        (error) => this.messageService.toastError(error)
      );
  }
  //#endregion
}
