//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { BudgetAllocateTypeDataSource } from "../../data-source/budget-allocate-type.data-source";
//#endregion
@Component({
  selector: "app-budget-allocate-type-combo",
  templateUrl: "./budget-allocate-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: BudgetAllocateTypeComboComponent,
    },
  ],
})
export class BudgetAllocateTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "budget_allocate_type";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private budgetAllocateTypeDataSource: BudgetAllocateTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of({
      result: this.budgetAllocateTypeDataSource.findItem(this.value),
    });
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.budgetAllocateTypeDataSource.items,
        totalCount: this.budgetAllocateTypeDataSource.items.length,
      },
    });
  }
  //#endregion
}
