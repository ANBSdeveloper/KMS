import { Injectable, Injector } from "@angular/core";
import {
  LocalizationService,
  PermissionCheckerService,
} from "@cbms/ng-core-vuexy";
import { TicketInvestmentStatus } from "./ticket-investmen-status.enum";

@Injectable()
export class TicketInvestmentStatusDataSource {
  localizationService: LocalizationService;
  items: any[];
  constructor(injector: Injector) {
    this.localizationService = injector.get(LocalizationService);

    this.items = [
      {
        id: TicketInvestmentStatus.RequestInvestment,
        name: this.localizationService.get("ticket_investment_status_request_investment"),
      },
      {
        id: TicketInvestmentStatus.DeniedRequestInvestment,
        name: this.localizationService.get("ticket_investment_status_denied_request_investment"),
      },
      {
        id: TicketInvestmentStatus.ConfirmedRequestInvestment,
        name: this.localizationService.get("ticket_investment_status_confirmed_request_investment"),
      },
      {
        id: TicketInvestmentStatus.ValidRequestInvestment1,
        name: this.localizationService.get("ticket_investment_status_valid_request_investment1"),
      },
      {
        id: TicketInvestmentStatus.InValidRequestInvestment1,
        name: this.localizationService.get("ticket_investment_status_in_valid_request_investment1"),
      },
      {
        id: TicketInvestmentStatus.ValidRequestInvestment2,
        name: this.localizationService.get("ticket_investment_status_valid_request_investment2"),
      },
      {
        id: TicketInvestmentStatus.InValidRequestInvestment2,
        name: this.localizationService.get("ticket_investment_status_in_valid_request_investment2"),
      },
      {
        id: TicketInvestmentStatus.ConfirmedInvestment,
        name: this.localizationService.get("ticket_investment_status_confirmed_investment"),
      },
      {
        id: TicketInvestmentStatus.DeniedInvestmentConfirmation,
        name: this.localizationService.get("ticket_investment_status_denied_investment_confirmation"),
      },
      {
        id: TicketInvestmentStatus.ApproveInvestment,
        name: this.localizationService.get("ticket_investment_status_approve_investment1"),
      },
      {
        id: TicketInvestmentStatus.DeniedInvestmentApproval,
        name: this.localizationService.get("ticket_investment_status_denied_investment_approval"),
      },
      {
        id: TicketInvestmentStatus.Approved,
        name: this.localizationService.get("ticket_investment_status_approved"),
      },
      {
        id: TicketInvestmentStatus.Denied,
        name: this.localizationService.get("ticket_investment_status_denied"),
      },
      {
        id: TicketInvestmentStatus.Updating,
        name: this.localizationService.get("ticket_investment_status_updating"),
      },
      {
        id: TicketInvestmentStatus.Operated,
        name: this.localizationService.get("ticket_investment_status_operated"),
      },
      {
        id: TicketInvestmentStatus.Acceptance,
        name: this.localizationService.get("ticket_investment_status_acceptance"),
      },
      {
        id: TicketInvestmentStatus.FinalSettlement,
        name: this.localizationService.get("ticket_investment_status_final_settlement"),
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
