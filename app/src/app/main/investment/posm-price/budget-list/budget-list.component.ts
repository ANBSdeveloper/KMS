//#region Import
import {
  Component,
  Injector,
  ViewEncapsulation,
} from "@angular/core";
import {
  BudgetDto,
  DataServiceProxy,
  BudgetListItemDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
import { InvestmentTypeDataSource } from "../../data-source/investment-type.data-source";
import { InvestmentType } from "../../data-source/investment-type.enum";
//#endregion
@Component({
  selector: "app-budget-list",
  templateUrl: "./budget-list.component.html",
  styleUrls: ["./budget-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class BudgetListComponent extends DxListComponentBase<BudgetDto> {
  //#region Variables
  entityName = "budget";
  permissionName = "Budgets";
  codeProperty = "budgetName";

  investmentType = InvestmentType;

  //#endregion
  constructor(
    injector: Injector,
    public investmentTypeDataSource: InvestmentTypeDataSource
  ) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      investmentType: [undefined],
    });

    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }
  //#region Actions
  showDetail(row: BudgetListItemDto) {
    this.router.navigate([`investment/budgets/${row.id}`]);
  }  

  create() {
    return this.router.navigate(["investment/new-budget"]);
  }
  //#endregion
  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var investmentType = this.c("investmentType").value
      ? this.c("investmentType").value
      : undefined;
    return this.getDataService<DataServiceProxy>().getBudgets(
      investmentType,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteBudget(id);
  }
  //#endregion
}
