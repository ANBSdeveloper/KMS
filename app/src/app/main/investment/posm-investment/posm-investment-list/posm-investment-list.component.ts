import { AuthenticationService } from "./../../../../auth/service/authentication.service";
import {
  DataServiceProxy,
  PosmInvestmentDirectorMultiApproveCommand,
  PosmInvestmentDirectorMultiApproveDto,
  PosmInvestmentDirectorMultiDenyCommand,
  PosmInvestmentDirectorMultiDenyDto,
  PosmInvestmentDto,
  PosmInvestmentTradeMultiApproveCommand,
  PosmInvestmentTradeMultiApproveDto,
  PosmInvestmentTradeMultiConfirmAcceptCommand,
  PosmInvestmentTradeMultiConfirmAcceptDto,
  PosmInvestmentTradeMultiDenyCommand,
  PosmInvestmentTradeMultiDenyDto,
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
import { PosmInvestmentRSMStaffComboComponent } from "../posm-investment-rsm-staff-combo/posm-investment-rsm-staff-combo.component";
import { PosmInvestmentASMStaffComboComponent } from "../posm-investment-asm-staff-combo/posm-investment-asm-staff-combo.component";
import { PosmInvestmentSSStaffComboComponent } from "../posm-investment-ss-staff-combo/posm-investment-ss-staff-combo.component";
import { PosmInvestmentStatusDataSource } from "../../data-source/posm-investment-status.data-source";
import { debounceTime, finalize, map, takeUntil } from "rxjs/operators";
import { PosmInvestmentStatusComboComponent } from "../posm-investment-status-combo/posm-investment-status-combo.component";
import moment from "moment";
import { RoleType } from "@app/main/system/role/role-type.enum";
import { Validators } from "@angular/forms";
import { of } from "rxjs";
import { PosmInvestmentStatus } from "../../data-source/posm-investment-status.enum";
import { Workbook } from "exceljs";
import { saveAs } from "file-saver";
import { exportDataGrid } from "devextreme/excel_exporter";
import { PosmInvestmentItemStatusDataSource } from "../../data-source/posm-investment-item-status.data-source";
import { environment } from "environments/environment";
import { HttpClient } from "@angular/common/http";
import { ReportService } from "@app/main/report/report.service";
import { DataService } from "@app/main/forms/form-elements/select/data.service";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-posm-investment-list",
  templateUrl: "./posm-investment-list.component.html",
  styleUrls: ["./posm-investment-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentListComponent extends DxListComponentBase<PosmInvestmentDto> {
  selectedKeyShops: any;
  //#region Variables
  entityName = "posmInvestment";
  permissionName = "PosmInvestments";
  @ViewChild("rsmStaffCombo")
  rsmStaffCombo: PosmInvestmentRSMStaffComboComponent;
  @ViewChild("asmStaffCombo")
  asmStaffCombo: PosmInvestmentASMStaffComboComponent;
  @ViewChild("ssStaffCombo")
  ssStaffCombo: PosmInvestmentSSStaffComboComponent;
  @ViewChild("posmInvestmenStatus")
  posmInvestmenStatus: PosmInvestmentStatusComboComponent;
  statusDefault = undefined;
  staff: StaffDto;
  //#endregion
  //#region Form
  constructor(
    injector: Injector,
    public posmInvestmentStatusDataSource: PosmInvestmentStatusDataSource,
    public posmInvestmentItemtatusDataSource: PosmInvestmentItemStatusDataSource,
    private authService: AuthenticationService,
    private reportService: ReportService,
    private http: HttpClient,
    public toastrService: ToastrService
  ) {
    super(injector);

    if (this.authService.currentUser.roles[0] == RoleType.Rsm) {
      this.statusDefault = PosmInvestmentStatus.Request;
    } else if (this.authService.currentUser.roles[0] == RoleType.Asm) {
      this.statusDefault = PosmInvestmentStatus.Request;
    } else if (
      this.authService.currentUser.roles[0] == RoleType.SalesDirector
    ) {
      this.statusDefault = PosmInvestmentStatus.TradeApprovedRequest;
    } else if (this.authService.currentUser.roles[0] == RoleType.Supply) {
      this.statusDefault = PosmInvestmentStatus.DirectorApprovedRequest;
    } else if (this.authService.currentUser.roles[0] == RoleType.Marketing) {
      this.statusDefault = PosmInvestmentStatus.ValidOrder;
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
        .getPosmInvestmentsByTime(
          status ? [status] : undefined,
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
        )
        .pipe(
          map((res) => ({
            ...res,
            result: {
              ...res.result,
              items: res.result.items.map((item) => ({
                ...item,
                status: this.statusNameFromId(item.status),
                statusCode: item.status,
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
    return this.posmInvestmentStatusDataSource.findItem(status)?.name;
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
  headerThirdState = false;
  firstStatus = undefined;
  onSelectionChanged(e) {
    let rows = this.dataGrid.instance.getVisibleRows().length;
    let tradeApprove = this.isGranted("PosmInvestments.TradeApprove");
    let confirmAccept = this.isGranted("PosmInvestments.ConfirmAccept2");
    let dirrectorApprove = this.isGranted("PosmInvestments.DirectorApprove");
    if (this.headerThirdState && e.selectedRowsData.length == rows) {
      this.headerThirdState = false;
      this.dataGrid.instance.clearSelection();
    } else {
      this.selectedKeyShops = e.selectedRowsData;
      let disableRows = !this.firstStatus
        ? this.selectedKeyShops
            .filter(
              (item) =>
                (item.statusCode != PosmInvestmentStatus.RsmApprovedRequest &&
                item.statusCode != PosmInvestmentStatus.ConfirmedAccept1 &&
                item.statusCode != PosmInvestmentStatus.TradeApprovedRequest)
                 || (item.statusCode == PosmInvestmentStatus.RsmApprovedRequest && !tradeApprove)
                 || (item.statusCode == PosmInvestmentStatus.ConfirmedAccept1 && !confirmAccept)
                 || (item.statusCode == PosmInvestmentStatus.TradeApprovedRequest && !dirrectorApprove)
            )
            .map((item) => item.id)
        : this.selectedKeyShops
            .filter((item) => item.statusCode != this.firstStatus)
            .map((item) => item.id);
            
      e.component.deselectRows(disableRows);

      if (e.selectedRowKeys.length >= 1 && rows >= 2) {
        this.headerThirdState = true;
      } else {
        this.headerThirdState = false;
      }

      var validSelectedRows = this.selectedKeyShops.filter(
        (item) =>
          (item.statusCode == PosmInvestmentStatus.RsmApprovedRequest &&
            tradeApprove) ||
          (item.statusCode == PosmInvestmentStatus.ConfirmedAccept1 &&
            confirmAccept) ||
          (item.statusCode == PosmInvestmentStatus.TradeApprovedRequest &&
            dirrectorApprove)
      );
  
      this.firstStatus =
        validSelectedRows.length > 0
          ? validSelectedRows[0].statusCode
          : undefined;
    }
    

    // if (e.currentSelectedRowKeys.length == e) {
    //   setTimeout(() => this.dataGrid.instance.clearSelection(), 200);
    // } else {
    // if (e.selectedRowsData.length == e.selectedRowKeys.length) {
    //
    // } else {
    //   setTimeout(() => this.dataGrid.instance.clearSelection(), 200);
    // }
  }
  //#endregion

  get allowSelection(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("PosmInvestments.TradeApprove") ||
      this.isGranted("PosmInvestments.TradeDeny") ||
      this.isGranted("PosmInvestments.DirectorApprove") ||
      this.isGranted("PosmInvestments.DirectorDeny") || 
      this.isGranted("PosmInvestments.ConfirmAccept2")
    );
  }
  get registerVisible(): boolean {
    return (
      (this.isGranted(this.permissionName) ||
        this.isGranted("PosmInvestments.Register")) &&
      this.staff != undefined
    );
  }

  register() {
    this.router.navigate([`investment/register-posm-investment`]);
  }

  showDetail(data: PosmInvestmentDto) {
    this.router.navigate([`investment/posm-investments/${data.id}`]);
  }

  export() {
    // const workbook = new Workbook();
    // const worksheet = workbook.addWorksheet("Sheet 1");

    // exportDataGrid({
    //   component: this.dataGrid.instance,
    //   worksheet,
    //   autoFilterEnabled: true,
    // }).then(() => {
    //   workbook.xlsx.writeBuffer().then((buffer) => {
    //     saveAs(
    //       new Blob([buffer], { type: "application/octet-stream" }),
    //       "DataGrid.xlsx"
    //     );
    //   });
    // });
    this.reportService.openReport(
      this.l("report_posm_investment_request"),
      "RP_PosmInvestment_Export",
      JSON.stringify({
        store: "RP_PosmInvestment_Export",
        storeParams: [
          {
            fromDate: this.cValue("fromDate"),
            toDate: this.cValue("toDate"),
            staffId: this.cValue("ssStaffId")
              ? this.cValue("ssStaffId")
              : this.cValue("asmStaffId")
              ? this.cValue("rsmStaffId")
              : undefined,
            status: this.cValue("status"),
          },
        ],
      })
    );
  }

  tradeApprove() {
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .tradeMultiApprovePosmInvestment(
        new PosmInvestmentTradeMultiApproveCommand({
          data: new PosmInvestmentTradeMultiApproveDto({
            note: "",
            ids: this.dataGrid.selectedRowKeys,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  tradeConfirm() {
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .multiConfirmAccept2(
        new PosmInvestmentTradeMultiConfirmAcceptCommand({
          data: new PosmInvestmentTradeMultiConfirmAcceptDto({
            note: "",
            ids: this.dataGrid.selectedRowKeys,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  tradeDeny() {
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .tradeMultiApprovePosmInvestment(
        new PosmInvestmentTradeMultiDenyCommand({
          data: new PosmInvestmentTradeMultiDenyDto({
            note: "",
            ids: this.dataGrid.selectedRowKeys,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  directorDeny() {
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .directorMultiApprovePosmInvestment(
        new PosmInvestmentDirectorMultiDenyCommand({
          data: new PosmInvestmentDirectorMultiDenyDto({
            note: "",
            ids: this.dataGrid.selectedRowKeys,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  directorApprove() {
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .directorMultiApprovePosmInvestment(
        new PosmInvestmentDirectorMultiApproveCommand({
          data: new PosmInvestmentDirectorMultiApproveDto({
            note: "",
            ids: this.dataGrid.selectedRowKeys,
          }),
        })
      )
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  onReadFile = (e) => {
    if (e.target.files && e.target.files[0]) {
      var file = e.target.files[0];
      var regex = /^(.)+(.xls|.xlsx)$/;
      if (!regex.test(file.name)) {
        this.messageService.toastError(this.l("error-import-file"));
        return;
      }
      if (file.length / (1024 * 1024) > 10) {
        this.messageService.toastError(this.l("error-import-size"));
        return;
      }

      this.import(file);

      e.target.value = null;
    }
  };

  import(file) {
    this.pageBlockUI.start();
    let url = environment.apiUrl + "/api/v1/posm-investments/import";
    const form = new FormData();
    form.append("file", file, file.name);
    return this.http
      .post<string>(url, form)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.toastrService.success(this.l("import-success"), "", {
            toastClass: "toast ngx-toastr",
            closeButton: true,
            positionClass: "toast-bottom-center",
          });
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
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

  get tradeApproveVisible() {
    return (
      this.firstStatus &&
      this.firstStatus == PosmInvestmentStatus.RsmApprovedRequest &&
      (this.isGranted("PosmInvestments.TradeApprove") ||
        this.isGranted("PosmInvestments.TradeDeny"))
    );
  }

  get tradeConfirmVisible() {
    return (
      this.firstStatus &&
      this.firstStatus == PosmInvestmentStatus.ConfirmedAccept1 &&
      this.isGranted("PosmInvestments.ConfirmAccept2")
    );
  }

  get directorApproveVisible() {
    return (
      this.firstStatus &&
      this.firstStatus == PosmInvestmentStatus.TradeApprovedRequest &&
      (this.isGranted("PosmInvestments.DirectorApprove") ||
        this.isGranted("PosmInvestments.DirectorDeny"))
    );
  }

  get importVisible() {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("PosmInvestments.ImportHistory")
    );
  }
}
