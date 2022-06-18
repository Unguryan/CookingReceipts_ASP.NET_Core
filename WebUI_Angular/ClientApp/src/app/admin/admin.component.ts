import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NavigationExtras, Router } from "@angular/router";

import { faRedo } from '@fortawesome/free-solid-svg-icons';
import { environment } from "../../environments/environment";
import { AuthExtensions, UserIDS } from "../AuthExtensions";

@Component({
  selector: 'admin',
  templateUrl: './admin.component.html'
})

export class AdminComponent {
  users: UserIDS[] = new Array<UserIDS>();
  refIcon: any;

  constructor(private http: HttpClient,
              private router: Router,
              private auth: AuthExtensions) {
    this.Init();
    this.refIcon = faRedo;
  }

  IsAdmin(user: UserIDS) {
    return user.roles.includes('Admin');
  }

  IsOwner(user: UserIDS) {
    return user.roles.includes('Owner');
  }

  OnSetAdminClick(user: UserIDS) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const data = JSON.stringify({ userName: user.userName, roles: "User, Admin" });

    const url = environment.idsAPI + "/User/ChangeRoles";

    this.http.post<UserIDS>(url, data,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      if (response.id == -1) {

      }
      else {
        this.Init();
      }

    }, error => {
      this.router.navigate(['']);
    })
  }

  OnRemoveAdminCClick(user: UserIDS) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const data = JSON.stringify({ userName: user.userName, roles: "User" });

    const url = environment.idsAPI + "/User/ChangeRoles";

    this.http.post<UserIDS>(url, data,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      if (response.id == -1) {

      }
      else {
        this.Init();
      }

    }, error => {
      this.router.navigate(['']);
    })
  }

  //OnChangeClick(rec: Receipt) {
  //  //this.categoryToChange

  //  const navigationExtras: NavigationExtras = {
  //    state: {
  //      receipt: rec
  //    }
  //  };

  //  this.router.navigate(['/receipts/change'], navigationExtras);
  //}

  //OnRemoveClick(rec: Receipt) {
  //  var token = localStorage.getItem('token');
  //  if (token == null || token == "") {
  //    this.router.navigate(['/login']);
  //    return;
  //  }

  //  const data = JSON.stringify({ Id: rec.id });
  //  const url = environment.receiptAPI + "/Receipts/RemoveReceipt";

  //  this.http.post<Receipt>(url, data,
  //    {
  //      headers: new HttpHeaders({
  //        'Content-Type': 'application/json',
  //        'Accept': '*/*',
  //        'Authorization': token
  //      }),
  //    },
  //  ).subscribe(response => {
  //    if (response.id == -1) {
  //      //Error
  //    }
  //    else {
  //      this.Init();
  //    }
  //  })
  //}

  async Init() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    //var user = await this.auth.GetUser();

    const url = environment.idsAPI + "/User";

    this.http.get<UserIDS[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      this.users = response;
    }, error => {
      this.router.navigate(['']);
    })
  }
}


