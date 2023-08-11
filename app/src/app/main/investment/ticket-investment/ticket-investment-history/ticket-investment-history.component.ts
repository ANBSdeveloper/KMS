//#region Import
import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";

import { PermissionComponentBase } from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  StaffDto,
  TicketInvestmentHistoryDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { Observable } from "rxjs";
import { filter, finalize, map } from "rxjs/operators";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
//#endregion

@Component({
  selector: "app-ticket-investment-history",
  templateUrl: "./ticket-investment-history.component.html",
  styleUrls: ["./ticket-investment-history.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketInvestmentHistoryComponent extends PermissionComponentBase {
  @Input() set id(value) {
    this._value = value;
    this.refresh();
  }

  @BlockUI("ticket_investment_history_content_block") blockUI: NgBlockUI;
  _value: number;
  items: TicketInvestmentHistoryDto[] = [];

  constructor(injector: Injector, private dataService: DataServiceProxy) {
    super(injector);
  }

  isRequest(item: TicketInvestmentHistoryDto) {
    return item.status == TicketInvestmentStatus.RequestInvestment;
  }
  cache = {};
  cacheStaff = {};
  cacheCustomer = {};

  refresh() {
    if (this._value) {
      this.blockUI.start();
      this.dataService
        .getTicketInvestmentHistory(this._value)
        .pipe(finalize(() => this.blockUI.stop()))
        .subscribe((response) => {
          this.items = response.result.filter(
            (p) => p.status <= TicketInvestmentStatus.Denied
          );
        });
    } else {
      this.items = [];
    }
  }
  getData(item: TicketInvestmentHistoryDto) {
    if (!this.cache[item.id]) {
      this.cache[item.id] = JSON.parse(item.data);
    }
    return this.cache[item.id];
  }

  getStaff(id): Observable<StaffDto> {
    if (!this.cacheStaff[id]) {
      this.cacheStaff[id] = this.dataService.getStaff(id).pipe(
        filter((item) => item != undefined),
        map((item) => item.result.code + " - " + item.result.name)
      );
    }
    return this.cacheStaff[id];
  }

  getCustomer(id): Observable<string> {
    if (!this.cacheCustomer[id]) {
      this.cacheCustomer[id] = this.dataService
        .getCustomer(id)
        .pipe(map((item) => item.result.name));
    }
    return this.cacheCustomer[id];
  }

  getTimePointCss(data) {
    if (data.Status == TicketInvestmentStatus.ConfirmedRequestInvestment)
      return "timeline-point-warning";
    if (data.Status == TicketInvestmentStatus.DeniedRequestInvestment)
      return "timeline-point-danger";
    if (data.Status == TicketInvestmentStatus.ValidRequestInvestment1)
      return "timeline-point-warning";
    if (data.Status == TicketInvestmentStatus.InValidRequestInvestment1)
      return "timeline-point-danger";
    if (data.Status == TicketInvestmentStatus.ValidRequestInvestment2)
      return "timeline-point-warning";
    if (data.Status == TicketInvestmentStatus.InValidRequestInvestment2)
      return "timeline-point-danger";
    if (data.Status == TicketInvestmentStatus.ConfirmedInvestment)
      return "timeline-point-warning";
    if (data.Status == TicketInvestmentStatus.DeniedInvestmentConfirmation)
      return "timeline-point-danger";
    if (data.Status == TicketInvestmentStatus.ApproveInvestment)
      return "timeline-point-warning";
    if (data.Status == TicketInvestmentStatus.DeniedInvestmentApproval)
      return "timeline-point-danger";
    if (data.Status == TicketInvestmentStatus.Approved)
      return "timeline-point-success";
    if (data.Status == TicketInvestmentStatus.Denied)
      return "timeline-point-danger";
  }
  getApproveTitle(data) {
    if (data.Status == TicketInvestmentStatus.ConfirmedRequestInvestment)
      return "Xác Nhận Yêu Cầu";
    if (data.Status == TicketInvestmentStatus.DeniedRequestInvestment)
      return "Từ Chối Yêu Cầu";
    if (data.Status == TicketInvestmentStatus.ValidRequestInvestment1)
      return "Xác Nhận PYC Hợp Lệ 1";
    if (data.Status == TicketInvestmentStatus.InValidRequestInvestment1)
      return "Từ Chối PYC Hợp Lệ 1";
    if (data.Status == TicketInvestmentStatus.ValidRequestInvestment2)
      return "Xác Nhận PYC Hợp Lệ 2";
    if (data.Status == TicketInvestmentStatus.InValidRequestInvestment2)
      return "Từ Chối PYC Hợp Lệ 2";
    if (data.Status == TicketInvestmentStatus.ConfirmedInvestment)
      return "Xác Nhận Đầu Tư";
    if (data.Status == TicketInvestmentStatus.DeniedInvestmentConfirmation)
      return "Từ Chối Đầu Tư";
    if (data.Status == TicketInvestmentStatus.ApproveInvestment)
      return "Duyệt Đầu Tư";
    if (data.Status == TicketInvestmentStatus.DeniedInvestmentApproval)
      return "Từ Chối Duyệt Đầu Tư";
    if (data.Status == TicketInvestmentStatus.Approved) return "Duyệt";
    if (data.Status == TicketInvestmentStatus.Denied) return "Từ Chối";
  }
}
