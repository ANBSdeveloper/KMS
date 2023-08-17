//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import {
  PosmPriceHeaderDto,
  DataServiceProxy,
} from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { Validators } from "@angular/forms";
import moment from "moment";
import { environment } from "environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-posm-price-list",
  templateUrl: "./posm-price-list.component.html",
  styleUrls: ["./posm-price-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmPriceListComponent extends DxListComponentBase<PosmPriceHeaderDto> {
  //#region Variables
  entityName = "posm_price";
  permissionName = "PosmPrices";
  isActive = true;
  //#endregion
  constructor(
    injector: Injector,
    public http: HttpClient,
    public toastrService: ToastrService
  ) {
    super(injector);
  }

  init() {
    this.filterFormGroup = this.fb.group({
      fromDate: [
        moment().add("month", -1).startOf("day").toDate(),
        [Validators.required],
      ],
      toDate: [moment().endOf("day").toDate(), [Validators.required]],
      search: [""],
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
    var fromDate = this.c("fromDate").value
      ? this.c("fromDate").value
      : undefined;
    var toDate = this.c("toDate").value ? this.c("toDate").value : undefined;
    var isActive =
    this.c("statusActive").value == "1"
      ? true
      : this.c("statusActive").value == "0"
      ? false
      : undefined;
    return this.getDataService<DataServiceProxy>().getPosmPrices(
      fromDate,
      toDate,
      isActive,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }

  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deletePosmPrice(id);
  }
  //#endregion

  showDetail(row: PosmPriceHeaderDto) {
    this.router.navigate([`master/posm-price/${row.id}`]);
  }

  create() {
    this.router.navigate([`master/new-posm-price`]);
  }

  toDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("toDate").setValue(undefined);
        this.messageService.toastError(this.l("error_todate"));
      }
    }
  }

  fromDateChange() {
    if (
      this.c("toDate").value != undefined &&
      this.c("fromDate").value != undefined
    ) {
      if (this.c("toDate").value < this.c("fromDate").value) {
        this.c("fromDate").setValue(undefined);
        this.messageService.toastError(this.l("error_fromdate"));
      }
    }
  }

  export() {
    let url = environment.apiUrl + "/api/v1/posm-prices/export";

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
      this.downloadFileFromBlob("posm-price.xlsx", (<any>res).body);
      this.pageBlockUI.stop();
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
    let url = environment.apiUrl + "/api/v1/posm-prices/import";
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
