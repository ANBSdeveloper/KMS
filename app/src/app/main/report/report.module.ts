import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { NgSelectModule } from "@ng-select/ng-select";
import { Ng2FlatpickrModule } from "ng2-flatpickr";

import { CoreCommonModule } from "@core/common.module";
import { CoreDirectivesModule } from "@core/directives/directives";
import { CorePipesModule } from "@core/pipes/pipes.module";
import { CoreSidebarModule } from "@core/components";

import { DataServiceProxy } from "@shared/services/data.service";
import { BlockUIModule } from "ng-block-ui";
import { ToastrModule } from "ngx-toastr";
import { ContentHeaderModule } from "@app/layout/components/content-header/content-header.module";
import { SharedModule } from "@shared/shared.module";
import { QuillModule } from "ngx-quill";
import { NgxMaskModule } from "ngx-mask";
import { ReportTicketInvestmentResultComponent } from "./ticket-investment/ticket-investment-result/ticket-investment-result.component";

import { DxDateBoxModule } from "devextreme-angular/ui/date-box";
import { DxTextBoxModule } from "devextreme-angular/ui/text-box";
import { DxButtonModule } from "devextreme-angular/ui/button";
import { DxDataGridModule } from "devextreme-angular/ui/data-grid";
import { DxTextAreaModule } from "devextreme-angular/ui/text-area";
import { ReportService } from "./report.service";
import { ReportViewerComponent } from "./report-viewer/report-viewer.component";
import { InvestmentModule } from "../investment/investment.module";
import { SystemModule } from "../system/system.module";
import { ReportTicketInvestmentTicketComponent } from "./ticket-investment/ticket-investment-ticket/ticket-investment-ticket.component";
import { ReportTicketInvestmentRemarkComponent } from "./ticket-investment/ticket-investment-remark/ticket-investment-remark.component";
import { ReportTicketInvestmentRewardComponent } from "./ticket-investment/ticket-investment-reward/ticket-investment-reward.component";
import { ReportTicketInvestmentOrderDetailComponent } from "./ticket-investment/ticket-investment-order-detail/ticket-investment-order-detail.component";
import { ReportTicketInvestmentScanQrCodeComponent } from "./ticket-investment/ticket-investment-scan-qrcode/ticket-investment-scan-qrcode.component";

// routing
const routes: Routes = [
  {
    path: "ticket-investment/result",
    component: ReportTicketInvestmentResultComponent,
  },
  {
    path: "ticket-investment/ticket",
    component: ReportTicketInvestmentTicketComponent,
  },
  {
    path: "ticket-investment/remark",
    component: ReportTicketInvestmentRemarkComponent,
  },
  {
    path: "ticket-investment/reward",
    component: ReportTicketInvestmentRewardComponent,
  },
  {
    path: "ticket-investment/order-detail",
    component: ReportTicketInvestmentOrderDetailComponent,
  },
  {
    path: "ticket-investment/scan-qrcode",
    component: ReportTicketInvestmentScanQrCodeComponent,
  },
  {
    path: "viewer",
    component: ReportViewerComponent,
  },
];

@NgModule({
  declarations: [
    ReportViewerComponent,
    ReportTicketInvestmentResultComponent,
    ReportTicketInvestmentTicketComponent,
    ReportTicketInvestmentRemarkComponent,
    ReportTicketInvestmentRewardComponent,
    ReportTicketInvestmentOrderDetailComponent,
    ReportTicketInvestmentScanQrCodeComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    CoreCommonModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    ContentHeaderModule,
    Ng2FlatpickrModule,
    CorePipesModule,
    CoreDirectivesModule,
    BlockUIModule.forRoot(),
    ToastrModule,
    CoreSidebarModule,
    SharedModule,
    QuillModule.forRoot(),
    NgxMaskModule.forRoot(),
    DxDateBoxModule,
    DxTextBoxModule,
    DxButtonModule,
    DxDataGridModule,
    DxTextAreaModule,
    InvestmentModule,
    SystemModule,
  ],
  exports: [ReportTicketInvestmentResultComponent],
  providers: [DataServiceProxy, ReportService],
})
export class ReportModule {}
