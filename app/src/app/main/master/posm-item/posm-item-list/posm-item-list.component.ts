//#region Import
import {
  Component,
  Injector,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import {
  PosmItemDto,
  DataServiceProxy,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { StatusActiveComboComponent } from "../../status-active-combo/status-active-combo.component";
import { PosmUnitTypeDataSource } from "../data-source/posm-unit-type.data-source";
import { PosmCalcTypeDataSource } from "../data-source/posm-calc-type.data-source";
import { environment } from "environments/environment";
import { HttpHeaders } from "@angular/common/http";
import { HttpClient } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-posm-item-list",
  templateUrl: "./posm-item-list.component.html",
  styleUrls: ["./posm-item-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmItemListComponent extends DxListComponentBase<PosmItemDto> {
  //#region Variables
  entityName = "posm_item";
  permissionName = "PosmItems";
  isActive = true;
  //#endregion
  constructor(
    injector: Injector,
    public posmUnitTypeDataSource: PosmUnitTypeDataSource,
    public posmCalcTypeDataSource: PosmCalcTypeDataSource,
    public http: HttpClient,
    public toastrService: ToastrService
  ) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      statusActive: [undefined],
      search: [""],
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
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getPosmItems(
      isActive,
      undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deletePosmItem(id);
  }
  //#endregion

  showDetail(row: PosmItemDto) {
    this.router.navigate([`master/posm-item/${row.id}`]);
  }

  create() {
    this.router.navigate([`master/new-posm-item`]);
  }

  export() {
    let url = environment.apiUrl + "/api/v1/posm-items/export";

    let options_: any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        Accept: "text/plain",
      }),
    };
    this.pageBlockUI.start();
    return this.http.post(url, {}, options_).subscribe((res) => {
      console.log(res);
      this.downloadFileFromBlob("posm-item.xlsx", (<any>res).body);
      this.pageBlockUI.start();
    });
  }
  public downloadFileFromBlob(name, blob) {
    var url = window.URL.createObjectURL(blob);
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style.display = "none";
    a.href = url;
    a.download = name;
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
  }
  onReadFile = (e) => {
    if (e.target.files && e.target.files[0]) {
      var file = e.target.files[0];
      var regex = /^(.)+(.xls|.xlsx)$/;
      if (!regex.test(file.name)) {
        this.messageService.toastError(this.l("error-import-file"));
        return;
      }
      if (file.length / (1024 * 1024) > 10) {
        this.messageService.toastError(this.l("error-import-size"));
        return;
      }

      this.import(file);

      e.target.value = null;
    }
  };

  import(file) {
    this.pageBlockUI.start();
    let url = environment.apiUrl + "/api/v1/posm-items/import";
    const form = new FormData();
    form.append("file", file, file.name);
    return this.http
      .post<string>(url, form)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        () => {
          this.toastrService.success(this.l("import-success"), "", {
            toastClass: "toast ngx-toastr",
            closeButton: true,
            positionClass: "toast-bottom-center",
          });
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

}
