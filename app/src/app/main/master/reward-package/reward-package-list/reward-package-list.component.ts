
//#region Import
import { Component, Injector, ViewChild, ViewEncapsulation } from "@angular/core";
import {
  RewardPackageDto,
  DataServiceProxy,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
import { StatusActiveComboComponent } from "../../status-active-combo/status-active-combo.component";
//#endregion
@Component({
  selector: "app-reward-package-list",
  templateUrl: "./reward-package-list.component.html",
  styleUrls: ["./reward-package-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class RewardPackageListComponent extends DxListComponentBase<RewardPackageDto> {
  //#region Variables
  entityName = "reward_package";
  permissionName = "RewardPackages";
  isActive = true;
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
    var isActive = this.c("statusActive").value == '1'? true: this.c("statusActive").value == '0'? false: undefined;
    return this.getDataService<DataServiceProxy>().getRewardPackages(
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );    
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteRewardPackage(id);
  }
  //#endregion

  showDetail(row: RewardPackageDto) {
    this.router.navigate([`master/reward-package/${row.id}`]);
  }

  create() {
    this.router.navigate([`master/new-reward-package`]);
  } 
}
