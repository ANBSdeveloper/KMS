//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { TicketInvestmentStatus } from "@app/main/investment/data-source/ticket-investmen-status.enum";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { map } from "rxjs/operators";

//#endregion
@Component({
  selector: "app-ticket-investment-by-user-combo",
  templateUrl: "./ticket-investment-by-user-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: TicketInvestmentByUserComboComponent,
    },
  ],
})
export class TicketInvestmentByUserComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "shop";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() dataSource = [];
  arrayStatus=[TicketInvestmentStatus.Approved,TicketInvestmentStatus.Updating,TicketInvestmentStatus.Operated];
  //#endregion

  constructor(injector: Injector) {      
    super(injector);
  }
  //#endregion

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getTicketInvestment(
        this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getTicketInvestmentsByUser(
        this.arrayStatus,
        undefined,
        undefined,
        maxResult,
        skip,
        search,
        "",
        undefined
    ).pipe(
      map((res) => ({
        result: {
          items: res.result.items.map((p) => ({
            ...p,
            displayName: p.customerCode + " - " + p.customerName,
          })),
          totalCount: res.result.totalCount,
        },
      }))
    );
  }
  //#endregion
  
}


