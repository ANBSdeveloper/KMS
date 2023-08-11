//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  BudgetDto,
  BudgetUpsertDto,
  BudgetZoneDto,
  BudgetAreaDto,
  BudgetBranchDto,
  StaffDtoApiResultObject,
  StaffDto,
} from "@shared/services/data.service";
import { Validators } from "@angular/forms";
import {
  DatatableDataSource,
  DataTableEntityConfig,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { InvestmentType } from "../../data-source/investment-type.enum";
import { BudgetAllocateType } from "../../data-source/budget-allocate-type.enum";
import { RoleType } from "@app/main/system/role/role-type.enum";
import { AuthenticationService } from "@app/auth/service";
import { BudgetAllocateTypeComboComponent } from "../budget-allocate-type-combo/budget-allocate-type-combo.component";
import { BudgetZoneComboComponent } from "../budget-zone-combo/budget-zone-combo.component";
import { Subject } from "rxjs";
import { CycleComboComponent } from "@app/main/master/cycle/cycle-combo/cycle-combo.component";
import { BudgetAreaComboComponent } from "../budget-area-combo/budget-area-combo.component";
//#endregion

@Component({
  selector: "app-budget-edit",
  templateUrl: "./budget-edit.component.html",
  styleUrls: ["./budget-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class BudgetEditComponent extends PageEditFormComponentBase<
  BudgetDto,
  BudgetUpsertDto,
  BudgetUpsertDto
> {
  //#region Variables
  @ViewChild("zoneCombo") zoneCombo: BudgetZoneComboComponent;
  @ViewChild("areaCombo") areaCombo: BudgetZoneComboComponent;
  @ViewChild("cycleCombo") cycleCombo: CycleComboComponent;

  entityName = "budget";
  permissionName = "Budgets";
  codeProperty = "budgetName";
  createUrl = "investment/new-budget";

  zoneDataSource = new DatatableDataSource<BudgetZoneDto>();
  areaDataSource = new DatatableDataSource<BudgetAreaDto>();
  branchDataSource = new DatatableDataSource<BudgetBranchDto>();

  zoneEntityConfig: DataTableEntityConfig = {
    entityName: "budget_zone",
    sidebarName: "budget_zone_sidebar",
  };
  areaEntityConfig: DataTableEntityConfig = {
    entityName: "budget_area",
    sidebarName: "budget_area_sidebar",
  };
  branchEntityConfig: DataTableEntityConfig = {
    entityName: "budget_branch",
    sidebarName: "budget_branch_sidebar",
  };

  nonFilterDetails: {
    zones: BudgetZoneDto[];
    areas: BudgetAreaDto[];
    branches: BudgetBranchDto[];
  } = {
    zones: [],
    areas: [],
    branches: [],
  };

  allocateAmount = 0;
  unallocateAmount = undefined;
  remainAmount = 0;
  usedAmount = 0;
  tempRemainAmount = 0;
  tempUsedAmount = 0;
  ownAmount = 0;
  staffInfo: StaffDto = undefined;

  zoneSearchChangeSubject = new Subject<string>();
  areaSearchChangeSubject = new Subject<string>();
  branchSearchChangeSubject = new Subject<string>();
  //#endregion

  constructor(
    injector: Injector,
    private authenticatonService: AuthenticationService
  ) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      cycleId: [undefined, [Validators.required]],
      fromDate: [undefined],
      toDate: [undefined],
      investmentType: [undefined, [Validators.required]],
      allocateType: [undefined, [Validators.required]],
      zoneId: [undefined],
      areaId: [undefined],
    });
    this.zoneDataSource.setData([]);
    this.zoneDataSource.onUpdate.subscribe((item) => {
      this.calcuateTotalAmount();
    });

    this.areaDataSource.setData([]);
    this.areaDataSource.onUpdate.subscribe((item) => {
      this.calcuateTotalAmount();
    });
    this.branchDataSource.setData([]);
    this.branchDataSource.onUpdate.subscribe((item) => {
      this.calcuateTotalAmount();
    });

    this.zoneSearchChangeSubject
      .pipe(debounceTime(200), takeUntil(this.unsubscribe$))
      .subscribe((value) => {
        var searchValue = value.toLocaleLowerCase();
        if (searchValue) {
          this.zoneDataSource.applyFilter(
            (item: BudgetZoneDto) =>
              item.zoneCode.toLocaleLowerCase().indexOf(searchValue) != -1 ||
              item.zoneName.toLocaleLowerCase().indexOf(searchValue) != -1
          );
        } else {
          this.zoneDataSource.clearFilter();
        }
      });

    this.areaSearchChangeSubject
      .pipe(debounceTime(200), takeUntil(this.unsubscribe$))
      .subscribe((value) => {
        var searchValue = value.toLocaleLowerCase();
        if (searchValue) {
          this.areaDataSource.applyFilter(
            (item: BudgetAreaDto) =>
              item.areaCode.toLocaleLowerCase().indexOf(searchValue) != -1 ||
              item.areaName.toLocaleLowerCase().indexOf(searchValue) != -1
          );
        } else {
          this.areaDataSource.clearFilter();
        }
      });

    this.branchSearchChangeSubject
      .pipe(debounceTime(200), takeUntil(this.unsubscribe$))
      .subscribe((value) => {
        var searchValue = value.toLocaleLowerCase();
        if (searchValue) {
          this.branchDataSource.applyFilter(
            (item: BudgetBranchDto) =>
              item.branchCode.toLocaleLowerCase().indexOf(searchValue) != -1 ||
              item.branchName.toLocaleLowerCase().indexOf(searchValue) != -1
          );
        } else {
          this.branchDataSource.clearFilter();
        }
      });
  }

  get formReadOnly(): boolean {
    return (
      this.zoneDataSource.hasChange ||
      this.areaDataSource.hasChange ||
      this.branchDataSource.hasChange ||
      this.readOnly
    );
  }

  get newModel() {
    return new BudgetDto({
      cycleId: undefined,
      investmentType: InvestmentType.BTTT,
    });
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();

    this.saveModel.zonesChanges = <any>this.zoneDataSource.submitData;
    this.saveModel.areasChanges = <any>this.areaDataSource.submitData;
    this.saveModel.branchesChanges = <any>this.branchDataSource.submitData;
  }

  initModel(): void {
    this.getDataService<DataServiceProxy>()
      .getStaffInfo()
      .subscribe((res) => {
        this.staffInfo = res.result;
        super.initModel();
      });
  }

  mapModelToFormGroup() {
    Object.keys(this.formGroup.controls).forEach((key) => {
      if (this.model.hasOwnProperty(key)) {
        this.formGroup.controls[key].setValue(this.model[key]);
      }
    });
    this.mapPropertyModelToFormGroup();

    this.checkChange();

    // init allocate type
    this.initAllocateType();

    if (!this.id) {
      this.pageBlockUI.start();

      this.getDataService<DataServiceProxy>()
        .getBudgetInitDetail(undefined, undefined)
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe((response) => {
          this.nonFilterDetails = {
            zones: response.result.zones.map(
              (item) =>
                <BudgetZoneDto>{
                  ...item,
                  allocateAmount: 0,
                  usedAmount: 0,
                  remainAmount: 0,
                  tempUsedAmount: 0,
                  tempRemainAmount: 0,
                }
            ),
            areas: response.result.areas.map(
              (item) =>
                <BudgetAreaDto>{
                  ...item,
                  allocateAmount: 0,
                  usedAmount: 0,
                  remainAmount: 0,
                  tempUsedAmount: 0,
                  tempRemainAmount: 0,
                }
            ),
            branches: response.result.branches.map(
              (item) =>
                <BudgetBranchDto>{
                  ...item,
                  allocateAmount: 0,
                  usedAmount: 0,
                  remainAmount: 0,
                  tempUsedAmount: 0,
                  tempRemainAmount: 0,
                }
            ),
          };
          this.applyAllocateType(this.c("allocateType").value);
        });
    } else {
      this.nonFilterDetails = {
        zones: this.model.zones,
        areas: this.model.areas,
        branches: this.model.branches,
      };

      this.applyAllocateType(this.c("allocateType").value);
    }
  }

  calcuateTotalAmount() {
    this.allocateAmount = 0;
    this.unallocateAmount = 0;
    this.remainAmount = 0;
    this.usedAmount = 0;
    this.tempRemainAmount = 0;
    this.tempUsedAmount = 0;

    switch (this.c("allocateType").value) {
      case BudgetAllocateType.Zone:
        this.zoneDataSource.items.forEach((item) => {
          this.allocateAmount += item.allocateAmount;
          this.remainAmount += item.remainAmount;
          this.usedAmount += item.usedAmount;
          this.tempRemainAmount += item.tempRemainAmount;
          this.tempUsedAmount += item.tempUsedAmount;
        });
        this.ownAmount = undefined;
        break;
      case BudgetAllocateType.Area:
        this.areaDataSource.items.forEach((item) => {
          this.allocateAmount += item.allocateAmount;
          this.remainAmount += item.remainAmount;
          this.usedAmount += item.usedAmount;
          this.tempRemainAmount += item.tempRemainAmount;
          this.tempUsedAmount += item.tempUsedAmount;
        });
        const zoneId = this.c("zoneId").value;
        const zone = this.nonFilterDetails.zones.find(
          (p) => p.zoneId == zoneId
        );
        this.ownAmount = zone?.allocateAmount;
        break;
      case BudgetAllocateType.Branch:
        this.branchDataSource.items.forEach((item) => {
          this.allocateAmount += item.allocateAmount;
          this.remainAmount += item.remainAmount;
          this.usedAmount += item.usedAmount;
          this.tempRemainAmount += item.tempRemainAmount;
          this.tempUsedAmount += item.tempUsedAmount;
        });
        const areaId = this.c("areaId").value;
        const area = this.nonFilterDetails.areas.find(
          (p) => p.areaId == areaId
        );
        this.ownAmount = area?.allocateAmount;
        break;
    }

    this.unallocateAmount = this.ownAmount - this.allocateAmount;
  }

  allocateTypeChange(e) {
    var combo = e as BudgetAllocateTypeComboComponent;
    this.applyAllocateType(combo.value);
  }

  zoneChange(e) {
    this.buildDetailDataSource();
  }

  areaChange(e) {
    this.buildDetailDataSource();
  }

  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getBudget(+id);
  }

  updateRequest(id, data): any {
    return this.getDataService<DataServiceProxy>().updateBudget(id, data);
  }

  createRequest(data): any {
    return this.getDataService<DataServiceProxy>().createBudget(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deleteBudget(id);
  }

  //#endregion

  //#region Budget Details
  zoneEdit(item) {
    this.openEditItemDataTable(this.zoneEntityConfig, item);
  }

  zoneEditVisible(_item) {
    return this.isEditGranted;
  }
  zoneSearchChange(e) {
    this.zoneSearchChangeSubject.next(e.target.value);
  }

  areaEdit(item) {
    this.openEditItemDataTable(this.areaEntityConfig, item);
  }

  areaEditVisible(_item) {
    return this.isEditGranted;
  }

  areaSearchChange(e) {
    this.areaSearchChangeSubject.next(e.target.value);
  }

  branchEdit(item) {
    this.openEditItemDataTable(this.branchEntityConfig, item);
  }

  branchEditVisible(_item) {
    return this.isEditGranted;
  }

  branchSearchChange(e) {
    this.branchSearchChangeSubject.next(e.target.value);
  }
  //#endregion

  //#region Permissions
  get isUpdateGranted(): boolean {
    return (
      this.isGranted(this.updatePermissionName) ||
      this.isGranted("Budgets.AllocateArea") ||
      this.isGranted("Budgets.AllocateSS")
    );
  }
  //#endregion

  //#region Handle Role
  get isAllowAllocateAll() {
    return this.isGranted("Budgets");
  }
  get isAllowAllocateArea() {
    return this.isGranted("Budgets.AllocateArea");
  }
  get isAllowAllocateBranch() {
    return this.isGranted("Budgets.AllocateBranch");
  }

  get allocateTypeVisible(): boolean {
    if (this.isAllowAllocateAll || this.isAllowAllocateArea) return true;
    return false;
  }

  get ownAmountVisible(): boolean {
    return this.c("zoneId").value || this.c("areaId").value;
  }

  get areaComboVisible(): boolean {
    return this.c("allocateType").value == BudgetAllocateType.Branch;
  }

  get zoneComboVisible(): boolean {
    return this.c("allocateType").value == BudgetAllocateType.Area;
  }

  get areaGridVisible(): boolean {
    return this.c("allocateType").value == BudgetAllocateType.Area;
  }

  get zoneGridVisible(): boolean {
    return this.c("allocateType").value == BudgetAllocateType.Zone;
  }

  get branchGridVisible(): boolean {
    return this.c("allocateType").value == BudgetAllocateType.Branch;
  }

  get isAllocTypeBranch() {
    return this.c("allocateType").value == BudgetAllocateType.Branch;
  }

  applyAllocateType(value: BudgetAllocateType) {
    this.buildComboDataSource();

    if (value == BudgetAllocateType.Zone) {
      this.c("zoneId").setValue(undefined);
      this.c("areaId").setValue(undefined);
    } else if (value == BudgetAllocateType.Area) {
      this.c("areaId").setValue(undefined);
      if (this.isAllowAllocateAll) {
        this.c("zoneId").setValue(this.staffInfo?.zoneId);
      } else {
        this.c("zoneId").setValue(undefined);
      }
    } else if (value == BudgetAllocateType.Branch) {
      this.c("zoneId").setValue(undefined);
      if (!this.isAllowAllocateAll && !this.isAllowAllocateArea) {
        this.c("areaId").setValue(this.staffInfo?.areaId);
      } else {
        this.c("areaId").setValue(undefined);
      }
    }

    this.buildDetailDataSource();
  }

  initAllocateType() {
    if (!this.c("allocateType").value) {
      if (this.id) {
        if (this.isAllowAllocateAll) {
          this.c("allocateType").setValue(BudgetAllocateType.Zone);
        } else if (this.isAllowAllocateArea) {
          this.c("allocateType").setValue(BudgetAllocateType.Area);
        } else if (this.isGranted("Budgets.AllocateBranch")) {
          this.c("allocateType").setValue(BudgetAllocateType.Branch);
        }
      } else {
        this.c("allocateType").setValue(BudgetAllocateType.Zone);
      }
    }
  }

  buildDetailDataSource() {
    const allocateType = this.c("allocateType").value;
    const zoneId = this.c("zoneId").value;
    const areaId = this.c("areaId").value;

    var areaItems = [];
    var branchItems = [];
    if (allocateType == BudgetAllocateType.Area) {
      areaItems = this.nonFilterDetails.areas.filter((p) => p.zoneId == zoneId);
    } else if (allocateType == BudgetAllocateType.Branch) {
      branchItems = this.nonFilterDetails.branches.filter(
        (p) => p.areaId == areaId
      );
    }
    this.zoneDataSource.setData(this.nonFilterDetails.zones);
    this.areaDataSource.setData(areaItems);
    this.branchDataSource.setData(branchItems);
    this.calcuateTotalAmount();
  }

  buildComboDataSource() {
    const allocateType = this.c("allocateType").value;
    if (allocateType == BudgetAllocateType.Zone) {
      this.zoneCombo.dataSource = this.nonFilterDetails.zones.map((item) => ({
        zoneId: item.zoneId,
        zoneCode: item.zoneCode,
        name: item.zoneName,
        displayName: item.zoneCode + " - " + item.zoneName,
      }));
    } else if (allocateType == BudgetAllocateType.Area) {
      if (this.isAllowAllocateAll) {
        this.zoneCombo.dataSource = this.nonFilterDetails.zones.map((item) => ({
          zoneId: item.zoneId,
          zoneCode: item.zoneCode,
          name: item.zoneName,
          displayName: item.zoneCode + " - " + item.zoneName,
        }));
      } else {
        const zoneIdOfUser = this.staffInfo.zoneId;
        this.zoneCombo.dataSource = this.nonFilterDetails.zones
          .filter((item) => item.zoneId == zoneIdOfUser)
          .map((item) => ({
            zoneId: item.zoneId,
            zoneCode: item.zoneCode,
            name: item.zoneName,
            displayName: item.zoneCode + " - " + item.zoneName,
          }));
      }
    } else if (allocateType == BudgetAllocateType.Branch) {
      if (this.isAllowAllocateAll) {
        this.areaCombo.dataSource = this.nonFilterDetails.areas.map((item) => ({
          areaId: item.areaId,
          areaCode: item.areaCode,
          name: item.areaName,
          displayName: item.areaCode + " - " + item.areaName,
        }));
      } else if(this.isAllowAllocateArea) {
        const zoneIdOfUser = this.staffInfo.zoneId;
        this.areaCombo.dataSource = this.nonFilterDetails.areas
          .filter((item) => item.zoneId == zoneIdOfUser)
          .map((item) => ({
            areaId: item.areaId,
            areaCode: item.areaCode,
            name: item.areaName,
            displayName: item.areaCode + " - " + item.areaName,
          }));
      } else {
        const areaIdOfUser = this.staffInfo.areaId;
        this.areaCombo.dataSource = this.nonFilterDetails.areas
          .filter((item) => item.areaId == areaIdOfUser)
          .map((item) => ({
            areaId: item.areaId,
            areaCode: item.areaCode,
            name: item.areaName,
            displayName: item.areaCode + " - " + item.areaName,
          }));
      }
    }
  }

  cycleChange(e) {
    this.c("fromDate").setValue(this.cycleCombo.selectedItem.fromDate);
    this.c("toDate").setValue(this.cycleCombo.selectedItem.toDate);
  }

  //#endregion
}
