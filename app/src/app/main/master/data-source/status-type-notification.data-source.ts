import { Injectable, Injector } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { StatusTypeNotification } from "./status-type-notification.enum";

@Injectable()
export class StatusTypeNotificationDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: StatusTypeNotification.HOLDING,
        name: this.localizationService.get("status_type_notification_holding"),
      },
      {
        id: StatusTypeNotification.SENDING,
        name: this.localizationService.get("status_type_notification_sending"),
      },
      {
        id: StatusTypeNotification.SENDED,
        name: this.localizationService.get("status_type_notification_sended"),
      },
      {
        id: StatusTypeNotification.CANCELED,
        name: this.localizationService.get("status_type_notification_canceled"),
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
