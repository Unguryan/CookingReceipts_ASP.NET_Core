import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private http: HttpClient, private router: Router) {
  }

  async canActivate() {
    const token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['login']);
      return false;
    }

    var tokenData = {
      Token: token,
      UserName: ""
    };

    const data = JSON.stringify(tokenData);
    var url = environment.idsAPI + '/User/VerifyToken';

    var res = this.http.post(url, data,
      {
        responseType: 'text',
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
        }),
      },
    ).toPromise();

    return await res === "true";
  }
}
