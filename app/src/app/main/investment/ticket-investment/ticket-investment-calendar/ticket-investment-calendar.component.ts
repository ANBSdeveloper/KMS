import {
  Component,
  ViewEncapsulation,
  Injector,
  ViewChild,
} from "@angular/core";

import { CalendarOptions, FullCalendarComponent } from "@fullcalendar/angular";

import { DxListComponentBase } from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  StaffDto,
  TicketInvestmentListItemDto,
} from "@shared/services/data.service";
import { EventRef } from "./calendar.model";
import { TicketInvestmentRSMStaffComboComponent } from "../ticket-investment-rsm-staff-combo/ticket-investment-rsm-staff-combo.component";
import { TicketInvestmentASMStaffComboComponent } from "../ticket-investment-asm-staff-combo/ticket-investment-asm-staff-combo.component";
import { AuthenticationService } from "@app/auth/service/authentication.service";
import { RoleType } from "@app/main/system/role/role-type.enum";
import moment from "moment";
import { TicketInvestmentStatus } from "../../data-source/ticket-investmen-status.enum";
import { finalize } from "rxjs/operators";

@Component({
  selector: "app-ticket-investment-calendar",
  templateUrl: "./ticket-investment-calendar.component.html",
  styleUrls: ["./ticket-investment-calendar.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class TicketInvestmentCalendar extends DxListComponentBase<TicketInvestmentListItemDto> {
  // Public
  public slideoutShow = false;
  public events = [];
  public event;
  @ViewChild("rsmStaffCombo")
  rsmStaffCombo: TicketInvestmentRSMStaffComboComponent;
  @ViewChild("asmStaffCombo")
  asmStaffCombo: TicketInvestmentASMStaffComboComponent;
  startDate = moment().startOf('month').toDate();
  endDate = moment().endOf('month').toDate();
  arrayStatus = [
    TicketInvestmentStatus.Approved,
    TicketInvestmentStatus.Denied,
    TicketInvestmentStatus.Updating,
    TicketInvestmentStatus.Operated,
    TicketInvestmentStatus.Acceptance,
    TicketInvestmentStatus.FinalSettlement,
  ];
  sidebarName = "ticket_investment_calendar_sidebar";
  staff: StaffDto;
  data = [];
  total = 0;
  @ViewChild("calendar") calendarComponent: FullCalendarComponent;

  public calendarOptions: CalendarOptions = {
    headerToolbar: {
      start: "sidebarToggle, prev,next, title",
      end: "",
    },
    initialView: "dayGridMonth",
    initialEvents: this.events,
    weekends: true,
    editable: true,
    eventResizableFromStart: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: 2,
    navLinks: true,
    eventClick: this.handleUpdateEventClick.bind(this),
    eventClassNames: this.eventClass.bind(this),
    select: this.handleDateSelect.bind(this),
  };

  constructor(injector: Injector, private authService: AuthenticationService) {
    super(injector);
  }

  eventClass(s) {
    const calendarsColor = {
      Business: "primary",
      Holiday: "success",
      Personal: "danger",
      Family: "warning",
      ETC: "info",
    };

    const colorName = calendarsColor[s.event._def.extendedProps.location];
    return `bg-light-${colorName}`;
  }

  handleUpdateEventClick(eventRef) {
    this.dataLabel(eventRef.event.start, eventRef.event.start);
  }

  toggleSidebar(name): void {}

  handleDateSelect(eventRef) {
    this.dataLabel(eventRef.start, eventRef.start);
  }

  dataLabel(startDate: Date, endDate: Date) {
    var rsmStaff = this.c("rsmStaffId").value ? this.c("rsmStaffId").value : "";
    var asmStaff = this.c("asmStaffId").value ? this.c("asmStaffId").value : "";
    var a = this.getDataService<DataServiceProxy>()
      .getOperationPlanInvestmentList(
        undefined,
        rsmStaff,
        asmStaff,
        undefined,
        startDate,
        endDate,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .subscribe((res) => {
        this.data = res.result.items;
        this.total = res.result.totalCount;
      });
  }

  ngOnInit(): void {
    this.filterFormGroup = this.fb.group({
      rsmStaffId: [undefined],
      asmStaffId: [undefined],
    });

    this.getDataService<DataServiceProxy>()
      .getStaffInfo()
      .subscribe((response) => {
        this.staff = response.result;

        if (this.staff != undefined) {
          if (this.isRsm) {
            this.c("rsmStaffId").setValue(this.staff.id);
            this.asmStaffCombo.value = undefined;
            setTimeout(() => {
              this.asmStaffCombo.loadData();
            }, 50);
          } else if (this.isAsm) {
            this.c("asmStaffId").setValue(this.staff.id);
          }
        }

        this.getData();
      });
  }
  ngAfterViewInit() {
    let _this = this;
    this.calendarOptions.customButtons = {
      sidebarToggle: {
        text: "",
        click() {
          _this.toggleSidebar(this.sidebarName);
        },
      },
      prev: {
        text: "",
        click() {
          _this.viewChange(true);
        },
      },
      next: {
        text: "",
        click() {
          _this.viewChange(false);
        },
      },
    };
  }
  viewChange(isPrev: boolean) {
    this.data = [];
    this.total = 0;
    if (isPrev) {
      this.calendarComponent.getApi().prev();
    } else {
      this.calendarComponent.getApi().next();
    }

    var calendarView = this.calendarComponent.getApi();
    this.startDate = calendarView.view.currentStart;
    this.endDate = calendarView.view.currentEnd;
    this.getData();
  }
  rsmStaffChange(record) {
    var calendarView = this.calendarComponent.getApi();
    this.startDate = calendarView.view.currentStart;
    this.endDate = calendarView.view.currentEnd;

    this.asmStaffCombo.value = undefined;
    setTimeout(() => {
      this.asmStaffCombo.loadData();
      this.getData();
    }, 50);
  }
  asmStaffChange(record) {
    var calendarView = this.calendarComponent.getApi();
    this.startDate = calendarView.view.currentStart;
    this.endDate = calendarView.view.currentEnd;

    this.getData();
  }
  get isRsm() {
    return (
      this.authService.currentUser.roles.find((p) => p == RoleType.Rsm) !=
      undefined
    );
  }

  get isAsm() {
    return (
      this.authService.currentUser.roles.find((p) => p == RoleType.Asm) !=
      undefined
    );
  }

  get isSS() {
    return (
      this.authService.currentUser.roles.find(
        (p) => p == RoleType.SalesSupervisor
      ) != undefined
    );
  }
  getData() {
    this.loading = true;
    var rsmStaff = this.c("rsmStaffId").value ? this.c("rsmStaffId").value : "";
    var asmStaff = this.c("asmStaffId").value ? this.c("asmStaffId").value : "";

    var arrayColor = [];
    var arrayObject = [];
    this.getDataService<DataServiceProxy>()
      .getTicketInvestmentsByTime(
        this.arrayStatus,
        rsmStaff,
        asmStaff,
        undefined,
        this.startDate,
        this.endDate,
        true,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res) => {
        res.result.items.forEach(function (item) {
          var count = 0;
          res.result.items.forEach(function (item1) {
            if (
              item.operationDate.toDateString() ==
              item1.operationDate.toDateString()
            ) {
              count++;
            }
          });
          if (count > 1) {
            const newEvent = new EventRef();
            newEvent.allDay = true;
            newEvent.id = item.id;
            newEvent.url = "";
            newEvent.title = item.code;
            newEvent.start = item.operationDate;
            newEvent.end = item.operationDate;
            newEvent.calendar = "";
            newEvent.extendedProps.location = "Personal";
            newEvent.extendedProps.description = undefined;
            newEvent.extendedProps.addGuest = undefined;
            arrayObject.push(newEvent);
          } else {
            const newEvent = new EventRef();
            newEvent.allDay = true;
            newEvent.id = item.id;
            newEvent.url = "";
            newEvent.title = item.code;
            newEvent.start = item.operationDate;
            newEvent.end = item.operationDate;
            newEvent.calendar = "";
            newEvent.extendedProps.location = "Business";
            newEvent.extendedProps.description = undefined;
            newEvent.extendedProps.addGuest = undefined;
            arrayObject.push(newEvent);
          }
        });

        this.events = <any>arrayObject;
        this.calendarOptions.events = <any>arrayObject;
      });
    this.event = arrayObject;
  }
}
