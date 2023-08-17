import { Injectable, Injector } from "@angular/core";
import { LocalizationService } from "@cbms/ng-core-vuexy";

@Injectable()
export class RoleDataSource {
  localizationService: LocalizationService;
  assigmentRoleNames = ["ADMIN", "SA", "TA", "TM", "NSD", "MGCD", "TLCD", "ACD", "PG", "SUPPLY","MKT", "BDD"];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);
  }
}
