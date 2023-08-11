import {
  InvestmentCustomerSettingDto,
  TicketMaterialDto,
} from "../../../../../shared/services/data.service";
//#region Import
import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { Validators } from "@angular/forms";
import { MaterialDto } from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { SidebarEditGridComponentBase } from "@cbms/ng-core-vuexy";
import { MaterialComboComponent } from "@app/main/master/material/material-combo/material-combo.component";
import { formatNumber } from "devextreme/localization";

//#endregion
@Component({
  selector: "app-ticket-material-sidebar",
  templateUrl: "./ticket-material-sidebar.component.html",
  styleUrls: ["./ticket-material-sidebar.component.scss"],
})
export class TicketMaterialSidebarComponent
  extends SidebarEditGridComponentBase<TicketMaterialDto>
  implements OnInit
{
  //#region Variables
  @ViewChild("materialCombo") materialCombo: MaterialComboComponent;
  @BlockUI("ticket_material_content_block") formBlockUI: NgBlockUI;
  @Input() setting: InvestmentCustomerSettingDto;

  entityName = "ticket_material";
  sidebarName = "ticket_material_sidebar";
  permissionName = "TicketInvestments";

  //#endregion
  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      materialId: [
        undefined,
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
      ],

      registerQuantity: [
        undefined,
        [Validators.min(1), Validators.maxLength(1000)],
      ],
      amount: [0, [Validators.min(1), Validators.required]],
      price: [0, [Validators.min(0)]],
      isDesign: [null, []],
    });

    this.formGroup.valueChanges.subscribe((_) => {
      var amount = this.cValue("price") * this.cValue("registerQuantity");
      if (amount != this.cValue("amount")) {
        this.c("amount").setValue(amount);
      }
    });
  }

  get isCreateGranted(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("TicketInvestments.Register")
    );
  }
  get isUpdateGranted(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("TicketInvestments.Register")
    );
  }

  get isDeleteGranted(): boolean {
    return (
      this.isGranted(this.permissionName) ||
      this.isGranted("TicketInvestments.Register")
    );
  }

  formValidate() {
    var duplicateRow = this.dataSource.items.find(
      (item) =>
        item["rowId"] != this.rowId &&
        item.materialId == this.c("materialId").value
    );
    if (duplicateRow) {
      this.messageService.toastError(
        this.l("duplicate_code", duplicateRow.materialName)
      );
      return false;
    }
    var totalAmount = this.c("amount").value;

    this.dataSource.items
      .filter((p) => p.materialId != this.c("materialId").value)
      .forEach((p) => (totalAmount += p.amount));

    if (totalAmount > this.setting.maxInvestAmount) {
      this.messageService.toastError(
        this.l(
          "over_max_material_amount",
          formatNumber(this.setting.maxInvestAmount, "#,##0")
        )
      );
      return false;
    }

    return true;
  }

  mapFormGroupToSaveModel() {
    super.mapFormGroupToSaveModel();
    const material = <MaterialDto>this.materialCombo.selectedItem;
    this.saveModel.materialName = material?.name;
    this.saveModel.materialCode = material?.code;
  }

  materialChange(e) {
    if (e.selectedItem) {
      this.c("price").setValue(e.selectedItem.value);
      this.c("isDesign").setValue(e.selectedItem.isDesign);
    } else {
      this.c("price").setValue(0);
      this.c("isDesign").setValue(false);
    }
  }

  get newModel() {
    return new TicketMaterialDto({
      materialId: undefined,
      price: 0,
      registerQuantity: 1,
    });
  }
  //#endregion
}
