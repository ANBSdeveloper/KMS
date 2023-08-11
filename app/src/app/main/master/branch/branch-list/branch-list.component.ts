//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { BranchDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-branch-list",
  templateUrl: "./branch-list.component.html",
  styleUrls: ["./branch-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class BranchListComponent extends DxListComponentBase<BranchDto> {
  //#region Variables
  entityName = "branch";
  permissionName = "Branches";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusActive: [undefined],
    });
    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getBranches(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  //#endregion
}
