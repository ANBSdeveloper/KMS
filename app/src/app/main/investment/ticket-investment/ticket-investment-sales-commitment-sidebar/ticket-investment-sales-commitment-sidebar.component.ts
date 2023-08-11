//#region Import
import {
  Component,
  EventEmitter,
  Injector,
  Input,
  OnInit,
  Output,
} from "@angular/core";
import {
  TicketInvestmentDto,
  TicketSalesCommitmentDto,
} from "@shared/services/data.service";
import {
  EntityHandlerService,
  PermissionComponentBase,
} from "@cbms/ng-core-vuexy";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import moment from "moment";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";
import { CoreSidebarService } from "@core/components/core-sidebar/core-sidebar.service";

//#endregion

//#endregion
@Component({
  selector: "app-ticket-investment-sales-commitment-sidebar",
  templateUrl: "./ticket-investment-sales-commitment-sidebar.component.html",
  styleUrls: ["./ticket-investment-sales-commitment-sidebar.component.scss"],
})
export class TicketInvestmentSalesCommitmentSidebarComponent
  extends PermissionComponentBase
  implements OnInit
{
  //#region Variables
  @Input() investment: TicketInvestmentDto;
  @Input() fromDate: Date;
  @Input() toDate: Date;
  @Input() data: TicketSalesCommitmentDto[];
  @Input() readOnly = false;

  @Output() update = new EventEmitter<TicketSalesCommitmentDto[]>();
  formGroup: FormGroup;

  entityName = "ticket_investment_sales_commitment";
  sidebarName = "ticket_investment_sales_commitment_sidebar";

  unsubscribe$: Subject<any>;
  //#endregion
  constructor(
    injector: Injector,
    private sidebarService: CoreSidebarService,
    private fb: FormBuilder,
    private entityHandler: EntityHandlerService
  ) {
    super(injector);
    this.unsubscribe$ = new Subject();
  }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      commitments: this.fb.array([]),
    });

    this.entityHandler
      .registerLoadingRequest(this.entityName)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((p) => {
        this.init();
      });
  }

  init() {
    this.formGroup = this.fb.group({
      commitments: this.fb.array([]),
    });

    if (this.data && this.data.length > 0) {
      for (var i = 0; i < this.data.length; i++) {
        const commitmentForm = this.fb.group({
          year: [this.data[i].year],
          yearMonth: [
            moment()
              .set("year", this.data[i].year)
              .set("month", this.data[i].month)
              .format("yyyy-MM"),
          ],
          month: [this.data[i].month],
          amount: [this.data[i].amount, Validators.required],
        });
        this.commitments.push(commitmentForm);
      }
    } else {
      var buyBeginDate = moment(this.fromDate).startOf("month");
      var buyEndDate = moment(this.toDate).startOf("month");

      var months = buyEndDate.diff(buyBeginDate, "month") + 1;
      for (var i = 0; i < months; i++) {
        var newDate = buyBeginDate.clone().add(i, "M");

        const commitmentForm = this.fb.group({
          year: [+newDate.format("yyyy")],
          yearMonth: [newDate.format("yyyy-MM")],
          month: [+newDate.format("MM")],
          amount: [0, Validators.required],
        });
        this.commitments.push(commitmentForm);
      }
    }
  }
  //#region Form & Model
  get commitments() {
    return this.formGroup.controls["commitments"] as FormArray;
  }

  amountError(form: FormGroup) {
    return (
      form.controls["amount"].invalid &&
      (form.controls["amount"].touched || form.controls["amount"].dirty)
    );
  }

  amountErrorValidation(form: FormGroup, validation) {
    return (
      form.controls["amount"].hasError(validation) &&
      (form.controls["amount"].touched || form.controls["amount"].dirty)
    );
  }

  cValue(form: FormGroup, name) {
    return form.controls[name].value;
  }

  close() {
    this.sidebarService.getSidebarRegistry(this.sidebarName).close();
  }

  updateData() {
    var salesCommitments = this.commitments.controls.map(
      (form: FormGroup) =>
        <TicketSalesCommitmentDto>{
          amount: form.controls["amount"].value,
          year: form.controls["year"].value,
          month: form.controls["month"].value,
        }
    );
    this.update.next(salesCommitments);
    this.close();
  }

  get updateVisible() {
    return !this.investment.id && !this.readOnly;
  }
  //#endregion

  //#region Languages
  get title() {
    return this.l("sales_commitment");
  }
  //#endregion
}
