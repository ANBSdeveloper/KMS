import { ProductUnitDto } from './../../../../../shared/services/data.service';
import { ProductComboComponent } from './../../product/product-combo/product-combo.component';
import { ProductUnitComboComponent } from '../../product-unit/product-unit-combo/product-unit-combo.component';
//#region Import
import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { Validators } from "@angular/forms";
import {
  ProductBaseDto,
  RewardItemDto,
  RewardPackageDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import {
  SidebarEditGridComponentBase,
} from "@cbms/ng-core-vuexy";

//#endregion
@Component({
  selector: "app-reward-item-sidebar",
  templateUrl: "./reward-item-sidebar.component.html",
  styleUrls: ["./reward-item-sidebar.component.scss"],
})
export class RewardItemSidebarComponent
  extends SidebarEditGridComponentBase<RewardItemDto>
  implements OnInit
{
  //#region Variables
  @ViewChild("productCombo") productCombo: ProductComboComponent;
  @ViewChild("productUnitCombo") productUnitCombo: ProductUnitComboComponent;
  @BlockUI("reward_item_content_block") formBlockUI: NgBlockUI;
  @Input() rewardPackage: RewardPackageDto;

  entityName = "reward_item";
  sidebarName = "reward_item_sidebar";
  permissionName = "RewardPackages";
  
  //#endregion
  constructor(injector: Injector) {
    super(injector);

    this.formGroup = this.fb.group({});
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      code: [
        undefined,
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
      ],
      name: [undefined, [Validators.required, Validators.maxLength(200)]],
      documentLink: [undefined, [Validators.maxLength(1000)]],
      quantity: [0, [Validators.min(1), Validators.required]],
      price: [0, [Validators.min(0)]],
      productUnitId: [undefined, []],
      productId: [undefined, []],
    });
  }

  formValidate(){
    var duplicateCodeRow = this.dataSource.items.find(item => item["rowId"] != this.rowId && item.code == this.c("code").value);
    if (duplicateCodeRow) {
      this.messageService.toastError(this.l("duplicate_code", duplicateCodeRow.code));
      return false;
    }
    return true;
  }

  mapFormGroupToSaveModel() {
    super.mapFormGroupToSaveModel();  
    const product = <ProductBaseDto>this.productCombo.selectedItem;
    const productUnit = <ProductUnitDto>this.productUnitCombo.selectedItem;
    this.saveModel.productUnitCode = productUnit== undefined? null: productUnit.code;
    this.saveModel.productCode = product == undefined? null: product.code;
  }

  get newModel() {
    return new RewardItemDto({
      price: 0,
      quantity: 1,
    });
  }
  //#endregion
  productComboChange(record) {
    this.productUnitCombo.value = undefined;
    if (!this.c("code").value && record?.selectedItem) {
      this.c("code").setValue(record.selectedItem.code);
      this.c("name").setValue(record.selectedItem.name);
    }
    setTimeout(() => {
      this.productUnitCombo.loadData();
    }, 50);
  }
}
