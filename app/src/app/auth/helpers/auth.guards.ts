import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from 'app/auth/service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  /**
   *
   * @param {Router} _router
   * @param {AuthenticationService} _authenticationService
   */
  constructor(private _router: Router, private _authenticationService: AuthenticationService) {}

  // canActivate
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this._authenticationService.currentUser;

    if (currentUser) {
      // check if route is restricted by permission
      //Todo: check permission to route

      if (route.data.permissions && !currentUser.isGrantedAny(route.data.permissions)) {
        // user not authorised so redirect to not-authorized page
        var isGranted = currentUser.isGrantedAny(route.data.permissions);
        console.log(isGranted);
        this._router.navigate(['/pages/miscellaneous/not-authorized']);
        return false;
      }

      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    this._router.navigate(['/pages/authentication/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
