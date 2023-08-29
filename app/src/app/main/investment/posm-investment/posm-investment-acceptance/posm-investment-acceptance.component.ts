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
  PosmInvestmentCompanyRemarkCommand,
  PosmInvestmentDto,
  PosmInvestmentItemDto,
  PosmInvestmentRemarkDto,
  PosmInvestmentSalesRemarkCommand,
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
import { finalize } from "rxjs/operators";
import { environment } from "environments/environment";

//#endregion

@Component({
  selector: "app-posm-investment-acceptance",
  templateUrl: "./posm-investment-acceptance.component.html",
  styleUrls: ["./posm-investment-acceptance.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentAcceptanceComponent extends FormComponentBase {
  photos = [];
  designPhotos = [];
  status = [
    PosmInvestmentItemStatus.Accepted,
    PosmInvestmentItemStatus.ConfirmedAccept1,
    PosmInvestmentItemStatus.ConfirmedAccept2,
  ];
  remarkOfSales;
  remarkOfCompany;
  posmItemDataSource = new DatatableDataSource<PosmInvestmentItemDto>();
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("posmItemDataGrid") posmItemGrid: DxDataGridComponent;
  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;
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
      acceptanceNote: [undefined],
      acceptanceDate: [moment().toDate(), [Validators.required]],
    });
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

  async mapToForm() {
    this.model = cloneDeep(this.investment);
    if (this.model?.items) {

      this.designPhotos = [
        this.model.designPhoto1 != null && this.model.designPhoto1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.designPhoto1) : this.model.designPhoto1,       
        this.model.designPhoto2 != null && this.model.designPhoto2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.designPhoto2) : this.model.designPhoto2, 
        this.model.designPhoto3 != null && this.model.designPhoto3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.designPhoto3) : this.model.designPhoto3, 
        this.model.designPhoto4 != null && this.model.designPhoto4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.designPhoto4) : this.model.designPhoto4, 
      ];

      this.posmItemDataSource.setData(this.model.items);
      this.posmItemGrid?.instance.refresh();
    }
  }
  clearSelection() {
    this.posmItemGrid.instance.clearSelection();
  }
  saveRemark(type) {
    var dataService = this.getDataService<DataServiceProxy>();
    this.pageBlockUI.start();
    if (type == "remarkOfSales") {
      dataService
        .salesRemarkPosmInvestment(
          this.investment.id,
          new PosmInvestmentSalesRemarkCommand({
            data: new PosmInvestmentRemarkDto({
              remark: this.remarkOfSales,
              posmInvestmentItemId: this.item.id,
            }),
          })
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe(
          (response) => {
            this.item.remarkOfSales = this.remarkOfSales;
          },
          (error) => this.messageService.toastError(error)
        );
    } else if (type == "remarkOfCompany") {
      dataService
        .companyRemarkPosmInvestment(
          this.investment.id,
          new PosmInvestmentCompanyRemarkCommand({
            data: new PosmInvestmentRemarkDto({
              remark: this.remarkOfCompany,
              posmInvestmentItemId: this.item.id,
            }),
          })
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe(
          (response) => {
            this.item.remarkOfCompany = this.remarkOfCompany;
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
    }
  }
  get editable() {
    return (
      <any>this.item?.status ==
        PosmInvestmentItemStatus.ConfirmedVendorProduce &&
      (this.isGranted("PosmInvestments.Accept") ||
        this.isGranted("PosmInvestments"))
    );
  }

  async selectionChanged(e) {
    if (e.selectedRowsData?.length > 0) {
      this.item = e.selectedRowsData[0];
      Object.keys(this.formGroup.controls).forEach((key) => {
        if (e.selectedRowsData[0].hasOwnProperty(key)) {
          this.formGroup.controls[key].setValue(e.selectedRowsData[0][key]);
        }
      });
      this.photos = [        
        this.item.acceptancePhoto1 != null && this.item.acceptancePhoto1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.item.acceptancePhoto1) : this.item.acceptancePhoto1,       
        this.item.acceptancePhoto2 != null && this.item.acceptancePhoto2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.item.acceptancePhoto2) : this.item.acceptancePhoto2, 
        this.item.acceptancePhoto3 != null && this.item.acceptancePhoto3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.item.acceptancePhoto3) : this.item.acceptancePhoto3, 
        this.item.acceptancePhoto4 != null && this.item.acceptancePhoto4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.item.acceptancePhoto4) : this.item.acceptancePhoto4,
      ];

      // this.designPhotos = [
      //   this.item.operationPhoto1,
      //   this.item.operationPhoto2,
      //   this.item.operationPhoto3,
      //   this.item.operationPhoto4,
      // ]

      if (!this.c("acceptanceDate").value) {
        this.c("acceptanceDate").setValue(moment().toDate());
      }

      this.remarkOfSales = this.item.remarkOfSales;

      this.remarkOfCompany = this.item.remarkOfCompany;
    } else {
      this.item = null;
      this.formGroup.reset();
      this.c("acceptanceDate").setValue(moment().toDate());
      this.photos = [undefined, undefined, undefined, undefined];
      this.designPhotos = [undefined, undefined, undefined, undefined];
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

  get salesRemarkable() {
    return (
      !this.item?.remarkOfSales && this.isGranted("PosmInvestments.SalesRemark")
    );
  }

  get salesRemarkableButtonVisible() {
    return (
      !this.item?.remarkOfSales && this.isGranted("PosmInvestments.SalesRemark")
    );
  }

  get companyRemarkable() {
    return (
      !this.item?.remarkOfCompany &&
      this.isGranted("PosmInvestments.CompanyRemark")
    );
  }

  get companyRemarkableButtonVisible() {
    return (
      !this.item?.remarkOfCompany &&
      this.isGranted("PosmInvestments.CompanyRemark")
    );
  }
}
