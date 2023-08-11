import {
  TicketAcceptanceDto,
  TicketConsumerRewardDetailDto,
  TicketDto,
} from "../../../../../shared/services/data.service";
//#region Import
import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { Validators } from "@angular/forms";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { SidebarEditGridComponentBase } from "@cbms/ng-core-vuexy";
import { TicketComboComponent } from "../ticket-combo/ticket-combo.component";

//#endregion
@Component({
  selector: "app-ticket-consumer-reward-detail-sidebar",
  templateUrl: "./ticket-consumer-reward-detail-sidebar.component.html",
  styleUrls: ["./ticket-consumer-reward-detail-sidebar.component.scss"],
})
export class TicketConsumerRewardDetailSidebarComponent
  extends SidebarEditGridComponentBase<TicketConsumerRewardDetailDto>
  implements OnInit
{
  //#region Variables
  @ViewChild("ticketCombo") ticketCombo: TicketComboComponent;
  @BlockUI("ticket_consumer_reward_detail_content_block")
  formBlockUI: NgBlockUI;

  @Input() rewardDetail: TicketConsumerRewardDetailDto;
  @Input() ticketInvestmentId;
  @Input() allocateTickets: TicketConsumerRewardDetailDto[];
  entityName = "ticket_consumer_reward_detail";
  sidebarName = "ticket_consumer_reward_detail_sidebar";
  permissionName = "TicketInvestments.Accept";

  //#endregion
  constructor(injector: Injector) {
    super(injector);

    this.formGroup = this.fb.group({});
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      ticketId: [undefined, [Validators.required]],
      consumerName: [undefined],
      consumerPhone: [undefined],
      note: [undefined],
    });
  }

  formValidate() {
    var duplicateCodeRow = this.dataSource.items.find(
      (item) =>
        item["rowId"] != this.rowId && item.ticketId == this.c("ticketId").value
    );
    if (duplicateCodeRow) {
      this.messageService.toastError(
        this.l("reward_ticket_is_used", duplicateCodeRow.ticketCode)
      );
      return false;
    }

    var otherUsed = this.allocateTickets.find(
      (item) => item.ticketId == this.c("ticketId").value
    );
    if (otherUsed) {
      this.messageService.toastError(
        this.l("reward_ticket_is_used", otherUsed.ticketCode)
      );
      return false;
    }
    return true;
  }

  mapFormGroupToSaveModel() {
    super.mapFormGroupToSaveModel();
    const ticket = <TicketDto>this.ticketCombo.selectedItem;
    this.saveModel.ticketCode = ticket.code;
  }

  get newModel() {
    return new TicketConsumerRewardDetailDto({});
  }

  ticketChange(e) {
    this.c("consumerName").setValue(
      this.ticketCombo.selectedItem?.consumerName
    );
    this.c("consumerPhone").setValue(
      this.ticketCombo.selectedItem?.consumerPhone
    );
  }
  //#endregion
}
