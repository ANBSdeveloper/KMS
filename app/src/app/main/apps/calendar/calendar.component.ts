import { Title } from '@angular/platform-browser';
import { Component, OnInit, AfterViewInit, ViewEncapsulation } from '@angular/core';

import { CalendarOptions, EventClickArg } from '@fullcalendar/angular';

import { CoreSidebarService } from '@core/components/core-sidebar/core-sidebar.service';

import { CalendarService } from 'app/main/apps/calendar/calendar.service';
import { EventRef } from 'app/main/apps/calendar/calendar.model';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CalendarComponent implements OnInit, AfterViewInit {
  // Public
  public slideoutShow = false;
  public events = [];
  public event;

  public calendarOptions: CalendarOptions = {
    headerToolbar: {
      start: 'sidebarToggle, prev,next, title',
      end: ''
    },
    initialView: 'dayGridMonth',
    initialEvents: this.events,
    weekends: true,
    editable: true,
    eventResizableFromStart: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: 2,
    navLinks: false,
    eventClick: this.handleUpdateEventClick.bind(this),
    eventClassNames: this.eventClass.bind(this),
    select: this.handleDateSelect.bind(this),
    dateClick: this.handleDateClick.bind(this),
  };

  /**
   * Constructor
   *
   * @param {CoreSidebarService} _coreSidebarService
   * @param {CalendarService} _calendarService
   */
  constructor(private _coreSidebarService: CoreSidebarService, private _calendarService: CalendarService) {}

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Add Event Class
   *
   * @param s
   */
  eventClass(s) {
    const calendarsColor = {
      Business: 'primary',
      Holiday: 'success',
      Personal: 'danger',
      Family: 'warning',
      ETC: 'info'
    };

    const colorName = calendarsColor[s.event._def.extendedProps.calendar];
    return `bg-light-${colorName}`;
  }

  /**
   * Update Event
   *
   * @param eventRef
   */
  handleUpdateEventClick(eventRef: EventClickArg) {
    this._coreSidebarService.getSidebarRegistry('calendar-event-sidebar').toggleOpen();
    this._calendarService.updateCurrentEvent(eventRef);
  }

  /**
   * Toggle the sidebar
   *
   * @param name
   */
  toggleSidebar(name): void {
    this._coreSidebarService.getSidebarRegistry(name).toggleOpen();
  }

  /**
   * Date select Event
   *
   * @param eventRef
   */
  handleDateSelect(eventRef) {
    const newEvent = new EventRef();
    newEvent.start = eventRef.start;
    this._coreSidebarService.getSidebarRegistry('calendar-event-sidebar').toggleOpen();
    this._calendarService.onCurrentEventChange.next(newEvent);
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // Subscribe to Event Change
    this._calendarService.onEventChange.subscribe(res => {
      this.events = res;
      this.calendarOptions.events = res;
    });

    this._calendarService.onCurrentEventChange.subscribe(res => {
      this.event = res;
    });
  }

  /**
   * Calendar's custom button on click toggle sidebar
   */
  ngAfterViewInit() {
    // Store this to _this as we need it on click event to call toggleSidebar
    let _this = this;
    this.calendarOptions.customButtons = {
      prev: {
        text: '',
        click() {
          console.log(this.c("calendarID"));
        }
      }
    };
  }
  handleDateClick(arg) {
    alert('date click! ' + arg.dateStr)
  }
}
