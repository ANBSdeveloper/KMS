import {
  PosmItemUpsertDto,
  PosmCatalogDto,
} from "./../../../../../shared/services/data.service";
import { Validators } from "@angular/forms";
//#region Import
import {
  Component,
  Injector,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  RewardItemDto,
  PosmItemDto,
} from "@shared/services/data.service";
import {
  DatatableDataSource,
  DataTableEntityConfig,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { Subject } from "rxjs";
import { filter, takeUntil } from "rxjs/operators";
import { NavigationEnd } from "@angular/router";
//#endregion
@Component({
  selector: "app-posm-item-edit",
  templateUrl: "./posm-item-edit.component.html",
  styleUrls: ["./posm-item-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmItemEditComponent extends PageEditFormComponentBase<
  PosmItemDto,
  PosmItemUpsertDto,
  PosmItemUpsertDto
> {
  //#region Variables
  // @ViewChild("zoneCombo") zoneCombo: ZoneComboComponent;
  // @ViewChild("areaCombo") areaCombo: AreaComboComponent;
  // @ViewChild("filterBranchByZoneAreaComponent")
  // filterBranchByZoneAreaComponent: FilterBranchByZoneAreaComponent;
  entityName = "posm_item";
  permissionName = "PosmItems";
  //#endregion
  posmCatalogConfig = <DataTableEntityConfig>{
    entityName: "posm_catalog",
    sidebarName: "posm_catalog_sidebar",
  };
  posmCatalogDataSource = new DatatableDataSource<PosmCatalogDto>();
  searchChangeSubject = new Subject<string>();
  searchValue: string = "";
  searchZone = 0;
  searchArea = 0;
  routeUrl = "";

  constructor(injector: Injector) {
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
      link: [undefined, [Validators.required]],
      name: [undefined, [Validators.required, Validators.maxLength(200)]],
      posmClassId: [undefined, [Validators.required]],
      posmTypeId: [undefined, [Validators.required]],
      unitType: [undefined, [Validators.required]],
      calcType: [undefined, [Validators.required]],
      isActive: [undefined],
      // fromDate: [undefined, [Validators.required]],
      // toDate: [undefined, [Validators.required]],
      // zoneId: [undefined],
      // areaId: [undefined],
    });
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
    this.posmCatalogDataSource.setData(this.model.catalogs);

    // if (!this.id) {
    //   this.pageBlockUI.start();
    //   this.getDataService<DataServiceProxy>()
    //     .getBranches(
    //       true,
    //       undefined,
    //       undefined,
    //       undefined,
    //       undefined,
    //       undefined
    //     )
    //     .pipe(finalize(() => this.pageBlockUI.stop()))
    //     .subscribe((response) => {
    //       this.rewardBranchDataSource.setData(
    //         response.result.items.map(
    //           (branch) =>
    //             new RewardBranchDto({
    //               branchId: branch.id,
    //               branchCode: branch.code,
    //               branchName: branch.name,
    //               areaName: branch.areaName,
    //               zoneName: branch.zoneName,
    //               zoneId: branch.zoneId,
    //               areaId: branch.areaId,
    //             })
    //         )
    //       );
    //       this.filterCatalog();
    //       this.selectedRewardBranches = [];
    //     });
    // } else {
    //   this.rewardBranchDataSource.setData(this.model.rewardBranches);
    //   this.filterCatalog();
    //   this.selectedRewardBranches = this.rewardBranchDataSource.items
    //     .filter((p) => p.isSelected)
    //     .map((p) => (<any>p).rowId);
    // }

    if (this.duplicateRequest) {
      this.posmCatalogDataSource.transferToNewState();
    }
  }

  mapPropertyFormGroupToSaveModel() {
    this.saveModel.catalogChanges = <any>(
      this.posmCatalogDataSource.submitData
    );
  }
  get newModel() {
    return new PosmItemDto({
      isActive: true,
      // fromDate: moment().toDate(),
      // toDate: moment().toDate(),
      catalogs: []
    });
  }

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getPosmItem(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updatePosmItem(
      id,
      data
    );
  }

  get createItemVisible(): boolean {
    return this.isEditGranted; // neu id = null && check quuyen tạo || id != nul && check quyen update
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createPosmItem(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deletePosmItem(id);
  }

  //#endregion

  //#region Reward Items Table
  deletePosmCatalogVisible(row: RewardItemDto) {
    return this.isEditGranted;
  }

  editPosmCatalogVisible(row: RewardItemDto) {
    return this.isEditGranted;
  }

  deletePosmCatalog(row: RewardItemDto) {
    this.posmCatalogDataSource.removeRecord(row);
  }

  editPosmCatalog(row: RewardItemDto) {
    this.openEditItemDataTable(this.posmCatalogConfig, row);
  }

  createPosmCatalog() {
    this.openNewItemDataTable(this.posmCatalogConfig);
  }
  //#endregion

  // toDateChange() {
  //   if (
  //     this.c("toDate").value != undefined &&
  //     this.c("fromDate").value != undefined
  //   ) {
  //     if (this.c("toDate").value < this.c("fromDate").value) {
  //       this.c("toDate").setValue(undefined);
  //       this.messageService.toastError(this.l("error_todate"));
  //     }
  //   }
  //   //this.c("fromDate").max(this.c("toDate").value);
  // }

  // fromDateChange() {
  //   if (
  //     this.c("toDate").value != undefined &&
  //     this.c("fromDate").value != undefined
  //   ) {
  //     if (this.c("toDate").value < this.c("fromDate").value) {
  //       this.c("fromDate").setValue(undefined);
  //       this.messageService.toastError(this.l("error_fromdate"));
  //     }
  //   }
  // }



  // zoneChange(item) {
  //   this.searchZone = item == null ? 0 : item;
  //   this.searchArea = 0;
  //   this.filterCatalog();
  // }

  // areaChange(item) {
  //   this.searchArea = item == null ? 0 : item;
  //   this.filterCatalog();
  // }
}
