//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { FormComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import { DynamicDialogRef, DynamicDialogConfig } from "primeng/dynamicdialog";
import {
  ChangeUserPasswordCommand,
  DataServiceProxy,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { FormGroup, Validators } from "@angular/forms";
import { finalize } from "rxjs/operators";
import { ToastService } from "@app/main/components/toasts/toasts.service";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-change-password-dialog",
  templateUrl: "./change-password-dialog.component.html",
  styleUrls: ["./change-password-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ChangePasswordDialogComponent extends FormComponentBase {
  @BlockUI("change_password_dialog_block") formBlockUI: NgBlockUI;
  passwordTextType: boolean;
  confirmPasswordTextType: boolean;
  formGroup: FormGroup;
  constructor(
    injector: Injector,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private toastr: ToastrService,
  ) {
    super(injector);

    this.formGroup = this.fb.group({
      password: [
        "",
        [
          Validators.required,
          Validators.minLength(5),
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

    this.formGroup.valueChanges.subscribe((field) => {
      var confirmPassword = this.c("confirmPassword");
      var password = this.c("password");
      if (field.password !== field.confirmPassword) {
        confirmPassword.setErrors({
          ...confirmPassword.errors,
          mismatch: true,
        });
      } else {
        if (confirmPassword.hasError("mismatch")) {
          this.cRemoveError(confirmPassword, "mismatch");
        }
      }

      if (password.value || confirmPassword.value) {
        if (!password.validator || password.validator.length == 0) {
          password.setValidators([
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(250),
          ]);
        }
        if (
          !confirmPassword.validator ||
          confirmPassword.validator.length == 0
        ) {
          confirmPassword.setValidators([
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(250),
          ]);
        }
      }
    });
  }

  close() {
    this.ref.close();
  }

  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  toggleConfirmPasswordTextType() {
    this.confirmPasswordTextType = !this.confirmPasswordTextType;
  }

  update() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return;
    }
    var command = new ChangeUserPasswordCommand({
      newPassword: this.cValue("password"),
    });

    this.formBlockUI.start();

    this.getDataService<DataServiceProxy>()
      .changeUserPassword(command)
      .pipe(finalize(() => this.formBlockUI.stop()))
      .subscribe(
        (_) => {
          this.toastr.success(
            "",
            this.localization.get("submit_success_message_title"),
            {
              toastClass: "toast ngx-toastr",
              closeButton: true,
              positionClass: "toast-bottom-center",
            }
          );
          this.ref.close();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }
  //#endregion
}
