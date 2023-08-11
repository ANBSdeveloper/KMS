import { APP_INITIALIZER, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { HttpClientInMemoryWebApiModule } from "angular-in-memory-web-api";
import { FakeDbService } from "@fake-db/fake-db.service";
import { OAuthModule, OAuthStorage, AuthConfig } from "angular-oauth2-oidc";
import "hammerjs";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { ToastrModule } from "ngx-toastr";
import { TranslateModule } from "@ngx-translate/core";
import { ContextMenuModule } from "@ctrl/ngx-rightclick";

import { CoreModule } from "@core/core.module";
import { CoreCommonModule } from "@core/common.module";
import { CoreSidebarModule, CoreThemeCustomizerModule } from "@core/components";
import { CardSnippetModule } from "@core/components/card-snippet/card-snippet.module";

import { coreConfig } from "app/app-config";
import { fakeBackendProvider } from "app/auth/helpers"; // used to create fake backend
import { ErrorInterceptor } from "app/auth/helpers";
import { AppComponent } from "app/app.component";
import { LayoutModule } from "app/layout/layout.module";
import { ContentHeaderModule } from "app/layout/components/content-header/content-header.module";
import { ContextMenuComponent } from "app/main/extensions/context-menu/context-menu.component";
import { AnimatedCustomContextMenuComponent } from "./main/extensions/context-menu/custom-context-menu/animated-custom-context-menu/animated-custom-context-menu.component";
import { BasicCustomContextMenuComponent } from "./main/extensions/context-menu/custom-context-menu/basic-custom-context-menu/basic-custom-context-menu.component";
import { SubMenuCustomContextMenuComponent } from "./main/extensions/context-menu/custom-context-menu/sub-menu-custom-context-menu/sub-menu-custom-context-menu.component";
import { authPasswordFlowConfig } from "./auth/auth-password-flow.config";
import { AppInitializer } from "./app-initializer";
import { API_BASE_URL, DataServiceProxy } from "@shared/services/data.service";
import { environment } from "environments/environment";
import { RootRoutingModule } from "./root-routing.module";
import { CoreSidebarService } from "@core/components/core-sidebar/core-sidebar.service";
import {
  AUTH_SERVICE,
  DATA_SERVICE,
  SIDEBAR,
  APP_CODE,
  CbmsModule,
} from "@cbms/ng-core-vuexy";
import { AuthenticationService } from "./auth/service";
import { SharedModule } from "@shared/shared.module";
import { PlatformLocation } from "@angular/common";

@NgModule({
  declarations: [
    AppComponent,
    ContextMenuComponent,
    BasicCustomContextMenuComponent,
    AnimatedCustomContextMenuComponent,
    SubMenuCustomContextMenuComponent,
  ],
  imports: [
    CbmsModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    HttpClientInMemoryWebApiModule.forRoot(FakeDbService, {
      delay: 0,
      passThruUnknownUrl: true,
    }),
    RootRoutingModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [environment.apiUrl],
        sendAccessToken: true,
      },
    }),
    NgbModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot(),
    ContextMenuModule,
    CoreModule.forRoot(coreConfig),
    CoreCommonModule,
    CoreSidebarModule,
    CoreThemeCustomizerModule,
    CardSnippetModule,
    LayoutModule,
    ContentHeaderModule,
    SharedModule,
  ],

  providers: [
    //{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    {
      provide: APP_CODE,
      useFactory: () => environment.appCode,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: (appInitializer: AppInitializer) => appInitializer.init(),
      deps: [AppInitializer],
      multi: true,
    },
    { provide: API_BASE_URL, useFactory: () => environment.apiUrl },
    {
      provide: SIDEBAR,
      useFactory: (service: CoreSidebarService) => service,
      deps: [CoreSidebarService],
    },
    {
      provide: DATA_SERVICE,
      useFactory: (service: DataServiceProxy) => service,
      deps: [DataServiceProxy],
    },
    {
      provide: AUTH_SERVICE,
      useFactory: (service: AuthenticationService) => service,
      deps: [AuthenticationService],
    },
    { provide: AuthConfig, useFactory: (location: PlatformLocation) => authPasswordFlowConfig(location), deps: [PlatformLocation] },
    { provide: OAuthStorage, useValue: localStorage },
    // provider used to create fake backend, comment while using real api
    //fakeBackendProvider,
  ],
  entryComponents: [
    BasicCustomContextMenuComponent,
    AnimatedCustomContextMenuComponent,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
