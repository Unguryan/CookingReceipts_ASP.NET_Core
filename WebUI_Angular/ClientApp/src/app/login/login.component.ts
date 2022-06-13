import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
})

export class LoginComponent implements OnInit {

  public invalidLogin: boolean = false;

  constructor(
    private http: HttpClient,
    private router: Router) { }

  ngOnInit() { }

  login(form: NgForm) {

    ///TODO: Add default value for bool

    if (form.value.RememberMe === "") {
      form.value.RememberMe = false;
    }

    const data = JSON.stringify(form.value);
    var url = environment.idsAPI + '/User/Login';

    this.http.post(url, data,
      {
        responseType: 'text',
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*'
        }),
      },
      ).subscribe(response => {
        const token = response;
        if (token === "") {
          this.invalidLogin = true;
        }
        else {
          localStorage.setItem('token', token);
          this.invalidLogin = false;
          this.router.navigate(['']);
        }
      })
  }

  onSignUpClick() {
    this.router.navigate(['register']);
  }
}
