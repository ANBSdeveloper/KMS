import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Injector,
  Input,
  NgModule,
  Output,
  ViewChildren,
} from "@angular/core";
import { FormComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy, PosmInvestmentItemsUpdateCommand } from "@shared/services/data.service";
import { environment } from "environments/environment";
import { Ng2ImgMaxService } from "ng2-img-max";

@Component({
  selector: "app-posm-investment-image-detail",
  templateUrl: "./posm-investment-image-detail.component.html",
  styleUrls: ["./posm-investment-image-detail.component.scss"],
})
export class PosmInvestmentImageDetailComponent extends FormComponentBase {
  @ViewChildren("imageInput") imageInputs: ElementRef[];
  @Input() posmInvestmentItemId: number | undefined;
  @Input() readOnly = false;
  @Input() header = "";
  @Input() size: number = 5;
  @Input() set dataImages(value: string[]) {
    this._dataImages = value;

    if (this._dataImages) {
      for (var i = 0; i < this.size; i++) {
        this.placeholderImages[i] = { data: this._dataImages[i] };
      }
    }
  }
  @Output() update = new EventEmitter<string[]>();

  _dataImages = [];
  displayDialog = false;
  placeholderImages: { data: string; loading?: boolean }[] = [];
  currentIndex = undefined;  

  constructor(
    injector: Injector,
    private ng2ImgMax: Ng2ImgMaxService,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit() {
    var posmInvestmentId = this.posmInvestmentItemId; 
    this.dataService.getPosmInvestmentItem(posmInvestmentId).subscribe(async (res) => {
      if (res.result.operationPhoto1) {      
        if(res.result.operationPhoto1.includes("/assets/img_save/")){          
          this.placeholderImages[0] = { data:  await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.operationPhoto1)};
        }
        else{
          this.placeholderImages[0] = { data: res.result.operationPhoto1 };
        }
      }
      else{
        this.placeholderImages[0] = { data: res.result.operationPhoto1 };
      }

      if (res.result.operationPhoto2) {      
        if(res.result.operationPhoto2.includes("/assets/img_save/")){          
          this.placeholderImages[1] = { data:  await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.operationPhoto2)};
        }
        else{
          this.placeholderImages[1] = { data: res.result.operationPhoto2 };
        }
      }
      else{
        this.placeholderImages[1] = { data: res.result.operationPhoto2 };
      }

      if (res.result.operationPhoto3) {      
        if(res.result.operationPhoto3.includes("/assets/img_save/")){          
          this.placeholderImages[2] = { data:  await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.operationPhoto3)};
        }
        else{
          this.placeholderImages[2] = { data: res.result.operationPhoto3 };
        }
      }
      else{
        this.placeholderImages[2] = { data: res.result.operationPhoto3 };
      }

      if (res.result.operationPhoto4) {      
        if(res.result.operationPhoto4.includes("/assets/img_save/")){          
          this.placeholderImages[3] = { data:  await this.convertImgUrl(`${environment.fakeApiUrl}` + res.result.operationPhoto4)};
        }
        else{
          this.placeholderImages[3] = { data: res.result.operationPhoto4 };
        }
      }
      else{
        this.placeholderImages[3] = { data: res.result.operationPhoto4 };
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

  getData(): string[] {
    return this.placeholderImages.map((p) => p.data);
  }

  uploadImage(e) {
    if (!this.readOnly) {
      e.click();
    }
  }
  clearImage(index) {
    var posmInvestmentId = this.posmInvestmentItemId; 
    var imgDataUpdate = '';
            this.getDataService<DataServiceProxy>()
            .posmInvestmentItemsUpdate(              
              new PosmInvestmentItemsUpdateCommand({
                id : posmInvestmentId,
                photoIndex : index + 1,
                operationPhoto: imgDataUpdate
              })
            )           
            .subscribe(
              (_) => {
                this.placeholderImages[index].data = undefined;
              },
              (error) => {
                this.messageService.toastError(error);
              }
            );
    this.placeholderImages[index].data = undefined;         
    this.update.next(this.getData());
  }
  viewImage(index) {
    this.displayDialog = true;
    this.currentIndex = index;
  }
  prevImage() {
    var index = this.currentIndex;
    do {
      index = index - 1;
      if (index < 0) {
        index = this.placeholderImages.length - 1;
      }

      if (this.placeholderImages[index].data) {
        this.currentIndex = index;
        break;
      }
    } while (true);
  }

  nextImage() {
    var index = this.currentIndex;
    do {
      index = index + 1;
      if (index > this.placeholderImages.length - 1) {
        index = 0;
      }

      if (this.placeholderImages[index].data) {
        this.currentIndex = index;
        break;
      }
    } while (true);
  }

  get currentImageData() {
    return this.placeholderImages[this.currentIndex]?.data;
  }

  onFileChange(index, e) {
    var posmInvestmentId = this.posmInvestmentItemId; 
    this.placeholderImages[index].loading = true;
    if (e.target.files && e.target.files[0]) {
      const reader = new FileReader();
      this.ng2ImgMax.resizeImage(e.target.files[0], 800, 600).subscribe(
        (result) => {
          reader.onload = (re: any) => {
            //Upload Img
            var imgDataUpdate = re.target.result;
            this.getDataService<DataServiceProxy>()
            .posmInvestmentItemsUpdate(              
              new PosmInvestmentItemsUpdateCommand({
                id : posmInvestmentId,
                photoIndex : index + 1,
                operationPhoto: imgDataUpdate
              })
            )           
            .subscribe(
              (_) => {
                //console.log(imgDataUpdateResult);
                this.placeholderImages[index].data = re.target.result;
                this.cd.detectChanges();
                this.placeholderImages[index].loading = false;
    
                this.update.next(this.getData());
    
                e.target.value = "";
              },
              (error) => {
                this.messageService.toastError(error);
              }
            );           
          };

          reader.readAsDataURL(result);
        },
        (error) => {
          this.placeholderImages[index].loading = false;
          console.log(error);
        }
      );
    }
  }

  base64Image(data: string): string {
    return data
      ? data.indexOf("base64") != -1
        ? data
        : "data:image/jpeg;base64," + data
      : undefined;
  } 
}
