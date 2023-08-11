//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { ShopComboComponent } from "@app/main/master/customer/shop-combo/shop-combo.component";

import {
  DatatableDataSource,
  DataTableEntityConfig,
  formHelper,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import {
  DataServiceProxy,
  InvestmentCustomerSettingDto,
  StaffDto,
  TicketInvestmentDto,
  TicketInvestmentRegisterDto,
  TicketInvestmentUpdateCommand,
  TicketInvestmentUpdateDto,
  TicketMaterialDto,
  TicketSalesCommitmentDto,
} from "@shared/services/data.service";
import Stepper from "bs-stepper";
import { DxDataGridComponent } from "devextreme-angular/ui/data-grid";
import { formatDate } from "devextreme/localization";
import moment from "moment";
import { finalize, takeUntil } from "rxjs/operators";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import { TicketAcceptanceComponent } from "../ticket-acceptance/ticket-acceptance.component";
import { TicketFinalComponent } from "../ticket-final/ticket-final.component";
import { TicketInvestmentHistoryComponent } from "../ticket-investment-history/ticket-investment-history.component";
import { TicketOperationComponent } from "../ticket-operation/ticket-operation.component";
import { TicketProgressListComponent } from "../ticket-progress-list/ticket-progress-list.component";
//#endregion

@Component({
  selector: "app-ticket-investment-edit",
  templateUrl: "./ticket-investment-edit.component.html",
  styleUrls: ["./ticket-investment-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketInvestmentEditComponent extends PageEditFormComponentBase<
  TicketInvestmentDto,
  TicketInvestmentRegisterDto,
  TicketInvestmentRegisterDto
> {
  @ViewChild("materialDataGrid") materialGrid: DxDataGridComponent;
  @ViewChild("shopCombo") shopCombo: ShopComboComponent;
  @ViewChild("surveyImageViewer") surveyImageViewer: ImageViewerComponent;
  @ViewChild("history") historyComponent: TicketInvestmentHistoryComponent;
  @ViewChild("operation") operationComponent: TicketOperationComponent;
  @ViewChild("acceptance") acceptanceComponent: TicketAcceptanceComponent;
  @ViewChild("final") finalComponent: TicketFinalComponent;
  @ViewChild("progressList") progressListComponent: TicketProgressListComponent;
  entityName = "ticket-investment";
  createUrl = "investment/new-ticket-investment";

  loading = false;
  hasChange = false;
  horizontalWizardStepper: Stepper;
  salesCommitments: TicketSalesCommitmentDto[];
  setting: InvestmentCustomerSettingDto = new InvestmentCustomerSettingDto({
    isPointEditable: false,
    amountPerPoint: 0,
    endIssueDaysBeforeOperation: 2,
    beginIssueDaysAfterCurrent: 7,
  });
  materialDataSource = new DatatableDataSource<TicketMaterialDto>();
  surveyPhotos = [];
  materialConfig = <DataTableEntityConfig>{
    entityName: "ticket_material",
    sidebarName: "ticket_material_sidebar",
  };
  staff: StaffDto;
  constructor(injector: Injector) {
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
      mobilePhone: [undefined],
      address: [undefined],
      code: [undefined],
      efficient: [undefined],
      rewardPackageId: [undefined, [Validators.required]],
      stockQuantity: [undefined, [Validators.required]],
      buyBeginDate: [undefined, [Validators.required]],
      buyEndDate: [undefined, [Validators.required]],
      pointsForTicket: [undefined, [Validators.min(0.01), Validators.required]],
      commitmentAmount: [undefined, [Validators.min(1), Validators.required]],
      ticketQuantity: [undefined, [Validators.required]],
      investmentAmount: [undefined, [Validators.required]],
      rewardAmount: [undefined, [Validators.required]],
      materialAmount: [undefined, [Validators.min(1), Validators.required]],
      salesPlanAmount: [undefined, [Validators.required]],
      issueTicketBeginDate: [undefined, [Validators.required]],
      operationDate: [undefined, [Validators.required]],
      note: [undefined],
    });

    this.formGroup.valueChanges.subscribe((_) => {
      this.validateExtend();
    });

    this.materialDataSource.onUpdate.subscribe((item) => {
      this.calculateMaterialAmount();
      this.calculateInvestmentAmount();
    });

    this.entityHandler
      .registerLoadingRequest("ticket_progress")
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => this.refresh());

    this.entityHandler
      .registerLoadingRequest("ticket_operation")
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => this.refresh());

    this.entityHandler
      .registerLoadingRequest("ticket_acceptance")
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => this.refresh());

    this.entityHandler
      .registerLoadingRequest("ticket_final")
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => this.refresh());
  }

  calculateMaterialAmount() {
    var totalAmount = 0;
    this.materialDataSource.items.forEach((p) => (totalAmount += p.amount));
    this.c("materialAmount").setValue(totalAmount);
  }

  calculateInvestmentAmount() {
    this.c("investmentAmount").setValue(
      this.cValue("materialAmount") + this.cValue("rewardAmount")
    );
  }

  calculateSalesPlanAmount() {
    var salesPlanAmount =
      this.cValue("ticketQuantity") *
      this.cValue("pointsForTicket") *
      this.setting.amountPerPoint;

    this.c("salesPlanAmount").setValue(salesPlanAmount);
  }

  validateExtend() {
    var buyBeginDateControl = this.c("buyBeginDate");
    var buyEndDateControl = this.c("buyEndDate");
    var buyBeginDate = moment(this.cValue("buyBeginDate"));
    var buyEndDate = moment(this.cValue("buyEndDate"));

    var registerDate = !this.model.id ? moment().startOf("day") : buyBeginDate;

    if (buyEndDate.isBefore(buyBeginDate)) {
      buyBeginDateControl.markAsTouched();
      buyBeginDateControl.setErrors({
        ...buyBeginDateControl.errors,
        invalidRange: true,
      });
      buyEndDateControl.markAsTouched();
      buyEndDateControl.setErrors({
        ...buyEndDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(buyBeginDateControl, "invalidRange");
      this.cRemoveError(buyEndDateControl, "invalidRange");
    }

    var issueTicketBeginDateControl = this.c("issueTicketBeginDate");
    var issueTicketBeginDate = moment(this.cValue("issueTicketBeginDate"));

    if (
      !this.model.id &&
      issueTicketBeginDate.isBefore(
        registerDate.add(this.setting.beginIssueDaysAfterCurrent, "day")
      )
    ) {
      issueTicketBeginDateControl.markAsTouched();
      issueTicketBeginDateControl.setErrors({
        ...issueTicketBeginDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(issueTicketBeginDateControl, "invalidRange");
    }

    var operationDateControl = this.c("operationDate");
    var operationDate = moment(this.cValue("operationDate"));

    if (operationDate.isSameOrBefore(issueTicketBeginDate)) {
      operationDateControl.markAsTouched();
      operationDateControl.setErrors({
        ...operationDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(operationDateControl, "invalidRange");
    }

    if (
      operationDate.isValid() &&
      moment(operationDate)
        .startOf("day")
        .isBefore(moment(buyEndDate).startOf("month"))
    ) {
      operationDateControl.markAsTouched();
      operationDateControl.setErrors({
        ...operationDateControl.errors,
        invalidRangeBuy: true,
      });
    } else {
      this.cRemoveError(operationDateControl, "invalidRangeBuy");
    }
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();

    this.horizontalWizardStepper?.to(this.businessStep);

    this.materialDataSource.setData(this.model.materials);
    this.salesCommitments = this.model.salesCommitments;

    this.c("efficient").setValue(this.model?.efficient);
    this.c("address").setValue(this.model?.address);
    this.c("mobilePhone").setValue(this.model?.mobilePhone);

    this.surveyPhotos = [
      this.model.surveyPhoto1,
      this.model.surveyPhoto2,
      this.model.surveyPhoto3,
      this.model.surveyPhoto4,
      this.model.surveyPhoto5,
    ];
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();

    this.saveModel.salesCommitments = this.salesCommitments;
    this.saveModel.materials = this.materialDataSource.submitData.upsertedItems;
    this.saveModel.surveyPhoto1 = this.surveyImageViewer.getData()[0];
    this.saveModel.surveyPhoto2 = this.surveyImageViewer.getData()[1];
    this.saveModel.surveyPhoto3 = this.surveyImageViewer.getData()[2];
    this.saveModel.surveyPhoto4 = this.surveyImageViewer.getData()[3];
    this.saveModel.surveyPhoto5 = this.surveyImageViewer.getData()[4];
  }

  //#region Form & Model
  get newModel() {
    return new TicketInvestmentDto({
      id: undefined,
      status: TicketInvestmentStatus.RequestInvestment,
      buyBeginDate: moment().startOf("day").toDate(),
      buyEndDate: moment().endOf("day").toDate(),
      commitmentAmount: 0,
      stockQuantity: 0,
      investmentAmount: 0,
      materialAmount: 0,
      salesPlanAmount: 0,
      rewardAmount: 0,
      salesCommitments: [],
      materials: [],
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getTicketInvestment(+id);
  }

  // updateRequest(id, data): any {
  //   return this.getDataService<DataServiceProxy>().updateBudget(id, data);
  // }

  createRequest(data): any {
    return this.getDataService<DataServiceProxy>().registerTicketInvestment(
      data
    );
  }

  shopChange(_) {
    if (this.shopCombo.selectedItem) {
      this.c("mobilePhone").setValue(this.shopCombo.selectedItem.mobilePhone);
      this.c("address").setValue(this.shopCombo.selectedItem.address);
      this.c("efficient").setValue(this.shopCombo.selectedItem.efficient);
      this.getDataService<DataServiceProxy>()
        .getInvestmentCustomerSetting(this.c("customerId").value)
        .subscribe((res) => {
          this.setting = res.result;
          this.c("pointsForTicket").setValue(
            this.setting.defaultPointsForTicket
          );
          this.c("issueTicketBeginDate").setValue(
            moment().add(res.result.beginIssueDaysAfterCurrent, "day").toDate()
          );
        });
    } else {
      this.c("mobilePhone").setValue(undefined);
      this.c("address").setValue(undefined);
      this.c("efficient").setValue(undefined);
    }
    this.c("rewardPackageId").setValue(undefined);
  }

  pointsForTicketChange(e) {
    this.calculateSalesPlanAmount();
    this.calculateInvestmentAmount();
  }

  rewardPackageChange(e) {
    if (e.selectedItem) {
      this.c("ticketQuantity").setValue(e.selectedItem.totalTickets);
      this.c("rewardAmount").setValue(e.selectedItem.totalAmount);
    } else {
      this.c("ticketQuantity").setValue(0);
      this.c("rewardAmount").setValue(0);
    }

    this.calculateSalesPlanAmount();
    this.calculateInvestmentAmount();
  }

  showCommitmentSales() {
    this.entityHandler.loadRequest(
      "ticket_investment_sales_commitment",
      undefined
    );
    this.sidebarService
      .getSidebarRegistry("ticket_investment_sales_commitment_sidebar")
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
      this.isGranted("TicketInvestments.Register") &&
      this.model.status == TicketInvestmentStatus.RequestInvestment &&
      !this.modelId &&
      this.cValue("customerId")
    );
  }

  get operationDateSaveVisible(): boolean {
    return (
      this.operationEditable &&
      this.c("operationDate").valid &&
      !moment(this.c("operationDate").value).isSame(
        moment(this.model.operationDate),
        "day"
      )
    );
  }

  get operationEditable(): boolean {
    return (
      this.isGranted("TicketInvestments") &&
      (this.model.status == TicketInvestmentStatus.RequestInvestment ||
        this.model.status ==
          TicketInvestmentStatus.ConfirmedRequestInvestment ||
        this.model.status == TicketInvestmentStatus.ValidRequestInvestment1 ||
        this.model.status == TicketInvestmentStatus.ValidRequestInvestment2 ||
        this.model.status == TicketInvestmentStatus.ConfirmedInvestment ||
        this.model.status == TicketInvestmentStatus.ApproveInvestment ||
        this.model.status == TicketInvestmentStatus.Approved ||
        this.model.status == TicketInvestmentStatus.Updating)
    );
  }

  get customerEditable() {
    return (
      this.isGranted("TicketInvestments.Register") &&
      this.model.status == TicketInvestmentStatus.RequestInvestment &&
      !this.modelId
    );
  }

  get registerButtonVisible(): boolean {
    return this.registerEditable && this.staff != undefined;
  }

  get pointEditable() {
    return this.setting.isPointEditable && this.registerEditable;
  }

  get createMaterialVisible() {
    return this.registerEditable && this.setting.id;
  }

  get businessStep(): number {
    if (!this.modelId) return 1;

    if (
      this.model.status >= TicketInvestmentStatus.RequestInvestment &&
      this.model.status < TicketInvestmentStatus.Approved
    )
      return 2;
    if (this.model.status == TicketInvestmentStatus.Approved) return 3;
    if (
      this.model.status == TicketInvestmentStatus.Updating &&
      !this.model.operation
    )
      return 3;
    if (
      this.model.status == TicketInvestmentStatus.Updating &&
      this.model.operation
    )
      return 4;
    if (this.model.status == TicketInvestmentStatus.Operated) return 5;
    if (this.model.status == TicketInvestmentStatus.Acceptance) return 6;
    return 1;
  }

  get saveVisible() {
    return this.registerEditable;
  }

  deleteMaterialVisible(row: TicketMaterialDto) {
    return this.registerEditable && this.setting.id;
  }

  editMaterialVisible(row: TicketMaterialDto) {
    return this.registerEditable && this.setting.id;
  }

  deleteMaterial(row: TicketMaterialDto) {
    this.materialDataSource.removeRecord(row);
  }

  editMaterial(row: TicketMaterialDto) {
    this.openEditItemDataTable(this.materialConfig, row);
  }

  createMaterial() {
    this.openNewItemDataTable(this.materialConfig);
  }

  saveOperationDate() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .updateTicketInvestment(
        new TicketInvestmentUpdateCommand({
          data: new TicketInvestmentUpdateDto({
            id: this.model.id,
            operationDate: this.c("operationDate").value,
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
  register() {
    this.submit();
  }
  approve() {
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .approveTicketInvestment(this.modelId)
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
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .denyTicketInvestment(this.modelId)
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

  refresh() {
    super.refresh();
    this.historyComponent.refresh();
    this.progressListComponent.refesh();
  }

  get issueTicketBeginDateError() {
    var validDate = moment()
      .add(this.setting.beginIssueDaysAfterCurrent, "day")
      .toDate();
    return this.l(
      "invalid_issue_ticket_begin_date",
      formatDate(validDate, "shortDate")
    );
  }

  get requestTitle() {
    if (!this.model.id) {
      return this.l("register");
    } else {
      if (
        this.model.status ==
          TicketInvestmentStatus.ConfirmedRequestInvestment ||
        this.model.status == TicketInvestmentStatus.ValidRequestInvestment1 ||
        this.model.status == TicketInvestmentStatus.ValidRequestInvestment2 ||
        this.model.status == TicketInvestmentStatus.ConfirmedInvestment ||
        this.model.status == TicketInvestmentStatus.ApproveInvestment
      ) {
        return this.l("wait_approve");
      } else if (
        this.model.status == TicketInvestmentStatus.DeniedRequestInvestment ||
        this.model.status == TicketInvestmentStatus.InValidRequestInvestment1 ||
        this.model.status == TicketInvestmentStatus.InValidRequestInvestment2 ||
        this.model.status ==
          TicketInvestmentStatus.DeniedInvestmentConfirmation ||
        this.model.status == TicketInvestmentStatus.DeniedInvestmentApproval ||
        this.model.status == TicketInvestmentStatus.Denied
      ) {
        return this.l("denied");
      }
      {
        return this.l("approved");
      }
    }
  }

  get approveButtonVisible(): boolean {
    return (
      !this.approveStepDisabled &&
      ((this.model.status == TicketInvestmentStatus.RequestInvestment &&
        this.isGranted("TicketInvestments.ApproveRequest")) ||
        (this.model.status ==
          TicketInvestmentStatus.ConfirmedRequestInvestment &&
          this.isGranted("TicketInvestments.ConfirmValid1")) ||
        (this.model.status == TicketInvestmentStatus.ValidRequestInvestment1 &&
          this.isGranted("TicketInvestments.ConfirmValid2")) ||
        (this.model.status == TicketInvestmentStatus.ValidRequestInvestment2 &&
          this.isGranted("TicketInvestments.ConfirmInvestment")) ||
        (this.model.status == TicketInvestmentStatus.ConfirmedInvestment &&
          this.isGranted("TicketInvestments.ApproveInvestment1")) ||
        (this.model.status == TicketInvestmentStatus.ApproveInvestment &&
          this.isGranted("TicketInvestments.ApproveInvestment2")))
    );
  }

  isGranted(perm: string) {
    return super.isGranted(this.permissionName) || super.isGranted(perm);
  }

  get denyButtonVisible(): boolean {
    return (
      !this.approveStepDisabled &&
      ((this.model.status == TicketInvestmentStatus.RequestInvestment &&
        this.isGranted("TicketInvestments.DenyRequest")) ||
        (this.model.status ==
          TicketInvestmentStatus.ConfirmedRequestInvestment &&
          this.isGranted("TicketInvestments.DenyValid1")) ||
        (this.model.status == TicketInvestmentStatus.ValidRequestInvestment1 &&
          this.isGranted("TicketInvestments.DenyValid2")) ||
        (this.model.status == TicketInvestmentStatus.ValidRequestInvestment2 &&
          this.isGranted("TicketInvestments.DenyInvestmentConfirmation")) ||
        (this.model.status == TicketInvestmentStatus.ConfirmedInvestment &&
          this.isGranted("TicketInvestments.DenyInvestment1")) ||
        (this.model.status == TicketInvestmentStatus.ApproveInvestment &&
          this.isGranted("TicketInvestments.DenyInvestment2")))
    );
  }

  get addUpdateProgressButtonVisible(): boolean {
    return (
      !this.approveStepDisabled &&
      (this.model.status == TicketInvestmentStatus.Approved ||
        this.model.status == TicketInvestmentStatus.Updating) &&
      this.isGranted("TicketInvestments.UpdateProgress") &&
      this.model.progresses.find((p) => p.id == undefined) == undefined &&
      !this.progressListComponent.isNew &&
      this.currentStep == 3
    );
  }
  get saveUpdateProgressButtonVisible(): boolean {
    return (
      !this.approveStepDisabled &&
      (this.model.status == TicketInvestmentStatus.Approved ||
        this.model.status == TicketInvestmentStatus.Updating) &&
      this.isGranted("TicketInvestments.UpdateProgress") &&
      this.progressListComponent.isNew &&
      this.currentStep == 3
    );
  }

  get updateOperationButtonVisible(): boolean {
    return (
      this.model.status == TicketInvestmentStatus.Updating &&
      this.isGranted("TicketInvestments.Operate") &&
      this.currentStep == 4
    );
  }

  get completeAcceptanceButtonVisible(): boolean {
    return (
      this.model.acceptance &&
      this.model.status == TicketInvestmentStatus.Operated &&
      this.isGranted("TicketInvestments.Accept") &&
      this.currentStep == 5
    );
  }

  get updateAcceptanceButtonVisible(): boolean {
    return (
      this.model.status == TicketInvestmentStatus.Operated &&
      this.isGranted("TicketInvestments.Accept") &&
      this.currentStep == 5
    );
  }

  get updateFinalButtonVisible(): boolean {
    return (
      this.model.status == TicketInvestmentStatus.Acceptance &&
      this.isGranted("TicketInvestments.FinalSettlement") &&
      this.currentStep == 6
    );
  }

  get approveStepDisabled(): boolean {
    return !this.model.id;
  }

  get progressStepDisabled(): boolean {
    return (
      !this.model.id || this.model.status < TicketInvestmentStatus.Approved
    );
  }

  get operationStepDisabled(): boolean {
    return (
      !this.model.id || this.model.status < TicketInvestmentStatus.Updating
    );
  }

  get acceptanceStepDisabled(): boolean {
    return (
      !this.model.id || this.model.status < TicketInvestmentStatus.Operated
    );
  }

  get finalStepDisabled(): boolean {
    return (
      !this.model.id || this.model.status < TicketInvestmentStatus.Acceptance
    );
  }

  registerStepClick() {
    //fix layout
    setTimeout(() => {
      this.materialGrid.instance.refresh();
    }, 200);
  }

  addUpdateProgress() {
    this.horizontalWizardStepper.to(3);
    this.progressListComponent.create();
  }

  saveUpdateProgress() {
    this.progressListComponent.save();
  }

  updateOperation() {
    this.operationComponent.save(false);
  }

  completeOperation() {
    this.operationComponent.save(true);
  }

  updateAcceptance() {
    this.acceptanceComponent.save(false);
  }

  completeAcceptance() {
    this.acceptanceComponent.save(true);
  }

  updateFinal() {
    this.finalComponent.save(false);
  }

  completeFinal() {
    this.finalComponent.save(true);
  }

  viewZoneCalendar() {
    let newRelativeUrl = this.router.createUrlTree([
      "investment/ticket-investment-calendar",
    ]);
    let baseUrl = window.location.href.replace(this.router.url, "");
    window.open(baseUrl + newRelativeUrl, "_blank");
  }
  get currentStep() {
    return +document
      .querySelector(".step.active")
      .getAttribute("data-stepIndex");
  }
}
