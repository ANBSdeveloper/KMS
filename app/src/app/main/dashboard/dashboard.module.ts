import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { TranslateModule } from "@ngx-translate/core";

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { NgApexchartsModule } from "ng-apexcharts";
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";

import { AuthGuard } from "app/auth/helpers";
import { Role } from "app/auth/models";

import { CoreCommonModule } from "@core/common.module";

import { InvoiceModule } from "app/main/apps/invoice/invoice.module";
import { InvoiceListService } from "app/main/apps/invoice/invoice-list/invoice-list.service";

import { DashboardService } from "app/main/dashboard/dashboard.service";

import { AnalyticsComponent } from "app/main/dashboard/analytics/analytics.component";
import { DashboardComponent } from "./dashboard.component";
import { ReportModule } from "../report/report.module";
import { SharedModule } from "@shared/shared.module";
import { DxPieChartModule } from "devextreme-angular/ui/pie-chart";


const routes = [
  {
    path: "index",
    component: DashboardComponent,
  },
  {
    path: "analytics",
    component: AnalyticsComponent,
    canActivate: [AuthGuard],
    resolve: {
      css: DashboardService,
      inv: InvoiceListService,
    },
  },
  {
    path: "",
    redirectTo: "/index",
    pathMatch: "full",
  },
];

@NgModule({
  declarations: [AnalyticsComponent, DashboardComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    TranslateModule,
    NgbModule,
    PerfectScrollbarModule,
    CoreCommonModule,
    NgApexchartsModule,
    InvoiceModule,
    ReportModule,
    SharedModule,
    DxPieChartModule
  ],
  providers: [DashboardService, InvoiceListService],
  exports: [AnalyticsComponent],
})
export class DashboardModule {}
