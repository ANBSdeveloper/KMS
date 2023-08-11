import { TicketInvestmentSSStaffComboComponent } from "./../../ticket-investment/ticket-investment-ss-staff-combo/ticket-investment-ss-staff-combo.component";
import { TicketInvestmentASMStaffComboComponent } from "./../../ticket-investment/ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component";
import { TicketInvestmentRSMStaffComboComponent } from "./../../ticket-investment/ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component";

import {
  DataServiceProxy,
  OrderListItemDto,
  StaffDto,
} from "./../../../../../shared/services/data.service";
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, takeUntil } from "rxjs/operators";
import moment from "moment";
import { Validators } from "@angular/forms";
import { of } from "rxjs";
import { ShopComboComponent } from "@app/main/master/customer/shop-combo/shop-combo.component";
import { RoleType } from "@app/main/system/role/role-type.enum";
import { AuthenticationService } from "@app/auth/service";

//#endregion
@Component({
  selector: "app-order-list",
  templateUrl: "./order-list.component.html",
  styleUrls: ["./order-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class OrderListComponent extends DxListComponentBase<OrderListItemDto> {
  [x: string]: any;
  //#region Variables
  entityName = "order";
  permissionName = "Orders";
  @ViewChild("rsmStaffCombo")
  rsmStaffCombo: TicketInvestmentRSMStaffComboComponent;
  @ViewChild("asmStaffCombo")
  asmStaffCombo: TicketInvestmentASMStaffComboComponent;
  @ViewChild("ssStaffCombo")
  ssStaffCombo: TicketInvestmentSSStaffComboComponent;
  @ViewChild("shopCombo") shopCombo: ShopComboComponent;
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
  constructor(injector: Injector, private authService: AuthenticationService) {
    super(injector);
  }

  //#endregion
  init() {
    this.filterFormGroup = this.fb.group({
      fromDate: [moment().add(-1, "month").toDate(), [Validators.required]],
      toDate: [moment().toDate(), [Validators.required]],
      rsmStaffId: [undefined],
      asmStaffId: [undefined],
      ssStaffId: [undefined],
      cutormerId: [undefined],
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

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var rsmStaff = this.c("rsmStaffId").value ? this.c("rsmStaffId").value : "";
    var asmStaff = this.c("asmStaffId").value ? this.c("asmStaffId").value : "";
    var ssStaff = this.c("ssStaffId").value ? this.c("ssStaffId").value : "";
    var cutormerId = this.c("cutormerId").value
      ? this.c("cutormerId").value
      : "";
    var fromDate = this.c("fromDate").value
      ? this.c("fromDate").value
      : undefined;
    var toDate = this.c("toDate").value ? this.c("toDate").value : undefined;
    if (fromDate != undefined && toDate != undefined) {
      return this.getDataService<DataServiceProxy>().getOrders(
        cutormerId,
        rsmStaff,
        asmStaff,
        ssStaff,
        fromDate,
        toDate,
        pageSize,
        skip,
        search,
        sort,
        undefined
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
  rsmStaffChange(record) {
    this.asmStaffCombo.value = undefined;
    this.ssStaffCombo.value = undefined;
    this.shopCombo.value = undefined;
    setTimeout(() => {
      this.asmStaffCombo.loadData();
      this.ssStaffCombo.loadData();
      this.shopCombo.loadData();
    }, 50);
  }
  asmStaffChange(record) {
    this.ssStaffCombo.value = undefined;
    this.shopCombo.value = undefined;
    setTimeout(() => {
      this.ssStaffCombo.loadData();
      this.shopCombo.loadData();
    }, 50);
  }
  ssStaffChange(record) {
    this.shopCombo.value = undefined;
    setTimeout(() => {
      this.shopCombo.loadData();
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
