import { Component } from '@angular/core';
import { HttpService, RegisterUserModel } from '../http.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  login:string = "";
  password:string = "";
  name:string = "";

  constructor(private httpService:HttpService){}

  buttonClick() {
    var registerUserModel = new RegisterUserModel()
    registerUserModel.login = this.login;
    registerUserModel.password = this.password;
    registerUserModel.name = this.name;
    this.httpService.register(registerUserModel);
  }
}
