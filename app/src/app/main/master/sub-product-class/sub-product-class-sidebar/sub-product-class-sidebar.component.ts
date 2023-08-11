//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { BrandDto, DataServiceProxy, SubProductClassDto } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { filter, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-sub-product-class-sidebar",
  templateUrl: "./sub-product-class-sidebar.component.html",
  styleUrls: ["./sub-product-class-sidebar.component.scss"],
})
export class SubProductClassSidebarComponent
  extends SidebarEditFormComponentBase<SubProductClassDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("sup_product_class_content_block") formBlockUI: NgBlockUI;

  entityName = "sub_product_class";
  sidebarName = "sub_product_class_sidebar";
  permissionName = "SubProductClasses";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      code: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
      ],
      name: ["", [Validators.required, Validators.maxLength(250)]],
      isActive: [{ value: true, disabled: this.readOnly }],
    });
  }
  
  get newModel() {
    return new BrandDto({
      isActive: true,
      name: "",
      code: "",
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getSubProductClass(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateSubProductClass(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createSubProductClass(data);
  }
  //#endregion

  @HostListener('window:keydown', ['$event'])
  keydown(e: KeyboardEvent) {
    if (
     (<any>this.sidebar).isOpened && e.key == 's' &&
      (navigator.platform.match('Mac') ? e.metaKey : e.ctrlKey)
    ) {
      console.log(e);
      e.preventDefault();
      this.submit();
    }
  }
}
