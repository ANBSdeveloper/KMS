//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { cloneDeep } from "lodash";
import {
  DatatableDataSource,
  EntityHandlerService,
  FormComponentBase,
} from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  PosmInvestmentDto,
  PosmInvestmentItemDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { PosmUnitTypeDataSource } from "@app/main/master/posm-item/data-source/posm-unit-type.data-source";
import {
  PosmInvestmentItemStatus,
  PosmInvestmentStatus,
} from "../../data-source/posm-investment-status.enum";
import { DxDataGridComponent } from "devextreme-angular/ui/data-grid";
import { PosmInvestmentItemStatusDataSource } from "../../data-source/posm-investment-item-status.data-source";
import { formatNumber } from "devextreme/localization";
import { PosmCalcType } from "@app/main/master/posm-item/data-source/posm-calc-type.data-source";

//#endregion

@Component({
  selector: "app-posm-investment-approve",
  templateUrl: "./posm-investment-approve.component.html",
  styleUrls: ["./posm-investment-approve.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentApproveComponent extends FormComponentBase {
  photos = [];
  posmItemDataSource = new DatatableDataSource<PosmInvestmentItemDto>();
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("posmItemDataGrid") posmItemGrid: DxDataGridComponent;
  @Input() set investment(value: PosmInvestmentDto) {
    this._investment = value;
    this.mapToForm();
  }
  status = [
    PosmInvestmentItemStatus.Request,
    PosmInvestmentItemStatus.AsmApprovedRequest,
    PosmInvestmentItemStatus.AsmDeniedRequest,
    PosmInvestmentItemStatus.RsmApprovedRequest,
    PosmInvestmentItemStatus.RsmDeniedRequest,
    PosmInvestmentItemStatus.TradeApprovedRequest,
    PosmInvestmentItemStatus.TradeDeniedRequest,
    PosmInvestmentItemStatus.DirectorApprovedRequest,
    PosmInvestmentItemStatus.DirectorDeniedRequest,
  ];
  get investment() {
    return this._investment;
  }

  _investment: PosmInvestmentDto;
  model: PosmInvestmentDto;
  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public entityHandler: EntityHandlerService,
    public posmUnitTypeDataSource: PosmUnitTypeDataSource,
    public posmInvestmentItemStatusDataSource: PosmInvestmentItemStatusDataSource
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      approveNote: [undefined],
    });
  }

  mapToForm() {
    this.model = cloneDeep(this.investment);
    if (this.model?.items) {
      this.posmItemDataSource.setData(this.model.items);
      this.posmItemGrid?.instance.refresh();
    }

    this.c("approveNote").setValue(this.model.approveNote);
  }

  save(complete: boolean) {
    // if (!this.formGroup.valid) {
    //   formHelper.validateAllFormFields(this.formGroup);
    //   return false;
    // }
    // this.pageBlockUI.start();
    // var command = new TicketInvestmentUpsertAcceptanceCommand({
    //   data: new TicketInvestmentUpsertAcceptanceDto({
    //     photo1: this.imageViewer.getData()[0],
    //     photo2: this.imageViewer.getData()[1],
    //     photo3: this.imageViewer.getData()[2],
    //     photo4: this.imageViewer.getData()[3],
    //     photo5: this.imageViewer.getData()[4],
    //     note: this.cValue("note"),
    //     acceptanceDate: this.cValue("acceptanceDate"),
    //   }),
    //   handleType: complete ? "complete" : "",
    // });
    // this.dataService
    //   .acceptTicketInvestment(this._investment.id, command)
    //   .pipe(finalize(() => this.pageBlockUI.stop()))
    //   .subscribe(
    //     (_) => {
    //       this.entityHandler.loadRequest("ticket_acceptance", undefined);
    //     },
    //     (error) => {
    //       this.messageService.toastError(error);
    //     }
    //   );
  }

  get editable() {
    return (
      (this._investment?.status == PosmInvestmentStatus.Request &&
        (this.isGranted("PosmInvestments.AsmApprove") ||
          this.isGranted("PosmInvestments.AsmDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this._investment?.status == PosmInvestmentStatus.AsmApprovedRequest &&
        (this.isGranted("PosmInvestments.RsmApprove") ||
          this.isGranted("PosmInvestments.RsmDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this._investment?.status == PosmInvestmentStatus.RsmApprovedRequest &&
        (this.isGranted("PosmInvestments.TradeApprove") ||
          this.isGranted("PosmInvestments.TradeDeny") ||
          this.isGranted("PosmInvestments"))) ||
      (this._investment?.status == PosmInvestmentStatus.TradeApprovedRequest &&
        (this.isGranted("PosmInvestments.DirectorApprove") ||
          this.isGranted("PosmInvestments.DirectorDeny") ||
          this.isGranted("PosmInvestments")))
    );
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
}
