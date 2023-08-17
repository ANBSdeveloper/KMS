//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { PosmClassDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";

//#endregion
@Component({
  selector: "app-posm-class-list",
  templateUrl: "./posm-class-list.component.html",
  styleUrls: ["./posm-class-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmClassListComponent extends DxListComponentBase<PosmClassDto> {
  //#region Variables
  entityName = "posm_class";
  sidebarName = "posm_class_sidebar";
  permissionName = "PosmClasses";
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
    return this.getDataService<DataServiceProxy>().getPosmClasses(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deletePosmClass(id);
  }
  //#endregion
}
