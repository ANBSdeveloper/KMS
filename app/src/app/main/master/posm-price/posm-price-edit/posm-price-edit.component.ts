import {
  PosmPriceUpsertDto,
  PosmPriceHeaderDto,
  PosmPriceDetailDto,
} from "./../../../../../shared/services/data.service";
import { Validators } from "@angular/forms";
//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { DataServiceProxy } from "@shared/services/data.service";
import {
  DatatableDataSource,
  DataTableEntityConfig,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { Subject } from "rxjs";
import { filter, takeUntil } from "rxjs/operators";
import moment from "moment";
import { NavigationEnd } from "@angular/router";
import { PosmUnitTypeDataSource } from "../../posm-item/data-source/posm-unit-type.data-source";
import { PosmCalcTypeDataSource } from "../../posm-item/data-source/posm-calc-type.data-source";
//#endregion
@Component({
  selector: "app-posm-price-edit",
  templateUrl: "./posm-price-edit.component.html",
  styleUrls: ["./posm-price-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmPriceEditComponent extends PageEditFormComponentBase<
  PosmPriceHeaderDto,
  PosmPriceUpsertDto,
  PosmPriceUpsertDto
> {
  //#region Variables
  entityName = "posm-price-edit";
  permissionName = "PosmPrices";
  //#endregion
  detailConfig = <DataTableEntityConfig>{
    entityName: "posm_price_detail",
    sidebarName: "posm_price_detail_sidebar",
  };
  detailDataSource = new DatatableDataSource<PosmPriceDetailDto>();

  searchChangeSubject = new Subject<string>();
  searchValue: string = "";
  routeUrl = "";

  constructor(
    injector: Injector,
    public posmUnitTypeDataSource: PosmUnitTypeDataSource
  ) {
    super(injector);
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        takeUntil(this.unsubscribe$)
      )
      .subscribe((event) => {
        this.routeUrl = (<NavigationEnd>event).url;
      });
  }

  configForm() {
    this.formGroup = this.fb.group({
      code: [
        undefined,
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
      ],
      name: [undefined, [Validators.required, Validators.maxLength(200)]],
      fromDate: [undefined, [Validators.required]],
      toDate: [undefined, [Validators.required]],
      isActive: [undefined]
    });
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
    this.detailDataSource.setData(this.model.details);

    if (this.duplicateRequest) {
      this.detailDataSource.transferToNewState();
    }
  }

  mapPropertyFormGroupToSaveModel() {
    this.saveModel.detailChanges = <any>this.detailDataSource.submitData;
  }
  get newModel() {
    return new PosmPriceHeaderDto({
      fromDate: moment().toDate(),
      toDate: moment().toDate(),
      isActive: true,
      details: [],
    });
  }

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getPosmPrice(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updatePosmPrice(id, data);
  }

  get createItemVisible(): boolean {
    return this.isEditGranted; // neu id = null && check quuyen tạo || id != nul && check quyen update
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createPosmPrice(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deletePosmPrice(id);
  }

  //#endregion

  //#region Reward Items Table
  deleteDetailVisible(row: PosmPriceDetailDto) {
    return this.isEditGranted;
  }

  editDetailVisible(row: PosmPriceDetailDto) {
    return this.isEditGranted;
  }

  deleteDetail(row: PosmPriceDetailDto) {
    this.detailDataSource.removeRecord(row);
  }

  editDetail(row: PosmPriceDetailDto) {
    this.openEditItemDataTable(this.detailConfig, row);
  }

  createDetail() {
    this.openNewItemDataTable(this.detailConfig);
  }
  //#endregion

  toDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("toDate").setValue(undefined);
        this.messageService.toastError(this.l("error_todate"));
      }
    }
  }

  fromDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("fromDate").setValue(undefined);
        this.messageService.toastError(this.l("error_fromdate"));
      }
    }
  }
}
