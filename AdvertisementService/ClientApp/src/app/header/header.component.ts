import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { HttpService } from '../http.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  imports: [HttpClientModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit { 
  constructor(private router: Router, public httpService: HttpService) {}

  ngOnInit(): void {
    this.httpService.logInEvent.subscribe(() => { 
      this.router.navigate(['profile']);
    });
    this.httpService.logOutEvent.subscribe(()=>{
      this.router.navigate(['ads']);
    });
  }

  profileClick() {
    this.router.navigate(['profile']);
  }

  logInClick() {
    this.router.navigate(['log-in']);
  }

  adsClick() {
    this.router.navigate(['ads']);
  }

  registerClick() {
    this.router.navigate(['register']);
  }

  logOutClick() {
    this.httpService.logout();
  }
}
