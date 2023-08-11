//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  DataServiceProxy,
  ProductPointDto,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { environment } from "environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-product-point-list",
  templateUrl: "./product-point-list.component.html",
  styleUrls: ["./product-point-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ProductPointListComponent extends DxListComponentBase<ProductPointDto> {
  //#region Variables
  entityName = "product_point";
  sidebarName = "product_point_sidebar";
  permissionName = "ProductPoints";
  //#endregion

  constructor(
    injector: Injector,
    private http: HttpClient,
    private toastrService: ToastrService
  ) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      productClassId: [undefined],
      statusActive: [undefined],
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
    var productClass = this.c("productClassId").value
      ? this.c("productClassId").value
      : undefined;
    var isActive =
      this.c("statusActive").value == "1"
        ? true
        : this.c("statusActive").value == "0"
        ? false
        : undefined;
    return this.getDataService<DataServiceProxy>().getProductPoints(
      productClass,
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteProductPoint(id);
  }
  //#endregion
  export() {
    let url = environment.apiUrl + "/api/v1/product-points/export";

    let options_: any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        Accept: "text/plain",
      }),
    };
    this.loading = true;
    return this.http
      .post(url, {}, options_)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res) => {
        this.downloadFileFromBlob("product_point.xlsx", (<any>res).body);
      });
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
    let url = environment.apiUrl + "/api/v1/product-points/import";
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
          //this.messageService.toastError();
          this.refresh();
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
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
}
