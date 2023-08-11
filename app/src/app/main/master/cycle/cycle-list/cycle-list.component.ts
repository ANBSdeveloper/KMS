//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { CycleDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-cycle-list",
  templateUrl: "./cycle-list.component.html",
  styleUrls: ["./cycle-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CycleListComponent extends DxListComponentBase<CycleDto> {
  //#region Variables
  entityName = "cycle";
  sidebarName = "cycle_sidebar";
  permissionName = "Cycles";
  codeProperty = "number";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusActive: [undefined],
      search: [""],
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
    return this.getDataService<DataServiceProxy>().getCycles(
      isActive,
      undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteCycle(id);
  }
  //#endregion
}
