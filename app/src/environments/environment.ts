// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  appCode: "kms",
  production: false,
  hmr: false,
  // apiUrl: "http://kms.vitadairy.vn/service",
  // authUrl: "http://kms.vitadairy.vn/service",
  // fakeApiUrl: "http://kms.vitadairy.vn",
  apiUrl: "https://localhost:5003",
  authUrl: "https://localhost:5003",
  fakeApiUrl: "http://localhost:4202",
  authHttps: false,
  appConfig: "app-config.json",
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
