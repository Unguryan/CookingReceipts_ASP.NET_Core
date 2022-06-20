import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { async, from } from "rxjs";
import { environment } from "../../environments/environment";
import { AuthExtensions, User } from "../AuthExtensions";
import { Category } from "../category-list/category-list.component";
import { Receipt } from "../my-receipt-list/my-receipt-list.component";


@Component({
  selector: 'receipt-details',
  templateUrl: './receipt-details.component.html'
})

export class ReceiptDetailsComponent {
  receipt: Receipt = new Receipt(-1, "", new User(-1, "", new Array<Receipt>(), new Array<Receipt>(),), new Array<Category>(), "", "");
  canUserLike: boolean = false;
  userReceipt: boolean = false;

  constructor(private router: Router,
    private http: HttpClient,
    private auth: AuthExtensions) {
    this.Init(router);
  }

  Init(router: Router) {

    var token = localStorage.getItem('token')!;
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    var e = this.router.getCurrentNavigation();
    const id = e?.extras.queryParams?.id;
    if (id != null) {

      const url = environment.receiptAPI + "/Receipts/" + id;

      this.http.get<Receipt>(url,
        {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'Authorization': token
          }),
        },
      ).subscribe(response => {
        if (response.id == -1) {
          this.router.navigate(['']);
        }
        else {
          this.receipt = response;
          this.CanUserLike();
        }
      }, error => {
        this.router.navigate(['']);
      })

      //const url = environment.categoryAPI + "/Category/";
      //this.http.get<Category[]>(url,
      //  {
      //    headers: new HttpHeaders({
      //      'Content-Type': 'application/json',
      //      'Accept': '*/*',
      //      'Authorization': token
      //    }),
      //  },
      //).subscribe(response => {
      //  this.allCategories = response;
      //  this.addedCategories = this.receipt.categories;
      //  for (var i = 0; i < this.addedCategories.length; i++) {
      //    var index = this.allCategories.indexOf(this.addedCategories[i]);
      //    this.allCategories.splice(index);
      //  }

      //}, error => {
      //  this.router.navigate(['/myReceipts']);
      //})
    }
    else {
      this.router.navigate(['']);
    }
  }

  async OnAddClick(rec: Receipt) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    var user = await this.auth.GetUser();

    const data = JSON.stringify({ ReceiptId: rec.id, UserId: user.id });
    const url = environment.receiptAPI + "/Receipts/AddLikedReceipt";

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
        this.canUserLike = false;
      }
    })
  }

  async OnRemoveClick(rec: Receipt) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    var user = await this.auth.GetUser();

    const data = JSON.stringify({ ReceiptId: rec.id, UserId: user.id });
    const url = environment.receiptAPI + "/Receipts/RemoveLikedReceipt";

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
        this.canUserLike = true;
      }
    })
  }

  async CanUserLike() {
    var token = localStorage.getItem('token')!;
    if (token == null || token == "") {
      this.canUserLike = false;
      this.router.navigate(['']);
    }

    var user = await this.auth.GetUser();

    var url = environment.receiptAPI + "/Receipts/ByUser/" + user.id;

    this.http.get<Receipt[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      var result = response.some(res => res.id == this.receipt.id);

      this.canUserLike = !result;
      if (result) {
        this.userReceipt = true;
      }
      else {

        var url = environment.receiptAPI + "/Receipts/LikedReceipts/" + user.id;

        this.http.get<Receipt[]>(url,
          {
            headers: new HttpHeaders({
              'Content-Type': 'application/json',
              'Accept': '*/*',
              'Authorization': token
            }),
          },
        ).subscribe(response => {
          var r = response.some(res => res.id == this.receipt.id);
          this.canUserLike = !r;
        }, error => {
          this.router.navigate(['']);
          this.canUserLike = false;
        });
      }


    }, error => {
      this.router.navigate(['']);
    })

  }
}
