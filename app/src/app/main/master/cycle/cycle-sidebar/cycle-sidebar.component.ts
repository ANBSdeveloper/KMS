import { Validators } from '@angular/forms';
//#region Import
import { Component, Injector, Input, OnInit } from "@angular/core";
import { SidebarEditFormComponentBase } from "@cbms/ng-core-vuexy";
import { CycleDto, DataServiceProxy } from "@shared/services/data.service";
import moment from "moment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
//#endregion
@Component({
  selector: "app-cycle-sidebar",
  templateUrl: "./cycle-sidebar.component.html",
  styleUrls: ["./cycle-sidebar.component.scss"],
})
export class CycleSidebarComponent
  extends SidebarEditFormComponentBase<CycleDto>
  implements OnInit
{
  //#region Variables
  @BlockUI("cycle_content_block") formBlockUI: NgBlockUI;
  @Input() dataCycle = [];
  entityName = "cycle";
  sidebarName = "cycle_sidebar";
  permissionName = "Cycles";
  fromDateMax=undefined;
  toDateMin=undefined;
  //#endregion

  constructor(injector: Injector) {
    super(injector);
  }

  //#region Form & Model
  configForm() {
    this.formGroup = this.fb.group({
      number: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(10),
        ],
      ],
      year: ["", [Validators.required]],
      isActive: [{ value: true, disabled: this.readOnly }],
      fromDate:[undefined,[Validators.required]],
      toDate:[undefined,[Validators.required]],
    });
  }
  
  get newModel() {
    return new CycleDto({
      isActive: true,
      number: "",
      year: moment().year(),
      // fromDate: moment().toDate(),
      // toDate:moment().toDate(),
  });
  }
  //#endregion

  //#region Api Request
  getRequest(id) {
    return this.getDataService<DataServiceProxy>().getCycle(id);
  }

  updateRequest(id, data) {
    return this.getDataService<DataServiceProxy>().updateCycle(id, data);
  }

  createRequest(data) {
    return this.getDataService<DataServiceProxy>().createCycle(data);
  }
  //#endregion

  toDateChange(){
    this.fromDateMax = this.c("toDate").value;
    if(this.c("toDate").value!=undefined && this.c("fromDate").value!=undefined){
      if(this.c("toDate").value < this.c("fromDate").value){
        //this.messageService.toastError(this.l("error_todate"));
      }
    }   
  }

  fromDateChange(){
    this.toDateMin = this.c("fromDate").value;
    if(this.c("toDate").value!=undefined && this.c("fromDate").value!=undefined){
      if(this.c("toDate").value < this.c("fromDate").value){
        //this.messageService.toastError(this.l("error_fromdate"));
      }
    }      
  }
}
