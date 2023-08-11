//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  UpsertUserDto,
  UserDto,
  UserRoleDto,
} from "@shared/services/data.service";
import { Validators } from "@angular/forms";
import { formHelper, PageEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { finalize, tap } from "rxjs/operators";
import { combineLatest } from "rxjs";
import {
  ITreeState,
  TreeComponent,
  TreeModel,
  TreeNode,
} from "@circlon/angular-tree-component";
import { RoleComboComponent } from "../../role/role-combo/role-combo.component";
import { RoleDataSource } from "../../data-source/role.data-source";
import moment from "moment";
//#endregion

@Component({
  selector: "app-user-edit",
  templateUrl: "./user-edit.component.html",
  styleUrls: ["./user-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class UserEditComponent extends PageEditFormComponentBase<
  UserDto,
  UpsertUserDto,
  UpsertUserDto
> {
  //#region Variables
  @ViewChild("roleCombo") roleCombo: RoleComboComponent;

  entityName = "user";
  permissionName = "Users";
  codeProperty = "userName";
  createUrl = "system/new-user";

  passwordTextType: boolean;
  confirmPasswordTextType: boolean;

  optionsFilter = {
    useCheckbox: true,
    useTriState: false,
  };
  nodes = [];
  assignmentRoles = this.roleDataSource.assigmentRoleNames;
  assignmentState: ITreeState;
  //#endregion

  constructor(injector: Injector, private roleDataSource: RoleDataSource) {
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
      isActive: [{ value: true, disabled: this.readOnly }],
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

      if (this.id) {
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
          if (
            confirmPassword.validator &&
            confirmPassword.validator.length > 0
          ) {
            confirmPassword.clearValidators();
            confirmPassword.updateValueAndValidity();
          }
        }
      }
      let birthdayControl = this.c("birthday");
      if (
        birthdayControl.value &&
        moment(birthdayControl.value).isSameOrAfter(moment().startOf("day"))
      ) {
        birthdayControl.setErrors({
          ...birthdayControl.errors,
          maxDate: true,
        });
      } else {
        if (birthdayControl.hasError("maxDate")) {
          delete birthdayControl.errors["maxDate"];
        }
      }
    });
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
    var roleId =
      this.model.roles.length > 0 ? this.model.roles[0].roleId : undefined;
    this.c("roleId").setValue(roleId);
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();
    var selectedIds = Object.keys(
      this.assignmentState.selectedLeafNodeIds
    ).filter((key) => this.assignmentState.selectedLeafNodeIds[key]);

    var deletedItems = this.model.assignments
      .filter((p) => selectedIds.indexOf(p.salesOrgId.toString()) == -1)
      .map((p) => ({
        id: p.id,
        salesOrgId: p.salesOrgId,
      }));

    var upsertedItems = selectedIds
      .filter(
        (p) =>
          this.model.assignments.findIndex(
            (a) => a.salesOrgId.toString() == p
          ) == -1
      )
      .map((id) => ({
        salesOrgId: +id,
      }));

    this.saveModel.assignmentChanges = <any>{
      upsertedItems: <any>upsertedItems,
      deletedItems: <any>deletedItems,
    };

    var roleChanges = {};

    if (this.model.roles.length > 0) {
      var diff = this.model.roles.find(
        (p) => p.roleId == this.c("roleId").value
      );
      if (!diff) {
        roleChanges = {
          upsertedItems: [
            {
              roleId: +this.c("roleId").value,
            },
          ],
          deletedItems: [
            {
              id: this.model.roles[0].id,
              roleId: this.model.roles[0].roleId,
            },
          ],
        };
      }
    } else {
      roleChanges = {
        upsertedItems: [
          <UserRoleDto>{
            roleId: +this.c("roleId").value,
          },
        ],
        deletedItems: [],
      };
    }

    this.saveModel.roleChanges = <any>roleChanges;
  }

  get newModel() {
    return new UserDto({
      isActive: true,
      name: "",
      userName: "",
      registerDate: new Date(),
      roles: [],
      assignments: [],
    });
  }

  mapModelToFormGroup() {
    super.mapModelToFormGroup();
    this.pageBlockUI.start();

    this.assignmentState = {
      ...this.assignmentState,
      selectedLeafNodeIds: {},
    };

    var zone$ = this.getDataService<DataServiceProxy>().getZones(
      undefined,
      undefined,
      undefined,
      undefined,
      undefined
    );
    var area$ = this.getDataService<DataServiceProxy>().getAreas(
      undefined,
      undefined,
      undefined,
      undefined,
      undefined
    );

    var branches$ = this.getDataService<DataServiceProxy>().getBranches(
      true,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined
    );

    const selectedLeafNodeIds = {};

    combineLatest([zone$, area$, branches$])
      .pipe(
        finalize(() => {
          this.pageBlockUI.stop();
        })
      )
      .subscribe(([zones, areas, branches]) => {
        this.nodes = [];
        zones.result.items.forEach((zone) => {
          let zoneNode = {
            id: zone.salesOrgId,
            name: `${zone.code}-${zone.name}`,
            children: [],
          };

          // var zoneSelected = this.model.assignments.find(
          //   (z) => z.salesOrgId == zone.salesOrgId
          // );
          // if (zoneSelected) {
          //   selectedLeafNodeIds[zone.salesOrgId] = true;
          // }

          let childAreas = areas.result.items.filter(
            (p) => p.zoneId == zone.id
          );
          childAreas.forEach((area) => {
            var areaNode = {
              id: area.salesOrgId,
              name: `${area.code}-${area.name}`,
              children: [],
            };

            // var areaSelected = this.model.assignments.find(
            //   (z) => z.salesOrgId == area.salesOrgId
            // );
            // if (areaSelected || zoneSelected) {
            //   selectedLeafNodeIds[area.salesOrgId] = true;
            // }

            let childBranches = branches.result.items.filter(
              (p) => p.areaId == area.id
            );
            childBranches.forEach((branch) => {
              var branchNode = {
                id: branch.salesOrgId,
                name: `${branch.code}-${branch.name}`,
                children: [],
              };

              // var branchSelectd = this.model.assignments.find(
              //   (z) => z.salesOrgId == branch.salesOrgId
              // );
              // if (branchSelectd || areaSelected || zoneSelected) {
              //   selectedLeafNodeIds[branch.salesOrgId] = true;
              // }

              areaNode.children.push(branchNode);
            });

            zoneNode.children.push(areaNode);
          });

          this.nodes.push(zoneNode);
        });

        this.model.assignments.forEach((item) => {
          selectedLeafNodeIds[item.salesOrgId] = true;
        });

        this.assignmentState = {
          ...this.assignmentState,
          selectedLeafNodeIds: selectedLeafNodeIds,
        };
      });
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

  //#endregion

  //#region Form Elements
  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  toggleConfirmPasswordTextType() {
    this.confirmPasswordTextType = !this.confirmPasswordTextType;
  }

  get assignmentVisible() {
    return (
      this.roleCombo?.selectedItem &&
      this.assignmentRoles.find(
        (p) =>
          p.toUpperCase() == this.roleCombo.selectedItem.roleName.toUpperCase()
      )
    );
  }

  get roleReadOnly() {
    return (
      this.readOnly ||
      (this.roleCombo &&
        this.roleCombo.selectedItem &&
        !this.assignmentRoles.find(
          (p) =>
            p.toUpperCase() ==
            this.roleCombo.selectedItem.roleName.toUpperCase()
        ))
    );
  }

  onTreeDeselect($event) {
    this.traverseChildNode($event.node, false);
  }

  onTreeSelect($event) {
    this.traverseChildNode($event.node, true);
  }

  traverseChildNode(node: TreeNode, checked: boolean) {
    if (node.hasChildren) {
      for (let i = 0; i < node.children.length; i++) {
        let childNode = node.children[i];
        childNode.setIsSelected(checked);
        this.traverseChildNode(childNode, checked);
      }
    }
  }

  get passwordRequired(): boolean {
    return !this.model.id;
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getUser(+id);
  }

  updateRequest(id, data): any {
    return this.getDataService<DataServiceProxy>().updateUser(id, data);
  }

  createRequest(data): any {
    return this.getDataService<DataServiceProxy>().createUser(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deleteUser(id);
  }

  //#endregion
}
