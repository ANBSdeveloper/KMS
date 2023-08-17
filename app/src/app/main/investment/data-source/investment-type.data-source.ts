import { Injectable, Injector } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { InvestmentType } from "./investment-type.enum";

@Injectable()
export class InvestmentTypeDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: InvestmentType.BTTT,
        name: this.localizationService.get("investment_type_bttt"),
      },
      {
        id: InvestmentType.POSM,
        name: this.localizationService.get("investment_type_posm"),
      },
    ];
  }

  findItem(value) {
    return this.items.find((p) => p.id == value);
  }

  getItemName(value) {
    const item = this.findItem(value);
    return item ? item.name : "";
  }
}
