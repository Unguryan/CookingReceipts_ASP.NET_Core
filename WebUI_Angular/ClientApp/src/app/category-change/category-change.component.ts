import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { Category } from "../category-list/category-list.component";


@Component({
  selector: 'category-change',
  templateUrl: './category-change.component.html'
})

export class CategoryChangeComponent {
  category: Category = new Category(-1, "");
  invalidChange: boolean = false;

  constructor(private router: Router,
              private http: HttpClient) {
    this.Init(router);
  }

  Init(router: Router) {
    var e = this.router.getCurrentNavigation();
    var state = e?.extras.state;
    if (state != null) {
      this.category = state.category;
    }
    else {
      this.router.navigate(['/categories']);
    }
  }

  ChangeCategory() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    const data = JSON.stringify(this.category);
    const url = environment.categoryAPI + "/Category/ChangeCategory";

    this.http.post<Category>(url, data,
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
        this.router.navigate(['/categories']);
      }
    })
  }

}
