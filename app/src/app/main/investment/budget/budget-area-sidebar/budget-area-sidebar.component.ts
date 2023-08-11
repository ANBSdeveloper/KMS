//#region Import
import { Component, Injector, Input, OnInit } from "@angular/core";
import {
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from "@angular/forms";
import {
  BudgetZoneDto,
  BudgetDto,
  BudgetAreaDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import {
  formatHelper,
  SidebarEditGridComponentBase,
} from "@cbms/ng-core-vuexy";
import { formatNumber } from "devextreme/localization";

//#endregion

//#region Validators
export function ownAmountValidator(func: () => number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (func() !== undefined && control.value && control.value > func())
      return {
        ownAmount: { ownAmount: func(), value: control.value },
      };
    return null;
  };
}

export function usedAmountValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (
      control.value &&
      control.value < control.parent.controls["usedAmount"].value
    )
      return {
        usedAmount: {
          usedAmount: control.parent.controls["usedAmount"].value,
          value: control.value,
        },
      };
    return null;
  };
}
//#endregion
@Component({
  selector: "app-budget-area-sidebar",
  templateUrl: "./budget-area-sidebar.component.html",
  styleUrls: ["./budget-area-sidebar.component.scss"],
})
export class BudgetAreaSidebarComponent
  extends SidebarEditGridComponentBase<BudgetAreaDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("budget_area_content_block") formBlockUI: NgBlockUI;

  @Input() budget: BudgetDto;
  @Input() ownAmount: number;

  entityName = "budget_area";
  sidebarName = "budget_area_sidebar";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      allocateAmount: [
        undefined,
        [
          Validators.required,
          Validators.min(0),
          ownAmountValidator(
            () =>
              this.ownAmount + (this.model.id ? this.model.allocateAmount : 0)
          ),
          usedAmountValidator(),
        ],
      ],
      remainAmount: [undefined, [Validators.required, Validators.min(0)]],
      usedAmount: [undefined, [Validators.required, Validators.min(0)]],
      tempRemainAmount: [undefined, [Validators.required, Validators.min(0)]],
      tempUsedAmount: [undefined, [Validators.required, Validators.min(0)]],
      areaName: [undefined, []],
      zoneName: [undefined, []],
    });

    this.formGroup.valueChanges.subscribe((value) => {
      if (
        this.c("remainAmount").value !=
        this.c("allocateAmount").value - this.c("usedAmount").value
      )
        this.c("remainAmount").setValue(
          this.c("allocateAmount").value - this.c("usedAmount").value
        );

      if (
        this.c("tempRemainAmount").value !=
        this.c("allocateAmount").value - this.c("tempUsedAmount").value
      )
        this.c("tempRemainAmount").setValue(
          this.c("allocateAmount").value - this.c("tempUsedAmount").value
        );
    });
  }

  mapModelToFormGroup() {
    super.mapModelToFormGroup();
  }

  mapFormGroupToSaveModel() {
    super.mapFormGroupToSaveModel();

    this.saveModel.allocateAmount = formatHelper.isEmptyOrNull(
      this.formGroup.controls["allocateAmount"].value
    )
      ? undefined
      : +this.formGroup.controls["allocateAmount"].value;
  }

  get newModel() {
    return new BudgetZoneDto({});
  }
  //#endregion

  //#region Form Elements
  get saveVisible(): boolean {
    return (
      ["Budgets", "Budgets.AllocateArea"].find((p) => this.isGranted(p)) !=
      undefined
    );
  }

  //#endregion

  //#region Languages
  get title() {
    return this.l("budget_allocate");
  }

  get unallocatedAmountFormatted(): string {
    return this.ownAmount ? formatNumber(this.ownAmount, "#,##0") : "";
  }
  //#endregion
}
