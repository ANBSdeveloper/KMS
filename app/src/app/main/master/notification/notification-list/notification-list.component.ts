//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DataServiceProxy,
  NotificationDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { StatusTypeNotificationDataSource } from "../../data-source/status-type-notification.data-source";
import { StatusTypeNotification } from "../../data-source/status-type-notification.enum";
//#endregion
@Component({
  selector: "notification-list",
  templateUrl: "./notification-list.component.html",
  styleUrls: ["./notification-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class NotificationListComponent extends DxListComponentBase<NotificationDto> {
  //#region Variables
  entityName = "notification";
  permissionName = "Notifications";
  codeProperty;
  //#endregion
  statusType = StatusTypeNotification;
  selectedKeyShops = [];
  idSave = [];
  constructor(
    injector: Injector,
    public statusDataSource: StatusTypeNotificationDataSource
  ) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusType: [undefined],
      objectType: [undefined],
    });
    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var statusType = this.c("statusType").value
      ? this.c("statusType").value
      : undefined;
    return this.getDataService<DataServiceProxy>().getNotifications(
      statusType,
      this.c("objectType").value ? this.c("objectType").value : undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  //#endregion
  get approveVisible(): boolean {
    return this.isGranted(this.permissionName);
  }

  showDetail(row: NotificationDto) {
    this.router.navigate([`master/notification/${row.id}`]);
  }

  create() {
    this.router.navigate([`master/new-notification`]);
  }
  send(row: any) {
    this.pageBlockUI.start();
    var countBranch = 0;
    this.getDataService<DataServiceProxy>()
      .getNotification(row.id)
      .pipe(finalize(() => {}))
      .subscribe(
        (response) => {
          response.result.notificationBranches.forEach((data) => {
            if (data.id != 0) {
              countBranch++;
            }
          });
          if (countBranch > 0) {
            return this.getDataService<DataServiceProxy>()
              .sendNotification(row.id)
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
          this.pageBlockUI.stop();
          return this.messageService.toastError(
            this.l("notification_branch_empty")
          );
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  //#endregion
  //#region Api Request
  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteNotification(id);
  }

  sendVisible(data: any) {
    if (
      data.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName)
    ) {
      return true;
    } else {
      return false;
    }
  }
  deleteVisible(data: any) {
    if (
      data.status == this.statusType.HOLDING &&
      this.isGranted(this.permissionName)
    ) {
      return true;
    } else {
      return false;
    }
  }
}
