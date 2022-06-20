import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { from } from "rxjs";
import { environment } from "../../environments/environment";
import { User } from "../AuthExtensions";
import { Category } from "../category-list/category-list.component";
import { Receipt } from "../my-receipt-list/my-receipt-list.component";


@Component({
  selector: 'receipt-change',
  templateUrl: './receipt-change.component.html'
})

export class ReceiptChangeComponent{
  receipt: Receipt = new Receipt(-1, "", new User(-1, "", new Array<Receipt>(), new Array<Receipt>(),), new Array<Category>(), "", "");
  invalidChange: boolean = false;
  allCategories: Category[] = new Array<Category>();
  addedCategories: Category[] = new Array<Category>();

  constructor(private router: Router,
    private http: HttpClient) {
    this.Init(router);
  }

  Init(router: Router) {

    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    var e = this.router.getCurrentNavigation();
    var state = e?.extras.state;
    if (state != null) {
      this.receipt = state.receipt;

      const url = environment.categoryAPI + "/Category/";

      this.http.get<Category[]>(url,
        {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'Authorization': token
          }),
        },
      ).subscribe(response => {
        this.allCategories = response;
        this.addedCategories = this.receipt.categories;
        for (var i = 0; i < this.addedCategories.length; i++) {
          var index = this.allCategories.indexOf(this.addedCategories[i]);
          this.allCategories.splice(index);
        }

      }, error => {
        this.router.navigate(['/myReceipts']);
      })
    }
    else {
      this.router.navigate(['/myReceipts']);
    }
  }

  ChangeReceipt() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    var obj = {
      ReceiptId: this.receipt.id,
      OwnerId: this.receipt.owner.id,
      Name: this.receipt.name,
      Categories: this.receipt.categories.map(x => x.id),
      Description: this.receipt.description,
      Ingredients: this.receipt.ingredients,
    }

    const data = JSON.stringify(obj);
    const url = environment.receiptAPI + "/Receipts/ChangeReceipt";

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
        this.invalidChange = true;
      }
      else {
        this.invalidChange = false;
        this.router.navigate(['/myReceipts']);
      }
    }, error => {
      console.log(error);
    })
  }

  onSelect(cat: Category) {
    this.addedCategories.push(cat);

    const index: number = this.allCategories.indexOf(cat);
    this.allCategories.splice(index, 1);
  }

  onRemove(cat: Category) {
    this.allCategories.push(cat);

    const index: number = this.addedCategories.indexOf(cat);
    this.addedCategories.splice(index, 1);
  }

}
