// This api will come in the next version

import { PlatformLocation } from "@angular/common";
import { AuthConfig } from "angular-oauth2-oidc";
import { environment } from "environments/environment";

export const authPasswordFlowConfig = (location: PlatformLocation) =>
  <AuthConfig>{
    // Url of the Identity Provider
    issuer: environment.authUrl,

    // URL of the SPA to redirect the user to after login
    redirectUri:
      window.location.origin + location.getBaseHrefFromDOM() + "/index.html",

    // URL of the SPA to redirect the user after silent refresh
    silentRefreshRedirectUri:
      window.location.origin +
      location.getBaseHrefFromDOM() +
      "/silent-refresh.html",

    // The SPA's id. The SPA is registerd with this id at the auth-server
    clientId: "js",

    dummyClientSecret: "cbms",

    // set the scope for the permissions the client should request
    // The first three are defined by OIDC. The 4th is a usecase-specific one
    scope: "api openid offline_access",
    requireHttps: environment.authHttps,
    showDebugInformation: true,

    oidc: false,
  };
