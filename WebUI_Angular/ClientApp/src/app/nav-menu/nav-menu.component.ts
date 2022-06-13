import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { AuthExtensions, User } from '../AuthExtensions';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  auth: AuthExtensions;
  isExpanded = false;
  public user: User | undefined;

  constructor(
    private http: HttpClient,
    private router: Router) {
    this.auth = new AuthExtensions(http);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  IsAuth(): Boolean {
    var token = localStorage.getItem('token');
    var res = token != null && token !== "";
    if (res && this.user == undefined) {
      this.SetUser();
    }
    
    return res;
    //return this.auth.IsAuth();
  }

  IsAdmin(): Boolean {
    if (this.IsAuth() && this.user != undefined) {
      if (this.user?.roles.includes('Admin') || this.user?.roles.includes('Owner')) {
        return true;
      }
    }
    return false;
  }

  SetUser() {
    if (this.user != undefined) {
      return;
    }
    //if (!this.IsAuth()) {
    //  return;
    //}

    this.auth.GetUser().then(x =>
    {
      this.user = x
    });
  }

  LogOut() {
    var token = localStorage.getItem('token');
    if (token == null || token === "") {
      return;
    }

    const data = JSON.stringify({ "Token": token, "Username": "" });
    var url = environment.idsAPI + '/User/Logout';

    this.http.post(url, data,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
          //'Access-Control-Allow-Origin': 'https://localhost:5001',
          //'Access-Control-Allow-Credentials': 'true'
        }),
      },
    ).subscribe(response => {
      const isSuccess = (<Boolean>response);
      if (isSuccess) {
        this.router.navigate(['']);
        localStorage.removeItem('token');
        this.user = undefined;
      }
    })

    


  }
}
