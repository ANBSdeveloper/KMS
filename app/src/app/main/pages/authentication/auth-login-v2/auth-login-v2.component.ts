import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";

import { AuthenticationService } from "app/auth/service";
import { CoreConfigService } from "@core/services/config.service";
import { ToastrService } from "ngx-toastr";
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: "app-auth-login-v2",
  templateUrl: "./auth-login-v2.component.html",
  styleUrls: ["./auth-login-v2.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AuthLoginV2Component extends AppComponentBase implements OnInit {
  //  Public
  public coreConfig: any;
  public loginForm: FormGroup;
  public loading = false;
  public submitted = false;
  public returnUrl: string;
  public error = "";
  public passwordTextType: boolean;

  // Private
  private _unsubscribeAll: Subject<any>;

  /**
   * Constructor
   *
   * @param {CoreConfigService} coreConfigService
   */
  constructor(
    injector: Injector,
    private coreConfigService: CoreConfigService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private toastrService: ToastrService
  ) {
    super(injector);
    // redirect to home if already logged in
    if (this.authenticationService.currentUser) {
      this.router.navigate(["/"]);
    }

    this._unsubscribeAll = new Subject();

    // Configure the layout
    this.coreConfigService.config = {
      layout: {
        navbar: {
          hidden: true,
        },
        menu: {
          hidden: true,
        },
        footer: {
          hidden: true,
        },
        customizer: false,
        enableLocalStorage: false,
      },
    };
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  /**
   * Toggle password
   */
  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    // Login
    this.loading = true;
    this.authenticationService
      .login(
        this.f.username.value,
        this.f.password.value,
        this.f.rememberMe.value
      )
      .then((user) => {
        setTimeout(() => {
          this.toastrService.success(
            this.l("login_success_message", user.roles.join(",")),
            this.l("login_success_title", user.name),
            {
              toastClass: "toast ngx-toastr",
              closeButton: true,
              positionClass: "toast-bottom-right",
            }
          );
        }, 1500);

        this.router.navigate([this.returnUrl]);
      })
      .catch((error) => {
        this.error = error.message;
        this.loading = false;
      });
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    var username = this.authenticationService.getRememberUsername();
    this.loginForm = this.formBuilder.group({
      username: [username, [Validators.required]],
      password: ["", Validators.required],
      rememberMe: [username ? true : false],
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";

    // Subscribe to config changes
    this.coreConfigService.config
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((config) => {
        this.coreConfig = config;
      });
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }
}
