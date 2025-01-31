import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpService, UserOutputModel } from '../http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-settings',
  imports: [FormsModule],
  templateUrl: './user-settings.component.html',
  styleUrl: './user-settings.component.css'
})
export class UserSettingsComponent implements OnInit {
  name: string = "";
  login: string = "";
  password: string = "";

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit(): void {
    if (!this.httpService.isLogined)
      this.router.navigate(['log-in']);
    else {
      this.login = <string>this.httpService.credentials?.login;
      this.password = <string>this.httpService.credentials?.password;
      this.httpService.getUser().subscribe({
        next: (data: UserOutputModel) => {
          this.name = data.name;
        },
        error: error => {
          console.log(error);
        }
      })
    }
  }

  saveNameButtonClick() {
    this.httpService.editName(this.name).subscribe({
      next: () => {
        this.router.navigate(['profile']);
      },
      error: error => {
        console.log(error);
      }
    })
  }

  saveCredentialsButtonClick() {
    this.httpService.editCredentials(this.login, this.password);
  }
}
