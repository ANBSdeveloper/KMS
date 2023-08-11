//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
//#endregion
@Component({
  selector: "app-budget-area-combo",
  templateUrl: "./budget-area-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: BudgetAreaComboComponent,
    },
  ],
})
export class BudgetAreaComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "areaId";
  @Input() label: string = "area";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  @Input() dataSource = [];
  @Input() required = true;
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }
  //#endregion

  // must override to clear base code
  ngOnInit(): void {}

  getRequest() {
    return of({
      result: this.dataSource.find((p) => p.areaId == this.value),
    });
  }
}
