import {
  Component,
  EventEmitter,
  Injector,
  Input,
  Output,
} from "@angular/core";
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: "app-pager",
  templateUrl: "./pager.component.html",
})
export class PagerComponent extends AppComponentBase {
  @Output() sizeChange: EventEmitter<number>;
  @Input() showLabel = true;
  size: number = 10;


  constructor(injector: Injector) {
    super(injector);
    this.sizeChange = new EventEmitter<number>();
  }

  setPageSize(e) {
    this.sizeChange.next(e.target.value);
  }
}
