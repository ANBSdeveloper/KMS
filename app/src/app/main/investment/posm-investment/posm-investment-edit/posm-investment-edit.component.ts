//#region Import
import {
  ChangeDetectorRef,
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { ShopComboComponent } from "@app/main/master/customer/shop-combo/shop-combo.component";
import {
  PosmCalcType,
  PosmCalcTypeDataSource,
} from "@app/main/master/posm-item/data-source/posm-calc-type.data-source";
import { PosmUnitTypeDataSource } from "@app/main/master/posm-item/data-source/posm-unit-type.data-source";
import { formatNumber } from "devextreme/localization";
import {
  DatatableDataSource,
  formHelper,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import {
  DataServiceProxy,
  InvestmentCustomerSettingDto,
  StaffDto,
  PosmInvestmentDto,
  PosmInvestmentRegisterDto,
  PosmInvestmentItemDto,
  PosmSalesCommitmentDto,
  PosmInvestmentAsmApproveCommand,
  PosmInvestmentAsmApproveDto,
  PosmInvestmentRsmApproveCommand,
  PosmInvestmentRsmApproveDto,
  PosmInvestmentTradeApproveCommand,
  PosmInvestmentTradeApproveDto,
  PosmInvestmentDirectorApproveCommand,
  PosmInvestmentDirectorApproveDto,
  PosmInvestmentAsmDenyCommand,
  PosmInvestmentAsmDenyDto,
  PosmInvestmentDirectorDenyCommand,
  PosmInvestmentDirectorDenyDto,
  PosmInvestmentRsmDenyCommand,
  PosmInvestmentRsmDenyDto,
  PosmInvestmentTradeDenyCommand,
  PosmInvestmentTradeDenyDto,
  PosmInvestmentSupplyConfirmRequestCommand,
  PosmInvestmentSupplyConfirmRequestDto,
  PosmInvestmentSupplyDenyRequestCommand,
  PosmInvestmentSupplyDenyRequestDto,
  PosmInvestmentSupSuggestCommand,
  PosmInvestmentSupSuggesDto,
  PosmInvestmentAsmConfirmSuggestCommand,
  PosmInvestmentAsmConfirmSuggesDto,
  PosmInvestmentRsmConfirmSuggestCommand,
  PosmInvestmentRsmConfirmSuggesDto,
  PosmInvestmentTradeConfirmSuggestCommand,
  PosmInvestmentTradeConfirmSuggesDto,
  PosmInvestmentMarketingConfirmProduceCommand,
  PosmInvestmentMarketingConfirmProduceDto,
  PosmInvestmentSupConfirmProduceCommand,
  PosmInvestmentSupConfirmProduceDto,
  PosmInvestmentSupplyConfirmProduceCommand,
  PosmInvestmentSupplyConfirmProduceDto,
  PosmInvestmentSupAcceptCommand,
  PosmInvestmentSupAcceptDto,
  PosmInvestmentAsmConfirmAcceptCommand,
  PosmInvestmentAsmConfirmAcceptDto,
  PosmInvestmentTradeConfirmAcceptCommand,
  PosmInvestmentTradeConfirmAcceptDto,
} from "@shared/services/data.service";
import Stepper from "bs-stepper";
import { DxDataGridComponent } from "devextreme-angular/ui/data-grid";
import moment from "moment";
import { DialogService } from "primeng/dynamicdialog";
import { finalize } from "rxjs/operators";
import {
  PosmInvestmentItemStatus,
  PosmInvestmentStatus,
} from "../../data-source/posm-investment-status.enum";
import { PosmInvestmentAcceptanceComponent } from "../posm-investment-acceptance/posm-investment-acceptance.component";
import { PosmInvestmentApproveComponent } from "../posm-investment-approve/posm-investment-approve.component";
import { PosmInvesetmentItemDialogComponent } from "../posm-investment-item-dialog/posm-investment-item-dialog.component";
import { PosmInvestmentOperationComponent } from "../posm-investment-operation/posm-investment-operation.component";
import { PosmInvestmentPrepareComponent } from "../posm-investment-prepare/posm-investment-prepare.component";
import { CustomerDialogComponent } from "@app/main/master/customer/customer-dialog/customer-dialog.component";
//#endregion

@Component({
  selector: "app-posm-investment-edit",
  templateUrl: "./posm-investment-edit.component.html",
  styleUrls: ["./posm-investment-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentEditComponent extends PageEditFormComponentBase<
  PosmInvestmentDto,
  PosmInvestmentRegisterDto,
  PosmInvestmentRegisterDto
> {
  @ViewChild("posmInvestmentApprove")
  posmInvestmentApprove: PosmInvestmentApproveComponent;
  @ViewChild("posmInvestmentPrepare")
  posmInvestmentPrepare: PosmInvestmentPrepareComponent;
  @ViewChild("posmInvestmentOperation")
  posmInvestmentOperation: PosmInvestmentOperationComponent;
  @ViewChild("posmInvestmentAcceptance")
  posmInvestmentAcceptance: PosmInvestmentAcceptanceComponent;
  @ViewChild("posmItemDataGrid") posmItemGrid: DxDataGridComponent;
  @ViewChild("shopCombo") shopCombo: ShopComboComponent;
  @ViewChild("shopPanelImageViewer") shopPanelImageViewer: ImageViewerComponent;
  @ViewChild("visibilityImageViewer")
  visibilityImageViewer: ImageViewerComponent;
  @ViewChild("visibilityCompetitorImageViewer")
  visibilityCompetitorImageViewer: ImageViewerComponent;
  entityName = "posm-investment";
  createUrl = "investment/new-posm-investment";

  loading = false;
  hasChange = false;
  horizontalWizardStepper: Stepper;
  salesCommitments: PosmSalesCommitmentDto[];
  setting: InvestmentCustomerSettingDto = new InvestmentCustomerSettingDto({
    isPointEditable: false,
    amountPerPoint: 0,
    endIssueDaysBeforeOperation: 2,
    beginIssueDaysAfterCurrent: 7,
  });
  posmItemDataSource = new DatatableDataSource<PosmInvestmentItemDto>();
  shopPanelPhotos = [];
  visibilityPhotos = [];
  visibilityCompetitorPhotos = [];
  staff: StaffDto;
  constructor(
    injector: Injector,
    public posmUnitTypeDataSource: PosmUnitTypeDataSource,
    public dialogService: DialogService,
    public cdr: ChangeDetectorRef
  ) {
    super(injector);

    this.getDataService<DataServiceProxy>()
      .getStaffInfo()
      .subscribe((response) => {
        this.staff = response.result;
      });
  }

  ngOnInit() {
    super.ngOnInit();
    this.horizontalWizardStepper = new Stepper(
      document.querySelector("#stepper1"),
      { linear: false, animation: true }
    );
  }

  modernHorizontalNext() {
    this.horizontalWizardStepper.next();
  }
  /**
   * Modern Horizontal Wizard Stepper Previous
   */
  modernHorizontalPrevious() {
    this.horizontalWizardStepper.previous();
  }

  configForm(): void {
    this.formGroup = this.fb.group({
      customerId: [undefined, [Validators.required]],
      customerCode: [undefined],
      customerLocationId: [undefined, [Validators.required]],
      mobilePhone: [undefined],
      address: [undefined],
      code: [undefined],
      setupContact1: [undefined],
      setupContact2: [undefined],
      efficient: [undefined],
      buyBeginDate: [undefined, [Validators.required]],
      buyEndDate: [undefined, [Validators.required]],
      commitmentAmount: [undefined, [Validators.min(1), Validators.required]],
      investmentAmount: [undefined, [Validators.required]],
      currentSalesAmount: [undefined, [Validators.required]],
      note: [undefined],
      registerDate: [undefined, [Validators.required]],
    });

    // this.formGroup.valueChanges.subscribe((_) => {
    //   this.validateExtend();
    // });

    this.posmItemDataSource.onUpdate.subscribe((item) => {
      this.calculateInvestmentAmount();
    });

    // this.entityHandler
    //   .registerLoadingRequest("posm_progress")
    //   .pipe(takeUntil(this.unsubscribe$))
    //   .subscribe(() => this.refresh());

    // this.entityHandler
    //   .registerLoadingRequest("posm_operation")
    //   .pipe(takeUntil(this.unsubscribe$))
    //   .subscribe(() => this.refresh());

    // this.entityHandler
    //   .registerLoadingRequest("posm_acceptance")
    //   .pipe(takeUntil(this.unsubscribe$))
    //   .subscribe(() => this.refresh());

    // this.entityHandler
    //   .registerLoadingRequest("posm_final")
    //   .pipe(takeUntil(this.unsubscribe$))
    //   .subscribe(() => this.refresh());
  }

  calculateInvestmentAmount() {
    var investmentAmount = 0;
    this.posmItemDataSource.items.forEach(
      (p) => (investmentAmount += p.totalCost)
    );
    this.c("investmentAmount").setValue(investmentAmount);
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();

    this.horizontalWizardStepper?.to(this.businessStep);

    this.posmItemDataSource.setData(this.model.items);
    this.salesCommitments = this.model.salesCommitments;

    // this.c("efficient").setValue(this.model?.efficient);
    // this.c("address").setValue(this.model?.address);
    // this.c("mobilePhone").setValue(this.model?.mobilePhone);
    if (this.model) {
      this.c("buyBeginDate").setValue(
        moment(this.model.creationTime)
          .add("month", 1)
          .startOf("month")
          .toDate()
      );
      this.c("buyEndDate").setValue(
        moment(this.model.creationTime).add("month", 3).endOf("month").toDate()
      );
    } else {
      this.c("buyBeginDate").setValue(
        moment().add("month", 1).startOf("month").toDate()
      );
      this.c("buyEndDate").setValue(
        moment().add("month", 3).endOf("month").toDate()
      );
    }

    this.shopPanelPhotos = [
      this.model.shopPanelPhoto1,
      this.model.shopPanelPhoto2,
      this.model.shopPanelPhoto3,
      this.model.shopPanelPhoto4,
    ];

    this.visibilityPhotos = [
      this.model.visibilityPhoto1,
      this.model.visibilityPhoto2,
      this.model.visibilityPhoto3,
      this.model.visibilityPhoto4,
    ];

    this.visibilityCompetitorPhotos = [
      this.model.visibilityCompetitorPhoto1,
      this.model.visibilityCompetitorPhoto2,
      this.model.visibilityCompetitorPhoto3,
      this.model.visibilityCompetitorPhoto4,
    ];
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();

    this.saveModel.salesCommitments = this.salesCommitments;
    this.saveModel.items = this.posmItemDataSource.submitData.upsertedItems;
    this.saveModel.shopPanelPhoto1 = this.shopPanelImageViewer.getData()[0];
    this.saveModel.shopPanelPhoto2 = this.shopPanelImageViewer.getData()[1];
    this.saveModel.shopPanelPhoto3 = this.shopPanelImageViewer.getData()[2];
    this.saveModel.shopPanelPhoto4 = this.shopPanelImageViewer.getData()[3];

    this.saveModel.visibilityPhoto1 = this.visibilityImageViewer.getData()[0];
    this.saveModel.visibilityPhoto2 = this.visibilityImageViewer.getData()[1];
    this.saveModel.visibilityPhoto3 = this.visibilityImageViewer.getData()[2];
    this.saveModel.visibilityPhoto4 = this.visibilityImageViewer.getData()[3];

    this.saveModel.visibilityCompetitorPhoto1 =
      this.visibilityCompetitorImageViewer.getData()[0];
    this.saveModel.visibilityCompetitorPhoto2 =
      this.visibilityCompetitorImageViewer.getData()[1];
    this.saveModel.visibilityCompetitorPhoto3 =
      this.visibilityCompetitorImageViewer.getData()[2];
    this.saveModel.visibilityCompetitorPhoto4 =
      this.visibilityCompetitorImageViewer.getData()[3];
  }

  //#region Form & Model
  get newModel() {
    return new PosmInvestmentDto({
      id: undefined,
      status: PosmInvestmentStatus.Request,
      commitmentAmount: 0,
      investmentAmount: 0,
      currentSalesAmount: 0,
      salesCommitments: [],
      items: [],
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getPosmInvestment(+id);
  }

  // updateRequest(id, data): any {
  //   return this.getDataService<DataServiceProxy>().updateBudget(id, data);
  // }

  createRequest(data): any {
    return this.getDataService<DataServiceProxy>().registerPosmInvestment(data);
  }

  shopChange(_) {
    if (this.shopCombo.selectedItem) {
      this.c("mobilePhone").setValue(this.shopCombo.selectedItem.mobilePhone);
      this.c("address").setValue(this.shopCombo.selectedItem.address);
      this.c("efficient").setValue(this.shopCombo.selectedItem.efficient);
    } else {
      this.c("mobilePhone").setValue(undefined);
      this.c("address").setValue(undefined);
      this.c("efficient").setValue(undefined);
    }
  }

  showCommitmentSales() {
    this.entityHandler.loadRequest(
      "posm_investment_sales_commitment",
      undefined
    );
    this.sidebarService
      .getSidebarRegistry("posm_investment_sales_commitment_sidebar")
      .toggleOpen();
  }

  updateSalesCommitments(e) {
    this.salesCommitments = e;
    var commitmentAmount = 0;
    this.salesCommitments.forEach((item) => (commitmentAmount += item.amount));
    this.c("commitmentAmount").setValue(commitmentAmount);
  }

  get registerEditable(): boolean {
    return (
      this.isGranted("PosmInvestments.Register") &&
      this.model?.status == PosmInvestmentStatus.Request &&
      !this.modelId &&
      this.cValue("customerId")
    );
  }

  get customerEditable() {
    return (
      this.isGranted("PosmInvestments.Register") &&
      this.model.status == PosmInvestmentStatus.Request &&
      !this.modelId
    );
  }

  get registerButtonVisible(): boolean {
    return this.registerEditable && this.staff != undefined;
  }

  get createPosmItemVisible() {
    return this.registerEditable;
  }

  get businessStep(): number {
    if (!this.modelId) return 1;

    if (
      this.model.status >= PosmInvestmentStatus.Request &&
      this.model.status < PosmInvestmentStatus.DirectorApprovedRequest
    )
      return 2;
    if (
      this.model.status >= PosmInvestmentStatus.DirectorApprovedRequest &&
      this.model.status < PosmInvestmentStatus.ValidOrder
    )
      return 3;
    if (
      this.model.status >= PosmInvestmentStatus.ValidOrder &&
      this.model.status < PosmInvestmentStatus.ConfirmedVendorProduce
    )
      return 4;
    // if (
    //   this.model.status == PosmInvestmentStatus.Updating &&
    //   !this.model.operation
    // )
    //   return 3;
    // if (
    //   this.model.status == PosmInvestmentStatus.Updating &&
    //   this.model.operation
    // )
    //   return 4;
    // if (this.model.status == PosmInvestmentStatus.Operated) return 5;
    // if (this.model.status == PosmInvestmentStatus.Acceptance) return 6;
    return 1;
  }

  get saveVisible() {
    return this.registerEditable;
  }

  deletePosmItemVisible(row: PosmInvestmentItemDto) {
    return (
      this.isGranted("PosmInvestments.Register") &&
      this.model?.status == PosmInvestmentStatus.Request
    );
  }

  editPosmItemVisible(row: PosmInvestmentItemDto) {
    return (
      this.isGranted("PosmInvestments.Register") &&
      this.model?.status == PosmInvestmentStatus.Request
    );
  }

  deletePosmItem(row: PosmInvestmentItemDto) {
    this.posmItemDataSource.removeRecord(row);
  }

  editPosmItem(row: PosmInvestmentItemDto) {
    this.dialogService.open(PosmInvesetmentItemDialogComponent, {
      data: {
        posmInvestment: this.model,
        posmInvestmentItem: row,
        readOnly: this.readOnly,
        change: (result) => {
          // var item = this.rewardItemDataSource.items.find(
          //   (p) => p.rewardItemId == result.rewardItemId
          // );
          // var updateValues = {
          //   ...item,
          //   ...result,
          // };
          // this.rewardItemDataSource.updateItem(updateValues);
        },
      },
      baseZIndex: 1001,
      header: this.l("posm_investment_item_title"),
      width: "70%",
    });
  }

  createPosmItem() {}

  // saveOperationDate() {
  //   if (!this.formGroup.valid) {
  //     formHelper.validateAllFormFields(this.formGroup);
  //     return false;
  //   }
  //   this.pageBlockUI.start();
  //   this.getDataService<DataServiceProxy>()
  //     .updatePosmInvestment(
  //       new PosmInvestmentUpdateCommand({
  //         data: new PosmInvestmentUpdateDto({
  //           id: this.model.id,
  //           operationDate: this.c("operationDate").value,
  //         }),
  //       })
  //     )
  //     .pipe(finalize(() => this.pageBlockUI.stop()))
  //     .subscribe(
  //       (_) => {
  //         this.refresh();
  //       },
  //       (error) => {
  //         this.messageService.toastError(error);
  //       }
  //     );
  // }
  register() {
    this.submit();
  }
  approve() {
    var request;
    var note = this.posmInvestmentApprove.c("approveNote").value;
    if (this.model.status == PosmInvestmentStatus.Request) {
      request =
        this.getDataService<DataServiceProxy>().asmApprovePosmInvestment(
          this.modelId,
          new PosmInvestmentAsmApproveCommand({
            data: new PosmInvestmentAsmApproveDto({ note: note }),
          })
        );
    } else if (this.model.status == PosmInvestmentStatus.AsmApprovedRequest) {
      request =
        this.getDataService<DataServiceProxy>().rsmApprovePosmInvestment(
          this.modelId,
          new PosmInvestmentRsmApproveCommand({
            data: new PosmInvestmentRsmApproveDto({ note: note }),
          })
        );
    } else if (this.model.status == PosmInvestmentStatus.RsmApprovedRequest) {
      request =
        this.getDataService<DataServiceProxy>().tradeApprovePosmInvestment(
          this.modelId,
          new PosmInvestmentTradeApproveCommand({
            data: new PosmInvestmentTradeApproveDto({ note: note }),
          })
        );
    } else if (this.model.status == PosmInvestmentStatus.TradeApprovedRequest) {
      request =
        this.getDataService<DataServiceProxy>().directorApprovePosmInvestment(
          this.modelId,
          new PosmInvestmentDirectorApproveCommand({
            data: new PosmInvestmentDirectorApproveDto({ note: note }),
          })
        );
    }

    if (request) {
      this.pageBlockUI.start();
      request.pipe(finalize(() => this.pageBlockUI.stop())).subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
    }
  }

  confirmProduce1() {
    if (!this.posmInvestmentOperation.formGroup.valid) {
      formHelper.validateAllFormFields(this.posmInvestmentOperation.formGroup);
      return false;
    }
    if (this.posmInvestmentOperation.imageViewer.getData().every((p) => !p)) {
      this.messageService.toastError(this.l("design_photo_required"));
      return false;
    }
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .marketingConfirmProduce(
        this.modelId,
        new PosmInvestmentMarketingConfirmProduceCommand({
          data: new PosmInvestmentMarketingConfirmProduceDto({
            link: this.posmInvestmentOperation.c("operationLink").value,
            note: this.posmInvestmentOperation.c("operationNote").value,
            photo1: this.posmInvestmentOperation.imageViewer.getData()[0],
            photo2: this.posmInvestmentOperation.imageViewer.getData()[1],
            photo3: this.posmInvestmentOperation.imageViewer.getData()[2],
            photo4: this.posmInvestmentOperation.imageViewer.getData()[3],
            posmInvestmentItemId: this.posmInvestmentOperation.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  confirmProduce2() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .supConfirmProduce(
        this.modelId,
        new PosmInvestmentSupConfirmProduceCommand({
          data: new PosmInvestmentSupConfirmProduceDto({
            posmInvestmentItemId: this.posmInvestmentOperation.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  confirmVendorProduce() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .supplyConfirmProduce(
        this.modelId,
        new PosmInvestmentSupplyConfirmProduceCommand({
          data: new PosmInvestmentSupplyConfirmProduceDto({
            posmInvestmentItemId: this.posmInvestmentOperation.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  confirmValid() {
    let item = <any>this.posmInvestmentPrepare?.item;
    if(      this.posmInvestmentPrepare.hasActualTotalCost()) {
      if (
        item.status == PosmInvestmentItemStatus.DirectorApprovedRequest &&
        !this.posmInvestmentPrepare.isValidConfirm()
      ) {
        this.messageService.toastError(
          this.l("actual_cost_must_less_than_total_cost")
        );
        return;
      }
    } else {
      this.posmInvestmentPrepare.setValidOrder();
    }
    
    
    if (!this.posmInvestmentPrepare.formGroup.valid) {
      formHelper.validateAllFormFields(this.posmInvestmentPrepare.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .supplyApprovePosmInvestment(
        this.modelId,
        new PosmInvestmentSupplyConfirmRequestCommand({
          data: new PosmInvestmentSupplyConfirmRequestDto({
            actualUnitPrice:
              this.posmInvestmentPrepare.c("actualUnitPrice").value,
            id: this.posmInvestmentPrepare.investment.id,
            note: this.posmInvestmentPrepare.c("prepareNote").value,
            vendorId: this.posmInvestmentPrepare.c("vendorId").value,
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  confirmInvalid() {
    if (!this.posmInvestmentPrepare.formGroup.valid) {
      formHelper.validateAllFormFields(this.posmInvestmentPrepare.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .supplyDenyPosmInvestment(
        this.modelId,
        new PosmInvestmentSupplyDenyRequestCommand({
          data: new PosmInvestmentSupplyDenyRequestDto({
            actualUnitPrice:
              this.posmInvestmentPrepare.c("actualUnitPrice").value,
            id: this.posmInvestmentPrepare.investment.id,
            note: this.posmInvestmentPrepare.c("prepareNote").value,
            vendorId: this.posmInvestmentPrepare.c("vendorId").value,
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  accept() {
    if (!this.posmInvestmentAcceptance.formGroup.valid) {
      formHelper.validateAllFormFields(this.posmInvestmentAcceptance.formGroup);
      return false;
    }
    if (this.posmInvestmentAcceptance.imageViewer.getData().every((p) => !p)) {
      this.messageService.toastError(this.l("acceptance_photo_required"));
      return false;
    }
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .accept(
        this.modelId,
        new PosmInvestmentSupAcceptCommand({
          data: new PosmInvestmentSupAcceptDto({
            photo1: this.posmInvestmentAcceptance.imageViewer.getData()[0],
            photo2: this.posmInvestmentAcceptance.imageViewer.getData()[1],
            photo3: this.posmInvestmentAcceptance.imageViewer.getData()[2],
            photo4: this.posmInvestmentAcceptance.imageViewer.getData()[3],
            note: this.posmInvestmentAcceptance.c("acceptanceNote").value,
            posmInvestmentItemId: this.posmInvestmentAcceptance.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  confirmAccept1() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .confirmAccept1(
        this.modelId,
        new PosmInvestmentAsmConfirmAcceptCommand({
          data: new PosmInvestmentAsmConfirmAcceptDto({
            posmInvestmentItemId: this.posmInvestmentAcceptance.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  confirmAccept2() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .confirmAccept2(
        this.modelId,
        new PosmInvestmentTradeConfirmAcceptCommand({
          data: new PosmInvestmentTradeConfirmAcceptDto({
            posmInvestmentItemId: this.posmInvestmentAcceptance.item.id,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  deny() {
    var request;
    var note = this.posmInvestmentApprove.c("approveNote").value;
    if (this.model.status == PosmInvestmentStatus.Request) {
      request = this.getDataService<DataServiceProxy>().asmDenyPosmInvestment(
        this.modelId,
        new PosmInvestmentAsmDenyCommand({
          data: new PosmInvestmentAsmDenyDto({ note: note }),
        })
      );
    } else if (this.model.status == PosmInvestmentStatus.AsmApprovedRequest) {
      request = this.getDataService<DataServiceProxy>().rsmDenyPosmInvestment(
        this.modelId,
        new PosmInvestmentRsmDenyCommand({
          data: new PosmInvestmentRsmDenyDto({ note: note }),
        })
      );
    } else if (this.model.status == PosmInvestmentStatus.RsmApprovedRequest) {
      request = this.getDataService<DataServiceProxy>().tradeDenyPosmInvestment(
        this.modelId,
        new PosmInvestmentTradeDenyCommand({
          data: new PosmInvestmentTradeDenyDto({ note: note }),
        })
      );
    } else if (this.model.status == PosmInvestmentStatus.TradeApprovedRequest) {
      request =
        this.getDataService<DataServiceProxy>().directorDenyPosmInvestment(
          this.modelId,
          new PosmInvestmentDirectorDenyCommand({
            data: new PosmInvestmentDirectorDenyDto({ note: note }),
          })
        );
    }

    if (request) {
      this.pageBlockUI.start();
      request.pipe(finalize(() => this.pageBlockUI.stop())).subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
    }
  }

  confirmSuggest() {
    var request;
    if (
      <any>this.posmInvestmentPrepare?.item.status ==
      PosmInvestmentItemStatus.SupSuggestedUpdateCost
    ) {
      request = this.getDataService<DataServiceProxy>().asmConfirmSuggest(
        this.modelId,
        new PosmInvestmentAsmConfirmSuggestCommand({
          data: new PosmInvestmentAsmConfirmSuggesDto({
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
          }),
        })
      );
    } else if (
      <any>this.posmInvestmentPrepare?.item.status ==
      PosmInvestmentItemStatus.AsmConfirmedUpdateCost
    ) {
      request = this.getDataService<DataServiceProxy>().rsmConfirmSuggest(
        this.modelId,
        new PosmInvestmentRsmConfirmSuggestCommand({
          data: new PosmInvestmentRsmConfirmSuggesDto({
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
          }),
        })
      );
    } else if (
      <any>this.posmInvestmentPrepare?.item.status ==
      PosmInvestmentItemStatus.RsmConfirmedUpdateCost
    ) {
      request = this.getDataService<DataServiceProxy>().tradeConfirmSuggest(
        this.modelId,
        new PosmInvestmentTradeConfirmSuggestCommand({
          data: new PosmInvestmentTradeConfirmSuggesDto({
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
          }),
        })
      );
    }

    if (request) {
      this.pageBlockUI.start();
      request.pipe(finalize(() => this.pageBlockUI.stop())).subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
    }
  }

  suggestBudget() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .suggestBudget(
        this.modelId,
        new PosmInvestmentSupSuggestCommand({
          data: new PosmInvestmentSupSuggesDto({
            posmInvestmentItemId: this.posmInvestmentPrepare.item.id,
            reason: "",
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  // deny() {
  //   this.pageBlockUI.start();
  //   this.getDataService<DataServiceProxy>()
  //     .denyPosmInvestment(this.modelId)
  //     .pipe(finalize(() => this.pageBlockUI.stop()))
  //     .subscribe(
  //       (_) => {
  //         this.refresh();
  //       },
  //       (error) => {
  //         this.messageService.toastError(error);
  //       }
  //     );
  // }

  refresh() {
    super.refresh();
    this.posmInvestmentPrepare?.clearSelection();
    this.posmInvestmentOperation?.clearSelection();
    this.posmInvestmentAcceptance?.clearSelection();
    // this.historyComponent.refresh();
    // this.progressListComponent.refesh();
  }

  // get requestTitle() {
  //   if (!this.model.id) {
  //     return this.l("register");
  //   } else {
  //     if (
  //       this.model.status ==
  //         PosmInvestmentStatus.ConfirmedRequestInvestment ||
  //       this.model.status == PosmInvestmentStatus.ValidRequestInvestment1 ||
  //       this.model.status == PosmInvestmentStatus.ValidRequestInvestment2 ||
  //       this.model.status == PosmInvestmentStatus.ConfirmedInvestment ||
  //       this.model.status == PosmInvestmentStatus.ApproveInvestment
  //     ) {
  //       return this.l("wait_approve");
  //     } else if (
  //       this.model.status == PosmInvestmentStatus.DeniedRequestInvestment ||
  //       this.model.status == PosmInvestmentStatus.InValidRequestInvestment1 ||
  //       this.model.status == PosmInvestmentStatus.InValidRequestInvestment2 ||
  //       this.model.status ==
  //         PosmInvestmentStatus.DeniedInvestmentConfirmation ||
  //       this.model.status == PosmInvestmentStatus.DeniedInvestmentApproval ||
  //       this.model.status == PosmInvestmentStatus.Denied
  //     ) {
  //       return this.l("denied");
  //     }
  //     {
  //       return this.l("approved");
  //     }
  //   }
  // }

  get approveButtonVisible(): boolean {
    return (
      (!this.approveStepDisabled &&
        this.model?.status == PosmInvestmentStatus.Request &&
        (this.isGranted("PosmInvestments.AsmApprove") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.AsmApprovedRequest &&
        (this.isGranted("PosmInvestments.RsmApprove") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.RsmApprovedRequest &&
        (this.isGranted("PosmInvestments.TradeApprove") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.TradeApprovedRequest &&
        (this.isGranted("PosmInvestments.DirectorApprove") ||
          this.isGranted("PosmInvestments")))
    );
  }

  get suggestButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 3 &&
      <any>this.posmInvestmentPrepare?.item?.status ==
        PosmInvestmentItemStatus.InvalidOrder &&
      (this.isGranted("PosmInvestments.SuggestBudget") ||
        this.isGranted("PosmInvestments"))
    );
  }
  get confirmSuggestButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 3 &&
      ((<any>this.posmInvestmentPrepare?.item?.status ==
        PosmInvestmentItemStatus.SupSuggestedUpdateCost &&
        (this.isGranted("PosmInvestments.AsmConfirmSuggest") ||
          this.isGranted("PosmInvestments"))) ||
        (<any>this.posmInvestmentPrepare?.item?.status ==
          PosmInvestmentItemStatus.AsmConfirmedUpdateCost &&
          (this.isGranted("PosmInvestments.RsmConfirmSuggest") ||
            this.isGranted("PosmInvestments"))) ||
        (<any>this.posmInvestmentPrepare?.item?.status ==
          PosmInvestmentItemStatus.RsmConfirmedUpdateCost &&
          (this.isGranted("PosmInvestments.TradeConfirmSuggest") ||
            this.isGranted("PosmInvestments"))))
    );
  }

  get confirmProduce1ButtonVisible(): boolean {
    return (
      !this.operationStepDisabled &&
      this.currentStep == 4 &&
      this.posmInvestmentOperation?.item &&
      <any>this.posmInvestmentOperation?.item.status ==
        PosmInvestmentItemStatus.ValidOrder &&
      (this.isGranted("PosmInvestments.MarketingConfirmProduce") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmProduce2ButtonVisible(): boolean {
    return (
      !this.operationStepDisabled &&
      this.currentStep == 4 &&
      this.posmInvestmentOperation?.item &&
      <any>this.posmInvestmentOperation?.item.status ==
        PosmInvestmentItemStatus.ConfirmedProduce1 &&
      (this.isGranted("PosmInvestments.SupConfirmProduce") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmVendorProduceButtonVisible(): boolean {
    return (
      !this.operationStepDisabled &&
      this.currentStep == 4 &&
      this.posmInvestmentOperation?.item &&
      <any>this.posmInvestmentOperation?.item.status ==
        PosmInvestmentItemStatus.ConfirmedProduce2 &&
      (this.isGranted("PosmInvestments.SupplyConfirmProduce") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmValidButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 3 &&
      this.posmInvestmentPrepare?.item &&
      (<any>this.posmInvestmentPrepare?.item.status ==
        PosmInvestmentItemStatus.DirectorApprovedRequest ||
        <any>this.posmInvestmentPrepare?.item.status ==
          PosmInvestmentItemStatus.TradeConfirmedUpdateCost) &&
      (this.isGranted("PosmInvestments.SupplyConfirmRequest") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get acceptButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 5 &&
      this.posmInvestmentAcceptance?.item &&
      <any>this.posmInvestmentAcceptance?.item.status ==
        PosmInvestmentItemStatus.ConfirmedVendorProduce &&
      (this.isGranted("PosmInvestments.Accept") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmAccept1ButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 5 &&
      this.posmInvestmentAcceptance?.item &&
      <any>this.posmInvestmentAcceptance?.item.status ==
        PosmInvestmentItemStatus.Accepted &&
      (this.isGranted("PosmInvestments.ConfirmAccept1") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmAccept2ButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 5 &&
      this.posmInvestmentAcceptance?.item &&
      <any>this.posmInvestmentAcceptance?.item.status ==
        PosmInvestmentItemStatus.ConfirmedAccept1 &&
      (this.isGranted("PosmInvestments.ConfirmAccept2") ||
        this.isGranted("PosmInvestments"))
    );
  }

  get confirmInvalidButtonVisible(): boolean {
    return (
      !this.prepareStepDisabled &&
      this.currentStep == 3 &&
      this.posmInvestmentPrepare?.item &&
      (<any>this.posmInvestmentPrepare?.item.status ==
        PosmInvestmentItemStatus.DirectorApprovedRequest ||
        <any>this.posmInvestmentPrepare?.item.status ==
          PosmInvestmentItemStatus.TradeConfirmedUpdateCost) &&
      (this.isGranted("PosmInvestments.SupplyDenyRequest") ||
        this.isGranted("PosmInvestments"))
    );
  }
  get denyButtonVisible(): boolean {
    return (
      (!this.approveStepDisabled &&
        this.model?.status == PosmInvestmentStatus.Request &&
        (this.isGranted("PosmInvestments.AsmDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.AsmApprovedRequest &&
        (this.isGranted("PosmInvestments.RsmDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.RsmApprovedRequest &&
        (this.isGranted("PosmInvestments.TradeDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this.model?.status == PosmInvestmentStatus.TradeApprovedRequest &&
        (this.isGranted("PosmInvestments.DirectorDeny") ||
          this.isGranted("PosmInvestments")))
    );
  }

  isGranted(perm: string) {
    return super.isGranted(this.permissionName) || super.isGranted(perm);
  }

  // get denyButtonVisible(): boolean {
  //   return (
  //     !this.approveStepDisabled &&
  //     ((this.model.status == PosmInvestmentStatus.RequestInvestment &&
  //       this.isGranted("PosmInvestments.DenyRequest")) ||
  //       (this.model.status ==
  //         PosmInvestmentStatus.ConfirmedRequestInvestment &&
  //         this.isGranted("PosmInvestments.DenyValid1")) ||
  //       (this.model.status == PosmInvestmentStatus.ValidRequestInvestment1 &&
  //         this.isGranted("PosmInvestments.DenyValid2")) ||
  //       (this.model.status == PosmInvestmentStatus.ValidRequestInvestment2 &&
  //         this.isGranted("PosmInvestments.DenyInvestmentConfirmation")) ||
  //       (this.model.status == PosmInvestmentStatus.ConfirmedInvestment &&
  //         this.isGranted("PosmInvestments.DenyInvestment1")) ||
  //       (this.model.status == PosmInvestmentStatus.ApproveInvestment &&
  //         this.isGranted("PosmInvestments.DenyInvestment2")))
  //   );
  // }

  // get addUpdateProgressButtonVisible(): boolean {
  //   return (
  //     !this.approveStepDisabled &&
  //     (this.model.status == PosmInvestmentStatus.Approved ||
  //       this.model.status == PosmInvestmentStatus.Updating) &&
  //     this.isGranted("PosmInvestments.UpdateProgress") &&
  //     this.model.progresses.find((p) => p.id == undefined) == undefined &&
  //     !this.progressListComponent.isNew &&
  //     this.currentStep == 3
  //   );
  // }

  // get saveUpdateProgressButtonVisible(): boolean {
  //   return (
  //     !this.approveStepDisabled &&
  //     (this.model.status == PosmInvestmentStatus.Approved ||
  //       this.model.status == PosmInvestmentStatus.Updating) &&
  //     this.isGranted("PosmInvestments.UpdateProgress") &&
  //     this.progressListComponent.isNew &&
  //     this.currentStep == 3
  //   );
  // }

  // get updateOperationButtonVisible(): boolean {
  //   return (
  //     this.model.status == PosmInvestmentStatus.Updating &&
  //     this.isGranted("PosmInvestments.Operate") &&
  //     this.currentStep == 4
  //   );
  // }

  // get completeAcceptanceButtonVisible(): boolean {
  //   return (
  //     this.model.acceptance &&
  //     this.model.status == PosmInvestmentStatus.Operated &&
  //     this.isGranted("PosmInvestments.Accept") &&
  //     this.currentStep == 5
  //   );
  // }

  // get updateAcceptanceButtonVisible(): boolean {
  //   return (
  //     this.model.status == PosmInvestmentStatus.Operated &&
  //     this.isGranted("PosmInvestments.Accept") &&
  //     this.currentStep == 5
  //   );
  // }

  // get updateFinalButtonVisible(): boolean {
  //   return (
  //     this.model.status == PosmInvestmentStatus.Acceptance &&
  //     this.isGranted("PosmInvestments.FinalSettlement") &&
  //     this.currentStep == 6
  //   );
  // }

  get approveStepDisabled(): boolean {
    return !this.model.id;
  }

  get prepareStepDisabled(): boolean {
    return (
      this.model?.items?.find(
        (p) => p.status >= PosmInvestmentItemStatus.DirectorApprovedRequest
      ) == null
    );
  }

  get operationStepDisabled(): boolean {
    return (
      this.model?.items?.find(
        (p) => p.status >= PosmInvestmentItemStatus.ValidOrder
      ) == null
    );
  }

  get acceptanceStepDisabled(): boolean {
    return (
      this.model?.items?.find(
        (p) => p.status >= PosmInvestmentItemStatus.ConfirmedVendorProduce
      ) == null
    );
  }

  registerStepClick() {
    //fix layout
    setTimeout(() => {
      this.posmItemGrid.instance.refresh();
    }, 200);
  }
  approveStepClick() {
    //fix layout
    setTimeout(() => {
      this.posmInvestmentApprove.posmItemGrid.instance.refresh();
    }, 200);
  }
  prepareStepClick() {
    //fix layout
    setTimeout(() => {
      this.posmInvestmentPrepare.posmItemGrid.instance.refresh();
    }, 200);
  }

  operationStepClick() {
    //fix layout
    setTimeout(() => {
      this.posmInvestmentOperation.posmItemGrid.instance.refresh();
    }, 200);
  }

  acceptanceStepClick() {
    //fix layout
    setTimeout(() => {
      this.posmInvestmentAcceptance.posmItemGrid.instance.refresh();
    }, 200);
  }

  addUpdateProgress() {
    this.horizontalWizardStepper.to(3);
  }

  get currentStep() {
    return +document
      .querySelector(".step.active")
      .getAttribute("data-stepIndex");
  }

  get registerItemCount(): number {
    return this.model?.items?.length;
  }

  get approveItemCount(): number {
    return this.model?.items?.filter(
      (p) =>
        p.status >= PosmInvestmentItemStatus.Request &&
        p.status < PosmInvestmentItemStatus.DirectorApprovedRequest
    ).length;
  }

  get prepareItemCount(): number {
    return this.model?.items?.filter(
      (p) =>
        p.status >= PosmInvestmentItemStatus.DirectorApprovedRequest &&
        p.status < PosmInvestmentItemStatus.ValidOrder
    ).length;
  }

  get operationItemCount(): number {
    return this.model?.items?.filter(
      (p) =>
        p.status >= PosmInvestmentItemStatus.ValidOrder &&
        p.status < PosmInvestmentItemStatus.Accepted
    ).length;
  }

  get acceptanceItemCount(): number {
    return this.model?.items?.filter(
      (p) => p.status >= PosmInvestmentItemStatus.Accepted
    ).length;
  }

  calculateSpecification = (rowData) => {
    if (rowData.calcType == PosmCalcType.WH) {
      return `${this.l("width")} ${formatNumber(
        rowData.width,
        "#,###.##"
      )} x ${this.l("height")} ${formatNumber(
        rowData.height,
        "#,###.##"
      )} = ${formatNumber(rowData.size, "#,###.##")}`;
    }
    if (rowData.calcType == PosmCalcType.WHD) {
      return `${this.l("width")} ${formatNumber(
        rowData.width,
        "#,###.##"
      )} x ${this.l("height")} ${formatNumber(
        rowData.height,
        "#,###.##"
      )} x ${this.l("depth")} ${formatNumber(
        rowData.depth,
        "#,###.##"
      )} = ${formatNumber(rowData.size, "#,###.##")}`;
    }
    if (rowData.calcType == PosmCalcType.HD) {
      return `${this.l("height")} ${formatNumber(
        rowData.height,
        "#,###.##"
      )} x ${this.l("depth")} ${formatNumber(
        rowData.depth,
        "#,###.##"
      )} = ${formatNumber(rowData.size, "#,###.##")}`;
    }
    if (rowData.calcType == PosmCalcType.F) {
      return `(${this.l("side_width_1")} ${formatNumber(
        rowData.sideWidth1,
        "#,###.##"
      )} + ${this.l("side_width_2")} ${formatNumber(
        rowData.sideWidth2,
        "#,###.##"
      )}) x ${this.l("width_face")} ${formatNumber(
        rowData.depth,
        "#,###.##"
      )} = ${formatNumber(rowData.size, "#,###.##")}`;
    }
    return "";
  };

  showCustomerDetail(_) {
    this.dialogService.open(CustomerDialogComponent, {
      data: {
        id: this.model.customerId,
      },
      baseZIndex: 1001,
      header: this.l("customer_information"),
      width: "70%",
    });
  }
}
