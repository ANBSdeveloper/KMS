//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { PosmInvestmentStatusDataSource } from "../../data-source/posm-investment-status.data-source";
//#endregion
@Component({
  selector: "app-posm-investment-status-combo",
  templateUrl: "./posm-investment-status-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmInvestmentStatusComboComponent,
    },
  ],
})
export class PosmInvestmentStatusComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "posm_investment_status";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  //#endregion

  constructor(
    injector: Injector,
    private investmenStatusDataSource: PosmInvestmentStatusDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of({
      result: this.investmenStatusDataSource.findItem(this.value),
    });
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.investmenStatusDataSource.items,
        totalCount: this.investmenStatusDataSource.items.length,
      },
    });
  }
  //#endregion
}
