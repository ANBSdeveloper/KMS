//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from "@angular/forms";
import { FilterBranchByZoneAreaComponent } from "@app/main/master/filter/filter-branch-by-zone-area/filter-branch-by-zone-area.component";
import {
  AppComponentBase,
  DatatableDataSource,
  formHelper,
  LocalizationService,
} from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  InvestmentBranchSettingDto,
  InvestmentSettingDtoApiResultObject,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { Toast, ToastrService } from "ngx-toastr";
import { Observable, Subject } from "rxjs";
import { debounceTime, finalize } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-investment-setting",
  templateUrl: "./investment-setting.component.html",
  styleUrls: ["./investment-setting.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class InvestmentSettingComponent extends AppComponentBase {
  //#region Variables
  permissionName = "InvestmentSettings";
  @ViewChild("filterBranchByZoneAreaComponent")
  filterBranchByZoneAreaComponent: FilterBranchByZoneAreaComponent;
  searchZone = 0;
  searchArea = 0;
  //#endregion
  @BlockUI() pageBlockUI: NgBlockUI;

  searchChangeSubject = new Subject<string>();
  searchValue: string = "";
  formGroup: FormGroup;
  selectedNotificationBranches = [];
  investmentBranchDataSource =
    new DatatableDataSource<InvestmentBranchSettingDto>();

  constructor(
    injector: Injector,
    public localizationService: LocalizationService,
    public dataService: DataServiceProxy,
    public fb: FormBuilder,
    private toastrService: ToastrService
  ) {
    super(injector);

    this.pageBlockUI.start();

    this.formGroup = fb.group({
      amountPerPoint: [undefined, [Validators.required, Validators.min(0)]],
      maxInvestAmount: [undefined, [Validators.required, Validators.min(0)]],
      maxInvestmentQueryMonths: [
        undefined,
        [Validators.required, Validators.min(0), Validators.max(36)],
      ],
      id: [undefined],
      checkQrCodeBranch: [undefined],
      defaultPointsForTicket: [
        undefined,
        [Validators.required, Validators.min(0)],
      ],
      investmentSettingBranchs: [],
      beginIssueDaysAfterCurrent: [
        undefined,
        [Validators.required, Validators.min(0), Validators.max(365)],
      ],
      endIssueDaysBeforeOperation: [
        undefined,
        [Validators.required, Validators.min(0), Validators.max(365)],
      ],
    });
    this.searchChangeSubject.pipe(debounceTime(300)).subscribe((value) => {
      this.searchValue = value.toLocaleLowerCase();
      this.filterBranch();
    });
    this.dataService
      .getInvestmentSetting()
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe((response) => {
        if (response.result) {
          this.mapModelToForm(response.result);
        }
      });
  }

  // configForm() {
  //   this.searchChangeSubject.pipe(debounceTime(300)).subscribe((value) => {
  //     this.searchValue = value.toLocaleLowerCase();
  //     this.filterBranch();
  //   });
  // }

  c(name: string): AbstractControl {
    return this.formGroup.get(name);
  }
  save() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return;
    }
    this.investmentBranchDataSource.items.forEach((item) => {
      const updateItem = {
        ...item,
        isSelected: false,
      };

      var isSelected =
        this.selectedNotificationBranches.indexOf(item["rowId"]) != -1;
      if (isSelected) {
        updateItem.isSelected = true;
      }

      this.investmentBranchDataSource.updateItem(updateItem);
    });

    const saveModel = <any>{
      data: <any>{
        id: this.c("id").value === null ? 0 : this.c("id").value,
        amountPerPoint: this.c("amountPerPoint").value,
        maxInvestAmount: this.c("maxInvestAmount").value,
        maxInvestmentQueryMonths: this.c("maxInvestmentQueryMonths").value,
        checkQrCodeBranch: this.c("checkQrCodeBranch").value,
        defaultPointsForTicket: this.c("defaultPointsForTicket").value,
        beginIssueDaysAfterCurrent: this.c("beginIssueDaysAfterCurrent").value,
        endIssueDaysBeforeOperation: this.c("endIssueDaysBeforeOperation")
          .value,
        investmentBranchSettingChanges: <any>{
          deletedItems:
            this.investmentBranchDataSource.submitData.upsertedItems.filter(
              (p) => p.id != 0 && !p.isSelected
            ),
          upsertedItems:
            this.investmentBranchDataSource.submitData.upsertedItems.filter(
              (p) => p.id == 0 && p.isSelected
            ),
        },
      },
    };
    this.pageBlockUI.start();
    var request: Observable<InvestmentSettingDtoApiResultObject> = saveModel.id
      ? this.dataService.updateInvestmentSetting(saveModel.id, saveModel)
      : this.dataService.createInvestmentSetting(saveModel);

    request.pipe(finalize(() => this.pageBlockUI.stop())).subscribe(
      (response) => {
        if (response.result) {
          this.mapModelToForm(response.result);
        }
        this.messageService.toastSuccess(
          this.l("submit_success_message_title"),
          this.l("submit_success_message_content")
        );
      },
      (error) => {
        this.messageService.toastError(error);
      }
    );
  }
  cErrorValidate(name: string, validateName: string): boolean {
    return (
      this.c(name).hasError(validateName) &&
      (this.c(name).touched || this.c(name).dirty)
    );
  }
  cError(name: string): boolean {
    return this.c(name).invalid && (this.c(name).touched || this.c(name).dirty);
  }
  mapModelToForm(result: any) {
    this.c("amountPerPoint").setValue(result.amountPerPoint);
    this.c("maxInvestAmount").setValue(result.maxInvestAmount);
    this.c("maxInvestmentQueryMonths").setValue(
      result.maxInvestmentQueryMonths
    );
    this.c("checkQrCodeBranch").setValue(result.checkQrCodeBranch);
    this.c("defaultPointsForTicket").setValue(result.defaultPointsForTicket);
    this.c("id").setValue(result.id);
    this.c("beginIssueDaysAfterCurrent").setValue(
      result.beginIssueDaysAfterCurrent
    );
    this.c("endIssueDaysBeforeOperation").setValue(
      result.endIssueDaysBeforeOperation
    );
    if (result.investmentSettingBranchs != null) {
      this.investmentBranchDataSource.setData(result.investmentSettingBranchs);
    }

    this.filterBranch();
    this.selectedNotificationBranches = this.investmentBranchDataSource.items
      .filter((p) => p.isSelected)
      .map((p) => (<any>p).rowId);
  }
  get saveVisible(): boolean {
    return this.isGranted(this.permissionName);
  }
  get assignmentVisible() {
    return true;
  }
  filterBranch() {
    this.investmentBranchDataSource.applyFilter(
      (item) =>
        (item.branchCode.toLocaleLowerCase().indexOf(this.searchValue) != -1 ||
          item.branchName.toLocaleLowerCase().indexOf(this.searchValue) !=
            -1) &&
        (this.isDisplayAllBranch ||
          (!this.isDisplayAllBranch && item.id != 0)) &&
        (this.searchZone == 0 || item.zoneId == this.searchZone) &&
        (this.searchArea == 0 || item.areaId == this.searchArea)
    );
    this.selectedNotificationBranches = this.investmentBranchDataSource.items
      .filter((p) => p.isSelected)
      .map((p) => (<any>p).rowId);
  }
  displayAllBranchChange(e) {
    this.isDisplayAllBranch = e;
    this.filterBranch();
  }
  isDisplayAllBranch = false;

  selectionChanged(e) {
    if (e.currentDeselectedRowKeys.length > 0) {
      (<number[]>e.currentDeselectedRowKeys).forEach((key) => {
        var rowData = this.investmentBranchDataSource.findItem(key);
        this.investmentBranchDataSource.updateItem({
          ...rowData,
          isSelected: false,
        });
      });
    }
    if (e.currentSelectedRowKeys.length > 0) {
      (<number[]>e.currentSelectedRowKeys).forEach((key) => {
        var rowData = this.investmentBranchDataSource.findItem(key);
        this.investmentBranchDataSource.updateItem({
          ...rowData,
          isSelected: true,
        });
      });
    }
  }
  isAllSelected = false;
  zoneChange(item) {
    this.searchZone = item == null ? 0 : item;
    this.searchArea = 0;
    this.filterBranch();
  }

  areaChange(item) {
    this.searchArea = item == null ? 0 : item;
    this.filterBranch();
  }

  searchChange(e) {
    this.searchChangeSubject.next(e);
  }
}
