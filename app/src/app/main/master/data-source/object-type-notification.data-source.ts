import { Injectable, Injector } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { ObjectTypeNotification } from "./object-type-notification.enum";

@Injectable()
export class ObjectTypeNotificationDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: ObjectTypeNotification.SHOP,
        name: this.localizationService.get("notification-object-type-shop"),
      },
      {
        id: ObjectTypeNotification.SALES,
        name: this.localizationService.get("notification-object-type-sales"),
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
