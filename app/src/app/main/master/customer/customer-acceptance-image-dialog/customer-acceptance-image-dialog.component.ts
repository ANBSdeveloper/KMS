//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  PermissionComponentBase,
} from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

//#endregion

@Component({
  selector: "app-customer-acceptance-image-dialog",
  templateUrl: "./customer-acceptance-image-dialog.component.html",
  styleUrls: ["./customer-acceptance-image-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerAcceptanceImageDialogComponent extends PermissionComponentBase {
  photos = [];
  designPhotos = [];
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("imageViewer") imageViewer: ImageViewerComponent;

  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) {
    super(injector);

  
  }
  ngOnInit() {
    this.pageBlockUI.start();
    this.dataService.getPosmInvestmentItem(this.config.data.posmInvestmentItemId).subscribe((res) => {
      res.result.acceptancePhoto1
      this.photos = [
        res.result.acceptancePhoto1,
        res.result.acceptancePhoto2,
        res.result.acceptancePhoto3,
        res.result.acceptancePhoto4,
      ];
      this.designPhotos = [
        res.result.operationPhoto1,
        res.result.operationPhoto2,
        res.result.operationPhoto3,
        res.result.operationPhoto4,
      ];

      this.pageBlockUI.stop();
    });
  }
}
