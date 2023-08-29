//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { DatatableDataSource, FormComponentBase } from "@cbms/ng-core-vuexy";
import { DynamicDialogRef, DynamicDialogConfig } from "primeng/dynamicdialog";
import { PosmInvestmentItemDto } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import { PosmCalcType } from "@app/main/master/posm-item/data-source/posm-calc-type.data-source";
import { environment } from "environments/environment";
//#endregion
@Component({
  selector: "app-posm-investment-item-dialog",
  templateUrl: "./posm-investment-item-dialog.component.html",
  styleUrls: ["./posm-investment-item-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvesetmentItemDialogComponent extends FormComponentBase {
  @BlockUI("posm_investment_item_dialog_block") formBlockUI: NgBlockUI;
  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;
  readOnly: false;
  posmInvestmentItemDataSource =
    new DatatableDataSource<PosmInvestmentItemDto>();
  posmInvestmentItem: PosmInvestmentItemDto;
  posmInvestmentItemId: number = 0;
  photos = [];
  constructor(
    injector: Injector,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      posmClassId: [undefined, [Validators.required]],
      posmItemId: [undefined, [Validators.required]],
      posmCatalogId: [undefined, [Validators.required]],
      brandId: [undefined, [Validators.required]],
      width: [undefined],
      height: [undefined],
      depth: [undefined],
      sideWidth1: [undefined],
      sideWidth2: [undefined],
      size: [undefined],
      unitPrice: [undefined, [Validators.required]],
      totalCost: [undefined, [Validators.required]],
      qty: [undefined, [Validators.required]],
      setupPlanDate: [undefined, [Validators.required]],
      requestType: [undefined, [Validators.required]],
      requestReason: [undefined, [Validators.required]],
      panelShopName: [undefined, [Validators.required]],
      panelShopPhone: [undefined, [Validators.required]],
      panelOtherInfo: [undefined],
      panelShopAddress: [undefined],
      inclueInfo: [undefined],
      unitType: [undefined, [Validators.required]]
    });
  }

  async ngOnInit() {
    this.posmInvestmentItem = this.config.data.posmInvestmentItem;
    this.posmInvestmentItemId = this.config.data.posmInvestmentItem?.id;
    this.readOnly = this.config.data.readOnly;

    this.photos = [
      this.posmInvestmentItem.photo1.includes("/assets/img_save/") ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.posmInvestmentItem.photo1) : this.posmInvestmentItem.photo1,
      this.posmInvestmentItem.photo2.includes("/assets/img_save/") ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.posmInvestmentItem.photo2) : this.posmInvestmentItem.photo2,
      this.posmInvestmentItem.photo3.includes("/assets/img_save/") ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.posmInvestmentItem.photo3) : this.posmInvestmentItem.photo3,
      this.posmInvestmentItem.photo4.includes("/assets/img_save/") ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.posmInvestmentItem.photo4) : this.posmInvestmentItem.photo4,
    ];

    Object.keys(this.formGroup.controls).forEach((key) => {
      if (this.posmInvestmentItem.hasOwnProperty(key)) {
        this.formGroup.controls[key].setValue(this.posmInvestmentItem[key]);
      }
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

  close() {
    this.ref.close();
  }

  save() {
    // var command = new PosmInvesetmentItemmInvestmentUpsertConsumerRewardCommand({
    //   data: new PosmInvesetmentItemmInvestmentUpsertConsumerRewardDto({
    //     photo1: this.imageViewer.getData()[0],
    //     photo2: this.imageViewer.getData()[1],
    //     photo3: this.imageViewer.getData()[2],
    //     photo4: this.imageViewer.getData()[3],
    //     photo5: this.imageViewer.getData()[4],
    //     rewardItemId: this.consumerReward.rewardItemId,
    //     detailChanges: new ConsumerRewardDetailCrudListDto({
    //       deletedItems: this.posmInvestmentItemDataSource.submitData.deletedItems.map(
    //         (item) =>
    //           new ConsumerRewardDetail({
    //             id: item.id,
    //           })
    //       ),
    //       upsertedItems:
    //         this.posmInvestmentItemDataSource.submitData.upsertedItems.map(
    //           (item) =>
    //             new ConsumerRewardDetail({
    //               id: item.id,
    //               ticketId: item.ticketId,
    //               note: item.note,
    //             })
    //         ),
    //     }),
    //   }),
    // });
    // this.formBlockUI.start();
    // this.getDataService<DataServiceProxy>()
    //   .updatePosmInvesetmentItemmInvestmentConsumerReward(
    //     this.ticketInvestmentId,
    //     this.consumerReward.rewardItemId,
    //     command
    //   )
    //   .pipe(finalize(() => this.formBlockUI.stop()))
    //   .subscribe(
    //     (response) => {
    //       this.messageService.toastSuccess(
    //         this.l("submit_success_message_title"),
    //         this.l("submit_success_message_content")
    //       );
    //       this.config.data.change(response.result);
    //       this.posmInvestmentItemDataSource.setData(response.result.details);
    //     },
    //     (error) => {
    //       this.messageService.toastError(error);
    //     }
    //   );
  }

  deletePosmInvesetmentItemmVisible(row) {
    return !this.readOnly;
  }

  editPosmInvesetmentItemmVisible(row) {
    return !this.readOnly;
  }

  deletePosmInvesetmentItemm(row) {
    this.posmInvestmentItemDataSource.removeRecord(row);
    //this.calcuateTotal();
  }

  editPosmInvesetmentItemm(row) {
    //   this.entityHandler.loadRequest(
    //     this.posmInvestmentItemConfig.entityName,
    //     row["rowId"]
    //   );
    //   this.sidebarService
    //     .getSidebarRegistry(this.posmInvestmentItemConfig.sidebarName)
    //     .open();
  }

  get calcType(): any {
    return this.posmInvestmentItem?.calcType;
  }

  get isWH() {
    return this.calcType == PosmCalcType.WH;
  }

  get isWHD() {
    return this.calcType == PosmCalcType.WHD;
  }
  get isSideVisible() {
    return this.calcType == PosmCalcType.F;
  }
  get isWidthVisible() {
    return (
      this.calcType == PosmCalcType.WH || this.calcType == PosmCalcType.WHD
    );
  }
  get isHeightVisibile() {
    return (
      this.calcType == PosmCalcType.WHD ||
      this.calcType == PosmCalcType.HD ||
      this.calcType == PosmCalcType.WH
    );
  }
  get isDepthVisibile() {
    return this.calcType == PosmCalcType.WHD;
  }
  get isSizeVisible() {
    return this.calcType != PosmCalcType.Q;
  }

  get isPanelInfoVisibility() {
    return this.c("inclueInfo").value;
  }
  //#endregion
}
