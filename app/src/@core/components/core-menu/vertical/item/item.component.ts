import { Component, Injector, Input } from '@angular/core';

import { CoreMenuItem } from '@core/types';
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: '[core-menu-vertical-item]',
  templateUrl: './item.component.html'
})
export class CoreMenuVerticalItemComponent extends AppComponentBase {
  @Input()
  item: CoreMenuItem;

  constructor(injector: Injector) {
    super(injector);
  }
}
