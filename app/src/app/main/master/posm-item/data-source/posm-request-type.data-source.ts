import { Injectable } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

export enum PosmRequestType {
  New = 1,
  Require = 2
}

@Injectable()
export class PosmRequestTypeDataSource {
  items: { id: number; name: string }[] = [];
  constructor(localizationService: LocalizationService) {
    this.items = [
      {
        id: PosmRequestType.New,
        name: localizationService.get("posm_request_type_new"),
      },
      {
        id: PosmRequestType.Require,
        name: localizationService.get("posm_request_type_repair"),
      }
    ];
  }

  findItem(id: number) {
    return this.items.find((p) => p.id == id);
  }
}
