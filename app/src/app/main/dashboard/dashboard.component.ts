import { Component, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "@app/auth/service";

@Component({
  selector: "dashboard",
  template: "",
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent {
  constructor(private authenticationService: AuthenticationService,private  router: Router) {
    
  }
  ngOnInit() {
    if (this.authenticationService.currentUser.isAdmin) {
      this.router.navigateByUrl("/dashboard/analytics");
    } else {
      this.router.navigateByUrl("/dashboard/analytics");
    }
  }
}
