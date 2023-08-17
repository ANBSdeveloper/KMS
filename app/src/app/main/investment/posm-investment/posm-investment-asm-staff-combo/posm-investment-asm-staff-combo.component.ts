//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { of } from "rxjs";
import { map } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-posm-investment-asm-staff-combo",
  templateUrl: "./posm-investment-asm-staff-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PosmInvestmentASMStaffComboComponent,
    },
  ],
})
export class PosmInvestmentASMStaffComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "asm_staff";
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
    const workingDescr = this.l("on_working");
    const offDescr = this.l("off_working");
    if (this.supervisorId || this.getDataWithoutSupervisor) {
      return this.getDataService<DataServiceProxy>()
        .getAsmStaffs(
          this.supervisorId ?? undefined,
          maxResult,
          skip,
          search,
          "[{'name': 'IsActive', 'direction': 'desc'}, {'name': 'Code', 'direction': 'asc'}]",
          undefined
        )
        .pipe(
          map((res) => ({
            result: {
              items: res.result.items.map((p) => ({
                ...p,
                displayName: p.code + " - " + p.name + " - " + (p.isActive ? workingDescr : offDescr),
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
