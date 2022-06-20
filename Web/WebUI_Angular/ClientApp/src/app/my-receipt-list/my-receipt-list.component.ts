import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NavigationExtras, Router } from "@angular/router";

import { faRedo } from '@fortawesome/free-solid-svg-icons';
import { environment } from "../../environments/environment";
import { AuthExtensions, User } from "../AuthExtensions";
import { Category } from "../category-list/category-list.component";

@Component({
  selector: 'my-receipt-list',
  templateUrl: './my-receipt-list.component.html'
})

export class MyReceiptListComponent {
  nameReceipt: string = "";
  receipts: Receipt[] = new Array<Receipt>();
  refIcon: any;

  constructor(private http: HttpClient,
              private router: Router,
              private auth: AuthExtensions) {
    this.Init();
    this.refIcon = faRedo;
  }

  OnSearchClick(nameCategory: string) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const url = environment.receiptAPI + "/Receipts/ByName/" + this.nameReceipt.toString();

    this.http.get<Receipt[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      this.receipts = response;
    }, error => {
      this.router.navigate(['']);
    })
  }

  OnChangeClick(rec: Receipt) {
    //this.categoryToChange

    const navigationExtras: NavigationExtras = {
      state: {
        receipt: rec
      }
    };

    this.router.navigate(['/receipts/change'], navigationExtras);
  }

  OnRemoveClick(rec: Receipt) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    const data = JSON.stringify({ Id: rec.id });
    const url = environment.receiptAPI + "/Receipts/RemoveReceipt";

    this.http.post<Receipt>(url, data,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      if (response.id == -1) {
        //Error
      }
      else {
        this.Init();
      }
    })
  }

  async Init() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    var user = await this.auth.GetUser();

    const url = environment.receiptAPI + "/Receipts/ByUser/" + user.id;

    this.http.get<Receipt[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      this.receipts = response;
    }, error => {
      this.router.navigate(['']);
    })
  }
}

export class Receipt {
  id: number;
  name: string;
  owner: User;
  categories: Category[];
  description: string;
  ingredients: string;

  constructor(id: number, name: string, Owner: User, Categories: Category[], description: string, ingredients: string) {
    this.id = id;
    this.name = name;
    this.owner = Owner;
    this.categories = Categories;
    this.description = description;
    this.ingredients = ingredients;
  }
}


