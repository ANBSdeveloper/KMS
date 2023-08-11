import {
  RewardBranchDto,
  RewardPackageUpsertDto,
} from "./../../../../../shared/services/data.service";
import { Validators } from "@angular/forms";
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  RewardItemDto,
  RewardPackageDto,
} from "@shared/services/data.service";
import {
  DatatableDataSource,
  DataTableEntityConfig,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import {
  RewardType,
  RewardTypeDataSource,
} from "../data-source/reward-type.data-source";
import { Subject } from "rxjs";
import { debounceTime, filter, finalize, takeUntil, tap } from "rxjs/operators";
import moment from "moment";
import { ZoneComboComponent } from "../../zone/zone-combo/zone-combo.component";
import { AreaComboComponent } from "../../area/area-combo/area-combo.component";
import { FilterBranchByZoneAreaComponent } from "../../filter/filter-branch-by-zone-area/filter-branch-by-zone-area.component";
import { NavigationEnd } from "@angular/router";
//#endregion
@Component({
  selector: "app-reward-package-edit",
  templateUrl: "./reward-package-edit.component.html",
  styleUrls: ["./reward-package-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class RewardPackageEditComponent extends PageEditFormComponentBase<
  RewardPackageDto,
  RewardPackageUpsertDto,
  RewardPackageUpsertDto
> {
  //#region Variables
  @ViewChild("zoneCombo") zoneCombo: ZoneComboComponent;
  @ViewChild("areaCombo") areaCombo: AreaComboComponent;
  @ViewChild("filterBranchByZoneAreaComponent")
  filterBranchByZoneAreaComponent: FilterBranchByZoneAreaComponent;
  entityName = "reward_package-edit";
  permissionName = "RewardPackages";
  //#endregion
  rewardItemConfig = <DataTableEntityConfig>{
    entityName: "reward_item",
    sidebarName: "reward_item_sidebar",
  };
  rewardItemDataSource = new DatatableDataSource<RewardItemDto>();
  rewardBranchDataSource = new DatatableDataSource<RewardBranchDto>();

  selectedRewardBranches = [];
  totalAmount = 0;
  totalTickets = 0;

  isDisplayAllBranch = false;

  searchChangeSubject = new Subject<string>();
  searchValue: string = "";
  searchZone = 0;
  searchArea = 0;
  routeUrl = "";
  isAllSelected = false;
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
      name: [undefined, [Validators.required, Validators.maxLength(200)]],
      type: [undefined, [Validators.required]],
      isActive: [undefined],
      fromDate: [undefined, [Validators.required]],
      toDate: [undefined, [Validators.required]],
      zoneId: [undefined],
      areaId: [undefined],
    });
    ("");
    this.searchChangeSubject.pipe(debounceTime(300)).subscribe((value) => {
      this.searchValue = value.toLocaleLowerCase();
      this.filterRewardBranch();
    });

    this.rewardItemDataSource.onUpdate.subscribe((item) => {
      this.calculateTotal();
    });
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
    this.rewardItemDataSource.setData(this.model.rewardItems);

    this.calculateTotal();
    if (!this.id) {
      this.isDisplayAllBranch = true;
      this.pageBlockUI.start();
      this.getDataService<DataServiceProxy>()
        .getBranches(
          true,
          undefined,
          undefined,
          undefined,
          undefined,
          undefined
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe((response) => {
          this.rewardBranchDataSource.setData(
            response.result.items.map(
              (branch) =>
                new RewardBranchDto({
                  branchId: branch.id,
                  branchCode: branch.code,
                  branchName: branch.name,
                  areaName: branch.areaName,
                  zoneName: branch.zoneName,
                  zoneId: branch.zoneId,
                  areaId: branch.areaId,
                })
            )
          );
          this.filterRewardBranch();
          this.selectedRewardBranches = [];
        });
    } else {
      this.rewardBranchDataSource.setData(this.model.rewardBranches);
      this.filterRewardBranch();
      this.selectedRewardBranches = this.rewardBranchDataSource.items
        .filter((p) => p.isSelected)
        .map((p) => (<any>p).rowId);
    }

    if (this.duplicateRequest) {
      this.rewardItemDataSource.transferToNewState();
      this.rewardBranchDataSource.transferToNewState();
    }
  }

  mapPropertyFormGroupToSaveModel() {
    this.saveModel.rewardItemChanges = <any>(
      this.rewardItemDataSource.submitData
    );
    this.rewardBranchDataSource.items.forEach((item) => {
      const updateItem = {
        ...item,
        isSelected: false,
      };

      var isSelected = this.selectedRewardBranches.indexOf(item["rowId"]) != -1;
      if (isSelected) {
        updateItem.isSelected = true;
      }

      this.rewardBranchDataSource.updateItem(updateItem);
    });
    this.saveModel.rewardBranchChanges = <any>{
      deletedItems: this.rewardBranchDataSource.submitData.upsertedItems.filter(
        (p) => p.id != 0 && !p.isSelected
      ),
      upsertedItems:
        this.rewardBranchDataSource.submitData.upsertedItems.filter(
          (p) => p.isSelected
        ),
    };
    this.calculateTotal();
  }
  get newModel() {
    return new RewardPackageDto({
      isActive: true,
      type: RewardType.BTTT,
      fromDate: moment().toDate(),
      toDate: moment().toDate(),
      rewardItems: [],
      rewardBranches: [],
    });
  }

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getRewardPackage(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateRewardPackage(
      id,
      data
    );
  }

  get createItemVisible(): boolean {
    return this.isEditGranted; // neu id = null && check quuyen tạo || id != nul && check quyen update
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createRewardPackage(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deleteRewardPackage(id);
  }

  //#endregion

  //#region Reward Items Table
  deleteRewardItemVisible(row: RewardItemDto) {
    return this.isEditGranted;
  }

  editRewardItemVisible(row: RewardItemDto) {
    return this.isEditGranted;
  }

  deleteRewardItem(row: RewardItemDto) {
    this.rewardItemDataSource.removeRecord(row);
    this.calculateTotal();
  }

  editRewardItem(row: RewardItemDto) {
    this.openEditItemDataTable(this.rewardItemConfig, row);
  }

  createRewardItem() {
    this.openNewItemDataTable(this.rewardItemConfig);
  }
  //#endregion

  //#region Branch Table
  selectionChanged(e) {
    if (e.currentDeselectedRowKeys.length > 0) {
      (<number[]>e.currentDeselectedRowKeys).forEach((key) => {
        var rowData = this.rewardBranchDataSource.findItem(key);
        this.rewardBranchDataSource.updateItem({
          ...rowData,
          isSelected: false,
        });
      });
    }
    if (e.currentSelectedRowKeys.length > 0) {
      (<number[]>e.currentSelectedRowKeys).forEach((key) => {
        var rowData = this.rewardBranchDataSource.findItem(key);
        this.rewardBranchDataSource.updateItem({
          ...rowData,
          isSelected: true,
        });
      });
    }
  }
  allSelectedChange(e) {
    this.isAllSelected = e.target.checked;
    this.rewardBranchDataSource.items.forEach((item) => {
      this.rewardBranchDataSource.updateItem({
        ...item,
        isSelected: this.isAllSelected,
      });
    });
  }
  searchChange(e) {
    this.searchChangeSubject.next(e);
  }
  displayAllBranchChange(e) {
    this.isDisplayAllBranch = e;
    this.filterRewardBranch();
  }

  filterRewardBranch() {
    this.rewardBranchDataSource.applyFilter(
      (item) =>
        (item.branchCode.toLocaleLowerCase().indexOf(this.searchValue) != -1 ||
          item.branchName.toLocaleLowerCase().indexOf(this.searchValue) !=
            -1) &&
        (this.isDisplayAllBranch ||
          (!this.isDisplayAllBranch && item.isSelected)) &&
        (this.searchZone == 0 || item.zoneId == this.searchZone) &&
        (this.searchArea == 0 || item.areaId == this.searchArea)
    );
    this.selectedRewardBranches = this.rewardBranchDataSource.items
      .filter((p) => p.isSelected)
      .map((p) => (<any>p).rowId);
  }

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
    //this.c("fromDate").max(this.c("toDate").value);
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

  calculateTotal() {
    this.totalAmount = 0;
    this.totalTickets = 0;

    this.rewardItemDataSource.items.forEach((item) => {
      this.totalAmount += item.price * item.quantity;
      this.totalTickets += item.quantity;
    });
  }

  zoneChange(item) {
    this.searchZone = item == null ? 0 : item;
    this.searchArea = 0;
    this.filterRewardBranch();
  }

  areaChange(item) {
    this.searchArea = item == null ? 0 : item;
    this.filterRewardBranch();
  }

  
  //#endregion
}
