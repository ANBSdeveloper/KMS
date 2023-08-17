import { ProductUnitDto } from './../../../../../shared/services/data.service';
import { ProductComboComponent } from './../../product/product-combo/product-combo.component';
import { ProductUnitComboComponent } from '../../product-unit/product-unit-combo/product-unit-combo.component';
//#region Import
import { Component, Injector, Input, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import {
  PosmCatalogDto,
  RewardPackageDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import {
  SidebarEditGridComponentBase,
} from "@cbms/ng-core-vuexy";

//#endregion
@Component({
  selector: "app-posm-catalog-sidebar",
  templateUrl: "./posm-catalog-sidebar.component.html",
  styleUrls: ["./posm-catalog-sidebar.component.scss"],
})
export class PosmCatalogSidebarComponent
  extends SidebarEditGridComponentBase<PosmCatalogDto>
  implements OnInit
{
  //#region Variables
 
  @BlockUI("posm_catalog_content_block") formBlockUI: NgBlockUI;
  @Input() rewardPackage: RewardPackageDto;

  entityName = "posm_catalog";
  sidebarName = "posm_catalog_sidebar";
  permissionName = "PosmItems";
  
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
      link: [undefined, [Validators.maxLength(1000)]]
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
  }

  get newModel() {
    return new PosmCatalogDto({
    });
  }
  //#endregion
}
