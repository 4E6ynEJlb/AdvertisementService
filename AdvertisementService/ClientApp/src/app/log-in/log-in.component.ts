import { Component } from '@angular/core';
import { Credentials, HttpService } from '../http.service';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

@Component({
  selector: 'app-log-in',
  imports: [
    FormsModule
  ],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent {
  login:string = "";
  password:string = "";
  
  constructor(private httpService:HttpService){}

  buttonClick() {
    var credentials = new Credentials();
    credentials.login = this.login;
    credentials.password = this.password;
    this.httpService.login(credentials);
  }
}
