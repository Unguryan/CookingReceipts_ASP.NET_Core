import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { from } from "rxjs";
import { environment } from "../../environments/environment";
import { Category } from "../category-list/category-list.component";


@Component({
  selector: 'category-create',
  templateUrl: './category-create.component.html'
})

export class CategoryCreateComponent {
  invalidAdd: boolean = false;

  constructor(private http: HttpClient,
              private router: Router) {

  }

  addCategory(form: NgForm) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    const data = JSON.stringify(form.value);
    const url = environment.categoryAPI + "/Category/AddCategory";

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
        this.invalidAdd = true;
      }
      else {
        this.invalidAdd = false;
        this.router.navigate(['/categories']);
      }
    })

  }
}
