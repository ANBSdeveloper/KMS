//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DatatableDataSource,
  DataTableEntityConfig,
  FormComponentBase,
} from "@cbms/ng-core-vuexy";
import { DynamicDialogRef, DynamicDialogConfig } from "primeng/dynamicdialog";
import {
  ConsumerRewardDetail,
  ConsumerRewardDetailCrudListDto,
  DataServiceProxy,
  TicketConsumerRewardDetailDto,
  TicketConsumerRewardDto,
  TicketInvestmentUpsertConsumerRewardCommand,
  TicketInvestmentUpsertConsumerRewardDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { Validators } from "@angular/forms";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import { finalize } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-ticket-consumer-reward-detail-dialog",
  templateUrl: "./ticket-consumer-reward-detail-dialog.component.html",
  styleUrls: ["./ticket-consumer-reward-detail-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketConsumerRewardDetailDialogComponent extends FormComponentBase {
  @BlockUI("consumer_reward_detail_dialog_block") formBlockUI: NgBlockUI;
  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;

  readOnly: false;
  allocateTickets: TicketConsumerRewardDetailDto[];
  rewardDetailDataSource = new DatatableDataSource<ConsumerRewardDetail>();
  consumerReward: TicketConsumerRewardDto;
  rewardDetailConfig = <DataTableEntityConfig>{
    entityName: "ticket_consumer_reward_detail",
    sidebarName: "ticket_consumer_reward_detail_sidebar",
  };

  photos = [];
  ticketInvestmentId: number = 0;
  constructor(
    injector: Injector,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      rewardQuantity: [undefined, [Validators.required]],
      rewardItemName: [undefined, [Validators.required]],
    });
  }

  ngOnInit() {
    this.consumerReward = this.config.data.consumerReward;
    this.ticketInvestmentId = this.config.data.ticketInvestmentId;
    this.allocateTickets = this.config.data.allocateTickets;
    this.c("rewardItemName").setValue(this.consumerReward.rewardItemName);
    this.c("rewardQuantity").setValue(this.consumerReward.rewardQuantity);

    this.rewardDetailDataSource.setData(this.consumerReward.details);
    this.readOnly = this.config.data.readOnly;
    this.photos = [
      this.consumerReward.photo1,
      this.consumerReward.photo2,
      this.consumerReward.photo3,
      this.consumerReward.photo4,
      this.consumerReward.photo5,
    ];

    this.rewardDetailDataSource.onUpdate.subscribe(() => {
      this.calcQuantity();
    });
  }

  calcQuantity() {
    var quantity = 0;
    this.rewardDetailDataSource.items.forEach((item) => quantity++);

    this.c("rewardQuantity").setValue(quantity);
  }
  addTicket() {
    this.entityHandler.loadRequest(
      this.rewardDetailConfig.entityName,
      undefined
    );
    this.sidebarService
      .getSidebarRegistry(this.rewardDetailConfig.sidebarName)
      .open();
  }

  close() {
    this.ref.close();
  }

  save() {
    var command = new TicketInvestmentUpsertConsumerRewardCommand({
      data: new TicketInvestmentUpsertConsumerRewardDto({
        photo1: this.imageViewer.getData()[0],
        photo2: this.imageViewer.getData()[1],
        photo3: this.imageViewer.getData()[2],
        photo4: this.imageViewer.getData()[3],
        photo5: this.imageViewer.getData()[4],
        rewardItemId: this.consumerReward.rewardItemId,
        detailChanges: new ConsumerRewardDetailCrudListDto({
          deletedItems: this.rewardDetailDataSource.submitData.deletedItems.map(
            (item) =>
              new ConsumerRewardDetail({
                id: item.id,
              })
          ),
          upsertedItems:
            this.rewardDetailDataSource.submitData.upsertedItems.map(
              (item) =>
                new ConsumerRewardDetail({
                  id: item.id,
                  ticketId: item.ticketId,
                  note: item.note,
                })
            ),
        }),
      }),
    });
    this.formBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .updateTicketInvestmentConsumerReward(
        this.ticketInvestmentId,
        this.consumerReward.rewardItemId,
        command
      )
      .pipe(finalize(() => this.formBlockUI.stop()))
      .subscribe(
        (response) => {
          this.messageService.toastSuccess(
            this.l("submit_success_message_title"),
            this.l("submit_success_message_content")
          );
          this.config.data.change(response.result);
          this.rewardDetailDataSource.setData(response.result.details);
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  deleteTicketVisible(row: TicketConsumerRewardDetailDto) {
    return !this.readOnly;
  }

  editTicketVisible(row: TicketConsumerRewardDetailDto) {
    return !this.readOnly;
  }

  deleteTicket(row: TicketConsumerRewardDetailDto) {
    this.rewardDetailDataSource.removeRecord(row);
    //this.calcuateTotal();
  }

  editTicket(row: TicketConsumerRewardDetailDto) {
    this.entityHandler.loadRequest(
      this.rewardDetailConfig.entityName,
      row["rowId"]
    );
    this.sidebarService
      .getSidebarRegistry(this.rewardDetailConfig.sidebarName)
      .open();
  }
  //#endregion
}
