//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
//#endregion
@Component({
  selector: "app-ticket-combo",
  templateUrl: "./ticket-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: TicketComboComponent,
    },
  ],
})
export class TicketComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "code";
  @Input() valueName: string = "id";
  @Input() label: string = "ticket_code";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() dataSource = [];
  @Input() ticketInvestmentId = undefined;
  @Input() required = true;
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }
  //#endregion

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getTicket(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>().getTickets(
      this.ticketInvestmentId
    );
  }
  //#endregion
}
