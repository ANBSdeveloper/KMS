import {
  PosmPriceHeaderDto,
} from "./../../../../../shared/services/data.service";
//#region Import
import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { Validators } from "@angular/forms";
import {
  PosmPriceDetailDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { SidebarEditGridComponentBase } from "@cbms/ng-core-vuexy";
import { PosmUnitTypeComboComponent } from "../../posm-item/posm-unit-type-combo/posm-unit-type-combo.component";
import { PosmCalcTypeComboComponent } from "../../posm-item/posm-calc-type-combo/posm-calc-type-combo.component";
import { PosmItemComboComponent } from "../../posm-item/posm-item-combo/posm-item-combo.component";

//#endregion
@Component({
  selector: "app-posm-price-detail-sidebar",
  templateUrl: "./posm-price-detail-sidebar.component.html",
  styleUrls: ["./posm-price-detail-sidebar.component.scss"],
})
export class PosmPriceDetailSidebarComponent
  extends SidebarEditGridComponentBase<PosmPriceDetailDto>
  implements OnInit
{
  //#region Variables
  @ViewChild("posmCombo") posmCombo: PosmItemComboComponent;
  @ViewChild("posmUnitTypeCombo") posmUnitTypeCombo: PosmUnitTypeComboComponent;
  @ViewChild("posmCalcTypeCombo") posmCalcTypeCombo: PosmCalcTypeComboComponent;
  @BlockUI("posm_price_detail_content_block") formBlockUI: NgBlockUI;
  @Input() posmPrice: PosmPriceHeaderDto;

  entityName = "posm_price_detail";
  sidebarName = "posm_price_detail_sidebar";
  permissionName = "PosmPrices";
  
  //#endregion
  constructor(injector: Injector) {
    super(injector);

    this.formGroup = this.fb.group({});
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      price: [0, [Validators.min(0)]],
      code: [undefined, []],
      name: [undefined, []],
      calcType: [undefined, []],
      unitType: [undefined, []],
      posmItemId: [undefined, [Validators.required]],
    });
  }

  formValidate() {
    var duplicateCodeRow = this.dataSource.items.find(
      (item) => item["rowId"] != this.rowId && item.posmItemId == this.c("posmItemId").value
    );
    if (duplicateCodeRow) {
      this.messageService.toastError(
        this.l("duplicate_code", this.posmCombo.selectedItem.code)
      );
      return false;
    }
    return true;
  }

  mapFormGroupToSaveModel() {
    super.mapFormGroupToSaveModel();
  }

  get newModel() {
    return new PosmPriceDetailDto({
      price: 0,
    });
  }
  //#endregion
  posmItemComboChange(record) {
    this.posmUnitTypeCombo.value = undefined;
    if (!this.c("code").value && record?.selectedItem) {
      this.c("unitType").setValue(record.selectedItem.unitType);
      this.c("code").setValue(record.selectedItem.code);
      this.c("name").setValue(record.selectedItem.name);
      this.c("calcType").setValue(record.selectedItem.calcType);
    }
  }
}
