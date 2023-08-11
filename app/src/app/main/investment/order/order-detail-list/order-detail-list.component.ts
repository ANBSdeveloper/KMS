import { TicketInvestmentSSStaffComboComponent } from './../../ticket-investment/ticket-investment-ss-staff-combo/ticket-investment-ss-staff-combo.component';
import { TicketInvestmentASMStaffComboComponent } from './../../ticket-investment/ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component';
import { TicketInvestmentRSMStaffComboComponent } from './../../ticket-investment/ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component';

import { DataServiceProxy, OrderDetailDto, OrderListItemDto } from './../../../../../shared/services/data.service';
//#region Import
import {
    Component,
    Injector,
    Input,
    ViewEncapsulation,
  } from "@angular/core";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { of } from 'rxjs';

  //#endregion
  @Component({
    selector: "app-order-detail-list",
    templateUrl: "./order-detail-list.component.html",
    styleUrls: ["./order-detail-list.component.scss"],
    encapsulation: ViewEncapsulation.None,
  })
  
  export class OrderDetailListComponent extends DxListComponentBase<OrderDetailDto> {
    [x: string]: any;
    //#region Variables
    entityName = "orderDetail";
    permissionName = "Orders";
    statusDefault=undefined;
    @Input() OrderId: undefined;
    
    //#endregion
    //#region Form
    configForm() {
      this.formGroup = this.fb.group({    
      });
    }
    constructor(injector: Injector
      ) {
      super(injector);
    }

    //#endregion
    
    
    //#region Api Request
    getListRequest(pageSize: number, skip: number, search: string, sort: string) {
        return this.getDataService<DataServiceProxy>().getOrderDetails(
            this.OrderId,
            pageSize,
            skip,
            search,
            sort,
            undefined
        );
    }    
    //#endregion
      
  }
  