import { Injectable } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

export enum PosmCalcType {
  WH = 1,
  HD = 2,
  WHD = 3,
  F = 4,
  Q = 5,
}

@Injectable()
export class PosmCalcTypeDataSource {
  items: { id: number; name: string }[] = [];
  constructor(localizationService: LocalizationService) {
    this.items = [
      {
        id: PosmCalcType.WH,
        name: localizationService.get("posm_calc_type_wh"),
      },
      {
        id: PosmCalcType.F,
        name: localizationService.get("posm_calc_type_sf"),
      },
      {
        id: PosmCalcType.WHD,
        name: localizationService.get("posm_calc_type_whd"),
      },
      {
        id: PosmCalcType.HD,
        name: localizationService.get("posm_calc_type_hd"),
      },
      {
        id: PosmCalcType.Q,
        name: localizationService.get("posm_calc_type_q"),
      },
    ];
  }

  findItem(id: number) {
    return this.items.find((p) => p.id == id);
  }
}
