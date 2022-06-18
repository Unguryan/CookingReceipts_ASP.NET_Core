import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Receipt } from './my-receipt-list/my-receipt-list.component';


@Injectable({
  providedIn: 'root'
})
export class AuthExtensions {

  constructor(
    private http: HttpClient) { }

  async GetUser(): Promise<UserIDS> {

    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      return new UserIDS(-1, "", "", "");
    }

    ///
    var tokenData = {
      Token: token,
      UserName: ""
    };

    const data = JSON.stringify(tokenData);
    var url = environment.idsAPI + '/User/GetUserByToken';

    var res = this.http.post<UserIDS>(url, data,
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


export class UserIDS {

  id: number;
  name: string;
  userName: string;
  roles: string;

  constructor(Id: number, Name: string, UserName: string, Roles: string) {
    this.id = Id;
    this.name = Name;
    this.userName = UserName;
    this.roles = Roles;
  }

  
}


export class User {

  id: number;
  name: string;
  receipts: Receipt[];
  likedReceipts: Receipt[];


  constructor(Id: number, Name: string, Receipts: Receipt[], LikedReceipts: Receipt[]) {
    this.id = Id;
    this.name = Name;
    this.receipts = Receipts;
    this.likedReceipts = LikedReceipts;
  }

  
}
