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
import { environment } from "environments/environment";

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
    this.dataService.getPosmInvestmentItem(this.config.data.posmInvestmentItemId).subscribe(async (res) => {
      res.result.acceptancePhoto1
      this.photos = [
        res.result.acceptancePhoto1,
        res.result.acceptancePhoto2,
        res.result.acceptancePhoto3,
        res.result.acceptancePhoto4,
      ];

      // this.designPhotos = [
      //   res.result.operationPhoto1,
      //   res.result.operationPhoto2,
      //   res.result.operationPhoto3,
      //   res.result.operationPhoto4,
      // ];
      
      this.designPhotos = [
        res.result.designPhoto1 != null && res.result.designPhoto1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.designPhoto1) : res.result.designPhoto1,       
        res.result.designPhoto2 != null && res.result.designPhoto2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.designPhoto2) : res.result.designPhoto2, 
        res.result.designPhoto3 != null && res.result.designPhoto3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.designPhoto3) : res.result.designPhoto3, 
        res.result.designPhoto4 != null && res.result.designPhoto4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.designPhoto4) : res.result.designPhoto4, 
      ];

      this.pageBlockUI.stop();
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
}
