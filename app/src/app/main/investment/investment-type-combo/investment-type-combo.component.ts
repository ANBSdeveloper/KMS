//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { InvestmentTypeDataSource } from "../data-source/investment-type.data-source";
//#endregion
@Component({
  selector: "app-investment-type-combo",
  templateUrl: "./investment-type-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: InvestmentTypeComboComponent,
    },
  ],
})
export class InvestmentTypeComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "investment_type";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() required = true;
  //#endregion

  constructor(
    injector: Injector,
    private roleDataSource: InvestmentTypeDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of({
      result: this.roleDataSource.findItem(this.value),
    });
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.roleDataSource.items,
        totalCount: this.roleDataSource.items.length,
      },
    });
  }
  //#endregion
}
