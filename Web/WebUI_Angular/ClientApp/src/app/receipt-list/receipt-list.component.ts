import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthExtensions } from "../AuthExtensions";
import { Receipt } from "../my-receipt-list/my-receipt-list.component";

import { faRedo } from '@fortawesome/free-solid-svg-icons';
import { environment } from "../../environments/environment";

@Component({
  selector: 'receipt-list',
  templateUrl: './receipt-list.component.html'
})

export class ReceiptListComponent {
  nameReceipt: string = "";
  receipts: Receipt[] = new Array<Receipt>();
  refIcon: any;

  constructor(private http: HttpClient,
    private router: Router,
    private auth: AuthExtensions) {
    this.Init();
    this.refIcon = faRedo;
  }

  OnReceiptClick(rec: Receipt) {
    this.router.navigate(['receipt'], { queryParams: { id: rec.id } });
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

  Init() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const url = environment.receiptAPI + "/Receipts";

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
