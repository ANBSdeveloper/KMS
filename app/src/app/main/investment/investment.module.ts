import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";

import { NgbModule, NgbRatingModule } from "@ng-bootstrap/ng-bootstrap";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
import {DialogModule} from 'primeng/dialog';

import { CoreCommonModule } from "@core/common.module";
import { CoreDirectivesModule } from "@core/directives/directives";
import { CorePipesModule } from "@core/pipes/pipes.module";
import { CoreSidebarModule } from "@core/components";

import { InvoiceModule } from "app/main/apps/invoice/invoice.module";

import { DataServiceProxy } from "@shared/services/data.service";
import { BlockUIModule } from "ng-block-ui";
import { ToastrModule } from "ngx-toastr";
import { ContentHeaderModule } from "@app/layout/components/content-header/content-header.module";
import { SharedModule } from "@shared/shared.module";
import { QuillModule } from "ngx-quill";
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { DxDateBoxModule } from "devextreme-angular/ui/date-box";
import { DxTextBoxModule } from "devextreme-angular/ui/text-box";
import { DxButtonModule } from "devextreme-angular/ui/button";
import { BudgetListComponent } from "./budget/budget-list/budget-list.component";
import { InvestmentTypeComboComponent } from "./investment-type-combo/investment-type-combo.component";
import { InvestmentTypeDataSource } from "./data-source/investment-type.data-source";
import { SystemModule } from "../system/system.module";
import { MasterModule } from "../master/master.module";
import { BudgetEditComponent } from "./budget/budget-edit/budget-edit.component";
import { BudgetAllocateTypeDataSource } from "./data-source/budget-allocate-type.data-source";
import { BudgetAllocateTypeComboComponent } from "./budget/budget-allocate-type-combo/budget-allocate-type-combo.component";
import { BudgetZoneSidebarComponent } from "./budget/budget-zone-sidebar/budget-zone-sidebar.component";

import { BudgetZoneComboComponent } from "./budget/budget-zone-combo/budget-zone-combo.component";
import { DxNumberBoxModule } from "devextreme-angular/ui/number-box";
import { InvestmentSettingComponent } from "./investment-setting/investment-setting.component";
import { DxDataGridModule } from "devextreme-angular/ui/data-grid";
import { TicketInvestmentRSMStaffComboComponent } from "./ticket-investment/ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component";
import { TicketInvestmentListComponent } from "./ticket-investment/ticket-investment-list/ticket-investment-list.component";
import { TicketInvestmentASMStaffComboComponent } from "./ticket-investment/ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component";
import { TicketInvestmentSSStaffComboComponent } from "./ticket-investment/ticket-investment-ss-staff-combo/ticket-investment-ss-staff-combo.component";
import { TicketInvestmentStatusDataSource } from "./data-source/ticket-investmen-status.data-source";
import { TicketInvestmentStatusComboComponent } from "./ticket-investment/ticket-investment-status-combo/ticket-investment-status-combo.component";
import { TicketInvestmentEditComponent } from "./ticket-investment/ticket-investment-edit/ticket-investment-edit.component";
import { TicketInvestmentSalesCommitmentSidebarComponent } from "./ticket-investment/ticket-investment-sales-commitment-sidebar/ticket-investment-sales-commitment-sidebar.component";
import { OrderListComponent } from "./order/order-list/order-list.component";
import { OrderDetailListComponent } from "./order/order-detail-list/order-detail-list.component";
import { TicketMaterialSidebarComponent } from "./ticket-investment/ticket-material-sidebar/ticket-material-sidebar.component";
import { TicketInvestmentHistoryComponent } from "./ticket-investment/ticket-investment-history/ticket-investment-history.component";
import { TicketProgressListComponent } from "./ticket-investment/ticket-progress-list/ticket-progress-list.component";
import { TicketProgressItemComponent } from "./ticket-investment/ticket-progress-item/ticket-progress-item.component";
import { DxTextAreaModule } from "devextreme-angular/ui/text-area";
import { AccordionModule } from "primeng/accordion";
import { TicketOperationComponent } from "./ticket-investment/ticket-operation/ticket-operation.component";
import { TicketAcceptanceComponent } from "./ticket-investment/ticket-acceptance/ticket-acceptance.component";
import { TicketConsumerRewardDetailDialogComponent } from "./ticket-investment/ticket-consumer-reward-detal-dialog/ticket-consumer-reward-detail-dialog.component";
import { DialogService, DynamicDialogModule } from "primeng/dynamicdialog";
import { TicketComboComponent } from "./ticket-investment/ticket-combo/ticket-combo.component";
import { TicketConsumerRewardDetailSidebarComponent } from "./ticket-investment/ticket-consumer-reward-detail-sidebar/ticket-consumer-reward-detail-sidebar.component";
import { SidebarService } from "@cbms/ng-core-vuexy";
import { TicketFinalComponent } from "./ticket-investment/ticket-final/ticket-final.component";
import { TicketPrintComponent } from "./ticket-investment/ticket-print/ticket-print.component";
import { TicketInvestmentByUserComboComponent } from "./ticket-investment/ticket-investment-combo/ticket-investment-by-user-combo/ticket-investment-by-user-combo.component";
import { ReportService } from "../report/report.service";

import { TicketInvestmentCalendar } from "./ticket-investment/ticket-investment-calendar/ticket-investment-calendar.component";
import { FullCalendarModule } from "@fullcalendar/angular";

import dayGridPlugin from "@fullcalendar/daygrid";
import interactionPlugin from "@fullcalendar/interaction";
import listPlugin from "@fullcalendar/list";
import timeGridPlugin from "@fullcalendar/timegrid";
import { CalendarService } from "../apps/calendar/calendar.service";
import { BudgetAreaComboComponent } from "./budget/budget-area-combo/budget-area-combo.component";
import { BudgetAreaSidebarComponent } from "./budget/budget-area-sidebar/budget-area-sidebar.component";
import { BudgetBranchSidebarComponent } from "./budget/budget-branch-sidebar/budget-branch-sidebar.component";
import { PosmInvestmentRSMStaffComboComponent } from "./posm-investment/posm-investment-rsm-staff-combo/posm-investment-rsm-staff-combo.component";
import { PosmInvestmentASMStaffComboComponent } from "./posm-investment/posm-investment-asm-staff-combo/posm-investment-asm-staff-combo.component";
import { PosmInvestmentSSStaffComboComponent } from "./posm-investment/posm-investment-ss-staff-combo/posm-investment-ss-staff-combo.component";
import { PosmInvestmentStatusComboComponent } from "./posm-investment/posm-investment-status-combo/posm-investment-status-combo.component";
import { PosmInvestmentListComponent } from "./posm-investment/posm-investment-list/posm-investment-list.component";
import { PosmInvestmentStatusDataSource } from "./data-source/posm-investment-status.data-source";
import { PosmInvestmentEditComponent } from "./posm-investment/posm-investment-edit/posm-investment-edit.component";
import { PosmInvestmentSalesCommitmentSidebarComponent } from "./posm-investment/posm-investment-sales-commitment-sidebar/posm-investment-sales-commitment-sidebar.component";
import { PosmInvesetmentItemDialogComponent } from "./posm-investment/posm-investment-item-dialog/posm-investment-item-dialog.component";
import { PosmInvestmentApproveComponent } from "./posm-investment/posm-investment-approve/posm-investment-approve.component";
import { PosmInvestmentPrepareComponent } from "./posm-investment/posm-investment-prepare/posm-investment-prepare.component";
import { PosmInvestmentOperationComponent } from "./posm-investment/posm-investment-operation/posm-investment-operation.component";
import { PosmInvestmentAcceptanceComponent } from "./posm-investment/posm-investment-acceptance/posm-investment-acceptance.component";
import { PosmInvestmentHistoryComponent } from "./posm-investment/posm-investment-history/posm-investment-history.component";
import { PosmInvestmentImageDetailComponent } from "./posm-investment/posm-investment-image-detail/posm-investment-image-detail.component";

FullCalendarModule.registerPlugins([
  dayGridPlugin,
  timeGridPlugin,
  listPlugin,
  interactionPlugin,
]);

// routing
const routes: Routes = [
  {
    path: "budget-list",
    component: BudgetListComponent,
  },
  {
    path: "budgets/:id",
    component: BudgetEditComponent,
  },
  {
    path: "new-budget",
    component: BudgetEditComponent,
  },
  {
    path: "ticket-investment-list",
    component: TicketInvestmentListComponent,
  },
  {
    path: "register-ticket-investment",
    component: TicketInvestmentEditComponent,
  },
  {
    path: "ticket-investments/:id",
    component: TicketInvestmentEditComponent,
  },
  {
    path: "investment-setting",
    component: InvestmentSettingComponent,
  },
  {
    path: "order-list",
    component: OrderListComponent,
  },
  {
    path: "ticket-print",
    component: TicketPrintComponent,
  },
  {
    path: "ticket-investment-calendar",
    component: TicketInvestmentCalendar,
  },
  {
    path: "posm-investment-list",
    component: PosmInvestmentListComponent,
  },
  {
    path: "posm-investments/:id",
    component: PosmInvestmentEditComponent,
  },
  {
    path: "register-posm-investment",
    component: PosmInvestmentEditComponent,
  },
  {
    path: "",
    redirectTo: "/budget-list",
    pathMatch: "full",
  },
];

@NgModule({
  declarations: [
    InvestmentTypeComboComponent,
    BudgetListComponent,
    BudgetEditComponent,
    BudgetAllocateTypeComboComponent,
    BudgetZoneSidebarComponent,
    BudgetZoneComboComponent,
    BudgetAreaSidebarComponent,
    BudgetBranchSidebarComponent,
    BudgetAreaComboComponent,
    InvestmentSettingComponent,
    TicketInvestmentRSMStaffComboComponent,
    TicketInvestmentListComponent,
    TicketInvestmentASMStaffComboComponent,
    TicketInvestmentSSStaffComboComponent,
    TicketInvestmentStatusComboComponent,
    TicketInvestmentEditComponent,
    TicketInvestmentSalesCommitmentSidebarComponent,
    OrderListComponent,
    OrderDetailListComponent,
    TicketMaterialSidebarComponent,
    TicketInvestmentHistoryComponent,
    TicketProgressListComponent,
    TicketProgressItemComponent,
    TicketOperationComponent,
    TicketAcceptanceComponent,
    TicketConsumerRewardDetailDialogComponent,
    TicketComboComponent,
    TicketConsumerRewardDetailSidebarComponent,
    TicketFinalComponent,
    TicketPrintComponent,
    TicketInvestmentByUserComboComponent,
    TicketInvestmentCalendar,

    PosmInvestmentRSMStaffComboComponent,
    PosmInvestmentASMStaffComboComponent,
    PosmInvestmentSSStaffComboComponent,
    PosmInvestmentStatusComboComponent,
    PosmInvestmentListComponent,
    PosmInvestmentEditComponent,
    PosmInvestmentSalesCommitmentSidebarComponent,
    PosmInvesetmentItemDialogComponent,
    PosmInvestmentApproveComponent,
    PosmInvestmentPrepareComponent,
    PosmInvestmentOperationComponent,
    PosmInvestmentAcceptanceComponent,
    PosmInvestmentHistoryComponent,
    PosmInvestmentImageDetailComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    CoreCommonModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    ContentHeaderModule,
    NgxDatatableModule,
    CorePipesModule,
    CoreDirectivesModule,
    BlockUIModule.forRoot(),
    ToastrModule,
    InvoiceModule,
    CoreSidebarModule,
    SharedModule,
    QuillModule.forRoot(),
    PerfectScrollbarModule,
    DxDateBoxModule,
    DxTextBoxModule,
    DxButtonModule,
    DxNumberBoxModule,
    SystemModule,
    MasterModule,
    DxDataGridModule,
    DxTextAreaModule,
    NgbRatingModule,
    AccordionModule,
    DynamicDialogModule,
    SystemModule,
    FullCalendarModule,
    DialogModule,
  ],
  providers: [
    DataServiceProxy,
    InvestmentTypeDataSource,
    BudgetAllocateTypeDataSource,
    TicketInvestmentStatusDataSource,
    PosmInvestmentStatusDataSource,
    DialogService,
    SidebarService,
    ReportService,
    CalendarService,
  ],
  exports: [
    TicketInvestmentASMStaffComboComponent,
    TicketInvestmentSSStaffComboComponent,
    TicketInvestmentRSMStaffComboComponent,
  ],
})
export class InvestmentModule {}
