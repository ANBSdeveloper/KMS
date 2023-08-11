import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";

import { LocalizationService } from "@cbms/ng-core-vuexy";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  /**
   * @param {Router} router
   * @param {AuthenticationService} authenticationService
   */
  constructor(
    private router: Router,
    private localizationService: LocalizationService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.localizationService.getCurrentLanguage()) {
      request = request.clone({
        setHeaders: {
          ".AspNetCore.Culture": this.localizationService.getCurrentLanguage(),
        },
      });
    }

    return next.handle(request).pipe(
      catchError((err) => {
        if (!err.status) {
          return throwError(this.localizationService.get("request_error"));
        }
        if ([401, 403].indexOf(err.status) !== -1) {
          // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
          this.router.navigate(["/pages/miscellaneous/not-authorized"]);

          // ? Can also logout and reload if needed
          // this._authenticationService.logout();
          // location.reload(true);
        }
        if (err.error instanceof Blob) {
          return this.blobToText(err.error).pipe(
            switchMap((result) => throwError(JSON.parse(result).error.message))
          );
        }
        // throwError
        if (err.error.error) {
          return throwError(err.error.error.message);
        }
        return throwError(err.error);
      })
    );
  }

  blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
      if (!blob) {
        observer.next("");
        observer.complete();
      } else {
        let reader = new FileReader();
        reader.onload = (event) => {
          observer.next((<any>event.target).result);
          observer.complete();
        };
        reader.readAsText(blob);
      }
    });
  }
}
