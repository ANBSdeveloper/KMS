import { Injectable, Injector } from "@angular/core";
import {
  LocalizationService,
  PermissionCheckerService,
} from "@cbms/ng-core-vuexy";
import { BudgetAllocateType } from "./budget-allocate-type.enum";

@Injectable()
export class BudgetAllocateTypeDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);
    const permissionService = injector.get(PermissionCheckerService);

    if (permissionService.isGranted("Budgets")) {
      this.items = [
        {
          id: BudgetAllocateType.Zone,
          name: this.localizationService.get("budget_allocate_type_zone"),
        },
        {
          id: BudgetAllocateType.Area,
          name: this.localizationService.get("budget_allocate_type_area"),
        },
        {
          id: BudgetAllocateType.Branch,
          name: this.localizationService.get("budget_allocate_type_branch"),
        },
      ];
    } else if (permissionService.isGranted("Budgets.AllocateArea")) {
      this.items = [
        {
          id: BudgetAllocateType.Area,
          name: this.localizationService.get("budget_allocate_type_area"),
        },
        {
          id: BudgetAllocateType.Branch,
          name: this.localizationService.get("budget_allocate_type_branch"),
        },
      ];
    } else if (permissionService.isGranted("Budgets.AllocateBranch")) {
      this.items = [
        {
          id: BudgetAllocateType.Branch,
          name: this.localizationService.get("budget_allocate_type_branch"),
        },
      ];
    }
  }

  findItem(value) {
    return this.items.find((p) => p.id == value);
  }

  getItemName(value) {
    const item = this.findItem(value);
    return item ? item.name : "";
  }
}
