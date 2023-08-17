//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { PosmTypeDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";

//#endregion
@Component({
  selector: "app-posm-type-list",
  templateUrl: "./posm-type-list.component.html",
  styleUrls: ["./posm-type-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmTypeListComponent extends DxListComponentBase<PosmTypeDto> {
  //#region Variables
  entityName = "posm_type";
  sidebarName = "posm_type_sidebar";
  permissionName = "PosmTypes";
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
    return this.getDataService<DataServiceProxy>().getPosmTypes(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deletePosmType(id);
  }
  //#endregion
}
