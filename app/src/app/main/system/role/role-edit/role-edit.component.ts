//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { FormGroup, Validators } from "@angular/forms";
import {
  DatatableDataSource,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import { ITreeState, TreeNode } from "@circlon/angular-tree-component";
import {
  DataServiceProxy,
  RoleDto,
  UpsertRoleDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";

// UpsertUserDto,
// UserDto,
// UserRoleDto,

//#endregion
@Component({
  selector: "app-role-edit",
  templateUrl: "./role-edit.component.html",
  styleUrls: ["./role-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class RoleEditComponent extends PageEditFormComponentBase<
  RoleDto,
  UpsertRoleDto,
  UpsertRoleDto
> {
  //#region Variables
  permissionName = "Roles";
  codeProperty = "roleName";
  //#endregion
  @BlockUI() pageBlockUI: NgBlockUI;

  formGroup: FormGroup;

  optionsFilter = {
    useCheckbox: true,
    useTriState: false,
  };
  nodes = [];
  assignmentState: ITreeState;
  roleItemDataSource = new DatatableDataSource<RoleDto>();
  constructor(injector: Injector, public dataService: DataServiceProxy) {
    super(injector);
  }

  get newModel() {
    return new RoleDto({
      isActive: true,
      roleName: "",
      displayName: "",
      description: "",
      permissions: [],
    });
  }

  configForm() {
    this.formGroup = this.fb.group({
      roleName: [
        "",
        [
          Validators.required,
          Validators.maxLength(200),
          Validators.minLength(2),
        ],
      ],
      displayName: [
        "",
        [
          Validators.required,
          Validators.maxLength(200),
          Validators.minLength(2),
        ],
      ],
      description: ["", [Validators.maxLength(2000), Validators.minLength(2)]],
      isActive: [false],
    });
  }
  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();
    var selectedPermissionIds = Object.keys(
      this.assignmentState.selectedLeafNodeIds
    ).filter((key) => this.assignmentState.selectedLeafNodeIds[key]);

    var deletedItems = this.model.permissions
      .filter(
        (p) => selectedPermissionIds.indexOf(p.permissionId.toString()) == -1
      )
      .map((p) => ({
        id: p.id,
        permissionId: p.permissionId,
      }));

    var upsertedItems = selectedPermissionIds
      .filter(
        (p) =>
          this.model.permissions.findIndex(
            (a) => a.permissionId.toString() == p
          ) == -1
      )
      .map((permissionId) => ({
        permissionId: +permissionId,
      }));

    this.saveModel.permissionChanges = <any>{
      upsertedItems: <any>upsertedItems,
      deletedItems: <any>deletedItems,
    };
  }

  mapModelToFormGroup() {
    super.mapModelToFormGroup();
    this.pageBlockUI.start();

    this.assignmentState = {
      ...this.assignmentState,
      selectedLeafNodeIds: {},
    };

    this.dataService
      .getPermissions(undefined, undefined, undefined, undefined, undefined)
      .subscribe((permisions) => {
        this.nodes = [];
        permisions.result.items
          .filter((p) => !p.parentId)
          .forEach((permission) => {
            let roleNode = {
              id: permission.id,
              name: permission.name,
              children: [],
            };

            let childPermissions = permisions.result.items.filter(
              (p) => p.parentId == permission.id
            );
            childPermissions.forEach((child) => {
              roleNode.children.push({
                id: child.id,
                name: child.name,
                children: [],
              });
            });

            this.nodes.push(roleNode);
          });

        const selectedLeafNodeIds = {};

        this.model.permissions.forEach((item) => {
          selectedLeafNodeIds[item.permissionId] = true;
        });

        this.assignmentState = {
          ...this.assignmentState,
          selectedLeafNodeIds: selectedLeafNodeIds,
        };
        this.pageBlockUI.stop();
      });
  }

  get assignmentVisible() {
    return true;
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
  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getRole(+id);
  }

  updateRequest(id, data): any {
    return this.getDataService<DataServiceProxy>().updateRole(id, data);
  }

  createRequest(data): any {
    return this.getDataService<DataServiceProxy>().createRole(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deleteRole(id);
  }
}
