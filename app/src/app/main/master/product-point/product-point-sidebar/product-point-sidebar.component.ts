//#region Import
import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { BrandDto, DataServiceProxy, ProductPointDto } from "@shared/services/data.service";
import moment from "moment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { filter, takeUntil } from "rxjs/operators";
//#endregion
@Component({
  selector: "app-product-point-sidebar",
  templateUrl: "./product-point-sidebar.component.html",
  styleUrls: ["./product-point-sidebar.component.scss"],
})
export class ProductPointSidebarComponent
  extends SidebarEditFormComponentBase<BrandDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("product_point_content_block") formBlockUI: NgBlockUI;

  entityName = "product_point";
  sidebarName = "product_point_sidebar";
  permissionName = "ProductPoints";
  codeProperty = "productId";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      productId:[undefined,[Validators.required]],
      productCode: [undefined],
      productName: [undefined],
      fromDate: [moment().toDate(),[Validators.required]],
      toDate: [moment().toDate(),[Validators.required]],
      isActive: [{ value: true, disabled: this.readOnly }],
      points:[0,[Validators.required, Validators.min(0)]]
    });

    this.formGroup.valueChanges.subscribe((values) => {
      var toDateControl = this.c("toDate");
      var fromDate = moment(this.cValue("fromDate"));
      var toDate = moment(this.cValue("toDate"));

      if (toDate.isBefore(fromDate)) {
        toDateControl.markAsTouched();
        toDateControl.setErrors({
          ...toDateControl.errors,
          invalidRange: true,
        });
      } else {
        this.cRemoveError(toDateControl, "invalidRange");
      }
    });

    
  }

  cRemoveError(control, validation: string) {
    if (control && control.hasError(validation)) {
      delete control.errors[validation];
      if (Object.keys(control.errors).length == 0) {
        control.setErrors(undefined);
      }
    }
  }
  
  
  get newModel() {
    return new ProductPointDto({
      productId: 0,
      productCode: "",
      productName: "",
      isActive: true,
      fromDate: moment().toDate(),
      toDate: moment().toDate(),
      points: 0,
      unit: "",
      productClassId: 0,
      productClassName: "",
      subProductClassName: "",
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getProductPoint(id);
  }

  updateRequest(id, data) {  
    return this.getDataService<DataServiceProxy>().updateProductPoint(id, data);
  }

  createRequest(data) {    
    return this.getDataService<DataServiceProxy>().createProductPoint(data);
  }
  //#endregion

  

}
