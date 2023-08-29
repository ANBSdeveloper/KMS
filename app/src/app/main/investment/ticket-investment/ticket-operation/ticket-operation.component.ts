//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { cloneDeep } from "lodash";
import {
  EntityHandlerService,
  FormComponentBase,
  formHelper,
} from "@cbms/ng-core-vuexy";
import { ImageViewerComponent } from "@shared/components/image-viewer/image-viewer.component";
import {
  DataServiceProxy,
  TicketInvestmentDto,
  TicketInvestmentOperateCommand,
  TicketInvestmentOperateDto,
  TicketOperationDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { finalize } from "rxjs/operators";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import moment from "moment";
import { environment } from "environments/environment";
//#endregion

@Component({
  selector: "app-ticket-operation",
  templateUrl: "./ticket-operation.component.html",
  styleUrls: ["./ticket-operation.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketOperationComponent extends FormComponentBase {
  operationPhotos = [];

  @BlockUI() pageBlockUI: NgBlockUI;

  @Input() set investment(value: TicketInvestmentDto) {
    this._investment = value;
    this.mapToForm();
  }
  @ViewChild("operationImageViewer") operationImageViewer: ImageViewerComponent;

  _investment: TicketInvestmentDto;
  model: TicketOperationDto;

  constructor(
    injector: Injector,
    public dataService: DataServiceProxy,
    public entityHandler: EntityHandlerService
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      note: [undefined],
      stockQuantity: [undefined, [Validators.required]],
      operationDate: [undefined, [Validators.required]],
      buyEndDate: [undefined, [Validators.required]],
      issueTicketBeginDate: [undefined, [Validators.required]],
      updateUserName: [undefined],
      updateTime: [undefined],
    });

    this.formGroup.valueChanges.subscribe((_) => {
      this.validateExtend();
    });
  }

  validateExtend() {
    var buyEndDate = moment(this.cValue("buyEndDate"));
    var issueTicketBeginDate = moment(this.cValue("issueTicketBeginDate"));
    var operationDateControl = this.c("operationDate");
    var operationDate = moment(this.cValue("operationDate"));

    if (operationDate.isSameOrBefore(issueTicketBeginDate)) {
      operationDateControl.markAsTouched();
      operationDateControl.setErrors({
        ...operationDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(operationDateControl, "invalidRange");
    }

    if (
      operationDate.isValid() &&
      moment(operationDate)
        .startOf("day")
        .isBefore(moment(buyEndDate).startOf("month"))
    )  {
      operationDateControl.markAsTouched();
      operationDateControl.setErrors({
        ...operationDateControl.errors,
        invalidRangeBuy: true,
      });
    } else {
      this.cRemoveError(operationDateControl, "invalidRangeBuy");
    }
  }

  async mapToForm() {
    if (this._investment.operation) {
      this.model = cloneDeep(this._investment.operation);
    } else {
      this.model = new TicketOperationDto({
        operationDate: this._investment.operationDate,
      });
    }

    this.operationPhotos = [
      this.model.photo1 != null && this.model.photo1 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo1) : this.model.photo1,       
        this.model.photo2 != null && this.model.photo2 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo2) : this.model.photo2, 
        this.model.photo3 != null && this.model.photo3 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo3) : this.model.photo3, 
        this.model.photo4 != null && this.model.photo4 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo4) : this.model.photo4,
        this.model.photo5 != null && this.model.photo5 != "" ? await this.convertImgUrl(`${environment.fakeApiUrl}` + this.model.photo5) : this.model.photo5,
    ];

    this.c("note").setValue(this.model.note);
    this.c("operationDate").setValue(this.model.operationDate);
    this.c("stockQuantity").setValue(this.model.stockQuantity);
    this.c("stockQuantity").setValue(this.model.stockQuantity);
    this.c("updateUserName").setValue(this.model.updateUserName);
    this.c("updateTime").setValue(this.model.lastModificationTime);
    this.c("buyEndDate").setValue(this._investment.buyEndDate);
    this.c("issueTicketBeginDate").setValue(
      this._investment.issueTicketBeginDate
    );
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

  save(complete: boolean) {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    var command = new TicketInvestmentOperateCommand({
      data: new TicketInvestmentOperateDto({
        photo1: this.operationImageViewer.getData()[0],
        photo2: this.operationImageViewer.getData()[1],
        photo3: this.operationImageViewer.getData()[2],
        photo4: this.operationImageViewer.getData()[3],
        photo5: this.operationImageViewer.getData()[4],
        note: this.cValue("note"),
        operationDate: this.cValue("operationDate"),
        stockQuantity: this.cValue("stockQuantity"),
      }),
      handleType: complete ? "complete" : "",
    });

    this.dataService
      .operateTicketInvestment(this._investment.id, command)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.entityHandler.loadRequest("ticket_operation", undefined);
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  get readOnly() {
    return this._investment?.status != TicketInvestmentStatus.Updating ||
    !(this.isGranted("TicketInvestments.Operate") ||
      this.isGranted("TicketInvestments"));
  }
}
