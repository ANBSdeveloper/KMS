import { PlatformLocation } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { OAuthService } from "angular-oauth2-oidc";
import { environment } from "environments/environment";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { AuthenticationService } from "./auth/service";

@Injectable({
  providedIn: "root",
})
export class AppInitializer {
  constructor(
    private oauthService: OAuthService,
    private httpClient: HttpClient,
    private platformLocation: PlatformLocation,
    private localizationService: LocalizationService,
    private authService: AuthenticationService
  ) {}
  /**
   * Initialize config, localization, user config
   * @returns
   */
  init(): () => Promise<boolean> {
    return () =>
      new Promise<boolean>((resolve, reject) => {
        //Todo: Handle can't connect to identity server
        this.oauthService
          .loadDiscoveryDocument()
          .then(() => {
            const baseHref = this.getBaseHref();
            return this.getAppConfiguration(baseHref)
              .then(() =>
                this.getLocalization(baseHref).then(() =>
                  this.loadUserProfile()
                )
              )
              .then(() => resolve(true));
          })
          .catch((error) => reject(error));
      });
  }

  private loadUserProfile(): Promise<any> {
    if (this.oauthService.hasValidAccessToken()) {
      return this.authService.loadUserProfile();
    }
    return Promise.resolve();
  }

  private getLocalization(appRootUrl: string): Promise<any> {
    return this.httpClient
      .get<any>(
        `${appRootUrl}assets/localization/${this.localizationService.getCurrentLanguage()}.json`
      )
      .toPromise()
      .then((response) => {
        this.localizationService.setData(response);
        console.log(this.localizationService.get("welcome", "trucvt"));
      });
  }

  private getAppConfiguration(appRootUrl: string): Promise<any> {
    return this.httpClient
      .get<any>(`${appRootUrl}assets/${environment.appConfig}`)
      .toPromise()
      .then((response) => {
        this.localizationService.initialize(
          response.defaultLanguage,
          response.supportLangauges
        );
      });
  }

  private getBaseHref(): string {
    const baseUrl = this.platformLocation.getBaseHrefFromDOM();
    if (baseUrl) {
      return baseUrl;
    }
    return "/";
  }
}
