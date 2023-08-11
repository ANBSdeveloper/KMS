//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { TicketInvestmentStatusDataSource } from "../../data-source/ticket-investmen-status.data-source";
//#endregion
@Component({
  selector: "app-ticket-investment-status-combo",
  templateUrl: "./ticket-investment-status-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: TicketInvestmentStatusComboComponent,
    },
  ],
})
export class TicketInvestmentStatusComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "ticket_investment_status";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  //#endregion

  constructor(
    injector: Injector,
    private investmenStatusDataSource: TicketInvestmentStatusDataSource
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
