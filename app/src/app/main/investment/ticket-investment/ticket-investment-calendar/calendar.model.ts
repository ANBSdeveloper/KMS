export class EventRef {
  id? = undefined;
  url: string;
  title: string = '';
  start: Date;
  end: Date;
  allDay = false;
  calendar: '';
  extendedProps = {
    location: '',
    description: '',
    addGuest: [],
  };
}
