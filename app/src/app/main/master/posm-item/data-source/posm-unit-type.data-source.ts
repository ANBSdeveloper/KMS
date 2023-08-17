import { Injectable } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

export enum PosmUnitType {
  PCS = 1,
  M2 = 2,
  M = 3,
}

@Injectable()
export class PosmUnitTypeDataSource {
  items: { id: number; name: string }[] = [];
  constructor(localizationService: LocalizationService) {
    this.items = [
      {
        id: PosmUnitType.PCS,
        name: localizationService.get("posm_unit_type_pcs"),
      },
      {
        id: PosmUnitType.M,
        name: localizationService.get("posm_unit_type_m"),
      },
      {
        id: PosmUnitType.M2,
        name: localizationService.get("posm_unit_type_m2"),
      },
    ];
  }

  findItem(id: number) {
    return this.items.find((p) => p.id == id);
  }
}
