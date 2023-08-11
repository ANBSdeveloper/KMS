

import { DataServiceProxy, TicketInvestmentUpdatePrintTicketQuantityCommand, TicketListDto} from './../../../../../shared/services/data.service';
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { TicketInvestmentByUserComboComponent } from '../ticket-investment-combo/ticket-investment-by-user-combo/ticket-investment-by-user-combo.component';
import { TicketInvestmentStatus } from '../../data-source/ticket-investmen-status.enum';
import { of } from 'rxjs';
import { Validators } from '@angular/forms';
import { ReportService } from '@app/main/report/report.service';

//#endregion
@Component({
  selector: "app-ticket-print",
  templateUrl: "./ticket-print.component.html",
  styleUrls: ["./ticket-print.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketPrintComponent extends DxListComponentBase<TicketListDto> {
  [x: string]: any;
  //#region Variables
  entityName = "ticket";
  permissionName = "Tickets";
  @ViewChild("ticketInvestmentCombo") ticketInvestmentCombo: TicketInvestmentByUserComboComponent;
  selectedTickets = [];
  //#endregion
  //#region Form
  constructor(
    injector: Injector,
    private reportService: ReportService
  ) {
    super(injector);
  }


  //#endregion
  init() {
    this.filterFormGroup = this.fb.group({
        ticketInvestmentId: [undefined,[Validators.required]],
        customerName:[undefined],
        address:[undefined],
        code:[undefined],
        status:[undefined],
    });

    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });

    super.init();
  }

  get printEnable(): boolean {
    return this.selectedTickets.length > 0;
  }

  get printVisible(): boolean {
    return this.isGranted(this.permissionName);
  }
  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var ticketInvestment = this.c("ticketInvestmentId").value ? this.c("ticketInvestmentId").value : "";
    if (ticketInvestment != undefined && ticketInvestment != "") {
      return this.getDataService<DataServiceProxy>().getTicketByTicketInvestmentId(
          ticketInvestment,
          pageSize,
          skip,
          search,
          sort,
          undefined
      ); 
    }  else {
      return of({
        result: {
          items: [],
          totalCount: 0,
        },
      });
    } 
  }

  print(): any {
    // loading
    this.pageBlockUI.start();
    var ticketInvestment = this.c("ticketInvestmentId").value ? this.c("ticketInvestmentId").value : "";
    var ids = this.selectedTickets.map((p) => p);
    return this.getDataService<DataServiceProxy>()
      .printTicket(
        new TicketInvestmentUpdatePrintTicketQuantityCommand({
          id:ticketInvestment,
          data: ids,
        })
      )
      .pipe(
        finalize(() => {
          this.pageBlockUI.stop();
        })
      )
      .subscribe(
        () => {
          let ticketids=ids.join(",");
         
          this.printPreview(ticketids);
   
          this.refresh();
          this.selectedTickets=[];
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  //#endregion
  ticketInvestmentIdChange(id){
    var ticketInvestment = this.ticketInvestmentCombo.items.find(
      (p) => p.id == id.value
    );
    if(ticketInvestment!=undefined){
      this.c("customerName").setValue(ticketInvestment.customerName);
      this.c("address").setValue(ticketInvestment.address);
      this.c("code").setValue(ticketInvestment.code);
      let statusDescr ="";
      if(ticketInvestment.status==TicketInvestmentStatus.Approved){
        statusDescr = this.l("ticket_investment_status_approved");
      }
      else if(ticketInvestment.status==TicketInvestmentStatus.Updating){
        statusDescr = this.l("ticket_investment_status_updating");
      }
      else if(ticketInvestment.status==TicketInvestmentStatus.Operated){
        statusDescr = this.l("ticket_investment_status_operated");
      }

      this.c("status").setValue(statusDescr);
    }
    else{
      this.c("customerName").setValue(undefined);
      this.c("address").setValue(undefined);
      this.c("code").setValue(undefined);
      this.c("status").setValue(undefined);
    }
    
  };
  onSelectionChanged(e) {
    // this.selectedTicket = e.selectedRowsData;
  }

  printPreview(strId) {
    this.reportService.openReport(
      this.l("report_ticket_investment_print_ticket"),
      "RP_TicketInvestment_PrintTicket",
      JSON.stringify({
        store: "RP_TicketInvestment_PrintTicket", 
        storeParams: [
          {
            ticketId: strId,
          },
        ],
      })
    );
  }
}
