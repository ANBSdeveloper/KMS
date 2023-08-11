import { AuthenticationService } from "./../../../../auth/service/authentication.service";
import {
  DataServiceProxy,
  TicketInvestmentDto,
  StaffDto,
} from "./../../../../../shared/services/data.service";
//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { TicketInvestmentRSMStaffComboComponent } from "../ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component";
import { TicketInvestmentASMStaffComboComponent } from "../ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component";
import { TicketInvestmentSSStaffComboComponent } from "../ticket-investment-ss-staff-combo/ticket-investment-ss-staff-combo.component";
import { TicketInvestmentStatusDataSource } from "../../data-source/ticket-investmen-status.data-source";
import { debounceTime, map, takeUntil } from "rxjs/operators";
import { TicketInvestmentStatusComboComponent } from "../ticket-investment-status-combo/ticket-investment-status-combo.component";
import moment from "moment";
import { RoleType } from "@app/main/system/role/role-type.enum";
import { Validators } from "@angular/forms";
import { of } from "rxjs";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import { Workbook } from "exceljs";
import { saveAs } from "file-saver";
import { exportDataGrid } from "devextreme/excel_exporter";
//#endregion
@Component({
  selector: "app-ticket-investment-list",
  templateUrl: "./ticket-investment-list.component.html",
  styleUrls: ["./ticket-investment-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketInvestmentListComponent extends DxListComponentBase<TicketInvestmentDto> {
  [x: string]: any;
  //#region Variables
  entityName = "ticketInvestment";
  permissionName = "TicketInvestments";
  @ViewChild("rsmStaffCombo")
  rsmStaffCombo: TicketInvestmentRSMStaffComboComponent;
  @ViewChild("asmStaffCombo")
  asmStaffCombo: TicketInvestmentASMStaffComboComponent;
  @ViewChild("ssStaffCombo")
  ssStaffCombo: TicketInvestmentSSStaffComboComponent;
  @ViewChild("ticketInvestmenStatus")
  ticketInvestmenStatus: TicketInvestmentStatusComboComponent;
  statusDefault = undefined;
  staff: StaffDto;
  //#endregion
  //#region Form
  configForm() {
    this.formGroup = this.fb.group({
      fromDate: [undefined, [Validators.required]],
      toDate: [undefined, [Validators.required]],
    });
  }
  constructor(
    injector: Injector,
    public ticketInvestmentStatusDataSource: TicketInvestmentStatusDataSource,
    private authService: AuthenticationService
  ) {
    super(injector);

    if (this.authService.currentUser.roles[0] == RoleType.Rsm) {
      this.statusDefault = TicketInvestmentStatus.ValidRequestInvestment2;
    } else if (this.authService.currentUser.roles[0] == RoleType.Asm) {
      this.statusDefault = TicketInvestmentStatus.RequestInvestment;
    } else if (
      this.authService.currentUser.roles[0] == RoleType.SalesDirector
    ) {
      this.statusDefault = TicketInvestmentStatus.ApproveInvestment;
    }

    this.authService.currentUser.roles;
  }

  //#endregion
  init() {
    this.filterFormGroup = this.fb.group({
      fromDate: [moment().add(-1, "month").toDate(), [Validators.required]],
      toDate: [moment().toDate(), [Validators.required]],
      rsmStaffId: [undefined],
      asmStaffId: [undefined],
      ssStaffId: [undefined],
      status: [this.statusDefault],
    });

    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });

    this.getDataService<DataServiceProxy>()
      .getStaffInfo()
      .subscribe((response) => {
        this.staff = response.result;

        if (this.staff != undefined) {
          if (this.isRsm) {
            this.c("rsmStaffId").setValue(this.staff.id);
            this.asmStaffCombo.value = undefined;
            setTimeout(() => {
              this.asmStaffCombo.loadData();
            }, 50);
          } else if (this.isAsm) {
            this.c("asmStaffId").setValue(this.staff.id);
            this.ssStaffCombo.value = undefined;
            setTimeout(() => {
              this.ssStaffCombo.loadData();
            }, 50);
          } else if (this.ssStaffCombo) {
            this.c("ssStaffId").setValue(this.staff.id);
          }
        }
      });

    super.init();
  }

  get isSystemUser() {
    return !this.isRsm && !this.isSS && !this.isAsm;
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var status = this.c("status").value ? this.c("status").value : "";
    var rsmStaff = this.c("rsmStaffId").value ? this.c("rsmStaffId").value : "";
    var asmStaff = this.c("asmStaffId").value ? this.c("asmStaffId").value : "";
    var ssStaff = this.c("ssStaffId").value ? this.c("ssStaffId").value : "";
    var fromDate = this.c("fromDate").value
      ? this.c("fromDate").value
      : undefined;
    var toDate = this.c("toDate").value ? this.c("toDate").value : undefined;
    if (fromDate != undefined && toDate != undefined) {
      return this.getDataService<DataServiceProxy>()
        .getTicketInvestmentsByTime(
          status ? [status] : undefined,
          rsmStaff,
          asmStaff,
          ssStaff,
          fromDate,
          toDate,
          undefined,
          pageSize,
          skip,
          search,
          sort,
          undefined
        )
        .pipe(
          map((res) => ({
            ...res,
            result: {
              ...res.result,
              items: res.result.items.map((item) => ({
                ...item,
                status: this.statusNameFromId(item.status),
              })),
            },
          }))
        );
    } else {
      return of({
        result: {
          items: [],
          totalCount: 0,
        },
      });
    }
  }

  statusNameFromId(status: number) {
    return this.l(
      status == 10
        ? "ticket_investment_status_request_investment"
        : status == 20
        ? "ticket_investment_status_denied_request_investment"
        : status == 30
        ? "ticket_investment_status_confirmed_request_investment"
        : status == 40
        ? "ticket_investment_status_valid_request_investment1"
        : status == 50
        ? "ticket_investment_status_in_valid_request_investment1"
        : status == 60
        ? "ticket_investment_status_valid_request_investment2"
        : status == 70
        ? "ticket_investment_status_in_valid_request_investment2"
        : status == 80
        ? "ticket_investment_status_confirmed_investment"
        : status == 90
        ? "ticket_investment_status_denied_investment_confirmation"
        : status == 100
        ? "ticket_investment_status_approve_investment1"
        : status == 110
        ? "ticket_investment_status_denied_investment_approval"
        : status == 120
        ? "ticket_investment_status_approved"
        : status == 130
        ? "ticket_investment_status_denied"
        : status == 140
        ? "ticket_investment_status_updating"
        : status == 150
        ? "ticket_investment_status_operated"
        : status == 160
        ? "ticket_investment_status_acceptance"
        : status == 170
        ? "ticket_investment_status_final_settlement"
        : ""
    );
  }
  rsmStaffChange(record) {
    this.asmStaffCombo.value = undefined;
    this.ssStaffCombo.value = undefined;
    setTimeout(() => {
      this.asmStaffCombo.loadData();
      this.ssStaffCombo.loadData();
    }, 50);
  }
  asmStaffChange(record) {
    this.ssStaffCombo.value = undefined;
    setTimeout(() => {
      this.ssStaffCombo.loadData();
    }, 50);
  }

  toDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("toDate").setValue(undefined);
        this.messageService.toastError(this.l("error_todate"));
      }
    }
  }

  fromDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("fromDate").setValue(undefined);
        this.messageService.toastError(this.l("error_fromdate"));
      }
    }
  }
  //#endregion

  get registerVisible(): boolean {
    return (
      (this.isGranted(this.permissionName) ||
        this.isGranted("TicketInvestments.Register")) &&
      this.staff != undefined
    );
  }

  register() {
    this.router.navigate([`investment/register-ticket-investment`]);
  }

  showDetail(data: TicketInvestmentDto) {
    this.router.navigate([`investment/ticket-investments/${data.id}`]);
  }

  export() {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet("Sheet 1");

    exportDataGrid({
      component: this.dataGrid.instance,
      worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(
          new Blob([buffer], { type: "application/octet-stream" }),
          "DataGrid.xlsx"
        );
      });
    });
  }

  get isRsm() {
    return (
      this.authService.currentUser.roles.find((p) => p == RoleType.Rsm) !=
      undefined
    );
  }

  get isAsm() {
    return (
      this.authService.currentUser.roles.find((p) => p == RoleType.Asm) !=
      undefined
    );
  }

  get isSS() {
    return (
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.SalesSupervisor
      ) != undefined
    );
  }
}
