import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";

import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";

import { CoreConfigService } from "@core/services/config.service";
import { AppComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import { AuthenticationService } from "@app/auth/service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-auth-forgot-password-v2",
  templateUrl: "./auth-forgot-password-v2.component.html",
  styleUrls: ["./auth-forgot-password-v2.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AuthForgotPasswordV2Component
  extends AppComponentBase
  implements OnInit
{
  // Public
  public error;
  public coreConfig: any;
  public success = false;
  public loading = false;
  public authForm: FormGroup;
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
    private authService: AuthenticationService,
    private fb: FormBuilder
  ) {
    super(injector);

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

    this.authForm = this.fb.group({
      username: ["", [Validators.required]],
      email: [
        "",
        [
          Validators.required,
          Validators.pattern("[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,3}$"),
        ],
      ],
    });
  }
  get f() {
    return this.authForm.controls;
  }
  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
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

  onSubmit(): void {
    if (this.authForm.invalid) {
      formHelper.validateAllFormFields(this.authForm);
      return;
    }
    this.error = "";
    this.loading = true;
    this.authService
      .sendEmailResetPassword(
        this.authForm.controls["username"].value,
        this.authForm.controls["email"].value,
        ''
      )
      .then((_) => (this.success = true))
      .catch((reason) => {
        this.error = reason;
      })
      .finally(() => (this.loading = false));
  }

  cErrorV(name, validation): boolean {
    const control = this.authForm.controls[name];
    return control.hasError(validation) && (control.dirty || control.touched);
  }

  cError(name: string) {
    return (
      this.authForm.get(name).invalid &&
      (this.authForm.get(name).dirty || this.authForm.get(name).touched)
    );
  }
}
