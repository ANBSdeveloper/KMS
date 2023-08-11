//#region Import
import { Component, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import {
  ProductClassDto,
  DataServiceProxy,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-product-class-sidebar",
  templateUrl: "./product-class-sidebar.component.html",
  styleUrls: ["./product-class-sidebar.component.scss"],
})
export class ProductClassSidebarComponent
  extends SidebarEditFormComponentBase<ProductClassDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("product_class_content_block") formBlockUI: NgBlockUI;

  entityName = "product_class";
  sidebarName = "product_class_sidebar";
  permissionName = "ProductClasses";
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
    return new ProductClassDto({
      isActive: true,
      name: "",
      code: "",
    });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getProductClass(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateProductClass(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createProductClass(data);
  }
  //#endregion
}
