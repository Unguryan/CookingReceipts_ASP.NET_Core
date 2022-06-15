import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  selector: 'categories',
  templateUrl: './category-list.component.html'
})

export class CategoryListComponent {

  nameCategory: string = "";
  categories: Category[] = new Array<Category>();

  constructor(private http: HttpClient, private router: Router) {

    this.Init();
  }

  OnSearchClick(nameCategory: string) {
    
  }

  OnChangeClick(category: Category) {

  }

  Init() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }
    
    this.http.get("https://localhost:5003/Category",
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
      response.toString();
      //this.categories = response;
    }, error => {
      this.router.navigate(['']);
    })
  }
}

export class Category {
  id: number;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}


