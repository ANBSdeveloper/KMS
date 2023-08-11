import { Component, OnInit, Input, Injector } from '@angular/core';
import { AppComponentBase } from '@cbms/ng-core-vuexy';

// Breadcrumb component interface
export interface Breadcrumb {
  type?: string;
  alignment?: string;
  links?: Array<{
    name: string;
    isLink: boolean;
    link?: string;
  }>;
}

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html'
})
export class BreadcrumbComponent extends AppComponentBase implements OnInit {
  // input variable
  @Input() breadcrumb: Breadcrumb;

  constructor(injector: Injector) {
    super(injector);
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit() {
    // concatenate default properties with passed properties
    this.breadcrumb = this.breadcrumb;
  }
}
