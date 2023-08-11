import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import {
  PerfectScrollbarConfigInterface,
  PerfectScrollbarModule,
  PERFECT_SCROLLBAR_CONFIG,
} from "ngx-perfect-scrollbar";

import { CoreCommonModule } from "@core/common.module";
import { CoreTouchspinModule } from "@core/components/core-touchspin/core-touchspin.module";

import { NavbarComponent } from "app/layout/components/navbar/navbar.component";
import { NavbarBookmarkComponent } from "app/layout/components/navbar/navbar-bookmark/navbar-bookmark.component";
import { NavbarSearchComponent } from "app/layout/components/navbar/navbar-search/navbar-search.component";
import { NavbarCartComponent } from "app/layout/components/navbar/navbar-cart/navbar-cart.component";
import { NavbarNotificationComponent } from "app/layout/components/navbar/navbar-notification/navbar-notification.component";
import { DialogModule } from "primeng/dialog";
import { DialogService } from "primeng/dynamicdialog";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BlockUIModule } from "ng-block-ui";
import { SystemModule } from "@app/main/system/system.module";
import { ChangePasswordDialogComponent } from "@app/main/system/users/change-password-dialog/change-password-dialog.component";
import { CommonModule } from "@angular/common";
import { FeatherModule } from "@app/main/ui/icons/feather/feather.module";

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
  wheelPropagation: false,
};

@NgModule({
  declarations: [
    NavbarComponent,
    NavbarSearchComponent,
    NavbarBookmarkComponent,
    NavbarCartComponent,
    NavbarNotificationComponent
  ],
  imports: [
    DialogModule,
    RouterModule,
    CommonModule,
    NgbModule,
    CoreCommonModule,
    PerfectScrollbarModule,
    CoreTouchspinModule,
    BlockUIModule,
    FormsModule,
    ReactiveFormsModule,
    FeatherModule,
    SystemModule
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
    DialogService,
  ],
  exports: [NavbarComponent],
})
export class NavbarModule {}
