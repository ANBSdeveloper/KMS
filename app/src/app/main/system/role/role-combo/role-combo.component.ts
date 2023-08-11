//#region Import
import { Component, Injector, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ComboComponentBase } from "@cbms/ng-core-vuexy";
import { DataServiceProxy } from "@shared/services/data.service";
import { filter, map } from "rxjs/operators";
import { RoleDataSource } from "../../data-source/role.data-source";
//#endregion
@Component({
  selector: "app-role-combo",
  templateUrl: "./role-combo.component.html",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: RoleComboComponent,
    },
  ],
})
export class RoleComboComponent extends ComboComponentBase {
  //#region Variables
  @Input() displayName: string = "displayName";
  @Input() valueName: string = "id";
  @Input() label: string = "role";
  @Input() readOnly: boolean = false;
  @Input() error: boolean = true;
  @Input() assignmentRole = true;
  @Input() placeholder = "";
  @Input() required = true;

  entityName = "role";
  sidebarName = "role_sidebar";
  permissionName = "Roles";

  assignmentRoles = this.roleDataSource.assigmentRoleNames;
  //#endregion

  constructor(injector: Injector, private roleDataSource: RoleDataSource) {
    super(injector);
  }

  //#region Api Request
  getRequest() {
    return this.getDataService<DataServiceProxy>().getRole(this.value);
  }

  getListRequest(maxResult: number, skip: number, search: string) {
    return this.getDataService<DataServiceProxy>()
      .getRoles(true, undefined, undefined, search, "", undefined)
      .pipe(
        map((res) => {
          if (this.assignmentRole) {
            res.result.items = res.result.items.filter((p) =>
              this.assignmentRoles.find(
                (a) => a.toUpperCase() == p.roleName.toUpperCase()
              )
            );
          }
          return res;
        })
      );
  }
  //#endregion
}
