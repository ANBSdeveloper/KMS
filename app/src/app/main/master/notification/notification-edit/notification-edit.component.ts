import {
  NotificationBranchDto,
  NotificationDto,
  NotificationUpsertDto,
} from "./../../../../../shared/services/data.service";
import { Validators } from "@angular/forms";
//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { DataServiceProxy } from "@shared/services/data.service";
import {
  DatatableDataSource,
  PageEditFormComponentBase,
} from "@cbms/ng-core-vuexy";
import сBox from "devextreme/ui/check_box";
import { Subject } from "rxjs";
import { debounceTime, finalize } from "rxjs/operators";
import { StatusTypeNotification } from "../../data-source/status-type-notification.enum";
import { FilterBranchByZoneAreaComponent } from "../../filter/filter-branch-by-zone-area/filter-branch-by-zone-area.component";
import { ObjectTypeNotification } from "../../data-source/object-type-notification.enum";
//#endregion
@Component({
  selector: "app-notification-edit",
  templateUrl: "./notification-edit.component.html",
  styleUrls: ["./notification-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class NotificationEditComponent extends PageEditFormComponentBase<
  NotificationDto,
  NotificationUpsertDto,
  NotificationUpsertDto
> {
  //#region Variables
  entityName = "notification";
  permissionName = "Notifications";
  @ViewChild("filterBranchByZoneAreaComponent")
  filterBranchByZoneAreaComponent: FilterBranchByZoneAreaComponent;
  searchZone = 0;
  searchArea = 0;
  //#endregion
  notificationBranchDataSource =
    new DatatableDataSource<NotificationBranchDto>();
  statusType = StatusTypeNotification;
  selectedNotificationBranches = [];
  createUrl = "master/new-notification";
  isDisplayAllBranch = false;

  searchChangeSubject = new Subject<string>();
  searchValue: string = "";

  isAllSelected = false;
  constructor(injector: Injector) {
    super(injector);
  }

  configForm() {
    this.formGroup = this.fb.group({
      code: [undefined],
      description: [
        undefined,
        [Validators.required, Validators.maxLength(200)],
      ],
      shortContent: [
        undefined,
        [Validators.required, Validators.maxLength(200)],
      ],
      content: [undefined, [Validators.required, Validators.maxLength(1000)]],
      status: [this.statusType.HOLDING],
      objectType: [undefined, [Validators.required]],
    });

    this.searchChangeSubject.pipe(debounceTime(300)).subscribe((value) => {
      this.searchValue = value.toLocaleLowerCase();
      this.filterNotificationBranch();
    });
  }

  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();

    if (!this.id) {
      this.pageBlockUI.start();
      this.isDisplayAllBranch = true;
      this.getDataService<DataServiceProxy>()
        .getBranches(
          true,
          undefined,
          undefined,
          undefined,
          undefined,
          undefined
        )
        .pipe(finalize(() => this.pageBlockUI.stop()))
        .subscribe((response) => {
          this.notificationBranchDataSource.setData(
            response.result.items.map(
              (branch) =>
                new NotificationBranchDto({
                  id: 0,
                  isSelected: false,
                  branchId: branch.id,
                  areaName: branch.areaName,
                  branchCode: branch.code,
                  branchName: branch.name,
                  zoneName: branch.zoneName,
                  zoneId: branch.zoneId,
                  areaId: branch.areaId,
                })
            )
          );
          this.filterNotificationBranch();
          this.selectedNotificationBranches = [];
        });
    } else {
      this.notificationBranchDataSource.setData(
        this.model.notificationBranches
      );
      this.filterNotificationBranch();
      this.selectedNotificationBranches =
        this.notificationBranchDataSource.items
          .filter((p) => p.isSelected)
          .map((p) => (<any>p).rowId);
    }

    if (this.duplicateRequest) {
      this.notificationBranchDataSource.transferToNewState();
    }
  }

  mapPropertyFormGroupToSaveModel() {
    this.notificationBranchDataSource.items.forEach((item) => {
      const updateItem = {
        ...item,
        isSelected: false,
      };

      var isSelected =
        this.selectedNotificationBranches.indexOf(item["rowId"]) != -1;
      if (isSelected) {
        updateItem.isSelected = true;
      }

      this.notificationBranchDataSource.updateItem(updateItem);
    });

    this.saveModel.notificationBranchChanges = <any>{
      deletedItems:
        this.notificationBranchDataSource.submitData.upsertedItems.filter(
          (p) => p.id != 0 && !p.isSelected
        ),
      upsertedItems:
        this.notificationBranchDataSource.submitData.upsertedItems.filter(
          (p) => p.isSelected
        ),
    };
  }

  get newModel() {
    return new NotificationDto({
      code: "",
      description: "",
      status: this.statusType.HOLDING,
      content: "",
      shortContent: "",
      objectType: ObjectTypeNotification.SALES,
      notificationBranches: [],
    });
  }

  // //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getNotification(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateNotification(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createNotification(data);
  }

  deleteRequest(id) {
    return this.getDataService<DataServiceProxy>().deleteNotification(id);
  }
  send() {
    var countBranch = 0;
    this.model.notificationBranches.forEach((data) => {
      if (data.id != 0) {
        countBranch++;
      }
    });
    if (countBranch > 0) {
      this.pageBlockUI.start();
      return this.getDataService<DataServiceProxy>()
        .sendNotification(this.model.id)
        .pipe(
          finalize(() => {
            this.pageBlockUI.stop();
          })
        )
        .subscribe(
          () => {
            this.refresh();
          },
          (error) => {
            this.messageService.toastError(error);
          }
        );
    }
    return this.messageService.toastError(this.l("notification_branch_empty"));
  }

  editorPreparing(e) {
    if (e.command === "select") {
      if (this.disableNotificationBranch) {
        e.editorOptions.disabled = true;
      }
    }
  }

  // //#region Branch Table
  selectionChanged(e) {
    if (this.disableNotificationBranch) {
      var originalSelectKeys = this.notificationBranchDataSource.items
        .filter((p) => p.isSelected)
        .map((p) => (<any>p).rowId);
      if (e.currentSelectedRowKeys.length != originalSelectKeys.length) {
        e.component.deselectAll();
        e.component.selectRows(originalSelectKeys);
      }
    } else {
      if (e.currentDeselectedRowKeys.length > 0) {
        (<number[]>e.currentDeselectedRowKeys).forEach((key) => {
          var rowData = this.notificationBranchDataSource.findItem(key);
          this.notificationBranchDataSource.updateItem({
            ...rowData,
            isSelected: false,
          });
        });
      }
      if (e.currentSelectedRowKeys.length > 0) {
        (<number[]>e.currentSelectedRowKeys).forEach((key) => {
          var rowData = this.notificationBranchDataSource.findItem(key);
          this.notificationBranchDataSource.updateItem({
            ...rowData,
            isSelected: true,
          });
        });
      }
    }
  }

  allSelectedChange(e) {
    this.isAllSelected = e.target.checked;
    this.notificationBranchDataSource.items.forEach((item) => {
      this.notificationBranchDataSource.updateItem({
        ...item,
        isSelected: this.isAllSelected,
      });
    });
  }
  searchChange(e) {
    this.searchChangeSubject.next(e);
  }
  displayAllBranchChange(e) {
    this.isDisplayAllBranch = e;
    this.filterNotificationBranch();
  }

  filterNotificationBranch() {
    this.notificationBranchDataSource.applyFilter(
      (item) =>
        (item.branchCode.toLocaleLowerCase().indexOf(this.searchValue) != -1 ||
          item.branchName.toLocaleLowerCase().indexOf(this.searchValue) !=
            -1) &&
        (this.isDisplayAllBranch ||
          (!this.isDisplayAllBranch && item.id != 0)) &&
        (this.searchZone == 0 || item.zoneId == this.searchZone) &&
        (this.searchArea == 0 || item.areaId == this.searchArea)
    );
    this.selectedNotificationBranches = this.notificationBranchDataSource.items
      .filter((p) => p.isSelected)
      .map((p) => (<any>p).rowId);
  }
  get objectTypeReadOnly() {
    if (this.model.status == this.statusType.HOLDING) {
      return false;
    } else {
      return true;
    }
  }
  get sendVisible() {
    if (
      this.model.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName) &&
      this.model.id != undefined
    ) {
      return true;
    } else {
      return false;
    }
  }
  get updateVisible() {
    if (
      this.model.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName)
    ) {
      return true;
    } else {
      return false;
    }
  }
  get deleteVisible() {
    if (
      this.model.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName) &&
      this.model.id != undefined
    ) {
      return true;
    } else {
      return false;
    }
  }
  get disableNotificationBranch() {
    if (
      this.model.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName)
    ) {
      return false;
    } else {
      return true;
    }
  }

  get readOnly(): boolean {
    return (
      !(
        (this.model[this.idProperty] && this.isUpdateGranted) ||
        (!this.model[this.idProperty] && this.isCreateGranted)
      ) || this.model.status != this.statusType.HOLDING
    );
  }

  zoneChange(item) {
    this.searchZone = item == null ? 0 : item;
    this.searchArea = 0;
    this.filterNotificationBranch();
  }

  areaChange(item) {
    this.searchArea = item == null ? 0 : item;
    this.filterNotificationBranch();
  }
  //#endregion
}
