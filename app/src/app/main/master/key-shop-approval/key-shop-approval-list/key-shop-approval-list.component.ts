//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  DataServiceProxy,
  CustomerApproveKeyShopListDto,
  CustomerApproveKeyShopCommand,
  CustomerRefuseKeyShopCommand,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { ZoneComboComponent } from "../../zone/zone-combo/zone-combo.component";
import { AreaComboComponent } from "../../area/area-combo/area-combo.component";
import { KeyShopStatus } from "../model/key-shop-status.enum";
import dxDataGrid from "devextreme/ui/data_grid";
//#endregion
@Component({
  selector: "app-key-shop-approval-list",
  templateUrl: "./key-shop-approval-list.component.html",
  styleUrls: ["./key-shop-approval-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class KeyShopApprovalListComponent extends DxListComponentBase<CustomerApproveKeyShopListDto> {
  //#region Variables
  keyShopStatus = KeyShopStatus;
  entityName = "key-shop-approval";
  permissionName = "Customers";
  //#endregion
  selectedKeyShops = [];
  idSave = [];
  @ViewChild("zoneCombo") zoneCombo: ZoneComboComponent;
  @ViewChild("areaCombo") areaCombo: AreaComboComponent;
  constructor(injector: Injector) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      zone: [undefined],
      area: [undefined],
    });
    this.filterFormGroup.valueChanges
      .pipe(debounceTime(300), takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.dataGrid.instance.refresh();
      });
    super.init();
  }

  get approveEnable(): boolean {
    return this.selectedKeyShops.length > 0;
  }
  get refuseEnable(): boolean {
    return this.selectedKeyShops.length > 0;
  }

  //#region Api Request
  getListRequest(pageSize: number, skip: number, search: string, sort: string) {
    var zone = this.c("zone").value ? this.c("zone").value : 0;
    var area = this.c("area").value ? this.c("area").value : 0;
    return this.getDataService<DataServiceProxy>().getCustomerApproveKeyShopList(
      zone,
      area,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  //#endregion
  get approveVisible(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("Customers.ApproveKeyShop")
    );
  }
  get refuseVisible(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("Customers.ApproveKeyShop")
    );
  }

  refresh() {
    super.refresh();
    this.dataGrid.instance.clearSelection();
  }
  approve(): any {
    // loading
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .approveKeyShop(
        new CustomerApproveKeyShopCommand({
          data: this.selectedKeyShops.map((p) => +p.id),
        })
      )
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
  refuse(): any {
    // loading
    this.pageBlockUI.start();

    return this.getDataService<DataServiceProxy>()
      .refuseKeyShop(
        new CustomerRefuseKeyShopCommand({
          data: this.selectedKeyShops.map((p) => +p.id),
        })
      )
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

  zoneChange(record) {
    this.areaCombo.value = undefined;
    setTimeout(() => {
      this.areaCombo.loadData();
    }, 50);
  }
  headerThirdState = false;
  onSelectionChanged(e) {
    console.log(e);
    console.log(this.dataGrid.instance.getVisibleRows());
    let rows = this.dataGrid.instance.getVisibleRows().length;

    if (this.headerThirdState && e.selectedRowsData.length == rows) {
      this.headerThirdState = false;
      this.dataGrid.instance.clearSelection();
    } else {
      this.selectedKeyShops = e.selectedRowsData;
      let disableRows = this.selectedKeyShops
        .filter((item) => item.keyShopStatus != KeyShopStatus.Created)
        .map((item) => item.id);
      e.component.deselectRows(disableRows);
      if (e.selectedRowKeys.length >= 1 && rows >= 2) {
        this.headerThirdState = true;
      } else {
        this.headerThirdState = false;
      }
    }

    // if (e.currentSelectedRowKeys.length == e) {
    //   setTimeout(() => this.dataGrid.instance.clearSelection(), 200);
    // } else {
    // if (e.selectedRowsData.length == e.selectedRowKeys.length) {
    //
    // } else {
    //   setTimeout(() => this.dataGrid.instance.clearSelection(), 200);
    // }
  }
  // }
}
