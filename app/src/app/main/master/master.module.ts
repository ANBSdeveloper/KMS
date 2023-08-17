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
import { BrandListComponent } from "./brand/brand-list/brand-list.component";
import { BrandSidebarComponent } from "./brand/brand-sidebar/brand-sidebar.component";
import { ProvinceListComponent } from "./province/province-list/province-list.component";
import { ProvinceSidebarComponent } from "./province/province-sidebar/province-sidebar.component";
import { SharedModule } from "@shared/shared.module";
import { DistrictListComponent } from "./district/district-list/district-list.component";
import { DistrictSidebarComponent } from "./district/district-sidebar/district-sidebar.component";
import { ProvinceComboComponent } from "./province/province-combo/province-combo.component";
import { ProductClassListComponent } from "./product-class/product-class-list/product-class-list.component";
import { ProductClassSidebarComponent } from "./product-class/product-class-sidebar/product-class-sidebar.component";
import { UserEditService } from "../apps/user/user-edit/user-edit.service";
import { UserViewService } from "../apps/user/user-view/user-view.service";
import { UserListService } from "../apps/user/user-list/user-list.service";
import { BrandComboComponent } from "./brand/brand-combo/brand-combo.component";
import { ProductClassComboComponent } from "./product-class/product-class-combo/product-class-combo.component";
import { QuillModule } from "ngx-quill";
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { NgxMaskModule } from "ngx-mask";
import { DistrictComboComponent } from "./district/district-combo/district-combo.component";
import { DxNumberBoxModule } from "devextreme-angular/ui/number-box";
import { ProductListComponent } from "./product/product-list/product-list.component";
import { ProductSidebarComponent } from "./product/product-sidebar/product-sidebar.component";
import { CycleComboComponent } from "./cycle/cycle-combo/cycle-combo.component";
import { DxTextBoxModule } from "devextreme-angular/ui/text-box";
import { RewardPackageListComponent } from "./reward-package/reward-package-list/reward-package-list.component";
import { RewardPackageEditComponent } from "./reward-package/reward-package-edit/reward-package-edit.component";
import { ProductComboComponent } from "./product/product-combo/product-combo.component";
import { RewardTypeComboComponent } from "./reward-package/reward-type-combo/reward-type-combo.component";
import { RewardPackageComboComponent } from "./reward-package/reward-package-combo/reward-package-combo.component";
import { RewardItemSidebarComponent } from "./reward-package/reward-item-sidebar/reward-item-sidebar.component";
import { ProductUnitComboComponent } from "./product-unit/product-unit-combo/product-unit-combo.component";
import { RewardTypeDataSource } from "./reward-package/data-source/reward-type.data-source";
import { MaterialListComponent } from "./material/material-list/material-list.component";
import { MaterialSidebarComponent } from "./material/material-sidebar/material-sidebar.component";
import { DxDataGridModule } from "devextreme-angular/ui/data-grid";
import { KeyShopApprovalListComponent } from "./key-shop-approval/key-shop-approval-list/key-shop-approval-list.component";
import { StatusTypeComboComponent } from "./key-shop-approval/status-type-combo/status-type-combo.component";
import { StatusTypeDataSource } from "./data-source/status-type.data-source";
import { DxDateBoxModule } from "devextreme-angular/ui/date-box";
import { StatusTypeNotificationDataSource } from "./data-source/status-type-notification.data-source";
import { StatusTypeNotificationComboComponent } from "./notification/status-type-notification-combo/status-type-notification-combo.component";
import { DxTextAreaModule } from "devextreme-angular/ui/text-area";
import { CycleListComponent } from "./cycle/cycle-list/cycle-list.component";
import { ZoneComboComponent } from "./zone/zone-combo/zone-combo.component";
import { AreaComboComponent } from "./area/area-combo/area-combo.component";
import { CycleSidebarComponent } from "./cycle/cycle-sidebar/cycle-sidebar.component";
import { NotificationListComponent } from "./notification/notification-list/notification-list.component";
import { NotificationEditComponent } from "./notification/notification-edit/notification-edit.component";
import { CustomerListComponent } from "./customer/customer-list/customer-list.component";
import { RSMStaffComboComponent } from "./staff/rsm-staff-combo/rsm-staff-combo.component";
import { ASMStaffComboComponent } from "./staff/asm-staff-combo/asm-staff-combo.component";
import { SSStaffComboComponent } from "./staff/ss-staff-combo/ss-staff-combo.component";
import { CustomerEditComponent } from "./customer/customer-edit/customer-edit.component";
import { BranchComboComponent } from "./branch/branch-combo/branch-combo.component";
import { WardComboComponent } from "./ward/ward-combo/ward-combo.component";
import { BranchListComponent } from "./branch/branch-list/branch-list.component";
import { ShopComboComponent } from "./customer/shop-combo/shop-combo.component";
import { DynamicDialogModule } from "primeng/dynamicdialog";
import { MaterialTypeComboComponent } from "./material-type/material-type-combo/material-type-combo.component";
import { RewardItemDialogComponent } from "./reward-package/reward-item-dialog/reward-item-dialog.component";
import { MaterialComboComponent } from "./material/material-combo/material-combo.component";
import { ProductPointListComponent } from "./product-point/product-point-list/product-point-list.component";
import { ProductPointSidebarComponent } from "./product-point/product-point-sidebar/product-point-sidebar.component";
import { ObjectTypeNotificationDataSource } from "./data-source/object-type-notification.data-source";
import { ObjectTypeNotificationComboComponent } from "./notification/object-type-notification-combo/object-type-notification-combo.component";
import { ZoneListComponent } from "./zone/zone-list/zone-list.component";
import { AreaListComponent } from "./area/area-list/area-list.component";
import { WardListComponent } from "./ward/ward-list/ward-list.component";
import { ProductEditComponent } from "./product/product-edit/product-edit.component";
import { SubProductClassListComponent } from "./sub-product-class/sub-product-class-list/sub-product-class-list.component";
import { SubProductClassSidebarComponent } from "./sub-product-class/sub-product-class-sidebar/sub-product-class-sidebar.component";
import { FilterBranchByZoneAreaComponent } from "./filter/filter-branch-by-zone-area/filter-branch-by-zone-area.component";
import { StatusActiveDataSource } from "./reward-package/data-source/status-active.data-source";
import { StatusActiveComboComponent } from "./status-active-combo/status-active-combo.component";
import { PosmClassListComponent } from "./posm-class/posm-class-list/posm-class-list.component";
import { PosmClassComboComponent } from "./posm-class/posm-class-combo/posm-class-combo.component";
import { PosmClassSidebarComponent } from "./posm-class/posm-class-sidebar/posm-class-sidebar.component";
import { VendorListComponent } from "./vendor/vendor-list/vendor-list.component";
import { VendorComboComponent } from "./vendor/vendor-combo/vendor-combo.component";
import { VendorSidebarComponent } from "./vendor/vendor-sidebar/vendor-sidebar.component";
import { PosmItemListComponent } from "./posm-item/posm-item-list/posm-item-list.component";
import { PosmUnitTypeDataSource } from "./posm-item/data-source/posm-unit-type.data-source";
import { PosmCalcTypeDataSource } from "./posm-item/data-source/posm-calc-type.data-source";
import { PosmUnitTypeComboComponent } from "./posm-item/posm-unit-type-combo/posm-unit-type-combo.component";
import { PosmCalcTypeComboComponent } from "./posm-item/posm-calc-type-combo/posm-calc-type-combo.component";
import { PosmItemEditComponent } from "./posm-item/posm-item-edit/posm-item-edit.component";
import { PosmCatalogSidebarComponent } from "./posm-item/posm-catalog-sidebar/posm-catalog-sidebar.component";
import { PosmPriceListComponent } from "./posm-price/posm-price-list/posm-price-list.component";
import { PosmPriceEditComponent } from "./posm-price/posm-price-edit/posm-price-edit.component";
import { PosmPriceDetailSidebarComponent } from "./posm-price/posm-price-detail-sidebar/posm-price-detail-sidebar.component";
import { PosmItemComboComponent } from "./posm-item/posm-item-combo/posm-item-combo.component";
import { CustomerLocationListComponent } from "./customer-location/customer-location-list/customer-location-list.component";
import { CustomerLocationComboComponent } from "./customer-location/customer-location-combo/customer-location-combo.component";
import { CustomerLocationSidebarComponent } from "./customer-location/customer-location-sidebar/customer-location-sidebar.component";
import { PosmCatalogComboComponent } from "./posm-item/posm-catalog-combo/posm-catalog-combo.component";
import { PosmRequestTypeComboComponent } from "./posm-item/posm-request-type-combo/posm-request-type-combo.component";
import { PosmRequestTypeDataSource } from "./posm-item/data-source/posm-request-type.data-source";
import { PosmInvestmentItemStatusDataSource } from "../investment/data-source/posm-investment-item-status.data-source";
import { CustomerDialogComponent } from "./customer/customer-dialog/customer-dialog.component";
import { PosmInvestmentStatusDataSource } from "../investment/data-source/posm-investment-status.data-source";
import { CoreCardModule } from "@core/components/core-card/core-card.module";
import { CustomerSalesMonthlyDialogComponent } from "./customer/customer-sales-monthly-dialog/customer-sales-monthly-dialog.component";
import { DxPivotGridModule } from "devextreme-angular";
import { ReportService } from "../report/report.service";
import { CustomerAcceptanceImageDialogComponent } from "./customer/customer-acceptance-image-dialog/customer-acceptance-image-dialog.component";
import { PosmTypeListComponent } from "./posm-type/posm-type-list/posm-type-list.component";
import { PosmTypeComboComponent } from "./posm-type/posm-type-combo/posm-type-combo.component";
import { PosmTypeSidebarComponent } from "./posm-type/posm-type-sidebar/posm-type-sidebar.component";
// routing
const routes: Routes = [
  {
    path: "brand-list",
    component: BrandListComponent,
  },
  {
    path: "product-class-list",
    component: ProductClassListComponent,
  },
  {
    path: "reward-package-list",
    component: RewardPackageListComponent,
  },
  {
    path: "reward-package/:id",
    component: RewardPackageEditComponent,
  },
  {
    path: "new-reward-package",
    component: RewardPackageEditComponent,
  },
  {
    path: "province-list",
    component: ProvinceListComponent,
  },
  {
    path: "district-list",
    component: DistrictListComponent,
  },
  {
    path: "product-list",
    component: ProductListComponent,
  },
  {
    path: "product/:id",
    component: ProductEditComponent,
  },
  {
    path: "material-list",
    component: MaterialListComponent,
  },
  {
    path: "key-shop-approval-list",
    component: KeyShopApprovalListComponent,
  },
  {
    path: "notification-list",
    component: NotificationListComponent,
  },
  {
    path: "notification/:id",
    component: NotificationEditComponent,
  },
  {
    path: "new-notification",
    component: NotificationEditComponent,
  },
  {
    path: "cycle-list",
    component: CycleListComponent,
  },
  {
    path: "customer-list",
    component: CustomerListComponent,
  },
  {
    path: "customer/:id",
    component: CustomerEditComponent,
  },
  {
    path: "new-customer",
    component: CustomerEditComponent,
  },
  {
    path: "branch-list",
    component: BranchListComponent,
  },
  {
    path: "product-point-list",
    component: ProductPointListComponent,
  },
  {
    path: "sub-product-class-list",
    component: SubProductClassListComponent,
  },
  {
    path: "zone-list",
    component: ZoneListComponent,
  },
  {
    path: "area-list",
    component: AreaListComponent,
  },
  {
    path: "ward-list",
    component: WardListComponent,
  },
  {
    path: "posm-class-list",
    component: PosmClassListComponent,
  },
  {
    path: "posm-type-list",
    component: PosmTypeListComponent,
  },
  {
    path: "vendor-list",
    component: VendorListComponent,
  },
  {
    path: "customer-location-list",
    component: CustomerLocationListComponent,
  },
  {
    path: "posm-item-list",
    component: PosmItemListComponent,
  },
  {
    path: "posm-item/:id",
    component: PosmItemEditComponent,
  },
  {
    path: "new-posm-item",
    component: PosmItemEditComponent,
  },
  {
    path: "posm-price-list",
    component: PosmPriceListComponent,
  },
  {
    path: "posm-price/:id",
    component: PosmPriceEditComponent,
  },
  {
    path: "new-posm-price",
    component: PosmPriceEditComponent,
  },
  {
    path: "",
    redirectTo: "/brand-list",
    pathMatch: "full",
  },
];

@NgModule({
  declarations: [
    BrandListComponent,
    BrandSidebarComponent,
    BrandComboComponent,
    ProductClassListComponent,
    ProductClassSidebarComponent,
    ProductClassComboComponent,
    ProvinceListComponent,
    ProvinceSidebarComponent,
    ProvinceComboComponent,
    DistrictListComponent,
    DistrictSidebarComponent,
    DistrictComboComponent,
    ProductListComponent,
    ProductSidebarComponent,
    CycleComboComponent,
    RewardPackageListComponent,
    RewardPackageEditComponent,
    ProductComboComponent,
    RewardTypeComboComponent,
    RewardItemSidebarComponent,
    ProductUnitComboComponent,
    MaterialListComponent,
    MaterialSidebarComponent,
    KeyShopApprovalListComponent,
    StatusTypeComboComponent,
    ZoneComboComponent,
    AreaComboComponent,
    CycleSidebarComponent,
    StatusTypeNotificationComboComponent,
    NotificationListComponent,
    CycleListComponent,
    NotificationEditComponent,
    CustomerListComponent,
    RSMStaffComboComponent,
    ASMStaffComboComponent,
    SSStaffComboComponent,
    CustomerEditComponent,
    BranchComboComponent,
    WardComboComponent,
    ShopComboComponent,
    RewardPackageComboComponent,
    BranchListComponent,
    RewardItemDialogComponent,
    MaterialComboComponent,
    MaterialTypeComboComponent,
    ProductPointListComponent,
    ProductPointSidebarComponent,
    ObjectTypeNotificationComboComponent,
    PosmItemComboComponent,
    ZoneListComponent,
    AreaListComponent,
    WardListComponent,
    PosmClassListComponent,
    PosmClassComboComponent,
    PosmClassSidebarComponent,
    PosmTypeListComponent,
    PosmTypeComboComponent,
    PosmTypeSidebarComponent,
    PosmItemListComponent,
    PosmItemEditComponent,
    PosmItemComboComponent,
    PosmCatalogSidebarComponent,
    PosmPriceListComponent,
    PosmPriceEditComponent,
    PosmPriceDetailSidebarComponent,
    VendorListComponent,
    VendorComboComponent,
    VendorSidebarComponent,
    ProductEditComponent,
    SubProductClassListComponent,
    SubProductClassSidebarComponent,
    FilterBranchByZoneAreaComponent,
    StatusActiveComboComponent,
    PosmUnitTypeComboComponent,
    PosmCalcTypeComboComponent,
    CustomerLocationListComponent,
    CustomerLocationComboComponent,
    CustomerLocationSidebarComponent,
    CustomerSalesMonthlyDialogComponent,
    PosmCatalogComboComponent,
    PosmRequestTypeComboComponent,
    CustomerDialogComponent,
    CustomerAcceptanceImageDialogComponent
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
    DxTextBoxModule,
    DxNumberBoxModule,
    DxDataGridModule,
    DxDateBoxModule,
    DxTextAreaModule,
    DynamicDialogModule,
    DxPivotGridModule,
    CoreCardModule,
    SharedModule
  ],
  exports: [
    CycleComboComponent,
    ShopComboComponent,
    RewardPackageComboComponent,
    MaterialComboComponent,
    FilterBranchByZoneAreaComponent,
    ZoneComboComponent,
    AreaComboComponent,
    StatusActiveComboComponent,
    PosmClassComboComponent,
    VendorComboComponent,
    CustomerLocationComboComponent,
    BrandComboComponent,
    PosmCatalogComboComponent,
    PosmItemComboComponent,
    PosmUnitTypeComboComponent,
    PosmRequestTypeComboComponent,
    CustomerDialogComponent,
    PosmTypeComboComponent,
  ],
  providers: [
    DataServiceProxy,
    UserListService,
    UserViewService,
    UserEditService,
    RewardTypeDataSource,
    StatusTypeDataSource,
    StatusTypeNotificationDataSource,
    ObjectTypeNotificationDataSource,
    StatusActiveDataSource,
    PosmUnitTypeDataSource,
    PosmCalcTypeDataSource,
    PosmRequestTypeDataSource,
    PosmInvestmentItemStatusDataSource,
    PosmInvestmentStatusDataSource,
    ReportService
  ],
})
export class MasterModule {}
