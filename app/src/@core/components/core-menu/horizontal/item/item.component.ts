import { Component, Injector, Input } from "@angular/core";
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: "[core-menu-horizontal-item]",
  templateUrl: "./item.component.html",
})
export class CoreMenuHorizontalItemComponent extends AppComponentBase {
  @Input()
  item: any;

  constructor(injector: Injector) {
    super(injector);
  }
}
