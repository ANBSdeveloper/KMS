import { Injectable } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

export enum RewardType {
    BTTT = 1,
    GV = 2
};

@Injectable()
export class RewardTypeDataSource {
    items: {id: number, name: string}[] = [];
    constructor(localizationService: LocalizationService) {
        this.items = [{
            id: RewardType.BTTT,
            name: localizationService.get("reward_type_bttt")
        }, {
            id: RewardType.GV,
            name: localizationService.get("reward_type_gv")
        }];
    }

    getItem(id: number){
        return this.items.find(p => p.id == id);
    }
}

