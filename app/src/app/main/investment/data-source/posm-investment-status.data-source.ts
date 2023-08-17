import { Injectable, Injector } from "@angular/core";
import {
  LocalizationService,
  PermissionCheckerService,
} from "@cbms/ng-core-vuexy";
import { PosmInvestmentStatus } from "./posm-investment-status.enum";

@Injectable()
export class PosmInvestmentStatusDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: PosmInvestmentStatus.Request,
        name: this.localizationService.get("posm_investment_status_request"),
      },
      {
        id: PosmInvestmentStatus.ASMDeniedRequest,
        name: this.localizationService.get("posm_investment_status_asm_denied_request"),
      },
      {
        id: PosmInvestmentStatus.AsmApprovedRequest,
        name: this.localizationService.get("posm_investment_status_asm_approved_request"),
      },
      {
        id: PosmInvestmentStatus.RSMDeniedRequest,
        name: this.localizationService.get("posm_investment_status_rsm_denied_request"),
      },
      {
        id: PosmInvestmentStatus.RsmApprovedRequest,
        name: this.localizationService.get("posm_investment_status_rsm_approved_request"),
      },
      {
        id: PosmInvestmentStatus.TradeDeniedRequest,
        name: this.localizationService.get("posm_investment_status_trade_denied_request"),
      },
      {
        id: PosmInvestmentStatus.TradeApprovedRequest,
        name: this.localizationService.get("posm_investment_status_trade_approved_request"),
      },
      {
        id: PosmInvestmentStatus.DirectorDeniedRequest,
        name: this.localizationService.get("posm_investment_status_director_denied_request"),
      },
      {
        id: PosmInvestmentStatus.DirectorApprovedRequest,
        name: this.localizationService.get("posm_investment_status_director_approved_request"),
      },
      {
        id: PosmInvestmentStatus.InvalidOrder,
        name: this.localizationService.get("posm_investment_status_invalid_order"),
      },
      {
        id: PosmInvestmentStatus.ValidOrder,
        name: this.localizationService.get("posm_investment_status_valid_order"),
      },
      {
        id: PosmInvestmentStatus.ConfirmedProduce1,
        name: this.localizationService.get("posm_investment_status_confirmed_produce1"),
      },
      {
        id: PosmInvestmentStatus.ConfirmedProduce2,
        name: this.localizationService.get("posm_investment_status_confirmed_produce2"),
      },
      {
        id: PosmInvestmentStatus.ConfirmedVendorProduce,
        name: this.localizationService.get("posm_investment_status_confirmed_vendor_produce"),
      },
      {
        id: PosmInvestmentStatus.Accepted,
        name: this.localizationService.get("posm_investment_status_accepted"),
      },
      {
        id: PosmInvestmentStatus.ConfirmedAccept1,
        name: this.localizationService.get("posm_investment_status_confirmed_accept1"),
      },
      {
        id: PosmInvestmentStatus.ConfirmedAccept2,
        name: this.localizationService.get("posm_investment_status_confirmed_accept2"),
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
