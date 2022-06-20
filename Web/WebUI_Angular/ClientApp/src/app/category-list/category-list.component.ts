import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component } from "@angular/core";
import { NavigationExtras, Router } from "@angular/router";
import { environment } from "../../environments/environment"

import { faRedo } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'category-list',
  templateUrl: './category-list.component.html'
})

export class CategoryListComponent {


  nameCategory: string = "";
  categories: Category[] = new Array<Category>();
    refIcon: any;

  constructor(private http: HttpClient, private router: Router) {
    this.Init();
    this.refIcon = faRedo;
  }

  OnSearchClick(nameCategory: string) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const url = environment.categoryAPI + "/Category/" + nameCategory.toString();

    this.http.get<Category[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      this.categories = response;
    }, error => {
      this.router.navigate(['']);
    })
  }

  OnChangeClick(cat: Category) {
    //this.categoryToChange

    const navigationExtras: NavigationExtras = {
      state: {
        category: cat
      }
    };

    this.router.navigate(['/categories/change'], navigationExtras);
  }

  OnRemoveClick(cat: Category) {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['/login']);
      return;
    }

    const data = JSON.stringify({ Id: cat.id });
    const url = environment.categoryAPI + "/Category/RemoveCategory";

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
        //Error
      }
      else {
        this.Init();
      }
    })
  }

  Init() {
    var token = localStorage.getItem('token');
    if (token == null || token == "") {
      this.router.navigate(['']);
      return;
    }

    const url = environment.categoryAPI + "/Category";
    
    this.http.get<Category[]>(url,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
          'Authorization': token
        }),
      },
    ).subscribe(response => {
      this.categories = response;
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


