import { Injectable, Injector } from "@angular/core";
import {
  LocalizationService,
  PermissionCheckerService,
} from "@cbms/ng-core-vuexy";
import { PosmInvestmentItemStatus } from "./posm-investment-status.enum";

@Injectable()
export class PosmInvestmentItemStatusDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: PosmInvestmentItemStatus.Request,
        name: this.localizationService.get("posm_investment_status_request"),
      },
      {
        id: PosmInvestmentItemStatus.AsmDeniedRequest,
        name: this.localizationService.get(
          "posm_investment_status_asm_denied_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.AsmApprovedRequest,
        name: this.localizationService.get(
          "posm_investment_status_asm_approved_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.RsmDeniedRequest,
        name: this.localizationService.get(
          "posm_investment_status_rsm_denied_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.RsmApprovedRequest,
        name: this.localizationService.get(
          "posm_investment_status_rsm_approved_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.TradeDeniedRequest,
        name: this.localizationService.get(
          "posm_investment_status_trade_denied_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.TradeApprovedRequest,
        name: this.localizationService.get(
          "posm_investment_status_trade_approved_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.DirectorDeniedRequest,
        name: this.localizationService.get(
          "posm_investment_status_director_denied_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.DirectorApprovedRequest,
        name: this.localizationService.get(
          "posm_investment_status_director_approved_request"
        ),
      },
      {
        id: PosmInvestmentItemStatus.InvalidOrder,
        name: this.localizationService.get(
          "posm_investment_status_invalid_order"
        ),
      },
      {
        id: PosmInvestmentItemStatus.SupSuggestedUpdateCost,
        name: this.localizationService.get(
          "posm_investment_status_suggest_budget"
        ),
      },
      {
        id: PosmInvestmentItemStatus.AsmConfirmedUpdateCost,
        name: this.localizationService.get(
          "posm_investment_status_asm_confirm_suggest"
        ),
      },
      {
        id: PosmInvestmentItemStatus.RsmConfirmedUpdateCost,
        name: this.localizationService.get(
          "posm_investment_status_rsm_confirm_suggest"
        ),
      },
      {
        id: PosmInvestmentItemStatus.TradeConfirmedUpdateCost,
        name: this.localizationService.get(
          "posm_investment_status_trade_confirm_suggest"
        ),
      },
      {
        id: PosmInvestmentItemStatus.ValidOrder,
        name: this.localizationService.get(
          "posm_investment_status_valid_order"
        ),
      },
      {
        id: PosmInvestmentItemStatus.ConfirmedProduce1,
        name: this.localizationService.get(
          "posm_investment_status_confirmed_produce1"
        ),
      },
      {
        id: PosmInvestmentItemStatus.ConfirmedProduce2,
        name: this.localizationService.get(
          "posm_investment_status_confirmed_produce2"
        ),
      },
      {
        id: PosmInvestmentItemStatus.ConfirmedVendorProduce,
        name: this.localizationService.get(
          "posm_investment_status_confirmed_vendor_produce"
        ),
      },
      {
        id: PosmInvestmentItemStatus.Accepted,
        name: this.localizationService.get("posm_investment_status_accepted"),
      },
      {
        id: PosmInvestmentItemStatus.ConfirmedAccept1,
        name: this.localizationService.get(
          "posm_investment_status_confirmed_accept1"
        ),
      },
      {
        id: PosmInvestmentItemStatus.ConfirmedAccept2,
        name: this.localizationService.get(
          "posm_investment_status_confirmed_accept2"
        ),
      },
    ];
  }

  findItem(value) {
    var item = this.items.find((p) => {
      return p.id == value;
    });
    return item;
  }

  getItemName(value) {
    const item = this.findItem(value);
    return item ? item.name : "";
  }
}
