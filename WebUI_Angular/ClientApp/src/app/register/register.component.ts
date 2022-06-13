import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
})

export class RegisterComponent implements OnInit {

  public invalidUserName: boolean = false;

  constructor(
    private http: HttpClient,
    private router: Router) { }

  ngOnInit() { }

  register(form: NgForm) {
    var regData = {
      Name: form.value.Name,
      UserName: form.value.UserName,
      Password: form.value.Password
    };
    const data = JSON.stringify(regData);
    var url = environment.idsAPI + '/User/Register';

    this.http.post(url, data,
      {
        responseType: 'text',
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': '*/*',
        }),
      },
    ).subscribe(response => {
      const token = response;
      if (token == null || token === "") {
        this.invalidUserName = true;
      }
      else {
        localStorage.setItem('token', token);
        this.invalidUserName = false;
        this.router.navigate(['']);
      }
    })
  }
}
//<!--public string Name { get; set; }

//        public string UserName { get; set; }

//        public string Password { get; set; } -->
