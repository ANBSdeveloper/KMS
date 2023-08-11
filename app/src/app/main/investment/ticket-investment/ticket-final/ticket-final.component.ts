//#region Import
import {
  Component,
  Injector,
  Input,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";
import { Validators } from "@angular/forms";
import { cloneDeep } from "lodash";
import { FormComponentBase, formHelper } from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  TicketFinalSettlementDto,
  TicketInvestmentDto,
  TicketInvestmentUpsertFinalSettlementCommand,
  TicketInvestmentUpsertFinalSettlementDto,
} from "@shared/services/data.service";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { finalize } from "rxjs/operators";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import moment from "moment";
import { CustomerDevelopmentUserComboComponent } from "@app/main/system/users/customer-development-user-combo/customer-development-user-combo.component";
import { TicketInvestmentStatusDataSource } from "../../data-source/ticket-investmen-status.data-source";
import { RoleType } from "@app/main/system/role/role-type.enum";
//#endregion

@Component({
  selector: "app-ticket-final",
  templateUrl: "./ticket-final.component.html",
  styleUrls: ["./ticket-final.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketFinalComponent extends FormComponentBase {
  @BlockUI() pageBlockUI: NgBlockUI;
  @ViewChild("decideUserCombo")
  decideUserCombo: CustomerDevelopmentUserComboComponent;
  @Input() set investment(value: TicketInvestmentDto) {
    this._investment = value;
    this.mapToForm();
  }

  get investment(): TicketInvestmentDto {
    return this._investment;
  }
  _investment: TicketInvestmentDto;
  model: TicketFinalSettlementDto;

  constructor(
    injector: Injector,
    private statusDataSource: TicketInvestmentStatusDataSource
  ) {
    super(injector);
    this.formGroup = this.fb.group({
      note: [undefined],
      finalDate: [undefined, [Validators.required]],
      acceptanceDate: [undefined],
      decideUserId: [undefined, [Validators.required]],
      decideUserName: [undefined, [Validators.required]],
      updateUserName: [undefined],
      updateTime: [undefined],
    });

    this.formGroup.valueChanges.subscribe((_) => {
      this.validateExtend();
    });
  }

  validateExtend() {
    var acceptanceDate = moment(this.cValue("acceptanceDate"));
    var finalDateControl = this.c("finalDate");
    var finalDate = moment(this.cValue("finalDate"));

    if (finalDate.isBefore(acceptanceDate)) {
      finalDateControl.markAsTouched();
      finalDateControl.setErrors({
        ...finalDateControl.errors,
        invalidRange: true,
      });
    } else {
      this.cRemoveError(finalDateControl, "invalidRange");
    }
  }

  mapToForm() {
    if (this.investment.finalSettlement) {
      this.model = cloneDeep(this.investment.finalSettlement);
    } else {
      this.model = new TicketFinalSettlementDto({
        date: moment().startOf("day").toDate(),
      });
    }

    this.c("note").setValue(this.model.note);
    this.c("finalDate").setValue(this.model.date);
    this.c("acceptanceDate").setValue(
      this.investment.acceptance?.acceptanceDate
    );
    this.c("decideUserId").setValue(this.model.decideUserId);
    this.c("decideUserName").setValue(this.model.decideUserName);
    this.c("updateUserName").setValue(this.model.updateUserName);
    this.c("updateTime").setValue(this.model.lastModificationTime);
  }

  save(complete: boolean) {
    if (!this.formGroup.valid) {
      formHelper.validateAllFormFields(this.formGroup);
      return false;
    }
    this.pageBlockUI.start();
    var command = new TicketInvestmentUpsertFinalSettlementCommand({
      data: new TicketInvestmentUpsertFinalSettlementDto({
        note: this.cValue("note"),
        date: this.cValue("finalDate"),
        decideUserId: this.cValue("decideUserId"),
      }),
      handleType: complete ? "complete" : "",
    });

    this.getDataService<DataServiceProxy>()
      .finalSettlementTicketInvestment(this.investment.id, command)
      .pipe(finalize(() => this.pageBlockUI.stop()))
      .subscribe(
        (_) => {
          this.entityHandler.loadRequest("ticket_final", undefined);
        },
        (error) => {
          this.messageService.toastError(error);
        }
      );
  }

  get readOnly() {
    return (
      this.investment?.status != TicketInvestmentStatus.Acceptance ||
      !(
        this.isGranted("TicketInvestments.FinalSettlement") ||
        this.isGranted("TicketInvestments")
      )
    );
  }

  decideUserChange(e) {
    this.c("decideUserName").setValue(this.decideUserCombo.selectedItem?.name);
  }

  get status() {
    return this.investment
      ? this.statusDataSource.getItemName(this.investment.status)
      : "";
  }

  get roleId() {
    return RoleType.CustomerDevelopmentManager;
  }
}
