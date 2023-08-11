//#region Import
import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";

import { PermissionComponentBase } from "@cbms/ng-core-vuexy";
import {
  TicketInvestmentDto,
  TicketProgressDto,
  TicketProgressMaterialDto,
  TicketProgressRewardItemDto,
} from "@shared/services/data.service";
import { formatDate } from "devextreme/localization";
import moment from "moment";

import { TicketProgressItemComponent } from "../ticket-progress-item/ticket-progress-item.component";
//#endregion

@Component({
  selector: "app-ticket-progress-list",
  templateUrl: "./ticket-progress-list.component.html",
  styleUrls: ["./ticket-progress-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketProgressListComponent extends PermissionComponentBase {
  newProgress = undefined;
  _investment: TicketInvestmentDto;

  @ViewChild("newProgressItem")
  newProgressCompomnent: TicketProgressItemComponent;

  @Input() set investment(value) {
    this._investment = value;
  }

  get investment() {
    return this._investment;
  }

  get ticketInvestmentId() {
    return this._investment?.id;
  }

  get isNew(): boolean {
    return this.newProgress != undefined;
  }

  get progresses(): TicketProgressDto[] {
    return this._investment ? this._investment.progresses : [];
  }

  get isEmpty() {
    return this.progresses?.length == 0 && !this.newProgress;
  }
  create() {
    if (!this.newProgress) {
      var sortProgresses = this._investment.progresses
        .filter((p) => p.id)
        .sort(
          (x, y) => +moment(x.creationTime).isBefore(moment(y.creationTime))
        );
      var lastProgress: TicketProgressDto =
        sortProgresses.length > 0
          ? (lastProgress = sortProgresses[0])
          : undefined;
      if (!lastProgress)
        lastProgress = new TicketProgressDto({
          materials: [],
          rewardItems: [],
        });
      this.newProgress = new TicketProgressDto({
        materials: this._investment.materials.map(
          (item) =>
            new TicketProgressMaterialDto({
              materialId: item.materialId,
              materialName: item.materialName,
              isDesign: item.isDesign,
              isReceived: lastProgress.materials.find(
                (p) => p.materialId == item.materialId
              )
                ? lastProgress.materials.find(
                    (p) => p.materialId == item.materialId
                  ).isReceived
                : false,
              isSentDesign: lastProgress.materials.find(
                (p) => p.materialId == item.materialId
              )
                ? lastProgress.materials.find(
                    (p) => p.materialId == item.materialId
                  ).isSentDesign
                : false,
              materialCode: item.materialCode,
              registerQuantity: item.registerQuantity,
              amount: item.amount,
            })
        ),
        rewardItems: this._investment.rewardItems.map(
          (item) =>
            new TicketProgressRewardItemDto({
              rewardItemId: item.rewardItemId,
              rewardItemName: item.rewardItemName,
              price: item.price,
              isReceived: lastProgress.rewardItems.find(
                (p) => p.rewardItemId == item.rewardItemId
              )
                ? lastProgress.rewardItems.find(
                    (p) => p.rewardItemId == item.rewardItemId
                  ).isReceived
                : false,
              documentLink: item.documentLink,
              rewardItemCode: item.rewardItemCode,
              quantity: item.quantity,
            })
        ),
      });
    }
  }

  save() {
    this.newProgressCompomnent.save();
  }

  refesh() {
    this.newProgress = undefined;
  }

  progressTitle(item: TicketProgressDto) {
    return (
      item.updateUserName +
      " " +
      formatDate(item.updateTime, "shortDateShortTime")
    );
  }
}
