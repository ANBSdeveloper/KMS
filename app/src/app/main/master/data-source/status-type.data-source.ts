import { Injectable, Injector } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { StatusType } from "./status-type.enum";

@Injectable()
export class StatusTypeDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: StatusType.UNREGISTERED,
        name: this.localizationService.get("status_type_unregistered"),
      },
      {
        id: StatusType.CREATED,
        name: this.localizationService.get("status_type_created"),
      },
      {
        id: StatusType.APPROVED,
        name: this.localizationService.get("status_type_approved"),
      },
      {
        id: StatusType.REGISTERED,
        name: this.localizationService.get("status_type_registered"),
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
