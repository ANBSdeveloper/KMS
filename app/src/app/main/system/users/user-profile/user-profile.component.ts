//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DataServiceProxy,
  UpdateProfileCommand,
  UpdateProfileDto,
  UserDto,
} from "@shared/services/data.service";
import { FormGroup, Validators } from "@angular/forms";
import { FormComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import { AuthenticationService } from "@app/auth/service";
import { finalize, switchMap } from "rxjs/operators";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion

@Component({
  selector: "app-user-profile",
  templateUrl: "./user-profile.component.html",
  styleUrls: ["./user-profile.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class UserProfileComponent extends FormComponentBase {
  @BlockUI() pageBlockUI: NgBlockUI;
  //#region Variables
  entityName = "user";
  codeProperty = "userName";

  passwordTextType: boolean;
  confirmPasswordTextType: boolean;
  formGroup: FormGroup;
  //#endregion

  contentHeader = {
    headerTitle: "user_profile",
    actionButton: false,
  };

  constructor(injector: Injector, private authService: AuthenticationService) {
    super(injector);
  }
  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      userName: [
        "",
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(50),
        ],
      ],
      name: ["", [Validators.required, Validators.maxLength(250)]],
      password: ["", [Validators.minLength(5), Validators.maxLength(250)]],
      confirmPassword: [
        "",
        [Validators.minLength(6), Validators.maxLength(250)],
      ],
      registerDate: [undefined, []],
      expireDate: [undefined, []],
      birthday: [undefined, []],
      roleId: [undefined, [Validators.required]],
      phoneNumber: [undefined, [Validators.required]],
      emailAddress: [undefined, [Validators.email]],
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
          delete confirmPassword.errors["mismatch"];
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
      } else {
        if (password.validator && password.validator.length > 0) {
          password.clearValidators();
          password.updateValueAndValidity();
        }
        if (confirmPassword.validator && confirmPassword.validator.length > 0) {
          confirmPassword.clearValidators();
          confirmPassword.updateValueAndValidity();
        }
      }
    });
  }

  ngOnInit() {
    this.configForm();
    this.pageBlockUI.start();
    this.getDataService<DataServiceProxy>()
      .getUser(this.authService.currentUser.id)
      .pipe(
        finalize(() => {
          this.pageBlockUI.stop();
        })
      )
      .subscribe((reponse) => {
        this.mapModelToForm(reponse.result);
      });
  }
  mapModelToForm(data: UserDto) {
    this.formGroup.reset();
    Object.keys(this.formGroup.controls).forEach((key) => {
      if (data.hasOwnProperty(key)) {
        this.formGroup.controls[key].setValue(data[key]);
      }
    });

    this.formGroup.controls["roleId"].setValue(data.roles[0].id);
  }
  formValidate(): boolean {
    if (
      !formHelper.validateDate(this.formGroup, "birthday") ||
      !formHelper.validateDate(this.formGroup, "expireDate")
    ) {
      return false;
    }
    return true;
  }

  submit() {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return false;
    }

    if (!this.formValidate()) return false;

    this.messageService.clearToast();

    let request = this.getDataService<DataServiceProxy>().updateProfile(
      UpdateProfileCommand.fromJS({
        data: UpdateProfileDto.fromJS({
          birthday: this.c("birthday").value,
          emailAddress: this.c("emailAddress").value,
          name: this.c("name").value,
          password: this.c("password").value,
          phoneNumber: this.c("phoneNumber").value,
        }),
      })
    );
    this.pageBlockUI.start();
    request
      .pipe(
        finalize(() => {
          this.pageBlockUI.stop();
        })
      )
      .subscribe(
        (response) => {
          this.mapModelToForm(response.result);

          this.messageService.toastSubmitSuccess();
        },
        (error) => {
          this.messageService.toastSubmitError(error);
        }
      );
  }
  //#endregion

  //#region Form Elements
  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  toggleConfirmPasswordTextType() {
    this.confirmPasswordTextType = !this.confirmPasswordTextType;
  }

  //#endregion
}
