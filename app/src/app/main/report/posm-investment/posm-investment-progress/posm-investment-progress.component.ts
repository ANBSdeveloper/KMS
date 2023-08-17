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
  selector: "report-posm-investment-progress",
  templateUrl: "./posm-investment-progress.component.html",
  styleUrls: ["./posm-investment-progress.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ReportPosmInvestmentProgressComponent extends FormComponentBase {
  //#region Variables
  //#endregion
  contentHeader = {
    headerTitle: "report_posm_investment_progress",
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
          name: "report_posm_investment_progress",
          isLink: true,
          link: "/report/posm-investment/progress",
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

  get isSystemUser() {
    return !this.isRsm && !this.isSS && !this.isAsm;
  }

  printPreview() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return;
    }

    this.reportService.openReport(
      this.l("report_posm_investment_progress"),
      "RP_PosmInvestment_Progress",
      JSON.stringify({
        store: "RP_PosmInvestment_Progress",
        storeParams: [
          {
            fromDate: this.cValue("fromDate"),
            toDate: this.cValue("toDate"),
            staffId: this.cValue("ssStaffId")
              ? this.cValue("ssStaffId")
              : this.cValue("asmStaffId")
              ? this.cValue("rsmStaffId")
              : undefined
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
