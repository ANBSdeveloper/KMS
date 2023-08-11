import { Component, Injector, Input } from '@angular/core';

import { CoreMenuItem } from '@core/types';
import { AppComponentBase } from "@cbms/ng-core-vuexy";

@Component({
  selector: '[core-menu-vertical-section]',
  templateUrl: './section.component.html'
})
export class CoreMenuVerticalSectionComponent extends AppComponentBase {
  @Input()
  item: CoreMenuItem;

  constructor(injector: Injector) {
    super(injector);
  }
}
