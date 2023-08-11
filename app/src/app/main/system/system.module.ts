import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
import { Ng2FlatpickrModule } from "ng2-flatpickr";

import { CoreCommonModule } from "@core/common.module";
import { CoreDirectivesModule } from "@core/directives/directives";
import { CorePipesModule } from "@core/pipes/pipes.module";
import { CoreSidebarModule } from "@core/components";

import { InvoiceModule } from "app/main/apps/invoice/invoice.module";

import { DataServiceProxy } from "@shared/services/data.service";
import { BlockUIModule } from "ng-block-ui";
import { ToastrModule } from "ngx-toastr";
import { ContentHeaderModule } from "@app/layout/components/content-header/content-header.module";
import { SharedModule } from "@shared/shared.module";
import { QuillModule } from "ngx-quill";
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { NgxMaskModule } from "ngx-mask";
import { UserListComponent } from "./users/user-list/user-list.component";
import { RoleComboComponent } from "./role/role-combo/role-combo.component";
import { UserEditComponent } from "./users/user-edit/user-edit.component";
import { TreeModule } from "@circlon/angular-tree-component";

import { RoleDataSource } from "./data-source/role.data-source";
import { DxDateBoxModule } from "devextreme-angular/ui/date-box";
import { DxTextBoxModule } from "devextreme-angular/ui/text-box";
import { DxButtonModule } from "devextreme-angular/ui/button";
import { DxDataGridModule } from "devextreme-angular/ui/data-grid";
import { RoleEditComponent } from "./role/role-edit/role-edit.component";
import { DxTextAreaModule } from "devextreme-angular/ui/text-area";
import { RoleListComponent } from "./role/role-list/role-list.component";
import { CustomerDevelopmentUserComboComponent } from "./users/customer-development-user-combo/customer-development-user-combo.component";
import { ChangePasswordDialogComponent } from "./users/change-password-dialog/change-password-dialog.component";
import { DialogModule } from "primeng/dialog";
import { DialogService } from "primeng/dynamicdialog";
import { CustomerDevelopmentLeadComboComponent } from "./users/customer-development-lead-combo/customer-development-lead-combo.component";
import { AppSettingListComponent } from "./app-setting/app-setting-list/app-setting-list.component";
import { AppSettingSidebarComponent } from "./app-setting/app-setting-sidebar/app-setting-sidebar.component";
import { MasterModule } from "../master/master.module";
import { UserProfileComponent } from "./users/user-profile/user-profile.component";

// routing
const routes: Routes = [
  {
    path: "user-profile",
    component: UserProfileComponent,
  },
  {
    path: "user-list",
    component: UserListComponent,
  },
  {
    path: "users/:id",
    component: UserEditComponent,
  },
  {
    path: "new-user",
    component: UserEditComponent,
  },
  {
    path: "role-list",
    component: RoleListComponent,
  },
  {
    path: "roles/:id",
    component: RoleEditComponent,
  },
  {
    path: "new-role",
    component: RoleEditComponent,
  },
  {
    path: "app-setting-list",
    component: AppSettingListComponent,
  },
  {
    path: "",
    redirectTo: "/user-list",
    pathMatch: "full",
  },
];

@NgModule({
  declarations: [
    UserListComponent,
    UserEditComponent,
    RoleComboComponent,
    RoleEditComponent,
    RoleListComponent,
    CustomerDevelopmentUserComboComponent,
    CustomerDevelopmentLeadComboComponent,
    ChangePasswordDialogComponent,
    AppSettingListComponent,
    AppSettingSidebarComponent,
    UserProfileComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    CoreCommonModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    ContentHeaderModule,
    Ng2FlatpickrModule,
    NgxDatatableModule,
    CorePipesModule,
    CoreDirectivesModule,
    BlockUIModule.forRoot(),
    ToastrModule,
    InvoiceModule,
    CoreSidebarModule,
    SharedModule,
    QuillModule.forRoot(),
    NgxMaskModule.forRoot(),
    PerfectScrollbarModule,
    DxDateBoxModule,
    DxTextBoxModule,
    TreeModule,
    DxButtonModule,
    DxDataGridModule,
    DxTextAreaModule,
    DialogModule,
    MasterModule
  ],
  exports: [
    RoleComboComponent,
    CustomerDevelopmentUserComboComponent,
    CustomerDevelopmentLeadComboComponent,
    ChangePasswordDialogComponent,
  ],
  providers: [DataServiceProxy, RoleDataSource, DialogService],
})
export class SystemModule {}
