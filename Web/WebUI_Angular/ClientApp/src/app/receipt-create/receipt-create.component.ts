import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { AuthExtensions } from "../AuthExtensions";
import { Category } from "../category-list/category-list.component";
import { Receipt } from "../my-receipt-list/my-receipt-list.component";


@Component({
  selector: 'receipt-create',
  templateUrl: './receipt-create.component.html'
})

export class ReceiptCreateComponent {
  invalidAdd: boolean = false;
  allCategories: Category[] = new Array<Category>();
  addedCategories: Category[] = new Array<Category>();

  constructor(private http: HttpClient,
              private router: Router,
              private auth: AuthExtensions) {
    this.Init();
  }
    Init() {
      var token = localStorage.getItem('token');
      if (token == null || token == "") {
        this.router.navigate(['']);
        return;
      }

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
      }, error => {
        this.router.navigate(['myReceipts']);
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

  addReceipt(form: NgForm) {
    const token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    this.auth.GetUser().then(u => {
      form.value.OwnerId = u.id;
      form.value.Categories = this.addedCategories.map(x => x.id);

      const data = JSON.stringify(form.value);
      const url = environment.receiptAPI + "/Receipts/AddReceipt";

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
          this.invalidAdd = true;
        }
        else {
          this.invalidAdd = false;
          this.router.navigate(['/myReceipts']);
        }
      })

    });

   

  }
}
