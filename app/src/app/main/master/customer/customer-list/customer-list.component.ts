import {
  CustomerByStaffListDto,
  StaffDto,
} from "./../../../../../shared/services/data.service";
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { CustomerDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { RSMStaffComboComponent } from "../../staff/rsm-staff-combo/rsm-staff-combo.component";
import { ASMStaffComboComponent } from "../../staff/asm-staff-combo/asm-staff-combo.component";
import { SSStaffComboComponent } from "../../staff/ss-staff-combo/ss-staff-combo.component";
import { debounceTime, takeUntil } from "rxjs/operators";
import { AuthenticationService } from "@app/auth/service";
import { RoleType } from "@app/main/system/role/role-type.enum";
//#endregion
@Component({
  selector: "app-customer-list",
  templateUrl: "./customer-list.component.html",
  styleUrls: ["./customer-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerListComponent extends DxListComponentBase<CustomerByStaffListDto> {
  //#region Variables
  entityName = "customer";
  permissionName = "Customers";
  @ViewChild("rsmStaffCombo") rsmStaffCombo: RSMStaffComboComponent;
  @ViewChild("asmStaffCombo") asmStaffCombo: ASMStaffComboComponent;
  @ViewChild("ssStaffCombo") ssStaffCombo: SSStaffComboComponent;
  selectedCustomers = [];
  allMode: string;
  checkBoxesMode: string;
  staff: StaffDto;
  //#endregion
  constructor(injector: Injector, private authService: AuthenticationService) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      rsmStaffId: [undefined],
      asmStaffId: [undefined],
      ssStaffId: [undefined],
      statusActive: [undefined],
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
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getCustomersByStaff(
      isActive,
      undefined,
      undefined,
      undefined,
      rsmStaff,
      asmStaff,
      ssStaff,
      undefined,
      undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    //return this.getDataService<DataServiceProxy>().deleteRewardPackage(id);
  }
  //#endregion

  showDetail(row: CustomerDto) {
    this.router.navigate([`master/customer/${row.id}`]);
  }

  create() {
    this.router.navigate([`master/new-customer`]);
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
