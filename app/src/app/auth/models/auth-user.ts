import { Role } from "./role";

export class AuthUser {
  id: number;
  username: string;
  email: string;
  phone: string;
  name: string;
  roles: string[];
  permissions: string[];

  static mapFromClaims(claims): AuthUser {
    const roleClaimName =
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    const permissionClaimName = "http://www.cbms.vn/identity/claims/permission";
    var roles = Array.isArray(claims[roleClaimName])
      ? claims[roleClaimName]
      : [claims[roleClaimName]];
    var permissions = Array.isArray(claims[permissionClaimName])
      ? claims[permissionClaimName]
      : [claims[permissionClaimName]];

    return Object.assign(new AuthUser(), {
      id: claims[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ],
      username: claims["http://www.cbms.vn/identity/claims/username"],
      name: claims[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
      ],
      email:
        claims[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        ],
      roles: roles,
      permissions: permissions,
      phone:
        claims[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"
        ],
    });
  }

  get isAdmin(): boolean {
    return (
      this.roles &&
      this.roles.findIndex(
        (p) => p.toUpperCase() == Role.Admin.toUpperCase()
      ) >= 0
    );
  }

  isGrantedAny(p: string[]): boolean {
    if (!p) return true;

    return p.some(
      (r) =>
        this.permissions.findIndex((p) => p.toUpperCase() == r.toUpperCase()) >=
        0
    );
  }
}
