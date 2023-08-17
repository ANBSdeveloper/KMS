//#region Import
import {
  Component,
  Injector,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { AuthenticationService } from "@app/auth/service";
import { FormComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import { StaffDto } from "@shared/services/data.service";
import { ReportService } from "../../report.service";
//#endregion

@Component({
  selector: "report-posm-investment-budget",
  templateUrl: "./posm-investment-budget.component.html",
  styleUrls: ["./posm-investment-budget.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ReportPosmInvestmentBudgetComponent extends FormComponentBase {
  //#region Variables
  //#endregion
  contentHeader = {
    headerTitle: "report_posm_investment_budget",
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
          name: "report_posm_investment_budget",
          isLink: true,
          link: "/report/posm-investment/budget",
        },
      ],
    },
  };


  staff: StaffDto;
  constructor(
    injector: Injector,
    private reportService: ReportService,
    private authService: AuthenticationService
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      cycleId: [undefined, [Validators.required]],
      zoneId: [undefined],
    });

    this.dataService.getStaffInfo().subscribe((response) => {
      this.staff = response.result;
      this.mapToForm();
    });
  }
  //#region Form & Model
  mapToForm() {
    
  }

  printPreview() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return;
    }

    this.reportService.openReport(
      this.l("report_posm_investment_budget"),
      "RP_PosmInvestment_Budget",
      JSON.stringify({
        store: "RP_PosmInvestment_Budget",
        storeParams: [
          {
            cycleId: this.cValue("cycleId"),
            zoneId: this.cValue("zoneId"),
          },
        ],
      })
    );
  }

  //#endregion
}
