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
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import { PosmInvestmentItemStatusDataSource } from "../../data-source/posm-investment-item-status.data-source";
import { PosmCalcType } from "@app/main/master/posm-item/data-source/posm-calc-type.data-source";
import { formatNumber } from "devextreme/localization";
import { PosmInvestmentImageDetailComponent } from "../posm-investment-image-detail/posm-investment-image-detail.component";

//#endregion

@Component({
  selector: "app-posm-investment-operation",
  templateUrl: "./posm-investment-operation.component.html",
  styleUrls: ["./posm-investment-operation.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentOperationComponent extends FormComponentBase {
  photos = [];
  status = [
    PosmInvestmentItemStatus.ConfirmedProduce1,
    PosmInvestmentItemStatus.ConfirmedProduce2,
    PosmInvestmentItemStatus.ConfirmedVendorProduce
  ];
  posmItemDataSource = new DatatableDataSource<PosmInvestmentItemDto>();
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("posmItemDataGrid") posmItemGrid: DxDataGridComponent;
  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;
  @ViewChild("posmInvestmentImageDetail")
  posmInvestmentImageDetail: PosmInvestmentImageDetailComponent;
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
      operationNote: [undefined],
      operationDate: [moment().toDate(), [Validators.required]],
      operationLink: [undefined, [Validators.required]],
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

  get editable() {
    return (
      <any>this.item?.status == PosmInvestmentItemStatus.ValidOrder &&
      (this.isGranted("PosmInvestments.MarketingConfirmProduce") ||
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
      this.photos = [
        this.item.operationPhoto1,
        this.item.operationPhoto2,
        this.item.operationPhoto3,
        this.item.operationPhoto4,
      ];

      if (!this.c("operationDate").value) {
        this.c("operationDate").setValue(moment().toDate());
      }
    } else {
      this.item = null;
      this.formGroup.reset();
      this.c("operationDate").setValue(moment().toDate());
      this.photos = [undefined, undefined, undefined, undefined];
    }
  }

  get detailVisible() {
    return this.item;
  }

  calculateSpecification = (rowData) => {
    if (rowData.calcType == PosmCalcType.WH) {
      return `${this.l('width')} ${formatNumber(rowData.width, '#,###.##')} x ${this.l('height')} ${formatNumber(rowData.height, '#,###.##')} = ${formatNumber(rowData.size, '#,###.##')}`;
    }
    if (rowData.calcType == PosmCalcType.WHD) {
      return `${this.l('width')} ${formatNumber(rowData.width, '#,###.##')} x ${this.l('height')} ${formatNumber(rowData.height, '#,###.##')} x ${this.l('depth')} ${formatNumber(rowData.depth, '#,###.##')} = ${formatNumber(rowData.size, '#,###.##')}`;
    }
    if (rowData.calcType == PosmCalcType.HD) {
      return `${this.l('height')} ${formatNumber(rowData.height, '#,###.##')} x ${this.l('depth')} ${formatNumber(rowData.depth, '#,###.##')} = ${formatNumber(rowData.size, '#,###.##')}`;
    }
    if (rowData.calcType == PosmCalcType.F) {
      return `(${this.l('side_width_1')} ${formatNumber(rowData.sideWidth1, '#,###.##')} + ${this.l('side_width_2')} ${formatNumber(rowData.sideWidth2, '#,###.##')}) x ${this.l('width_face')} ${formatNumber(rowData.depth, '#,###.##')} = ${formatNumber(rowData.size, '#,###.##')}`;
    }
    return '';
  }
}
