//#region Import
import { Component, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { ProductBaseDto, DataServiceProxy } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { filter, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-product-sidebar",
  templateUrl: "./product-sidebar.component.html",
  styleUrls: ["./product-sidebar.component.scss"],
})
export class ProductSidebarComponent
  extends SidebarEditFormComponentBase<ProductBaseDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("product_content_block") formBlockUI: NgBlockUI;

  entityName = "product";
  sidebarName = "product_sidebar";
  permissionName = "Products";
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
      point:[0,[Validators.min(0)]]
    });
  }
  
  // checkChange() {
  //   let flat = false;
  //   Object.keys(this.formGroup.controls).forEach((key) => {
  //     if (this.model.hasOwnProperty(key)) {
  //       if (this.prevModel[key] != this.formGroup.controls[key].value) {
  //         console.log(key);
  //         flat = true;
  //         return false;
  //       }
  //     }
  //   });

  //   this.hasChange = flat;
  // }

  get newModel() {
    return new ProductBaseDto({
      name: "",
      code: "",
      point: 0

  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getProduct(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateProduct(id, data);
  }

  //#endregion
}
