//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { StatusTypeNotificationDataSource } from "../../data-source/status-type-notification.data-source";
//#endregion
@Component({
  selector: "app-status-type-notification-combo",
  templateUrl: "./status-type-notification-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: StatusTypeNotificationComboComponent,
    },
  ],
})
export class StatusTypeNotificationComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "status_type";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder: string = "";
  //#endregion

  constructor(
    injector: Injector,
    private statusDataSource: StatusTypeNotificationDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of({
      result: this.statusDataSource.findItem(this.value),
    });
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.statusDataSource.items,
        totalCount: this.statusDataSource.items.length,
      },
    });
  }
  //#endregion
}
