//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { MaterialDto, DataServiceProxy } from "@shared/services/data.service";
import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import { FormGroup } from "@angular/forms";
import { debounceTime, finalize, takeUntil } from "rxjs/operators";
import { report } from "node:process";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "environments/environment";
import { Observable } from "rxjs";
import { ToastrService } from "ngx-toastr";
//#endregion
@Component({
  selector: "app-material-list",
  templateUrl: "./material-list.component.html",
  styleUrls: ["./material-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class MaterialListComponent extends DxListComponentBase<MaterialDto> {
  //#region Variables
  entityName = "material";
  sidebarName = "material_sidebar";
  permissionName = "Materials";
  formGroup: FormGroup;
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
      statusActive: [undefined],
      materialTypeId: [undefined],
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
    return this.getDataService<DataServiceProxy>().getMaterials(
      isActive,
      this.cValue("materialTypeId") ? this.cValue("materialTypeId") : undefined,
      pageSize,
      skip,
      search,
      sort,
      undefined
    );
  }
  deleteRequest(id: number) {
    return this.getDataService<DataServiceProxy>().deleteMaterial(id);
  }

  export() {
    let url = environment.apiUrl + "/api/v1/materials/export";

    let options_: any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        Accept: "text/plain",
      }),
    };

    return this.http.post(url, {}, options_).subscribe((res) => {
      console.log(res);
      this.downloadFileFromBlob("material.xlsx", (<any>res).body);
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
    let url = environment.apiUrl + "/api/v1/materials/import";
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
  //#endregion
}
