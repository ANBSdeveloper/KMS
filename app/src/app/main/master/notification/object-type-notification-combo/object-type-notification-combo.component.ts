//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { of } from "rxjs";
import { ObjectTypeNotificationDataSource } from "../../data-source/object-type-notification.data-source";
//#endregion
@Component({
  selector: "app-object-type-notification-combo",
  templateUrl: "./object-type-notification-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ObjectTypeNotificationComboComponent,
    },
  ],
})
export class ObjectTypeNotificationComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "name";
  @Input() valueName: string = "id";
  @Input() label: string = "objectType";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = false;
  @Input() placeholder = "";
  @Input() required = true;
  //#endregion

  entityName = "notification";
  permissionName = "Notifications";

  constructor(
    injector: Injector,
    private objectDataSource: ObjectTypeNotificationDataSource
  ) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return of({
      result: this.objectDataSource.findItem(this.value),
    });
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return of({
      result: {
        items: this.objectDataSource.items,
        totalCount: this.objectDataSource.items.length,
      },
    });
  }
  //#endregion
}
