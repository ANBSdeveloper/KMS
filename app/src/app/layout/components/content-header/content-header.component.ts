import { Component, OnInit, Input, Injector } from "@angular/core";
import { contentHeader } from "@app/menu/content-header";
import { AppComponentBase } from "@cbms/ng-core-vuexy";

// ContentHeader component interface
export interface ContentHeader {
  headerTitle: string;
  actionButton: boolean;
  breadcrumb?: {
    type?: string;
    links?: Array<{
      name?: string;
      isLink?: boolean;
      link?: string;
    }>;
  };
}

@Component({
  selector: "app-content-header",
  templateUrl: "./content-header.component.html",
})
export class ContentHeaderComponent extends AppComponentBase implements OnInit {
  // input variable
  @Input() contentHeader: ContentHeader;
  @Input() contentHeaderName: string;
  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    if (!this.contentHeader && this.contentHeaderName) {
      this.contentHeader = contentHeader[this.contentHeaderName];
    }
  }
}
