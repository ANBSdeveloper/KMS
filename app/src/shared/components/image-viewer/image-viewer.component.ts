import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Injector,
  Input,
  Output,
  ViewChildren,
} from "@angular/core";
import { AppComponentBase } from "@cbms/ng-core-vuexy";
import { Ng2ImgMaxService } from "ng2-img-max";

@Component({
  selector: "app-image-viewer",
  templateUrl: "./image-viewer.component.html",
  styleUrls: ["./image-viewer.component.scss"],
})
export class ImageViewerComponent extends AppComponentBase {
  @ViewChildren("imageInput") imageInputs: ElementRef[];
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

  ngOnInit() {}

  getData(): string[] {
    return this.placeholderImages.map((p) => p.data);
  }

  uploadImage(e) {
    if (!this.readOnly) {
      e.click();
    }
  }
  clearImage(index) {
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
    this.placeholderImages[index].loading = true;
    if (e.target.files && e.target.files[0]) {
      const reader = new FileReader();
      this.ng2ImgMax.resizeImage(e.target.files[0], 800, 600).subscribe(
        (result) => {
          reader.onload = (re: any) => {
            this.placeholderImages[index].data = re.target.result;
            this.cd.detectChanges();
            this.placeholderImages[index].loading = false;

            this.update.next(this.getData());

            e.target.value = "";
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
