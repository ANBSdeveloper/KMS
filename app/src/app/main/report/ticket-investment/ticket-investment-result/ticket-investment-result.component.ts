//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { AuthenticationService } from "@app/auth/service";
import { TicketInvestmentASMStaffComboComponent } from "@app/main/investment/ticket-investment/ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component";
import { TicketInvestmentRSMStaffComboComponent } from "@app/main/investment/ticket-investment/ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component";
import { TicketInvestmentSSStaffComboComponent } from "@app/main/investment/ticket-investment/ticket-investment-ss-staff-combo/ticket-investment-ss-staff-combo.component";
import { RoleType } from "@app/main/system/role/role-type.enum";
import { FormComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import { StaffDto } from "@shared/services/data.service";
import moment from "moment";
import { ReportService } from "../../report.service";
//#endregion

@Component({
  selector: "report-ticket-investment-result",
  templateUrl: "./ticket-investment-result.component.html",
  styleUrls: ["./ticket-investment-result.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ReportTicketInvestmentResultComponent extends FormComponentBase {
  //#region Variables
  //#endregion
  contentHeader = {
    headerTitle: "report_ticket_investment_result",
    actionButton: false,
    breadcrumb: {
      type: "",
      links: [
        {
          name: "home",
          isLink: true,
          link: "/",
        },
        {
          name: "menu_report",
          isLink: false,
        },
        {
          name: "report_ticket_investment_result",
          isLink: true,
          link: "/report/ticket/result",
        },
      ],
    },
  };

  @ViewChild("rsmStaffCombo")
  rsmStaffCombo: TicketInvestmentRSMStaffComboComponent;
  @ViewChild("asmStaffCombo")
  asmStaffCombo: TicketInvestmentASMStaffComboComponent;
  @ViewChild("ssStaffCombo")
  ssStaffCombo: TicketInvestmentSSStaffComboComponent;

  staff: StaffDto;
  constructor(
    injector: Injector,
    private reportService: ReportService,
    private authService: AuthenticationService
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      fromDate: [moment().startOf("day").toDate(), [Validators.required]],
      toDate: [moment().endOf("day").toDate(), [Validators.required]],
      rsmStaffId: [undefined],
      asmStaffId: [undefined],
      ssStaffId: [undefined],
      leadUserId: [undefined],
    });

    this.dataService.getStaffInfo().subscribe((response) => {
      this.staff = response.result;
      this.mapToForm();
    });
  }
  //#region Form & Model
  mapToForm() {
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

    if (this.isCustomerDevelopmentLead) {
      this.c("leadUserId").setValue(this.authService.currentUser.id);
    }
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

  get isCustomerDevelopmentLead() {
    return (
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.CustomerDevelopmentLead
      ) != undefined
    );
  }

  get isCustomerDevelopmentUser() {
    return (
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.CustomerDevelopmentLead
      ) != undefined ||
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.CustomerDevelopmentAdmin
      ) != undefined ||
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.CustomerDevelopmentManager
      ) != undefined
    );
  }

  get isSystemUser() {
    return !this.isRsm && !this.isSS && !this.isAsm;
  }

  printPreview() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return;
    }

    this.reportService.openReport(
      this.l("report_ticket_investment_result"),
      "RP_TicketInvestment_Result",
      JSON.stringify({
        store: "RP_TicketInvestment_Result",
        storeParams: [
          {
            fromDate: this.cValue("fromDate"),
            toDate: this.cValue("toDate"),
            staffId: this.cValue("ssStaffId")
              ? this.cValue("ssStaffId")
              : this.cValue("asmStaffId")
              ? this.cValue("rsmStaffId")
              : undefined,
            leadUserId: this.cValue("leadUserId"),
          },
        ],
      })
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
  //#endregion
}
