//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  RoleDto,
  DataServiceProxy,
  RoleListItemDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-role-list",
  templateUrl: "./role-list.component.html",
  styleUrls: ["./role-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class RoleListComponent extends DxListComponentBase<RoleDto> {
  //#region Variables
  entityName = "role";
  permissionName = "Roles";
  codeProperty = "roleName";
  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusActive: [undefined],
      roleId: [undefined],
    });

    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });

    super.init();
  }
  //#region Actions
  showDetail(row: RoleListItemDto) {
    this.router.navigate([`system/roles/${row.id}`]);
  }
  create() {
    return this.router.navigate(["system/new-role"]);
  }
  //#endregion
  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getRoles(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteRole(id);
  }
  //#endregion
}
