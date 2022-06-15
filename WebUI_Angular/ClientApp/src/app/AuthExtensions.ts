import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthExtensions {

  constructor(
    private http: HttpClient) { }

  async GetUser(): Promise<User> {

    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      return new User("", "", "");
    }

    ///
    var tokenData = {
      Token: token,
      UserName: ""
    };

    const data = JSON.stringify(tokenData);
    var url = environment.idsAPI + '/User/GetUserByToken';

    var res = this.http.post<User>(url, data,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
        }),
      },
    ).toPromise();

    var r = (await res);
    return r;
  }

}


export class User {

  constructor(Name: string, UserName: string, Roles: string) {
    this.name = Name;
    this.userName = UserName;
    this.roles = Roles;
  }

  name: string;
  userName: string;
  roles: string;
}
