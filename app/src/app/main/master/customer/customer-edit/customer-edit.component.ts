import {
  CustomerRecentSalesDto,
  MonthData,
  TicketInvestmentListItemDto,
} from "./../../../../../shared/services/data.service";
import { WardComboComponent } from "./../../ward/ward-combo/ward-combo.component";
import { DistrictComboComponent } from "./../../district/district-combo/district-combo.component";
import { ProvinceComboComponent } from "./../../province/province-combo/province-combo.component";
import { ZoneComboComponent } from "./../../zone/zone-combo/zone-combo.component";
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  BudgetUpsertDto,
  CustomerDto,
} from "@shared/services/data.service";
import { Validators } from "@angular/forms";
import {
  DatatableDataSource,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { AuthenticationService } from "@app/auth/service";
import { Subject } from "rxjs";
import { BranchComboComponent } from "../../branch/branch-combo/branch-combo.component";
import { SSStaffComboComponent } from "../../staff/ss-staff-combo/ss-staff-combo.component";
import { AreaComboComponent } from "../../area/area-combo/area-combo.component";
import { RSMStaffComboComponent } from "../../staff/rsm-staff-combo/rsm-staff-combo.component";
import { ASMStaffComboComponent } from "../../staff/asm-staff-combo/asm-staff-combo.component";
import moment from "moment";
import { finalize } from "rxjs/operators";
//#endregion

@Component({
  selector: "app-customer-edit",
  templateUrl: "./customer-edit.component.html",
  styleUrls: ["./customer-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerEditComponent extends PageEditFormComponentBase<
  CustomerDto,
  undefined,
  undefined
> {
  //#region Variables
  @ViewChild("branchCombo") branchCombo: BranchComboComponent;
  @ViewChild("rsmStaffCombo") rsmStaffCombo: RSMStaffComboComponent;
  @ViewChild("asmStaffCombo") asmStaffCombo: ASMStaffComboComponent;
  @ViewChild("ssStaffCombo") ssStaffCombo: SSStaffComboComponent;
  @ViewChild("zoneCombo") zoneCombo: ZoneComboComponent;
  @ViewChild("areaCombo") areaCombo: AreaComboComponent;
  @ViewChild("provinceCombo") provinceCombo: ProvinceComboComponent;
  @ViewChild("districtCombo") districtCombo: DistrictComboComponent;
  @ViewChild("wardCombo") wardCombo: WardComboComponent;

  entityName = "customer";
  permissionName = "Customers";
  codeProperty = "customerName";
  createUrl = "customer/new-customer";

  searchChangeSubject = new Subject<string>();
  customerRecentSalesDataSource =
    new DatatableDataSource<CustomerRecentSalesDto>();
  ticketInvestmentListItemDataSource =
    new DatatableDataSource<TicketInvestmentListItemDto>();

  totalMonth = 0;
  //#endregion

  constructor(
    injector: Injector,
    private authenticatonService: AuthenticationService
  ) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      code: [
        undefined,
        [
          Validators.required,
          Validators.maxLength(50),
          Validators.minLength(2),
        ],
      ],
      name: [undefined, [Validators.required, Validators.maxLength(200)]],
      branchId: [undefined, [Validators.required]],
      isActive: [undefined, []],
      salesSupervisorStaffId: [undefined, []],
      channelCode: [undefined, []],
      channelName: [undefined, []],
      zoneId: [undefined, []],
      areaId: [undefined, []],
      provinceId: [undefined, []],
      districtId: [undefined, []],
      wardId: [undefined, []],
      address: [undefined, []],
      mobilePhone: [undefined, []],
      asmStaffId: [undefined, []],
      rsmStaffId: [undefined, []],
      efficient: [undefined],
      totalRequest: [undefined],
      totalAmountBttt: [undefined],
      totalApproved: [undefined],
    });

    // this.formInfoGrossRevenue = this.fb.group({

    // });
  }

  get formReadOnly(): boolean {
    return true; //this.detailDataSource.hasChange || this.readOnly;
  }

  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getCustomer(+id);
  }

  mapModelToFormGroup() {
    super.mapModelToFormGroup();

    var totalRequest = 0;
    var totalAmountBttt = 0;
    var totalApproved = 0;

    this.getDataService<DataServiceProxy>()
      .getRecentSales(this.model.id, moment().toDate(), undefined)
      .subscribe((response) => {
        this.customerRecentSalesDataSource = <any>response.result;
        response.result.monthData.forEach((element) => {
          this.totalMonth += element.amount;
        });
      });

    this.getDataService<DataServiceProxy>()
      .getTicketInvestmentsByCustomer(
        this.id,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .pipe(
        finalize(() => {
          this.c("totalAmountBttt").setValue(totalAmountBttt);
          this.c("totalRequest").setValue(totalRequest);
          this.c("totalApproved").setValue(totalApproved);
        })
      )
      .subscribe((response) => {
        this.ticketInvestmentListItemDataSource = <any>response.result;
        response.result.items.forEach((element) => {
          totalAmountBttt += element.investmentAmount;
          totalRequest += 1;
          if (element.status >= 120) {
            totalApproved += 1;
          }
        });
      });
  }
  showDetail(row: TicketInvestmentListItemDto) {
    this.router.navigate([`investment/ticket-investments/${row.id}`]);
  }

  //#endregion
}
