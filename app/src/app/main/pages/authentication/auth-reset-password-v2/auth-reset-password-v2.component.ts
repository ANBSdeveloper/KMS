import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";

import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";

import { CoreConfigService } from "@core/services/config.service";
import { ActivatedRoute } from "@angular/router";
import { AppComponentBase } from "@cbms/ng-core-vuexy";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { AuthenticationService } from "@app/auth/service";
import moment from "moment";

@Component({
  selector: "app-auth-reset-password-v2",
  templateUrl: "./auth-reset-password-v2.component.html",
  styleUrls: ["./auth-reset-password-v2.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AuthResetPasswordV2Component
  extends AppComponentBase
  implements OnInit
{
  // Public
  public newPasswordVar;
  public confPasswordVar;
  public coreConfig: any;
  public passwordTextType: boolean;
  public confPasswordTextType: boolean;
  public validTicket: boolean;
  public error: string;
  public success: boolean;
  public loading: boolean;
  public resetForm: FormGroup;
  private token: string;
  // Private
  private _unsubscribeAll: Subject<any>;

  /**
   * Constructor
   *
   * @param {CoreConfigService} _coreConfigService
   */
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _coreConfigService: CoreConfigService,
    private authService: AuthenticationService,
    private activedRoute: ActivatedRoute
  ) {
    super(injector);
    this._unsubscribeAll = new Subject();

    // Configure the layout
    this._coreConfigService.config = {
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

    this.activedRoute.queryParams.subscribe((params) => {
      if (!params.token || params.expiration_time == null) {
        this.validTicket = false;
      } else {
        var expirationTime = moment(params.expiration_time);
        if (expirationTime.isBefore(Date.now())) {
          this.validTicket = false;
        } else {
          this.token = params.token;
          this.validTicket = true;
        }
      }
    });

    this.resetForm = this.fb.group({
      newPassword: [
        "",
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(250),
        ],
      ],
      confirmPassword: [
        "",
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(250),
        ],
      ],
    });

    this.resetForm.valueChanges.subscribe((field) => {
      var confirmPassword = this.c("confirmPassword");
      if (field.newPassword !== field.confirmPassword) {
        confirmPassword.setErrors({
          ...confirmPassword.errors,
          mismatch: true,
        });
      } else {
        if (confirmPassword.hasError("mismatch")) {
          delete confirmPassword.errors["mismatch"];
        }
      }
    });
  }

  /**
   * Toggle password
   */
  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  /**
   * Toggle confirm password
   */
  toggleConfPasswordTextType() {
    this.confPasswordTextType = !this.confPasswordTextType;
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // Subscribe to config changes
    this._coreConfigService.config
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
    if (this.resetForm.invalid) {
      return;
    }

    this.error = "";
    this.loading = true;
    this.authService
      .resetPassword(this.resetForm.value.newPassword, this.token)
      .then((_) => (this.success = true))
      .catch((reason) => {
        this.error = reason;
      })
      .finally(() => (this.loading = false));
  }

  cError(name: string) {
    return (
      this.resetForm.get(name).invalid &&
      (this.resetForm.get(name).dirty || this.resetForm.get(name).touched)
    );
  }

  c(name: string) {
    return this.resetForm.get(name);
  }

  cErrorV(name, validation): boolean {
    const control = this.resetForm.controls[name];
    return control.hasError(validation) && (control.dirty || control.touched);
  }

  get formVisible(): boolean {
    return this.validTicket !== false && !this.success;
  }
}
