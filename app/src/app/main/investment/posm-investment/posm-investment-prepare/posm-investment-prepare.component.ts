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
import moment from "moment";
import { Validators } from "@angular/forms";
import { DxDataGridComponent } from "devextreme-angular/ui/data-grid";
import { PosmInvestmentItemStatus } from "../../data-source/posm-investment-status.enum";
import { PosmInvestmentItemStatusDataSource } from "../../data-source/posm-investment-item-status.data-source";
import { formatNumber } from "devextreme/localization";
import { PosmCalcType } from "@app/main/master/posm-item/data-source/posm-calc-type.data-source";
import { DxNumberBoxComponent } from "devextreme-angular";

//#endregion

@Component({
  selector: "app-posm-investment-prepare",
  templateUrl: "./posm-investment-prepare.component.html",
  styleUrls: ["./posm-investment-prepare.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentPrepareComponent extends FormComponentBase {
  photos = [];
  status = [
    PosmInvestmentItemStatus.ValidOrder,
    PosmInvestmentItemStatus.SupSuggestedUpdateCost,
    PosmInvestmentItemStatus.AsmConfirmedUpdateCost,
    PosmInvestmentItemStatus.RsmConfirmedUpdateCost,
    PosmInvestmentItemStatus.TradeConfirmedUpdateCost,
    PosmInvestmentItemStatus.InvalidOrder,
  ];
  posmItemDataSource = new DatatableDataSource<PosmInvestmentItemDto>();
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("posmItemDataGrid") posmItemGrid: DxDataGridComponent;
  @ViewChild("actualTotalCost") actualTotalCost: DxNumberBoxComponent;
  @Input() set investment(value: PosmInvestmentDto) {
    this._investment = value;
    this.mapToForm();
  }

  get investment() {
    return this._investment;
  }

  _investment: PosmInvestmentDto;
  model: PosmInvestmentDto;
  public item: PosmInvestmentItemDto;
  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public entityHandler: EntityHandlerService,
    public posmUnitTypeDataSource: PosmUnitTypeDataSource,
    public posmInvestmentItemStatusDataSource: PosmInvestmentItemStatusDataSource
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      prepareNote: [undefined],
      prepareDate: [moment().toDate(), [Validators.required]],
      unitPrice: [undefined, [Validators.required]],
      totalCost: [undefined, [Validators.required]],
      actualUnitPrice: [undefined, [Validators.required]],
      actualTotalCost: [undefined, [Validators.required]],
      vendorId: [undefined, [Validators.required]],
    });
  }

  mapToForm() {
    this.model = cloneDeep(this.investment);
    if (this.model?.items) {
      this.posmItemDataSource.setData(this.model.items);
      this.posmItemGrid?.instance.refresh();
    }
  }
  clearSelection() {
    this.posmItemGrid.instance.clearSelection();
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
      <any>this.item?.status ==
        (PosmInvestmentItemStatus.DirectorApprovedRequest ||
          PosmInvestmentItemStatus.TradeConfirmedUpdateCost) &&
      (this.isGranted("PosmInvestments.SupplyConfirmRequest") ||
        this.isGranted("PosmInvestments.SupplyDenyRequest") ||
        this.isGranted("PosmInvestments"))
    );
  }

  selectionChanged(e) {
    if (e.selectedRowsData?.length > 0) {
      this.item = e.selectedRowsData[0];
      Object.keys(this.formGroup.controls).forEach((key) => {
        if (e.selectedRowsData[0].hasOwnProperty(key)) {
          this.formGroup.controls[key].setValue(e.selectedRowsData[0][key]);
        }
      });

      if (!this.c("prepareDate").value) {
        this.c("prepareDate").setValue(moment().toDate());
      }
    } else {
      this.item = null;
      this.formGroup.reset();
      this.c("prepareDate").setValue(moment().toDate());
    }
  }

  setValidOrder() {
    if (!this.c("actualTotalCost").value) {
      this.c("actualTotalCost").setValue(this.c("totalCost").value);
      this.c("actualUnitPrice").setValue(
        this.c("totalCost").value / (this.item.qty * this.item.posmValue)
      );
    }
  }

  isValidConfirm() {
    return (
      this.c("actualTotalCost").value <= this.c("totalCost").value * 1.01 &&
      this.c("actualTotalCost").value >= this.c("totalCost").value * 0.99
    );
  }

  hasActualTotalCost() {
    return !this.c("actualTotalCost").value;
  }
  actualTotalCostValueChanged(e) {
    if (this.item) {
      this.c("actualUnitPrice").setValue(
        e.value === undefined
          ? undefined
          : e.value / (this.item.qty * this.item.posmValue)
      );
    }
  }
  get detailVisible() {
    return this.item;
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
