//#region Import
import {
  Component,
  Injector,
  ViewEncapsulation,
} from "@angular/core";
import { PermissionComponentBase } from "@cbms/ng-core-vuexy";
import { DynamicDialogRef, DynamicDialogConfig } from "primeng/dynamicdialog";
//#endregion
@Component({
  selector: "app-customer-sales-monthly-dialog",
  templateUrl: "./customer-sales-monthly-dialog.component.html",
  styleUrls: ["./customer-sales-monthly-dialog.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class CustomerSalesMonthlyDialogComponent extends PermissionComponentBase {
  monthData = [];
  constructor(
    injector: Injector,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) {
    super(injector);

    
  }

  ngOnInit() {
    this.monthData = this.config.data;
  }
   
  
  close() {
    this.ref.close();
  }
  //#endregion
}
