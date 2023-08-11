import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "./auth/helpers";

const routes: Routes = [
  {
    path: "dashboard",
    canActivate: [AuthGuard],
    loadChildren: () =>
      import("./main/dashboard/dashboard.module").then(
        (m) => m.DashboardModule
      ),
  },
  {
    path: "master",
    loadChildren: () =>
      import("./main/master/master.module").then((m) => m.MasterModule),
    canActivate: [AuthGuard],
  },
  {
    path: "investment",
    loadChildren: () =>
      import("./main/investment/investment.module").then(
        (m) => m.InvestmentModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: "system",
    loadChildren: () =>
      import("./main/system/system.module").then((m) => m.SystemModule),
    canActivate: [AuthGuard],
  },
  {
    path: "report",
    loadChildren: () =>
      import("./main/report/report.module").then((m) => m.ReportModule),
    canActivate: [AuthGuard],
  },
  {
    path: "pages",
    loadChildren: () =>
      import("./main/pages/pages.module").then((m) => m.PagesModule),
  },
  {
    path: "",
    redirectTo: "/dashboard/index",
    pathMatch: "full",
  },
  {
    path: "**",
    redirectTo: "/pages/miscellaneous/error", //Error 404 - Page not found
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: "enabled", // Add options right here
      relativeLinkResolution: "legacy",
    }),
  ],
  exports: [RouterModule],
  providers: [],
})
export class RootRoutingModule {}
