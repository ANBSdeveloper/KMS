import {
  ProductItemDto,
  UpsertProductDto,
} from "./../../../../../shared/services/data.service";
//#region Import
import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { DataServiceProxy } from "@shared/services/data.service";
import { PageEditFormComponentBase } from "@cbms/ng-core-vuexy";

//#endregion
@Component({
  selector: "app-product-edit",
  templateUrl: "./product-edit.component.html",
  styleUrls: ["./product-edit.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ProductEditComponent extends PageEditFormComponentBase<
  ProductItemDto,
  UpsertProductDto,
  UpsertProductDto
> {
  //#region Variables
  entityName = "product-edit";
  permissionName = "Products";
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  configForm() {
    this.formGroup = this.fb.group({
      code: [undefined],
      caseUnit: [undefined],
      name: [undefined],
      unit: [undefined],
      productClassName: [undefined],
      packSize: [undefined],
      subProductClassName: [undefined],
      description: [undefined],
      brandName: [undefined],
      status: [undefined],
    });
  }



  mapPropertyModelToFormGroup() {
    super.mapPropertyModelToFormGroup();
  }

  mapPropertyFormGroupToSaveModel() {
    super.mapPropertyFormGroupToSaveModel();
  }

  // //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getProductItem(id);
  }
  updateRequest(id, data): any {
    return this.getDataService<DataServiceProxy>().updateProductItem(id, data);
  }
  //#endregion
}
