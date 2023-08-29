//#region Import
import {
  Component,
  EventEmitter,
  Injector,
  Input,
  Output,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";

import {
  DatatableDataSource,
  EntityHandlerService,
  FormComponentBase,
} from "@cbms/ng-core-vuexy";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import {
  DataServiceProxy,
  TicketInvestmentUpsertProgressCommand,
  TicketInvestmentUpsertProgressDto,
  TicketInvestmentUpsertProgressMaterialDto,
  TicketInvestmentUpsertProgressRewardItemDto,
  TicketProgressDto,
  TicketProgressMaterialDto,
  TicketProgressRewardItemDto,
} from "@shared/services/data.service";
import { environment } from "environments/environment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { finalize } from "rxjs/operators";
//#endregion

@Component({
  selector: "app-ticket-progress-item",
  templateUrl: "./ticket-progress-item.component.html",
  styleUrls: ["./ticket-progress-item.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketProgressItemComponent extends FormComponentBase {
  documentPhotos = [];

  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public entityHandler: EntityHandlerService
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      note: [undefined],
    });
  }

  @BlockUI() pageBlockUI: NgBlockUI;
  @Input() set data(value: TicketProgressDto) {
    this._data = value;
    this.mapToForm();
  }
  @ViewChild("documentImageViewer") documentImageViewer: ImageViewerComponent;
  @Input() ticketInvestmentId: number;
  materialDataSource = new DatatableDataSource<TicketProgressMaterialDto>();
  rewardItemDataSource = new DatatableDataSource<TicketProgressRewardItemDto>();
  _data: TicketProgressDto;
  cachePhotos = {};
  materialPhotos = {};
  designMaterials = [];

  getMaterialPhotos(material: TicketProgressMaterialDto): string[] {
    if (!this.cachePhotos[material.id]) {
      this.cachePhotos[material.id] = [
        material.photo1,
        material.photo2,
        material.photo3,
        material.photo4,
        material.photo5,
      ];
    }

    return this.cachePhotos[material.id];
  }

  ngOnInit() {}

  async mapToForm() {
    this.documentPhotos = [
      this._data.documentPhoto1 != null && this._data.documentPhoto1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this._data.documentPhoto1) : this._data.documentPhoto1,       
        this._data.documentPhoto2 != null && this._data.documentPhoto2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this._data.documentPhoto2) : this._data.documentPhoto2, 
        this._data.documentPhoto3 != null && this._data.documentPhoto3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this._data.documentPhoto3) : this._data.documentPhoto3, 
        this._data.documentPhoto4 != null && this._data.documentPhoto4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this._data.documentPhoto4) : this._data.documentPhoto4,
        this._data.documentPhoto5 != null && this._data.documentPhoto5 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this._data.documentPhoto5) : this._data.documentPhoto5,
    ];

    this.c("note").setValue(this._data.note);

    this.materialDataSource.setData(this._data.materials);
    this.rewardItemDataSource.setData(this._data.rewardItems);
    if (!this._data.id) {
      this.materialDataSource.transferToNewState();
      this.rewardItemDataSource.transferToNewState();
    }

    this.designMaterials = this._data.materials
      ? this._data.materials.filter((p) => p.isDesign)
      : [];
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

  save() {
    this.pageBlockUI.start();

    var materials = this.materialDataSource.submitData.upsertedItems;
    materials.forEach((material) => {
      if (this.materialPhotos[material.materialId]) {
        material.photo1 = this.materialPhotos[material.materialId][0];
        material.photo2 = this.materialPhotos[material.materialId][1];
        material.photo3 = this.materialPhotos[material.materialId][2];
        material.photo4 = this.materialPhotos[material.materialId][3];
        material.photo5 = this.materialPhotos[material.materialId][4];
      }
    });

    var command = new TicketInvestmentUpsertProgressCommand({
      data: new TicketInvestmentUpsertProgressDto({
        documentPhoto1: this.documentImageViewer.getData()[0],
        documentPhoto2: this.documentImageViewer.getData()[1],
        documentPhoto3: this.documentImageViewer.getData()[2],
        documentPhoto4: this.documentImageViewer.getData()[3],
        documentPhoto5: this.documentImageViewer.getData()[4],
        note: this.cValue("note"),
        upsertMaterials: materials.map(
          (item) => new TicketInvestmentUpsertProgressMaterialDto({ ...item })
        ),
        upsertRewardItems:
          this.rewardItemDataSource.submitData.upsertedItems.map(
            (item) =>
              new TicketInvestmentUpsertProgressRewardItemDto({ ...item })
          ),
      }),
    });
    this.dataService
      .createTicketInvestmentProgress(this.ticketInvestmentId, command)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.entityHandler.loadRequest("ticket_progress", undefined);
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  updateMaterialPhoto(materialId, data: string[]) {
    this.materialPhotos[materialId] = data;
  }

  get readOnly() {
    return this._data?.id;
  }
}
