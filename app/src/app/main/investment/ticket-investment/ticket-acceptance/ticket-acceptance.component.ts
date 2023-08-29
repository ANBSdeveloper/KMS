//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { cloneDeep } from "lodash";
import {
  DatatableDataSource,
  EntityHandlerService,
  FormComponentBase,
  formHelper,
} from "@cbms/ng-core-vuexy";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import {
  DataServiceProxy,
  TicketAcceptanceDto,
  TicketConsumerRewardDto,
  TicketInvestmentCompanyRemarkCommand,
  TicketInvestmentCustomerDevelopmentRemarkCommand,
  TicketInvestmentDto,
  TicketInvestmentRemarkDto,
  TicketInvestmentSalesRemarkCommand,
  TicketInvestmentSummaryDto,
  TicketInvestmentUpsertAcceptanceCommand,
  TicketInvestmentUpsertAcceptanceDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { finalize } from "rxjs/operators";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import moment from "moment";
import { TicketConsumerRewardDetailDialogComponent } from "../ticket-consumer-reward-detal-dialog/ticket-consumer-reward-detail-dialog.component";
import { DialogService } from "primeng/dynamicdialog";
import { isBuffer } from "node:util";
import { environment } from "environments/environment";
//#endregion

@Component({
  selector: "app-ticket-acceptance",
  templateUrl: "./ticket-acceptance.component.html",
  styleUrls: ["./ticket-acceptance.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketAcceptanceComponent extends FormComponentBase {
  photos = [];
  remarkOfSales;
  remarkOfCustomerDevelopement;
  remarkOfCompany;
  rewardItemDataSource = new DatatableDataSource<TicketConsumerRewardDto>();
  @BlockUI() pageBlockUI: NgBlockUI;

  @Input() set investment(value: TicketInvestmentDto) {
    this._investment = value;
    this.mapToForm();
  }

  get investment() {
    return this._investment;
  }

  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;

  _investment: TicketInvestmentDto;
  model: TicketAcceptanceDto;
  summary: TicketInvestmentSummaryDto;
  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public entityHandler: EntityHandlerService,
    public dialogService: DialogService
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      note: [undefined],
      operationDate: [undefined, [Validators.required]],
      acceptanceDate: [undefined, [Validators.required]],
      smsTicketQuantity: [undefined],
      registerTicketQuantity: [undefined],
      printTicketQuantity: [undefined],
      actualSalesAmount: [undefined],
      commitmentSalesAmount: [undefined],
      updateUserName: [undefined],
      updateTime: [undefined],
    });

    this.formGroup.valueChanges.subscribe((_) => {
      this.validateExtend();
    });
  }

  validateExtend() {
    var operationDate = moment(this.cValue("operationDate")).startOf("day");
    var acceptanceDateControl = this.c("acceptanceDate");
    var acceptanceDate = moment(this.cValue("acceptanceDate"));
    if (acceptanceDate.isBefore(operationDate)) {
      acceptanceDateControl.markAsTouched();
      acceptanceDateControl.setErrors({
        ...acceptanceDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(acceptanceDateControl, "invalidRange");
    }
  }

  async mapToForm() {
    if (this.investment.acceptance) {
      this.model = cloneDeep(this.investment.acceptance);
    } else {
      this.model = new TicketAcceptanceDto({
        acceptanceDate: this.investment.operation?.operationDate,
      });
    }

    var consumerRewards = this.investment.rewardItems
      ? this.investment.rewardItems.map((item) => {
          var consumerReward = this.investment.consumerRewards.find(
            (p) => p.rewardItemId == item.rewardItemId
          );
          return new TicketConsumerRewardDto({
            id: consumerReward ? consumerReward.id : undefined,
            quantity: item.quantity,
            rewardItemId: item.rewardItemId,
            rewardItemCode: item.rewardItemCode,
            rewardItemName: item.rewardItemName,
            photo1: consumerReward ? consumerReward.photo1 : undefined,
            photo2: consumerReward ? consumerReward.photo2 : undefined,
            photo3: consumerReward ? consumerReward.photo3 : undefined,
            photo4: consumerReward ? consumerReward.photo4 : undefined,
            photo5: consumerReward ? consumerReward.photo5 : undefined,
            rewardQuantity: consumerReward ? consumerReward.rewardQuantity : 0,
            details: consumerReward ? consumerReward.details : [],
          });
        })
      : [];

    this.rewardItemDataSource.setData(consumerRewards);

    if (!this.investment.acceptance) {
      this.rewardItemDataSource.transferToNewState();
    }

    this.photos = [ 
        this.model.photo1 != null && this.model.photo1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo1) : this.model.photo1,       
        this.model.photo2 != null && this.model.photo2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo2) : this.model.photo2, 
        this.model.photo3 != null && this.model.photo3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo3) : this.model.photo3, 
        this.model.photo4 != null && this.model.photo4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo4) : this.model.photo4,
        this.model.photo5 != null && this.model.photo5 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo5) : this.model.photo5,
    ];

    this.c("note").setValue(this.model.note);
    this.c("operationDate").setValue(this.investment.operation?.operationDate);
    this.c("acceptanceDate").setValue(this.model.acceptanceDate);
    this.c("updateUserName").setValue(this.model.updateUserName);
    this.c("updateTime").setValue(this.model.lastModificationTime);

    if (this.investment.id) {
      this.getDataService<DataServiceProxy>()
        .getTicketInvestmentSummary(this.investment.id)
        .subscribe((response) => {
          this.summary = response.result;
          this.c("smsTicketQuantity").setValue(
            response.result.smsTicketQuantity
          );
          this.c("printTicketQuantity").setValue(
            response.result.printTicketQuantity
          );
          this.c("registerTicketQuantity").setValue(
            response.result.ticketQuantity
          );
          this.c("actualSalesAmount").setValue(
            response.result.actualSalesAmount
          );
          this.c("commitmentSalesAmount").setValue(
            response.result.commitmentSalesAmount
          );
          this.remarkOfSales = response.result.remarkOfSales;

          this.remarkOfCustomerDevelopement =
            response.result.remarkOfCustomerDevelopement;

          this.remarkOfCompany = response.result.remarkOfCompany;
        });
    }
  }

  async convertImgUrl(url): Promise<string> {
    console.log("Downloading image...");
    var res = await fetch(url);
    var blob = await res.blob();

    const result = await new Promise((resolve, reject) => {
      var reader = new FileReader();
      reader.addEventListener("load", function () {
        resolve(reader.result);
      }, false);

      reader.onerror = () => {
        return reject(this);
      };
      reader.readAsDataURL(blob);
    })

    return result.toString()
  }

  save(complete: boolean) {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    var command = new TicketInvestmentUpsertAcceptanceCommand({
      data: new TicketInvestmentUpsertAcceptanceDto({
        photo1: this.imageViewer.getData()[0],
        photo2: this.imageViewer.getData()[1],
        photo3: this.imageViewer.getData()[2],
        photo4: this.imageViewer.getData()[3],
        photo5: this.imageViewer.getData()[4],
        note: this.cValue("note"),
        acceptanceDate: this.cValue("acceptanceDate"),
      }),
      handleType: complete ? "complete" : "",
    });

    this.dataService
      .acceptTicketInvestment(this._investment.id, command)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.entityHandler.loadRequest("ticket_acceptance", undefined);
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  showRewardDetail(data: TicketConsumerRewardDto): void {
    if (!this.rewardItemEditable) return;
    
    var allocateTickets = [];
    this.investment.consumerRewards
      .filter((p) => p.id != data.id)
      .forEach((items) => allocateTickets.push(...items.details));

    this.dialogService.open(TicketConsumerRewardDetailDialogComponent, {
      data: {
        consumerReward: data,
        ticketInvestmentId: this.investment.id,
        allocateTickets: allocateTickets,
        readOnly: this.readOnly,
        change: (result: TicketConsumerRewardDto) => {
          var item = this.rewardItemDataSource.items.find(
            (p) => p.rewardItemId == result.rewardItemId
          );
          var updateValues = {
            ...item,
            ...result,
          };
          this.rewardItemDataSource.updateItem(updateValues);
        },
      },
      baseZIndex: 1001,
      header: this.l("consumer_reward_detail_title"),
      width: "70%",
    });
  }
  get readOnly() {
    return (
      this.investment?.status != TicketInvestmentStatus.Operated ||
      !(
        this.isGranted("TicketInvestments.Accept") ||
        this.isGranted("TicketInvestments")
      )
    );
  }

  get salesRemarkable() {
    return (
      !this.summary?.remarkOfSales &&
      this.investment?.acceptance?.id &&
      this.isGranted("TicketInvestments.SalesRemark")
    );
  }

  get salesRemarkableButtonVisible() {
    return (
      this.remarkOfSales &&
      !this.summary?.remarkOfSales &&
      this.isGranted("TicketInvestments.SalesRemark")
    );
  }

  get companyRemarkable() {
    return (
      !this.summary?.remarkOfCompany &&
      this.investment?.acceptance?.id &&
      this.isGranted("TicketInvestments.CompanyRemark")
    );
  }

  get companyRemarkableButtonVisible() {
    return (
      this.remarkOfCompany &&
      !this.summary?.remarkOfCompany &&
      this.isGranted("TicketInvestments.CompanyRemark")
    );
  }

  get customerDevelopmentRemarkable() {
    return (
      !this.summary?.remarkOfCustomerDevelopement &&
      this.investment?.acceptance?.id &&
      this.isGranted("TicketInvestments.CustomerDevelopmentRemark")
    );
  }

  get customerDevelopmentRemarkableButtonVisible() {
    return (
      this.remarkOfCustomerDevelopement &&
      !this.summary?.remarkOfCustomerDevelopement &&
      this.isGranted("TicketInvestments.CustomerDevelopmentRemark")
    );
  }

  get rewardItemEditable(): boolean {
    return this.investment?.acceptance?.id != undefined;
  }

  saveRemark(type) {
    var dataService = this.getDataService<DataServiceProxy>();
    this.pageBlockUI.start();
    if (type == "remarkOfSales") {
      dataService
        .salesRemarkTicketInvestment(
          this.investment.id,
          new TicketInvestmentSalesRemarkCommand({
            data: new TicketInvestmentRemarkDto({
              remark: this.remarkOfSales,
            }),
          })
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe(
          (response) => {
            this.summary.remarkOfSales = this.remarkOfSales;
          },
          (error) => this.messageService.toastError(error)
        );
    } else if (type == "remarkOfCompany") {
      dataService
        .companyRemarkTicketInvestment(
          this.investment.id,
          new TicketInvestmentCompanyRemarkCommand({
            data: new TicketInvestmentRemarkDto({
              remark: this.remarkOfCompany,
            }),
          })
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe(
          (response) => {
            this.summary.remarkOfCompany = this.remarkOfCompany;
          },
          (error) => this.messageService.toastError(error)
        );
    } else if (type == "remarkOfCustomerDevelopment") {
      dataService
        .customerDevelopmentRemarkTicketInvestment(
          this.investment.id,
          new TicketInvestmentCustomerDevelopmentRemarkCommand({
            data: new TicketInvestmentRemarkDto({
              remark: this.remarkOfCustomerDevelopement,
            }),
          })
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe(
          (response) => {
            this.summary.remarkOfCustomerDevelopement =
              this.remarkOfCustomerDevelopement;
          },
          (error) => this.messageService.toastError(error)
        );
    }
  }

  clearRemark(type) {
    if (type == "remarkOfSales") {
      this.remarkOfSales = undefined;
    } else if (type == "remarkOfCompany") {
      this.remarkOfSales = undefined;
    } else if (type == "remarkOfCustomerDevelopement") {
      this.remarkOfCustomerDevelopement = undefined;
    }
  }
}
