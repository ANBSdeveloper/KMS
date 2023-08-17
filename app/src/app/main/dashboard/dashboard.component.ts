import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "@app/auth/service";
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: "dashboard",
  template: "",
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent extends AppComponentBase{
  constructor(injector: Injector, private authenticationService: AuthenticationService,private  router: Router) {
    super(injector);
  }
  ngOnInit() {
    if (this.isGranted("Dashboards") || this.isGranted("Dashboards.Admin")) {
      this.router.navigateByUrl("/dashboard/analytics");
    } else {
      this.router.navigateByUrl("/investment/posm-investment-list");
    }
  }
}
