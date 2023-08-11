import { Injectable } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

export enum Status {
    InActive = 0,
    Active = 1
};

@Injectable()
export class StatusActiveDataSource {
    items: {id: number, name: string}[] = [];
    constructor(localizationService: LocalizationService) {
        this.items = [{
            id: Status.InActive,
            name: localizationService.get("inactive")
        }, {
            id: Status.Active,
            name: localizationService.get("active")
        }];
    }

    getItem(id: number){
        return this.items.find(p => p.id == id);
    }
}

