import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';


import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
//import { MatOptionModule } from '@angular/material/';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

//import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AdminComponent } from './admin/admin.component';

import { AuthGuard } from './guards/auth-guard.guard';
import { OwnerAdminGuard } from './guards/owner-admin-guard.guard';
import { OwnerGuard } from './guards/owner-guard.guard';

import { ReceiptListComponent } from './receipt-list/receipt-list.component';
import { MyReceiptListComponent } from './my-receipt-list/my-receipt-list.component';
import { LikedReceiptListComponent } from './liked-receipt-list/liked-receipt-list.component';
import { ReceiptCreateComponent } from './receipt-create/receipt-create.component';
import { ReceiptChangeComponent } from './receipt-change/receipt-change.component';
import { ReceiptDetailsComponent } from './receipt-details/receipt-details.component';

import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryCreateComponent } from './category-create/category-create.component';
import { CategoryChangeComponent } from './category-change/category-change.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

//import { CounterComponent } from './counter/counter.component';
//import { FetchDataComponent } from './fetch-data/fetch-data.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    //HomeComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,

    //AuthGuard,
    //OwnerAdminGuard,

    ReceiptListComponent,
    MyReceiptListComponent,
    LikedReceiptListComponent,
    ReceiptCreateComponent,
    ReceiptChangeComponent,
    ReceiptDetailsComponent,

    CategoryListComponent,
    CategoryCreateComponent,
    CategoryChangeComponent

    //CounterComponent,
    //FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'admin', component: AdminComponent, canActivate: [OwnerGuard] },

      { path: '', component: ReceiptListComponent },
      { path: 'receipt', component: ReceiptDetailsComponent },
      { path: 'myReceipts', component: MyReceiptListComponent, canActivate: [AuthGuard] },
      { path: 'likedReceipts', component: LikedReceiptListComponent, canActivate: [AuthGuard] },
      { path: 'receipts/add', component: ReceiptCreateComponent, canActivate: [AuthGuard] },
      { path: 'receipts/change', component: ReceiptChangeComponent, canActivate: [AuthGuard] },

      { path: 'categories', component: CategoryListComponent, canActivate: [OwnerAdminGuard] },
      { path: 'categories/add', component: CategoryCreateComponent, canActivate: [OwnerAdminGuard] },
      { path: 'categories/change', component: CategoryChangeComponent, canActivate: [OwnerAdminGuard] },

      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },
    ]),
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatFormFieldModule,
    MatRadioModule,
    MatSelectModule,
    MatSnackBarModule,
    BrowserAnimationsModule,
    FontAwesomeModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
