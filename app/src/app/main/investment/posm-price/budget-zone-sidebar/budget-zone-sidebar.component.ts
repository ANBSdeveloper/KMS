//#region Import
import { Component, Injector, Input, OnInit } from "@angular/core";
import {
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from "@angular/forms";
import { BudgetZoneDto, BudgetDto } from "@shared/services/data.service";
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
  selector: "app-budget-zone-sidebar",
  templateUrl: "./budget-zone-sidebar.component.html",
  styleUrls: ["./budget-zone-sidebar.component.scss"],
})
export class BudgetZoneSidebarComponent
  extends SidebarEditGridComponentBase<BudgetZoneDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("budget_zone_content_block") formBlockUI: NgBlockUI;

  @Input() budget: BudgetDto;
  @Input() ownAmount: number;

  entityName = "budget_zone";
  sidebarName = "budget_zone_sidebar";
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
    return ["Budgets"].find((p) => this.isGranted(p)) != undefined;
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
