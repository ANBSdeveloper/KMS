import {
  CustomerRecentSalesDto,
  MonthData,
  PosmInvestmentItemExtDto,
  PosmInvestmentListDto,
  TicketInvestmentListItemDto,
} from './../../../../../shared/services/data.service';
import { WardComboComponent } from './../../ward/ward-combo/ward-combo.component';
import { DistrictComboComponent } from './../../district/district-combo/district-combo.component';
import { ProvinceComboComponent } from './../../province/province-combo/province-combo.component';
import { ZoneComboComponent } from './../../zone/zone-combo/zone-combo.component';
//#region Import
import {
  Component,
  ElementRef,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { DataServiceProxy, CustomerDto } from '@shared/services/data.service';
import { Validators } from '@angular/forms';
import { DatatableDataSource, FormComponentBase } from '@cbms/ng-core-vuexy';
import { AuthenticationService } from '@app/auth/service';
import { Subject } from 'rxjs';
import { BranchComboComponent } from '../../branch/branch-combo/branch-combo.component';
import { SSStaffComboComponent } from '../../staff/ss-staff-combo/ss-staff-combo.component';
import { AreaComboComponent } from '../../area/area-combo/area-combo.component';
import { RSMStaffComboComponent } from '../../staff/rsm-staff-combo/rsm-staff-combo.component';
import { ASMStaffComboComponent } from '../../staff/asm-staff-combo/asm-staff-combo.component';
import moment from 'moment';
import { finalize, map } from 'rxjs/operators';
import {
  DialogService,
  DynamicDialogConfig,
  DynamicDialogRef,
} from 'primeng/dynamicdialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { PosmInvestmentStatusDataSource } from '@app/main/investment/data-source/posm-investment-status.data-source';
import { DxPivotGridComponent } from 'devextreme-angular/ui/pivot-grid';
import { ReportService } from '@app/main/report/report.service';
import { Router } from '@angular/router';
import { CustomerSalesMonthlyDialogComponent } from '../customer-sales-monthly-dialog/customer-sales-monthly-dialog.component';
import { CustomerAcceptanceImageDialogComponent } from '../customer-acceptance-image-dialog/customer-acceptance-image-dialog.component';
//#endregion

@Component({
  selector: 'app-customer-dialog',
  templateUrl: './customer-dialog.component.html',
  styleUrls: ['./customer-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerDialogComponent extends FormComponentBase {
  //#region Variables
  @ViewChild('pivotGrid', { static: false }) pivotGrid: DxPivotGridComponent;
  @ViewChild('pivotGridSales', { static: false })
  pivotGridSales: DxPivotGridComponent;
  @ViewChild('pivotGridHistory', { static: false })
  pivotGridHistory: DxPivotGridComponent;
  @ViewChild('branchCombo') branchCombo: BranchComboComponent;
  @ViewChild('rsmStaffCombo') rsmStaffCombo: RSMStaffComboComponent;
  @ViewChild('asmStaffCombo') asmStaffCombo: ASMStaffComboComponent;
  @ViewChild('ssStaffCombo') ssStaffCombo: SSStaffComboComponent;
  @ViewChild('zoneCombo') zoneCombo: ZoneComboComponent;
  @ViewChild('areaCombo') areaCombo: AreaComboComponent;
  @ViewChild('provinceCombo') provinceCombo: ProvinceComboComponent;
  @ViewChild('districtCombo') districtCombo: DistrictComboComponent;
  @ViewChild('wardCombo') wardCombo: WardComboComponent;
  @ViewChild('cardHeader') private cardHeader: ElementRef;
  @ViewChild('customerInfo1') private customerInfo1: ElementRef;
  @ViewChild('customerInfo2') private customerInfo2: ElementRef;
  pivotGridDataSource;
  collapseStatus = true;
  customerRecentSalesDataSource = new CustomerRecentSalesDto();
  pivotGridHistoryDataSource;
  pivotGridSalesDataSource;
  searchChangeSubject = new Subject<string>();
  ticketInvestmentListItemDataSource =
    new DatatableDataSource<TicketInvestmentListItemDto>();
  posmInvestmentListItemDataSource =
    new DatatableDataSource<PosmInvestmentListDto>();
  loading = false;
  totalMonth = 0;
  model: any;
  @BlockUI() pageBlockUI: NgBlockUI;
  //#endregion

  constructor(
    injector: Injector,
    private authenticatonService: AuthenticationService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    public posmStatusDataSource: PosmInvestmentStatusDataSource,
    public dialogService: DialogService,
    public reportService: ReportService,
    public router: Router
  ) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      code: [undefined],
      name: [undefined],
      branchId: [undefined, []],
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
      totalRequestBttt: [undefined],
      totalAmountBttt: [undefined],
      totalApprovedBttt: [undefined],
      totalRequestPosm: [undefined],
      totalAmountPosm: [undefined],
      totalApprovedPosm: [undefined],
    });

    // this.formInfoGrossRevenue = this.fb.group({

    // });
  }

  get formReadOnly(): boolean {
    return true; //this.detailDataSource.hasChange || this.readOnly;
  }

  ngOnInit() {
    this.configForm();
    this.getRequest(this.config.data.id)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.pageBlockUI.stop();
        })
      )
      .subscribe((response) => {
        var result = response.result;
        this.model = (result.constructor as any).fromJS(result);
        this.mapModelToFormGroup();
      });
  }

  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getCustomer(+id);
  }

  mapModelToFormGroup() {
    this.formGroup.reset();
    Object.keys(this.formGroup.controls).forEach((key) => {
      if (this.model.hasOwnProperty(key)) {
        this.formGroup.controls[key].setValue(this.model[key]);
      }
    });
    var totalRequestBttt = 0;
    var totalAmountBttt = 0;
    var totalApprovedBttt = 0;
    var totalRequestPosm = 0;
    var totalAmountPosm = 0;
    var totalApprovedPosm = 0;
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
        this.config.data.id,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .pipe(
        finalize(() => {
          this.c('totalAmountBttt').setValue(totalAmountBttt);
          this.c('totalRequestBttt').setValue(totalRequestBttt);
          this.c('totalApprovedBttt').setValue(totalApprovedBttt);
        })
      )
      .subscribe((response) => {
        this.ticketInvestmentListItemDataSource = <any>response.result;
        response.result.items.forEach((element) => {
          totalAmountBttt += element.investmentAmount;
          totalRequestBttt += 1;
          if (element.status >= 120) {
            totalApprovedBttt += 1;
          }
        });
      });

    this.getDataService<DataServiceProxy>()
      .getPosmInvestmentItemsByCustomer(
        this.config.data.id,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .pipe(
        map((res) => ({
          ...res,
          result: {
            ...res.result,
            items: res.result.items.map((item) => ({
              ...item,
              statusName: this.posmStatusDataSource.getItemName(item.status),
            })),
          },
        })),
        finalize(() => {
          this.c('totalAmountPosm').setValue(totalAmountPosm);
          this.c('totalRequestPosm').setValue(totalRequestPosm);
          this.c('totalApprovedPosm').setValue(totalApprovedPosm);
        })
      )
      .subscribe((response) => {
        this.posmInvestmentListItemDataSource = <any>response.result;
        response.result.items.forEach((element) => {
          totalAmountPosm += element.investmentAmount;
          totalRequestPosm += 1;
          if (element.status >= 150) {
            totalApprovedPosm += 1;
          }
        });
      });

    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: 'RP_PivotPosm',
            storeParams: [
              {
                customerId: this.model.id,
              },
            ],
          })
        )
      )
      .subscribe((res) => {
        this.pivotGridDataSource = {
          fields: [
            {
              caption: 'Loại POSM',
              dataField: 'PosmTypeName',
              area: 'row',
              width: 200,
              expanded: true,
            },
            {
              caption: 'Trạng thái đầu tư',
              dataField: 'InvestmentStatus',
              area: 'column',
              expanded: true,
            },
            {
              caption: 'Năm',
              dataField: 'Year',
              area: 'column',
              expanded: true,
            },
            {
              caption: 'Số lượng',
              dataField: 'PosmInvestmentItemId',
              dataType: 'number',
              summaryType: 'count',
              format: '#,##0',
              area: 'data',
            },
            {
              caption: 'Số tiền',
              dataField: 'InvestmentAmount',
              dataType: 'number',
              summaryType: 'sum',
              format: '#,##0',
              area: 'data',
            },
          ],

          store: res.result,
        };

        this.pivotGrid.instance.option('dataSource', this.pivotGridDataSource);
      });

    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: 'RP_PivotPosmHistory',
            storeParams: [
              {
                customerId: this.model.id,
              },
            ],
          })
        )
      )
      .subscribe((res) => {
        this.pivotGridHistoryDataSource = {
          fields: [
            {
              caption: 'Loại POSM',
              dataField: 'PosmTypeName',
              width: 300,
              area: 'row',
              expanded: true,
            },
            {
              caption: 'Năm',
              dataField: 'Year',
              area: 'column',
              expanded: true,
            },
            {
              caption: 'Số lượng',
              dataField: 'PosmInvestmentItemId',
              dataType: 'number',
              summaryType: 'count',
              format: '#,##0',
              area: 'data',
            },
            {
              caption: 'Số tiền',
              dataField: 'InvestmentAmount',
              dataType: 'number',
              summaryType: 'sum',
              format: '#,##0',
              area: 'data',
            },
          ],

          store: res.result,
        };

        this.pivotGridHistory.instance.option(
          'dataSource',
          this.pivotGridHistoryDataSource
        );
      });

    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: 'RP_CustomerSales',
            storeParams: [
              {
                customerId: this.model.id,
              },
            ],
          })
        )
      )
      .subscribe((res) => {
        this.pivotGridSalesDataSource = {
          fields: [
            {
              caption: 'Năm',
              dataField: 'Year',
              width: 300,
              area: 'row',
              expanded: true,
              sortOrder: 'desc',
            },
            {
              caption: 'Tháng',
              dataField: 'Month',
              area: 'column',
              expanded: true,
            },
            {
              caption: 'Số tiền',
              dataField: 'Amount',
              dataType: 'number',
              summaryType: 'sum',
              format: '#,##0',
              area: 'data',
            },
          ],

          store: res.result,
        };

        this.pivotGridSales.instance.option(
          'dataSource',
          this.pivotGridSalesDataSource
        );
      });
  }
  showDetail(row: TicketInvestmentListItemDto) {
    this.router.navigate([`investment/ticket-investments/${row.id}`]);
  }
  collapse() {
    const cardHeaderEl = this.cardHeader.nativeElement;
    this.collapseStatus = !this.collapseStatus;
    if (this.collapseStatus) {
      setTimeout(() => {
        this.customerInfo1.nativeElement.classList.add('collapse');
        this.customerInfo2.nativeElement.classList.add('collapse');
      }, 100);
    } else {
      this.customerInfo1.nativeElement.classList.remove('collapse');
      this.customerInfo2.nativeElement.classList.remove('collapse');
    }
  }

  yearClick(data) {
    this.dialogService.open(CustomerSalesMonthlyDialogComponent, {
      data: this.customerRecentSalesDataSource.monthData,
      baseZIndex: 1001,
      header: this.l('info_gross_revenue_monthly'),
    });
  }

  showDetailPosm(data: PosmInvestmentItemExtDto) {
    const url = this.router.serializeUrl(
      this.router.createUrlTree([
        `investment/posm-investments/${data.posmInvestmentId}`,
      ])
    );
    window.open(url, '_blank');
  }

  showConfirmImage(data) {
    this.dialogService.open(CustomerAcceptanceImageDialogComponent, {
      data: data,
      baseZIndex: 1001,
      header: data.posmItemName,
    });
  }
  //#endregion
}
