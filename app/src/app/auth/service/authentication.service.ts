import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

import { AuthUser } from "app/auth/models";
import { OAuthService } from "angular-oauth2-oidc";
import { LocalizationService } from "@cbms/ng-core-vuexy";
import { environment } from "environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: "root" })
export class AuthenticationService {
  //public
  public currentUser$: Observable<AuthUser>;

  //private
  private rememberUserNameKey = `${environment.appCode}.username`;
  private currentUserSubject = new BehaviorSubject<AuthUser>(null);

  /**
   *
   * @param {HttpClient} http
   * @param {ToastrService} toastrService
   */
  constructor(
    private oauthService: OAuthService,
    private httpClient: HttpClient,
    private localizationService: LocalizationService
  ) {
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  // getter: currentUserValue
  public get currentUser(): AuthUser {
    return this.currentUserSubject.value;
  }

  /**
   *  Confirms if user is admin
   */
  get isAdmin() {
    return this.currentUser$ && this.currentUserSubject.value.isAdmin;
  }

  /**
   *  Confirms if user is client
   */
  get isClient() {
    return false;
  }

  /**
   * User login
   *
   * @param email
   * @param password
   * @returns user
   */
  login(username: string, password: string, rememberMe: boolean) {
    return this.oauthService
      .fetchTokenUsingPasswordFlowAndLoadUserProfile(username, password)
      .then((userInfo) => {
        var user = AuthUser.mapFromClaims(userInfo);

        if (rememberMe) {
          this.saveRemeberUsername(username);
        } else {
          this.removeRemeberUsername();
        }

        this.currentUserSubject.next(user);
        return user;
      })
      .catch((error) => {
        throw new Error(
          this.localizationService.get("login_invalid_credentials")
        );
      });
  }

  /**
   * User logout
   *
   */
  logout() {
    this.oauthService.logOut();
    this.currentUserSubject.next(null);
  }

  /**
   * Storage rember user name to local storage
   *
   * @param username
   * @returns void
   */
  private saveRemeberUsername(username: string) {
    localStorage.setItem(this.rememberUserNameKey, username);
  }

  /**
   * Remove remeber user name in local storage
   *
   * @param username
   * @returns void
   */
  private removeRemeberUsername() {
    localStorage.removeItem(this.rememberUserNameKey);
  }
  /**
   * Get remeber user name in local storage
   *
   * @param username
   * @returns void
   */
  getRememberUsername(): string {
    return localStorage.getItem(this.rememberUserNameKey) ?? "";
  }

  /**
   * Load user information from access token
   *
   * @returns Promise<AuthUser>
   */
  loadUserProfile(): Promise<AuthUser> {
    return this.oauthService
      .loadUserProfile()
      .then((userInfo) => {
        var user = AuthUser.mapFromClaims(userInfo);
        this.currentUserSubject.next(user);
        return user;
      })
      .catch((error) => {
        return this.oauthService.revokeTokenAndLogout();
      });
  }

  sendEmailResetPassword(
    username: string,
    email: string,
    returnUrl: string
  ): Promise<any> {
    const url = `${environment.apiUrl}/api/v1/user-management/send-email-reset-password`;
    return this.httpClient
      .post(url, {
        username,
        email,
        returnUrl,
      })
      .toPromise();
  }

  resetPassword(password: string, token: string): Promise<any> {
    const url = `${environment.apiUrl}/api/v1/user-management/reset-password`;
    return this.httpClient
      .post(url, {
        password,
        token,
      })
      .toPromise();
  }
}
