import { Component, OnInit, Injector, ViewChild, ViewContainerRef } from '@angular/core';
import { AdvertisementOutputModel, HttpService, UserOutputModel } from '../http.service';
import { Router } from '@angular/router';
import { UserAdvertisementComponent } from '../user-advertisement/user-advertisement.component';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  greetings: string = "";
  id: string = ""

  @ViewChild('container', { read: ViewContainerRef }) container!: ViewContainerRef;

  constructor(private httpService: HttpService, private router: Router)  {}

  ngOnInit(): void {
    if (!this.httpService.isLogined)
    this.router.navigate(['log-in']);
    else {
      this.httpService.getUser().subscribe({
        next: (data: UserOutputModel) => {
          this.greetings = "С возвращением, " + data.name + "!";
          this.id = "Id: " + data.id;
          this.httpService.isAdmin = data.isAdmin;
        },
        error: error => {
          console.log(error);
          this.ngOnInit();
        }
      });
      this.httpService.getUserAdvertisements().subscribe({
        next: (data: AdvertisementOutputModel[]) => {
          this.viewAdvertisements(data);
        },
        error: error => {
          console.log(error);
          this.ngOnInit();
        }
      })
    }
  }

  private viewAdvertisements(ads:AdvertisementOutputModel[]) {
    this.container.clear();
    ads.forEach(advertisement=>{
      var injector = Injector.create({
        providers: [
          {provide: "USER_ADVERTISEMENT", useValue:advertisement},
        ],
      });
      this.container.createComponent(UserAdvertisementComponent, {injector:injector});
    });
  }

  newAdvertisementClick() {
    this.router.navigate(['new-advertisement'])
  }

  settingsClick() {
    this.router.navigate(['settings']);
  }
}