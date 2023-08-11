//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { of } from "rxjs";
import { map } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-ticket-investment-ss-staff-combo",
  templateUrl: "./ticket-investment-ss-staff-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: TicketInvestmentSSStaffComboComponent,
    },
  ],
})
export class TicketInvestmentSSStaffComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "ss_staff";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() dataSource = [];
  @Input() supervisorId = undefined;
  @Input() getDataWithoutSupervisor = false;
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }
  //#endregion

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>()
      .getStaff(this.value)
      .pipe(
        map((res) => ({
          result: {
            ...res.result,
            displayName: res.result.code + " - " + res.result.name,
          },
        }))
      );
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    if (this.supervisorId || this.getDataWithoutSupervisor) {
      return this.getDataService<DataServiceProxy>()
        .getSalesSuppervisorStaffs(
          this.supervisorId ?? undefined,
          maxResult,
          skip,
          search,
          "",
          undefined
        )
        .pipe(
          map((res) => ({
            result: {
              items: res.result.items.map((p) => ({
                ...p,
                displayName: p.code + " - " + p.name,
              })),
              totalCount: res.result.totalCount,
            },
          }))
        );
    } else {
      return of({
        result: {
          items: [],
          totalCount: 0,
        },
      });
    }
  }
  //#endregion
}
