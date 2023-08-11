//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  UserDto,
  DataServiceProxy,
  UserListItemDto,
} from "@shared/services/data.service";
import { DxListComponentBase, ListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-user-list",
  templateUrl: "./user-list.component.html",
  styleUrls: ["./user-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class UserListComponent extends DxListComponentBase<UserDto> {
  //#region Variables
  entityName = "user";
  permissionName = "Users";
  codeProperty = "userName";
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
  showDetail(row: UserListItemDto) {
    this.router.navigate([`system/users/${row.id}`]);
  }
  create() {
    return this.router.navigate(["system/new-user"]);
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
    return this.getDataService<DataServiceProxy>().getUsers(
      isActive,
      this.cValue("roleId") ? this.cValue("roleId") : undefined,
      undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteUser(id);
  }
  //#endregion
}
